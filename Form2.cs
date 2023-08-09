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
        public Form2()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            BuildDataGridView(dgvCustomers);
            CustomerRecords.LoadCustomersFromData();
            UpdateCustomerGrid();
        }

        private void BuildDataGridView(DataGridView dgv)
        {
            dgv.DefaultCellStyle.SelectionBackColor = Color.Yellow;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            //dgv.AutoSizeColumnsMode = (DataGridViewAutoSizeColumnsMode)DataGridViewAutoSizeColumnMode.Fill;
            dgv.RowHeadersVisible = false;
            dgv.Columns.Add("CustomerIDColumn", "Customer ID");
            dgv.Columns["CustomerIDColumn"].DataPropertyName = "CustomerID";

            dgv.Columns.Add("NameColumn", "Name");
            dgv.Columns["NameColumn"].DataPropertyName = "Name";

            dgv.Columns.Add("Address1Column", "Address 1");
            dgv.Columns["Address1Column"].DataPropertyName = "Address.Address1";

            dgv.Columns.Add("Address2Column", "Address 2");
            dgv.Columns["Address1Column"].DataPropertyName = "Address.Address2";

            dgv.Columns.Add("PostalCodeColumn", "Postal Code");
            dgv.Columns["PostalCodeColumn"].DataPropertyName = "Address.PostalCode";

            dgv.Columns.Add("PhoneColumn", "Phone");
            dgv.Columns["PhoneColumn"].DataPropertyName = "Address.Phone";

            dgv.Columns.Add("CityColumn", "City");
            dgv.Columns["CityColumn"].DataPropertyName = "Address.City.Name";

            dgv.Columns.Add("CountryColumn", "Country");
            dgv.Columns["CountryColumn"].DataPropertyName = "Address.City.Country.Name";

        }

        public void UpdateCustomerGrid()
        {
            dgvCustomers.AutoGenerateColumns = false;
            BindingList<Customer> customers = CustomerRecords.GetAllCustomers;

            foreach (Customer c in customers)
            {
                Console.WriteLine(c.CustomerID);
                Console.WriteLine(c.Name);
                Console.WriteLine(c.Address.Address1);
                Console.WriteLine(c.Address.Address2);
                Console.WriteLine(c.Address.PostalCode);
                Console.WriteLine(c.Address.Phone);
                Console.WriteLine(c.Address.City.Name);
                Console.WriteLine(c.Address.City.Country.Name);
                Console.WriteLine("------------------------------");
            }
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
            if(dgvCustomers.Rows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this customer?", "Delete", MessageBoxButtons.YesNo);
                if(dialogResult == DialogResult.Yes)
                {
                    int rowIndex = dgvCustomers.CurrentCell.RowIndex;
                    CustomerRecords.DeleteCustomer(rowIndex);
                    UpdateCustomerGrid();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var nextForm = new Form3();
            nextForm.Show();
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
    }
}
