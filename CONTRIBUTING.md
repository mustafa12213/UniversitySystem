# Contributing to UniSystem

Thank you for your interest in contributing to the UniSystem project! We welcome contributions from the community and are grateful for every pull request, bug report, and suggestion.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Workflow](#development-workflow)
- [Coding Standards](#coding-standards)
- [Commit Message Guidelines](#commit-message-guidelines)
- [Pull Request Process](#pull-request-process)
- [Reporting Bugs](#reporting-bugs)
- [Suggesting Enhancements](#suggesting-enhancements)

## Code of Conduct

### Our Pledge

We are committed to providing a welcoming and inspiring community for all. Please be respectful and constructive in all interactions.

### Expected Behavior

- Use welcoming and inclusive language
- Be respectful of differing opinions, viewpoints, and experiences
- Gracefully accept constructive criticism
- Focus on what is best for the community
- Show empathy towards other community members

### Unacceptable Behavior

- Harassment or discrimination of any kind
- Insulting/derogatory comments
- Personal or political attacks
- Public or private harassment
- Publishing others' private information
- Other conduct which could reasonably be considered inappropriate

## Getting Started

### Prerequisites

- Visual Studio 2019 or later
- .NET Framework 4.7.2 or higher
- Git installed on your system
- A GitHub account

### Development Setup

1. **Fork the repository**
   - Click the "Fork" button on the top right of the repository page
   - This creates a copy of the repository under your account

2. **Clone your fork**
   ```bash
   git clone https://github.com/YOUR-USERNAME/UniversitySystem.git
   cd UniversitySystem
   ```

3. **Add upstream remote**
   ```bash
   git remote add upstream https://github.com/mustafa12213/UniversitySystem.git
   ```

4. **Create a branch for your changes**
   ```bash
   git checkout -b feature/your-feature-name
   ```

5. **Build the project**
   ```bash
   # Restore NuGet packages
   nuget restore UniversitySystem.sln

   # Build in Visual Studio or using MSBuild
   msbuild UniversitySystem.sln
   ```

## Development Workflow

### Feature Development

1. **Create a feature branch**
   ```bash
   git checkout -b feature/descriptive-name
   ```
   Branch naming conventions:
   - `feature/` - for new features
   - `bugfix/` - for bug fixes
   - `docs/` - for documentation updates
   - `refactor/` - for code refactoring
   - `test/` - for test additions

2. **Make your changes**
   - Write clean, readable code
   - Follow the coding standards (see below)
   - Add comments for complex logic
   - Test your changes thoroughly

3. **Keep your branch updated**
   ```bash
   git fetch upstream
   git rebase upstream/main
   ```

4. **Push to your fork**
   ```bash
   git push origin feature/descriptive-name
   ```

## Coding Standards

### C# Code Style

- **Naming Conventions**
  - Classes: PascalCase (e.g., `LoginForm`)
  - Methods: PascalCase (e.g., `InitializeComponents`)
  - Variables: camelCase (e.g., `userName`)
  - Constants: UPPER_SNAKE_CASE (e.g., `MAX_LOGIN_ATTEMPTS`)
  - Private fields: _camelCase (e.g., `_loginForm`)

- **Formatting**
  - Use 4 spaces for indentation (not tabs)
  - Open braces on the same line: `if (condition) {`
  - Place closing braces on separate lines
  - Use meaningful variable and method names

- **Best Practices**
  - Use `using` statements for resource management
  - Avoid deeply nested conditions (max 3 levels)
  - Keep methods focused on a single responsibility
  - Add XML comments to public methods
  - Use LINQ where appropriate
  - Avoid magic numbers; use named constants

### Example Code Style

```csharp
/// <summary>
/// Authenticates a user with the provided credentials.
/// </summary>
/// <param name="username">The user's username</param>
/// <param name="password">The user's password</param>
/// <returns>true if authentication succeeds; otherwise false</returns>
public static bool AuthenticateUser(string username, string password)
{
	if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
	{
		return false;
	}

	try
	{
		using (var connection = new SQLiteConnection(ConnectionString))
		{
			connection.Open();
			var command = new SQLiteCommand(
				"SELECT Password FROM Users WHERE Username = @username",
				connection);
			command.Parameters.AddWithValue("@username", username);

			var result = command.ExecuteScalar();
			if (result == null)
			{
				return false;
			}

			string storedHash = result.ToString();
			string inputHash = HashPassword(password);

			return storedHash == inputHash;
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Authentication error: {ex.Message}");
		return false;
	}
}
```

## Commit Message Guidelines

Write clear and descriptive commit messages following this format:

```
<type>: <subject>

<body>

<footer>
```

### Type

- `feat`: A new feature
- `fix`: A bug fix
- `docs`: Documentation only changes
- `style`: Changes that don't affect code meaning (formatting, etc.)
- `refactor`: Code change that neither fixes a bug nor adds a feature
- `perf`: Code change that improves performance
- `test`: Adding or updating tests
- `chore`: Changes to build process, dependencies, etc.

### Subject

- Use imperative, present tense: "add" not "added" or "adds"
- Don't capitalize first letter
- No period at the end
- Maximum 50 characters

### Body

- Optional but recommended for non-trivial changes
- Explain what and why, not how
- Wrap at 72 characters
- Separate from subject with a blank line

### Footer

- Reference issues with "Fixes #123" or "Relates to #456"

### Examples

```
feat: add grade calculation functionality

Implement automated grade calculation based on assignments and exams.
This feature calculates the final grade using weighted averages.

Fixes #45
```

```
fix: prevent database from being recreated on startup

Check if database file exists before creating a new one to preserve
existing data between sessions.

Fixes #12
```

## Pull Request Process

1. **Before Submitting**
   - Ensure your code builds without errors
   - Test your changes thoroughly
   - Update documentation if needed
   - Rebase on the latest main branch

2. **Create a Pull Request**
   - Go to the original repository
   - Click "New Pull Request"
   - Select your fork and branch
   - Fill in the PR template with:
	 - Clear title describing the change
	 - Description of what was changed and why
	 - Reference to any related issues
	 - Screenshots (if UI changes)

3. **PR Description Template**
   ```markdown
   ## Description
   Brief description of your changes.

   ## Type of Change
   - [ ] Bug fix
   - [ ] New feature
   - [ ] Breaking change
   - [ ] Documentation update

   ## Related Issues
   Fixes #(issue number)

   ## How Has This Been Tested?
   Describe the tests you ran and how to reproduce them.

   ## Screenshots (if applicable)
   Add screenshots for UI changes.

   ## Checklist
   - [ ] My code follows the style guidelines
   - [ ] I have performed a self-review
   - [ ] I have commented complex logic
   - [ ] I have updated documentation
   - [ ] My changes generate no new warnings
   - [ ] I have added tests where appropriate
   ```

4. **After Submission**
   - Respond to review comments promptly
   - Make requested changes in new commits
   - Request re-review when changes are complete
   - Keep conversations professional and constructive

## Reporting Bugs

### Before Submitting a Bug Report

- Check existing issues to avoid duplicates
- Collect relevant information:
  - Operating system and version
  - .NET Framework version
  - Exact steps to reproduce
  - Expected vs. actual behavior
  - Error messages or logs
  - Screenshots if applicable

### Submitting a Bug Report

1. Go to the [Issues](https://github.com/mustafa12213/UniversitySystem/issues) page
2. Click "New Issue"
3. Use the bug report template:

```markdown
## Description
Brief description of the bug.

## Steps to Reproduce
1. Step one
2. Step two
3. Step three

## Expected Behavior
What should happen.

## Actual Behavior
What actually happens.

## Environment
- OS: [e.g., Windows 10]
- .NET Framework: [e.g., 4.7.2]
- Visual Studio: [e.g., 2019]

## Logs/Screenshots
Add any relevant logs, error messages, or screenshots.
```

## Suggesting Enhancements

1. Go to the [Issues](https://github.com/mustafa12213/UniversitySystem/issues) page
2. Click "New Issue"
3. Use the feature request template:

```markdown
## Description
Clear description of the enhancement.

## Motivation
Why this enhancement is needed.

## Proposed Solution
How you envision this working.

## Alternatives Considered
Other approaches considered.

## Additional Context
Any other relevant information.
```

## Questions?

- Open a [Discussion](https://github.com/mustafa12213/UniversitySystem/discussions)
- Check existing documentation in the `/docs` folder
- Email: (contact information if available)

## Recognition

Contributors will be recognized in:
- The project's CONTRIBUTORS file
- Release notes
- GitHub contributors page

Thank you for contributing to UniSystem! 🎉
