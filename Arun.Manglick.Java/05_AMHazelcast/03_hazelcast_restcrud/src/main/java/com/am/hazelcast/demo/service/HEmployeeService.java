package com.am.hazelcast.demo.service;

import com.am.hazelcast.demo.entity.Employee;

import java.util.List;

public interface HEmployeeService {
    List<Employee> findAll();
}
