package com.amspringbootcamp.jpa.controller;

import com.amspringbootcamp.jpa.entity.Employee;
import com.amspringbootcamp.jpa.model.Student;
import com.amspringbootcamp.jpa.service.EmployeeService;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.beans.propertyeditors.StringTrimmerEditor;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.validation.BindingResult;
import org.springframework.web.bind.WebDataBinder;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

@Controller
@RequestMapping("/mvc/employee")
public class EmployeeController {

    private EmployeeService employeeService;

    public EmployeeController(EmployeeService employeeService) {
        this.employeeService = employeeService;
    }

    @Autowired
    private Map<String, List<Employee>>  employeeMap;

    @Value("${countries}")
    private List<String> countries;

    @Value("${languages}")
    private List<String> languages;

    @Value("${operatingsystem}")
    private List<String> operatingsystem;

// ------------------------------------------------------------
    @GetMapping("/health")
    public String healthCheck(Model theModel){
        theModel.addAttribute("serverDate", new java.util.Date());
        return "mvc/checkEmployeeHealth";
    }

// Not in use - Code Commenting 24Mar2024
    @GetMapping("/mvc/employees")
    public String showOnboardingForm(Model theModel) {
        theModel.addAttribute("student", new Student());
        theModel.addAttribute("countries", countries);
        theModel.addAttribute("languages", languages);
        theModel.addAttribute("operatingsystem", operatingsystem);
        return "mvc/onboardingStudentForm";
    }
// --------------------------------------------------------------------------------------------
    @GetMapping("/list")
    public String listEmployees(Model theModel){
        List<Employee> employeeList = employeeMap.get("1");
        if(employeeList == null) {
            employeeList = employeeService.findAll();
            employeeMap.put("1", employeeList);
        }
        theModel.addAttribute("employees", employeeList);
        return "mvc/show-list-employees-page";
    }

    @GetMapping("/addEmployee")
    public String addNewEmployee(Model theModel) {
        theModel.addAttribute("employee", new Employee());
        return "mvc/show-new-employees-page";
    }

    @PostMapping("/addEmployee")
    public String saveEmployee(@ModelAttribute("employee") Employee theEmployee) {
        //save Employee in cache and Database
        employeeService.save(theEmployee);
        employeeMap.remove("1");
        return "redirect:/mvc/employee/list";
    }

    @GetMapping("/updateEmployee")
    public String updateEmployee(@RequestParam("employeeId") int theId,Model theModel) {
        Employee theEmployee = employeeService.findById(theId);
        theModel.addAttribute("employee", theEmployee);
        return "mvc/show-update-employees-page";
    }

    @PostMapping("/updateEmployee")
    public String updateEmployee(@ModelAttribute("employee") Employee theEmployee) {
        employeeService.save(theEmployee);
        employeeMap.remove("1");
        return "redirect:/mvc/employee/list";
    }
    @PostMapping("/deleteEmployee")
    public String deleteEmployee(@RequestParam("employeeId") int theId,Model theModel) {
        employeeService.deleteById(theId);
        employeeMap.remove("1");
        return "redirect:/mvc/employee/list";
    }
// --------------------------------------------------------------------------------------------

    // Lecture 199- This is used to trim the whitespace before applying validation
    @InitBinder
    public void initBinder(WebDataBinder dataBinder) {
        StringTrimmerEditor stringTrimmerEditor = new StringTrimmerEditor(true);
        dataBinder.registerCustomEditor(String.class, stringTrimmerEditor);
    }
}
