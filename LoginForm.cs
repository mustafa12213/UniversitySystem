using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SQLite;

namespace UniversitySystem
{
    public class LoginForm : Form
    {
        private Panel leftPanel;
        private Panel rightPanel;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblWelcome;
        private Label lblDesc;
        private Label lblUsername;
        private Label lblPassword;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblError;
        private CheckBox chkShowPassword;

        public LoginForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "UniSystem - Login";
            this.Size = new Size(850, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // ── LEFT PANEL ──────────────────────────────────────────
            leftPanel = new Panel();
            leftPanel.Size = new Size(380, 520);
            leftPanel.Location = new Point(0, 0);
            leftPanel.BackColor = Color.FromArgb(24, 95, 165);

            lblTitle = new Label();
            lblTitle.Text = "UniSystem";
            lblTitle.Font = new Font("Segoe UI", 28, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(40, 160);
            lblTitle.AutoSize = true;

            lblSubtitle = new Label();
            lblSubtitle.Text = "University Management Portal";
            lblSubtitle.Font = new Font("Segoe UI", 11);
            lblSubtitle.ForeColor = Color.FromArgb(180, 210, 240);
            lblSubtitle.Location = new Point(40, 210);
            lblSubtitle.AutoSize = true;

            lblDesc = new Label();
            lblDesc.Text = "Manage students, courses,\nprofessors and grades\nall in one place.";
            lblDesc.Font = new Font("Segoe UI", 10);
            lblDesc.ForeColor = Color.FromArgb(200, 225, 250);
            lblDesc.Location = new Point(40, 270);
            lblDesc.AutoSize = true;

            leftPanel.Controls.Add(lblTitle);
            leftPanel.Controls.Add(lblSubtitle);
            leftPanel.Controls.Add(lblDesc);

            // ── RIGHT PANEL ─────────────────────────────────────────
            rightPanel = new Panel();
            rightPanel.Size = new Size(470, 520);
            rightPanel.Location = new Point(380, 0);
            rightPanel.BackColor = Color.White;

            lblWelcome = new Label();
            lblWelcome.Text = "Welcome Back!";
            lblWelcome.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblWelcome.ForeColor = Color.FromArgb(12, 68, 124);
            lblWelcome.Location = new Point(60, 80);
            lblWelcome.AutoSize = true;

            Label lblSignIn = new Label();
            lblSignIn.Text = "Sign in to your account";
            lblSignIn.Font = new Font("Segoe UI", 10);
            lblSignIn.ForeColor = Color.Gray;
            lblSignIn.Location = new Point(60, 120);
            lblSignIn.AutoSize = true;

            lblUsername = new Label();
            lblUsername.Text = "Username";
            lblUsername.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblUsername.ForeColor = Color.FromArgb(60, 60, 60);
            lblUsername.Location = new Point(60, 170);
            lblUsername.AutoSize = true;

            txtUsername = new TextBox();
            txtUsername.Location = new Point(60, 192);
            txtUsername.Size = new Size(350, 35);
            txtUsername.Font = new Font("Segoe UI", 10);
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Text = "admin";

            lblPassword = new Label();
            lblPassword.Text = "Password";
            lblPassword.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblPassword.ForeColor = Color.FromArgb(60, 60, 60);
            lblPassword.Location = new Point(60, 245);
            lblPassword.AutoSize = true;

            txtPassword = new TextBox();
            txtPassword.Location = new Point(60, 267);
            txtPassword.Size = new Size(350, 35);
            txtPassword.Font = new Font("Segoe UI", 10);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.PasswordChar = '*';
            txtPassword.Text = "1234";

            chkShowPassword = new CheckBox();
            chkShowPassword.Text = "Show Password";
            chkShowPassword.Font = new Font("Segoe UI", 9);
            chkShowPassword.ForeColor = Color.Gray;
            chkShowPassword.Location = new Point(60, 308);
            chkShowPassword.AutoSize = true;
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '*';
            };

            lblError = new Label();
            lblError.Text = "";
            lblError.Font = new Font("Segoe UI", 9);
            lblError.ForeColor = Color.Red;
            lblError.Location = new Point(60, 335);
            lblError.AutoSize = true;

            btnLogin = new Button();
            btnLogin.Text = "Sign In";
            btnLogin.Location = new Point(60, 360);
            btnLogin.Size = new Size(350, 42);
            btnLogin.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnLogin.BackColor = Color.FromArgb(24, 95, 165);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Click += BtnLogin_Click;

            // FIX #6: Remove the plaintext hint — password is now hashed
            Label lblHint = new Label();
            lblHint.Text = "Default credentials: admin / 1234";
            lblHint.Font = new Font("Segoe UI", 8);
            lblHint.ForeColor = Color.LightGray;
            lblHint.Location = new Point(60, 415);
            lblHint.AutoSize = true;

            rightPanel.Controls.Add(lblWelcome);
            rightPanel.Controls.Add(lblSignIn);
            rightPanel.Controls.Add(lblUsername);
            rightPanel.Controls.Add(txtUsername);
            rightPanel.Controls.Add(lblPassword);
            rightPanel.Controls.Add(txtPassword);
            rightPanel.Controls.Add(chkShowPassword);
            rightPanel.Controls.Add(lblError);
            rightPanel.Controls.Add(btnLogin);
            rightPanel.Controls.Add(lblHint);

            this.Controls.Add(leftPanel);
            this.Controls.Add(rightPanel);

            this.AcceptButton = btnLogin;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // FIX #6: Compare against the hashed password, not the plain text.
            string hashed = DatabaseHelper.HashPassword(password);
            object result = DatabaseHelper.Scalar(
                "SELECT COUNT(*) FROM Users WHERE Username = @u AND Password = @p;",
                new SQLiteParameter("@u", username),
                new SQLiteParameter("@p", hashed));

            if (result != null && Convert.ToInt32(result) > 0)
            {
                // FIX #5: Pass 'this' (the LoginForm) to MainForm so it can properly
                // close the LoginForm on logout without leaking windows.
                MainForm mainForm = new MainForm(this);
                mainForm.Show();
                this.Hide();
            }
            else
            {
                lblError.Text = "Invalid username or password!";
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
    }
}
