package com.spring.customagent.service;

import com.spring.customagent.entity.Student;

import java.util.List;

public interface StudentService {
    List<Student> getAllStudents();
    List<Student> getStudentByName(String firstname);
    Student addStudent(Student student);
    Student updateStudent(Student student);
    void deleteStudent(Long id);
}
