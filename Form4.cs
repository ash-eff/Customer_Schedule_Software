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
            txtAddress.Text = customer.AddressOne;
            txtAddressTwo.Text = customer.AddressTwo;
            txtZip.Text = customer.PostalCode;
            txtPhone.Text = customer.PhoneNumber;
            txtCity.Text = customer.City;
            txtCountry.Text = customer.Country;
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
            customer.AddressOne = txtAddress.Text;
            customer.AddressTwo = txtAddressTwo.Text;
            customer.PostalCode = txtZip.Text;
            customer.PhoneNumber = txtPhone.Text;
            customer.City = txtCity.Text;
            customer.Country = txtCountry.Text;
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            _parentForm.RefreshData();
        }
    }
}
