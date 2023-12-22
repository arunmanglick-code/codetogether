package com.amspringbootcamp.jpa.dao;

import com.amspringbootcamp.jpa.entity.Student;

import java.util.List;

public interface StudentDAO {
    void saveStudent(Student objStudent);

    Student findStudentById(Integer id);

    List<Student> getAllStudents(String query);

    List<Student> getAllStudentsbyLastName(String query, String theLastName);

    void updateStudent(Student objStudent);

    int UpdateAllStudents(String query);

    void deleteStudent(Integer id);

    int deleteAllStudents();
}
