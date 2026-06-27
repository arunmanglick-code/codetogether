# IPS Proxy — Design Documentation

This document contains High-Level Design (HLD) and Low-Level Design (LLD) diagrams for the `ips-proxy` service, generated from the `develop` branch codebase.

---

## Table of Contents

1. [High-Level Design](#high-level-design)
   - [System Context](#1-system-context)
   - [Deployment Architecture](#2-deployment-architecture)
2. [Low-Level Design](#low-level-design)
   - [Component & Class Diagram](#3-component--class-diagram)
   - [Filter Chain Execution Order](#4-filter-chain-execution-order)
   - [Request Processing — Happy Path](#5-request-processing--happy-path)
   - [JWT Authentication Flow](#6-jwt-authentication-flow)
   - [Configuration Properties](#7-configuration-properties)

---

## High-Level Design

### 1. System Context

The IPS Proxy sits between external callers (e.g. Oracle Integration Cloud) and the downstream n8n workflow automation platform. It enforces authentication, entitlement checking, API-key injection, and request routing.

```mermaid
graph TB
    subgraph Clients["External Callers"]
        OIC["Oracle Integration Cloud\n(OIC)"]
        Other["Other HTTP Clients"]
    end

    subgraph AuthServer["Vertex Auth Server\n(per environment)"]
        JWKS["JWKS endpoint\n/.well-known/jwks.json"]
    end

    subgraph Proxy["IPS Proxy  ·  Spring Boot + Spring Cloud Gateway"]
        GW["Spring Cloud Gateway\n(Reactive Netty)"]
    end

    subgraph Downstream["Downstream Services"]
        N8N["n8n Webhook\n/webhook/oracleaccl/n8n/**"]
    end

    OIC -- "POST /v1/oracleaccl/n8n/**\nBearer JWT" --> GW
    Other -- "HTTP request\nBearer JWT" --> GW
    GW -- "Fetch RSA public keys\n(once / cached)" --> JWKS
    GW -- "Forwarded request\n+ ORCL_ACCL_N8N_WF_KEY header" --> N8N
```

---

### 2. Deployment Architecture

The proxy is containerised and deployed to Azure Kubernetes Service via a shared Helm chart. APISIX acts as the public-facing API Gateway / Ingress.

```mermaid
graph TB
    Internet["Internet / VPN"]

    subgraph AzureCloud["Azure Cloud  (dev / stage / prod)"]
        subgraph AKS["Azure Kubernetes Service (AKS)"]
            APISIX["APISIX API Gateway\n(Ingress Controller)"]

            subgraph ProxyPod["ips-proxy Pod"]
                Container["Docker Container\neclipse-temurin:21-jre\nport 8080"]
                EnvVars["Env Vars:\nSPRING_PROFILES_ACTIVE\nPROXY_API_KEY\nPROXY_JWKS_URI\nN8N_BASE_URL"]
            end

            N8NPod["n8n Pod\n(unrestricted webhook)"]
        end

        KeyVault["1Password Secrets\n(API key, etc.)"]
    end

    Internet --> APISIX
    APISIX --> Container
    Container --> N8NPod
    KeyVault -.->|"injected at startup"| EnvVars
```

**Environments and hostnames:**

| Profile | Hostname | Ingress |
|---------|----------|---------|
| `dev` | `ips-proxy-dev.dev.az.vtxdev.net` | Private (VPN) |
| `stage` | `ips-proxy.stage.az.vtxdev.net` | Public |
| `stage` (central) | `ips-proxy-centralus.stage.az.vtxdev.net` | Public |
| `prod` | `n8n.vertexcloud.com` (n8n) | — |

---

## Low-Level Design

### 3. Component & Class Diagram

```mermaid
classDiagram
    direction TB

    class IpsProxyApplication {
        +main(args: String[]) void
    }

    class ProxyProperties {
        -jwksUri: String
        -apiKey: String
        -apiKeyHeader: String
        -n8nBaseUrl: String
        -reconTimeoutSeconds: long
        +getJwksUri() String
        +getApiKey() String
        +getApiKeyHeader() String
        +getN8nBaseUrl() String
        +getReconTimeoutSeconds() long
    }

    class GatewayHttpClientConfig {
        +redirectFollowingWebClient(ProxyProperties) WebClient
        +jwtDecoder(ProxyProperties) NimbusReactiveJwtDecoder
    }

    class JwtAuthenticationGatewayFilterFactory {
        +CLAIMS_ATTR: String = "jwt.claims"
        -jwtDecoder: ReactiveJwtDecoder
        +apply(Config) GatewayFilter
        -unauthorized(exchange) Mono~Void~
    }
    class JwtAuthConfig["JwtAuthenticationGatewayFilterFactory.Config"] {
        (no fields — marker class)
    }

    class EntitlementCheckGatewayFilterFactory {
        +apply(Config) GatewayFilter
        -extractEntitlements(Jwt, String) Collection
        -forbidden(exchange) Mono~Void~
    }
    class EntitlementConfig["EntitlementCheckGatewayFilterFactory.Config"] {
        -requiredEntitlement: String
        -claimsKey: String
    }

    class ApiKeyGatewayFilterFactory {
        -props: ProxyProperties
        +apply(Config) GatewayFilter
    }
    class ApiKeyConfig["ApiKeyGatewayFilterFactory.Config"] {
        (no fields — marker class)
    }

    class RedirectFollowingGlobalFilter {
        -webClient: WebClient
        +getOrder() int
        +filter(exchange, chain) Mono~Void~
        -getRouteTimeout(exchange) Duration
    }

    class RequestLoggingGlobalFilter {
        +getOrder() int
        +filter(exchange, chain) Mono~Void~
        -populateMdc(exchange) void
        -logInboundRequest(exchange) void
        -logOutboundDetails(exchange) void
        -sanitizeHeaders(HttpHeaders) HttpHeaders
    }

    class ResponseTimeLoggingGlobalFilter {
        +getOrder() int
        +filter(exchange, chain) Mono~Void~
        -logTiming(exchange) void
    }

    IpsProxyApplication ..> GatewayHttpClientConfig : bootstraps
    GatewayHttpClientConfig --> ProxyProperties : reads
    GatewayHttpClientConfig ..> JwtAuthenticationGatewayFilterFactory : provides NimbusJwtDecoder
    GatewayHttpClientConfig ..> RedirectFollowingGlobalFilter : provides WebClient

    JwtAuthenticationGatewayFilterFactory +-- JwtAuthConfig
    EntitlementCheckGatewayFilterFactory +-- EntitlementConfig
    ApiKeyGatewayFilterFactory +-- ApiKeyConfig

    ApiKeyGatewayFilterFactory --> ProxyProperties : reads apiKey/apiKeyHeader
    EntitlementCheckGatewayFilterFactory ..> JwtAuthenticationGatewayFilterFactory : reads CLAIMS_ATTR
    RequestLoggingGlobalFilter ..> JwtAuthenticationGatewayFilterFactory : reads CLAIMS_ATTR
```

---

### 4. Filter Chain Execution Order

Spring Cloud Gateway processes filters in `order` value sequence. Lower numbers run earlier. Global filters run on every request; per-route filters apply only to matched routes.

```mermaid
flowchart TB
    subgraph legend["Legend"]
        direction LR
        G["  Global filter  "]:::global
        R["  Per-route filter  "]:::route
    end

    subgraph chain["Filter Execution Order (lowest order first → last)"]
        direction TB
        F1["① ResponseTimeLoggingGlobalFilter\norder = HIGHEST_PRECEDENCE + 10\nRecords start timestamp"]:::global
        F2["② [Spring Cloud Gateway built-ins]\nRoutePredicateHandlerMapping,\nNettyWriteResponseFilter, etc."]:::builtin
        F3["③ RequestLoggingGlobalFilter\norder = LOWEST_PRECEDENCE − 100\nPopulates MDC, logs inbound request"]:::global
        F4["④ JwtAuthentication (per-route)\nValidates Bearer token via JWKS\nStores Jwt in exchange attributes"]:::route
        F5["⑤ EntitlementCheck (per-route)\nReads Jwt from attributes\nVerifies required entitlement/audience"]:::route
        F6["⑥ RewritePath (per-route)\n/v1/oracleaccl/n8n/** → /webhook/oracleaccl/n8n/**"]:::route
        F7["⑦ ApiKey (per-route)\nInjects ORCL_ACCL_N8N_WF_KEY header"]:::route
        F8["⑧ Retry (per-route)\n3 retries on BAD_GATEWAY/503/504 for GET"]:::route
        F9["⑨ AddRequestHeader / RemoveRequestHeader\nAdds X-Forwarded-By, removes Cookie"]:::route
        F10["⑩ RedirectFollowingGlobalFilter\norder = NettyRoutingFilter.ORDER − 1\nRoutes request, follows 3xx redirects"]:::global
        F11["⑪ [NettyRoutingFilter — bypassed]\nMark already-routed; response written\nby RedirectFollowingGlobalFilter"]:::builtin
    end

    F1 --> F2 --> F3 --> F4 --> F5 --> F6 --> F7 --> F8 --> F9 --> F10 --> F11

    classDef global fill:#dbeafe,stroke:#3b82f6,color:#1e3a5f
    classDef route  fill:#dcfce7,stroke:#22c55e,color:#14532d
    classDef builtin fill:#f3f4f6,stroke:#9ca3af,color:#374151
```

---

### 5. Request Processing — Happy Path

End-to-end sequence for a successful `POST /v1/oracleaccl/n8n/<path>` request.

```mermaid
sequenceDiagram
    autonumber
    participant Client as Client<br/>(OIC / HTTP)
    participant Proxy as IPS Proxy<br/>(Spring Cloud Gateway)
    participant Auth as Vertex Auth Server<br/>(JWKS)
    participant N8N as n8n Webhook

    Client->>Proxy: POST /v1/oracleaccl/n8n/<path><br/>Authorization: Bearer <jwt>

    Note over Proxy: ResponseTimeLoggingGlobalFilter<br/>records start timestamp

    Note over Proxy: RequestLoggingGlobalFilter<br/>logs inbound request + sanitized headers

    Note over Proxy: JwtAuthenticationGatewayFilterFactory
    Proxy->>Auth: GET /.well-known/jwks.json<br/>(cached after first fetch)
    Auth-->>Proxy: RSA public keys (JWK Set)
    Proxy->>Proxy: Validate JWT signature & expiry<br/>Store Jwt in exchange attributes

    Note over Proxy: EntitlementCheckGatewayFilterFactory
    Proxy->>Proxy: Read 'aud' claim from Jwt<br/>Assert contains 'verx://migration-api'

    Note over Proxy: RewritePath filter
    Proxy->>Proxy: /v1/oracleaccl/n8n/<path><br/>→ /webhook/oracleaccl/n8n/<path>

    Note over Proxy: ApiKeyGatewayFilterFactory
    Proxy->>Proxy: Add ORCL_ACCL_N8N_WF_KEY header

    Note over Proxy: Add X-Forwarded-By, Remove Cookie

    Note over Proxy: RedirectFollowingGlobalFilter<br/>(WebClient with redirect-following)
    Proxy->>N8N: POST /webhook/oracleaccl/n8n/<path><br/>ORCL_ACCL_N8N_WF_KEY: [redacted]<br/>X-Forwarded-By: ips-proxy

    alt n8n returns 3xx redirect
        N8N-->>Proxy: 302 Location: <new-url>
        Proxy->>N8N: POST <new-url> (auto-followed)
    end

    N8N-->>Proxy: 200 OK + response body
    Proxy-->>Client: 200 OK + response body

    Note over Proxy: ResponseTimeLoggingGlobalFilter<br/>logs timeSpentMs + status<br/>RequestLoggingGlobalFilter<br/>logs outbound details + clears MDC
```

---

### 6. JWT Authentication Flow

Detailed flow inside `JwtAuthenticationGatewayFilterFactory` showing error paths.

```mermaid
flowchart TD
    A([Incoming Request]) --> B{Authorization header\npresent and starts\nwith 'Bearer '?}

    B -- No --> C[Set HTTP 401 Unauthorized]
    C --> Z([Return response to client])

    B -- Yes --> D[Extract Bearer token]
    D --> E[jwtDecoder.decode token\nvia JWKS]

    E --> F{Decode result}

    F -- TimeoutException --> G[Set HTTP 504 Gateway Timeout\nlog error]
    G --> Z

    F -- Other error\ne.g. invalid/expired --> H[Set HTTP 401 Unauthorized\nlog warning]
    H --> Z

    F -- Success --> I[Store Jwt in\nexchange attributes\nkey: 'jwt.claims']
    I --> J[Continue filter chain\nEntitlementCheck next]

    subgraph EntitlementCheck["EntitlementCheckGatewayFilterFactory"]
        J --> K{Jwt present\nin attributes?}
        K -- No --> L[Set HTTP 403 Forbidden\nlog warning]
        L --> Z
        K -- Yes --> M["Extract claim from Jwt\n(default: 'aud')"]
        M --> N{Required entitlement\npresent in claim?}
        N -- No --> O[Set HTTP 403 Forbidden\nlog warning with subject]
        O --> Z
        N -- Yes --> P[Continue filter chain]
    end

    P --> Q([Route to downstream n8n])
```

---

### 7. Configuration Properties

How `ProxyProperties` is bound from environment variables and `application.yml` across deployment profiles.

```mermaid
flowchart LR
    subgraph EnvVars["Environment Variables"]
        EV1["PROXY_JWKS_URI"]
        EV2["PROXY_API_KEY"]
        EV3["N8N_BASE_URL"]
        EV4["PORT"]
    end

    subgraph AppYml["application.yml\n(proxy: prefix)"]
        P1["jwks-uri\n(default: stage-auth.vertexcloud.com)"]
        P2["api-key\n(default: empty)"]
        P3["api-key-header\n(= ORCL_ACCL_N8N_WF_KEY)"]
        P4["n8n-base-url\n(default: https://n8n.io)"]
        P5["recon-timeout-seconds\n(default: 300)"]
    end

    subgraph ProxyProps["ProxyProperties @Component\n@ConfigurationProperties(prefix='proxy')"]
        PP1["jwksUri"]
        PP2["apiKey"]
        PP3["apiKeyHeader"]
        PP4["n8nBaseUrl"]
        PP5["reconTimeoutSeconds"]
    end

    subgraph Consumers["Consumers"]
        C1["GatewayHttpClientConfig\n→ NimbusReactiveJwtDecoder\n→ WebClient (timeout)"]
        C2["JwtAuthenticationGatewayFilterFactory\n→ jwksUri"]
        C3["ApiKeyGatewayFilterFactory\n→ apiKey, apiKeyHeader"]
        C4["Spring Cloud Gateway routes\n→ n8nBaseUrl (uri:)\n→ reconTimeoutSeconds (metadata)"]
    end

    EV1 -->|overrides| P1
    EV2 -->|overrides| P2
    EV3 -->|overrides| P4

    P1 --> PP1
    P2 --> PP2
    P3 --> PP3
    P4 --> PP4
    P5 --> PP5

    PP1 --> C1
    PP1 --> C2
    PP2 --> C3
    PP3 --> C3
    PP4 --> C4
    PP5 --> C1
    PP5 --> C4
```

**Profile-specific defaults:**

| Property | `dev` | `qa` | `stage` | `prod` |
|----------|-------|------|---------|--------|
| `jwks-uri` | `dev-auth.vertexcloud.com` | `qa-auth.vertexcloud.com` | `stage-auth.vertexcloud.com` | `auth.vertexcloud.com` |
| `n8n-base-url` | `amp-n8n-unrestricted-webhook.dev.az.vtxdev.net` | `amp-n8n-unrestricted-webhook.qz.az.vtxdev.net` ¹ | `amp-n8n-unrestricted-webhook.stage.az.vtxdev.net` | `n8n.vertexcloud.com` |
| `api-key` | _(empty)_ | _(from env var)_ | _(from env var)_ | _(from env var)_ |
| Log level | `INFO` | `INFO` | `INFO` | `WARN` |

> ¹ The QA n8n base URL uses the `qz` subdomain (not `qa`) — this matches the value defined in `application.yml` and is intentional.