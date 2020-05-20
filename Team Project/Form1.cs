using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;


namespace Team_Project
{
    public partial class Form1 : Form
    {
       // DataGridTextBoxColumn textBoxColumn = new DataGridTextBoxColumn();
        Button[] btn;
        public Form1()
        {
            InitializeComponent();
            //datagridview_make();
            this.Load += button;
        }
        private void button(object sender, EventArgs e)
        {
            int x = 68;
            int y = 102;
            btn = new Button[5*6];
            int j = 0;
            for (int i = 0; i < 5*6; i++)
            {
                btn[i] = new Button();
                btn[i].Parent = this;
                btn[i].Text = "";
                btn[i].Size = new Size(79, 40);
                btn[i].Location = new Point(x, y);
                int k = (i ) / 5+1;
                btn[i].Name = i.ToString();// ((char)(j + 65)).ToString() + k.ToString();
                //btn[i].Text = btn[i].Name;
                j++;
                
               // btn[i].BackColor = Color.LightGray;
                //btn[i].ForeColor = Color.LightGray;
                btn[i].MouseDown += new MouseEventHandler(btn_Click);

                x += 79;
                if ((i + 1) % 5 == 0)
                {
                    y += 40;
                    x = 68;
                    j = 0;
                }
               /* panel1.Controls.Add(btn[i]);
                panel1.Visible = false;
                panel1.BackColor = Color.Transparent;*/
                this.Controls.Add(btn[i]);

            }
        }
        public void btn_Click(object sender, MouseEventArgs e)
        {
            /*Button btnn = sender as Button;
            int index = int.Parse(btnn.Name);

            if (btnn.Text != "")
                btnn.Text = "";*///이렇게 하면 같은 과목 다른시간이 남아있게 됨
        }

       /* public void datagridview_make()
        {
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
        }*/

        private void button1_Click(object sender, EventArgs e)
        {
            /* string str_FilePath = "SW_sample.xlxs";
             string str_Provider = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + str_FilePath + @";Extended Properties='Excel 12.0;HDR=No'";

             OleDbConnection oleDb_Conn = new OleDbConnection(str_Provider);
             oleDb_Conn.Open();

             string str_Query = "SELECT * FROM [Sheet1$]";
             OleDbCommand oleDb_Comm = new OleDbCommand(str_Query, oleDb_Conn);
             OleDbDataAdapter data_Adapter = new OleDbDataAdapter(oleDb_Comm);
             System.Data.DataTable table = new DataTable();
             data_Adapter.Fill(table);

             oleDb_Conn.Close();


             list_subject.Items.Clear();

             foreach(DataRow row in table.Rows)
             {
                 list_subject.Items.Add(row.ToString());
             }
             */
            read();


        }

        private void read()
        {
            Encoding encode = System.Text.Encoding.GetEncoding("ks_c_5601-1987");
            FileStream stream = File.Open("sample.csv", FileMode.Open, FileAccess.Read);

            using (var csv = new StreamReader(stream,encode))
            {
                csv.ReadLine();
                //string line;
               /* while (!csv.EndOfStream)
                {
                    list_subject.Items.Add(csv.ReadLine());
                }
                */
                List<string> file = new List<string>();
                while (!csv.EndOfStream)
                {
                    file.Add(csv.ReadLine());
                }
                foreach (string item in file)
                {
                    list_subject.Items.Add(item);
                }
            }
        }

        private void list_subject_DoubleClick(object sender, EventArgs e)
        {
            string item= list_subject.SelectedItem.ToString();
            char[] num=new char[4];
            num = item.Substring(2, 4).ToCharArray();

            int n = (int)num[0] - 65 + (((int)num[1] - 49) * 5);
            btn[n].Text = item.Substring(7, 6).Replace(",", "");

             n = (int)num[2] - 65 + (((int)num[3] - 49) * 5);
            btn[n].Text = item.Substring(7, 6).Replace(",","");

            comboBox1.Items.Add(item);
        }

        private void comboBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string item = comboBox1.SelectedItem.ToString();
            char[] num = new char[4];
            num = item.Substring(2, 4).ToCharArray();

            int n = (int)num[0] - 65 + (((int)num[1] - 49) * 5);
            btn[n].Text = "";

            n = (int)num[2] - 65 + (((int)num[3] - 49) * 5);
            btn[n].Text = "";
            comboBox1.Items.Remove(item);
            comboBox1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(79*5, 40*6);
            int x = 0;
            int y = 0;
            int i = 0;
            foreach(Control c in btn)
            {
                c.DrawToBitmap(bmp,new Rectangle(x,y, 79, 40));
                x += 79;
                if ((i + 1) % 5 == 0)
                {
                    y += 40;
                    x = 0;
                    
                }
                i++;
            }
            //this.panel1.DrawToBitmap(bmp, new Rectangle(0, 0, this.panel1.Width, this.panel1.Height));
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPG File(.jpg)|*.jpg|Bitmap File (.bmp)|*.bmp|PNG File (.png)|*.png|GIF File (.gif)|*.gif"; //저장할 확장자명 
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;
                bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                MessageBox.Show("저장되었습니다");
            }
        }
    }
}
