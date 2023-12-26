package com.amspringbootcamp.jpa.service;

import com.amspringbootcamp.jpa.dao.EmployeeDAO;
import com.amspringbootcamp.jpa.entity.Employee;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;

@Service
public class EmployeeServiceImpl implements EmployeeService {
    private EmployeeDAO employeeDAO;

    public EmployeeServiceImpl(EmployeeDAO employeeDAO) {
        this.employeeDAO = employeeDAO;
    }

    @Override
    public List<Employee> findAll() {
        List<Employee> employeeList = employeeDAO.findAll();
        return employeeList;
    }

    @Override
    public Employee findById(int employeeId) {
        Employee objEmployee = employeeDAO.findById(employeeId);
        return objEmployee;
    }

    @Transactional
    @Override
    public Employee save(Employee theEmployee) {
        Employee objEmployee = employeeDAO.save(theEmployee);
        return objEmployee;
    }

    @Transactional
    @Override
    public void deleteById(int employeeId) {
        employeeDAO.deleteById(employeeId);
    }
}
