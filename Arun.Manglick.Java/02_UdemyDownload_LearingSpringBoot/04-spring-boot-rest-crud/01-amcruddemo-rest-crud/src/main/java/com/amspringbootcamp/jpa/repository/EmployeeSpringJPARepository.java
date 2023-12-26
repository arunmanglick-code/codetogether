package com.amspringbootcamp.jpa.repository;

import com.amspringbootcamp.jpa.entity.Employee;
import org.springframework.data.jpa.repository.JpaRepository;

public interface EmployeeSpringJPARepository extends JpaRepository<Employee,Integer> {
    // That's it, no further code required, No Implementation required
}
