using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FitnessClub
{
    class schedule
    {
        DB db = new DB();
        public List<string[]> return_schedule(string id)
        {
            SqlCommand command = new SqlCommand("SELECT * FROM [schedule] where activity_id = '" + id + "'", db.GetConnection());


            trainer trainer = new trainer();
            List<trainer> trainers = trainer.return_trainers();


            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {

                data.Add(new string[6]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = trainers.Find(item => item.Id == (Convert.ToInt32(reader[1].ToString()))).fullname;
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString().Substring(0, 5);
                data[data.Count - 1][4] = reader[4].ToString();
                data[data.Count - 1][5] = reader[5].ToString();
            }
            reader.Close();
            db.closeConnection();

            return data;
        }

        public List<string[]> return_schedule_trainer(string id)
        {
            SqlCommand command = new SqlCommand("SELECT * FROM [schedule] where trainer_id = '" + id + "'", db.GetConnection());

            activity activity = new activity();
            List<activity> activities = activity.return_activities();

            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {

                data.Add(new string[6]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = activities.Find(item => item.Id == (Convert.ToInt32(reader[2].ToString()))).activity_name;
                data[data.Count - 1][3] = reader[3].ToString().Substring(0, 5);
                data[data.Count - 1][4] = reader[4].ToString();
                data[data.Count - 1][5] = reader[5].ToString();
            }
            reader.Close();
            db.closeConnection();

            return data;
        }

        public List<string[]> return_schedule_client()
        {
            SqlCommand command = new SqlCommand("select B.Id, name + ' ' + surname as fullname, activity_name, date, start_time, end_time, cost, payment " +
                "from (select A.Id, trainer_id, date, start_time, end_time, activity_name, cost, payment " +
                "from (select [clients_activities].Id, schedule.activity_id, schedule.trainer_id,schedule.date, schedule.start_time, schedule.end_time, payment " +
                "from [clients_activities] join schedule on [clients_activities].activity_id = schedule.Id " +
                "where [clients_activities].client_id = @client_id) as A join Activity on A.activity_id = Activity.Id) as B " +
                "join Trainers on B.trainer_id = Trainers.Id", db.GetConnection());
            command.Parameters.AddWithValue("@client_id", DB.Id);

            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {

                data.Add(new string[8]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString().Substring(0, 5);
                data[data.Count - 1][4] = reader[4].ToString();
                data[data.Count - 1][5] = reader[5].ToString();
                data[data.Count - 1][6] = reader[6].ToString();
                data[data.Count - 1][7] = reader[7].ToString();
            }
            reader.Close();
            db.closeConnection();

            return data;
        }

        public List<string> return_date_list()
        {
            SqlCommand command1 = new SqlCommand("SELECT date, count(id) FROM [schedule] group by date ", db.GetConnection());
            db.openConnection();
            SqlDataReader reader1 = command1.ExecuteReader();
            List<string> date_list = new List<string>();

            while (reader1.Read())
            {
                date_list.Add(reader1[0].ToString().Substring(0, 5));
            }
            reader1.Close();
            db.closeConnection();

            return date_list;
        }

        public List<string> return_date_list_client()
        {
            SqlCommand command1 = new SqlCommand("SELECT date, count(B.Id)" +
                "from (select A.Id, trainer_id, date, start_time, end_time, activity_name, cost, payment " +
                "from (select [clients_activities].Id, schedule.activity_id, schedule.trainer_id,schedule.date, schedule.start_time, schedule.end_time, payment " +
                "from [clients_activities] join schedule on [clients_activities].activity_id = schedule.Id " +
                "where [clients_activities].client_id = @client_id) as A join Activity on A.activity_id = Activity.Id) as B " +
                "join Trainers on B.trainer_id = Trainers.Id group by date ", db.GetConnection());
            command1.Parameters.AddWithValue("@client_id", DB.Id);

            db.openConnection();
            SqlDataReader reader1 = command1.ExecuteReader();
            List<string> date_list = new List<string>();

            while (reader1.Read())
            {
                date_list.Add(reader1[0].ToString().Substring(0, 5));
            }
            reader1.Close();
            db.closeConnection();

            return date_list;
        }

        public void lv_add_group(List<string> date_list, ListView lv)
        {
            string k = "";
            int m;
            string[] month = new string[12] { "Января", "Февраля", "Марта", "Апреля", "Мая", "Июня", "Июля", "Августа", "Сентября", "Октября", "Ноября", "Декабря", };
            foreach (string i in date_list)
            {
                m = Convert.ToInt32(i.Substring(3, 2));
                if (Convert.ToInt32(i.Substring(0, 2)) > 9)
                    k = i.Substring(0, 2) + " " + month[Convert.ToInt32(m) - 1];
                else
                    k = i.Substring(1, 1) + " " + month[Convert.ToInt32(m) - 1];
                lv.Groups.Add(i, k);
                k = "";
            }
        }

        public void lv_add_item(List<string[]> data, ListView lv)
        {
            foreach (string[] s in data)
            {
                ListViewItem lvi = new ListViewItem(s[0]);

                lvi.Group = lv.Groups[s[3]];

                lvi.SubItems.Add(s[1]);
                lvi.SubItems.Add(s[4]);
                lvi.SubItems.Add(s[5]);
                lv.Items.Add(lvi);
            }
        }

        public void lv_add_item_trainer(List<string[]> data, ListView lv)
        {
            foreach (string[] s in data)
            {
                ListViewItem lvi = new ListViewItem(s[2]);

                lvi.Group = lv.Groups[s[3]];

                lvi.SubItems.Add(s[4]);
                lvi.SubItems.Add(s[5]);
                lv.Items.Add(lvi);
            }
        }

        public void lv_add_item_client(List<string[]> data, ListView lv)
        {
            foreach (string[] s in data)
            {
                ListViewItem lvi = new ListViewItem(s[0]);

                lvi.Group = lv.Groups[s[3]];

                lvi.SubItems.Add(s[1]);
                lvi.SubItems.Add(s[2]);
                lvi.SubItems.Add(s[4]);
                lvi.SubItems.Add(s[5]);
                lvi.SubItems.Add(s[6]);
                /*if (s[7] == "0")
                    lvi.SubItems.Add("Нет");
                else
                    lvi.SubItems.Add("Есть");*/
                lv.Items.Add(lvi);
            }
        }

        public void fill_listview(ListView lv, string activity_id)
        {
            List<string[]> data = return_schedule(activity_id);
            List<string> date_list = return_date_list();
            lv_add_group(date_list, lv);
            lv_add_item(data, lv);
        }

        public void fill_listview_trainer(ListView lv, string activity_id)
        {
            List<string[]> data = return_schedule_trainer(activity_id);
            List<string> date_list = return_date_list();
            lv_add_group(date_list, lv);
            lv_add_item_trainer(data, lv);
        }

        public void fill_listview_client(ListView lv)
        {
            List<string[]> data = return_schedule_client();
            List<string> date_list = return_date_list_client();
            lv_add_group(date_list, lv);
            lv_add_item_client(data, lv);
        }

    }
}
