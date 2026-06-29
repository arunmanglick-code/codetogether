package com.spring.customagent.controller;

import com.spring.customagent.entity.Student;
import com.spring.customagent.request.StudentRequest;
import com.spring.customagent.service.StudentService;

import jakarta.validation.Valid;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

@RestController
@RequestMapping("/student")
public class StudentController {

    private final StudentService studentService;

    public StudentController(StudentService studentService) {
        this.studentService = studentService;
    }

    @GetMapping("/list")
    public Page<Student> listStudents(Pageable pageable) {
        return studentService.getAllStudents(pageable);
    }

    @GetMapping("/{id}")
    public Student getStudent(@PathVariable Long id) {
        Student student = studentService.getStudentById(id);
        if (student == null) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Student not found with id: " + id);
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
                "ACTIVE"
        );
        return studentService.addStudent(student);
    }

    @PutMapping("/{id}")
    public Student updateStudent(@PathVariable Long id, @RequestBody @Valid StudentRequest request) {
        Student existing = studentService.getStudentById(id);
        if (existing == null) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Student not found with id: " + id);
        }
        existing.setFirstname(request.getFirstname());
        existing.setLastname(request.getLastname());
        existing.setEmail(request.getEmail());
        existing.setAge(request.getAge());
        return studentService.updateStudent(existing);
    }

    @DeleteMapping("/{id}")
    @ResponseStatus(HttpStatus.NO_CONTENT)
    public void deleteStudent(@PathVariable Long id) {
        Student existing = studentService.getStudentById(id);
        if (existing == null) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Student not found with id: " + id);
        }
        studentService.deleteStudent(id);
    }
}
