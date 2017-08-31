using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization;
using System.Drawing.Imaging;


namespace MyPersonalCheatEngine
{
    public partial class Form1 : Form
    {
        bool table_isloaded = false;
        bool refreshing = false;
        float timer = 0;
        DataTable table = new DataTable();




        public void displayValues()
        {
            MemoryReader mem = new MemoryReader();
            String type;
            String name = "";
            int Base = 0;
            int[] offsets = { 0, 0, 0, 0, 0 };
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                type = dataGridView1.Rows[i].Cells[8].Value.ToString();
                Console.WriteLine("I am in Line Nr." + (i + 1));
                Array.Resize(ref offsets, 5);
                offsets[0] = 0;
                offsets[1] = 0;
                offsets[2] = 0;
                offsets[3] = 0;
                offsets[4] = 0;
                string hexadd = "0x";
                //base "name.exe"+008989

                Char delimiter = '+';
                String[] substrings = dataGridView1.Rows[i].Cells[1].Value.ToString().Split(delimiter);
                //substrings[1] = hexadd + substrings[1];
                substrings[0] = new string((from c in substrings[0]
                                            where char.IsLetterOrDigit(c)
                                            select c).ToArray());
                substrings[0] = substrings[0].Replace("exe", "");
                substrings[0] = substrings[0].Replace("dll", "");
                if (substrings.Count() < 2)
                {
                    name = "harald";
                    if (substrings[0].Contains("THREAD"))
                    {
                        goto leave3;
                    }
                    Console.WriteLine(substrings[0]);
                    Base = int.Parse(substrings[0], System.Globalization.NumberStyles.HexNumber);
                }
                else
                {
                    name = substrings[0];

                    Console.WriteLine(substrings[1]);
                    Base = int.Parse(substrings[1], System.Globalization.NumberStyles.HexNumber);
                }

                //offsets
                String offset;
                offset = dataGridView1.Rows[i].Cells[2].Value.ToString();
                Console.WriteLine(offset + "offset");
                if (offset != "")
                {
                    // offset = hexadd + offset;
                    Console.WriteLine("offset 1 found");
                    offsets[0] = int.Parse(offset, System.Globalization.NumberStyles.HexNumber);
                }
                else
                {
                    Console.WriteLine("offset not found");
                    goto leave2;
                }
                offset = dataGridView1.Rows[i].Cells[3].Value.ToString();
                if (offset != "")
                {
                    Console.WriteLine("offset 2 found");
                    //offset = hexadd + offset;
                    offsets[1] = int.Parse(offset, System.Globalization.NumberStyles.HexNumber);
                }
                else
                {

                    Array.Resize(ref offsets, 1);
                    goto leave;
                }
                offset = dataGridView1.Rows[i].Cells[4].Value.ToString();
                if (offset != "")
                {
                    Console.WriteLine("offset 3 found");
                    //offset = hexadd + offset;
                    offsets[2] = int.Parse(offset, System.Globalization.NumberStyles.HexNumber);
                }
                else
                {

                    Array.Resize(ref offsets, 2);
                    goto leave;
                }
                offset = dataGridView1.Rows[i].Cells[5].Value.ToString();
                if (offset != "")
                {
                    Console.WriteLine("offset 4 found");

                    //offset = hexadd + offset;

                    offsets[3] = int.Parse(offset, System.Globalization.NumberStyles.HexNumber);
                }
                else
                {
                    Array.Resize(ref offsets, 3);
                    goto leave;
                }
                offset = dataGridView1.Rows[i].Cells[6].Value.ToString();
                if (offset != "")
                {
                    Console.WriteLine("offset 5 found");
                    //offset = hexadd + offset;
                    offsets[4] = int.Parse(offset, System.Globalization.NumberStyles.HexNumber);
                }
                else
                {
                    Array.Resize(ref offsets, 4);
                    goto leave;
                }
            leave:
                if (name != "mono")
                {

                    float heelt = mem.getValue(name, Base, offsets, type);
                    dataGridView1.Rows[i].Cells[7].Value = heelt;
                    if (timer < int.Parse(textX.Text))
                    {
                        if (heelt < 0)
                        {
                            heelt *= -1;
                        }
                        chart1.Series["Series" + (i + 1).ToString()].Points.AddXY(timer, heelt);
                        timer += 0.05f;
                    }
                }
                else
                {
                    dataGridView1.Rows[i].Cells[7].Value = "Dll's not supported";
                }
                goto leave3;
            leave2:
                float heelt2 = mem.getsingleValue(name, Base, type);
                dataGridView1.Rows[i].Cells[7].Value = heelt2;
                if (timer < int.Parse(textX.Text))
                {
                    if (heelt2 < 0)
                    {
                        heelt2 *= -1;
                    }
                    chart1.Series["Series" + (i + 1).ToString()].Points.AddXY(timer, heelt2);
                    timer += 0.05f;
                }
            leave3:
                i = i;
            }


        }
        public void generateTable(String[] desc, String[] baseadr, String[] off1, String[] off2, String[] off3, String[] off4, String[] off5, String[] type, int rowindex)
        {
            textX.Text = ("X Scale of the grid here");
            textY.Text = ("Y Scale of the grid here");
            table = new DataTable();
            table.Columns.Add("Description");
            table.Columns.Add("BaseAdress");
            table.Columns.Add("Offset1");
            table.Columns.Add("Offset2");
            table.Columns.Add("Offset3");
            table.Columns.Add("Offset4");
            table.Columns.Add("Offset5");
            table.Columns.Add("Value");
            table.Columns.Add("Type");
            for (int i = 0; i < rowindex; i++)
            {

                DataRow row = table.NewRow();
                row["Description"] = desc[i];
                row["BaseAdress"] = baseadr[i];

                row["Offset1"] = off1[i];
                row["Offset2"] = off2[i];
                row["Offset3"] = off3[i];
                row["Offset4"] = off4[i];
                row["Offset5"] = off5[i];

                row["Value"] = "";
                row["Type"] = type[i];

                table.Rows.Add(row);
            }
            int num = 0;
            foreach (DataRow drow in table.Rows)
            {


                dataGridView1.Rows.Add();
                dataGridView1.Rows[num].Cells[0].Value = drow["Description"].ToString();
                dataGridView1.Rows[num].Cells[1].Value = drow["BaseAdress"].ToString();
                dataGridView1.Rows[num].Cells[2].Value = drow["Offset1"].ToString();
                dataGridView1.Rows[num].Cells[3].Value = drow["Offset2"].ToString();
                dataGridView1.Rows[num].Cells[4].Value = drow["Offset3"].ToString();
                dataGridView1.Rows[num].Cells[5].Value = drow["Offset4"].ToString();
                dataGridView1.Rows[num].Cells[6].Value = drow["Offset5"].ToString();
                dataGridView1.Rows[num].Cells[7].Value = drow["Value"].ToString();
                dataGridView1.Rows[num].Cells[8].Value = drow["Type"].ToString();
                num++;
            }

        }
        public void loadCTFile(String[] lines)
        {

            String[] desc = new String[99];
            String[] baseadr = new String[99];
            String[] off1 = new String[99];
            String[] off2 = new String[99];
            String[] off3 = new String[99];
            String[] off4 = new String[99];
            String[] off5 = new String[99];
            String[] type = new String[99];
            int offsetnumber = 0;
            int rowindex = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                String _line = lines[i];

                if (_line.Contains("Description"))
                {
                    _line = _line.Replace(" ", string.Empty);
                    _line = _line.Replace("<Description>", string.Empty);
                    _line = _line.Replace("</Description>", string.Empty);
                    _line = new string((from c in _line
                                        where char.IsLetterOrDigit(c)
                                        select c).ToArray());
                    desc[rowindex] = _line;

                }
                if (_line.Contains("VariableType"))
                {
                    _line = _line.Replace(" ", string.Empty);
                    _line = _line.Replace("<VariableType>", string.Empty);
                    _line = _line.Replace("</VariableType>", string.Empty);
                    _line = new string((from c in _line
                                        where char.IsLetterOrDigit(c)
                                        select c).ToArray());
                    type[rowindex] = _line;

                }
                if (_line.Contains("Address") && !_line.Contains("RealAddress"))
                {


                    _line = _line.Replace(" ", string.Empty);
                    _line = _line.Replace("<Address>", string.Empty);
                    _line = _line.Replace("</Address>", string.Empty);

                    baseadr[rowindex] = _line;
                    if (lines[i + 1].Contains("/CheatEntry"))
                    {
                        rowindex++;
                    }

                }
                if (_line.Contains("<Offset>"))
                {
                    _line = _line.Replace(" ", string.Empty);
                    _line = _line.Replace("<Offset>", string.Empty);
                    _line = _line.Replace("</Offset>", string.Empty);

                    switch (offsetnumber)
                    {
                        case 0:
                            off1[rowindex] = _line;
                            break;
                        case 1:
                            off2[rowindex] = _line;
                            break;
                        case 2:
                            off3[rowindex] = _line;
                            break;
                        case 3:
                            off4[rowindex] = _line;
                            break;
                        case 4:
                            off5[rowindex] = _line;

                            break;


                    }

                    if (lines[i + 1].Contains("/Offsets"))
                    {

                        rowindex++;
                        offsetnumber = 0;

                    }
                    else
                    {

                        offsetnumber++;
                    }
                }
            }
            //All Values have been read, now we get the Data into the table

            generateTable(desc, baseadr, off1, off2, off3, off4, off5, type, rowindex);
        }




        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            displayValues();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //Reads a Ct File and assigns Values
            selectCT.Title = "Select a Valid CT table";
            if (selectCT.ShowDialog() == DialogResult.OK)
            {

                string[] lines = System.IO.File.ReadAllLines(selectCT.FileName);
                loadCTFile(lines);
                table_isloaded = true;


            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (table_isloaded == true && checkBox1.Checked == true)
            {

                refreshing = true;
                int o;
                if (int.TryParse(textX.Text, out o))
                {
                    chart1.ChartAreas["ChartArea1"].AxisX.Maximum = int.Parse(textX.Text);
                    chart1.ChartAreas["ChartArea1"].AxisY.Maximum = int.Parse(textY.Text);
                }
                else
                {
                    textX.Text = "50";
                }


            }
            else
            {
                refreshing = false;
                chart1.SaveImage(AppDomain.CurrentDomain.BaseDirectory + "\\mychart.png", ImageFormat.Png);
                chart1.Series[0].Points.Clear();
                chart1.Series[1].Points.Clear();
                chart1.Series[2].Points.Clear();
                timer = 0;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (refreshing)
            {
                displayValues();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void textX_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
