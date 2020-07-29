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
    public partial class Discount : MetroFramework.Forms.MetroForm
    {
        public Discount()
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
            SqlCommand command = new SqlCommand("select Name + ' ' + Surname as 'fullname', Count from (select client_id, count(distinct schedule.activity_id) as 'Count'  from clients_activities join schedule on clients_activities.activity_id = schedule.Id group by client_id) as G join [User] on G.client_id = [User].Id where Count>2", db.GetConnection());

            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {

                data.Add(new string[2]);

                data[data.Count - 1][0] = reader[0].ToString();
                if (Convert.ToInt32(reader[1].ToString()) == 3)
                    data[data.Count - 1][1] = "5%";
                else if (Convert.ToInt32(reader[1].ToString()) == 4)
                    data[data.Count - 1][1] = "10%";
                else
                    data[data.Count - 1][1] = "15%";
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
