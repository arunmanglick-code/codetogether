package com.amspringbootcamp.jpa.controller;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;

@Controller
public class SecurityController {
    @GetMapping("/")
    public String showHome(Model theModel){
        theModel.addAttribute("serverDate", new java.util.Date());
        return "security/homePage";
    }

    @GetMapping("/login")
    public String showMyLoginPage(Model theModel){
        theModel.addAttribute("serverDate", new java.util.Date());
        return "security/loginPage";
    }

    @GetMapping("/access-denied")
    public String showAccessDenied(Model theModel){
        theModel.addAttribute("serverDate", new java.util.Date());
        return "security/accessDeniedPage";
    }

//    @PostMapping("/authenticateTheUser")
//    public String processLogin(Model theModel){
//        theModel.addAttribute("serverDate", new java.util.Date());
//        return "security/welcomePage";
//    }

//    @PostMapping("/logoutUser")
//    public String processLogout(Model theModel){
//        theModel.addAttribute("serverDate", new java.util.Date());
//        return "security/loginPage";
//    }
}
