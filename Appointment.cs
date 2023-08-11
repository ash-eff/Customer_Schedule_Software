using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace software_2_c969
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int CustomerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string AppointmentType { get; set; }
        public int UserId { get; set; }
        public string StringStartTime { get; set; }
        public string StringEndTime { get; set; }
        public string StringDate { get; set; }
        public string ScheduledBy { get; set; }
        public string ScheduledFor { get; set; }

        public Appointment(int customerId, DateTime startTime, DateTime endTime, string appointmentType, int userId)
        {
            CustomerId = customerId;
            StartTime = startTime;
            EndTime = endTime;
            AppointmentType = appointmentType;
            UserId = userId;
            StringStartTime = startTime.TimeOfDay.ToString();
            StringEndTime = endTime.TimeOfDay.ToString();
            string day = startTime.Day.ToString();
            string month = startTime.Month.ToString();
            string year = startTime.Year.ToString();
            StringDate = month + "/" + day + "/" + year;
            SetScheduledBy(userId);
            SetScheduledFor(customerId);
        }

        public void Update(int customerId, DateTime startTime, DateTime endTime, string appointmentType, int userId)
        {
            CustomerId = customerId;
            StartTime = startTime;
            EndTime = endTime;
            AppointmentType = appointmentType;
            UserId = userId;
            StringStartTime = startTime.TimeOfDay.ToString();
            StringEndTime = endTime.TimeOfDay.ToString();
            string day = startTime.Day.ToString();
            string month = startTime.Month.ToString();
            string year = startTime.Year.ToString();
            StringDate = month + "/" + day + "/" + year;
            SetScheduledBy(userId);
            SetScheduledFor(customerId);
        }

        private void SetScheduledBy(int userId)
        {
            string connectingString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;

                string query = "SELECT userName FROM user WHERE userId = @userId";
                command.Parameters.AddWithValue("@userId", userId);
                command.CommandText = query;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ScheduledBy = reader.GetString(0);
                    }
                }
            }

        }

        private void SetScheduledFor(int customerId)
        {
            string connectingString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;

                string query = "SELECT customerName FROM customer WHERE customerId = @customerId";
                command.Parameters.AddWithValue("@customerId", customerId);
                command.CommandText = query;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ScheduledFor = reader.GetString(0);
                    }
                }
            }
        }
    }
}
