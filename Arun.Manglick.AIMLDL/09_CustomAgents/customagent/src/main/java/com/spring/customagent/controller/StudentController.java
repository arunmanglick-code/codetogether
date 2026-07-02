package com.spring.customagent.controller;

import com.spring.customagent.dto.StudentRequest;
import com.spring.customagent.entity.Student;
import com.spring.customagent.service.StudentService;

import jakarta.validation.Valid;
import java.util.List;

import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseStatus;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.server.ResponseStatusException;

@RestController
@RequestMapping("/student")
public class StudentController {

    private final StudentService studentService;

    public StudentController(StudentService studentService) {
        this.studentService = studentService;
    }

    @GetMapping("/list")
    public List<Student> listStudents(
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "20") int size) {
        return studentService.getAllStudents(page, size);
    }

    @GetMapping("/{id}")
    public Student getStudent(@PathVariable Long id) {
        Student student = studentService.getStudentById(id);
        if (student == null) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Student not found");
        }
        return student;
    }

    @PostMapping("/add")
    @ResponseStatus(HttpStatus.CREATED)
    public Student addStudent(@RequestBody @Valid StudentRequest request) {
        Student student = new Student(
                request.getFirstname(),
                request.getLastname(),
                request.getEmail(),
                request.getAge(),
                request.getStatus());
        return studentService.addStudent(student);
    }

    @PutMapping("/{id}")
    public Student updateStudent(@PathVariable Long id, @RequestBody @Valid StudentRequest request) {
        Student existing = studentService.getStudentById(id);
        if (existing == null) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Student not found");
        }
        existing.setFirstname(request.getFirstname());
        existing.setLastname(request.getLastname());
        existing.setEmail(request.getEmail());
        existing.setAge(request.getAge());
        existing.setStatus(request.getStatus());
        return studentService.updateStudent(existing);
    }

    @DeleteMapping("/{id}")
    @ResponseStatus(HttpStatus.NO_CONTENT)
    public void deleteStudent(@PathVariable Long id) {
        Student existing = studentService.getStudentById(id);
        if (existing == null) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Student not found");
        }
        studentService.deleteStudent(id);
    }
}
