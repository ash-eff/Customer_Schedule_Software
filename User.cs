using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace software_2_c969
{
    public class User
    { 
        public int UserId { get; set; }
        public string Name { get; set; }
        public User(int userId, string name)
        {
            UserId = userId;
            Name = name;
        }
    }
}
