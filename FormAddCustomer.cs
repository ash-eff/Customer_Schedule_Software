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
    public partial class FormAddCustomer : Form
    {
        private FormMain _parentForm;
        private Customer customer = null;
        public Customer SetCustomer { set { customer = value; } }
        public FormAddCustomer(FormMain parentForm)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _parentForm = parentForm;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(ValidateUserInformation())
            {
                UpdateCustomer();
                CustomerRecords.AddCustomerData(customer, _parentForm.GetWorkingUser.Name);
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
            Country country = new Country(0, txtCountry.Text);
            City city = new City(0, txtCity.Text, country);
            Address address = new Address(0, txtAddress1.Text, txtAddress2.Text, txtPostalCode.Text, txtPhone.Text, city);
            customer = new Customer(0, txtName.Text, address);
        }
    }
}
