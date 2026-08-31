package com.spring.customagent.repository;

import com.spring.customagent.entity.Student;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;
import java.util.Optional;

public interface StudentRepository extends JpaRepository<Student, Long> {
    List<Student> findByFirstName(String firstName);
    Page<Student> findByFirstName(String firstName, Pageable pageable);
    Optional<Student> findByEmail(String email);
}
