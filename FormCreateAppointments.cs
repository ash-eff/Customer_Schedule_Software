using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

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
            cmbType.SelectedIndex = 0;
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
            TimeZoneInfo userTimeZone = TimeZoneInfo.Local;
            TimeZoneInfo centralTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

            DateTime localStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
            DateTime localEndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 0, 0);
            TimeSpan interval = TimeSpan.FromMinutes(APPOINTMENT_LENGTH);

            List<DateTime> localTimes = new List<DateTime>();

            DateTime currentTime = localStartTime;
            while(currentTime <= localEndTime)
            {
                DateTime convertedTime = TimeZoneInfo.ConvertTime(currentTime, centralTimeZone, userTimeZone);
                localTimes.Add(convertedTime);
                currentTime = currentTime.Add(interval);
            }

            //cmbTime.Items.Clear();

            foreach(DateTime localTime in localTimes)
            {
                cmbTime.Items.Add(localTime.ToString("hh:mm tt"));
            }

            if(cmbTime.Items.Count > 0)
            {
                cmbTime.SelectedIndex = 0;
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                int customerId = _parentForm.GetWorkingCustomer.CustomerID;
                int userId = _parentForm.GetParentForm.GetWorkingUser.UserId;
                DateTime date = DateTime.Parse(selectedDate);
                DateTime startTime = DateTime.Parse(selectedTime);
                DateTime endTime = DateTime.Parse(selectedEndTime);
                DateTime finalStartTime = new DateTime(date.Year, date.Month, date.Day, startTime.Hour, startTime.Minute, startTime.Second);
                DateTime finalEndTime = new DateTime(date.Year, date.Month, date.Day, endTime.Hour, endTime.Minute, endTime.Second);

                if (!IsWithinWorkingHours(startTime, endTime))
                {
                    MessageBox.Show("Appointments must be scheduled during hours of operation.", "Invalid Appointment Time.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!IsAnAvailableTime(finalStartTime))
                {
                    MessageBox.Show("This time overlaps with an appointment that you already have with another customer.", "Unavailable Appointment Time.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Appointment newAppointment = new Appointment(customerId, finalStartTime, finalEndTime, selectedAppointmentType, userId);
                    CustomerAppointments.AddAppointmentData(newAppointment);
                    this.Hide();
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Date/Time Parsing Error: " + ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding requested appointment: " + ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool IsWithinWorkingHours(DateTime startTime, DateTime endTime)
        {
            TimeSpan validStartingTime = new TimeSpan(9, 0, 0);
            TimeSpan validEndingTime = new TimeSpan(14, 0, 0);

            return startTime.TimeOfDay >= validStartingTime && endTime.TimeOfDay <= validEndingTime; // LAMBDA comparing starting and ending hours. Done because it looks cleaner than an if statement 
        }

        private bool IsAnAvailableTime(DateTime startTime)
        {
            int userId = _parentForm.GetParentForm.GetWorkingUser.UserId;
            //Customer customer = _parentForm.GetWorkingCustomer;
            string connectingString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            string appointmentQuery = "SELECT start FROM appointment WHERE userId = @userId";
            List<DateTime> appointmentTimes = new List<DateTime>();

            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                connection.Open();
                
                using(MySqlCommand command = new MySqlCommand(appointmentQuery, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime appointmentTime = reader.GetDateTime(0);
                            appointmentTimes.Add(appointmentTime);
                        }
                    }
                }
            }

            return !appointmentTimes.Any(appointmentTime => appointmentTime == startTime); //LAMBDA more concise that what I had initiall (listed below)

            //foreach(DateTime appintmentTime in appointmentTimes)
            //{
            //    if(appintmentTime == startTime)
            //    {
            //        return false;
            //    }
            //}
            //
            //return true;
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
