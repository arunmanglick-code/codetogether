package com.amspringbootcamp.jpa.security;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.http.HttpMethod;
import org.springframework.security.config.Customizer;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.provisioning.InMemoryUserDetailsManager;
import org.springframework.security.provisioning.JdbcUserDetailsManager;
import org.springframework.security.provisioning.UserDetailsManager;
import org.springframework.security.web.SecurityFilterChain;

import javax.sql.DataSource;

@Configuration
public class SecurityConfig {

    @Bean
    public UserDetailsManager userDetailsManager(DataSource dataSource){
        JdbcUserDetailsManager theUserDetailsManager = new JdbcUserDetailsManager(dataSource);

        /* Below code is required in case you want to use your custom tables instead of default 'users' and 'authorities'
        theUserDetailsManager
                .setUsersByUsernameQuery("select user_id, pw, active from members where user_id=?");
        theUserDetailsManager
                .setAuthoritiesByUsernameQuery("select user_id, role from roles where user_id=?");
        */

        return theUserDetailsManager;
    }

//    @Bean
//    public InMemoryUserDetailsManager userDetailConfiguration() {
//        // {noop} Let’s Spring Security know the passwords are stored as plain text
//
//        UserDetails user1 = User.builder()
//                .username("user1")
//                .password("{noop}tester1")
//                .roles("EMPLOYEE")
//                .build();
//
//        UserDetails user2 = User.builder()
//                .username("user2")
//                .password("{noop}tester2")
//                .roles("EMPLOYEE", "MANAGER")
//                .build();
//
//        UserDetails user3 = User.builder()
//                .username("user3")
//                .password("{noop}tester3")
//                .roles("EMPLOYEE", "MANAGER", "ADMIN")
//                .build();
//
//        return new InMemoryUserDetailsManager(user1, user2, user3);
//    }

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
