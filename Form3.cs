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
    public partial class Form3 : Form
    {
        private Form2 _parentForm;
        private Customer customer = null;
        public Customer SetCustomer { set { customer = value; } }
        public Form3(Form2 parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            UpdateCustomer();
            CustomerRecords.AddCustomerData(customer, _parentForm.GetWorkingUser);
            this.Hide();
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
