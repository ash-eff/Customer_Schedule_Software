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
		private string errorMessage;
		private string errorTitle;

		public Form1()
		{
			InitializeComponent();
			this.StartPosition = FormStartPosition.CenterScreen;
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
					errorMessage = "Error: User name and password do not match.";
					errorTitle = "Unknown User";
					this.Text = "Login";
					break;
				case "gd": //Scottish Gaelic
					lblUserName.Text = "Ainm-cleachdaiche";
					lblPassword.Text = "Facal-faire";
					btnLogin.Text = "Logadh a-steach";
					this.Text = "Logadh a-steach";
					errorMessage = "Mearachd: Chan eil an t-ainm-cleachdaidh agus am facal-faire co-ionnan.";
					errorTitle = "Cleachdaiche neo-aithnichte";
					break;
				default:
					lblUserName.Text = "User Name";
					lblPassword.Text = "Password";
					btnLogin.Text = "Login";
					errorMessage = "Error: User name and password do not match.";
					errorTitle = "Unknown User";
					this.Text = "Login";
					break;
			}
        }

        private void InitializeDatabaseConnection()
        {
			string connectingString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
			connection = new MySqlConnection(connectingString);
        }

		private void MatchUserNameAndPassword()
        {
			string query = "SELECT * FROM user";
			MySqlCommand command = new MySqlCommand(query, connection);

			try
			{
				connection.Open();

				using (MySqlDataReader reader = command.ExecuteReader())
				{
					bool userFound = false;
					while (reader.Read())
					{
						string userName = reader.GetString("userName");
						string password = reader.GetString("password");
						if (userName == textBox1.Text && password == textBox2.Text)
						{
							userFound = true;
						}
					}
					if (!userFound)
					{
						MessageBox.Show(errorMessage, errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					else
					{
						var nextForm = new Form2();
						nextForm.Show();
						this.Hide();
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

		private void onTextChanged(object sender, EventArgs e)
        {
			if(textBox1.Text != "" & textBox2.Text != "")
            {
				btnLogin.Enabled = true;
            }
            else
            {
				btnLogin.Enabled = false;
			}
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
			MatchUserNameAndPassword();
		}
    }
}
