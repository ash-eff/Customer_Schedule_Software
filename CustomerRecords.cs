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
    static class CustomerRecords
    {
        private static BindingList<Customer> Customers = new BindingList<Customer>();

        public static BindingList<Customer> GetAllCustomers { get { return Customers; } }

        public static void DeleteCustomer(int atIndex)
        {
            int customerId = Customers[atIndex].CustomerID;
            Customers.RemoveAt(atIndex);
            RemoveCustomerFromData(customerId);
        }

        private static void RemoveCustomerFromData(int customerId)
        {
            string connectingString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                
                connection.Open();

                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                string query = "Delete From customer where customerId = @customerId";
                command.CommandText = query;
                command.Parameters.AddWithValue("@customerId", customerId);
                command.ExecuteNonQuery();
            }
        }

        public static void LoadCustomersFromData()
        {
            string connectingString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                connection.Open();

                string query = "select cu.customerId, cu.customerName, ad.address, ad.address2, ad.postalCode, ad.phone, ci.city, co.country " +
                                "from customer cu " +
                                "join address ad on cu.addressId = ad.addressId " +
                                "join city ci on ad.cityId = ci.cityId " +
                                "join country co on ci.countryId = co.countryId;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int customerId = reader.GetInt32(0);
                            bool exists = Customers.Any(c => c.CustomerID == customerId); // lambda
                            if (!exists)
                            {
                                string customerName = reader.GetString(1);
                                string address = reader.GetString(2);
                                string address2 = reader.GetString(3);
                                string postalCode = reader.GetString(4);
                                string phone = reader.GetString(5);
                                string city = reader.GetString(6);
                                string country = reader.GetString(7);
                                Customer customer = new Customer(customerId, customerName, address, address2, postalCode, phone, city, country);
                                AddCustomerToList(customer);
                            }
                        }
                    }
                }
            }
        }

        private static void AddCustomerToList(Customer customer)
        {
            Customers.Add(customer);
        }

    }
}
