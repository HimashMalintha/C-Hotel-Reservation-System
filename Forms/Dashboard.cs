using System;
using System.Drawing;
using System.Windows.Forms;
using Hotel_Reservation.Models;

namespace Hotel_Reservation
{
    public partial class Dashboard : Form
    {
        private Panel panelHeader, panelSidebar, panelMain;
        private Label labelTitle;
        private Button buttonRooms, buttonReservations, buttonCustomers, buttonFeedback, buttonLogout;

        public Dashboard()
        {
            this.Text = "Admin Dashboard - Hotel Management";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = Color.WhiteSmoke;

            InitializeDashboardUI();
        }

        private void InitializeDashboardUI()
        {
            // 🔹 **Header Panel**
            panelHeader = new Panel
            {
                Size = new Size(900, 80),
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(0, 102, 204) // Blue Header
            };
            this.Controls.Add(panelHeader);

            labelTitle = new Label
            {
                Text = "Admin Dashboard",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(330, 20)
            };
            panelHeader.Controls.Add(labelTitle);

            // 🔹 **Sidebar Panel**
            panelSidebar = new Panel
            {
                Size = new Size(250, 600),
                Dock = DockStyle.Left,
                BackColor = Color.FromArgb(40, 40, 40)
            };
            this.Controls.Add(panelSidebar);

            // 🔹 Sidebar Buttons
            buttonRooms = CreateSidebarButton("Manage Rooms", 100);
            buttonRooms.Click += ButtonRooms_Click;

            buttonReservations = CreateSidebarButton("Manage Reservations", 160);
            buttonReservations.Click += ButtonReservations_Click;

            buttonCustomers = CreateSidebarButton("Manage Customers", 220);
            buttonCustomers.Click += ButtonCustomers_Click;

            buttonFeedback = CreateSidebarButton("Manage Feedback", 280);
            buttonFeedback.Click += ButtonFeedback_Click;

            buttonLogout = CreateSidebarButton("Logout", 380, Color.Red);
            buttonLogout.Click += ButtonLogout_Click;

            panelSidebar.Controls.Add(buttonRooms);
            panelSidebar.Controls.Add(buttonReservations);
            panelSidebar.Controls.Add(buttonCustomers);
            panelSidebar.Controls.Add(buttonFeedback);
            panelSidebar.Controls.Add(buttonLogout);

            // 🔹 **Main Content Panel**
            panelMain = new Panel
            {
                Size = new Size(650, 520),
                Location = new Point(250, 80),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(panelMain);

            Label labelMainContent = new Label
            {
                Text = "Dashboard Overview",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(230, 20),
                AutoSize = true
            };
            panelMain.Controls.Add(labelMainContent);
        }

        // 🔹 **Sidebar Button Creator**
        private Button CreateSidebarButton(string text, int top, Color? bgColor = null)
        {
            Button button = new Button
            {
                Text = text,
                Size = new Size(230, 50),
                Location = new Point(10, top),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = bgColor ?? Color.FromArgb(0, 123, 255),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 70);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(100, 100, 100);

            return button;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Name = "Dashboard";
            this.Load += new System.EventHandler(this.Dashboard_Load);
            this.ResumeLayout(false);
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
        }

        // ✅ **Event Handlers for Sidebar Buttons**
        private void ButtonRooms_Click(object sender, EventArgs e)
        {
            AddRoom addRoomForm = new AddRoom();
            addRoomForm.ShowDialog(); // Open Add Room form
        }

        private void ButtonReservations_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Reservations Management Page (To Be Implemented)", "Reservations", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ButtonCustomers_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Customers Management Page (To Be Implemented)", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ButtonFeedback_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Feedback Management Page (To Be Implemented)", "Feedback", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ButtonLogout_Click(object sender, EventArgs e)
        {
            // ✅ Navigate back to Login Form
            Form1 loginForm = new Form1();
            loginForm.Show();

            // Close the Dashboard
            this.Close();
        }
    }
}
