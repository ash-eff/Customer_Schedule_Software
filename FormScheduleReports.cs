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
    public partial class FormScheduleReports : Form
    {
        BindingList<Appointment> appointmentsByUser = new BindingList<Appointment>();
        public FormScheduleReports()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            BuildDataGridView(dvgSchedule);
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
            appointmentsByUser.Clear();
            BindingList<Appointment> appointments = CustomerAppointments.GetAllAppointments;
            foreach (Appointment appointment in appointments)
            {
                if (appointment.UserId.ToString() == txtId.Text)
                {
                    appointmentsByUser.Add(appointment);
                }
            }

            if (appointmentsByUser.Count == 0)
            {
                MessageBox.Show("No matching appointments found or User does not exist.", "No match.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (!IsValidId())
            {
                MessageBox.Show("Please enter a valid ID.", "No ID.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                GrabAppointmentInformation();
                dvgSchedule.DataSource = appointmentsByUser;
            }
        }

        private bool IsValidId()
        {
            if (string.IsNullOrWhiteSpace(txtId.Text)) { return false; }
            return true;
        }
    }
}
