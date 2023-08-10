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
        private Appointment workingAppointment;
        private string selectedDate;
        private string selectedTime;
        private string selectedEndTime;
        private string selectedAppointmentType;
        const int APPOINTMENT_LENGTH = 60;

        public FormUpdateAppointments(FormCustomerAppointments parentForm, Appointment appointment)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.WindowsDefaultLocation;
            _parentForm = parentForm;
            workingAppointment = appointment;
            GenerateDates(DateTime.Now);
            GenerateTimes();
            PopulateFields();
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
        }

        private void GenerateTimes()
        {
            TimeZoneInfo userTimeZone = TimeZoneInfo.Local;
            TimeZoneInfo centralTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

            DateTime localStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
            DateTime localEndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 0, 0);
            TimeSpan interval = TimeSpan.FromMinutes(APPOINTMENT_LENGTH);

            List<DateTime> localTimes = new List<DateTime>();

            DateTime currentTime = localStartTime;
            while (currentTime <= localEndTime)
            {
                DateTime convertedTime = TimeZoneInfo.ConvertTime(currentTime, centralTimeZone, userTimeZone);
                Console.WriteLine("Converting " + currentTime.ToShortTimeString() + " to " + convertedTime.ToShortTimeString());
                localTimes.Add(convertedTime);
                currentTime = currentTime.Add(interval);
            }

            //cmbTime.Items.Clear();

            foreach (DateTime localTime in localTimes)
            {
                cmbTime.Items.Add(localTime.ToString());
            }
        }

        private void PopulateFields()
        {
            DateTime appointmentDate = workingAppointment.StartTime;
            cmbBoxDate.SelectedIndex = cmbBoxDate.FindStringExact(appointmentDate.Date.ToString());
            cmbTime.SelectedIndex = cmbTime.FindStringExact(appointmentDate.TimeOfDay.ToString());
            cmbType.SelectedIndex = cmbType.FindStringExact(workingAppointment.AppointmentType);
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
