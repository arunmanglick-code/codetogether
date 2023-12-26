package com.amspringbootcamp.jpa.rest.student;

import com.amspringbootcamp.jpa.entity.Student;
import com.amspringbootcamp.jpa.entity.StudentNotFoundException;
import jakarta.annotation.PostConstruct;
import org.springframework.web.bind.annotation.*;

import java.util.ArrayList;
import java.util.List;

@RestController
@RequestMapping("/amStudentRest")
public class amStudentRestController {

    // StudentNotFoundException Exception
//    @ExceptionHandler
//    public ResponseEntity<StudentErrorResponse> handleException(StudentNotFoundException exc) {
//        StudentErrorResponse errorResponse = new StudentErrorResponse();
//
//        errorResponse.setErrorStatus(NOT_FOUND.value());
//        errorResponse.setErrorMessage(exc.getMessage());
//        errorResponse.setErrorTimestamp(System.currentTimeMillis());
//
//        return new ResponseEntity<>(errorResponse, NOT_FOUND);
//    }
//    // Generic Exception
//    @ExceptionHandler
//    public ResponseEntity<StudentErrorResponse> handleException(Exception exc) {
//        StudentErrorResponse errorResponse = new StudentErrorResponse();
//
//        errorResponse.setErrorStatus(BAD_REQUEST.value());
//        errorResponse.setErrorMessage(exc.getMessage());
//        errorResponse.setErrorTimestamp(System.currentTimeMillis());
//
//        return  new ResponseEntity<>(errorResponse, BAD_REQUEST);
//    }

    List<Student> theStudents = new ArrayList<>();
    @GetMapping("/")
    public String healthCheck(){
        return "Student App is Healthy";
    }

    @PostConstruct
    public void loadStudentData(){
        theStudents = new ArrayList<>();
        theStudents.add(new Student("John","Scott","johnscott@xyz.com",21,"Active"));
        theStudents.add(new Student("Peter","Wolf","johnscott@xyz.com",21,"Active"));
        theStudents.add(new Student("Andrew","Tiger","johnscott@xyz.com",21,"Active"));
    }

    @GetMapping("/students")
    public List<Student> getStudents(){
//        theStudents = new ArrayList<>();
//        theStudents.add(new Student("John","Scott","johnscott@xyz.com",21,"Active"));
//        theStudents.add(new Student("Peter","Wolf","johnscott@xyz.com",21,"Active"));
//        theStudents.add(new Student("Andrew","Tiger","johnscott@xyz.com",21,"Active"));
        return theStudents;
    }

    @GetMapping("/students/{studentId}")
    public Student getStudent(@PathVariable int studentId){

        if((studentId >= theStudents.size()) || studentId < 0){
            throw new StudentNotFoundException("AM Student Id Not Found: " + studentId);
        }
        return theStudents.get(studentId);
    }
}
