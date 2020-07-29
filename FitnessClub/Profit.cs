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
    public partial class Profit : MetroFramework.Forms.MetroForm
    {
        public Profit()
        {
            InitializeComponent();
            this.StyleManager = metroStyleManager1;
            metroListView1.StyleManager = metroStyleManager1;
            LoadData();
        }

        public void LoadData()
        {
            DB db = new DB();
            metroListView1.Items.Clear();
            SqlCommand command = new SqlCommand("select activity_name, sum(cost) as 'cost' from clients_activities join schedule on clients_activities.activity_id = schedule.Id join Activity on schedule.activity_id = Activity.Id group by activity_name", db.GetConnection());

            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            int profit = 0;
            while (reader.Read())
            {
                profit += Convert.ToInt32(reader[1].ToString());
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
            metroLabel1.Text = profit.ToString();
        }
    }
}
