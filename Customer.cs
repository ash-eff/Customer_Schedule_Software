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
        public string AddressOne { get; set; }
        public string AddressTwo { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public Customer(int customerID, string name, string addressOne, string addressTwo, string postalCode, string phoneNumber, string city, string country)
        {
            CustomerID = customerID;
            Name = name;
            AddressOne = addressOne;
            AddressTwo = addressTwo;
            PostalCode = postalCode;
            PhoneNumber = phoneNumber;
            City = city;
            Country = country;
        }
    }
}
