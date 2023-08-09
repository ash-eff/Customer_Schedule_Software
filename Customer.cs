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
        public Customer(int customerID, string name, Address address)
        {
            CustomerID = customerID;
            Name = name;
            Address = address;
        }
    }
}
