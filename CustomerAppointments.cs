using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace software_2_c969
{
    public static class CustomerAppointments
    {
        private static BindingList<Appointment> Appointments = new BindingList<Appointment>();
        public static BindingList<Appointment> GetAllAppointments { get { return Appointments; } }

        private static string connectingString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;

        public static void LoadAppointmentsFromData()
        {
            string connectingString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                connection.Open();

                string query = "SELECT appointmentId, customerId, start, end, type, userId FROM appointment";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int appointmentId = reader.GetInt32("appointmentId");
                            bool exists = Appointments.Any(a => a.AppointmentId == appointmentId); // lambda
                            if (!exists)
                            {
                                int customerId = reader.GetInt32("customerId");
                                string formattedStartDateTime = reader.GetString("start");
                                string formattedEndDateTime = reader.GetString("end");
                                string selectedAppointmentType = reader.GetString("type");
                                int userId = reader.GetInt32("userId");
                                Appointment newAppointment = new Appointment(customerId, formattedStartDateTime, formattedEndDateTime, selectedAppointmentType, userId);
                                newAppointment.AppointmentId = appointmentId;
                                AddAppointmentToList(newAppointment);
                            }
                        }
                    }
                }

                Console.WriteLine("Number of appointments: " + Appointments.Count());
            }
        }

        public static void AddAppointmentData(Appointment appointment)
        {
            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;

                string appointmentQuery = "INSERT INTO appointment (customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy)" +
                        "VALUES (@customerId, @userId, @default, @default, @default, @default, @appointmentType, @default, @start, @end, NOW(), @user, NOW(), @user)";
                command.CommandText = appointmentQuery;
                command.Parameters.AddWithValue("@customerId", appointment.CustomerId);
                command.Parameters.AddWithValue("@userId", appointment.CreatedByUser);
                command.Parameters.AddWithValue("@appointmentType", appointment.AppointmentType);
                command.Parameters.AddWithValue("@start", appointment.StartTime);
                command.Parameters.AddWithValue("@end", appointment.EndTime);
                command.Parameters.AddWithValue("@user", appointment.CreatedByUser.ToString());
                command.Parameters.AddWithValue("@default", "not needed");
                command.ExecuteNonQuery();

                // add appointmentId for appointment
                string appintmentIdQuery = "SELECT LAST_INSERT_ID() AS appointmentId";
                command.CommandText = appintmentIdQuery;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        appointment.AppointmentId = reader.GetInt32("appointmentId");
                    }
                }

                AddAppointmentToList(appointment);

            }
        }

        public static void DeleteAppointment(Appointment appointment)
        {
            Appointments.Remove(appointment);
            RemoveAppointmentFromData(appointment);
        }

        private static void AddAppointmentToList(Appointment appointment)
        {
            Appointments.Add(appointment);
        }

        private static void RemoveAppointmentFromData(Appointment appointment)
        {

            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {

                connection.Open();

                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                string query = "DELETE FROM appointment WHERE appointmentId = @appointmentId";
                command.CommandText = query;
                command.Parameters.AddWithValue("@appointmentId", appointment.AppointmentId);
                command.ExecuteNonQuery();
            }
        }
    }
}
