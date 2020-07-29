using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FitnessClub
{
    public partial class Congestion : MetroFramework.Forms.MetroForm
    {
        public Congestion()
        {
            InitializeComponent();
            this.StyleManager = metroStyleManager1;
            LoadData();
        }

        public void LoadData()
        {
            DB db = new DB();

            chart1.Series.Clear();
            SqlCommand command = new SqlCommand("select Trainers.name + ' ' + Trainers.surname, count(schedule.Id) as 'activity' from schedule full join Trainers on schedule.trainer_id = Trainers.Id group by Trainers.name + ' ' + Trainers.surname order by activity DESC", db.GetConnection());

            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            List<string> seriesArray = new List<string>();
            List<int> pointsArray = new List<int>();
            while (reader.Read())
            {
                seriesArray.Add(reader[0].ToString());
                pointsArray.Add(Convert.ToInt32(reader[1]));
            }
            reader.Close();
            db.closeConnection();

            for (int i = 0; i< seriesArray.Count(); i++)
            {
                Series series = this.chart1.Series.Add(seriesArray[i]);
                series.Points.Add(pointsArray[i]);
            }

        }
    }
}
