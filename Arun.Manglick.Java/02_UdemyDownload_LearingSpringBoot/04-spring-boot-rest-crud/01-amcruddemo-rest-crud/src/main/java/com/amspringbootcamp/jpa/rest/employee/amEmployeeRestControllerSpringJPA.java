package com.amspringbootcamp.jpa.rest.employee;

import com.amspringbootcamp.jpa.entity.Employee;
import com.amspringbootcamp.jpa.service.EmployeeService;
import com.amspringbootcamp.jpa.service.EmployeeServiceSpringJPA;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/amEmployeeRestSpringJPA")
public class amEmployeeRestControllerSpringJPA {
//    private EmployeeDAO employeeDAO;
    private EmployeeServiceSpringJPA employeeService;

    @Autowired
    public amEmployeeRestControllerSpringJPA(EmployeeServiceSpringJPA employeeService){
        this.employeeService = employeeService;
    }

    @GetMapping("/")
    public String healthCheck(){
        return "Employee App is Healthy";
    }

    @GetMapping("/employees")
    public List<Employee> getEmployees(){
//        List<Employee> employeeList = employeeDAO.findAll();
        List<Employee> employeeList = employeeService.findAll();
        return employeeList;
    }

    @GetMapping("/employees/{employeeId}")
    public Employee getEmployee(@PathVariable int employeeId){
        Employee employee = employeeService.findById(employeeId);
        if (employee == null){
            throw new RuntimeException("Employee Not Found with Id: "+ employeeId);
        }
        return employee;
    }

    @PostMapping("/employees")
    public Employee addEmployee(@RequestBody Employee employee){
        employee.setId(0); // Save
        Employee dbEmployee = employeeService.save(employee);
        return dbEmployee;
    }

    @PutMapping("/employees")
    public Employee updateEmployee(@RequestBody Employee employee){
        Employee dbEmployee = employeeService.save(employee);
        return dbEmployee;
    }

    @DeleteMapping("/employees/{employeeId}")
    public String deleteEmployee(@PathVariable int employeeId){
        Employee employee = employeeService.findById(employeeId);
        if (employee == null){
            throw new RuntimeException("Employee Not Found with Id: "+ employeeId);
        }
        employeeService.deleteById(employeeId);
        return "Delete Employee with Id:" + employeeId;
    }
}
