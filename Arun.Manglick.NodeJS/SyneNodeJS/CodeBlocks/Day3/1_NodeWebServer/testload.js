import http from "k6/http";
import { check } from "k6";

export default function() {
  // Send a JSON encoded POST request
  const headers = {
    'x-shopify-shop-domain': 'vertex-eks.myshopify.com',
    'x-shopify-request-id': 'requestid',
    'x-shopify-hmac-sha256': 'zsVSVsonb5PdZ8j6K6Wn46jYau92ZMiHO+iW7NIHsv0=',
    'Content-Type': 'application/json'
  }
  const body = JSON.stringify({
    "idempotent_key": "f2fa4e7898fa5102e9311d5a841be7db",
    "request": {
      "tax_included": false,
      "datetime_created_utc": "2023-04-17T08:21:48.000Z",
      "currency_code": "CAD"
    },
    "cart": {
      "buyer_identity": {
        "tax_exempt": false,
        "customer": {
          "id": "6919198867738"
        },
        "purchasing_company": null
      },
      "billing_address": {
        "address1": "1270 York Road",
        "city": "Gettysburg",
        "zip": "17325",
        "address2": null,
        "country_code": "US",
        "province_code": "PA"
      },
      "delivery_groups": [
        {
          "id": "05b63f9e002a970b7d05c851aab2d30e",
          "selected_delivery_option": {
            "subtotal_amount": {
              "amount": "1.0",
              "currency_code": "CAD"
            },
            "total_amount": {
              "amount": "1.0",
              "currency_code": "CAD"
            },
            "delivery_method_type": "SHIPPING"
          },
          "origin_address": {
            "address1": "123 Sweet Street",
            "city": "Carmangay",
            "zip": "T0L 0N0",
            "address2": null,
            "country_code": "CA",
            "province_code": "AB"
          },
          "delivery_address": {
            "address1": "1270 York Road",
            "city": "Gettysburg",
            "zip": "17325",
            "address2": null,
            "country_code": "US",
            "province_code": "PA"
          },
          "cart_lines": [
            {
              "id": "ccebfdf4e2da4ee8c663612ef657ed09",
              "quantity": 5,
              "cost": {
                "amount_per_quantity": {
                  "amount": "44.0",
                  "currency_code": "CAD"
                },
                "subtotal_amount": {
                  "amount": "220.0",
                  "currency_code": "CAD"
                },
                "total_amount": {
                  "amount": "220.0",
                  "currency_code": "CAD"
                }
              },
              "discount_allocations": [],
              "merchandise": {
                "id": "44781448593690",
                "sku": "sku-hosted-1",
                "product": {
                  "id": "44781448593690",
                  "handle": "The 3p Fulfilled Snowboard - Default Title",
                  "is_gift_card": false,
                  "metafields": []
                },
                "requires_shipping": true,
                "tax_exempt": false,
                "metafields": [],
                "__type": "PRODUCT_VARIANT"
              }
            }
          ]
        }
      ]
    },
    "shop": {
      "billing_address": {
        "address1": null,
        "city": null,
        "zip": null,
        "address2": null,
        "country_code": "IN",
        "province_code": null
      }
    }
  });

  // let res = http.post("https://bigcommerce.cst-stage.vtxdev.net/calculate_taxes", body, { headers });
  let res = http.get("http://127.0.0.1:58437/");
   // Use JSON.parse to deserialize the JSON (instead of using the r.json() method)
  let j = JSON.parse(res.body);

  // Verify response
  check(res, {
    "status is 200": (r) => r.status === 200,
  });
}