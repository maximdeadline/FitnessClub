using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FitnessClub
{
    public partial class Accountant : MetroFramework.Forms.MetroForm
    {
        public Accountant()
        {
            InitializeComponent();
            this.StyleManager = metroStyleManager1;
        }

        private void metroButton9_Click(object sender, EventArgs e)
        {
            Services services = new Services();
            services.Show();
        }

        private void metroButton10_Click(object sender, EventArgs e)
        {
            Congestion congestion = new Congestion();
            congestion.Show();
        }

        private void metroButton11_Click(object sender, EventArgs e)
        {
            Discount discount = new Discount();
            discount.Show();
        }

        private void metroButton12_Click(object sender, EventArgs e)
        {
            Profit profit = new Profit();
            profit.Show();
        }

        private void Accountant_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
