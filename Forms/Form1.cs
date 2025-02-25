using System;
using System.Drawing;
using System.Windows.Forms;
using Hotel_Reservation.Forms;
using Hotel_Reservation.Models;

namespace Hotel_Reservation
{
    public partial class Form1 : Form
    {
        private Label labelTitle, labelEmail, labelPassword;
        private TextBox textBoxEmail, textBoxPassword;
        private Button buttonLogin, buttonShowPassword, buttonSignUp;
        private DatabaseHelper db;

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(400, 350);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public Form1()
        {
            this.Text = "Hotel Reservation - Login";
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = Color.WhiteSmoke;

            db = new DatabaseHelper(); // ✅ Initialize Database Helper

            InitializeLoginUI();
        }

        private void InitializeLoginUI()
        {
            labelTitle = new Label
            {
                Text = "Login",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50),
                AutoSize = true,
                Location = new Point(160, 20)
            };
            this.Controls.Add(labelTitle);

            labelEmail = new Label
            {
                Text = "Email:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(50, 80),
                AutoSize = true
            };
            this.Controls.Add(labelEmail);

            textBoxEmail = new TextBox
            {
                Size = new Size(200, 25),
                Location = new Point(140, 75),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(textBoxEmail);

            labelPassword = new Label
            {
                Text = "Password:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(50, 130),
                AutoSize = true
            };
            this.Controls.Add(labelPassword);

            textBoxPassword = new TextBox
            {
                Size = new Size(170, 25),
                Location = new Point(140, 125),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle,
                UseSystemPasswordChar = true
            };
            this.Controls.Add(textBoxPassword);

            buttonShowPassword = new Button
            {
                Text = "👁",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(30, 25),
                Location = new Point(310, 125),
                BackColor = Color.WhiteSmoke,
                FlatStyle = FlatStyle.Flat
            };
            buttonShowPassword.FlatAppearance.BorderSize = 0;
            buttonShowPassword.Click += ButtonShowPassword_Click;
            this.Controls.Add(buttonShowPassword);

            buttonLogin = new Button
            {
                Text = "Sign In",
                Size = new Size(200, 40),
                Location = new Point(140, 180),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            buttonLogin.FlatAppearance.BorderSize = 0;
            buttonLogin.Click += ButtonLogin_Click;
            this.Controls.Add(buttonLogin);

            buttonSignUp = new Button
            {
                Text = "Sign Up",
                Size = new Size(200, 40),
                Location = new Point(140, 230),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            buttonSignUp.FlatAppearance.BorderSize = 0;
            buttonSignUp.Click += ButtonSignUp_Click;
            this.Controls.Add(buttonSignUp);
        }

        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            string email = textBoxEmail.Text.Trim();
            string password = textBoxPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("❌ Please enter both email and password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ **Admin Login Condition**
            if (email == "admin@gmail.com" && password == "1234")
            {
                MessageBox.Show("✅ Admin Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 🔹 Open **Admin Dashboard**
                Dashboard adminDashboard = new Dashboard();
                this.Hide();
                adminDashboard.Show();
                return;
            }

            // ✅ **Fetch user details from MongoDB**
            User authenticatedUser = db.AuthenticateUser(email, password);

            if (authenticatedUser != null)  // 🔹 Check if user exists
            {
                string customerName = authenticatedUser.Name; // Fetch customer name

                MessageBox.Show("✅ Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 🔹 Open **Customer Dashboard**
                CustomerDashboard customerDashboard = new CustomerDashboard(customerName, email);
                this.Hide();
                customerDashboard.Show();
            }
            else
            {
                MessageBox.Show("❌ Invalid Email or Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonShowPassword_Click(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = !textBoxPassword.UseSystemPasswordChar;
        }

        private void ButtonSignUp_Click(object sender, EventArgs e)
        {
            SignUpForm signUpForm = new SignUpForm();
            this.Hide();
            signUpForm.Show();
        }
    }
}
