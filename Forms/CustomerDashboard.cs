using System;
using System.Drawing;
using System.Windows.Forms;
using Hotel_Reservation.Models;
using Hotel_Reservation.Forms;  // Ensure BookRoom & FeedbackForm are in this namespace

namespace Hotel_Reservation.Forms
{
    public partial class CustomerDashboard : Form
    {
        private Panel panelHeader, panelSidebar, panelContent;
        private Label labelWelcome;
        private Button buttonBookRoom, buttonAddFeedback, buttonEditProfile, buttonLogout;
        private string customerName, customerEmail;

        public CustomerDashboard(string name, string email)
        {
            customerName = name;
            customerEmail = email;
            InitializeUI(); // Initialize UI components
        }

        private void CustomerDashboard_Load(object sender, EventArgs e)
        {
            labelWelcome.Text = $"Welcome, {customerName}!";
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Name = "CustomerDashboard";
            this.Load += new System.EventHandler(this.CustomerDashboard_Load);
            this.ResumeLayout(false);
        }

        private void InitializeUI()
        {
            // 🔹 Form Settings
            this.Text = "Customer Dashboard";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // 🔹 Header Panel
            panelHeader = new Panel
            {
                Size = new Size(this.Width, 60),
                BackColor = Color.DarkBlue,
                Dock = DockStyle.Top
            };
            labelWelcome = new Label
            {
                Text = "Welcome, " + customerName,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 15)
            };
            panelHeader.Controls.Add(labelWelcome);

            // 🔹 Sidebar Panel
            panelSidebar = new Panel
            {
                Size = new Size(180, this.Height),
                BackColor = Color.FromArgb(40, 44, 52),
                Dock = DockStyle.Left
            };

            // 🔹 Sidebar Buttons
            buttonBookRoom = CreateSidebarButton("Book Room", 80);
            buttonAddFeedback = CreateSidebarButton("Add Feedback", 130);
            buttonEditProfile = CreateSidebarButton("Edit Profile", 180);
            buttonLogout = CreateSidebarButton("Logout", 230, Color.DarkRed);

            // 🔹 Event Handlers
            buttonBookRoom.Click += ButtonBookRoom_Click;
            buttonAddFeedback.Click += ButtonAddFeedback_Click;
            buttonEditProfile.Click += ButtonEditProfile_Click;
            buttonLogout.Click += ButtonLogout_Click;

            // 🔹 Add buttons to Sidebar
            panelSidebar.Controls.Add(buttonBookRoom);
            panelSidebar.Controls.Add(buttonAddFeedback);
            panelSidebar.Controls.Add(buttonEditProfile);
            panelSidebar.Controls.Add(buttonLogout);

            // 🔹 Content Panel (Placeholder for Forms)
            panelContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            // 🔹 Add panels to Form
            this.Controls.Add(panelHeader);
            this.Controls.Add(panelSidebar);
            this.Controls.Add(panelContent);
        }

        // 🔹 Sidebar Button Creator
        private Button CreateSidebarButton(string text, int top, Color? bgColor = null)
        {
            return new Button
            {
                Text = text,
                Size = new Size(160, 40),
                Location = new Point(10, top),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = bgColor ?? Color.FromArgb(40, 167, 69),
                FlatStyle = FlatStyle.Flat
            };
        }

        // ✅ **Navigate to Book Room Form**
        private void ButtonBookRoom_Click(object sender, EventArgs e)
        {
            BookRoom bookRoom = new BookRoom(customerEmail);
            this.Hide();
            bookRoom.Show();
        }

        // ✅ **Navigate to Add Feedback Form**
        private void ButtonAddFeedback_Click(object sender, EventArgs e)
        {
            FeedbackForm feedbackForm = new FeedbackForm(customerEmail);
            this.Hide();
            feedbackForm.Show();
        }

        // ✅ **Navigate to Edit Profile Form**
        private void ButtonEditProfile_Click(object sender, EventArgs e)
        {
            EditProfileForm editProfile = new EditProfileForm(customerEmail);
            this.Hide();
            editProfile.Show();
        }

        // ✅ **Logout Navigates to Login Page**
        private void ButtonLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                Form1 loginForm = new Form1();
                loginForm.Show();
            }
        }
    }
}