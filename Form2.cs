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
            dgv.AutoSizeColumnsMode = (DataGridViewAutoSizeColumnsMode)DataGridViewAutoSizeColumnMode.Fill;
            dgv.RowHeadersVisible = false;
        }

        public void UpdateCustomerGrid()
        {
            dgvCustomers.AutoGenerateColumns = false;
            dgvCustomers.DataSource = CustomerRecords.GetAllCustomers;
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
