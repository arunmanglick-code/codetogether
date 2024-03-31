package com.am.hazelcast.demo.service;

import com.am.hazelcast.demo.dao.EmployeeDAO;
import com.am.hazelcast.demo.entity.Employee;

import java.util.List;

public class HEmployeeServiceImpl implements HEmployeeService {
    private EmployeeDAO employeeDAO;
    @Override
    public List<Employee> findAll() {
        List<Employee> employeeList = employeeDAO.findAll();
        return employeeList;
    }
}
