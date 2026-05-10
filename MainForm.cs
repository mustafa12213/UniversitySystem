using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace UniversitySystem
{
    public class MainForm : Form
    {
        private Panel sidePanel;
        private Panel contentPanel;
        private Panel topBar;
        private Label lblPageTitle;
        private Button btnActive;

        private Button btnDashboard, btnStudents, btnCourses, btnGrades, btnLogout;

        // FIX #5: Hold a reference to the LoginForm so we can close it (not leak it)
        // when this MainForm closes, and show a fresh one on logout.
        private readonly Form _loginForm;

        public MainForm(Form loginForm)
        {
            _loginForm = loginForm;
            InitializeComponents();
            ShowDashboard();
        }

        private void InitializeComponents()
        {
            this.Text = "UniSystem - University Management";
            this.Size = new Size(1100, 660);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(244, 246, 250);
            this.MinimumSize = new Size(900, 600);

            // FIX #5: When this form closes for any reason (including logout),
            // also close the hidden LoginForm so no windows are leaked.
            this.FormClosed += (s, e) => _loginForm.Close();

            sidePanel = new Panel();
            sidePanel.Size = new Size(200, 660);
            sidePanel.Location = new Point(0, 0);
            sidePanel.BackColor = Color.FromArgb(12, 68, 124);
            sidePanel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;

            Label lblLogo = new Label();
            lblLogo.Text = "UniSystem";
            lblLogo.Font = new Font("Segoe UI", 15, FontStyle.Bold);
            lblLogo.ForeColor = Color.White;
            lblLogo.Location = new Point(20, 24);
            lblLogo.AutoSize = true;

            Label lblAdmin = new Label();
            lblAdmin.Text = "Admin Panel";
            lblAdmin.Font = new Font("Segoe UI", 9);
            lblAdmin.ForeColor = Color.FromArgb(150, 190, 230);
            lblAdmin.Location = new Point(20, 50);
            lblAdmin.AutoSize = true;

            Panel divider = new Panel();
            divider.Size = new Size(160, 1);
            divider.Location = new Point(20, 72);
            divider.BackColor = Color.FromArgb(50, 100, 160);

            sidePanel.Controls.Add(lblLogo);
            sidePanel.Controls.Add(lblAdmin);
            sidePanel.Controls.Add(divider);

            btnDashboard = CreateNavButton("Dashboard", 90);
            btnStudents  = CreateNavButton("Students",  130);
            btnCourses   = CreateNavButton("Courses",   170);
            btnGrades    = CreateNavButton("Grades",    210);

            btnDashboard.Click += (s, e) => { SetActive(btnDashboard); ShowDashboard(); };
            btnStudents.Click  += (s, e) => { SetActive(btnStudents);  ShowStudents(); };
            btnCourses.Click   += (s, e) => { SetActive(btnCourses);   ShowCourses(); };
            btnGrades.Click    += (s, e) => { SetActive(btnGrades);    ShowGrades(); };

            sidePanel.Controls.Add(btnDashboard);
            sidePanel.Controls.Add(btnStudents);
            sidePanel.Controls.Add(btnCourses);
            sidePanel.Controls.Add(btnGrades);

            btnLogout = new Button();
            btnLogout.Text = "Logout";
            btnLogout.Size = new Size(160, 36);
            btnLogout.Location = new Point(20, 580);
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.ForeColor = Color.FromArgb(150, 190, 230);
            btnLogout.Font = new Font("Segoe UI", 9);
            btnLogout.TextAlign = ContentAlignment.MiddleLeft;
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            // FIX #5: Show a brand-new LoginForm, then close THIS MainForm.
            // FormClosed (above) will close the old hidden LoginForm automatically.
            btnLogout.Click += (s, e) =>
            {
                LoginForm newLogin = new LoginForm();
                newLogin.Show();
                this.Close(); // triggers FormClosed → _loginForm.Close()
            };
            sidePanel.Controls.Add(btnLogout);

            topBar = new Panel();
            topBar.Size = new Size(900, 50);
            topBar.Location = new Point(200, 0);
            topBar.BackColor = Color.White;
            topBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            lblPageTitle = new Label();
            lblPageTitle.Text = "Dashboard";
            lblPageTitle.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblPageTitle.ForeColor = Color.FromArgb(12, 68, 124);
            lblPageTitle.Location = new Point(24, 14);
            lblPageTitle.AutoSize = true;

            Label lblUser = new Label();
            lblUser.Text = "Admin";
            lblUser.Font = new Font("Segoe UI", 9);
            lblUser.ForeColor = Color.Gray;
            lblUser.Location = new Point(790, 18);
            lblUser.AutoSize = true;
            lblUser.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            topBar.Controls.Add(lblPageTitle);
            topBar.Controls.Add(lblUser);

            contentPanel = new Panel();
            contentPanel.Location = new Point(200, 50);
            contentPanel.Size = new Size(900, 610);
            contentPanel.BackColor = Color.FromArgb(244, 246, 250);
            contentPanel.AutoScroll = true;
            contentPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            this.Controls.Add(sidePanel);
            this.Controls.Add(topBar);
            this.Controls.Add(contentPanel);

            SetActive(btnDashboard);
        }

        private Button CreateNavButton(string text, int y)
        {
            Button btn = new Button();
            btn.Text = "  " + text;
            btn.Size = new Size(160, 36);
            btn.Location = new Point(20, y);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.ForeColor = Color.FromArgb(150, 190, 230);
            btn.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private void SetActive(Button btn)
        {
            Button[] allBtns = { btnDashboard, btnStudents, btnCourses, btnGrades };
            foreach (var b in allBtns)
            {
                b.BackColor = Color.Transparent;
                b.ForeColor = Color.FromArgb(150, 190, 230);
                b.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            }
            btn.BackColor = Color.FromArgb(30, 80, 150);
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnActive = btn;
        }

        private Panel CreateStatCard(string title, string value, string sub, Color accent, int x, int y)
        {
            Panel card = new Panel();
            card.Size = new Size(190, 90);
            card.Location = new Point(x, y);
            card.BackColor = Color.White;

            Panel leftBar = new Panel();
            leftBar.Size = new Size(4, 90);
            leftBar.Location = new Point(0, 0);
            leftBar.BackColor = accent;
            card.Controls.Add(leftBar);

            Label lbl = new Label();
            lbl.Text = title.ToUpper();
            lbl.Font = new Font("Segoe UI", 7, FontStyle.Bold);
            lbl.ForeColor = Color.Gray;
            lbl.Location = new Point(16, 14);
            lbl.AutoSize = true;
            card.Controls.Add(lbl);

            Label val = new Label();
            val.Text = value;
            val.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            val.ForeColor = accent;
            val.Location = new Point(14, 30);
            val.AutoSize = true;
            card.Controls.Add(val);

            Label subLbl = new Label();
            subLbl.Text = sub;
            subLbl.Font = new Font("Segoe UI", 8);
            subLbl.ForeColor = Color.LightGray;
            subLbl.Location = new Point(16, 68);
            subLbl.AutoSize = true;
            card.Controls.Add(subLbl);

            return card;
        }

        private Label CreateSectionTitle(string text, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(12, 68, 124);
            lbl.Location = new Point(x, y);
            lbl.AutoSize = true;
            return lbl;
        }

        // ── DASHBOARD ──────────────────────────────────────────────
        private void ShowDashboard()
        {
            contentPanel.Controls.Clear();
            lblPageTitle.Text = "Dashboard";

            string totalStudents   = DatabaseHelper.Scalar("SELECT COUNT(*) FROM Students;").ToString();
            string totalCourses    = DatabaseHelper.Scalar("SELECT COUNT(*) FROM Courses;").ToString();
            // FIX #7: Added the missing Enrollments stat card
            string totalEnrollments = DatabaseHelper.Scalar("SELECT COUNT(*) FROM Enrollments;").ToString();
            object avgGpaObj       = DatabaseHelper.Scalar("SELECT ROUND(AVG(GPA),1) FROM Students;");
            string avgGpa          = (avgGpaObj == null || avgGpaObj == DBNull.Value) ? "N/A" : avgGpaObj.ToString();

            contentPanel.Controls.Add(CreateSectionTitle("Overview", 20, 20));
            contentPanel.Controls.Add(CreateStatCard("Total Students", totalStudents,    "Registered in system",   Color.FromArgb(24, 95, 165),  20,  50));
            contentPanel.Controls.Add(CreateStatCard("Courses",        totalCourses,     "Across all departments", Color.FromArgb(15, 110, 86),  230, 50));
            contentPanel.Controls.Add(CreateStatCard("Enrollments",    totalEnrollments, "Active enrollments",     Color.FromArgb(180, 100, 20), 440, 50));
            contentPanel.Controls.Add(CreateStatCard("Average GPA",    avgGpa,           "Across all students",    Color.FromArgb(83, 58, 183),  650, 50));

            contentPanel.Controls.Add(CreateSectionTitle("Recent Enrollments", 20, 160));

            ListView lv = new ListView();
            lv.Location = new Point(20, 190);
            lv.Size = new Size(840, 180);
            lv.View = View.Details;
            lv.FullRowSelect = true;
            lv.GridLines = true;
            lv.Font = new Font("Segoe UI", 9);
            lv.BackColor = Color.White;
            lv.BorderStyle = BorderStyle.FixedSingle;
            lv.Columns.Add("Student Name", 200);
            lv.Columns.Add("Course Code",  120);
            lv.Columns.Add("Department",   180);
            lv.Columns.Add("Date",         140);
            lv.Columns.Add("Status",       120);

            DataTable dt = DatabaseHelper.Query(
                "SELECT StudentName, CourseCode, Department, EnrollDate, Status FROM Enrollments ORDER BY rowid DESC LIMIT 10;");
            foreach (DataRow row in dt.Rows)
            {
                var item = new ListViewItem(row[0].ToString());
                for (int j = 1; j < row.ItemArray.Length; j++)
                    item.SubItems.Add(row[j].ToString());
                lv.Items.Add(item);
            }
            contentPanel.Controls.Add(lv);
        }

        // ── STUDENTS ───────────────────────────────────────────────
        private void ShowStudents()
        {
            contentPanel.Controls.Clear();
            lblPageTitle.Text = "Students";

            TextBox search = new TextBox();
            search.Location = new Point(20, 20);
            search.Size = new Size(250, 30);
            search.Font = new Font("Segoe UI", 10);
            search.BorderStyle = BorderStyle.FixedSingle;
            search.Text = "Search students...";
            search.ForeColor = Color.Gray;
            search.GotFocus  += (sv, ev) => { if (search.Text == "Search students...") { search.Text = ""; search.ForeColor = Color.Black; } };
            search.LostFocus += (sv, ev) => { if (search.Text == "") { search.Text = "Search students..."; search.ForeColor = Color.Gray; } };

            Button btnAdd = new Button();
            btnAdd.Text = "+ Add Student";
            btnAdd.Location = new Point(560, 18);
            btnAdd.Size = new Size(140, 32);
            btnAdd.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnAdd.BackColor = Color.FromArgb(24, 95, 165);
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Cursor = Cursors.Hand;

            Button btnDelete = new Button();
            btnDelete.Text = "Delete";
            btnDelete.Location = new Point(710, 18);
            btnDelete.Size = new Size(110, 32);
            btnDelete.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnDelete.BackColor = Color.FromArgb(200, 50, 50);
            btnDelete.ForeColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Cursor = Cursors.Hand;

            ListView lv = new ListView();
            lv.Location = new Point(20, 62);
            lv.Size = new Size(840, 380);
            lv.View = View.Details;
            lv.FullRowSelect = true;
            lv.GridLines = true;
            lv.Font = new Font("Segoe UI", 9);
            lv.BackColor = Color.White;
            lv.BorderStyle = BorderStyle.FixedSingle;
            lv.Columns.Add("ID",         70);
            lv.Columns.Add("Name",      180);
            lv.Columns.Add("Department",160);
            lv.Columns.Add("Year",       80);
            lv.Columns.Add("GPA",        60);
            lv.Columns.Add("Email",     180);
            lv.Columns.Add("Status",    100);

            Action loadStudents = () =>
            {
                lv.Items.Clear();
                string q = search.Text.ToLower();
                bool filtering = q != "" && q != "search students...";
                DataTable dt;
                if (filtering)
                    dt = DatabaseHelper.Query(
                        "SELECT StudentId, Name, Department, Year, GPA, Email, Status FROM Students " +
                        "WHERE LOWER(Name) LIKE @q OR LOWER(StudentId) LIKE @q OR LOWER(Department) LIKE @q ORDER BY StudentId;",
                        new SQLiteParameter("@q", "%" + q + "%"));
                else
                    dt = DatabaseHelper.Query(
                        "SELECT StudentId, Name, Department, Year, GPA, Email, Status FROM Students ORDER BY StudentId;");

                foreach (DataRow row in dt.Rows)
                {
                    var item = new ListViewItem(row[0].ToString());
                    for (int j = 1; j < row.ItemArray.Length; j++) item.SubItems.Add(row[j].ToString());

                    // FIX #3: Read status from the ListViewItem's sub-item (index 6),
                    // not directly from the DataRow, to be robust against column reordering.
                    string status = item.SubItems[6].Text;
                    if (status == "Active")   item.BackColor = Color.FromArgb(240, 255, 245);
                    if (status == "Inactive") item.BackColor = Color.FromArgb(255, 240, 240);
                    if (status == "Pending")  item.BackColor = Color.FromArgb(255, 250, 235);
                    lv.Items.Add(item);
                }
            };
            loadStudents();

            search.TextChanged += (s, e) => loadStudents();
            btnAdd.Click       += (s, e) => { ShowAddStudentDialog(); loadStudents(); };
            btnDelete.Click    += (s, e) =>
            {
                if (lv.SelectedItems.Count == 0) { MessageBox.Show("Select a student first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                string sid = lv.SelectedItems[0].Text, name = lv.SelectedItems[0].SubItems[1].Text;
                if (MessageBox.Show($"Delete \"{name}\" ({sid})?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DatabaseHelper.Execute("DELETE FROM Students WHERE StudentId=@id;", new SQLiteParameter("@id", sid));
                    loadStudents();
                }
            };

            contentPanel.Controls.Add(search);
            contentPanel.Controls.Add(btnAdd);
            contentPanel.Controls.Add(btnDelete);
            contentPanel.Controls.Add(lv);
        }

        private void ShowAddStudentDialog()
        {
            Form dlg = new Form();
            dlg.Text = "Add New Student";
            dlg.Size = new Size(420, 480);
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
            dlg.MaximizeBox = false;
            dlg.BackColor = Color.White;

            string[] labels = { "Student ID", "Full Name", "Department", "Year", "GPA", "Email", "Status" };
            Control[] inputs = new Control[labels.Length];

            for (int i = 0; i < labels.Length; i++)
            {
                Label lbl = new Label();
                lbl.Text = labels[i];
                lbl.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lbl.ForeColor = Color.FromArgb(60, 60, 60);
                lbl.Location = new Point(30, 20 + i * 52);
                lbl.AutoSize = true;
                dlg.Controls.Add(lbl);

                if (labels[i] == "Department") { var c = new ComboBox(); c.Items.AddRange(new[] { "Computer Science", "Mathematics", "Physics", "Engineering" }); c.Location = new Point(30, 38 + i * 52); c.Size = new Size(340, 28); c.Font = new Font("Segoe UI", 10); c.DropDownStyle = ComboBoxStyle.DropDownList; c.SelectedIndex = 0; dlg.Controls.Add(c); inputs[i] = c; }
                else if (labels[i] == "Year")  { var c = new ComboBox(); c.Items.AddRange(new[] { "1st", "2nd", "3rd", "4th" }); c.Location = new Point(30, 38 + i * 52); c.Size = new Size(340, 28); c.Font = new Font("Segoe UI", 10); c.DropDownStyle = ComboBoxStyle.DropDownList; c.SelectedIndex = 0; dlg.Controls.Add(c); inputs[i] = c; }
                else if (labels[i] == "Status"){ var c = new ComboBox(); c.Items.AddRange(new[] { "Active", "Inactive", "Pending" }); c.Location = new Point(30, 38 + i * 52); c.Size = new Size(340, 28); c.Font = new Font("Segoe UI", 10); c.DropDownStyle = ComboBoxStyle.DropDownList; c.SelectedIndex = 0; dlg.Controls.Add(c); inputs[i] = c; }
                else { var t = new TextBox(); t.Location = new Point(30, 38 + i * 52); t.Size = new Size(340, 28); t.Font = new Font("Segoe UI", 10); t.BorderStyle = BorderStyle.FixedSingle; dlg.Controls.Add(t); inputs[i] = t; }
            }

            Button btnSave = new Button();
            btnSave.Text = "Add Student"; btnSave.Location = new Point(30, 395); btnSave.Size = new Size(150, 36);
            btnSave.Font = new Font("Segoe UI", 10, FontStyle.Bold); btnSave.BackColor = Color.FromArgb(24, 95, 165);
            btnSave.ForeColor = Color.White; btnSave.FlatStyle = FlatStyle.Flat; btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) =>
            {
                string sid = ((TextBox)inputs[0]).Text.Trim(), name = ((TextBox)inputs[1]).Text.Trim();
                string dept = ((ComboBox)inputs[2]).Text, year = ((ComboBox)inputs[3]).Text;
                string gpa = ((TextBox)inputs[4]).Text.Trim(), email = ((TextBox)inputs[5]).Text.Trim();
                string stat = ((ComboBox)inputs[6]).Text;
                if (string.IsNullOrEmpty(sid) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email)) { MessageBox.Show("Fill in ID, Name and Email.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (!double.TryParse(gpa, out double gpaVal) || gpaVal < 0 || gpaVal > 4.0) { MessageBox.Show("GPA must be 0–4.0.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                try
                {
                    DatabaseHelper.Execute("INSERT INTO Students (StudentId,Name,Department,Year,GPA,Email,Status) VALUES (@id,@n,@d,@y,@g,@e,@s);",
                        new SQLiteParameter("@id", sid), new SQLiteParameter("@n", name), new SQLiteParameter("@d", dept),
                        new SQLiteParameter("@y", year), new SQLiteParameter("@g", gpaVal), new SQLiteParameter("@e", email), new SQLiteParameter("@s", stat));
                    MessageBox.Show("Student added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); dlg.Close();
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message, "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            Button btnCancel = new Button();
            btnCancel.Text = "Cancel"; btnCancel.Location = new Point(200, 395); btnCancel.Size = new Size(150, 36);
            btnCancel.Font = new Font("Segoe UI", 10); btnCancel.BackColor = Color.FromArgb(240, 240, 240);
            btnCancel.FlatStyle = FlatStyle.Flat; btnCancel.Click += (s, e) => dlg.Close();

            dlg.Controls.Add(btnSave); dlg.Controls.Add(btnCancel);
            dlg.ShowDialog();
        }

        // ── COURSES ────────────────────────────────────────────────
        private void ShowCourses()
        {
            contentPanel.Controls.Clear();
            lblPageTitle.Text = "Courses";

            // FIX #8: Added a search bar to Courses (consistent with Students page)
            TextBox search = new TextBox();
            search.Location = new Point(20, 20);
            search.Size = new Size(250, 30);
            search.Font = new Font("Segoe UI", 10);
            search.BorderStyle = BorderStyle.FixedSingle;
            search.Text = "Search courses...";
            search.ForeColor = Color.Gray;
            search.GotFocus  += (sv, ev) => { if (search.Text == "Search courses...") { search.Text = ""; search.ForeColor = Color.Black; } };
            search.LostFocus += (sv, ev) => { if (search.Text == "") { search.Text = "Search courses..."; search.ForeColor = Color.Gray; } };

            ListView lv = new ListView();
            lv.Location = new Point(20, 60); lv.Size = new Size(840, 380);
            lv.View = View.Details; lv.FullRowSelect = true; lv.GridLines = true;
            lv.Font = new Font("Segoe UI", 9); lv.BackColor = Color.White; lv.BorderStyle = BorderStyle.FixedSingle;
            lv.Columns.Add("Code",        90); lv.Columns.Add("Course Name", 200);
            lv.Columns.Add("Department",  160); lv.Columns.Add("Credits",     70);
            lv.Columns.Add("Professor",   160); lv.Columns.Add("Students",    80);

            Action loadCourses = () =>
            {
                lv.Items.Clear();
                string q = search.Text.ToLower();
                bool filtering = q != "" && q != "search courses...";
                DataTable dt;
                if (filtering)
                    dt = DatabaseHelper.Query(
                        "SELECT Code, Name, Department, Credits, Professor, Students FROM Courses " +
                        "WHERE LOWER(Code) LIKE @q OR LOWER(Name) LIKE @q OR LOWER(Department) LIKE @q ORDER BY Code;",
                        new SQLiteParameter("@q", "%" + q + "%"));
                else
                    dt = DatabaseHelper.Query("SELECT Code, Name, Department, Credits, Professor, Students FROM Courses ORDER BY Code;");

                foreach (DataRow row in dt.Rows)
                {
                    var item = new ListViewItem(row[0].ToString());
                    for (int j = 1; j < row.ItemArray.Length; j++) item.SubItems.Add(row[j].ToString());
                    lv.Items.Add(item);
                }
            };
            loadCourses();

            search.TextChanged += (s, e) => loadCourses();

            Button btnAdd = new Button();
            btnAdd.Text = "+ Add Course"; btnAdd.Location = new Point(600, 18); btnAdd.Size = new Size(130, 32);
            btnAdd.Font = new Font("Segoe UI", 9, FontStyle.Bold); btnAdd.BackColor = Color.FromArgb(24, 95, 165);
            btnAdd.ForeColor = Color.White; btnAdd.FlatStyle = FlatStyle.Flat; btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Cursor = Cursors.Hand; btnAdd.Click += (s, e) => { ShowAddCourseDialog(); loadCourses(); };

            Button btnDelete = new Button();
            btnDelete.Text = "Delete"; btnDelete.Location = new Point(740, 18); btnDelete.Size = new Size(110, 32);
            btnDelete.Font = new Font("Segoe UI", 9, FontStyle.Bold); btnDelete.BackColor = Color.FromArgb(200, 50, 50);
            btnDelete.ForeColor = Color.White; btnDelete.FlatStyle = FlatStyle.Flat; btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.Click += (s, e) =>
            {
                if (lv.SelectedItems.Count == 0) { MessageBox.Show("Select a course first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                string code = lv.SelectedItems[0].Text, name = lv.SelectedItems[0].SubItems[1].Text;
                if (MessageBox.Show($"Delete \"{name}\" ({code})?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                { DatabaseHelper.Execute("DELETE FROM Courses WHERE Code=@c;", new SQLiteParameter("@c", code)); loadCourses(); }
            };

            contentPanel.Controls.Add(search);
            contentPanel.Controls.Add(btnAdd);
            contentPanel.Controls.Add(btnDelete);
            contentPanel.Controls.Add(lv);
        }

        private void ShowAddCourseDialog()
        {
            Form dlg = new Form();
            dlg.Text = "Add New Course"; dlg.Size = new Size(420, 420);
            dlg.StartPosition = FormStartPosition.CenterParent; dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
            dlg.MaximizeBox = false; dlg.BackColor = Color.White;

            string[] labels = { "Course Code", "Course Name", "Department", "Credits", "Professor" };
            Control[] inputs = new Control[5];

            for (int i = 0; i < labels.Length; i++)
            {
                Label lbl = new Label(); lbl.Text = labels[i]; lbl.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lbl.ForeColor = Color.FromArgb(60, 60, 60); lbl.Location = new Point(30, 20 + i * 60); lbl.AutoSize = true;
                dlg.Controls.Add(lbl);

                if (labels[i] == "Department") { var c = new ComboBox(); c.Items.AddRange(new[] { "Computer Science", "Mathematics", "Physics", "Engineering" }); c.Location = new Point(30, 40 + i * 60); c.Size = new Size(340, 28); c.Font = new Font("Segoe UI", 10); c.DropDownStyle = ComboBoxStyle.DropDownList; c.SelectedIndex = 0; dlg.Controls.Add(c); inputs[i] = c; }
                else if (labels[i] == "Credits") { var c = new ComboBox(); c.Items.AddRange(new[] { "1", "2", "3", "4" }); c.Location = new Point(30, 40 + i * 60); c.Size = new Size(340, 28); c.Font = new Font("Segoe UI", 10); c.DropDownStyle = ComboBoxStyle.DropDownList; c.SelectedIndex = 2; dlg.Controls.Add(c); inputs[i] = c; }
                else { var t = new TextBox(); t.Location = new Point(30, 40 + i * 60); t.Size = new Size(340, 28); t.Font = new Font("Segoe UI", 10); t.BorderStyle = BorderStyle.FixedSingle; dlg.Controls.Add(t); inputs[i] = t; }
            }

            Button btnSave = new Button();
            btnSave.Text = "Add Course"; btnSave.Location = new Point(30, 330); btnSave.Size = new Size(150, 36);
            btnSave.Font = new Font("Segoe UI", 10, FontStyle.Bold); btnSave.BackColor = Color.FromArgb(24, 95, 165);
            btnSave.ForeColor = Color.White; btnSave.FlatStyle = FlatStyle.Flat; btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) =>
            {
                string code = ((TextBox)inputs[0]).Text.Trim(), name = ((TextBox)inputs[1]).Text.Trim();
                string dept = ((ComboBox)inputs[2]).Text; int credits = int.Parse(((ComboBox)inputs[3]).Text);
                string prof = ((TextBox)inputs[4]).Text.Trim();
                if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(prof)) { MessageBox.Show("Fill in Code, Name and Professor.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                try
                {
                    DatabaseHelper.Execute("INSERT INTO Courses (Code,Name,Department,Credits,Professor,Students) VALUES (@c,@n,@d,@cr,@p,0);",
                        new SQLiteParameter("@c", code), new SQLiteParameter("@n", name), new SQLiteParameter("@d", dept),
                        new SQLiteParameter("@cr", credits), new SQLiteParameter("@p", prof));
                    MessageBox.Show("Course added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); dlg.Close();
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message, "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            Button btnCancel = new Button();
            btnCancel.Text = "Cancel"; btnCancel.Location = new Point(200, 330); btnCancel.Size = new Size(150, 36);
            btnCancel.Font = new Font("Segoe UI", 10); btnCancel.BackColor = Color.FromArgb(240, 240, 240);
            btnCancel.FlatStyle = FlatStyle.Flat; btnCancel.Click += (s, e) => dlg.Close();

            dlg.Controls.Add(btnSave); dlg.Controls.Add(btnCancel);
            dlg.ShowDialog();
        }

        // ── GRADES ─────────────────────────────────────────────────
        private void ShowGrades()
        {
            contentPanel.Controls.Clear();
            lblPageTitle.Text = "Grades";

            Label lblCourse = new Label(); lblCourse.Text = "Course:"; lblCourse.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblCourse.Location = new Point(20, 24); lblCourse.AutoSize = true;

            ComboBox cmbCourse = new ComboBox();
            cmbCourse.Items.Add("All Courses");
            DataTable coursesDt = DatabaseHelper.Query("SELECT Code FROM Courses ORDER BY Code;");
            foreach (DataRow r in coursesDt.Rows) cmbCourse.Items.Add(r[0].ToString());
            cmbCourse.Location = new Point(75, 20); cmbCourse.Size = new Size(160, 28);
            cmbCourse.Font = new Font("Segoe UI", 9); cmbCourse.SelectedIndex = 0; cmbCourse.DropDownStyle = ComboBoxStyle.DropDownList;

            Label lblSem = new Label(); lblSem.Text = "Semester:"; lblSem.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblSem.Location = new Point(255, 24); lblSem.AutoSize = true;

            ComboBox cmbSem = new ComboBox();
            cmbSem.Items.Add("All Semesters");
            DataTable semDt = DatabaseHelper.Query("SELECT DISTINCT Semester FROM Grades ORDER BY Semester DESC;");
            foreach (DataRow r in semDt.Rows) cmbSem.Items.Add(r[0].ToString());
            cmbSem.Location = new Point(320, 20); cmbSem.Size = new Size(160, 28);
            cmbSem.Font = new Font("Segoe UI", 9); cmbSem.SelectedIndex = 0; cmbSem.DropDownStyle = ComboBoxStyle.DropDownList;

            // FIX #4: Add "Add Grade" and "Delete Grade" buttons
            Button btnAddGrade = new Button();
            btnAddGrade.Text = "+ Add Grade"; btnAddGrade.Location = new Point(560, 18); btnAddGrade.Size = new Size(130, 32);
            btnAddGrade.Font = new Font("Segoe UI", 9, FontStyle.Bold); btnAddGrade.BackColor = Color.FromArgb(24, 95, 165);
            btnAddGrade.ForeColor = Color.White; btnAddGrade.FlatStyle = FlatStyle.Flat; btnAddGrade.FlatAppearance.BorderSize = 0;
            btnAddGrade.Cursor = Cursors.Hand;

            Button btnDeleteGrade = new Button();
            btnDeleteGrade.Text = "Delete"; btnDeleteGrade.Location = new Point(700, 18); btnDeleteGrade.Size = new Size(110, 32);
            btnDeleteGrade.Font = new Font("Segoe UI", 9, FontStyle.Bold); btnDeleteGrade.BackColor = Color.FromArgb(200, 50, 50);
            btnDeleteGrade.ForeColor = Color.White; btnDeleteGrade.FlatStyle = FlatStyle.Flat; btnDeleteGrade.FlatAppearance.BorderSize = 0;
            btnDeleteGrade.Cursor = Cursors.Hand;

            ListView lv = new ListView();
            lv.Location = new Point(20, 60); lv.Size = new Size(840, 360);
            lv.View = View.Details; lv.FullRowSelect = true; lv.GridLines = true;
            lv.Font = new Font("Segoe UI", 9); lv.BackColor = Color.White; lv.BorderStyle = BorderStyle.FixedSingle;
            lv.Columns.Add("Student",  180); lv.Columns.Add("Course",   100);
            lv.Columns.Add("Midterm",   90); lv.Columns.Add("Final",     90);
            lv.Columns.Add("Total",     90); lv.Columns.Add("Grade",     90);
            lv.Columns.Add("Semester", 130);
            // Hidden column to store the grade row Id for deletion
            lv.Columns.Add("Id", 0);

            Action loadGrades = () =>
            {
                lv.Items.Clear();
                string cFilter = cmbCourse.SelectedIndex == 0 ? null : cmbCourse.SelectedItem.ToString();
                string sFilter = cmbSem.SelectedIndex    == 0 ? null : cmbSem.SelectedItem.ToString();
                DataTable dt = DatabaseHelper.Query(
                    "SELECT g.Id, s.Name, g.CourseCode, g.Midterm, g.Final, g.Total, g.Grade, g.Semester " +
                    "FROM Grades g JOIN Students s ON g.StudentId = s.StudentId " +
                    "WHERE (@c IS NULL OR g.CourseCode=@c) AND (@s IS NULL OR g.Semester=@s) " +
                    "ORDER BY g.Semester DESC, s.Name;",
                    new SQLiteParameter("@c", (object)cFilter ?? DBNull.Value),
                    new SQLiteParameter("@s", (object)sFilter ?? DBNull.Value));
                foreach (DataRow row in dt.Rows)
                {
                    // Columns: Id, Name, CourseCode, Midterm, Final, Total, Grade, Semester
                    var item = new ListViewItem(row[1].ToString()); // Name
                    item.SubItems.Add(row[2].ToString()); // CourseCode
                    item.SubItems.Add(row[3].ToString()); // Midterm
                    item.SubItems.Add(row[4].ToString()); // Final
                    item.SubItems.Add(row[5].ToString()); // Total
                    item.SubItems.Add(row[6].ToString()); // Grade
                    item.SubItems.Add(row[7].ToString()); // Semester
                    item.SubItems.Add(row[0].ToString()); // Id (hidden)
                    string g = row[6].ToString();
                    if (g == "A+" || g == "A")      item.BackColor = Color.FromArgb(240, 255, 245);
                    else if (g == "B+" || g == "B") item.BackColor = Color.FromArgb(240, 248, 255);
                    else if (g == "C+" || g == "C") item.BackColor = Color.FromArgb(255, 250, 235);
                    else                             item.BackColor = Color.FromArgb(255, 240, 240);
                    lv.Items.Add(item);
                }
            };
            loadGrades();

            cmbCourse.SelectedIndexChanged += (s, e) => loadGrades();
            cmbSem.SelectedIndexChanged    += (s, e) => loadGrades();

            btnAddGrade.Click += (s, e) => { ShowAddGradeDialog(); loadGrades(); };
            btnDeleteGrade.Click += (s, e) =>
            {
                if (lv.SelectedItems.Count == 0) { MessageBox.Show("Select a grade record first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                string studentName = lv.SelectedItems[0].Text;
                string gradeId = lv.SelectedItems[0].SubItems[7].Text;
                if (MessageBox.Show($"Delete grade record for \"{studentName}\"?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DatabaseHelper.Execute("DELETE FROM Grades WHERE Id=@id;", new SQLiteParameter("@id", gradeId));
                    loadGrades();
                }
            };

            contentPanel.Controls.Add(lblCourse); contentPanel.Controls.Add(cmbCourse);
            contentPanel.Controls.Add(lblSem);    contentPanel.Controls.Add(cmbSem);
            contentPanel.Controls.Add(btnAddGrade);
            contentPanel.Controls.Add(btnDeleteGrade);
            contentPanel.Controls.Add(lv);
        }

        private void ShowAddGradeDialog()
        {
            Form dlg = new Form();
            dlg.Text = "Add Grade Record";
            dlg.Size = new Size(420, 420);
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
            dlg.MaximizeBox = false;
            dlg.BackColor = Color.White;

            string[] fieldLabels = { "Student ID", "Course Code", "Midterm (0-50)", "Final (0-50)", "Semester" };
            TextBox[] fields = new TextBox[fieldLabels.Length];

            for (int i = 0; i < fieldLabels.Length; i++)
            {
                Label lbl = new Label();
                lbl.Text = fieldLabels[i];
                lbl.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lbl.ForeColor = Color.FromArgb(60, 60, 60);
                lbl.Location = new Point(30, 20 + i * 60);
                lbl.AutoSize = true;
                dlg.Controls.Add(lbl);

                fields[i] = new TextBox();
                fields[i].Location = new Point(30, 40 + i * 60);
                fields[i].Size = new Size(340, 28);
                fields[i].Font = new Font("Segoe UI", 10);
                fields[i].BorderStyle = BorderStyle.FixedSingle;
                dlg.Controls.Add(fields[i]);
            }
            fields[4].Text = "Spring 2026";

            Button btnSave = new Button();
            btnSave.Text = "Add Grade"; btnSave.Location = new Point(30, 330); btnSave.Size = new Size(150, 36);
            btnSave.Font = new Font("Segoe UI", 10, FontStyle.Bold); btnSave.BackColor = Color.FromArgb(24, 95, 165);
            btnSave.ForeColor = Color.White; btnSave.FlatStyle = FlatStyle.Flat; btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) =>
            {
                string sid      = fields[0].Text.Trim();
                string code     = fields[1].Text.Trim();
                string midStr   = fields[2].Text.Trim();
                string finStr   = fields[3].Text.Trim();
                string semester = fields[4].Text.Trim();

                if (string.IsNullOrEmpty(sid) || string.IsNullOrEmpty(code) || string.IsNullOrEmpty(semester))
                { MessageBox.Show("Student ID, Course Code and Semester are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (!int.TryParse(midStr, out int mid) || mid < 0 || mid > 50)
                { MessageBox.Show("Midterm must be 0–50.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (!int.TryParse(finStr, out int fin) || fin < 0 || fin > 50)
                { MessageBox.Show("Final must be 0–50.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                int total = mid + fin;
                string grade = total >= 90 ? "A+" : total >= 85 ? "A" : total >= 80 ? "B+" :
                               total >= 75 ? "B"  : total >= 70 ? "C+" : total >= 65 ? "C" :
                               total >= 60 ? "D"  : "F";
                try
                {
                    DatabaseHelper.Execute(
                        "INSERT INTO Grades (StudentId,CourseCode,Midterm,Final,Total,Grade,Semester) " +
                        "VALUES (@sid,@cc,@mid,@fin,@tot,@gr,@sem);",
                        new SQLiteParameter("@sid", sid),
                        new SQLiteParameter("@cc",  code),
                        new SQLiteParameter("@mid", mid),
                        new SQLiteParameter("@fin", fin),
                        new SQLiteParameter("@tot", total),
                        new SQLiteParameter("@gr",  grade),
                        new SQLiteParameter("@sem", semester));
                    MessageBox.Show($"Grade added! Calculated grade: {grade}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dlg.Close();
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message, "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            Button btnCancel = new Button();
            btnCancel.Text = "Cancel"; btnCancel.Location = new Point(200, 330); btnCancel.Size = new Size(150, 36);
            btnCancel.Font = new Font("Segoe UI", 10); btnCancel.BackColor = Color.FromArgb(240, 240, 240);
            btnCancel.FlatStyle = FlatStyle.Flat; btnCancel.Click += (s, e) => dlg.Close();

            dlg.Controls.Add(btnSave); dlg.Controls.Add(btnCancel);
            dlg.ShowDialog();
        }
    }
}
