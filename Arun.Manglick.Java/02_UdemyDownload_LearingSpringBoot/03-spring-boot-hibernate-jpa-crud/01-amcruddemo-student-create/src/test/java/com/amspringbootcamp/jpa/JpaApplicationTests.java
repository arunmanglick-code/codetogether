package com.amspringbootcamp.jpa;

import com.amspringbootcamp.jpa.dao.StudentDAO;
import com.amspringbootcamp.jpa.entity.Student;
import org.junit.jupiter.api.Test;
import org.mockito.Mockito;
import org.springframework.boot.test.context.SpringBootTest;

import java.util.Arrays;
import java.util.Collections;
import java.util.List;

@SpringBootTest
class JpaApplicationTests {

	@Test
	void contextLoads() {
	}

	@Test
	void createStudentReturnsIdAndSavesStudent() {
		StudentDAO mockDao = Mockito.mock(StudentDAO.class);
		Mockito.doAnswer(invocation -> {
			Student s = invocation.getArgument(0);
			s.setId(123);
			return null;
		}).when(mockDao).saveStudent(Mockito.any(Student.class));

		JpaApplication app = new JpaApplication();
		Integer id = app.createStudent(mockDao);

		org.assertj.core.api.Assertions.assertThat(id).isEqualTo(123);
	}

	@Test
	void readStudentPrintsDetailsIfFound() {
		StudentDAO mockDao = Mockito.mock(StudentDAO.class);
		Student student = new Student("Ruchi", "Manglick", "arun.manglick@vertexinc.com", 25, "Active");
		Mockito.when(mockDao.findStudentById(1)).thenReturn(student);

		JpaApplication app = new JpaApplication();
		app.readStudent(1, mockDao);
		Mockito.verify(mockDao).findStudentById(1);
	}

	@Test
	void readStudentHandlesNullStudent() {
		StudentDAO mockDao = Mockito.mock(StudentDAO.class);
		Mockito.when(mockDao.findStudentById(2)).thenReturn(null);

		JpaApplication app = new JpaApplication();
		app.readStudent(2, mockDao);
		Mockito.verify(mockDao).findStudentById(2);
	}

	@Test
	void queryStudentsReturnsMatchingStudents() {
		StudentDAO mockDao = Mockito.mock(StudentDAO.class);
		List<Student> students = Arrays.asList(
			new Student("Arun", "Manglick", "arun@x.com", 30, "Active"),
			new Student("Ruchi", "Manglick", "ruchi@x.com", 25, "Active")
		);
		Mockito.when(mockDao.getAllStudentsbyLastName(Mockito.anyString(), Mockito.anyString())).thenReturn(students);

		JpaApplication app = new JpaApplication();
		app.queryStudents(mockDao);
		Mockito.verify(mockDao).getAllStudentsbyLastName(Mockito.anyString(), Mockito.anyString());
	}

	@Test
	void queryStudentsHandlesEmptyList() {
		StudentDAO mockDao = Mockito.mock(StudentDAO.class);
		Mockito.when(mockDao.getAllStudentsbyLastName(Mockito.anyString(), Mockito.anyString())).thenReturn(Collections.emptyList());

		JpaApplication app = new JpaApplication();
		app.queryStudents(mockDao);
		Mockito.verify(mockDao).getAllStudentsbyLastName(Mockito.anyString(), Mockito.anyString());
	}
}
