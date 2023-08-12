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
    public partial class FormCalendar : Form
    {
         public FormCalendar()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            cmbSelect.SelectedIndex = 0;
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

            DateTime todaysDate = DateTime.Today;

            foreach (Appointment appointment in appointments)
            {
                if (appointments == null) { break; }

                if (cmbSelect.Text == "Next 7 Days")
                {
                    DateTime nextSevenDays = todaysDate.AddDays(7);
                    if (appointment.StartTime >= todaysDate && appointment.StartTime < nextSevenDays)
                    {
                        customerAppointments.Add(appointment);
                    }
                }
                else if (cmbSelect.Text == "Next 30 Days")
                {
                    DateTime nextThirtyDays = todaysDate.AddDays(30);
                    if (appointment.StartTime >= todaysDate && appointment.StartTime < nextThirtyDays)
                    {
                        customerAppointments.Add(appointment);
                    }
                }
                else
                {
                    customerAppointments.Add(appointment);
                }

            }

            dgvAppointments.DataSource = customerAppointments;
            //RefreshData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateAppointmentGrid();
        }
    }
}
