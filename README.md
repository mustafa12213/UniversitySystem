# UniSystem - University Management System

A comprehensive desktop application for managing university operations, built with C# and Windows Forms. This system provides an intuitive interface for managing students, courses, grades, and user authentication with a SQLite database backend.

## Table of Contents

- [Features](#features)
- [System Requirements](#system-requirements)
- [Installation](#installation)
- [Usage](#usage)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Database](#database)
- [Technologies Used](#technologies-used)
- [Bug Fixes & Improvements](#bug-fixes--improvements)
- [Contributing](#contributing)
- [License](#license)

## Features

- **User Authentication**: Secure login system with role-based access control
- **Dashboard**: Comprehensive overview of system statistics and key information
- **Student Management**: Add, view, update, and manage student records
- **Course Management**: Create and manage courses offered by the university
- **Grade Management**: Track and manage student grades and academic performance
- **Role-Based Access**: Different permission levels for admin and staff users
- **Data Persistence**: SQLite database for reliable data storage
- **Responsive UI**: Modern Windows Forms interface with professional design

## System Requirements

- **Operating System**: Windows 7 or later
- **.NET Framework**: 4.7.2 or higher
- **RAM**: Minimum 512 MB
- **Disk Space**: Minimum 50 MB for installation

## Installation

### Prerequisites

Before you begin, ensure you have the following installed:
- [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework/net472) or higher
- Visual Studio 2019 or later (for development), or just the .NET Framework (for running)

### Steps

1. **Clone the Repository**
   ```bash
   git clone https://github.com/mustafa12213/UniversitySystem.git
   cd UniversitySystem
   ```

2. **Restore NuGet Packages**
   ```bash
   nuget restore UniversitySystem.sln
   ```
   Or using Visual Studio: `Tools > NuGet Package Manager > Package Manager Console > Update-Package`

3. **Build the Project**
   - Open `UniversitySystem.sln` in Visual Studio
   - Build the solution (Ctrl+Shift+B)
   - The executable will be generated in `bin/Debug/` or `bin/Release/`

4. **Run the Application**
   - Press `F5` to run with debugging, or
   - Run `UniversitySystem.exe` directly from the output folder

## Usage

### Initial Login

On first launch, the application automatically creates a SQLite database and seeds demo data with the following test credentials:

- **Username**: `admin`
- **Password**: `admin123`

### Main Interface

1. **Login Form**: Authenticate using your credentials
2. **Dashboard**: View system statistics and key information
3. **Navigation Sidebar**: Access different modules:
   - Students
   - Courses
   - Grades
   - User Management

### Common Tasks

#### Managing Students
- Navigate to the **Students** section
- Click "Add Student" to create a new student record
- View, edit, or delete existing student records

#### Managing Courses
- Access the **Courses** module
- Add new courses with course code, name, and credits
- View enrolled students for each course

#### Managing Grades
- Open the **Grades** section
- Assign grades to students for specific courses
- Track academic performance

## Architecture

The application follows a layered architecture:

```
┌─────────────────────────────────────┐
│     Presentation Layer              │
│  (Windows Forms UI Components)      │
├─────────────────────────────────────┤
│     Business Logic Layer            │
│  (Form Logic & Data Validation)     │
├─────────────────────────────────────┤
│     Data Access Layer               │
│  (DatabaseHelper with SQLite)       │
├─────────────────────────────────────┤
│     Data Storage Layer              │
│  (SQLite Database File)             │
└─────────────────────────────────────┘
```

### Key Components

- **LoginForm.cs**: Handles user authentication
- **MainForm.cs**: Main application window with navigation
- **Database.cs**: DatabaseHelper class managing all database operations
- **Program.cs**: Application entry point

## Project Structure

```
UniversitySystem/
├── UniversitySystem.sln           # Solution file
├── UniversitySystem.csproj        # Project file
├── Program.cs                     # Application entry point
├── LoginForm.cs                   # Login interface
├── MainForm.cs                    # Main application window
├── Database.cs                    # Database helper & SQL operations
├── App.config                     # Application configuration
├── packages.config                # NuGet dependencies
├── Properties/
│   ├── AssemblyInfo.cs           # Assembly metadata
│   ├── Resources.Designer.cs     # Resource management
│   └── Settings.Designer.cs      # Application settings
├── packages/                      # NuGet packages
└── bin/                          # Compiled output
	├── Debug/
	└── Release/
```

## Database

### SQLite Database

The application uses SQLite for data persistence. The database file (`university.db`) is automatically created in the application's working directory on first run.

### Database Schema

The database includes the following tables:

- **Users**: User accounts and authentication
  - Id, Username, Password, Role

- **Students**: Student information
  - Id, Name, Email, Phone, EnrollmentDate

- **Courses**: Course information
  - Id, CourseCode, CourseName, Credits

- **Enrollments**: Student-Course relationships
  - Id, StudentId, CourseId, EnrollmentDate

- **Grades**: Student grades for courses
  - Id, StudentId, CourseId, Grade, DateAssigned

### Key Features

- **Automatic Initialization**: Database schema is created automatically on first run
- **Demo Data**: Sample data is seeded on initial setup for testing
- **Password Security**: User passwords are hashed using SHA-256 encryption
- **Data Persistence**: All changes are immediately persisted to disk

## Technologies Used

- **Language**: C# (.NET Framework 4.7.2)
- **UI Framework**: Windows Forms
- **Database**: SQLite 3
- **Build Tool**: MSBuild
- **IDE**: Visual Studio 2019+

### Dependencies

- **System.Data.SQLite**: SQLite database provider for .NET
  - Version: 1.0.119.0
  - Used for database connectivity and operations

## Bug Fixes & Improvements

This project includes several important bug fixes and improvements:

### Fix #1: Database Persistence
- **Issue**: Database was being recreated on every application launch, losing all data
- **Solution**: Modified `DatabaseHelper.Initialize()` to only create the database if it doesn't exist
- **Impact**: Data now persists correctly between sessions

### Fix #2: Resource Management
- **Issue**: Forms were not properly disposed, causing memory leaks
- **Solution**: Implemented proper disposal patterns and form cleanup
- **Impact**: Improved memory efficiency and application stability

### Fix #3: UI Responsiveness
- **Issue**: Database operations on UI thread causing interface freezing
- **Solution**: Optimized database queries and implemented async patterns where applicable
- **Impact**: Smoother user experience

### Fix #4: Authentication
- **Issue**: Password storage in plain text
- **Solution**: Implemented SHA-256 password hashing
- **Impact**: Enhanced security

### Fix #5: Form Lifecycle
- **Issue**: Forms not properly released when closing
- **Solution**: Proper form closure and reference management
- **Impact**: Eliminated window leaks and improved resource cleanup

## Contributing

Contributions are welcome! To contribute:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/YourFeature`)
3. Commit your changes (`git commit -m 'Add YourFeature'`)
4. Push to the branch (`git push origin feature/YourFeature`)
5. Open a Pull Request

Please ensure your code follows the existing style and includes appropriate comments.

## Future Enhancements

Potential improvements for future versions:

- [ ] Migrate to .NET 6+ or .NET Framework Core
- [ ] Implement unit testing with xUnit or NUnit
- [ ] Add email notifications for grade updates
- [ ] Implement role-based permission system
- [ ] Add advanced reporting and analytics
- [ ] Implement data export (PDF, Excel)
- [ ] Multi-language support
- [ ] Cloud database integration

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contact & Support

For questions, issues, or suggestions:

- **GitHub Issues**: [Report an issue](https://github.com/mustafa12213/UniversitySystem/issues)
- **GitHub Discussions**: [Start a discussion](https://github.com/mustafa12213/UniversitySystem/discussions)

---

**Last Updated**: 2026
**Version**: 1.0.0
**Status**: Active Development
