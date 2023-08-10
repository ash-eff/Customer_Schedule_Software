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
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string AppointmentType { get; set; }
        public int CreatedByUser { get; set; }
        public Appointment(int customerId, string startTime,  string endTime, string appointmentType, int createdByUser)
        {
            CustomerId = customerId;
            StartTime = startTime;
            EndTime = endTime;
            AppointmentType = appointmentType;
            CreatedByUser = createdByUser;
        }
    }
}
