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
    public partial class Registration : MetroFramework.Forms.MetroForm
    {
        DB db = new DB();
        public Registration()
        {
            InitializeComponent();
            this.StyleManager = metroStyleManager1;
        }

        private void Registration_Load(object sender, EventArgs e)
        {

        }

        public bool checkLogin()
        {
            bool f = false;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT count(*) FROM [User] WHERE Login = '" + metroTextBox3.Text + "'", db.GetConnection());
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);
            if (dt.Rows[0][0].ToString() == "1")
            {
                f = true;
            }
            else
            {
                f = false;
            }
            return f;
        }
        public bool checkTrener()
        {
            db.openConnection();
            bool f = false;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT count(*) FROM [Trainers] WHERE passport = '" + metroTextBox6.Text + "'", db.GetConnection());
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);
            if (dt.Rows[0][0].ToString() == "1")
            {
                f = false;
            }
            else
            {
                f = true;
            }
            db.closeConnection();
            return f;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(metroTextBox5.Text) | string.IsNullOrWhiteSpace(metroTextBox5.Text) |
                string.IsNullOrEmpty(metroTextBox4.Text) | string.IsNullOrWhiteSpace(metroTextBox4.Text) |
                string.IsNullOrEmpty(metroTextBox3.Text) | string.IsNullOrWhiteSpace(metroTextBox3.Text) |
                string.IsNullOrEmpty(metroTextBox2.Text) | string.IsNullOrWhiteSpace(metroTextBox2.Text) |
                string.IsNullOrEmpty(metroTextBox1.Text) | string.IsNullOrWhiteSpace(metroTextBox1.Text))
            {
                MessageBox.Show("Введены не верные значения", "Ошибка", MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
            }
            else if ((metroTextBox1.Text != metroTextBox2.Text))
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButtons.OK,
                                     MessageBoxIcon.Error);
            }
            else if (checkLogin())
            {
                MessageBox.Show("Такой логин уже существует", "Ошибка", MessageBoxButtons.OK,
                                     MessageBoxIcon.Error);
            }
            else if ((string.IsNullOrEmpty(metroTextBox6.Text) | string.IsNullOrWhiteSpace(metroTextBox6.Text)) & (metroComboBox1.Text == "Тренер"))
            {
                MessageBox.Show("Введите паспортные данные", "Ошибка", MessageBoxButtons.OK,
                                     MessageBoxIcon.Error);
            }
            else if (checkTrener() & (metroComboBox1.Text == "Тренер"))
            {
                MessageBox.Show("Вас еще не приняли на работу", "Ошибка", MessageBoxButtons.OK,
                                     MessageBoxIcon.Error);
            }            
            else
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("INSERT INTO [User] (Name, Surname, Login," +
                                            " Password, DateOfRegistration, position, passport) VALUES (@Name, @Surname, @Login, @Password," +
                                            " @DateOfRegistration, @position, @passport)", db.GetConnection());

                    sqlCommand.Parameters.AddWithValue("Name", metroTextBox5.Text);
                    sqlCommand.Parameters.AddWithValue("Surname", metroTextBox4.Text);
                    sqlCommand.Parameters.AddWithValue("Login", metroTextBox3.Text);
                    sqlCommand.Parameters.AddWithValue("Password", metroTextBox2.Text);
                    sqlCommand.Parameters.AddWithValue("position", metroComboBox1.Text);
                    sqlCommand.Parameters.AddWithValue("DateOfRegistration", DateTime.Now);
                    sqlCommand.Parameters.AddWithValue("passport", metroTextBox6.Text);

                    db.openConnection();

                    if (sqlCommand.ExecuteNonQuery() == 1)
                        MessageBox.Show("Вы успешно зарегестрировались", "Регистриция", MessageBoxButtons.OK,
                                     MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                                     MessageBoxOptions.DefaultDesktopOnly);
                    else
                        MessageBox.Show("Регистрация не прошла");

                    db.closeConnection();

                    this.Close();
                }                
                catch (Exception)
                {
                    MessageBox.Show("Регистрация не прошла");
                }
            }
        }
    }
}
