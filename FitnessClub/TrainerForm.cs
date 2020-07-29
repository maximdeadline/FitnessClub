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
    public partial class TrainerForm : MetroFramework.Forms.MetroForm
    {
        DB db = new DB();
        public string id = DB.Id.ToString();
        public TrainerForm()
        {
            InitializeComponent();
            this.StyleManager = metroStyleManager1;
            metroListView1.StyleManager = metroStyleManager1;
            metroListView2.StyleManager = metroStyleManager1;
            metroTabControl1.StyleManager = metroStyleManager1;
            LoadData();
        }

        private void LoadData()
        {
            DB db = new DB();
            metroListView1.Items.Clear();
            trainer trainer = new trainer();
            List<trainer> trainers = trainer.return_trainers();
            int id = trainers.Find(item => item.fullname == (DB.Name+' '+DB.Surname)).Id;

            SqlCommand sqlCommand = new SqlCommand("select Name + ' ' + Surname as Name, review from Reviews join [User] on [User].Id = Reviews.client_id where trainer_id = @trainer_id", db.GetConnection());
            sqlCommand.Parameters.AddWithValue("@trainer_id", id);
            db.openConnection();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                ListViewItem lvi = new ListViewItem(reader[0].ToString());

                lvi.SubItems.Add(reader[1].ToString());
                metroListView1.Items.Add(lvi);
            }
            reader.Close();
            db.closeConnection();
        }

        private void TrainerForm_Load(object sender, EventArgs e)
        {
            metroLabel1.Text = DB.Name;
            metroLabel2.Text = DB.Surname;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            metroListView2.Items.Clear();
            schedule schedule = new schedule();
            schedule.fill_listview_trainer(metroListView2, DB.Id.ToString());
        }

        private void TrainerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
