package com.spring.customagent.dao;

import com.spring.customagent.entity.Student;

import java.util.List;

public interface StudentDAO {

    List<Student> getAllStudents();

    List<Student> getStudentByName(String firstname);

    Student addStudent(Student student);

    Student updateStudent(Student student);

    void deleteStudent(Long id);
}
