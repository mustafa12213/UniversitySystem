using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace UniversitySystem
{
    /// <summary>
    /// Central database helper — wraps SQLite.
    /// The .db file lives next to the .exe in the output folder.
    /// </summary>
    public static class DatabaseHelper
    {
        private static readonly string DbPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "university.db");

        public static string ConnectionString =>
            $"Data Source={DbPath};Version=3;";

        // ── Initialise ─────────────────────────────────────────────
        public static void Initialize()
        {
            // FIX #1: Only create the file when it doesn't exist.
            // The original code called SQLiteConnection.CreateFile() unconditionally,
            // which overwrites (destroys) the existing database on every app launch.
            bool isNew = !File.Exists(DbPath);
            if (isNew)
                SQLiteConnection.CreateFile(DbPath);

            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                CreateSchema(conn);
                if (isNew) SeedData(conn);
            }
        }

        // ── Schema ─────────────────────────────────────────────────
        private static void CreateSchema(SQLiteConnection conn)
        {
            string[] ddl =
            {
                @"CREATE TABLE IF NOT EXISTS Users (
                    Id       INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL,
                    Role     TEXT NOT NULL DEFAULT 'admin'
                );",

                @"CREATE TABLE IF NOT EXISTS Students (
                    Id         INTEGER PRIMARY KEY AUTOINCREMENT,
                    StudentId  TEXT NOT NULL UNIQUE,
                    Name       TEXT NOT NULL,
                    Department TEXT NOT NULL,
                    Year       TEXT NOT NULL,
                    GPA        REAL NOT NULL DEFAULT 0.0,
                    Email      TEXT NOT NULL,
                    Status     TEXT NOT NULL DEFAULT 'Active'
                );",

                @"CREATE TABLE IF NOT EXISTS Courses (
                    Id         INTEGER PRIMARY KEY AUTOINCREMENT,
                    Code       TEXT NOT NULL UNIQUE,
                    Name       TEXT NOT NULL,
                    Department TEXT NOT NULL,
                    Credits    INTEGER NOT NULL DEFAULT 3,
                    Professor  TEXT NOT NULL,
                    Students   INTEGER NOT NULL DEFAULT 0
                );",

                @"CREATE TABLE IF NOT EXISTS Grades (
                    Id         INTEGER PRIMARY KEY AUTOINCREMENT,
                    StudentId  TEXT NOT NULL,
                    CourseCode TEXT NOT NULL,
                    Midterm    INTEGER NOT NULL DEFAULT 0,
                    Final      INTEGER NOT NULL DEFAULT 0,
                    Total      INTEGER NOT NULL DEFAULT 0,
                    Grade      TEXT NOT NULL,
                    Semester   TEXT NOT NULL,
                    FOREIGN KEY (StudentId)  REFERENCES Students(StudentId),
                    FOREIGN KEY (CourseCode) REFERENCES Courses(Code)
                );",

                @"CREATE TABLE IF NOT EXISTS Enrollments (
                    Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                    StudentName TEXT NOT NULL,
                    CourseCode  TEXT NOT NULL,
                    Department  TEXT NOT NULL,
                    EnrollDate  TEXT NOT NULL,
                    Status      TEXT NOT NULL DEFAULT 'Active'
                );"
            };

            foreach (var sql in ddl)
                ExecuteNonQuery(conn, sql);
        }

        // ── Seed ───────────────────────────────────────────────────
        private static void SeedData(SQLiteConnection conn)
        {
            // FIX #2: Use parameterized queries everywhere in SeedData.
            // The original used raw string interpolation ($"VALUES ('{value}'...)"),
            // which is vulnerable to SQL injection and breaks on values with quotes.

            // Users — password stored as SHA-256 hash (fix #6)
            ExecuteNonQuery(conn,
                "INSERT OR IGNORE INTO Users (Username, Password, Role) VALUES (@u,@p,'admin');",
                new SQLiteParameter("@u", "admin"),
                new SQLiteParameter("@p", HashPassword("1234")));

            // Students
            object[,] students =
            {
                {"S001","Ahmed Hassan",  "Computer Science","3rd",3.8,"ahmed@uni.edu",   "Active"},
                {"S002","Sara Mohamed",  "Mathematics",     "2nd",3.5,"sara@uni.edu",    "Active"},
                {"S003","Omar Ali",      "Physics",         "1st",2.9,"omar@uni.edu",    "Pending"},
                {"S004","Nour Ibrahim",  "Engineering",     "4th",3.9,"nour@uni.edu",    "Active"},
                {"S005","Youssef Saad",  "Computer Science","2nd",2.1,"youssef@uni.edu", "Inactive"},
                {"S006","Fatma Khalid",  "Mathematics",     "3rd",3.6,"fatma@uni.edu",   "Active"},
                {"S007","Kareem Adel",   "Computer Science","1st",3.2,"kareem@uni.edu",  "Active"},
                {"S008","Lina Hassan",   "Engineering",     "2nd",3.7,"lina@uni.edu",    "Active"},
            };
            for (int i = 0; i < students.GetLength(0); i++)
                ExecuteNonQuery(conn,
                    "INSERT OR IGNORE INTO Students (StudentId,Name,Department,Year,GPA,Email,Status) " +
                    "VALUES (@id,@n,@d,@y,@g,@e,@s);",
                    new SQLiteParameter("@id", students[i,0]),
                    new SQLiteParameter("@n",  students[i,1]),
                    new SQLiteParameter("@d",  students[i,2]),
                    new SQLiteParameter("@y",  students[i,3]),
                    new SQLiteParameter("@g",  students[i,4]),
                    new SQLiteParameter("@e",  students[i,5]),
                    new SQLiteParameter("@s",  students[i,6]));

            // Courses
            object[,] courses =
            {
                {"CS101",   "Intro to Programming",  "Computer Science",3,"Dr. Kamal",  45},
                {"CS201",   "Data Structures",        "Computer Science",3,"Dr. Samira", 38},
                {"CS301",   "Software Engineering",   "Computer Science",3,"Dr. Hassan", 30},
                {"MATH101", "Calculus I",             "Mathematics",     4,"Dr. Layla",  60},
                {"MATH201", "Linear Algebra",         "Mathematics",     3,"Dr. Farid",  42},
                {"PHY101",  "Physics I",              "Physics",         4,"Dr. Nadia",  55},
                {"ENG301",  "Technical Writing",      "Engineering",     2,"Dr. Mona",   28},
            };
            for (int i = 0; i < courses.GetLength(0); i++)
                ExecuteNonQuery(conn,
                    "INSERT OR IGNORE INTO Courses (Code,Name,Department,Credits,Professor,Students) " +
                    "VALUES (@c,@n,@d,@cr,@p,@s);",
                    new SQLiteParameter("@c",  courses[i,0]),
                    new SQLiteParameter("@n",  courses[i,1]),
                    new SQLiteParameter("@d",  courses[i,2]),
                    new SQLiteParameter("@cr", courses[i,3]),
                    new SQLiteParameter("@p",  courses[i,4]),
                    new SQLiteParameter("@s",  courses[i,5]));

            // Grades
            object[,] spring =
            {
                {"S001","CS101",   42,48,90,"A",  "Spring 2026"},
                {"S002","MATH201", 38,44,82,"B+", "Spring 2026"},
                {"S003","PHY101",  30,35,65,"C",  "Spring 2026"},
                {"S004","ENG301",  45,50,95,"A+", "Spring 2026"},
                {"S005","CS201",   25,28,53,"D",  "Spring 2026"},
                {"S006","MATH101", 40,46,86,"B+", "Spring 2026"},
                {"S007","CS101",   43,47,90,"A",  "Spring 2026"},
            };
            object[,] fall =
            {
                {"S001","CS201",   36,40,76,"B",  "Fall 2025"},
                {"S002","CS101",   41,45,86,"B+", "Fall 2025"},
                {"S003","MATH101", 22,24,46,"F",  "Fall 2025"},
                {"S004","CS101",   44,49,93,"A",  "Fall 2025"},
                {"S005","PHY101",  28,30,58,"D",  "Fall 2025"},
                {"S006","CS201",   39,43,82,"B+", "Fall 2025"},
                {"S007","MATH101", 35,38,73,"C+", "Fall 2025"},
            };
            InsertGrades(conn, spring);
            InsertGrades(conn, fall);

            // Enrollments
            object[,] enroll =
            {
                {"Ahmed Hassan",  "CS101",   "Computer Science", "Apr 20, 2026", "Active"},
                {"Sara Mohamed",  "MATH201", "Mathematics",      "Apr 19, 2026", "Active"},
                {"Omar Ali",      "PHY101",  "Physics",          "Apr 18, 2026", "Pending"},
                {"Nour Ibrahim",  "ENG301",  "Engineering",      "Apr 17, 2026", "Active"},
                {"Youssef Saad",  "CS201",   "Computer Science", "Apr 16, 2026", "Inactive"},
            };
            for (int i = 0; i < enroll.GetLength(0); i++)
                ExecuteNonQuery(conn,
                    "INSERT INTO Enrollments (StudentName,CourseCode,Department,EnrollDate,Status) " +
                    "VALUES (@n,@c,@d,@e,@s);",
                    new SQLiteParameter("@n", enroll[i,0]),
                    new SQLiteParameter("@c", enroll[i,1]),
                    new SQLiteParameter("@d", enroll[i,2]),
                    new SQLiteParameter("@e", enroll[i,3]),
                    new SQLiteParameter("@s", enroll[i,4]));
        }

        private static void InsertGrades(SQLiteConnection conn, object[,] data)
        {
            for (int i = 0; i < data.GetLength(0); i++)
                ExecuteNonQuery(conn,
                    "INSERT INTO Grades (StudentId,CourseCode,Midterm,Final,Total,Grade,Semester) " +
                    "VALUES (@sid,@cc,@mid,@fin,@tot,@gr,@sem);",
                    new SQLiteParameter("@sid", data[i,0]),
                    new SQLiteParameter("@cc",  data[i,1]),
                    new SQLiteParameter("@mid", data[i,2]),
                    new SQLiteParameter("@fin", data[i,3]),
                    new SQLiteParameter("@tot", data[i,4]),
                    new SQLiteParameter("@gr",  data[i,5]),
                    new SQLiteParameter("@sem", data[i,6]));
        }

        // ── Password hashing ───────────────────────────────────────
        /// <summary>Returns the hex-encoded SHA-256 hash of a plain-text password.</summary>
        public static string HashPassword(string plain)
        {
            using (var sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(plain));
                var sb = new StringBuilder(bytes.Length * 2);
                foreach (byte b in bytes) sb.AppendFormat("{0:x2}", b);
                return sb.ToString();
            }
        }

        // ── Public query helpers ───────────────────────────────────

        public static DataTable Query(string sql, params SQLiteParameter[] parameters)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    foreach (var p in parameters) cmd.Parameters.Add(p);
                    var dt = new DataTable();
                    new SQLiteDataAdapter(cmd).Fill(dt);
                    return dt;
                }
            }
        }

        public static int Execute(string sql, params SQLiteParameter[] parameters)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                return ExecuteNonQuery(conn, sql, parameters);
            }
        }

        public static object Scalar(string sql, params SQLiteParameter[] parameters)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    foreach (var p in parameters) cmd.Parameters.Add(p);
                    return cmd.ExecuteScalar();
                }
            }
        }

        private static int ExecuteNonQuery(SQLiteConnection conn, string sql,
            params SQLiteParameter[] parameters)
        {
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                foreach (var p in parameters) cmd.Parameters.Add(p);
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
