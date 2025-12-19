using System;
using System.Drawing;
using System.Windows.Forms;
using LostAndFoundSystem.BLL;

namespace LostAndFoundSystem.UI
{
    /// <summary>
    /// Login form for user authentication
    /// </summary>
    public partial class LoginForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblUsername;
        private Label lblPassword;
        private Label lblTitle;
        private AuthService _authService;

        public LoginForm()
        {
            InitializeComponents();
            _authService = new AuthService();
        }

        private void InitializeComponents()
        {
            // Form settings
            this.Text = "Campus Lost & Found - Login";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Title label
            lblTitle = new Label
            {
                Text = "Campus Lost & Found System",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(50, 20),
                Size = new Size(300, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(0, 122, 204)
            };

            // Username label
            lblUsername = new Label
            {
                Text = "Username:",
                Location = new Point(50, 80),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 10)
            };

            // Username textbox
            txtUsername = new TextBox
            {
                Location = new Point(50, 105),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 10)
            };

            // Password label
            lblPassword = new Label
            {
                Text = "Password:",
                Location = new Point(50, 140),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 10)
            };

            // Password textbox
            txtPassword = new TextBox
            {
                Location = new Point(50, 165),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 10),
                PasswordChar = '●'
            };

            // Login button
            btnLogin = new Button
            {
                Text = "Login",
                Location = new Point(125, 210),
                Size = new Size(150, 35),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            // Add controls to form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);

            // Set enter key to trigger login
            this.AcceptButton = btnLogin;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", 
                    "Validation Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }

            // Attempt login
            if (_authService.Login(username, password, out string errorMessage))
            {
                // Login successful
                this.Hide();
                DashboardForm dashboard = new DashboardForm();
                dashboard.FormClosed += (s, args) => this.Close();
                dashboard.Show();
            }
            else
            {
                // Login failed
                MessageBox.Show(errorMessage, 
                    "Login Failed", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
    }
}
