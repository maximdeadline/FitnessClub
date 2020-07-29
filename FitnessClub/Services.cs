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
    public partial class Services : MetroFramework.Forms.MetroForm
    {
        DB db = new DB();
        public Services()
        {
            InitializeComponent();
            this.StyleManager = metroStyleManager1;
            LoadData();
        }

        public void LoadData()
        {
            metroListView1.Items.Clear();
            SqlCommand command = new SqlCommand("select activity_name, Count from (select schedule.activity_id, count(clients_activities.Id) as 'Count' from clients_activities inner join schedule on clients_activities.activity_id = schedule.Id group by schedule.activity_id) as A join Activity on A.activity_id = Activity.Id order by Count DESC", db.GetConnection());

            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {

                data.Add(new string[2]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
            }
            reader.Close();
            db.closeConnection();

            foreach (string[] s in data)
            {
                ListViewItem lvi = new ListViewItem(s[0]);


                lvi.SubItems.Add(s[1]);
                metroListView1.Items.Add(lvi);
            }
        }
    }
}
