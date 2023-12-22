package com.amspringbootcamp.jpa;

import com.amspringbootcamp.jpa.dao.StudentDAO;
import com.amspringbootcamp.jpa.entity.Student;
import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.Bean;

import java.util.List;

@SpringBootApplication
public class JpaApplication {

    public static void main(String[] args) {
        SpringApplication.run(JpaApplication.class, args);
    }

//    @Bean
//    public CommandLineRunner commandLineRunner(String[] args) {
//        return runner -> {
//            System.out.println("AM Welcome to JPA");
//        };
//    }

    @Bean
    public CommandLineRunner commandLineRunner(StudentDAO objStudentDao) {
        return runner -> {
            System.out.println("AM Welcome to JPA");
            Integer studentId = createStudent(objStudentDao);
//            readStudent(studentId, objStudentDao);
//            queryStudents(objStudentDao);
//            UpdateStudents(objStudentDao);
//            updateStudent(objStudentDao);
//            deleteStudent(objStudentDao);
//            deleteAllStudent(objStudentDao);
        };
    }

    private Integer createStudent(StudentDAO objStudentDao) {
        // Create Student Object
        // Save Student Object
        // Display Id of Saved Student Object
        System.out.println("Creating New Student");

        Student objStudent = new Student("Ruchi", "Manglick", "arun.manglick@vertexinc.com", 25, "Active");
        objStudentDao.saveStudent(objStudent);
        Integer studentId = objStudent.getId();

        System.out.println("Saved Student Id:" + studentId);
        return studentId;
    }

    private void readStudent(Integer id, StudentDAO objStudentDAO){
        System.out.println("Read Student having Id:" + id);
        Student objStudent = objStudentDAO.findStudentById(id);

        if(objStudent != null)
            System.out.println("Found Student Details: " + objStudent);
    }
    private void queryStudents(StudentDAO objStudentDao) {
        String query1 = "FROM Student";
        String query2 = "FROM Student where firstName = 'Arun'";
        String query3 = "FROM Student where firstName = 'Arun' OR lastName = 'Manglick'";
        String query4 = "FROM Student where email LIKE '%.com%' order by lastName desc";
        String query5 = "FROM Student where firstName";
        String updateQuery1 = "Update Student set email='arunmanglick@gmail.com'";

        List<Student> studentList = objStudentDao.getAllStudentsbyLastName(query5, "Arun");

        for (Student tempStudent:studentList)
            System.out.println("Queried Student Details: " + tempStudent);
    }

    private void UpdateStudents(StudentDAO objStudentDao) {
        String updateQuery1 = "Update Student set email='arunmanglick@gmail.com'";
        int results =objStudentDao.UpdateAllStudents(updateQuery1);
        System.out.println("Total Student Updated: " + results);
    }


    private void updateStudent(StudentDAO objStudentDAO){
        int id = 1;
        System.out.println("Read Student Before Update having Id:" + id);
        Student objStudent = objStudentDAO.findStudentById(id);
        System.out.println("Found Student Details: " + objStudent);

        if(objStudent != null) {
            objStudent.setFirstName("John");
            objStudentDAO.updateStudent(objStudent);

            System.out.println("Read Student After Update");
            objStudent = objStudentDAO.findStudentById(id);
            System.out.println("Found Student Details: " + objStudent);
        }
    }


    private void deleteStudent(StudentDAO objStudentDAO){
        int id = 2;
        System.out.println("Read Student Before Update having Id:" + id);
        Student objStudent = objStudentDAO.findStudentById(id);
        System.out.println("Found Student Details: " + objStudent);

        if(objStudent != null) {
            objStudentDAO.deleteStudent(id);
            System.out.println("Student Deleted with Id:" + id);
        }
        else
            System.out.println("Student Not Found to Delete with Id:" + id);
    }

    private void deleteAllStudent(StudentDAO objStudentDAO){
        int count = objStudentDAO.deleteAllStudents();
        System.out.println("Total Student Deleted:" + count);
    }
}
