using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Hotel_Reservation.Models;

namespace Hotel_Reservation
{
    public partial class AddRoom : Form
    {
        private TextBox txtRoomNo, txtFloor, txtPrice, txtType, txtAmenities;
        private Button btnAddRoom, btnDeleteRoom, btnUpdateRoom;
        private DataGridView dgvRooms;
        private DatabaseHelper db;
        private Label lblTitle;

        public AddRoom()
        {
            InitializeComponent();
            this.Text = "Manage Rooms";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            db = new DatabaseHelper();

            InitializeUI();
            LoadRooms();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "AddRoom";
            this.Load += new System.EventHandler(this.AddRoom_Load);
            this.ResumeLayout(false);
        }

        private void AddRoom_Load(object sender, EventArgs e)
        {
            LoadRooms();
        }

        private void InitializeUI()
        {
            lblTitle = new Label
            {
                Text = "Room Management",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                AutoSize = true,
                Location = new Point(300, 20)
            };

            Label lblRoomNo = new Label { Text = "Room No:", Location = new Point(30, 70), AutoSize = true };
            txtRoomNo = new TextBox { Location = new Point(150, 70), Width = 250 };

            Label lblFloor = new Label { Text = "Floor:", Location = new Point(30, 110), AutoSize = true };
            txtFloor = new TextBox { Location = new Point(150, 110), Width = 250 };

            Label lblPrice = new Label { Text = "Price:", Location = new Point(30, 150), AutoSize = true };
            txtPrice = new TextBox { Location = new Point(150, 150), Width = 250 };

            Label lblType = new Label { Text = "Type:", Location = new Point(30, 190), AutoSize = true };
            txtType = new TextBox { Location = new Point(150, 190), Width = 250 };

            Label lblAmenities = new Label { Text = "Amenities:", Location = new Point(30, 230), AutoSize = true };
            txtAmenities = new TextBox { Location = new Point(150, 230), Width = 250 };

            btnAddRoom = new Button
            {
                Text = "Add Room",
                Location = new Point(50, 280),
                Size = new Size(120, 40),
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnAddRoom.Click += BtnAddRoom_Click;

            btnDeleteRoom = new Button
            {
                Text = "Delete Room",
                Location = new Point(200, 280),
                Size = new Size(120, 40),
                BackColor = Color.IndianRed,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnDeleteRoom.Click += BtnDeleteRoom_Click;

            btnUpdateRoom = new Button
            {
                Text = "Update Room",
                Location = new Point(350, 280),
                Size = new Size(120, 40),
                BackColor = Color.Goldenrod,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnUpdateRoom.Click += BtnUpdateRoom_Click;

            dgvRooms = new DataGridView
            {
                Location = new Point(30, 340),
                Size = new Size(720, 200),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.Fixed3D
            };

            dgvRooms.CellClick += DgvRooms_CellClick;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblRoomNo);
            this.Controls.Add(txtRoomNo);
            this.Controls.Add(lblFloor);
            this.Controls.Add(txtFloor);
            this.Controls.Add(lblPrice);
            this.Controls.Add(txtPrice);
            this.Controls.Add(lblType);
            this.Controls.Add(txtType);
            this.Controls.Add(lblAmenities);
            this.Controls.Add(txtAmenities);
            this.Controls.Add(btnAddRoom);
            this.Controls.Add(btnDeleteRoom);
            this.Controls.Add(btnUpdateRoom);
            this.Controls.Add(dgvRooms);
        }

        private void BtnAddRoom_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtRoomNo.Text) || string.IsNullOrWhiteSpace(txtFloor.Text) ||
                    string.IsNullOrWhiteSpace(txtPrice.Text) || string.IsNullOrWhiteSpace(txtType.Text) ||
                    string.IsNullOrWhiteSpace(txtAmenities.Text))
                {
                    MessageBox.Show("❌ Please fill all fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txtFloor.Text, out int floor) || !decimal.TryParse(txtPrice.Text, out decimal price))
                {
                    MessageBox.Show("❌ Floor and Price must be numbers!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string lastId = db.GetLastRoomId();
                int newId = int.TryParse(lastId.Substring(2), out int lastNumericId) ? lastNumericId + 1 : 1;
                string formattedId = $"RM{newId:D3}";

                Room room = new Room
                {
                    Id = formattedId,
                    RoomNo = txtRoomNo.Text,
                    Floor = floor,
                    Price = price,
                    Type = txtType.Text,
                    Amenities = txtAmenities.Text
                };

                if (db.DoesRoomExist(room.RoomNo))
                {
                    MessageBox.Show("❌ Room already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                db.AddRoom(room);
                MessageBox.Show("✅ Room Added Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRooms();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdateRoom_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtFloor.Text, out int floor) || !decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("❌ Floor and Price must be valid numbers!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Room updatedRoom = new Room
            {
                RoomNo = txtRoomNo.Text,
                Floor = floor,
                Price = price,
                Type = txtType.Text,
                Amenities = txtAmenities.Text
            };

            if (db.UpdateRoom(txtRoomNo.Text, updatedRoom))
            {
                MessageBox.Show("✅ Room Updated Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRooms();
            }
        }

        private void BtnDeleteRoom_Click(object sender, EventArgs e)
        {
            if (dgvRooms.SelectedRows.Count == 0)
            {
                MessageBox.Show("❌ Please select a room to delete!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string selectedRoomNo = dgvRooms.SelectedRows[0].Cells["RoomNo"].Value.ToString();

            try
            {
                db.DeleteRoom(selectedRoomNo);
                MessageBox.Show($"✅ Room {selectedRoomNo} Deleted Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRooms();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error deleting room: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvRooms_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvRooms.Rows[e.RowIndex];
                txtRoomNo.Text = row.Cells["RoomNo"].Value.ToString();
                txtFloor.Text = row.Cells["Floor"].Value.ToString();
                txtPrice.Text = row.Cells["Price"].Value.ToString();
                txtType.Text = row.Cells["Type"].Value.ToString();
                txtAmenities.Text = row.Cells["Amenities"].Value.ToString();
            }
        }

        private void LoadRooms()
        {
            dgvRooms.DataSource = db.GetRooms();
        }
    }
}
