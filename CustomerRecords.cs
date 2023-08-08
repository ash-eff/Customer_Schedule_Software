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

        private static string connectingString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;

        public static void DeleteCustomer(int atIndex)
        {
            int customerId = Customers[atIndex].CustomerID;
            Customers.RemoveAt(atIndex);
            RemoveCustomerFromData(customerId);
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

        public static void UpdateCustomerData(Customer customer)
        {
            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;

                string customerQuery = "UPDATE customer SET customerName = @newCustomerName, WHERE customerID = @customerID";
                command.CommandText = customerQuery;
                command.Parameters.AddWithValue("@newCustomerName", customer.Name);
                command.Parameters.AddWithValue("@customerID", customer.CustomerID);
                command.ExecuteNonQuery();

                string addressQuery = "UPDATE address SET address = @newAddress, address2 = @newAddress2, postalCode = @newPostalCode, phone = @newPhone WHERE address = @address";
                command.CommandText = addressQuery;
                command.Parameters.AddWithValue("@newAddress", customer.AddressOne);
                command.Parameters.AddWithValue("@newAddress2", customer.AddressTwo);
                command.Parameters.AddWithValue("@newPostalCode", customer.PostalCode);
                command.Parameters.AddWithValue("@newPhone", customer.PhoneNumber);
                command.Parameters.AddWithValue("@customerID", customer.AddressOne);
                command.ExecuteNonQuery();

            }
        }

        public static Customer GetCustomer(int atIndex)
        {
            return Customers[atIndex];
        }

        private static void AddCustomerToList(Customer customer)
        {
            Customers.Add(customer);
        }

        private static void RemoveCustomerFromData(int customerId)
        {
            
            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {

                connection.Open();

                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                string query = "DELETE FROM customer WHERE customerId = @customerId";
                command.CommandText = query;
                command.Parameters.AddWithValue("@customerId", customerId);
                command.ExecuteNonQuery();
            }
        }


    }
}
