package com.arunmanglick.openapi;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.Bean;
import org.springframework.boot.CommandLineRunner;

@SpringBootApplication
public class OpenapiApplication {
	public static void main(String[] args) {
		SpringApplication.run(OpenapiApplication.class, args);
	}
	@Bean
    public CommandLineRunner commandLineRunner(String[] args) {
        return runner -> {
            System.out.println("AM Welcome to OpenAPI Project");
        };
    }
}
