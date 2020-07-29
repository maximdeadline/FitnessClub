using MetroFramework;
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
    public partial class ClientForm : MetroFramework.Forms.MetroForm
    {
        DB db = new DB();
        public ClientForm()
        {
            InitializeComponent();
            this.StyleManager = metroStyleManager1;
            metroListView1.StyleManager = metroStyleManager1;
            metroListView2.StyleManager = metroStyleManager1;
            metroListView3.StyleManager = metroStyleManager1;
            metroTabControl1.StyleManager = metroStyleManager1;
            metroLabel1.Text = DB.Name;
            metroLabel2.Text = DB.Surname;
            LoadSchedule();
        }

        public void LoadSchedule()
        {
            metroListView2.Items.Clear();
            schedule schedule = new schedule();
            schedule.fill_listview_client(metroListView2);
        }

        private void combo1_click(object sender, EventArgs e)
        {
            activity activity = new activity();
            List<activity> activities = activity.return_activities();

            metroComboBox1.DataSource = activities;
            metroComboBox1.DisplayMember = "activity_name";
            metroComboBox1.ValueMember = "Id";
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            schedule schedule = new schedule();
            schedule.fill_listview(metroListView1, metroComboBox1.SelectedValue.ToString());
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO [clients_activities](client_id,activity_id,payment) VALUES (@client_id, @activity_id, @payment)", db.GetConnection());
                command.Parameters.AddWithValue("@client_id", DB.Id);
                command.Parameters.AddWithValue("@activity_id", Convert.ToInt32(metroTextBox1.Text));
                command.Parameters.AddWithValue("@payment", 0);

                db.openConnection();
                command.ExecuteNonQuery();
                MessageBox.Show("Вы успешно записались на занятие", "Добавление занятия", MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);

                db.closeConnection();

                metroTextBox1.Text = "";
            }
            catch (Exception)
            {
                MessageBox.Show("Запись не прошла");
            }

        }

       
        private void metroComboBox2_MouseClick(object sender, MouseEventArgs e)
        {
            trainer trainer = new trainer();
            List<trainer> trainers = trainer.return_trainers();

            metroComboBox2.DataSource = trainers;
            metroComboBox2.DisplayMember = "fullname";
            metroComboBox2.ValueMember = "Id";
                        
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {            
            metroListView3.Items.Clear();
            string[] words = metroComboBox2.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            metroLabel7.Text = words[0];
            metroLabel6.Text = words[1];

            SqlCommand sqlCommand = new SqlCommand("select Name + ' ' + Surname as Name, review from Reviews join [User] on [User].Id = Reviews.client_id where trainer_id = @trainer_id", db.GetConnection());
            sqlCommand.Parameters.AddWithValue("@trainer_id", Convert.ToInt32(metroComboBox2.SelectedValue));
            db.openConnection();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                ListViewItem lvi = new ListViewItem(reader[0].ToString());

                lvi.SubItems.Add(reader[1].ToString());
                metroListView3.Items.Add(lvi);
            }
            reader.Close();
            db.closeConnection();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            if (metroComboBox2.Text == "")
                MessageBox.Show("Выберите тренера");
            else
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("INSERT INTO [Reviews] (client_id, trainer_id, review) VALUES (@client_id, @trainer_id, @review)", db.GetConnection());
                    sqlCommand.Parameters.AddWithValue("@client_id", DB.Id);
                    sqlCommand.Parameters.AddWithValue("@trainer_id", Convert.ToInt32(metroComboBox2.SelectedValue));
                    sqlCommand.Parameters.AddWithValue("@review", richTextBox1.Text);
                    db.openConnection();
                    if (sqlCommand.ExecuteNonQuery() == 1)
                        MessageBox.Show("Вы успешно добавили отзыв", "Отзыв", MessageBoxButtons.OK,
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

        private void metroButton5_Click(object sender, EventArgs e)
        {
            LoadSchedule();
        }

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
