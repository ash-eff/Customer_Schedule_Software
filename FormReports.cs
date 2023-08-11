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
    public partial class FormReports : Form
    {
        public FormReports()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnMonth_Click(object sender, EventArgs e)
        {
            var nextForm = new FormTypeReports();
            nextForm.ShowDialog(this);
        }
        private void btmConsultants_Click(object sender, EventArgs e)
        {
            var nextForm = new FormScheduleReports();
            nextForm.ShowDialog(this);
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            var nextForm = new FormConsultantCountReports();
            nextForm.ShowDialog(this);
        }
    }
}
