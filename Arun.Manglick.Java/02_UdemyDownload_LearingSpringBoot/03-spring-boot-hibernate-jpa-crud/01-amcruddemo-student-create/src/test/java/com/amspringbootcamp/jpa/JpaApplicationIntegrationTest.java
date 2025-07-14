package com.amspringbootcamp.jpa;

import com.amspringbootcamp.jpa.dao.StudentDAO;
import com.amspringbootcamp.jpa.entity.Student;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;

@SpringBootTest
@Transactional
public class JpaApplicationIntegrationTest {

    @Autowired
    private StudentDAO studentDAO;

    @Test
    void createStudentPersistsAndReturnsId() {
        JpaApplication app = new JpaApplication();
        Integer id = app.createStudent(studentDAO);
        Assertions.assertNotNull(id, "Student ID should not be null after save");
        Student student = studentDAO.findStudentById(id);
        Assertions.assertNotNull(student, "Student should be found in DB after save");
        Assertions.assertEquals("Ruchi", student.getFirstName());
        Assertions.assertEquals("Manglick", student.getLastName());
        Assertions.assertEquals("arun.manglick@vertexinc.com", student.getEmail());
        Assertions.assertEquals(25, student.getAge());
        Assertions.assertEquals("Active", student.getStatus());
    }

    @Test
    void readStudentReturnsNullForNonexistentId() {
        JpaApplication app = new JpaApplication();
        Integer nonExistentId = 99999;
        Student student = studentDAO.findStudentById(nonExistentId);
        Assertions.assertNull(student, "Student should be null for nonexistent ID");
    }

    @Test
    void updateStudentUpdatesFieldsCorrectly() {
        JpaApplication app = new JpaApplication();
        Integer id = app.createStudent(studentDAO);
        Student student = studentDAO.findStudentById(id);
        Assertions.assertNotNull(student);
        student.setFirstName("UpdatedName");
        studentDAO.updateStudent(student);
        Student updated = studentDAO.findStudentById(id);
        Assertions.assertEquals("UpdatedName", updated.getFirstName());
    }

    @Test
    void deleteStudentRemovesStudentFromDatabase() {
        JpaApplication app = new JpaApplication();
        Integer id = app.createStudent(studentDAO);
        Student student = studentDAO.findStudentById(id);
        Assertions.assertNotNull(student);
        studentDAO.deleteStudent(id);
        Student deleted = studentDAO.findStudentById(id);
        Assertions.assertNull(deleted);
    }

    @Test
    void deleteAllStudentRemovesAllStudents() {
        JpaApplication app = new JpaApplication();
        app.createStudent(studentDAO);
        app.createStudent(studentDAO);
        int countBefore = studentDAO.deleteAllStudents();
        Assertions.assertTrue(countBefore >= 2);
        int countAfter = studentDAO.deleteAllStudents();
        Assertions.assertEquals(0, countAfter);
    }

    @Test
    void queryStudentsReturnsExpectedResults() {
        JpaApplication app = new JpaApplication();
        app.createStudent(studentDAO); // Ruchi Manglick
        Student s2 = new Student("Arun", "Manglick", "arun@x.com", 30, "Active");
        studentDAO.saveStudent(s2);
        List<Student> students = studentDAO.getAllStudentsbyLastName("FROM Student where firstName", "Arun");
        Assertions.assertTrue(students.stream().anyMatch(s -> "Arun".equals(s.getFirstName())));
    }
}
