using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;

namespace ИгорьДиплом
{
    public partial class Tests : Form
    {
        public Tests()
        {
            InitializeComponent();
        }

        public string SearchPath(string Path)
        {
            switch (NumberTest) {
                case 1:
                    return @AppDomain.CurrentDomain.BaseDirectory + "Tests" + @"\Test" + NumberTest.ToString();
                case 2:
                    return @AppDomain.CurrentDomain.BaseDirectory + "Tests" + @"\Test" + NumberTest.ToString();
                case 3:
                    return @AppDomain.CurrentDomain.BaseDirectory + "Tests" + @"\Test" + NumberTest.ToString();
                case 4:
                    return @AppDomain.CurrentDomain.BaseDirectory + "Tests" + @"\Test" + NumberTest.ToString();
                case 5:
                    return @AppDomain.CurrentDomain.BaseDirectory + "Tests" + @"\Test" + NumberTest.ToString();
            }
            return "Er";
        }
        public void NextQuest()
        {
            if (NumberQuest != 11) {
                if (var1.Checked)
                    Answer = 1;
                if (var2.Checked)
                    Answer = 2;
                if (var3.Checked)
                    Answer = 3;
                if (var4.Checked)
                    Answer = 4;
                Rihtg = Convert.ToInt16(File.ReadAllText(path + @"\quest" + NumberQuest.ToString() + @"\Answer.txt"));
                if (Answer == Rihtg)
                    CountR++;
                var1.Text = File.ReadAllText(path + @"\quest" + NumberQuest.ToString() + @"\var1.txt", Encoding.GetEncoding(1251));
                var2.Text = File.ReadAllText(path + @"\quest" + NumberQuest.ToString() + @"\var2.txt", Encoding.GetEncoding(1251));
                var3.Text = File.ReadAllText(path + @"\quest" + NumberQuest.ToString() + @"\var3.txt", Encoding.GetEncoding(1251));
                var4.Text = File.ReadAllText(path + @"\quest" + NumberQuest.ToString() + @"\var4.txt", Encoding.GetEncoding(1251));
                QuestionLabel.Text= File.ReadAllText(path + @"\quest" + NumberQuest.ToString() + @"\quest.txt", Encoding.GetEncoding(1251));
                NumberQuest++;
            }
            else
            {
                timer1.Stop();
                NumberQuest = 0;
                if (CountR == 10)
                    Mark = 5;
                if (CountR >= 8)
                    Mark = 4;
                if (CountR >= 5)
                    Mark = 3;
                if (CountR < 5)
                    Mark = 2;
                string connectionString = ConfigurationManager.ConnectionStrings["Db"].ConnectionString;
                string Expr = String.Format("INSERT INTO Result (Family,Name,Mark,Time,Klass) Values (N'{0}',N'{1}',{2},N'{3}',N'{4}')", Family, Name, Mark, Time, Klass);
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                var comand =new SqlCommand( Expr ,sqlConnection);
                comand.ExecuteNonQuery();
                button2.Enabled = false;
                MessageBox.Show("Ваш результат Оценка:" + Mark.ToString() + "   Время:" + label4.Text+"   Количество правильных ответов:"+CountR);
                sqlConnection.Close();
            }
        }
        //params
        public string Family;
        public string Name;
        public string Klass;
        public string Time;
        public int Mark;
        public int CountR=0;
        public int NumberTest;
        public int NumberQuest=0;
        public string path;
        public int Answer=0;
        public int Rihtg=0;
        TimeSpan time1;
        DateTime initial_time = DateTime.Now;

        private void button1_Click(object sender, EventArgs e)
        {
           
            try
            {
                timer1.Start();
                button2.Enabled = true;
                Family = textBox1.Text;
                Name = textBox2.Text;
                Klass=textBox3.Text;
                if(Family=="" || Name=="" || Klass=="")
                {
                    MessageBox.Show("Вы не заполнили одно из полей", "Ошибка", MessageBoxButtons.OK);
                    return;
                }
                timer1.Start();
                CountR = 0;
                panel1.Visible = true;
                if (radioButton1.Checked)
                    NumberTest = 1;
                if (radioButton2.Checked)
                    NumberTest = 2;
                if (radioButton3.Checked)
                    NumberTest = 3;
                if (radioButton4.Checked)
                    NumberTest = 4;
                if (radioButton5.Checked)
                    NumberTest = 5;
                path = SearchPath(path);
                var1.Text = File.ReadAllText(path + @"\quest1\var1.txt", Encoding.GetEncoding(1251));
                var2.Text = File.ReadAllText(path + @"\quest1\var2.txt", Encoding.GetEncoding(1251));
                var3.Text = File.ReadAllText(path + @"\quest1\var3.txt", Encoding.GetEncoding(1251));
                var4.Text = File.ReadAllText(path + @"\quest1\var4.txt", Encoding.GetEncoding(1251));
                QuestionLabel.Text = File.ReadAllText(path + @"\quest1\quest.txt", Encoding.GetEncoding(1251));
                NumberQuest=2;
                panel3.Visible = false;
            }
            catch (FormatException)
            {
                MessageBox.Show("Ошибка", "Ошибка", MessageBoxButtons.OK);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NextQuest();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime current_time = DateTime.Now;
            time1 = current_time - initial_time;
            label4.Text = time1.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            timer1.Stop();
            NumberQuest = 0;
        }

        private void Tests_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
            string connectionString = ConfigurationManager.ConnectionStrings["Db"].ConnectionString;
            string sqlExpr = "Select * from Result";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            var command = new SqlCommand(sqlExpr, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                object Family = reader.GetValue(1);
                object Name = reader.GetValue(2);
                object Marl = reader.GetValue(3);
                object Time = reader.GetValue(4);
                object Klass = reader.GetValue(5);
                dataGridView1.Rows.Add(Family, Name, Marl, Time, Klass);
            }
            connection.Close(); 
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
            panel1.Visible = false;
            panel2.Visible = false;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            try
            {
                string search = textBox4.Text;
                string connectionString = ConfigurationManager.ConnectionStrings["Db"].ConnectionString;
                string sqlExpr = "Select * from Result";
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                var command = new SqlCommand(sqlExpr, connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    object Family = reader.GetValue(1);
                    object Name = reader.GetValue(2);
                    object Marl = reader.GetValue(3);
                    object Time = reader.GetValue(4);
                    object Klass = reader.GetValue(5);
                    if (comboBox1.SelectedIndex == 0)
                    { 
                        if(Family.ToString().Contains(search))
                        dataGridView1.Rows.Add(Family, Name, Marl, Time, Klass);
                    }
                    if (comboBox1.SelectedIndex == 1)
                    {
                        if (Klass.ToString().Contains(search))
                            dataGridView1.Rows.Add(Family, Name, Marl, Time, Klass);
                    }
                }
                connection.Close();                
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка");
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
