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
    public partial class FormCreateAppointments : Form
    {
        private FormCustomerAppointments _parentForm;
        private string selectedDate;
        private string selectedTime;
        private string selectedEndTime;
        private string selectedAppointmentType;
        const int APPOINTMENT_LENGTH = 60;

        public FormCreateAppointments(FormCustomerAppointments parentForm)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.WindowsDefaultLocation;
            _parentForm = parentForm;
            GenerateDates(DateTime.Now);
            GenerateTimes();
            //cmbType.SelectedIndex = 0;
        }

        private void GenerateDates(DateTime fromDate)
        {
            DateTime startDate = fromDate;
            int numberOfDays = 30;

            for(int i = 0; i < numberOfDays; i++)
            {
                DateTime currentDate = startDate.AddDays(i);

                if(currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    cmbBoxDate.Items.Add(currentDate.ToLongDateString());
                }
            }

            if(cmbBoxDate.Items.Count > 0)
            {
                cmbBoxDate.SelectedIndex = 0;
            }
        }

        private void GenerateTimes()
        {
            DateTime localStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
            DateTime localEndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 0, 0);
            TimeSpan interval = TimeSpan.FromMinutes(APPOINTMENT_LENGTH);

            List<DateTime> utcTimes = new List<DateTime>();

            DateTime currentTime = localStartTime;
            while(currentTime <= localEndTime)
            {
                utcTimes.Add(currentTime.ToUniversalTime());
                currentTime = currentTime.Add(interval);
            }

            foreach(DateTime utcTime in utcTimes)
            {
                cmbTime.Items.Add(utcTime.ToString());
            }

            if(cmbTime.Items.Count > 0)
            {
                cmbTime.SelectedIndex = 0;
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            int customerId = _parentForm.GetWorkingCustomer.CustomerID;
            int userId = _parentForm.GetParentForm.GetWorkingUser.UserId;
            //DateTime date = DateTime.Parse(selectedDate);
            DateTime startTime = DateTime.Parse(selectedTime);
            DateTime endTime = DateTime.Parse(selectedEndTime);
            //DateTime combinedStartDateTime = date.Date + startTime.TimeOfDay;
            //DateTime combinedEndDateTime = date.Date + endTime.TimeOfDay;
            //string formattedStartDateTime = combinedStartDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            //string formattedEndDateTime = combinedEndDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            Appointment newAppointment = new Appointment(customerId, startTime, endTime, selectedAppointmentType, userId);
            CustomerAppointments.AddAppointmentData(newAppointment);
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
            DateTime timeSpan = DateTime.Parse(selectedTime);
            TimeSpan interval = TimeSpan.FromMinutes(APPOINTMENT_LENGTH);
            DateTime dateTimeEnd = timeSpan.Add(interval);
            selectedEndTime = dateTimeEnd.ToString();
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
