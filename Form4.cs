using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace software_2_c969
{
    public partial class Form4 : Form
    {
        private Form2 _parentForm;
        private Customer customer = null;
        public Customer SetCustomer { set { customer = value; PopulateFields(); } }
        public Form4(Form2 parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
        }

        public void PopulateFields()
        {
            txtId.Text = customer.CustomerID.ToString();
            txtName.Text = customer.Name;
            txtAddress.Text = customer.Address.Address1;
            txtAddressTwo.Text = customer.Address.Address2;
            txtZip.Text = customer.Address.PostalCode;
            txtPhone.Text = customer.Address.Phone;
            txtCity.Text = customer.Address.City.Name;
            txtCountry.Text = customer.Address.City.Country.Name;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            UpdateCustomer();
            CustomerRecords.UpdateCustomerData(customer);
            this.Hide();
        }

        private void UpdateCustomer()
        {
            customer.CustomerID = int.Parse(txtId.Text);
            customer.Name = txtName.Text;
            customer.Address.Address1 = txtAddress.Text;
            customer.Address.Address2 = txtAddressTwo.Text;
            customer.Address.PostalCode = txtZip.Text;
            customer.Address.Phone = txtPhone.Text;
            customer.Address.City.Name = txtCity.Text;
            customer.Address.City.Country.Name = txtCountry.Text;
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            _parentForm.RefreshData();
        }
    }
}
