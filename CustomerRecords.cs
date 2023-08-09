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

                string query = "SELECT cu.customerId, cu.customerName, cu.addressId, ad.address, ad.address2, ad.postalCode, ad.phone, ad.cityId, ci.city, ci.countryId, co.country " +
                                "FROM customer cu " +
                                "JOIN address ad ON cu.addressId = ad.addressId " +
                                "JOIN city ci ON ad.cityId = ci.cityId " +
                                "JOIN country co ON ci.countryId = co.countryId;";
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
                                int addressID = reader.GetInt32(2);
                                string address1 = reader.GetString(3);
                                string address2 = reader.GetString(4);
                                string postalCode = reader.GetString(5);
                                string phone = reader.GetString(6);
                                int cityID = reader.GetInt32(7);
                                string cityName = reader.GetString(8);
                                int countryID = reader.GetInt32(9);
                                string countryName = reader.GetString(10);
                                Country country = new Country(countryID, countryName);
                                City city = new City(cityID, cityName, country);
                                Address address = new Address(addressID, address1, address2, postalCode, phone, city);
                                Customer customer = new Customer(customerId, customerName, address);
                                AddCustomerToList(customer);
                            }
                        }
                    }
                }
            }
        }

        public static void UpdateCustomerData(Customer customer, string user)
        {
            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;

                HandleCustomerCountry(command, customer, user);
                HandleCustomerCity(command, customer, user);
                UpdateCustomerAddress(command, customer, user);
                UpdateCustomerInfo(command, customer, user);
            }
        }

        public static void AddCustomerData(Customer customer, string user)
        {
            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;

                HandleCustomerCountry(command, customer, user);
                HandleCustomerCity(command, customer, user);
                AddCustomerAddress(command, customer, user);
                AddCustomerInfo(command, customer, user);
            }
        }

        private static void HandleCustomerCountry(MySqlCommand command, Customer customer, string user)
        {
            string countryQuery = "INSERT INTO country (country, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                "SELECT @country, NOW(), @user , NOW(), @user " +
                "WHERE NOT EXISTS(SELECT 1 FROM country WHERE country = @country)";
            command.CommandText = countryQuery;
            command.Parameters.AddWithValue("@country", customer.Address.City.Country.Name);
            command.Parameters.AddWithValue("@user", user);
            command.ExecuteNonQuery();

            // change countryID for customer
            string countryIdQuery = "SELECT countryId FROM country WHERE country = @country";
            command.CommandText = countryIdQuery;
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    customer.Address.City.Country.CountryID = reader.GetInt32(0);
                }
            }
        }

        private static void HandleCustomerCity(MySqlCommand command, Customer customer, string user)
        {

            string cityQuery = "INSERT INTO city (city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                "SELECT @city, @countryId, NOW(), @user, NOW(), @user " +
                "WHERE NOT EXISTS(SELECT 1 FROM city WHERE city = @city)";
            command.CommandText = cityQuery;
            command.Parameters.AddWithValue("@city", customer.Address.City.Name);
            command.Parameters.AddWithValue("@countryId", customer.Address.City.Country.CountryID);
            //command.Parameters.AddWithValue("@user", user);
            command.ExecuteNonQuery();

            // change cityId for customer
            string cityIdQuery = "SELECT cityId FROM city WHERE city = @city";
            command.CommandText = cityIdQuery;
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    customer.Address.City.CityId = reader.GetInt32(0);
                }
            }
        }

        private static void UpdateCustomerAddress(MySqlCommand command, Customer customer, string user)
        { 
            string addressQuery = "UPDATE address " +
                "SET address = @address1, address2 = @address2, cityId = @cityId, postalCode = @postalCode, phone = @phone, lastUpdate = NOW(), lastUpdateBy = @user " +
                "WHERE addressId = @addressId";
            command.CommandText = addressQuery;
            command.Parameters.AddWithValue("@address1", customer.Address.Address1);
            command.Parameters.AddWithValue("@address2", customer.Address.Address2);
            command.Parameters.AddWithValue("@cityId", customer.Address.City.CityId);
            command.Parameters.AddWithValue("@postalCode", customer.Address.PostalCode);
            command.Parameters.AddWithValue("@phone", customer.Address.Phone);
            command.Parameters.AddWithValue("@addressId", customer.Address.AddressId);
            //command.Parameters.AddWithValue("@user", user);
            command.ExecuteNonQuery();
        }

        private static void AddCustomerAddress(MySqlCommand command, Customer customer, string user)
        {
            string addressQuery = "INSERT INTO address (address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy)" +
                "VALUES (@address1, @address2, @cityId, @postalCode, @phone, NOW(), @user, NOW(), @user)";
            command.CommandText = addressQuery;
            command.Parameters.AddWithValue("@address1", customer.Address.Address1);
            command.Parameters.AddWithValue("@address2", customer.Address.Address2);
            command.Parameters.AddWithValue("@cityId", customer.Address.City.CityId);
            command.Parameters.AddWithValue("@postalCode", customer.Address.PostalCode);
            command.Parameters.AddWithValue("@phone", customer.Address.Phone);
            command.Parameters.AddWithValue("@addressId", customer.Address.AddressId);
            command.ExecuteNonQuery();

            // add addressID for customer address
            string addressIdQuery = "SELECT LAST_INSERT_ID() AS addressId";
            command.CommandText = addressIdQuery;
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    customer.Address.AddressId = reader.GetInt32(0);
                }
            }
        }

        private static void UpdateCustomerInfo(MySqlCommand command, Customer customer, string user)
        {
            string customerQuery = "UPDATE customer " +
                "SET customerName = @newCustomerName, addressId = @addressId, lastUpdate = NOW(), lastUpdateBy = @user " +
                "WHERE customerID = @customerID";
            command.CommandText = customerQuery;
            command.Parameters.AddWithValue("@newCustomerName", customer.Name);
            command.Parameters.AddWithValue("@customerID", customer.CustomerID);
            command.ExecuteNonQuery();
        }

        private static void AddCustomerInfo(MySqlCommand command, Customer customer, string user)
        {
            string customerQuery = "INSERT INTO customer (customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy)" +
                "VALUES (@customerName, @newAddressId, 1, NOW(), @user, NOW(), @user)";
            command.CommandText = customerQuery;
            command.Parameters.AddWithValue("@customerName", customer.Name);
            command.Parameters.AddWithValue("@newAddressId", customer.Address.AddressId);
            command.ExecuteNonQuery();

            // add customerId for customer
            string customerIdQuery = "SELECT LAST_INSERT_ID() AS customerId";
            command.CommandText = customerIdQuery;
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    customer.CustomerID = reader.GetInt32(0);
                }
            }

            AddCustomerToList(customer);
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
