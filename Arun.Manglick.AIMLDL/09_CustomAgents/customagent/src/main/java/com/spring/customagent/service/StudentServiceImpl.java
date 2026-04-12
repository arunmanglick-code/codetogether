package com.spring.customagent.service;

import com.spring.customagent.entity.Student;
import com.spring.customagent.repository.StudentRepository;
import jakarta.persistence.EntityNotFoundException;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.lang.NonNull;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;

@Service
public class StudentServiceImpl implements StudentService {

    private static final Logger logger = LoggerFactory.getLogger(StudentServiceImpl.class);

    private final StudentRepository studentRepository;

    public StudentServiceImpl(StudentRepository studentRepository) {
        this.studentRepository = studentRepository;
    }

    @Override
    public List<Student> getAllStudents() {
        return studentRepository.findAll();
    }

    @Override
    public Page<Student> getAllStudents(Pageable pageable) {
        return studentRepository.findAll(pageable);
    }

    @Override
    public List<Student> getStudentsByFirstname(@NonNull String firstname) {
        if (firstname.isBlank()) {
            throw new IllegalArgumentException("Firstname must not be blank");
        }
        return studentRepository.findByFirstname(firstname);
    }

    @Override
    public Student getStudentById(@NonNull Long id) {
        return studentRepository.findById(id)
                .orElseThrow(() -> new EntityNotFoundException("Student not found with id: " + id));
    }

    @Override
    @Transactional
    public Student addStudent(@NonNull Student student) {
        logger.debug("Adding student with firstname: {}", student.getFirstname());
        return studentRepository.save(student);
    }

    @Override
    @Transactional
    public Student updateStudent(@NonNull Student student) {
        if (student.getId() == null || !studentRepository.existsById(student.getId())) {
            throw new EntityNotFoundException("Student not found with id: " + student.getId());
        }
        return studentRepository.save(student);
    }

    @Override
    @Transactional
    public void deleteStudent(@NonNull Long id) {
        if (!studentRepository.existsById(id)) {
            throw new EntityNotFoundException("Student not found with id: " + id);
        }
        studentRepository.deleteById(id);
    }
}
