package com.amspringbootcamp.jpa.controller;

import jakarta.servlet.http.HttpServletRequest;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

@Controller
public class amController {
    @GetMapping("/mvc/health")
    public String healthCheck(Model theModel){
        theModel.addAttribute("serverDate", new java.util.Date());
        return "checkHealth";
    }

    @RequestMapping("/mvc/onboarding")
    public String showOnboardingForm() {
         return "onboardingForm";
    }

    @RequestMapping("/mvc/processOnboarding")
    public String processOnboardingForm(HttpServletRequest request, Model onBoardModel) {
        // read the request parameter from the HTML form
        String userName = request.getParameter("userName");

        // convert the data to all caps
        userName = userName.toUpperCase();

        // create the message
        String result = "Welcome! " + userName;
        // add message to the model
        onBoardModel.addAttribute("message", result);

        return "processOnboardingForm";
    }
}
