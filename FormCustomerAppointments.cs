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
    public partial class FormCustomerAppointments : Form
    {
        private FormMain _parentForm;
        private Customer customer = null;

        public FormMain GetParentForm {  get { return _parentForm; } }
        public Customer GetWorkingCustomer {  get { return customer; } }

        public Customer SetCustomer { set { customer = value; this.Text = $"Appointments for {customer.Name}"; UpdateAppointmentGrid(); } }
        public FormCustomerAppointments(FormMain parentForm)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _parentForm = parentForm;
            comboBox1.SelectedIndex = 0;
            BuildDataGridView(dgvAppointments);
            
        }

        public void RefreshData()
        {
            UpdateAppointmentGrid();
            dgvAppointments.Refresh();
        }

        private void BuildDataGridView(DataGridView dgv)
        {
            dgv.DefaultCellStyle.SelectionBackColor = Color.Yellow;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.AutoGenerateColumns = false;
            dgv.RowHeadersVisible = false;
        }

        public void UpdateAppointmentGrid()
        {
            BindingList<Appointment> appointments = CustomerAppointments.GetAllAppointments;
            BindingList<Appointment> customerAppointments = new BindingList<Appointment>();
            foreach(Appointment appointment in appointments)
            {
                if (appointment.CustomerId == customer.CustomerID)
                {
                    customerAppointments.Add(appointment);
                }
            }
            if(appointments.Count > 0)
            {
                dgvAppointments.DataSource = customerAppointments;
            }

            //RefreshData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            var nextForm = new FormCreateAppointments(this);
            //nextForm.SetCustomer = customer;
            nextForm.ShowDialog(this);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.Rows.Count > 0)
            {
                int rowIndex = dgvAppointments.CurrentRow.Index;
                DataGridViewRow selectedRow = dgvAppointments.SelectedRows[rowIndex];
                Appointment selectedAppointment = selectedRow.DataBoundItem as Appointment;
                if(selectedAppointment != null)
                {
                    CustomerAppointments.DeleteAppointment(selectedAppointment);
                    RefreshData();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.SelectedRows.Count > 0)
            {
                int rowIndex = dgvAppointments.CurrentRow.Index;
                DataGridViewRow selectedRow = dgvAppointments.SelectedRows[rowIndex];
                Appointment selectedAppointment = selectedRow.DataBoundItem as Appointment;
                if (selectedAppointment != null)
                {
                    var nextForm = new FormUpdateAppointments(this, selectedAppointment);
                    nextForm.ShowDialog(this);
                }
            }
        }
    }
}
