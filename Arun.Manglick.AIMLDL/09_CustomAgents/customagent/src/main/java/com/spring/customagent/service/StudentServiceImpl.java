package com.spring.customagent.service;

import com.spring.customagent.dao.StudentDAO;
import com.spring.customagent.entity.Student;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class StudentServiceImpl implements StudentService {

    @Autowired
    private StudentDAO studentDAO;

    @Override
    public Page<Student> getAllStudents(Pageable pageable) {
        List<Student> all = studentDAO.getAllStudents();
        int start = (int) pageable.getOffset();
        int end = Math.min(start + pageable.getPageSize(), all.size());
        List<Student> pageContent = (start > all.size()) ? List.of() : all.subList(start, end);
        return new PageImpl<>(pageContent, pageable, all.size());
    }

    @Override
    public List<Student> getStudentByName(String firstname) {
        return studentDAO.getStudentsByFirstname(firstname);
    }

    @Override
    public Student getStudentById(Long id) {
        return studentDAO.getStudentById(id);
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
