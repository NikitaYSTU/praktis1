using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace practice3
{
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();
        }

        private void dia_Click(object sender, EventArgs e)
        {
            Form1 dia = new Form1();
            dia.ShowDialog();
        }

        private void categorybtn_Click(object sender, EventArgs e)
        {
            category cat = new category();
            cat.ShowDialog();
        }

        private void docbtn_Click(object sender, EventArgs e)
        {
            doctor doc = new doctor();
            doc.ShowDialog();
        }

        private void patientbtn_Click(object sender, EventArgs e)
        {
            patient pat = new patient();
            pat.ShowDialog();
        }

        private void perdatbtn_Click(object sender, EventArgs e)
        {
            personal_data pd = new personal_data();
            pd.ShowDialog();
        }

        private void pricebtn_Click(object sender, EventArgs e)
        {
            prices pr = new prices();
            pr.ShowDialog();
        }

        private void specbtn_Click(object sender, EventArgs e)
        {
            speciality sp = new speciality();
            sp.ShowDialog();
        }

        private void tickbtn_Click(object sender, EventArgs e)
        {
            ticket tk = new ticket();
            tk.ShowDialog();
        }

        private void visbtn_Click(object sender, EventArgs e)
        {
            visit vs = new visit();
            vs.ShowDialog();
        }

        private void vispurbtn_Click(object sender, EventArgs e)
        {
            visit_purpose vsp = new visit_purpose();
            vsp.ShowDialog();
        }
    }
}
