using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace software_2_c969
{
    public class Country
    {
        public int CountryID { get; set; }
        public string Name { get; set; }

        public Country(int countryID, string name)
        {
            CountryID = countryID;
            Name = name;
        }
    }
}
