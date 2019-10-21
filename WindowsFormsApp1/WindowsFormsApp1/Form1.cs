using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace WindowsFormsApp1
{   
    public partial class Form1 : Form
    {
        
        NpgsqlConnection con = new NpgsqlConnection("Server='localhost';Port=5432;User Id=postgres;Password=admin;Database=steam;");
        string sql= null;
        string[] sqlQ = new string[12]
            {   "SELECT * FROM users WHERE iduser=",
                "SELECT name,price FROM games",
                "SELECT idgame FROM comments INTERSECT SELECT idgame FROM gamelibrary",
                "SELECT idgame FROM games EXCEPT SELECT idgame FROM gamelibrary",
                "SELECT iduser FROM comments UNION SELECT iduser FROM gamelibrary",
                "SELECT * FROM genres,agelimit",
                "SELECT games.name, developer.name FROM games,developer WHERE games.iddeveloper = developer.iddeveloper",
                "",
                "SELECT * FROM ",
                "SELECT namecountry FROM country",
                "SELECT name FROM access",
                "SELECT table_name FROM information_schema.tables WHERE table_schema NOT IN('information_schema', 'pg_catalog')"
            };
         public Form1()
        {
            InitializeComponent();            
        }

        List<string[]> data = new List<string[]>();

        void Ins(string user)
        {
            sql = user;                     
            try
            {
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                con.Open();
                int done = 0;
                done =com.ExecuteNonQuery();
                con.Close();

                con.Open();
                sql = "INSERT INTO Purse(Value) VALUES (0)";
                com = new NpgsqlCommand(sql, con);                
                com.ExecuteNonQuery();
                con.Close();

                if(done==1)
                {
                    MessageBox.Show("Пользователь добавлен");
                }
            }
            catch (Exception ex)
            {                
                MessageBox.Show(ex.Message);
            }
            con.Close();
        }
        void DelUser(string user)
        {
            sql = user;
            try
            {
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                con.Open();
                
                com.ExecuteNonQuery();
                con.Close(); 
                MessageBox.Show("Пользователь удалён");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            con.Close();
        }
        void AddCountry(string user)
        {
            sql = user;
            try
            {
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                con.Open();

                com.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Игра добавлена");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            con.Close();
        }
        void ToComboBox(ComboBox mainBox, string mainSQL)
        {
            mainBox.Items.Clear();
            sql = mainSQL;
            NpgsqlCommand com = new NpgsqlCommand(sql, con);            
            con.Open();
            NpgsqlDataReader reader;
            reader = com.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    mainBox.Items.Add(reader.GetString(0));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    System.Windows.Forms.Application.Restart();
                }
            }
            con.Close();
        }
        void Query(DataGridView mainGrid, string mainSQL)
        {
            sql = mainSQL;
            mainGrid.Rows.Clear();
            mainGrid.Columns.Clear();
           
            NpgsqlCommand com = new NpgsqlCommand(sql, con);
            con.Open();
            NpgsqlDataReader reader = null;
             try
             {
                    reader = com.ExecuteReader();
            }            
             catch (Exception ex)
            {
                System.Windows.Forms.Application.Restart();
            }

            ArrayList nameColumns = new ArrayList();
            while (reader.Read())
            {
                try
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        nameColumns.Add(reader.GetName(i));
                    }
                    break;
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    System.Windows.Forms.Application.Restart();
                }
            }
            mainGrid.ColumnCount = nameColumns.Count;
            for (int i = 0; i < nameColumns.Count; i++)
            {
                mainGrid.Columns[i].HeaderText = nameColumns[i].ToString();
            }
            con.Close();

            con.Open();
            com = new NpgsqlCommand(sql, con);
            reader = com.ExecuteReader();//выполнение запроса
            int tmp1 = reader.FieldCount;
            mainGrid.ColumnCount = tmp1;
            int k = 0;
            while (reader.Read())
            {
                try
                {
                    mainGrid.Rows.Add();// добавление строки 
                    for (int i = 0; i < tmp1; i++)
                    {
                        object result = reader.GetValue(i);//получение значения столбца
                        object tmp = result;
                        mainGrid.Rows[k].Cells[i].Value = tmp.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    System.Environment.Exit(1);
                }
                k++;
            }
            con.Close();
        }
        void UpMail(string mainSQL, int a)
        {
            sql = mainSQL;
            try
            {
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                con.Open();

                com.ExecuteNonQuery();
                con.Close();
                if (a == 2)
                {
                    MessageBox.Show("Почта успешно изменена");
                }
                else if(a==1)
                {
                    MessageBox.Show("Баланс успешно изменён");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            con.Close();
        }        
        void BuyGame()
        {
            double a=0,b =0;
            sql = "SELECT value FROM purse WHERE iduser =" + textBox13.Text;
            NpgsqlCommand com = new NpgsqlCommand(sql, con);
            con.Open();
            NpgsqlDataReader reader;
            reader = com.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    a = reader.GetDouble(0);
                    con.Close();
                    sql = "SELECT price FROM games WHERE idgame = " + textBox14.Text;
                    com = new NpgsqlCommand(sql, con);
                    con.Open();
                    reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            b = reader.GetDouble(0);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    con.Close();
                    if (a < b)
                    {
                        MessageBox.Show("Недостаточно средств");
                    }
                    else
                    {
                        UpMail("UPDATE purse SET value = value +" + -b + " WHERE iduser =" + textBox13.Text, 1);
                        sql = "INSERT INTO GameLibrary (IdGame, IdUser) VALUES ("+textBox14.Text+","+textBox13.Text+")";
                        con.Open();
                        com = new NpgsqlCommand(sql, con);
                        com.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Покупка успешно проведена");
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            con.Close();

            

        }
        void AddGame(string q)
        {
            sql = q;
            try
            {
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                con.Open();

                com.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Игра добавлена");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                System.Windows.Forms.Application.Restart();
            }
            con.Close();
        }
    
        //void Table()
        //{           
        //    dataGridView1.Rows.Clear();
        //    dataGridView1.Columns.Clear();
        //    object a = comboBox1.SelectedItem;
        //    sql = "SELECT column_name FROM information_schema.columns WHERE table_name = '" + a + "'";            
        //    NpgsqlCommand com = new NpgsqlCommand(sql, con);
        //    con.Open();
        //    NpgsqlDataReader reader;
        //    reader = com.ExecuteReader();


        //    ArrayList nameColumns = new ArrayList();
        //    while (reader.Read())
        //    {
        //        try
        //        {
        //            nameColumns.Add(reader.GetValue(0));                   
        //        }
        //        catch { }

        //    }
        //    dataGridView1.ColumnCount = nameColumns.Count;
        //    for (int i = 0; i < nameColumns.Count; i++)
        //    {
        //        dataGridView1.Columns[i].HeaderText = nameColumns[i].ToString();
        //    }
        //    con.Close();
        //    con.Open();
        //    sql = "SELECT * FROM " + a;//строка с запросом
        //    com = new NpgsqlCommand(sql, con);
        //    reader = com.ExecuteReader();//выполнение запроса
        //    int tmp1 = reader.FieldCount;
        //    dataGridView1.ColumnCount = tmp1;
        //    int k = 0;
        //    while (reader.Read())
        //    {
        //        try
        //        {
        //            dataGridView1.Rows.Add();// добавление строки 
        //            for (int i = 0; i < tmp1; i++)
        //            {
        //                string llll = reader.GetName(k + 1);
        //                object result = reader.GetValue(i);//получение значения столбца
        //                object tmp = result;
        //                dataGridView1.Rows[k].Cells[i].Value = tmp.ToString();
        //            }
        //        }
        //        catch { }
        //        k++;
        //    }
        //    con.Close();

        //}
        //void Qure(string q)
        //{
        //    sql = q;            
        //    dataGridView2.Rows.Clear();
        //    dataGridView2.Columns.Clear();

        //    NpgsqlCommand com = new NpgsqlCommand(sql, con);
        //    con.Open();
        //    NpgsqlDataReader reader;
        //    reader = com.ExecuteReader();

        //    ArrayList nameColumns = new ArrayList();
        //    while (reader.Read())
        //    {
        //        try
        //        {
        //            for (int i = 0; i < reader.FieldCount; i++)
        //            {
        //                nameColumns.Add(reader.GetName(i));                        
        //            }
        //            break;
        //        }
        //        catch { }
        //    }
        //    dataGridView2.ColumnCount = nameColumns.Count;
        //    for (int i = 0; i < nameColumns.Count; i++)
        //    {
        //        dataGridView2.Columns[i].HeaderText = nameColumns[i].ToString();
        //    }
        //    con.Close();

        //    con.Open();           
        //    com = new NpgsqlCommand(sql, con);
        //    reader = com.ExecuteReader();//выполнение запроса
        //    int tmp1 = reader.FieldCount;
        //    dataGridView2.ColumnCount = tmp1;
        //    int k = 0;
        //    while (reader.Read())
        //    {
        //        try
        //        {
        //            dataGridView2.Rows.Add();// добавление строки 
        //            for (int i = 0; i < tmp1; i++)
        //            {
        //                object result = reader.GetValue(i);//получение значения столбца
        //                object tmp = result;
        //                dataGridView2.Rows[k].Cells[i].Value = tmp.ToString();
        //            }
        //        }
        //        catch { }
        //        k++;
        //    }
        //    con.Close();
        //}
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox2.Items.Add("Выборка: пользователя по ID");
            comboBox2.Items.Add("Проекция: вывод только имя и цену игр");
            comboBox2.Items.Add("Пересечение: пересечение таблиц: библиотека и комментарии. Вывод id купленных и комментируемых игр");
            comboBox2.Items.Add("Разность: Вывод id игр которые ещё не покупали");
            comboBox2.Items.Add("Объединение: таблиц коментарии и библиотека, вывод id пользователей хотя бы раз прокомментировавших игры");
            comboBox2.Items.Add("Произведение: каждому жанру может соответствовать любое возрастное ограничение");
            comboBox2.Items.Add("Соединение: вывод названия игры и разработчика игры");
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Query(dataGridView1, sqlQ[8]+comboBox1.SelectedItem);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_MouseHover(object sender, EventArgs e)
        {
            ToComboBox(comboBox1, sqlQ[11]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(comboBox2.SelectedIndex==0)
            {

                Query(dataGridView2,sqlQ[0] + textBox1.Text);
            }
            switch (comboBox2.SelectedIndex)
            {
                case 1:
                    Query(dataGridView2, sqlQ[1]); 
                    break;
                case 2:
                    Query(dataGridView2, sqlQ[2]);
                    break;
                case 3:
                    Query(dataGridView2, sqlQ[3]);
                    break;
                case 4:
                    Query(dataGridView2, sqlQ[4]);
                    break;
                case 5:
                    Query(dataGridView2, sqlQ[5]);
                    break;
                case 6:
                    Query(dataGridView2, sqlQ[6]);
                    break;               
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Query(dataGridView3, richTextBox1.Text);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_MouseMove(object sender, MouseEventArgs e)
        {
            ToComboBox(comboBox3, sqlQ[9]);            
        }
       
        private void comboBox4_MouseMove(object sender, MouseEventArgs e)
        {
            ToComboBox(comboBox4, sqlQ[10]);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string user = "INSERT INTO Users(Login, Password, IdCountry, Nickname, Mail, IdAccess) VALUES"
             + "('" + textBox2.Text + " ', '" + textBox3.Text + "'," + (comboBox4.SelectedIndex + 1) + ", '" + textBox4.Text + "', '" + textBox5.Text + "'," + (comboBox4.SelectedIndex+1) +")";
            Ins(user);
           // MessageBox.Show(user);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            DelUser("DELETE FROM users WHERE iduser=" + textBox6.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
           AddCountry("INSERT INTO Country (NameCountry) VALUES ('" + textBox7.Text + "')");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Query(dataGridView4, "SELECT name FROM games " +
            "LEFT JOIN gamelibrary ON games.idgame = gamelibrary.idgame " +
            "LEFT JOIN users ON gamelibrary.iduser = users.iduser " +
            "WHERE users.iduser = '" + textBox8.Text + "'");           
        }

        private void button8_Click(object sender, EventArgs e)
        {
            UpMail("UPDATE users SET mail = '"+ textBox10.Text + "' WHERE iduser =" + textBox9.Text,2);
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            UpMail("UPDATE purse SET value = value +" + textBox12.Text + " WHERE iduser =" + textBox11.Text, 1);           
        }

        private void comboBox5_MouseHover(object sender, EventArgs e)
        {
            

        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView5_MouseHover(object sender, EventArgs e)
        {
            Query(dataGridView5, "SELECT idgame,name,price FROM games");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            BuyGame();
        }

        private void comboBox5_MouseHover_1(object sender, EventArgs e)
        {
            ToComboBox(comboBox5, "SELECT namegenre FROM genres");
        }

        private void comboBox6_MouseHover(object sender, EventArgs e)
        {
            ToComboBox(comboBox6, "SELECT name FROM developer");
        }

        private void comboBox7_MouseHover(object sender, EventArgs e)
        {
            ToComboBox(comboBox7, "SELECT name FROM publisher");
        }

        private void comboBox8_MouseHover(object sender, EventArgs e)
        {
           
                ToComboBox(comboBox8, "SELECT name FROM agelimit");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string q = "INSERT INTO Games(Name, idDeveloper, idpublisher, idgenre, price, idagelimit) VALUES " +
                "('" + textBox15.Text + "', '" + (comboBox6.SelectedIndex + 1)+"', " +
                (comboBox7.SelectedIndex + 1)+", " + (comboBox5.SelectedIndex + 1) + ", " +
                textBox16.Text + ", " +( comboBox8.SelectedIndex + 1) + ")";
            AddCountry(q);
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
