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
    public partial class Form5 : Form
    {
        private Form2 _parentForm;
        public Form5(Form2 parentForm)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            comboBox1.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            var nextForm = new Form6();
            //nextForm.SetCustomer = customer;
            nextForm.ShowDialog(this);
        }
    }
}
