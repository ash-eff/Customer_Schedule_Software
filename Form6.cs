using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace software_2_c969
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            GenerateDates(DateTime.Now);
            GenerateTimes();
            cmbType.SelectedIndex = 0;
        }

        private void GenerateDates(DateTime fromDate)
        {
            DateTime startDate = fromDate;
            int numberOfDays = 30;

            for(int i = 0; i < numberOfDays; i++)
            {
                DateTime currentDate = startDate.AddDays(i);

                if(currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    cmbBoxDate.Items.Add(currentDate.ToLongDateString());
                }
            }

            if(cmbBoxDate.Items.Count > 0)
            {
                cmbBoxDate.SelectedIndex = 0;
            }
        }

        private void GenerateTimes()
        {
            TimeSpan startTime = new TimeSpan(9, 0, 0);
            TimeSpan endTime = new TimeSpan(14, 0, 0);
            TimeSpan interval = TimeSpan.FromMinutes(15);

            for(TimeSpan currentTime = startTime; currentTime < endTime; currentTime = currentTime.Add(interval))
            {
                cmbTime.Items.Add(currentTime.ToString("hh:mm tt"));
            }

            if(cmbTime.Items.Count > 0)
            {
                cmbTime.SelectedIndex = 0;
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
