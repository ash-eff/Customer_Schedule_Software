using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace software_2_c969
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public int AddressID { get; set; }
        public string AddressOne { get; set; }
        public string AddressTwo { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public int CityID { get; set; }
        public string City { get; set; }
        public int CountryID { get; set; }
        public string Country { get; set; }

        public Customer(int customerID, string name, int addressID, string addressOne, string addressTwo, string postalCode, string phoneNumber, int cityID, string city, int countryID, string country)
        {
            CustomerID = customerID;
            Name = name;
            AddressID = addressID;
            AddressOne = addressOne;
            AddressTwo = addressTwo;
            PostalCode = postalCode;
            PhoneNumber = phoneNumber;
            CityID = cityID;
            City = city;
            CountryID = countryID;
            Country = country;
        }
    }
}
