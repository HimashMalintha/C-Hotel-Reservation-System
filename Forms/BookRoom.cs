using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Hotel_Reservation.Models;
using MongoDB.Driver;

namespace Hotel_Reservation.Forms
{
    public partial class BookRoom : Form
    {
        private string customerEmail;
        private DatabaseHelper db;
        private ComboBox comboRoomType, comboRoomNo;
        private DateTimePicker dateCheckIn, dateCheckOut;
        private TextBox txtTotalPrice, txtPricePerDay;
        private Button btnBook, btnBack;
        private Label lblReservationId;

        public BookRoom(string email)
        {
            customerEmail = email;
            db = new DatabaseHelper();
            InitializeComponent();
            InitializeUI();
            LoadRoomTypes();
            GenerateReservationId();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(600, 600);
            this.Name = "BookRoom";
            this.Load += new System.EventHandler(this.BookRoom_Load);
            this.ResumeLayout(false);
        }

        private void InitializeUI()
        {
            this.Text = "Book a Room";
            this.Size = new System.Drawing.Size(600, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245);

            Label lblTitle = new Label
            {
                Text = "Room Booking",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                AutoSize = true,
                Location = new Point(200, 20)
            };

            lblReservationId = new Label
            {
                Text = "Reservation ID: ",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.DarkRed,
                AutoSize = true,
                Location = new Point(50, 70)
            };

            Label lblRoomType = new Label { Text = "Room Type:", Location = new Point(50, 120), AutoSize = true, Font = new Font("Arial", 10) };
            comboRoomType = new ComboBox { Location = new Point(200, 120), Width = 300, Font = new Font("Arial", 10) };
            comboRoomType.SelectedIndexChanged += ComboRoomType_SelectedIndexChanged;

            Label lblRoomNo = new Label { Text = "Room No:", Location = new Point(50, 160), AutoSize = true, Font = new Font("Arial", 10) };
            comboRoomNo = new ComboBox { Location = new Point(200, 160), Width = 300, Font = new Font("Arial", 10) };
            comboRoomNo.SelectedIndexChanged += ComboRoomNo_SelectedIndexChanged;

            Label lblPricePerDay = new Label { Text = "Price Per Day:", Location = new Point(50, 200), AutoSize = true, Font = new Font("Arial", 10) };
            txtPricePerDay = new TextBox { Location = new Point(200, 200), Width = 300, ReadOnly = true, Font = new Font("Arial", 10) };

            Label lblCheckIn = new Label { Text = "Check-In Date:", Location = new Point(50, 240), AutoSize = true, Font = new Font("Arial", 10) };
            dateCheckIn = new DateTimePicker { Location = new Point(200, 240), Width = 300, Format = DateTimePickerFormat.Short, Font = new Font("Arial", 10) };
            dateCheckIn.MinDate = DateTime.Today;
            dateCheckIn.ValueChanged += DateCheckIn_ValueChanged;

            Label lblCheckOut = new Label { Text = "Check-Out Date:", Location = new Point(50, 280), AutoSize = true, Font = new Font("Arial", 10) };
            dateCheckOut = new DateTimePicker { Location = new Point(200, 280), Width = 300, Format = DateTimePickerFormat.Short, Font = new Font("Arial", 10) };
            dateCheckOut.MinDate = DateTime.Today.AddDays(1);
            dateCheckOut.ValueChanged += CalculateTotalPrice;

            Label lblTotalPrice = new Label { Text = "Total Price:", Location = new Point(50, 320), AutoSize = true, Font = new Font("Arial", 10) };
            txtTotalPrice = new TextBox { Location = new Point(200, 320), Width = 300, ReadOnly = true, Font = new Font("Arial", 10) };

            btnBook = new Button
            {
                Text = "Book Room",
                Location = new Point(200, 380),
                Size = new Size(140, 50),
                BackColor = Color.Green,
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnBook.Click += BtnBook_Click;

            btnBack = new Button
            {
                Text = "Back",
                Location = new Point(360, 380),
                Size = new Size(140, 50),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnBack.Click += BtnBack_Click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblReservationId);
            this.Controls.Add(lblRoomType);
            this.Controls.Add(comboRoomType);
            this.Controls.Add(lblRoomNo);
            this.Controls.Add(comboRoomNo);
            this.Controls.Add(lblPricePerDay);
            this.Controls.Add(txtPricePerDay);
            this.Controls.Add(lblCheckIn);
            this.Controls.Add(dateCheckIn);
            this.Controls.Add(lblCheckOut);
            this.Controls.Add(dateCheckOut);
            this.Controls.Add(lblTotalPrice);
            this.Controls.Add(txtTotalPrice);
            this.Controls.Add(btnBook);
            this.Controls.Add(btnBack);
     }
    



private void BookRoom_Load(object sender, EventArgs e) // ✅ Fixed missing function
        {
            // This function can be used for loading any additional data when the form loads
        }

        private void BtnBack_Click(object sender, EventArgs e) // ✅ Fixed missing function
        {
            CustomerDashboard dashboard = new CustomerDashboard("Customer", customerEmail);
            this.Hide();
            dashboard.Show();
        }

        private void LoadRoomTypes()
        {
            List<Room> rooms = db.GetRooms();
            comboRoomType.Items.Clear();

            foreach (var room in rooms.Select(r => r.Type).Distinct())
            {
                comboRoomType.Items.Add(room);
            }
        }

        private void GenerateReservationId()
        {
            var lastReservation = db.GetLastReservation();

            int newId = 1;
            if (lastReservation != null && lastReservation.ReservationId.StartsWith("Res"))
            {
                string lastIdStr = lastReservation.ReservationId.Replace("Res", "");
                if (int.TryParse(lastIdStr, out int lastNumericId))
                {
                    newId = lastNumericId + 1;
                }
            }

            lblReservationId.Text = $"Reservation ID: Res{newId}";
        }

        private void ComboRoomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedType = comboRoomType.SelectedItem.ToString();
            List<Room> rooms = db.GetRooms().Where(r => r.Type == selectedType).ToList();

            comboRoomNo.Items.Clear();
            foreach (var room in rooms)
            {
                comboRoomNo.Items.Add(room.RoomNo);
            }
        }

        private void ComboRoomNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Room selectedRoom = db.GetRooms().FirstOrDefault(r => r.RoomNo == comboRoomNo.SelectedItem?.ToString());
            if (selectedRoom != null)
            {
                txtPricePerDay.Text = selectedRoom.Price.ToString();
                CalculateTotalPrice(null, null);
            }
        }

        private void DateCheckIn_ValueChanged(object sender, EventArgs e)
        {
            dateCheckOut.MinDate = dateCheckIn.Value.AddDays(1);
            CalculateTotalPrice(null, null);
        }

        private void CalculateTotalPrice(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtPricePerDay.Text, out decimal pricePerDay))
            {
                int stayDuration = (dateCheckOut.Value.Date - dateCheckIn.Value.Date).Days;
                txtTotalPrice.Text = (stayDuration * pricePerDay).ToString();
            }
        }

        private void BtnBook_Click(object sender, EventArgs e)
        {
            GenerateReservationId();
            db.AddReservation(new Reservation
            {
                ReservationId = lblReservationId.Text.Split(':')[1].Trim(),
                CustomerEmail = customerEmail,
                RoomNo = comboRoomNo.SelectedItem.ToString(),
                RoomType = comboRoomType.SelectedItem.ToString(),
                CheckInDate = dateCheckIn.Value.Date.AddDays(1),  // ✅ Corrected date addition
                CheckOutDate = dateCheckOut.Value.Date.AddDays(1), // ✅ Corrected date addition
                TotalPrice = decimal.Parse(txtTotalPrice.Text),
                BookingDate = DateTime.UtcNow
            });


            MessageBox.Show("Room booked successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
