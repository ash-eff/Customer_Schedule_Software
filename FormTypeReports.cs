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
    public partial class FormTypeReports : Form
    {
        BindingList<Appointment> appointmentsByType = new BindingList<Appointment>();
        public FormTypeReports()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            BuildDataGridView(dgvType);

        }

        private void BuildDataGridView(DataGridView dgv)
        {
            dgv.DefaultCellStyle.SelectionBackColor = Color.Yellow;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.AutoGenerateColumns = false;
            dgv.RowHeadersVisible = false;
        }

        private void GrabAppointmentInformation()
        {
            appointmentsByType.Clear();
            BindingList<Appointment> appointments = CustomerAppointments.GetAllAppointments;
            foreach(Appointment appointment in appointments)
            {
                if (appointment.AppointmentType == cmbType.Text && appointment.StartTime.ToString("MMMM") == cmbMonth.Text)
                {
                    appointmentsByType.Add(appointment);
                }
            }

            if(appointmentsByType.Count == 0)
            {
                MessageBox.Show("No matching appointments found.", "No match.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            //dgvType.DataSource = appointments;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if(!IsValidMonth())
            {
                MessageBox.Show("Please select a Month.", "No month.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
            else if (!IsValidType())
            {
                MessageBox.Show("Please select a type.", "No type.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                GrabAppointmentInformation();
                dgvType.DataSource = appointmentsByType;
            }
        }

        private bool IsValidType()
        {
            if(string.IsNullOrWhiteSpace(cmbType.Text)) { return false; }
            return true;
        }

        private bool IsValidMonth()
        {
            if (string.IsNullOrWhiteSpace(cmbMonth.Text)) { return false; }
            return true;
        }
    }
}
