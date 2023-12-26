package com.amspringbootcamp.jpa.dao;

import com.amspringbootcamp.jpa.entity.Employee;

import java.util.List;

public interface EmployeeDAO {
    List<Employee> findAll();

    Employee findById(int employeeId);

    Employee save (Employee theEmployee);

    void deleteById(int employeeId);
}
