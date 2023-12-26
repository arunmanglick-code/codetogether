package com.amspringbootcamp.jpa.service;

import com.amspringbootcamp.jpa.dao.EmployeeDAO;
import com.amspringbootcamp.jpa.entity.Employee;
import com.amspringbootcamp.jpa.repository.EmployeeSpringJPARepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Optional;

@Service
public class EmployeeServiceImplSpringJPA implements EmployeeServiceSpringJPA {
    private EmployeeSpringJPARepository employeeRepo;

    public EmployeeServiceImplSpringJPA(EmployeeSpringJPARepository theEmployeeRepo) {
        this.employeeRepo = theEmployeeRepo;
    }

    @Override
    public List<Employee> findAll() {
        List<Employee> employeeList = employeeRepo.findAll();
        return employeeList;
    }

    @Override
    public Employee findById(int employeeId) {
        Optional<Employee> result = employeeRepo.findById(employeeId);
        Employee objEmployee = null;
        if(result.isPresent()) {
            objEmployee = result.get();
        }
        return objEmployee;
    }

    @Override
    public Employee save(Employee theEmployee) {
        Employee objEmployee = employeeRepo.save(theEmployee);
        return objEmployee;
    }

    @Override
    public void deleteById(int employeeId) {
        employeeRepo.deleteById(employeeId);
    }
}
