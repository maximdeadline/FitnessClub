using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FitnessClub
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public Form1()
        {
            InitializeComponent();
            this.StyleManager = metroStyleManager1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT * FROM [User] WHERE Login = '" + metroTextBox1.Text + "' and Password = '" + metroTextBox2.Text + "'", db.GetConnection());
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                DB.Id = Convert.ToInt32(dt.Rows[0][0]);
                DB.Name = dt.Rows[0][1].ToString();
                DB.Surname = dt.Rows[0][2].ToString();
                if (dt.Rows[0][6].ToString()=="Тренер")
                {
                    TrainerForm f = new TrainerForm();
                    f.Show();
                    this.Hide();
                }
                else if (dt.Rows[0][6].ToString() == "Директор")
                {
                    DirectorForm f = new DirectorForm();
                    f.Show();
                    this.Hide();
                }
                else if (dt.Rows[0][6].ToString() == "Клиент")
                {
                    ClientForm f = new ClientForm();
                    f.Show();
                    this.Hide();
                }
                else 
                {
                    Accountant f = new Accountant();
                    f.Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Не правильный логин или пароль");
            }
        }
    }
}
