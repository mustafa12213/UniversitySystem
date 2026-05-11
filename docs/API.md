# API Documentation

## Overview

This document provides comprehensive technical documentation for the UniSystem database and API structure. While UniSystem is currently a desktop application, this document outlines the data models and future REST API structure.

## Table of Contents

- [Data Models](#data-models)
- [Database Schema](#database-schema)
- [Current Architecture](#current-architecture)
- [Future REST API](#future-rest-api)

## Data Models

### User Model

Represents a user in the system with authentication credentials.

```csharp
public class User
{
	public int Id { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }  // Hashed with SHA-256
	public string Role { get; set; }      // "admin" or other roles
}
```

**Fields:**
- `Id`: Primary key (auto-incremented)
- `Username`: Unique username for login (required)
- `Password`: SHA-256 hashed password (required)
- `Role`: User role for access control (default: "admin")

**Validation:**
- Username: 3-50 characters, alphanumeric + underscore
- Password: Minimum 8 characters, hashed before storage

### Student Model

Represents a student in the university system.

```csharp
public class Student
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public string Phone { get; set; }
	public DateTime EnrollmentDate { get; set; }
}
```

**Fields:**
- `Id`: Primary key (auto-incremented)
- `Name`: Student's full name (required)
- `Email`: Student's email address (required)
- `Phone`: Student's phone number (optional)
- `EnrollmentDate`: Date of enrollment (auto-set to current date)

**Validation:**
- Name: 2-100 characters
- Email: Valid email format
- Phone: Optional, 10-15 digits
- EnrollmentDate: Cannot be in the future

### Course Model

Represents a course offered by the university.

```csharp
public class Course
{
	public int Id { get; set; }
	public string CourseCode { get; set; }
	public string CourseName { get; set; }
	public int Credits { get; set; }
}
```

**Fields:**
- `Id`: Primary key (auto-incremented)
- `CourseCode`: Unique course identifier (e.g., "CS-101")
- `CourseName`: Full name of the course (required)
- `Credits`: Number of credit hours (1-4)

**Validation:**
- CourseCode: 3-10 characters, alphanumeric + hyphen
- CourseName: 5-100 characters
- Credits: Integer between 1 and 4

### Enrollment Model

Represents student enrollment in a course.

```csharp
public class Enrollment
{
	public int Id { get; set; }
	public int StudentId { get; set; }
	public int CourseId { get; set; }
	public DateTime EnrollmentDate { get; set; }
}
```

**Fields:**
- `Id`: Primary key (auto-incremented)
- `StudentId`: Foreign key to Students table
- `CourseId`: Foreign key to Courses table
- `EnrollmentDate`: Date of enrollment in course

**Constraints:**
- StudentId and CourseId must exist
- A student cannot be enrolled in the same course twice

### Grade Model

Represents a student's grade in a course.

```csharp
public class Grade
{
	public int Id { get; set; }
	public int StudentId { get; set; }
	public int CourseId { get; set; }
	public decimal GradeValue { get; set; }
	public DateTime DateAssigned { get; set; }
}
```

**Fields:**
- `Id`: Primary key (auto-incremented)
- `StudentId`: Foreign key to Students table
- `CourseId`: Foreign key to Courses table
- `GradeValue`: Numeric grade (0-100 or 0.0-4.0)
- `DateAssigned`: When the grade was assigned

**Validation:**
- GradeValue: 0-100 for percentage scale
- DateAssigned: Cannot be in the future
- Must have valid StudentId and CourseId

## Database Schema

### Complete ERD (Entity Relationship Diagram)

```
┌─────────────────────────┐
│       Users             │
├─────────────────────────┤
│ Id (PK)                 │
│ Username (UNIQUE)       │
│ Password (hashed)       │
│ Role                    │
└─────────────────────────┘

┌─────────────────────────┐         ┌──────────────────────┐
│      Students           │────┐    │    Courses           │
├─────────────────────────┤    │    ├──────────────────────┤
│ Id (PK)                 │    │    │ Id (PK)              │
│ Name                    │    │    │ CourseCode (UNIQUE)  │
│ Email (UNIQUE)          │    └────│ CourseName           │
│ Phone                   │    ┌────│ Credits              │
│ EnrollmentDate          │    │    └──────────────────────┘
└─────────────────────────┘    │
							   │
					┌──────────────────────┐
					│   Enrollments        │
					├──────────────────────┤
					│ Id (PK)              │
					│ StudentId (FK)       │
					│ CourseId (FK)        │
					│ EnrollmentDate       │
					└──────────────────────┘
							   │
					┌──────────────────────┐
					│      Grades          │
					├──────────────────────┤
					│ Id (PK)              │
					│ StudentId (FK)       │
					│ CourseId (FK)        │
					│ GradeValue           │
					│ DateAssigned         │
					└──────────────────────┘
```

### SQL Table Definitions

```sql
-- Users Table
CREATE TABLE Users (
	Id INTEGER PRIMARY KEY AUTOINCREMENT,
	Username TEXT NOT NULL UNIQUE,
	Password TEXT NOT NULL,
	Role TEXT NOT NULL DEFAULT 'admin'
);

-- Students Table
CREATE TABLE Students (
	Id INTEGER PRIMARY KEY AUTOINCREMENT,
	Name TEXT NOT NULL,
	Email TEXT NOT NULL UNIQUE,
	Phone TEXT,
	EnrollmentDate TEXT NOT NULL DEFAULT CURRENT_DATE
);

-- Courses Table
CREATE TABLE Courses (
	Id INTEGER PRIMARY KEY AUTOINCREMENT,
	CourseCode TEXT NOT NULL UNIQUE,
	CourseName TEXT NOT NULL,
	Credits INTEGER NOT NULL CHECK (Credits BETWEEN 1 AND 4)
);

-- Enrollments Table
CREATE TABLE Enrollments (
	Id INTEGER PRIMARY KEY AUTOINCREMENT,
	StudentId INTEGER NOT NULL,
	CourseId INTEGER NOT NULL,
	EnrollmentDate TEXT NOT NULL DEFAULT CURRENT_DATE,
	FOREIGN KEY (StudentId) REFERENCES Students(Id),
	FOREIGN KEY (CourseId) REFERENCES Courses(Id),
	UNIQUE (StudentId, CourseId)
);

-- Grades Table
CREATE TABLE Grades (
	Id INTEGER PRIMARY KEY AUTOINCREMENT,
	StudentId INTEGER NOT NULL,
	CourseId INTEGER NOT NULL,
	GradeValue REAL NOT NULL CHECK (GradeValue BETWEEN 0 AND 100),
	DateAssigned TEXT NOT NULL DEFAULT CURRENT_DATE,
	FOREIGN KEY (StudentId) REFERENCES Students(Id),
	FOREIGN KEY (CourseId) REFERENCES Courses(Id)
);
```

## Current Architecture

### DatabaseHelper Class

Central data access layer for all database operations.

```csharp
public static class DatabaseHelper
{
	// Database Connection
	public static string ConnectionString { get; }

	// Initialization
	public static void Initialize();

	// User Methods
	public static bool AuthenticateUser(string username, string password);
	public static bool AddUser(string username, string password, string role);

	// Student Methods
	public static void AddStudent(string name, string email, string phone);
	public static DataTable GetAllStudents();
	public static void UpdateStudent(int id, string name, string email, string phone);
	public static void DeleteStudent(int id);

	// Course Methods
	public static void AddCourse(string code, string name, int credits);
	public static DataTable GetAllCourses();

	// Enrollment Methods
	public static void EnrollStudent(int studentId, int courseId);
	public static DataTable GetStudentCourses(int studentId);

	// Grade Methods
	public static void AssignGrade(int studentId, int courseId, decimal grade);
	public static DataTable GetStudentGrades(int studentId);
	public static DataTable GetCourseGrades(int courseId);
}
```

### Key Features

1. **Connection Management**
   - Uses `using` statements for automatic connection disposal
   - Supports connection pooling through SQLite

2. **Query Execution**
   - Parameterized queries to prevent SQL injection
   - Using SQLiteCommand for efficient query execution

3. **Error Handling**
   - Try-catch blocks for exception management
   - Logging of database errors

4. **Security**
   - SHA-256 password hashing
   - Parameterized queries
   - Role-based access control

## Future REST API

### Planned Endpoints

#### Authentication
```
POST /api/auth/login
- Request: { "username": "", "password": "" }
- Response: { "token": "", "userId": 0, "role": "" }

POST /api/auth/logout
- Response: { "message": "Logged out successfully" }
```

#### Students
```
GET /api/students
- Response: [ Student[] ]

GET /api/students/{id}
- Response: Student

POST /api/students
- Request: { "name": "", "email": "", "phone": "" }
- Response: Student

PUT /api/students/{id}
- Request: { "name": "", "email": "", "phone": "" }
- Response: Student

DELETE /api/students/{id}
- Response: { "message": "Student deleted" }
```

#### Courses
```
GET /api/courses
- Response: [ Course[] ]

GET /api/courses/{id}
- Response: Course

POST /api/courses
- Request: { "courseCode": "", "courseName": "", "credits": 0 }
- Response: Course

PUT /api/courses/{id}
- Request: { "courseCode": "", "courseName": "", "credits": 0 }
- Response: Course

DELETE /api/courses/{id}
- Response: { "message": "Course deleted" }
```

#### Grades
```
GET /api/students/{id}/grades
- Response: [ Grade[] ]

POST /api/grades
- Request: { "studentId": 0, "courseId": 0, "gradeValue": 0 }
- Response: Grade

PUT /api/grades/{id}
- Request: { "gradeValue": 0 }
- Response: Grade
```

### Authentication

Future API will use JWT (JSON Web Tokens):

```
Authorization: Bearer <token>
```

---

**Last Updated**: 2026-01-15
**Status**: Active Development
**Version**: 1.0.0
