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
        public Address Address { get; set; }
        public string Address1 { get { return Address?.Address1; } }
        public string Address2 { get { return Address?.Address2; } }
        public string PostalCode { get { return Address?.PostalCode; } }
        public string Phone { get { return Address?.Phone; } }
        public string City { get { return Address?.City.Name; } }
        public string Country { get { return Address?.City.Country.Name; } }

        public Customer(int customerID, string name, Address address)
        {
            CustomerID = customerID;
            Name = name;
            Address = address;
        }
    }
}
