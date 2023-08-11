using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace software_2_c969
{
    public partial class FormConsultantCountReports : Form
    {
        public FormConsultantCountReports()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            CountConsultants();
        }

        private void CountConsultants()
        {
            string connectingString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;

                string countryQuery = "SELECT COUNT(*) FROM user";
                command.CommandText = countryQuery;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lblNumber.Text = reader.GetInt32(0).ToString();
                    }
                }
            }
        }
    }
}
