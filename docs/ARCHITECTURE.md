# Architecture Documentation

## System Architecture

UniSystem follows a **layered architecture** pattern with clear separation of concerns.

## Architecture Diagram

```
┌────────────────────────────────────────────────────┐
│           Presentation Layer                       │
│  ┌─────────────────────────────────────────────┐  │
│  │  Windows Forms UI Components                │  │
│  │  • LoginForm.cs                             │  │
│  │  • MainForm.cs                              │  │
│  │  • UI Event Handlers                        │  │
│  └─────────────────────────────────────────────┘  │
└────────────────────────────────────────────────────┘
					   │
					   ▼
┌────────────────────────────────────────────────────┐
│        Business Logic Layer                        │
│  ┌─────────────────────────────────────────────┐  │
│  │  Form Logic & Data Validation               │  │
│  │  • Input validation                         │  │
│  │  • Business rules enforcement               │  │
│  │  • Data transformation                      │  │
│  └─────────────────────────────────────────────┘  │
└────────────────────────────────────────────────────┘
					   │
					   ▼
┌────────────────────────────────────────────────────┐
│        Data Access Layer                           │
│  ┌─────────────────────────────────────────────┐  │
│  │  DatabaseHelper (Database.cs)               │  │
│  │  • SQL Query Construction                   │  │
│  │  • Connection Management                    │  │
│  │  • Data Command Execution                   │  │
│  └─────────────────────────────────────────────┘  │
└────────────────────────────────────────────────────┘
					   │
					   ▼
┌────────────────────────────────────────────────────┐
│        Data Storage Layer                          │
│  ┌─────────────────────────────────────────────┐  │
│  │  SQLite Database File (university.db)       │  │
│  │  • Persistent Data Storage                  │  │
│  │  • ACID Compliance                          │  │
│  └─────────────────────────────────────────────┘  │
└────────────────────────────────────────────────────┘
```

## Layer Responsibilities

### 1. Presentation Layer

**Purpose**: Handle user interface and user interactions

**Components**:
- **LoginForm.cs**: User login interface
  - Username/password input
  - Authentication logic
  - Error message display

- **MainForm.cs**: Main application window
  - Navigation sidebar
  - Content panel
  - Dashboard and module views
  - User interactions handling

**Responsibilities**:
- Display information to users
- Collect user input
- Validate user input at UI level
- Call business logic for processing
- Display results to users

**Key Methods**:
```csharp
private void InitializeComponents()      // UI Setup
private void ShowDashboard()             // Display dashboard
private void OnLoginButtonClick()        // Handle login
```

### 2. Business Logic Layer

**Purpose**: Enforce business rules and data validation

**Components**:
- Input validation for all user entries
- Business rule enforcement
- Data transformation
- Error handling and messaging

**Responsibilities**:
- Validate student information
- Validate course data
- Enforce enrollment constraints
- Check grade ranges
- Apply business rules

**Key Rules**:
```csharp
// Example: Student name validation
if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
{
	throw new ArgumentException("Invalid student name");
}

// Example: Grade validation
if (grade < 0 || grade > 100)
{
	throw new ArgumentException("Grade must be between 0 and 100");
}
```

### 3. Data Access Layer

**Purpose**: Manage database connectivity and operations

**Components**:
- **DatabaseHelper.cs**: Central database helper
  - Connection string management
  - SQL query construction
  - Parameter handling
  - Result mapping

**Responsibilities**:
- Execute database queries
- Manage connections
- Handle parameterized queries (SQL injection prevention)
- Transform SQL results to usable data
- Error handling at database level

**Key Methods**:
```csharp
public static void AddStudent(string name, string email, string phone)
public static DataTable GetAllStudents()
public static void AssignGrade(int studentId, int courseId, decimal grade)
```

### 4. Data Storage Layer

**Purpose**: Persistent storage of application data

**Components**:
- **university.db**: SQLite database file
- Tables: Users, Students, Courses, Enrollments, Grades
- Schema constraints and relationships

**Characteristics**:
- File-based database (portable)
- ACID compliant
- Supports transactions
- Auto-increment primary keys
- Foreign key constraints

## Data Flow

### Example: Login Process

```
User Input (UI)
	  │
	  ▼
LoginForm.OnLoginClick()
	  │
	  ▼
Input Validation (Business Logic)
	  │
	  ▼
DatabaseHelper.AuthenticateUser(username, password)
	  │
	  ▼
SQLite Query Execution
	  │
	  ▼
Password Hash Comparison
	  │
	  ▼
Return Result to LoginForm
	  │
	  ▼
Display Result to User
```

### Example: Add Student

```
User Input (MainForm)
	  │
	  ▼
Input Validation (Business Logic)
	  │
	  ▼
DatabaseHelper.AddStudent()
	  │
	  ▼
SQL INSERT Query Construction
	  │
	  ▼
SQLite Execution
	  │
	  ▼
university.db Updated
	  │
	  ▼
Result Returned to MainForm
	  │
	  ▼
Refresh UI (Students List)
```

## Design Patterns

### 1. Static Factory Pattern (DatabaseHelper)

```csharp
public static class DatabaseHelper
{
	// Private constructor prevents instantiation
	// All methods are static for direct access
	public static void Initialize() { }
}
```

**Benefits**:
- Centralized data access
- Easy to mock for testing
- No instance overhead

### 2. Using Statement Pattern

```csharp
using (var connection = new SQLiteConnection(ConnectionString))
{
	connection.Open();
	// Database operations
} // Connection automatically closed and disposed
```

**Benefits**:
- Automatic resource cleanup
- Prevents connection leaks
- Exception-safe

### 3. Parameterized Queries

```csharp
var command = new SQLiteCommand(
	"SELECT * FROM Students WHERE Email = @email", 
	connection);
command.Parameters.AddWithValue("@email", email);
```

**Benefits**:
- Prevents SQL injection
- Improves performance (query caching)
- Cleaner code

## Component Interactions

### Component Diagram

```
┌──────────────────────────┐
│   Program.cs             │
│   (Entry Point)          │
└────────────┬─────────────┘
			 │
			 ▼
┌──────────────────────────┐
│   LoginForm.cs           │
│   (Authentication)       │
└────────────┬─────────────┘
			 │
			 ├──────────────────────────────────────┐
			 │                                      │
			 ▼                                      ▼
┌──────────────────────────┐      ┌───────────────────────────┐
│   MainForm.cs            │      │   DatabaseHelper.cs       │
│   (Main Application)     │      │   (Data Access)           │
└──────────────────────────┘      └───────────────┬───────────┘
			 │                                    │
			 │                    ┌───────────────┴────────┐
			 │                    │                        │
			 │                    ▼                        ▼
			 │          ┌──────────────────────┐  ┌──────────────────┐
			 │          │  SQLiteConnection    │  │  SQLiteCommand   │
			 │          │  (Connection Mgmt)   │  │  (Query Exec)    │
			 │          └──────────────────────┘  └──────────────────┘
			 │                    │                        │
			 │                    └────────────┬───────────┘
			 │                                 │
			 ▼                                 ▼
		┌────────────────────────────────────────────┐
		│   university.db (SQLite Database)          │
		│   • Users                                  │
		│   • Students                               │
		│   • Courses                                │
		│   • Enrollments                            │
		│   • Grades                                 │
		└────────────────────────────────────────────┘
```

## Security Architecture

### 1. Authentication

```
User Credentials
	  │
	  ▼
SHA-256 Hash
	  │
	  ▼
Compare with Stored Hash
	  │
	  ▼
Grant/Deny Access
```

### 2. Data Validation

```
User Input
	  │
	  ▼
UI-Level Validation
	  │
	  ▼
Business Logic Validation
	  │
	  ▼
Database Constraints
	  │
	  ▼
Safe Storage
```

### 3. SQL Injection Prevention

```
User Input
	  │
	  ▼
Parameterized Query (@parameter)
	  │
	  ▼
SQLite Parameter Binding
	  │
	  ▼
Safe Execution
```

## Scalability Considerations

### Current Limitations
- **Single-user desktop application** (no concurrent access)
- SQLite suitable for **small to medium datasets**
- All operations on **UI thread** (potential freezing)

### Future Improvements
- **Multi-threaded operations** for background tasks
- **Migration to SQL Server or PostgreSQL** for scalability
- **REST API** for multi-user support
- **Async/await patterns** for better responsiveness
- **Caching** for frequently accessed data

## Error Handling Strategy

### Error Flow

```
Database Operation
	  │
	  ├─ Success ──► Return Result
	  │
	  └─ Exception ──► Catch Exception
							│
							├─ Log Error
							│
							├─ Display User-Friendly Message
							│
							└─ Return Error Code
```

### Error Categories

1. **Validation Errors**: Input doesn't meet requirements
2. **Database Errors**: Connection, query execution
3. **Business Logic Errors**: Rules violated
4. **System Errors**: Unexpected failures

## Performance Optimization

### Current Optimizations
- **Connection pooling** (SQLite native)
- **Parameterized queries** (prevent full table scans)
- **Indexed primary keys** (faster lookups)
- **Using statements** (efficient resource usage)

### Future Optimizations
- **Database indexing** on frequently searched fields
- **Query optimization** (reduce N+1 queries)
- **Caching** of frequently accessed data
- **Async database operations** (prevent UI blocking)
- **Batch operations** (multiple inserts/updates)

---

**Last Updated**: 2026-01-15
**Status**: Active Development
**Version**: 1.0.0
