package com.amspringbootcamp.jpa.controller;

import com.amspringbootcamp.jpa.model.Student;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.beans.propertyeditors.StringTrimmerEditor;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.validation.BindingResult;
import org.springframework.web.bind.WebDataBinder;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@Controller
public class StudentController {

    @Value("${countries}")
    private List<String> countries;

    @Value("${languages}")
    private List<String> languages;

    @Value("${operatingsystem}")
    private List<String> operatingsystem;

    @GetMapping("/mvc/student/health")
    public String healthCheck(Model theModel){
        theModel.addAttribute("serverDate", new java.util.Date());
        return "checkHealth";
    }

    @GetMapping("/mvc/student/onboarding")
    public String showOnboardingForm(Model theModel) {
        theModel.addAttribute("student", new Student());
        theModel.addAttribute("countries", countries);
        theModel.addAttribute("languages", languages);
        theModel.addAttribute("operatingsystem", operatingsystem);
        return "mvc/onboardingStudentForm";
    }

    // Lecture 199- This is used to trim the whitespace before applying validation
    @InitBinder
    public void initBinder(WebDataBinder dataBinder) {
        StringTrimmerEditor stringTrimmerEditor = new StringTrimmerEditor(true);
        dataBinder.registerCustomEditor(String.class, stringTrimmerEditor);
    }
    @PostMapping("/mvc/student/processOnboarding")
    public String processStudentOnboardingForm(@Valid @ModelAttribute("student") Student theStudent, BindingResult theBindingResult,Model theModel ) {
        // read the request parameter from the HTML form
        if(!theBindingResult.hasErrors()) {
            String firstName = theStudent.getFirstName();
            String lastName = theStudent.getLastName();
            String country = theStudent.getCountry();
            return "processStudentOnboardingForm";
        }
        else
        {
            theModel.addAttribute("countries", countries);
            theModel.addAttribute("languages", languages);
            theModel.addAttribute("operatingsystem", operatingsystem);
            return "mvc/onboardingStudentForm";
        }
    }

}
