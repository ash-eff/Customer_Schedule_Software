using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace software_2_c969
{
    public partial class FormUpdateCustomer : Form
    {
        private FormMain _parentForm;
        private Customer customer = null;
        public Customer SetCustomer { set { customer = value; PopulateFields(); } }
        public FormUpdateCustomer(FormMain parentForm)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _parentForm = parentForm;
        }

        public void PopulateFields()
        {
            txtId.Text = customer.CustomerID.ToString();
            txtName.Text = customer.Name;
            txtAddress1.Text = customer.Address.Address1;
            txtAddress2.Text = customer.Address.Address2;
            txtPostalCode.Text = customer.Address.PostalCode;
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
            if (ValidateUserInformation())
            {
                UpdateCustomer();
                CustomerRecords.UpdateCustomerData(customer, _parentForm.GetWorkingUser.Name);
                this.Hide();
            }
        }

        private bool ValidateUserInformation()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a valid customer name.", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAddress1.Text))
            {
                MessageBox.Show("Please enter a valid Address.", "Invalid Address Line 1", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPostalCode.Text) || !txtPostalCode.Text.All(char.IsDigit))
            {
                MessageBox.Show("Please enter a valid Postal Code.", "Invalid Postal Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCity.Text) || txtCity.Text.All(char.IsDigit))
            {
                MessageBox.Show("Please enter a valid City.", "Invalid City", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCountry.Text) || txtCountry.Text.All(char.IsDigit))
            {
                MessageBox.Show("Please enter a valid Country.", "Invalid Country", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!IsValidPhoneNumber(txtPhone.Text))
            {
                MessageBox.Show("Please enter a valid phone number. Format should be ###-###-####.", "Invalid Phone", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            string numberPattern = @"^\d{3}-\d{3}-\d{4}$";
            bool isValid = Regex.IsMatch(phoneNumber, numberPattern);
            return isValid;
        }
        private void UpdateCustomer()
        {
            customer.Name = txtName.Text;
            customer.Address.Address1 = txtAddress1.Text;
            customer.Address.Address2 = txtAddress2.Text;
            customer.Address.PostalCode = txtPostalCode.Text;
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
