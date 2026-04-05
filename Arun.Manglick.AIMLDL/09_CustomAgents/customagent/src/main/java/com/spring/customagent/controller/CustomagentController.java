package com.spring.customagent.controller;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.bind.annotation.GetMapping;

@RestController
public class CustomagentController {
    
    @GetMapping("/customagent")
    public String helloCustomAgent() {
        return "Hello, Custom Agent SpringBoot Project!";
    }       
}
