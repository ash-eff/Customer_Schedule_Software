using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace software_2_c969
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int CustomerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string AppointmentType { get; set; }
        public int CreatedByUser { get; set; }
        public string StringStartTime { get; set; }
        public string StringEndTime { get; set; }
        public string StringDate { get; set; }

        public Appointment(int customerId, DateTime startTime, DateTime endTime, string appointmentType, int createdByUser)
        {
            CustomerId = customerId;
            StartTime = startTime;
            EndTime = endTime;
            AppointmentType = appointmentType;
            CreatedByUser = createdByUser;
            StringStartTime = startTime.TimeOfDay.ToString();
            StringEndTime = endTime.TimeOfDay.ToString();
            string day = startTime.Day.ToString();
            string month = startTime.Month.ToString();
            string year = startTime.Year.ToString();
            StringDate = month + "/" + day + "/" + year;
        }
    }
}
