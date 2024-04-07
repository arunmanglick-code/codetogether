package com.am.hazelcast.demo.dao;

import com.am.hazelcast.demo.entity.Employee;

import java.util.List;

public interface EmployeeDAO {
    List<Employee> findAll();
}
