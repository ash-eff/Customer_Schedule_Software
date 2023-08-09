using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace software_2_c969
{
    public class City
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }

        public City(int cityID, string name, Country country)
        {
            CityId = cityID;
            Name = name;
            Country = country;
        }
    }
}
