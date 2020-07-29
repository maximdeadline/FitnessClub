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
    public partial class DirectorForm : MetroFramework.Forms.MetroForm
    {
        DB db = new DB();
        public DirectorForm()
        {
            InitializeComponent();
            this.StyleManager = metroStyleManager1;
            metroTab.StyleManager = metroStyleManager1;
            metroGrid1.StyleManager = metroStyleManager1;
            metroListView1.StyleManager = metroStyleManager1;
            metroListView2.StyleManager = metroStyleManager1;
            LoadData();
            LoadActivity();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(metroTextBox2.Text) | string.IsNullOrWhiteSpace(metroTextBox2.Text) |
                string.IsNullOrEmpty(metroTextBox1.Text) | string.IsNullOrWhiteSpace(metroTextBox1.Text) |
                string.IsNullOrEmpty(metroTextBox5.Text) | string.IsNullOrWhiteSpace(metroTextBox5.Text))
            {
                MessageBox.Show("Введены не верные значения", "Ошибка", MessageBoxButtons.OK,
                                 MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                                 MessageBoxOptions.DefaultDesktopOnly);
            }
            else
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("INSERT INTO [Trainers] (name, surname, phone," +
                                            " adress, passport) VALUES (@Name, @Surname, @phone, @adress," +
                                            " @passport)", db.GetConnection());

                    sqlCommand.Parameters.AddWithValue("Name", metroTextBox1.Text);
                    sqlCommand.Parameters.AddWithValue("Surname", metroTextBox2.Text);
                    sqlCommand.Parameters.AddWithValue("phone", metroTextBox3.Text);
                    sqlCommand.Parameters.AddWithValue("adress", metroTextBox4.Text);
                    sqlCommand.Parameters.AddWithValue("passport", metroTextBox5.Text);

                    db.openConnection();

                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Вы успешно добавили тренера", "Добавление тренера", MessageBoxButtons.OK,
                                     MessageBoxIcon.Information);

                    metroTextBox1.Text = "";
                    metroTextBox2.Text = "";
                    metroTextBox3.Text = "";
                    metroTextBox4.Text = "";
                    metroTextBox5.Text = "";
                    db.closeConnection();
                }                
                catch (Exception)
                {
                    MessageBox.Show("Добавление не прошло");
                }
            }
        }

        
        private void LoadData()
        {
            metroGrid1.Rows.Clear();
            SqlCommand sqlCommand = new SqlCommand("SELECT [User].Id, [User].Name, [User].Surname, phone, adress, [User].passport FROM [Trainers] left join [User] on [Trainers].passport = [User].passport ", db.GetConnection());
            db.openConnection();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                data.Add(new string[6]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();
                data[data.Count - 1][4] = reader[4].ToString();
                data[data.Count - 1][5] = reader[5].ToString();
            }
            reader.Close();
            db.closeConnection();

            foreach (string[] s in data)
                metroGrid1.Rows.Add(s);
        }

        public void LoadActivity()
        {
            metroListView1.Items.Clear();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Activity]", db.GetConnection());
            db.openConnection();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            List<string[]> activ = new List<string[]>();
            while (reader.Read())
            {
                activ.Add(new string[3]);

                activ[activ.Count - 1][0] = reader[0].ToString();
                activ[activ.Count - 1][1] = reader[1].ToString();
                activ[activ.Count - 1][2] = reader[2].ToString();
            }
            reader.Close();
            db.closeConnection();

            foreach (string[] s in activ)
            {
                ListViewItem lvi = new ListViewItem(s[0]);
                lvi.SubItems.Add(s[1]);
                lvi.SubItems.Add(s[2]);
                metroListView1.Items.Add(lvi);
            }
        }

        private void DirectorForm_Load(object sender, EventArgs e)
        {
            
        }


        private void metroButton2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Уволить тренера?",
            "Сообщение",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                string id = metroTextBox6.Text;

                SqlCommand command1 = new SqlCommand("Select [Trainers].Id from [Trainers] left join [User] on [Trainers].passport = [User].passport where [User].Id = @id", db.GetConnection());
                command1.Parameters.AddWithValue("id", metroTextBox6.Text);


                db.openConnection();

                SqlDataReader reader1 = command1.ExecuteReader();
                string tr_id = "";
                if (reader1.HasRows)
                {
                    reader1.Read();
                    tr_id = reader1[0].ToString();
                    reader1.Close();
                }
                else
                {
                    MessageBox.Show("Такого Id в базе данных нет");
                    reader1.Close();
                    return;
                }

                SqlCommand command2 = new SqlCommand("delete from [Trainers] where Id ='" + tr_id + "'", db.GetConnection());
                command2.ExecuteNonQuery();

                SqlCommand command = new SqlCommand("delete from [User] where Id = @id", db.GetConnection());
                command.Parameters.AddWithValue("id", id);
                command.ExecuteNonQuery();

                MessageBox.Show("Вы успешно уволили тренера", "Увольнение тренера", MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);

                db.closeConnection();
                metroGrid1.Rows.Clear();
                LoadData();

            }
        }

        private void combo1_click(object sender, MouseEventArgs e)
        {
            trainer trainer = new trainer();
            List<trainer> trainers = trainer.return_trainers();

            metroComboBox1.DataSource = trainers;
            metroComboBox1.DisplayMember = "fullname";
            metroComboBox1.ValueMember = "Id";

        }

        private void combo2_click(object sender, MouseEventArgs e)
        {
            activity activity = new activity();
            List<activity> activities = activity.return_activities();

            metroComboBox2.DataSource = activities;
            metroComboBox2.DisplayMember = "activity_name";
            metroComboBox2.ValueMember = "Id";
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(metroComboBox1.Text) | string.IsNullOrWhiteSpace(metroComboBox1.Text) |
               string.IsNullOrEmpty(metroComboBox2.Text) | string.IsNullOrWhiteSpace(metroComboBox2.Text) |
               string.IsNullOrEmpty(metroComboBox3.Text) | string.IsNullOrWhiteSpace(metroComboBox3.Text) |
               string.IsNullOrEmpty(metroComboBox4.Text) | string.IsNullOrWhiteSpace(metroComboBox4.Text) |
               string.IsNullOrEmpty(metroComboBox5.Text) | string.IsNullOrWhiteSpace(metroComboBox5.Text) |
               string.IsNullOrEmpty(metroComboBox6.Text) | string.IsNullOrWhiteSpace(metroComboBox6.Text))
            {
                MessageBox.Show("Введены не верные значения", "Ошибка", MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("INSERT INTO [schedule] (trainer_id, activity_id, date," +
                                            " start_time, end_time) VALUES (@trainer_id, @activity_id, @date," +
                                            " @start_time, @end_time)", db.GetConnection());

                    string date = metroDateTime1.Value.ToShortDateString();
                    string cdate = date.Substring(3, 2) + '.' + date.Substring(0, 2) + '.' + date.Substring(6, 4);
                    sqlCommand.Parameters.AddWithValue("trainer_id", metroComboBox1.SelectedValue.ToString());
                    sqlCommand.Parameters.AddWithValue("activity_id", metroComboBox2.SelectedValue.ToString());
                    sqlCommand.Parameters.AddWithValue("date", cdate);
                    sqlCommand.Parameters.AddWithValue("start_time", metroComboBox3.Text.ToString() + ":" + metroComboBox4.Text.ToString());
                    sqlCommand.Parameters.AddWithValue("end_time", metroComboBox6.Text.ToString() + ":" + metroComboBox5.Text.ToString());

                    db.openConnection();

                    if (sqlCommand.ExecuteNonQuery() == 1)
                        MessageBox.Show("Вы успешно добавили занятие в расписание", "Добавление занятие", MessageBoxButtons.OK,
                                     MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Добавление не прошло");

                    db.closeConnection();
                }                
                catch (Exception)
                {
                    MessageBox.Show("Добавление не прошло");
                }
            }
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            LoadActivity();
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Удалить занятие?",
            "Сообщение",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                db.openConnection();
              
                SqlCommand command = new SqlCommand("delete from [Activity] where Id ='" + metroTextBox7.Text + "'", db.GetConnection());
                command.ExecuteNonQuery();

                MessageBox.Show("Вы успешно удалили занятие", "Удаление занятия", MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);

                db.closeConnection();
                LoadActivity();

            }
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(metroTextBox8.Text) | string.IsNullOrWhiteSpace(metroTextBox8.Text) |
                string.IsNullOrEmpty(metroTextBox10.Text) | string.IsNullOrWhiteSpace(metroTextBox10.Text))
            {
                MessageBox.Show("Введены не верные значения", "Ошибка", MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("INSERT INTO [Activity] (activity_name, cost) VALUES (@activity_name, @cost)", db.GetConnection());

                    sqlCommand.Parameters.AddWithValue("activity_name", metroTextBox8.Text);
                    sqlCommand.Parameters.AddWithValue("cost", Convert.ToInt32(metroTextBox10.Text));

                    db.openConnection();
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Вы успешно добавили занятие", "Добавление занятия", MessageBoxButtons.OK,
                                     MessageBoxIcon.Information);

                    db.closeConnection();
                    LoadActivity();
                }
                catch (Exception)
                {
                    MessageBox.Show("Добавление не прошло");
                }
            }
        }

        private void metro7_click(object sender, MouseEventArgs e)
        {
            activity activity = new activity();
            List<activity> activities = activity.return_activities();

            metroComboBox7.DataSource = activities;
            metroComboBox7.DisplayMember = "activity_name";
            metroComboBox7.ValueMember = "Id";
        }

        private void metroComboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void metroButton7_Click(object sender, EventArgs e)
        {
            metroListView2.Items.Clear();

            schedule schedule = new schedule();
            schedule.fill_listview(metroListView2, metroComboBox7.SelectedValue.ToString());
            
        }

        private void DirectorForm_SizeChanged(object sender, EventArgs e)
        {
            metroPanel1.Location = new System.Drawing.Point(((this.Width - metroPanel1.Width)/2)-10, 0);
        }

        private void metroButton8_Click(object sender, EventArgs e)
        {            
            DialogResult result = MessageBox.Show("Удалить занятие?",
            "Сообщение",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                db.openConnection();

                SqlCommand command = new SqlCommand("delete from [schedule] where Id ='" + metroTextBox9.Text + "'", db.GetConnection());
                command.ExecuteNonQuery();

                MessageBox.Show("Вы успешно удалили занятие", "Удаление занятия", MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);

                db.closeConnection();
                LoadActivity();

            }
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

        private void DirectorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void metroButton13_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
