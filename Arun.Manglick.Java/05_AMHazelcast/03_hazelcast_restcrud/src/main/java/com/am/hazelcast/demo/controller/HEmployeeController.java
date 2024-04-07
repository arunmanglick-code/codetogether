package com.am.hazelcast.demo.controller;

import com.am.hazelcast.demo.entity.Employee;
import com.am.hazelcast.demo.service.HEmployeeService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

import java.util.List;

@Controller
@RequestMapping("/hazel")
public class HEmployeeController {

    private HEmployeeService employeeService;
    public HEmployeeController(HEmployeeService employeeService) {
        this.employeeService = employeeService;
    }

    @GetMapping("/health")
    public String healthCheck(Model theModel){
        theModel.addAttribute("serverDate", new java.util.Date());
        return "/checkHazelHealth";
    }

    @GetMapping("/list")
    public String listEmployees(Model theModel){
        List<Employee> employeeList = employeeService.findAll();
        theModel.addAttribute("employees", employeeList);
        return "/show-list-employees-page";
    }
}
