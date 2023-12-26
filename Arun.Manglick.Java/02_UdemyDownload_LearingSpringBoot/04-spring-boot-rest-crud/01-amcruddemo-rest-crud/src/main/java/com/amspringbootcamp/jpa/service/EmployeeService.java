package com.amspringbootcamp.jpa.service;

import com.amspringbootcamp.jpa.entity.Employee;

import java.util.List;
import java.util.Optional;

public interface EmployeeService {
    List<Employee> findAll();
    Employee findById(int employeeId);

    Employee save (Employee theEmployee);

    void deleteById(int employeeId);
}
