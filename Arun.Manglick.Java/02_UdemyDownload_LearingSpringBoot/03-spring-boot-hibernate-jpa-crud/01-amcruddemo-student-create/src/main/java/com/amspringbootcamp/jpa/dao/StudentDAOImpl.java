package com.amspringbootcamp.jpa.dao;


import com.amspringbootcamp.jpa.entity.Student;
import jakarta.persistence.EntityManager;
import jakarta.persistence.TypedQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;

@Repository
public class StudentDAOImpl implements StudentDAO{

    // Define field for Entity Manager
    private EntityManager entityManager;

    // Inject Entity Manager using Constructor Injection
    @Autowired
    public StudentDAOImpl(EntityManager entityManager) {
        this.entityManager = entityManager;
    }

    // Implement SaveStudent Method
    @Override
    @Transactional
    public void saveStudent(Student objStudent) {
        entityManager.persist(objStudent);
    }

    @Override
    public Student findStudentById(Integer id) {
        return entityManager.find(Student.class, id);
    }

    @Override
    @Transactional
    public List<Student> getAllStudents(String query) {
        TypedQuery<Student> theQuery = entityManager.createQuery(query, Student.class);
        return theQuery.getResultList();
    }

    @Override
    public List<Student> getAllStudentsbyLastName(String query, String theLastName) {
        TypedQuery<Student> theQuery = entityManager.createQuery(query + "=:theData", Student.class);
        theQuery.setParameter("theData", theLastName);
        return theQuery.getResultList();
    }
    @Override
    public int UpdateAllStudents(String query){
        int rowsUpdated  = entityManager.createQuery(query).executeUpdate();
        return rowsUpdated;
    }


    @Override
    @Transactional
    public void updateStudent(Student objStudent) {
        entityManager.merge(objStudent);
    }


    @Override
    @Transactional
    public void deleteStudent(Integer id) {
        Student objStudent = findStudentById(id);
        if(objStudent != null) {
            {
                entityManager.remove(objStudent);
            }
        }
    }
    @Override
    @Transactional
    public int deleteAllStudents() {
        int rowsDeleted  = entityManager.createQuery("Delete from Student").executeUpdate();
        return rowsDeleted;
    }

}
