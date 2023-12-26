package com.amspringbootcamp.jpa.security;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.http.HttpMethod;
import org.springframework.security.config.Customizer;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.provisioning.InMemoryUserDetailsManager;
import org.springframework.security.web.SecurityFilterChain;

@Configuration
public class SecurityConfig {

    @Bean
    public InMemoryUserDetailsManager userDetailConfiguration(){
        // {noop} Let’s Spring Security know the passwords are stored as plain text

        UserDetails user1= User.builder()
                .username("user1")
                .password("{noop}tester1")
                .roles("EMPLOYEE")
                .build();

        UserDetails user2= User.builder()
                .username("user2")
                .password("{noop}tester2")
                .roles("EMPLOYEE","MANAGER")
                .build();

        UserDetails user3= User.builder()
                .username("user3")
                .password("{noop}tester3")
                .roles("EMPLOYEE","MANAGER","ADMIN")
                .build();

        return new InMemoryUserDetailsManager(user1,user2,user3);
    }

    @Bean
    public SecurityFilterChain filterChain(HttpSecurity http) throws Exception {
        http.authorizeHttpRequests(config ->
                config
                        .requestMatchers(HttpMethod.GET, "/amEmployeeRest/employees").hasRole("EMPLOYEE")
                        .requestMatchers(HttpMethod.GET, "/amEmployeeRest/employees/**").hasRole("EMPLOYEE")
                        .requestMatchers(HttpMethod.POST, "/amEmployeeRest/employees").hasRole("MANAGER")
                        .requestMatchers(HttpMethod.PUT, "/amEmployeeRest/employees").hasRole("MANAGER")
                        .requestMatchers(HttpMethod.DELETE, "/amEmployeeRest/employees/**").hasRole("ADMIN"));

        // use HTTP Basic authentication
        http.httpBasic(Customizer.withDefaults());
        http.csrf(csrf -> csrf.disable());
        return http.build();
    }
}
