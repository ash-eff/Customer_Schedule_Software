using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace software_2_c969
{
    public partial class FormUpdateAppointments : Form
    {
        private FormCustomerAppointments _parentForm;
        private string selectedDate;
        private string selectedTime;
        private string selectedEndTime;
        private string selectedAppointmentType;

        public FormUpdateAppointments(FormCustomerAppointments parentForm)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.WindowsDefaultLocation;
            _parentForm = parentForm;
            GenerateDates(DateTime.Now);
            GenerateTimes();
            cmbType.SelectedIndex = 0;
        }

        private void GenerateDates(DateTime fromDate)
        {
            DateTime startDate = fromDate;
            int numberOfDays = 30;

            for (int i = 0; i < numberOfDays; i++)
            {
                DateTime currentDate = startDate.AddDays(i);

                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    cmbBoxDate.Items.Add(currentDate.ToLongDateString());
                }
            }

            if (cmbBoxDate.Items.Count > 0)
            {
                cmbBoxDate.SelectedIndex = 0;
            }
        }

        private void GenerateTimes()
        {
            TimeSpan startTime = new TimeSpan(9, 0, 0);
            TimeSpan endTime = new TimeSpan(14, 0, 0);
            TimeSpan interval = TimeSpan.FromMinutes(15);

            for (TimeSpan currentTime = startTime; currentTime < endTime; currentTime = currentTime.Add(interval))
            {
                cmbTime.Items.Add($"{currentTime.Hours:D2}:{currentTime.Minutes:D2}");
            }

            if (cmbTime.Items.Count > 0)
            {
                cmbTime.SelectedIndex = 0;
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            int customerId = _parentForm.GetWorkingCustomer.CustomerID;
            int userId = _parentForm.GetParentForm.GetWorkingUser.UserId;
            DateTime date = DateTime.Parse(selectedDate);
            TimeSpan startTime = TimeSpan.Parse(selectedTime);
            TimeSpan endTime = TimeSpan.Parse(selectedEndTime);
            DateTime combinedStartDateTime = date.Date + startTime;
            DateTime combinedEndDateTime = date.Date + endTime;
            //string formattedStartDateTime = combinedStartDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            //string formattedEndDateTime = combinedEndDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            //Appointment newAppointment = new Appointment(customerId, formattedStartDateTime, formattedEndDateTime, selectedAppointmentType, userId);
            //CustomerAppointments.AddAppointmentData(newAppointment);
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void cmbBoxDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedDate = cmbBoxDate.Text;
        }

        private void cmbTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTime = cmbTime.Text;
            TimeSpan timeSpan = TimeSpan.ParseExact(selectedTime, "hh\\:mm", CultureInfo.InvariantCulture);
            TimeSpan interval = TimeSpan.FromMinutes(15);
            TimeSpan dateTimeEnd = timeSpan.Add(interval);
            selectedEndTime = $"{dateTimeEnd.Hours:D2}:{dateTimeEnd.Minutes:D2}";
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedAppointmentType = cmbType.Text;
        }

        private void Form6_FormClosing(object sender, FormClosingEventArgs e)
        {
            _parentForm.RefreshData();
        }
    }
}
