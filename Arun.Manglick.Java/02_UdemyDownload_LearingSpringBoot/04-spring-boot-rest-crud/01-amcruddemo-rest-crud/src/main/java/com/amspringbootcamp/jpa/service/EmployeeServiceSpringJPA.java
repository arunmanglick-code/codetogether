package com.amspringbootcamp.jpa.service;

import com.amspringbootcamp.jpa.entity.Employee;

import java.util.List;

public interface EmployeeServiceSpringJPA {
    List<Employee> findAll();
    Employee findById(int employeeId);

    Employee save (Employee theEmployee);

    void deleteById(int employeeId);
}
