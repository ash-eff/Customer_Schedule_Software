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
    public partial class Form2 : Form
    {
        private string workingUser = null;
        public string GetWorkingUser { get { return workingUser; } }

        public Form2(int userID)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            SetWorkingUser(userID);
            BuildDataGridView(dgvCustomers);
            CustomerRecords.LoadCustomersFromData();
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

                string countryQuery = "SELECT userName FROM user WHERE userId = @userId";
                command.CommandText = countryQuery;
                command.Parameters.AddWithValue("@userId", userId);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        workingUser = reader.GetString("userName");
                        Console.WriteLine("Welcome, " + workingUser);
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

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var nextForm = new Form3(this);
            nextForm.ShowDialog(this);
            //this.Hide();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(dgvCustomers.Rows.Count > 0)
            {
                int customerIndex = dgvCustomers.CurrentCell.RowIndex;
                Customer customer = CustomerRecords.GetCustomer(customerIndex);
                var nextForm = new Form4(this);
                nextForm.SetCustomer = customer;
                nextForm.ShowDialog(this);
            }
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            var nextForm = new Form5(this);
            nextForm.ShowDialog(this);
            //this.Hide();
        }
    }
}
