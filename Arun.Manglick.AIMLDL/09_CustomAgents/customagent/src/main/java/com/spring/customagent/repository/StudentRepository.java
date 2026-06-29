/*
 * Author: Arun Manglick
 * Created: 2026-06-29
 * Updated: 2026-06-29
 */
package com.spring.customagent.repository;

import com.spring.customagent.entity.Student;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

/**
 * Spring Data JPA repository for the {@link Student} entity.
 *
 * <p><strong>NOTE:</strong> This repository is <em>NOT</em> part of the active code path.
 * The application uses a manual DAO pattern via
 * {@link com.spring.customagent.dao.StudentDAOImpl} backed by JPA {@code EntityManager}.
 * This interface is retained for reference/demonstration purposes only and should not
 * be injected into production service classes.</p>
 *
 * @deprecated Not wired into the active service chain. Use
 *             {@link com.spring.customagent.dao.StudentDAO} instead.
 */
@Deprecated
public interface StudentRepository extends JpaRepository<Student, Long> {
    List<Student> findByFirstname(String firstname);
}
