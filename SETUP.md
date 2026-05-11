# Development Setup Guide

This guide provides detailed instructions for setting up the UniSystem project for local development.

## Table of Contents

- [System Requirements](#system-requirements)
- [Prerequisites](#prerequisites)
- [Installation Steps](#installation-steps)
- [Project Structure](#project-structure)
- [Running the Application](#running-the-application)
- [Database Setup](#database-setup)
- [Testing the Application](#testing-the-application)
- [Troubleshooting](#troubleshooting)
- [Development Tips](#development-tips)

## System Requirements

### Minimum Requirements
- **Operating System**: Windows 7 SP1 or later
- **Processor**: 1 GHz or equivalent
- **RAM**: 512 MB
- **Disk Space**: 100 MB for installation + 50 MB for development tools

### Recommended Requirements
- **Operating System**: Windows 10 or Windows 11
- **Processor**: Intel Core i5 or equivalent
- **RAM**: 8 GB or more
- **Disk Space**: 200 MB for installation + development environment
- **Display**: 1920x1080 resolution

## Prerequisites

### Required Software

1. **.NET Framework 4.7.2 or higher**
   - Download: https://dotnet.microsoft.com/download/dotnet-framework/net472
   - Installation: Follow Microsoft's installation wizard
   - Verification:
	 ```bash
	 reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client" /v Version
	 ```

2. **Visual Studio 2019 or later**
   - Download: https://visualstudio.microsoft.com/downloads/
   - Workloads to install:
	 - .NET desktop development
	 - Desktop development with C++
   - Extensions (Optional):
	 - ReSharper (for code analysis)
	 - SQLite Toolbox (for database management)

3. **Git**
   - Download: https://git-scm.com/download/win
   - Verify installation:
	 ```bash
	 git --version
	 ```

4. **NuGet Package Manager**
   - Usually included with Visual Studio
   - Can also be downloaded from: https://www.nuget.org/downloads

### Optional Tools

- **Visual Studio Code**: Lightweight code editor
- **DB Browser for SQLite**: For database inspection
- **Postman**: For API testing (future REST API)
- **GitHub Desktop**: GUI for Git operations

## Installation Steps

### Step 1: Clone the Repository

Open PowerShell or Command Prompt and run:

```bash
cd C:\Users\YourUsername\Documents
git clone https://github.com/mustafa12213/UniversitySystem.git
cd UniversitySystem
```

Or, if you're contributing, clone your fork:

```bash
git clone https://github.com/YOUR-USERNAME/UniversitySystem.git
cd UniversitySystem
```

### Step 2: Restore NuGet Packages

#### Option A: Using Visual Studio
1. Open `UniversitySystem.sln` in Visual Studio
2. Go to `Tools > NuGet Package Manager > Package Manager Console`
3. Run: `Update-Package -Reinstall`

#### Option B: Using Command Line
```bash
nuget restore UniversitySystem.sln
```

#### Option C: Using .NET CLI (if using modern .NET)
```bash
dotnet restore
```

### Step 3: Verify NuGet Packages

Check that the following package is installed:
- **System.Data.SQLite**: Version 1.0.119.0 or compatible

```bash
# Check packages.config
type packages.config
```

### Step 4: Open in Visual Studio

1. Launch Visual Studio
2. Open File > Open > Project/Solution
3. Navigate to the `UniversitySystem.sln` file
4. Visual Studio will load the solution and display the project structure

### Step 5: Build the Solution

```bash
# In Visual Studio: Ctrl+Shift+B
# Or from command line:
msbuild UniversitySystem.sln /p:Configuration=Debug
```

If build is successful, you should see:
```
Build succeeded. (0 warnings)
```

### Step 6: Verify the Build

Check that the executable was created:
```bash
# Check for output files
dir bin\Debug\
```

You should see:
- `UniversitySystem.exe`
- `System.Data.SQLite.dll`
- `App.config`

## Project Structure

```
UniversitySystem/
├── UniversitySystem.sln              # Solution file
├── UniversitySystem.csproj           # Project file
├── bin/
│   ├── Debug/                        # Debug build output
│   │   ├── UniversitySystem.exe
│   │   ├── university.db             # SQLite database (created on first run)
│   │   └── *.dll                     # Dependencies
│   └── Release/                      # Release build output
├── obj/                              # Intermediate build files
├── Properties/
│   ├── AssemblyInfo.cs
│   ├── Resources.Designer.cs
│   └── Settings.Designer.cs
├── packages/                         # NuGet packages
│   └── System.Data.SQLite.Core...
├── Program.cs                        # Entry point
├── LoginForm.cs                      # Login UI
├── MainForm.cs                       # Main application window
├── Database.cs                       # Database helper class
├── App.config                        # Application configuration
├── packages.config                   # NuGet package list
├── README.md                         # Project documentation
├── CONTRIBUTING.md                   # Contribution guidelines
├── CHANGELOG.md                      # Version history
├── LICENSE                           # MIT License
└── SETUP.md                          # This file
```

## Running the Application

### Method 1: From Visual Studio

1. Press `F5` (Run with Debugging) or `Ctrl+F5` (Run without Debugging)
2. The application will start
3. Login with default credentials:
   - Username: `admin`
   - Password: `admin123`

### Method 2: From Command Line

```bash
cd bin\Debug\
UniversitySystem.exe
```

### Method 3: From File Explorer

1. Navigate to `UniversitySystem\bin\Debug\`
2. Double-click `UniversitySystem.exe`

### First Run

On first launch:
- The application creates `university.db` in the output directory
- The database schema is initialized
- Demo data is seeded into the database
- You'll see the login form

## Database Setup

### Automatic Setup

The application handles database setup automatically:

1. **Check if database exists**
   - If `university.db` doesn't exist, create it

2. **Create schema**
   - Creates tables: Users, Students, Courses, Enrollments, Grades

3. **Seed demo data**
   - Creates admin user
   - Adds sample students
   - Adds sample courses
   - Creates sample enrollments and grades

### Manual Database Inspection

To view the database directly:

1. **Using Visual Studio**
   - Install "SQLite Toolbox" extension
   - Right-click on the project > New > SQLite Connection
   - Browse to `university.db`

2. **Using DB Browser for SQLite**
   - Download: https://sqlitebrowser.org/
   - Open `university.db`
   - View tables and data

### Database Location

The database file (`university.db`) is stored in:
```
UniversitySystem\bin\Debug\university.db
```

Or after building Release:
```
UniversitySystem\bin\Release\university.db
```

## Testing the Application

### Manual Testing Checklist

- [ ] Application starts without errors
- [ ] Login form displays correctly
- [ ] Default credentials work (admin/admin123)
- [ ] Dashboard loads after login
- [ ] Navigation sidebar works
- [ ] Student management functions
- [ ] Course management functions
- [ ] Grade management functions
- [ ] Logout returns to login form

### Test Credentials

| Username | Password | Role |
|----------|----------|------|
| admin    | admin123 | Admin |

### Testing Workflows

#### 1. Add a Student
1. Click "Students" in sidebar
2. Click "Add Student"
3. Fill in student details
4. Click "Save"
5. Verify student appears in list

#### 2. Add a Course
1. Click "Courses" in sidebar
2. Click "Add Course"
3. Fill in course details
4. Click "Save"
5. Verify course appears in list

#### 3. Assign Grades
1. Click "Grades" in sidebar
2. Select student and course
3. Enter grade
4. Click "Save"
5. Verify grade is stored

#### 4. Logout
1. Click "Logout" button
2. Should return to login form

## Troubleshooting

### Issue: "Could not load file or assembly System.Data.SQLite"

**Solution:**
1. Restore NuGet packages:
   ```bash
   nuget restore UniversitySystem.sln
   ```
2. Clean and rebuild:
   - Visual Studio: Build > Clean Solution
   - Visual Studio: Build > Rebuild Solution

### Issue: ".NET Framework 4.7.2 is not installed"

**Solution:**
1. Download .NET Framework 4.7.2: https://dotnet.microsoft.com/download/dotnet-framework/net472
2. Run the installer
3. Restart your computer
4. Rebuild the solution

### Issue: "Project file is missing"

**Solution:**
1. Ensure you cloned the entire repository:
   ```bash
   git clone https://github.com/mustafa12213/UniversitySystem.git
   ```
2. Check that all files are present (see Project Structure section)
3. If files are missing, clone again

### Issue: "Database is locked"

**Solution:**
1. Close the application completely
2. Delete `university.db` from `bin\Debug\` or `bin\Release\`
3. Rebuild and run the application
4. The database will be recreated

### Issue: "Cannot connect to database"

**Solution:**
1. Check that the database file exists in the correct location
2. Verify file permissions (the app needs read/write access)
3. Check that no other process is using the database
4. Try deleting the database and recreating it:
   ```bash
   # Delete the database file
   del bin\Debug\university.db
   # Run the app to recreate it
   ```

### Issue: Login fails with correct credentials

**Solution:**
1. Delete the database: `del bin\Debug\university.db`
2. Rebuild the project
3. Run the application (this will recreate the database with demo data)
4. Try login again

## Development Tips

### Debugging

1. **Set Breakpoints**
   - Click in the left margin of code to set breakpoint
   - Run with F5
   - Execution will pause at breakpoint

2. **Use Debug Windows**
   - Debug > Windows > Locals: View local variables
   - Debug > Windows > Watch: Monitor specific variables
   - Debug > Windows > Immediate: Execute code at runtime

3. **Common Debug Shortcuts**
   - F10: Step over
   - F11: Step into
   - Shift+F11: Step out
   - Ctrl+Shift+F5: Restart debugging

### Code Analysis

- **Visual Studio Code Analysis**
  - Tools > Options > Text Editor > C# > Code Style > Analysis
  - Enable "Run background code analysis"

- **ReSharper** (if installed)
  - Provides advanced code inspections
  - Suggests refactorings and optimizations

### Useful Visual Studio Shortcuts

| Shortcut | Action |
|----------|--------|
| Ctrl+Shift+B | Build Solution |
| F5 | Start Debugging |
| Ctrl+F5 | Start without Debugging |
| F7 | View Code |
| Shift+F7 | View Designer |
| Ctrl+- | Go to Previous Location |
| Ctrl+Shift+- | Go to Next Location |
| F12 | Go to Definition |
| Ctrl+F | Find in File |
| Ctrl+H | Find and Replace |
| Ctrl+K, Ctrl+C | Comment Selection |
| Ctrl+K, Ctrl+U | Uncomment Selection |

### Database Queries

To execute custom SQL queries:

1. Open Database.cs
2. Add a new public static method
3. Use SQLiteConnection and SQLiteCommand
4. Call the method from your form

Example:
```csharp
public static int GetStudentCount()
{
	using (var conn = new SQLiteConnection(ConnectionString))
	{
		conn.Open();
		var command = new SQLiteCommand("SELECT COUNT(*) FROM Students", conn);
		return (int)command.ExecuteScalar();
	}
}
```

## Getting Help

If you encounter issues:

1. Check the [Troubleshooting](#troubleshooting) section above
2. Search existing [GitHub Issues](https://github.com/mustafa12213/UniversitySystem/issues)
3. Check the [README.md](README.md) for general information
4. Read [CONTRIBUTING.md](CONTRIBUTING.md) for contribution guidelines
5. Open a new issue with detailed information about your problem

---

**Last Updated**: 2026-01-15
**Status**: Active Development
**Maintainer**: Mustafa
