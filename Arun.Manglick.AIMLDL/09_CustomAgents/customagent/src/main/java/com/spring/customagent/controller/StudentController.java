package com.spring.customagent.controller;

import com.spring.customagent.entity.Student;
import com.spring.customagent.service.StudentService;

import java.util.List;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class StudentController {

    private final StudentService studentService;

    public StudentController(StudentService studentService) {
        this.studentService = studentService;
    }

    @GetMapping("/student")
    public String helloStudent() {
        return "Hello, Student REST Controller SpringBoot Project!";
    }

    @GetMapping("/student/list")
    public List<Student> listStudents() {
        return studentService.getAllStudents();
    }

    @GetMapping("/student/list/paged")
    public Page<Student> listStudentsPaged(Pageable pageable) {
        return studentService.getAllStudents(pageable);
    }

    @PostMapping("/student/add")
    public Student addStudent(@RequestBody Student student) {
        return studentService.addStudent(student);
    }
}
