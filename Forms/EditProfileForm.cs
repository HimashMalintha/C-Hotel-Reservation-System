using System;
using System.Drawing;
using System.Windows.Forms;
using Hotel_Reservation.Models;

namespace Hotel_Reservation.Forms
{
    public partial class EditProfileForm : Form
    {
        private Panel panelHeader, panelContainer;
        private Label labelTitle, labelUserId, labelName, labelAddress, labelContact, labelEmail, labelPassword;
        private TextBox textBoxUserId, textBoxName, textBoxAddress, textBoxContact, textBoxEmail, textBoxPassword;
        private Button buttonUpdate, buttonCancel, buttonTogglePassword;
        private DatabaseHelper db;
        private string userEmail;

        public EditProfileForm(string email)
        {
            userEmail = email;
            db = new DatabaseHelper();
            InitializeComponent();
            LoadUserData();
        }

        private void InitializeComponent()
        {
            // 🔹 **Increase Form Size**
            this.ClientSize = new Size(750, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Text = "Edit Profile";

            // 🔹 **Header Panel**
            panelHeader = new Panel
            {
                Size = new Size(750, 90),
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(30, 144, 255)
            };
            this.Controls.Add(panelHeader);

            labelTitle = new Label
            {
                Text = "Edit Your Profile",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(230, 25)
            };
            panelHeader.Controls.Add(labelTitle);

            // 🔹 **Container Panel (Reduce height slightly)**
            panelContainer = new Panel
            {
                Size = new Size(700, 450),
                Location = new Point(25, 110),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(panelContainer);

            // 🔹 **Labels & Textboxes**
            labelUserId = CreateLabel("User ID:", 40);
            textBoxUserId = CreateTextBox(250, 35, true);

            labelName = CreateLabel("Full Name:", 90);
            textBoxName = CreateTextBox(250, 85);

            labelAddress = CreateLabel("Address:", 140);
            textBoxAddress = CreateTextBox(250, 135);

            labelContact = CreateLabel("Contact No:", 190);
            textBoxContact = CreateTextBox(250, 185);

            labelEmail = CreateLabel("Email:", 240);
            textBoxEmail = CreateTextBox(250, 235, true);

            labelPassword = CreateLabel("Password:", 290);
            textBoxPassword = CreateTextBox(250, 285);
            textBoxPassword.UseSystemPasswordChar = true; // ✅ **Password hidden on load**

            // 🔹 **Toggle Password Visibility Button**
            buttonTogglePassword = new Button
            {
                Text = "👁",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(35, 30),
                Location = new Point(560, 285),
                BackColor = Color.LightGray,
                FlatStyle = FlatStyle.Flat
            };
            buttonTogglePassword.FlatAppearance.BorderSize = 0;
            buttonTogglePassword.Click += TogglePasswordVisibility;
            panelContainer.Controls.Add(buttonTogglePassword);

            // 🔹 **Update & Cancel Buttons (Now properly positioned)**
            buttonUpdate = CreateButton("Update Info", 190, 580, Color.FromArgb(40, 167, 69));
            buttonUpdate.Click += ButtonUpdate_Click;
            this.Controls.Add(buttonUpdate);

            buttonCancel = CreateButton("Cancel", 380, 580, Color.FromArgb(220, 53, 69));
            buttonCancel.Click += ButtonCancel_Click;
            this.Controls.Add(buttonCancel);
        }

        // 🔹 **Helper Methods for Creating Labels & TextBoxes**
        private Label CreateLabel(string text, int top)
        {
            Label label = new Label
            {
                Text = text,
                Location = new Point(60, top),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true
            };
            panelContainer.Controls.Add(label);
            return label;
        }

        private TextBox CreateTextBox(int left, int top, bool isReadOnly = false)
        {
            TextBox textBox = new TextBox
            {
                Location = new Point(left, top),
                Width = 320,
                Font = new Font("Segoe UI", 12),
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = isReadOnly,
                BackColor = isReadOnly ? Color.LightGray : Color.White,
                ForeColor = Color.Black,
                Padding = new Padding(5)
            };
            panelContainer.Controls.Add(textBox);
            return textBox;
        }

        private Button CreateButton(string text, int left, int top, Color bgColor)
        {
            Button button = new Button
            {
                Text = text,
                Size = new Size(180, 50),
                Location = new Point(left, top),
                BackColor = bgColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            button.MouseEnter += (sender, e) => button.BackColor = Color.DarkSlateGray;
            button.MouseLeave += (sender, e) => button.BackColor = bgColor;

            return button;
        }

        // ✅ **Toggle Password Visibility**
        private void TogglePasswordVisibility(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = !textBoxPassword.UseSystemPasswordChar;
        }

        // ✅ **Load User Data from MongoDB**
        private void LoadUserData()
        {
            User user = db.GetUserByEmail(userEmail);
            if (user != null)
            {
                textBoxUserId.Text = user.UserId;
                textBoxName.Text = user.Name;
                textBoxAddress.Text = user.Address;
                textBoxContact.Text = user.Contact;
                textBoxEmail.Text = user.Email;
                textBoxPassword.Text = user.Password;
            }
            else
            {
                MessageBox.Show("❌ User not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ **Update User Info**
        private void ButtonUpdate_Click(object sender, EventArgs e)
        {
            // 🔹 **Validate Inputs**
            if (string.IsNullOrWhiteSpace(textBoxName.Text) ||
                string.IsNullOrWhiteSpace(textBoxAddress.Text) ||
                string.IsNullOrWhiteSpace(textBoxContact.Text) ||
                string.IsNullOrWhiteSpace(textBoxPassword.Text))
            {
                MessageBox.Show("❌ Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ **Ensure Contact is 10 Digits**
            if (textBoxContact.Text.Length != 10 || !long.TryParse(textBoxContact.Text, out _))
            {
                MessageBox.Show("❌ Contact number must be exactly 10 digits.", "Invalid Contact", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            User updatedUser = new User
            {
                UserId = textBoxUserId.Text,
                Name = textBoxName.Text,
                Address = textBoxAddress.Text,
                Contact = textBoxContact.Text,
                Email = textBoxEmail.Text,
                Password = textBoxPassword.Text
            };

            db.UpdateUser(updatedUser);
            MessageBox.Show("✅ User info updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ✅ **Cancel & Navigate Back to Dashboard**
        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            new CustomerDashboard(textBoxName.Text, userEmail).Show();
        }
    }
}
