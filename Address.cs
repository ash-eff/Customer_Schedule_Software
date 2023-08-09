using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace software_2_c969
{
    public class Address
    {
        public int AddressId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public City City { get; set; }

        public Address(int addressID, string address1, string address2, string postalCode, string phone, City city)
        {
            AddressId = addressID;
            Address1 = address1;
            Address2 = address2;
            PostalCode = postalCode;
            Phone = phone;
            City = city;
        }
    }
}
