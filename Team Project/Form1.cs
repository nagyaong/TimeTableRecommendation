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
using System.Media;
using System.Text.RegularExpressions;

namespace Team_Project
{
    public partial class Form1 : Form
    {
        const int btn_width = 74; //버튼 너비
        const int btn_height = 39; //버튼 높이
        const int b_h2 = btn_height/3*2; //버튼 높이
        const int bx = 70; //시간표 시작 위치 x좌표
        const int by = 110; //시간표 시작 위치 y좌표
        const int bn_c = 6; //시간표 열 개수
        const int bn_r = 10; //시간표 행 개수
        const int l_w = 37; //리스트뷰 학점 등 너비
        const int l_w3= 49; //리스트뷰 교수명 너비
        const int c_n = 11;//색개수
        int y_n=5;//채워진 요일
        int x_n=6;//채워진 교시
        Button[] btn; //시간표 구성할 버튼배열

        int credit;//현재 학점
        int maxC = 0;       // 최대학점

        int[,] color = new int[11,3] { { 220, 234, 250 }, 
                                      { 250, 188, 182 },
                                      {207,250,250 },
                                      { 250,225,182},
                                      {195,250,219 },
                                      { 145,171,199},
                                      {199,149,145 },
                                      {177,214,227 },
                                      {177,227,213 },
                                      {199,225,255 },
                                      {64,104,148 }
        };
        
        int[] c = Enumerable.Repeat<int>(0, c_n).ToArray<int>();

        public Form1()
        {            
            InitializeComponent();
            lvw_Subject_Init();
            this.Load += button;
        }

        private void init_combo()
        {
            chkBox_1.Checked = false;
            chkBox_2.Checked = false;
            chkBox_3.Checked = false;
            chkBox_4.Checked = false;
            chkBox_5.Checked = false;
            chkBox_6.Checked = false;
            chkBox_7.Checked = false;
            chkBox_8.Checked = false;
            chkBox_9.Checked = false;
            chkBox_10.Checked = false;

            chkBox_Mon.Checked = false;
            chkBox_Tue.Checked = false;
            chkBox_Wed.Checked = false;
            chkBox_Thu.Checked = false;
            chkBox_Fri.Checked = false;
            chkBox_Sat.Checked = false;
        }

        private void lvw_Subject_Init()
        {
            txtbox_Credit.Text = credit.ToString(); //이수학점 초기화
            txtbox_Credit.TextAlign = HorizontalAlignment.Center; //이수학점 가운데 정렬

            lvw_Subject.View = View.Details;

            //리스트 뷰 칼럼 추가 
            lvw_Subject.Columns.Add("SubNum", "학정번호");
            lvw_Subject.Columns.Add("SubName", "과목명");
            lvw_Subject.Columns.Add("Class", "분반");
            lvw_Subject.Columns.Add("Type", "이수");
            lvw_Subject.Columns.Add("Credit", "학점");
            lvw_Subject.Columns.Add("How_many", "시수");
            lvw_Subject.Columns.Add("Prof", "교수");
            lvw_Subject.Columns.Add("Day1", "요일");
            lvw_Subject.Columns.Add("Time1", "시간");
            lvw_Subject.Columns.Add("Day2", "요일");
            lvw_Subject.Columns.Add("Time2", "시간");
            lvw_Subject.Columns.Add("note", "비고");
            lvw_Subject.Columns.Add("last", "");

            //칼럼별 너비 지정
            lvw_Subject.Columns[1].Width = l_w*2; 
            lvw_Subject.Columns[2].Width = l_w; 
            lvw_Subject.Columns[3].Width = l_w;
            lvw_Subject.Columns[4].Width = l_w;
            lvw_Subject.Columns[5].Width = l_w;
            lvw_Subject.Columns[6].Width = l_w3;
            lvw_Subject.Columns[7].Width = l_w;
            lvw_Subject.Columns[8].Width = l_w;
            lvw_Subject.Columns[9].Width = l_w;
            lvw_Subject.Columns[10].Width = l_w;

            //칼럼 정렬
            for (int i = 0; i < lvw_Subject.Columns.Count; i++)
            {
                lvw_Subject.Columns[i].TextAlign = HorizontalAlignment.Center;
            }
        }

        //버튼 생성,초기화,배치하는 부분
        private void button(object sender, EventArgs e) 
        {
            int x = bx;
            int y = by;

            btn = new Button[bn_r*bn_c];

            for (int i = 0; i < bn_r * bn_c; i++)
            {
                btn[i] = new Button();
                btn[i].Parent = this;
                btn[i].Text = "";
                if (i >= bn_c * 6)
                    btn[i].Size = new Size(btn_width, b_h2);
                else
                    btn[i].Size = new Size(btn_width, btn_height);

                btn[i].Location = new Point(x,y);
                btn[i].Name = i.ToString();

                x += btn_width;

                if ((i + 1) % bn_c == 0)
                {
                    if (i >= bn_c * 6)
                        y += b_h2;
                    else
                        y += btn_height;
                    x = bx;
                }
                this.Controls.Add(btn[i]);
                btn[i].Click += new System.EventHandler(button_Click);
            }
        }

        // 각 버튼 시간대를 문자열로 변환
        public string button_string(Button b)
        {
            char day = 'A';
            char time = '1';

            for (int j = 0; j < 10; j++)     // 교시
            {
                for (int i = 0; i < 6; i++)         // 요일
                {
                    if (b == btn[(j * 6) + i])
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(day);
                        sb.Append(time);

                        return sb.ToString();
                    }
                    day = (char)((int)day + 1);
                }

                day = 'A';
                time = (char)((int)time + 1);
            }
            return "";
        }

        //기피시간 지정
        private void button_Click(object sender, EventArgs e)       
        {
            Button button = sender as Button;
            if (button.Text == "")
            {
                button.Text = "XXX";

                for (int j = 0; j < 9; j++)     // 교시
                {
                    for (int i = 0; i < 6; i++)         // 요일
                    {
                        if (button == btn[(j * 6) + i])
                        {
                            for (int a = 0; a < lvw_Subject.Items.Count; a++)       // 전부 검사
                            {
                                string s = lvw_Subject.Items[a].Text;

                                if (s.Contains(button_string(button)) && s.Contains("Z"))
                                {
                                    lvw_Subject.Items.RemoveAt(a);
                                    a--;
                                }
                            }
                        }
                    }
                }
            }
            else if (button.Text == "XXX")
            {
                button.Text = "";

                some_read(button_string(button));
            }
            else
            { MessageBox.Show("이미 과목을 추가한 시간대입니다."); }      /* 과목 있는 버튼 누를경우, 추가했음 */
        }

        //확인 버튼 
        private void button1_Click(object sender, EventArgs e) 
        {
            if (string.IsNullOrEmpty(cmbBox_Major.Text) || string.IsNullOrEmpty(cmbBox_Grade.Text) || string.IsNullOrEmpty(cmbBox_Max_Credit.Text)){
                MessageBox.Show("전공 학년 학점을 기입해주세요");
            }
            else {
                lvw_Subject.Clear();
                lvw_Subject_Init();
                read();
                init_combo();
            }
        }

        //특정 csv 파일 읽어오기 
        public void some_read(string str)
        {
            Encoding encode = System.Text.Encoding.GetEncoding("ks_c_5601-1987");
            FileStream stream_general = File.Open("2020-1_교양.csv", FileMode.Open, FileAccess.Read);

            using (var csv = new StreamReader(stream_general, encode))
            {
                csv.ReadLine();

                List<string> file = new List<string>();
                while (!csv.EndOfStream)
                {
                    file.Add(csv.ReadLine());
                }

                foreach (string item in file)
                {
                    var value = item.Split(',');
                    ListViewItem lvi = new ListViewItem(value[0]);

                    if (value[0].Contains(str))
                    {
                        for (int i = 1; i < 11; i++)
                            lvi.SubItems.Add(value[i]);
                        lvw_Subject.Items.Add(lvi);
                    }
                }

                lvw_Subject.EndUpdate();
            }
        }

        //csv 파일 읽어오기
        private void read() 
        {
            Encoding encode = System.Text.Encoding.GetEncoding("ks_c_5601-1987");
            string filename = "2020-1_";
            //file path 중 공통 부분 
            string common;          
            common = filename;
            filename += cmbBox_Major.Text;
            filename += "_";
            filename += cmbBox_Grade.Text;
            filename += "학년.csv";
            common += "공통_";
            common += cmbBox_Grade.Text;
            common+= "학년.csv";

            //전공 
            FileStream stream_major = File.Open(filename, FileMode.Open, FileAccess.Read);
            //소융대 공통 
            FileStream stream_common = File.Open(common, FileMode.Open, FileAccess.Read);
            //교양 
            FileStream stream_general = File.Open("2020-1_교양.csv", FileMode.Open, FileAccess.Read);

            using (var csv = new StreamReader(stream_major,encode))
            {
                csv.ReadLine();
                
                List<string> file = new List<string>();
                while (!csv.EndOfStream)
                {
                    file.Add(csv.ReadLine());
                }
                
                lvw_Subject.BeginUpdate();

                foreach (string item in file)
                {
                    var value = item.Split(',');
                    ListViewItem lvi = new ListViewItem(value[0]);

                    //전필 과목 추가
                    if (value[0].Contains("X"))
                    {
                        for (int i = 1; i < 12; i++)
                            lvi.SubItems.Add(value[i]);

                        lvw_Subject.Items.Add(lvi);
                    }
                }
                foreach (string item in file)
                {
                    var value = item.Split(',');
                    ListViewItem lvi = new ListViewItem(value[0]);

                    //교필 및 기필 
                    if (value[0].Contains("W"))
                    {
                        for (int i = 1; i < 12; i++)
                        {
                            //MessageBox.Show(value[i]);
                            lvi.SubItems.Add(value[i]);
                        }

                        lvw_Subject.Items.Add(lvi);
                    }
                }

                foreach (string item in file)
                {
                    var value = item.Split(',');
                    ListViewItem lvi = new ListViewItem(value[0]);

                    //전선
                    if (value[0].Contains("Y"))
                    {
                        for (int i = 1; i < 12; i++)
                        {
                            lvi.SubItems.Add(value[i]);
                        }

                        lvw_Subject.Items.Add(lvi);
                    }
                }

                
            }
            stream_major.Close();


            //소융대 공통 과목 csv 파일 읽기
            using (var csv = new StreamReader(stream_common, encode))
            {
                csv.ReadLine();

                List<string> file = new List<string>();
                while (!csv.EndOfStream)
                {
                    file.Add(csv.ReadLine());
                }

                foreach (string item in file)
                {
                    var value = item.Split(',');
                    ListViewItem lvi = new ListViewItem(value[0]);

                    //전필 
                    if (value[0].Contains("X"))
                    {
                        for (int i = 1; i < 12; i++)
                            lvi.SubItems.Add(value[i]);

                        lvw_Subject.Items.Add(lvi);
                    }
                }
                foreach (string item in file)
                {
                    var value = item.Split(',');
                    ListViewItem lvi = new ListViewItem(value[0]);

                    //교필 및 기필
                    if (value[0].Contains("W"))
                    {
                        for (int i = 1; i < 12; i++)
                        {
                            lvi.SubItems.Add(value[i]);
                        }
                        lvw_Subject.Items.Add(lvi);
                    }
                }

            }

            //교양 과목 csv 파일 읽기 
            using (var csv = new StreamReader(stream_general, encode))
             {
                 csv.ReadLine();

                 List<string> file = new List<string>();
                 while (!csv.EndOfStream)
                 {
                     file.Add(csv.ReadLine());
                 }

                 foreach (string item in file)
                 {
                     var value = item.Split(',');
                     ListViewItem lvi = new ListViewItem(value[0]);

                    //교필 및 기필 
                     if (value[0].Contains("W"))
                     {
                         for (int i = 1; i < 12; i++)
                         {
                             lvi.SubItems.Add(value[i]);
                         }
                         lvw_Subject.Items.Add(lvi);
                     }
                 }
                 foreach (string item in file)
                 {
                     var value = item.Split(',');
                     ListViewItem lvi = new ListViewItem(value[0]);

                    //교선
                     if (value[0].Contains("Z"))
                     {
                         for (int i = 1; i < 12; i++)
                             lvi.SubItems.Add(value[i]);

                         lvw_Subject.Items.Add(lvi);
                     }
                 }

             }

            //file 닫고 리스트 뷰 갱신
            stream_general.Close();
            lvw_Subject.EndUpdate();

        }


        //더블클릭으로 시간표에 추가
        private void lvw_Subject_DoubleClick(object sender, EventArgs e) 
        {
            string item = "";

            string[] s = new string[lvw_Subject.Columns.Count-1];
            ListViewItem l = lvw_Subject.FocusedItem;

            for (int i = 0; i < lvw_Subject.Columns.Count-1; i++)
            {
                s[i] = l.SubItems[i].Text;
                item += l.SubItems[i].Text;
                item += " ";
            }
            int sub_n = s[0].Length-2;
            //인강
            if (sub_n == 0)
            {
                //중복 추가 검사 
                for(int i = 0; i < cmbBox_List_Online.Items.Count; i++)
                {
                    if (item == cmbBox_List_Online.Items[i].ToString())
                    {
                        MessageBox.Show("이미 추가되었습니다.");
                        return;
                    }
                }
                //추가 메세지 
                cmbBox_List_Online.Items.Add(item);
                MessageBox.Show("추가되었습니다.");
            }
            
            //현강 
            else
            {
                char[] num = new char[sub_n];
                num = s[0].Substring(2, sub_n).ToCharArray();
                int[] text = new int[sub_n / 2];
                int j = 0;
                int ind = 0;

                //현재 요일
                int c_y =0;
                //현재 교시
                int c_x =0;

                while (j < sub_n)
                {
                    int m = (int)num[j + 1]-49;
                    int o = (int)num[j] - 65;
                    //10교시 처리
                    if (j+2<sub_n&&(int)num[j + 1] == 49 && (int)num[j + 2] == 48)
                        m = 58-49;
                    if (m+1 > c_x)
                        c_x = m+1;
                    if (o+1 > c_y)
                        c_y = o+1;

                    int n = o + ((m) * bn_c);
                    text[ind++] = n;

                    if (j + 2 >= sub_n - 1)
                        break;
                    j += 2;
                }
               
                //중복 시간대 추가 검사 
                for (int i = 0; i < sub_n / 2; i++)
                {
                    int nn = text[i];
                    if (btn[nn].Text != "" && btn[nn].Text != "XXX")        /* 조건 변경한 부분 */
                    {
                        MessageBox.Show("이미 같은 시간대에 수업이 추가되어 있습니다.");
                        return;
                    }
                }
                int ci = 0;
                for (int k = 0; k < c_n; k++)
                {
                    if (c[k] == 0)
                    {
                        ci = k;
                        c[k] = 1;
                        break;
                    }
                }
                 
                for (int i = 0; i < sub_n / 2; i++)
                {
                    int nn = text[i];
                    
                    btn[nn].Text = s[1];
                    int c1, c2, c3;
                    
                    c1 = color[ci, 0];
                    c2 = color[ci, 1];
                    c3 = color[ci, 2];
                    btn[nn].BackColor = Color. FromArgb(c1,c2,c3);

                    if (c_y > y_n)
                        y_n = c_y;

                    if (c_x > x_n) 
                        x_n = c_x;
                }
                item += ci.ToString();

                //내 선택 과목에 추가 
                cmbBox_list_my_Subject.Items.Add(item);
            }

            //이수 학점 설정 
            txtbox_Credit.Text = (int.Parse(txtbox_Credit.Text) + int.Parse(s[4])).ToString();

            //이수 학점 갱신 
            credit = Convert.ToInt32(txtbox_Credit.Text);

            //최대 학점 초과 시 빨간색으로 알림 
            if (credit > maxC)
                txtbox_Credit.ForeColor = Color.Red;
        }

        
        //지우기 버튼 
        private void button2_Click(object sender, EventArgs e)
        {
            if (cmbBox_list_my_Subject.Items.Count == 0)
                MessageBox.Show("시간표가 비어있습니다.");
            else if (cmbBox_list_my_Subject.SelectedItem == null)
                MessageBox.Show("선택한 과목이 없습니다.");
            else
            {
                string item = cmbBox_list_my_Subject.SelectedItem.ToString();
                int ci = int.Parse(item.Substring(item.Length - 1, 1));

                c[ci]=0;
                string[] it = item.Split(' ');               

                int sub_n = it[0].Length-1;
                char[] num = new char[sub_n];
                num = item.Substring(1, sub_n).ToCharArray();
                int n;
                int j = 1;
                while (j < sub_n)
                {
                    
                    int m = (int)num[j + 1]-49;
                    int o = (int)num[j] - 65;
                    if (j + 2 < sub_n && (int)num[j + 1] == 49 && (int)num[j + 2] == 48)//10교시 처리
                        m = 58-49;

                    n = o + ((m) * bn_c);

                    btn[n].Text = "";
                    btn[n].BackColor = Control.DefaultBackColor;

                    btn[n].UseVisualStyleBackColor = true;
                    if (j + 2 >= sub_n - 1)
                        break;
                    j += 2;
                }

                cmbBox_list_my_Subject.Items.Remove(item);
                cmbBox_list_my_Subject.Text = "";

                txtbox_Credit.Text = (int.Parse(txtbox_Credit.Text) - ((int)num[0] - 48)).ToString();

                int credit = Convert.ToInt32(txtbox_Credit.Text);
                if (maxC > credit)
                    txtbox_Credit.ForeColor = Color.Black;
            }
        }

        private int how_many(string s)
        {
            int count = 0;
            string[] cmb_list_my_Subject_pre = new string[cmbBox_list_my_Subject.Items.Count];

            for (int i = 0; i < cmbBox_list_my_Subject.Items.Count; i++)
            {
                cmb_list_my_Subject_pre[i] = cmbBox_list_my_Subject.Items[i].ToString();
                string[] cmb_list_my_Subject = cmb_list_my_Subject_pre[i].Split(' ');

                for (int j = 0; j < cmbBox_list_my_Subject.Items.Count; j++)
                    if (cmb_list_my_Subject[0].Contains(s))
                    {
                        count++;
                    }
            }
            return count;
        }

        //시간표 저장 버튼 
        private void button3_Click(object sender, EventArgs e)
        {
            //파일 저장시 교시 지정            
            if (how_many("10")>0)
                x_n = 10;
            else if (how_many("9") > 0)
                 x_n = 9;
            else if (how_many("8") > 0)
                x_n = 8;
            else if (how_many("7") > 0)
                x_n = 7;
            else
                 x_n = 6;

            if (how_many("F") > 0)
                y_n = 6;
            else
                y_n = 5;

            int height = (x_n - 10) * b_h2;
            int width = (y_n - 6)*btn_width;

            Bitmap bmp = new Bitmap(groupBox2.Width+width,groupBox2.Height+groupBox1.Height+height-7);
            int x = 0;
            int y = 0;
            int i = 0;

            groupBox2.DrawToBitmap(bmp, new Rectangle(0,0,groupBox2.Width,groupBox2.Height));
            y = groupBox2.Height;
            groupBox1.DrawToBitmap(bmp, new Rectangle(x, y-6, groupBox1.Width, groupBox1.Height));
            x = groupBox1.Width;

            foreach(Control c in btn)
            {
                if (y_n == 5)
                {
                    if ((i + 1) % bn_c == 0)
                    {
                        i++;
                        continue;

                    }
                }
                
                c.DrawToBitmap(bmp,new Rectangle(x,y, btn_width, btn_height));
                x += btn_width;
                int p = 2;

                if (y_n == 6)
                    p = 1;
                if ((i + p) % bn_c == 0)
                {
                    if (i >= bn_c * 6)
                        y += b_h2;
                    else
                        y += btn_height;

                    x = groupBox1.Width;
                }
                i++;
            }

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            //저장할 확장자명
            saveFileDialog1.Filter = "JPG File(.jpg)|*.jpg|Bitmap File (.bmp)|*.bmp|" +
                "PNG File (.png)|*.png|GIF File (.gif)|*.gif";  

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;
                bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                MessageBox.Show("저장되었습니다");
            }
        }


        /*
         기피 요일 설정
        */
        private void chkBox_Mon_CheckedChanged(object sender, EventArgs e)
        {          
            if (chkBox_Mon.Checked == true)
            {
                //모두 검사 
                for (int a = 0; a < lvw_Subject.Items.Count; a++)      
                {
                    string s = lvw_Subject.Items[a].Text;
                    if (s.Contains("A") && s.Contains("Z"))
                    {
                        lvw_Subject.Items.RemoveAt(a);
                        a--;
                    }
                }
            }
            else
                some_read("A");
        }
        private void chkBox_Tue_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_Tue.Checked == true)
            {
                // 전부 검사
                for (int a = 0; a < lvw_Subject.Items.Count; a++)       
                {
                    string s = lvw_Subject.Items[a].Text;
                    if (s.Contains("B") && s.Contains("Z"))
                    {
                        lvw_Subject.Items.RemoveAt(a);
                        a--;
                    }
                }
            }
            else
                some_read("B");
        }
        private void chkBox_Wed_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_Wed.Checked == true)
            {
                // 전부 검사
                for (int a = 0; a < lvw_Subject.Items.Count; a++)       
                {
                    string s = lvw_Subject.Items[a].Text;
                    if (s.Contains("C") && s.Contains("Z"))
                    {
                        lvw_Subject.Items.RemoveAt(a);
                        a--;
                    }
                }
            }
            else
                some_read("C");
        }
        private void chkBox_Thu_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_Thu.Checked == true)
            {
                for (int a = 0; a < lvw_Subject.Items.Count; a++)       // 전부 검사
                {
                    string s = lvw_Subject.Items[a].Text;
                    if (s.Contains("D") && s.Contains("Z"))
                    {
                        lvw_Subject.Items.RemoveAt(a);
                        a--;
                    }
                }
            }
            else
                some_read("D");
        }
        private void chkBox_Fri_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_Fri.Checked == true)
            {
                for (int a = 0; a < lvw_Subject.Items.Count; a++)       // 전부 검사
                {
                    string s = lvw_Subject.Items[a].Text;
                    if (s.Contains("E") && s.Contains("Z"))
                    {
                        lvw_Subject.Items.RemoveAt(a);
                        a--;
                    }
                }
            }
            else
                some_read("E");
        }
        private void chkBox_Sat_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_Sat.Checked == true)
            {
                for (int a = 0; a < lvw_Subject.Items.Count; a++)       // 전부 검사
                {
                    string s = lvw_Subject.Items[a].Text;
                    if (s.Contains("F") && s.Contains("Z"))
                    {
                        lvw_Subject.Items.RemoveAt(a);
                        a--;
                    }
                }
            }
            else
                some_read("F");
        }


        /*
        기피 시간대 설정
        */
        private void chkBox_1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_1.Checked == true)
            {
                for (int a = 0; a < lvw_Subject.Items.Count; a++)       // 전부 검사
                {
                    string s = lvw_Subject.Items[a].Text;
                    if (s.Contains("1") && s.Contains("Z"))
                    {
                        lvw_Subject.Items.RemoveAt(a);
                        a--;
                    }
                }
            }
            else
                some_read("1");
        }
        private void chkBox_2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_2.Checked == true)
            {
                for (int a = 0; a < lvw_Subject.Items.Count; a++)       // 전부 검사
                {
                    string s = lvw_Subject.Items[a].Text;
                    if (s.Contains("2") && s.Contains("Z"))
                    {
                        lvw_Subject.Items.RemoveAt(a);
                        a--;
                    }
                }
            }
            else
                some_read("2");
        }
        private void chkBox_3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_3.Checked == true)
            {
                for (int a = 0; a < lvw_Subject.Items.Count; a++)       // 전부 검사
                {
                    string s = lvw_Subject.Items[a].Text;
                    if (s.Contains("A3") || s.Contains("B3") || s.Contains("C3") || s.Contains("D3") || s.Contains("E3"))
                    {
                        if (s.Contains("Z"))
                        {
                            lvw_Subject.Items.RemoveAt(a);
                            a--;
                        }
                    }
                }
            }
            else
            {
                some_read("A3"); some_read("B3"); some_read("C3"); some_read("D3"); some_read("E3");
            }
        }
        private void chkBox_4_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_4.Checked == true)
            {
                for (int a = 0; a < lvw_Subject.Items.Count; a++)       // 전부 검사
                {
                    string s = lvw_Subject.Items[a].Text;
                    if (s.Contains("4") && s.Contains("Z"))
                    {
                        lvw_Subject.Items.RemoveAt(a);
                        a--;
                    }
                }
            }
            else
                some_read("4");
        }
        private void chkBox_5_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_5.Checked == true)
            {
                for (int a = 0; a < lvw_Subject.Items.Count; a++)       // 전부 검사
                {
                    string s = lvw_Subject.Items[a].Text;
                    if (s.Contains("5") && s.Contains("Z"))
                    {
                        lvw_Subject.Items.RemoveAt(a);
                        a--;
                    }
                }
            }
            else
                some_read("5");
        }
        private void chkBox_6_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_6.Checked == true)
            {
                for (int a = 0; a < lvw_Subject.Items.Count; a++)       // 전부 검사
                {
                    string  s = lvw_Subject.Items[a].Text;
                    if (s.Contains("6") && s.Contains("Z"))
                    {
                        lvw_Subject.Items.RemoveAt(a);
                        a--;
                    }
                }
            }
            else
                some_read("6");
        }
        private void chkBox_7_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_7.Checked == true)
            {
                for (int a = 0; a < lvw_Subject.Items.Count; a++)       // 전부 검사
                {
                    string s = lvw_Subject.Items[a].Text;
                    if (s.Contains("7") && s.Contains("Z"))
                    {
                        lvw_Subject.Items.RemoveAt(a);
                        a--;
                    }
                }
            }
            else
                some_read("7");
        }
        private void chkBox_8_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_8.Checked == true)
            {
                for (int a = 0; a < lvw_Subject.Items.Count; a++)       // 전부 검사
                {
                    string s = lvw_Subject.Items[a].Text;
                    if (s.Contains("8") && s.Contains("Z"))
                    {
                        lvw_Subject.Items.RemoveAt(a);
                        a--;
                    }
                }
            }
            else
                some_read("8");
        }
        private void chkBox_9_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_9.Checked == true)
            {
                for (int a = 0; a < lvw_Subject.Items.Count; a++)       // 전부 검사
                {
                    string s = lvw_Subject.Items[a].Text;
                    if (s.Contains("9") && s.Contains("Z"))
                    {
                        lvw_Subject.Items.RemoveAt(a);
                        a--;
                    }
                }
            }
            else
                some_read("9");
        }
        private void chkBox_10_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_10.Checked == true)
            {
                for (int a = 0; a < lvw_Subject.Items.Count; a++)       // 전부 검사
                {
                    string s = lvw_Subject.Items[a].Text;
                    if (s.Contains("10") && s.Contains("Z"))
                    {
                        lvw_Subject.Items.RemoveAt(a);
                        a--;
                    }
                }
            }
            else
                some_read("10");
        }

        // 최대학점 콤보박스
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            maxC = Convert.ToInt32(cmbBox_Max_Credit.SelectedItem.ToString());
        }


        //시간표 초기화
        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < bn_c*bn_r; i++)
            {
                btn[i].Text = "";
                //콤보 박스 비우기 
                cmbBox_list_my_Subject.Items.Clear();
                cmbBox_List_Online.Items.Clear();
                btn[i].BackColor = Control.DefaultBackColor;

                btn[i].UseVisualStyleBackColor = true;
            }

            this.Load -= button;
            this.Load += button;

            lvw_Subject.Clear();
            lvw_Subject_Init();
            read();

            credit = 0;
            txtbox_Credit.Text = credit.ToString();
            txtbox_Credit.ForeColor = Color.Black;

            
            y_n = 5;
            x_n = 6;

            //c배열(색) 초기화
            c = Enumerable.Repeat<int>(0, c_n).ToArray<int>();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size=new Size(1000, 566);
        }

        // 인강 지우기 버튼
        private void button5_Click(object sender, EventArgs e)
        {
            if (cmbBox_List_Online.Items.Count == 0)
                MessageBox.Show("추가된 인강이 없습니다.");
            else if (cmbBox_List_Online.SelectedItem == null)
                MessageBox.Show("선택한 과목이 없습니다.");
            else
            {

                string item = cmbBox_List_Online.SelectedItem.ToString();

                string[] it = item.Split(' ');

                int sub_n = it[0].Length - 1;

                char[] num = new char[sub_n];
                num = item.Substring(1, sub_n).ToCharArray();

                cmbBox_List_Online.Items.Remove(item);
                cmbBox_List_Online.Text = "";

                txtbox_Credit.Text = (int.Parse(txtbox_Credit.Text) - ((int)num[0] - 48)).ToString();

                int credit = Convert.ToInt32(txtbox_Credit.Text);
                if (maxC > credit)
                    txtbox_Credit.ForeColor = Color.Black;
            }
        }       
    }
}
