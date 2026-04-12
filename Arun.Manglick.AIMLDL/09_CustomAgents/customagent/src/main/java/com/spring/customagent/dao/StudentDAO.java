package com.spring.customagent.dao;

import com.spring.customagent.entity.Student;
import jakarta.annotation.Nonnull;

import java.util.List;

public interface StudentDAO {

    List<Student> getAllStudents();

    Student getStudentById(@Nonnull Long id);

    List<Student> getStudentsByFirstname(@Nonnull String firstname);

    Student addStudent(@Nonnull Student student);

    Student updateStudent(@Nonnull Student student);

    void deleteStudent(@Nonnull Long id);
}
