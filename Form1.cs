using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace software_2_c969
{
	public partial class Form1 : Form
	{
		private MySqlConnection connection;

		public Form1()
		{
			InitializeComponent();
			InitializeDatabaseConnection();
			SetLanguageForUser(GetLanguageCode());
			
		}

		private string GetLanguageCode()
        {
			string userLanguageCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
			return userLanguageCode;
		}

		private void SetLanguageForUser(string languageCode)
        {
			Console.WriteLine(languageCode);
			switch (languageCode)
            {
				case "en": //English
					lblUserName.Text = "User Name";
					lblPassword.Text = "Password";
					btnLogin.Text = "Login";
					this.Text = "Login";
					break;
				case "gd": //Scottish Gaelic
					lblUserName.Text = "Ainm-cleachdaiche";
					lblPassword.Text = "Facal-faire";
					btnLogin.Text = "Logadh a-steach";
					this.Text = "Logadh a-steach";
					break;
				default:
					lblUserName.Text = "User Name";
					lblPassword.Text = "Password";
					btnLogin.Text = "Login";
					this.Text = "Login";
					break;
			}
        }

        private void InitializeDatabaseConnection()
        {
			string connectingString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
			connection = new MySqlConnection(connectingString);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
			// might want to make sure that something is in username and password fields
			string query = "SELECT * FROM user";
			MySqlCommand command = new MySqlCommand(query, connection);

			try
			{
				connection.Open();

				using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
						// do stuff to check user name and password here
						Console.WriteLine(reader["userName"].ToString());
                    }
                }
			}
			catch (MySqlException ex)
			{
				MessageBox.Show("Error: " + ex.Message);
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
		}
    }
}
