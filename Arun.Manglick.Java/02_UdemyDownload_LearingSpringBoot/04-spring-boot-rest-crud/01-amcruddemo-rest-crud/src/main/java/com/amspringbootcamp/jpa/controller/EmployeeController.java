package com.amspringbootcamp.jpa.controller;

import com.amspringbootcamp.jpa.entity.Employee;
import com.amspringbootcamp.jpa.model.Student;
import com.amspringbootcamp.jpa.service.EmployeeService;
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
@RequestMapping("/mvc/employee")
public class EmployeeController {

    private EmployeeService employeeService;

    public EmployeeController(EmployeeService employeeService) {
        this.employeeService = employeeService;
    }

    @Value("${countries}")
    private List<String> countries;

    @Value("${languages}")
    private List<String> languages;

    @Value("${operatingsystem}")
    private List<String> operatingsystem;

    @GetMapping("/health")
    public String healthCheck(Model theModel){
        theModel.addAttribute("serverDate", new java.util.Date());
        return "mvc/checkEmployeeHealth";
    }

    @GetMapping("/list")
    public String listEmployees(Model theModel){
        List<Employee> employeeList = employeeService.findAll();
        theModel.addAttribute("employees", employeeList);
        return "mvc/show-list-employees-page";
    }

    @GetMapping("/addEmployee")
    public String addNewEmployee(Model theModel) {
        theModel.addAttribute("employee", new Employee());
        return "mvc/show-new-employees-page";
    }

    @GetMapping("/updateEmployee")
    public String updateEmployee(@RequestParam("employeeId") int theId,Model theModel) {
        Employee theEmployee = employeeService.findById(theId);
        theModel.addAttribute("employee", theEmployee);
        return "mvc/show-update-employees-page";
    }

    @GetMapping("/deleteEmployee")
    public String deleteEmployee(@RequestParam("employeeId") int theId,Model theModel) {
        employeeService.deleteById(theId);
        return "redirect:/mvc/employee/list";
    }

    @PostMapping("/addEmployee")
    public String saveEmployee(@ModelAttribute("employee") Employee theEmployee) {
        employeeService.save(theEmployee);
        return "redirect:/mvc/employee/list";
    }

    @GetMapping("/mvc/employees")
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


}
