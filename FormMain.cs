using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace software_2_c969
{
    public partial class FormMain : Form
    {
        private const string LOGFILEPATH = "login_log.txt";

        private User workingUser;
        public User GetWorkingUser { get { return workingUser; } }

        public FormMain(int userID)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            SetWorkingUser(userID);
            RecordLogin(workingUser);
            BuildDataGridView(dgvCustomers);
            CustomerRecords.LoadCustomersFromData();
            CustomerAppointments.LoadAppointmentsFromData();
            UpdateCustomerGrid();
            CheckForUpcomingAppointments();
        }

        private void RecordLogin(User user)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string logEntry = $"{timestamp} - User {user.Name} logged in";

                using (StreamWriter writer = File.AppendText(LOGFILEPATH))
                {
                    writer.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error recording login: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetWorkingUser(int userId)
        {
            string connectingString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;

                string countryQuery = "SELECT userId, userName FROM user WHERE userId = @userId";
                command.CommandText = countryQuery;
                command.Parameters.AddWithValue("@userId", userId);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        workingUser = new User(reader.GetInt32("userId"), reader.GetString("userName"));
                        Console.WriteLine("Welcome, " + workingUser.Name);
                    }
                }
            }
        }

        private void CheckForUpcomingAppointments()
        {
            DateTime currentTime = DateTime.Now;
            Console.WriteLine("The current Time is :" + currentTime);
            string connectingString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            string appointmentQuery = "SELECT start FROM appointment WHERE userId = @userId";

            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(appointmentQuery, connection))
                {
                    command.Parameters.AddWithValue("@userId", workingUser.UserId);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime appointmentTime = reader.GetDateTime(0);
                            TimeSpan timeDifference = appointmentTime - currentTime;
                            if(timeDifference.TotalMinutes <= 15 && timeDifference.TotalMinutes >= 0)
                            {
                                MessageBox.Show("You have an appointment at  " + appointmentTime.TimeOfDay, "Appointment Alert!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                    }
                }
            }
        }

        private void BuildDataGridView(DataGridView dgv)
        {
            dgv.DefaultCellStyle.SelectionBackColor = Color.Yellow;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.AutoGenerateColumns = false;
            dgv.RowHeadersVisible = false;
        }

        public void UpdateCustomerGrid()
        {
            BindingList<Customer> customers = CustomerRecords.GetAllCustomers;
            dgvCustomers.DataSource = customers;
            RefreshData();
        }

        public void RefreshData()
        {
            dgvCustomers.Refresh();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
             if (dgvCustomers.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvCustomers.SelectedRows[0];
                Customer selectedCustomer = selectedRow.DataBoundItem as Customer;
                if (selectedCustomer != null)
                {
                    CustomerRecords.DeleteCustomer(selectedCustomer);
                    RefreshData();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var nextForm = new FormAddCustomer(this);
            nextForm.ShowDialog(this);
            //this.Hide();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int customerIndex = dgvCustomers.CurrentCell.RowIndex;
            Customer customer = CustomerRecords.GetCustomer(customerIndex);
            var nextForm = new FormUpdateCustomer(this);
            nextForm.SetCustomer = customer;
            nextForm.ShowDialog(this);
            //this.Hide();
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            int customerIndex = dgvCustomers.CurrentCell.RowIndex;
            Customer customer = CustomerRecords.GetCustomer(customerIndex);
            var nextForm = new FormCustomerAppointments(this);
            nextForm.SetCustomer = customer;
            nextForm.ShowDialog(this);
            //this.Hide();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            var nextForm = new FormReports();
            nextForm.ShowDialog(this);
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            var nextForm = new FormCalendar();
            nextForm.ShowDialog(this);
        }
    }
}
