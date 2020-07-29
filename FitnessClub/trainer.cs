using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FitnessClub
{
    class trainer
    {
        public int Id { get; set; }
        public string fullname { get; set; }

        DB db = new DB();

        public List<trainer> return_trainers()
        {
            SqlCommand sqlCommand = new SqlCommand("SELECT Id, Name, Surname FROM [Trainers] ", db.GetConnection());
            db.openConnection();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            List<trainer> trainers = new List<trainer>();
            while (reader.Read())
            {
                trainers.Add(new trainer());
                trainers[trainers.Count - 1] = new trainer { Id = Convert.ToInt32(reader[0]), fullname = reader[1].ToString() + " " + reader[2].ToString() };
            }
            reader.Close();
            db.closeConnection();

            return trainers;
        }
    }
}
