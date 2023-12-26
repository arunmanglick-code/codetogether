package com.amspringbootcamp.jpa.dao;

import com.amspringbootcamp.jpa.entity.Employee;
import com.amspringbootcamp.jpa.entity.Student;
import jakarta.persistence.EntityManager;
import jakarta.persistence.TypedQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public class EmployeeDAOImpl implements EmployeeDAO{

    // Define field for Entity Manager
    private EntityManager entityManager;

    // Inject Entity Manager using Constructor Injection
    @Autowired
    public EmployeeDAOImpl(EntityManager entityManager) {
        this.entityManager = entityManager;
    }
    @Override
    public List<Employee> findAll() {
        String query1 = "FROM Employee";
        TypedQuery<Employee> theQuery = entityManager.createQuery(query1, Employee.class);
        return theQuery.getResultList();
    }

    @Override
    public Employee findById(int employeeId) {
        return entityManager.find(Employee.class, employeeId);
    }

    @Override
    public Employee save(Employee theEmployee) {
//        entityManager.persist(theEmployee);

        // if (id==0,then save, else update)
        Employee objEmployee = entityManager.merge(theEmployee);
        return  objEmployee;
    }

    @Override
    public void deleteById(int employeeId) {
        Employee theEmployee = findById(employeeId);
        if(theEmployee != null) {
            {
                entityManager.remove(theEmployee);
            }
        }
    }
}
