using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace software_2_c969
{
    public partial class FormMain : Form
    {
        private User workingUser;
        public User GetWorkingUser { get { return workingUser; } }

        public FormMain(int userID)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            SetWorkingUser(userID);
            BuildDataGridView(dgvCustomers);
            CustomerRecords.LoadCustomersFromData();
            CustomerAppointments.LoadAppointmentsFromData();
            UpdateCustomerGrid();
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
            if (dgvCustomers.Rows.Count > 0)
            {
                int rowIndex = dgvCustomers.CurrentCell.RowIndex;
                DataGridViewRow selectedRow = dgvCustomers.SelectedRows[rowIndex];
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
            if(dgvCustomers.Rows.Count > 0)
            {
                int customerIndex = dgvCustomers.CurrentCell.RowIndex;
                Customer customer = CustomerRecords.GetCustomer(customerIndex);
                var nextForm = new FormUpdateCustomer(this);
                nextForm.SetCustomer = customer;
                nextForm.ShowDialog(this);
            }
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            int customerIndex = dgvCustomers.CurrentCell.RowIndex;
            Customer customer = CustomerRecords.GetCustomer(customerIndex);
            Console.WriteLine(customer.Name);
            var nextForm = new FormCustomerAppointments(this);
            nextForm.SetCustomer = customer;
            nextForm.ShowDialog(this);
            //this.Hide();
        }
    }
}
