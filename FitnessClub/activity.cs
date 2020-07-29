using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessClub
{
    class activity
    {
        public int Id { get; set; }
        public string activity_name { get; set; }
        DB db = new DB();

        public List<activity> return_activities()
        {

            SqlCommand sqlCommand = new SqlCommand("SELECT Id, activity_name FROM [Activity] ", db.GetConnection());
            db.openConnection();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            List<activity> activities = new List<activity>();
            while (reader.Read())
            {
                activities.Add(new activity());
                activities[activities.Count - 1] = new activity { Id = Convert.ToInt32(reader[0]), activity_name = reader[1].ToString() };
            }
            reader.Close();
            db.closeConnection();

            return activities;
        }
    }
}
