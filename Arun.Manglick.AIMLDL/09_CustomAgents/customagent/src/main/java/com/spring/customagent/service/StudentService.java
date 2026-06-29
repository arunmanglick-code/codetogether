package com.spring.customagent.service;

import com.spring.customagent.entity.Student;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;

import java.util.List;

public interface StudentService {
    Page<Student> getAllStudents(Pageable pageable);
    List<Student> getStudentByName(String firstname);
    Student getStudentById(Long id);
    Student addStudent(Student student);
    Student updateStudent(Student student);
    void deleteStudent(Long id);
}
