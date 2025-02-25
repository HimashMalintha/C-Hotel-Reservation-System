using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Hotel_Reservation.Models;

namespace Hotel_Reservation.Forms
{
    public partial class FeedbackForm : Form
    {
        private string customerEmail;
        private DatabaseHelper db;
        private DataGridView dgvReservations, dgvFeedbacks;
        private Label lblSelectedReservation, lblRating, lblFeedback, lblFeedbackList;
        private ComboBox cmbRating;
        private TextBox txtFeedback;
        private Button btnSubmit, btnBack;
        private Panel panelReservations, panelFeedbackList;

        private string selectedReservationId = null; // ✅ Store Selected Reservation ID

        public FeedbackForm(string email)
        {
            customerEmail = email;
            db = new DatabaseHelper();
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(1000, 750);
            this.Name = "FeedbackForm";
            this.Text = "Add Feedback";
            this.Load += FeedbackForm_Load;
            this.ResumeLayout(false);
        }

        private void InitializeUI()
        {
            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10);

            // 🔹 Header Label
            Label lblTitle = new Label
            {
                Text = "Your Past Reservations",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            // 🔹 Reservations Panel (Scrollable)
            panelReservations = new Panel
            {
                Location = new Point(20, 60),
                Size = new Size(950, 250),
                AutoScroll = true
            };

            // 🔹 Reservations DataGridView
            dgvReservations = new DataGridView
            {
                Size = new Size(1200, 250),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.Fixed3D,
                MultiSelect = false,
                ScrollBars = ScrollBars.Both
            };
            dgvReservations.SelectionChanged += DgvReservations_SelectionChanged;

            panelReservations.Controls.Add(dgvReservations);

            // 🔹 Selected Reservation Label
            lblSelectedReservation = new Label
            {
                Text = "Selected Reservation: None",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 330)
            };

            // 🔹 Rating Label & ComboBox
            lblRating = new Label { Text = "Rating:", Location = new Point(20, 370), AutoSize = true };
            cmbRating = new ComboBox { Location = new Point(150, 370), Width = 100 };
            cmbRating.Items.AddRange(new object[] { "1", "2", "3", "4", "5" });

            // 🔹 Feedback Label & TextBox
            lblFeedback = new Label { Text = "Your Feedback:", Location = new Point(20, 410), AutoSize = true };
            txtFeedback = new TextBox
            {
                Location = new Point(150, 410),
                Size = new Size(700, 100),
                Multiline = true
            };

            // 🔹 Submit Button
            btnSubmit = new Button
            {
                Text = "Submit Feedback",
                Location = new Point(150, 530),
                Size = new Size(180, 50),
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            btnSubmit.Click += BtnSubmitFeedback_Click;

            // 🔹 Back Button
            btnBack = new Button
            {
                Text = "Back",
                Location = new Point(350, 530),
                Size = new Size(150, 50),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            btnBack.Click += BtnBack_Click;

            // 🔹 Feedback List Header
            lblFeedbackList = new Label
            {
                Text = "Your Submitted Feedback",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 600)
            };

            // 🔹 Feedback List Panel (Scrollable)
            panelFeedbackList = new Panel
            {
                Location = new Point(20, 630),
                Size = new Size(950, 250),
                AutoScroll = true
            };

            // 🔹 Feedbacks DataGridView
            dgvFeedbacks = new DataGridView
            {
                Size = new Size(900, 250),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.Fixed3D,
                MultiSelect = false
            };

            panelFeedbackList.Controls.Add(dgvFeedbacks);

            // 🔹 Add Controls to Form
            this.Controls.Add(lblTitle);
            this.Controls.Add(panelReservations);
            this.Controls.Add(lblSelectedReservation);
            this.Controls.Add(lblRating);
            this.Controls.Add(cmbRating);
            this.Controls.Add(lblFeedback);
            this.Controls.Add(txtFeedback);
            this.Controls.Add(btnSubmit);
            this.Controls.Add(btnBack);
            this.Controls.Add(lblFeedbackList);
            this.Controls.Add(panelFeedbackList);
        }

        private void FeedbackForm_Load(object sender, EventArgs e)
        {
            LoadReservations();
            LoadFeedbacks();
        }

        private void LoadReservations()
        {
            List<Reservation> reservations = db.GetReservations().Where(r => r.CustomerEmail == customerEmail).ToList();
            dgvReservations.DataSource = reservations;

            if (dgvReservations.Columns.Count > 0)
            {
                dgvReservations.Columns["Id"].Visible = false;
                dgvReservations.Columns["ReservationId"].Width = 120;
                dgvReservations.Columns["CustomerEmail"].Width = 180;
                dgvReservations.Columns["RoomNo"].Width = 80;
                dgvReservations.Columns["RoomType"].Width = 120;
                dgvReservations.Columns["CheckInDate"].Width = 200;
                dgvReservations.Columns["CheckOutDate"].Width = 200;
                dgvReservations.Columns["TotalPrice"].Width = 150;
                dgvReservations.Columns["BookingDate"].Width = 200;
            }
        }

        private void LoadFeedbacks()
        {
            if (dgvFeedbacks == null)
                return;

            List<Feedback> feedbacks = db.GetFeedbacksByEmail(customerEmail);
            dgvFeedbacks.DataSource = feedbacks;

            if (dgvFeedbacks.Columns.Count > 0)
            {
                dgvFeedbacks.Columns["Id"].Visible = false;
                dgvFeedbacks.Columns["ReservationId"].Width = 120;
                dgvFeedbacks.Columns["CustomerEmail"].Width = 200;
                dgvFeedbacks.Columns["Message"].Width = 400;
                dgvFeedbacks.Columns["Rating"].Width = 100;
            }
        }

        private void DgvReservations_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReservations.SelectedRows.Count > 0)
            {
                selectedReservationId = dgvReservations.SelectedRows[0].Cells["ReservationId"].Value.ToString();
                lblSelectedReservation.Text = $"Selected Reservation: {selectedReservationId}";
            }
        }

        private void BtnSubmitFeedback_Click(object sender, EventArgs e)
        {
            if (cmbRating.SelectedItem == null || string.IsNullOrWhiteSpace(txtFeedback.Text) || string.IsNullOrEmpty(selectedReservationId))
            {
                MessageBox.Show("❌ Please select a reservation, enter feedback, and select a rating!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Feedback feedback = new Feedback
            {
                ReservationId = selectedReservationId, // ✅ Add ReservationId to feedback
                CustomerEmail = customerEmail,
                Message = txtFeedback.Text,
                Rating = int.Parse(cmbRating.SelectedItem.ToString())
            };

            db.AddFeedback(feedback);
            MessageBox.Show("✅ Feedback submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadFeedbacks();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            CustomerDashboard dashboard = new CustomerDashboard("Customer", customerEmail);
            this.Hide();
            dashboard.Show();
        }
    }
}
