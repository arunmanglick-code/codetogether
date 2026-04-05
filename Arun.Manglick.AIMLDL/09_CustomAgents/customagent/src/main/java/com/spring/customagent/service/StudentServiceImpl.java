package com.spring.customagent.service;

import com.spring.customagent.dao.StudentDAO;
import com.spring.customagent.entity.Student;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class StudentServiceImpl implements StudentService {

    @Autowired
    private StudentDAO studentDAO;

    @Override
    public List<Student> getAllStudents() {
        return studentDAO.getAllStudents();
    }

    @Override
    public List<Student> getStudentByName(String firstname) {
        return studentDAO.getStudentByName(firstname);
    }

    @Override
    public Student addStudent(Student student) {
        return studentDAO.addStudent(student);
    }

    @Override
    public Student updateStudent(Student student) {
        return studentDAO.updateStudent(student);
    }

    @Override
    public void deleteStudent(Long id) {
        studentDAO.deleteStudent(id);
    }
}
