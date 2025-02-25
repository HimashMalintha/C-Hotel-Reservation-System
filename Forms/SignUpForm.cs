using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Hotel_Reservation.Models;

namespace Hotel_Reservation
{
    public partial class SignUpForm : Form
    {
        private Label labelTitle, labelName, labelAddress, labelContact, labelEmail, labelPassword;
        private TextBox textBoxName, textBoxAddress, textBoxContact, textBoxEmail, textBoxPassword;
        private Button buttonRegister, buttonBack;
        private DatabaseHelper db;

        public SignUpForm()
        {
            this.Text = "Hotel Reservation - Sign Up";
            this.Size = new Size(400, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = Color.WhiteSmoke;

            db = new DatabaseHelper(); // Initialize DB Helper

            InitializeSignUpUI();
        }

        private void InitializeSignUpUI()
        {
            labelTitle = new Label
            {
                Text = "Sign Up",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                AutoSize = true,
                Location = new Point(150, 20)
            };
            this.Controls.Add(labelTitle);

            labelName = new Label { Text = "Full Name:", Location = new Point(30, 70), AutoSize = true };
            textBoxName = new TextBox { Location = new Point(140, 70), Width = 200 };
            this.Controls.Add(labelName);
            this.Controls.Add(textBoxName);

            labelAddress = new Label { Text = "Address:", Location = new Point(30, 110), AutoSize = true };
            textBoxAddress = new TextBox { Location = new Point(140, 110), Width = 200 };
            this.Controls.Add(labelAddress);
            this.Controls.Add(textBoxAddress);

            labelContact = new Label { Text = "Contact No:", Location = new Point(30, 150), AutoSize = true };
            textBoxContact = new TextBox { Location = new Point(140, 150), Width = 200 };
            textBoxContact.KeyPress += TextBoxContact_KeyPress;
            this.Controls.Add(labelContact);
            this.Controls.Add(textBoxContact);

            labelEmail = new Label { Text = "Email:", Location = new Point(30, 190), AutoSize = true };
            textBoxEmail = new TextBox { Location = new Point(140, 190), Width = 200 };
            this.Controls.Add(labelEmail);
            this.Controls.Add(textBoxEmail);

            labelPassword = new Label { Text = "Password:", Location = new Point(30, 230), AutoSize = true };
            textBoxPassword = new TextBox { Location = new Point(140, 230), Width = 200, UseSystemPasswordChar = true };
            this.Controls.Add(labelPassword);
            this.Controls.Add(textBoxPassword);

            buttonRegister = new Button
            {
                Text = "Register",
                Size = new Size(200, 40),
                Location = new Point(100, 280),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            buttonRegister.Click += ButtonRegister_Click;
            this.Controls.Add(buttonRegister);

            buttonBack = new Button
            {
                Text = "Back to Login",
                Size = new Size(200, 40),
                Location = new Point(100, 330),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            buttonBack.Click += ButtonBack_Click;
            this.Controls.Add(buttonBack);
        }

        // ✅ Validate Contact Number (Only 10 Digits)
        private void TextBoxContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // ✅ **Check if Email Exists Before Registering**
        private void ButtonRegister_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text.Trim();
            string address = textBoxAddress.Text.Trim();
            string contact = textBoxContact.Text.Trim();
            string email = textBoxEmail.Text.Trim();
            string password = textBoxPassword.Text.Trim();

            // 🔹 Check Empty Fields
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address) ||
                string.IsNullOrWhiteSpace(contact) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("❌ Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 🔹 Validate Contact Number (Must be exactly 10 digits)
            if (contact.Length != 10)
            {
                MessageBox.Show("❌ Contact number must be exactly 10 digits.", "Invalid Contact", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 🔹 Validate Email Format
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("❌ Please enter a valid email address.", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ **Check if Email Already Exists**
            if (db.CheckUserExists(email))
            {
                MessageBox.Show("❌ Email already registered! Please use a different email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 🔹 Create User Object
            User user = new User
            {
                Name = name,
                Address = address,
                Contact = contact,
                Email = email,
                Password = password // Store securely in DB
            };

            // 🔹 Register User in MongoDB
            db.RegisterUser(user);

            MessageBox.Show("✅ Registration Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // 🔹 Redirect to Login
            Form1 loginForm = new Form1();
            this.Hide();
            loginForm.Show();
        }

        // 🔹 Back to Login Button Click
        private void ButtonBack_Click(object sender, EventArgs e)
        {
            Form1 loginForm = new Form1();
            this.Hide();
            loginForm.Show();
        }
    }
}
