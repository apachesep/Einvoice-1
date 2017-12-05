using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Variant
{
    public partial class Variant : Form
    {
        public Variant()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DoingOK();
        }

        private void butSearch_Click(object sender, EventArgs e)
        {
            string Condition = "";
            if (radRegion.Checked)
            {

                if (comboBox3.SelectedValue.ToString() == "2")
                { Condition = "(ProgramTime BETWEEN '" + Convert.ToDateTime(dateTimePicker1.Value).ToString("yyyy/MM/dd") + "' AND '" + Convert.ToDateTime(dateTimePicker2.Value).AddDays(1).ToString("yyyy/MM/dd") + "')"; }
                else
                { Condition = "(ProgramTime BETWEEN '" + Convert.ToDateTime(dateTimePicker1.Value).ToString("yyyy/MM/dd") + "' AND '" + Convert.ToDateTime(dateTimePicker2.Value).AddDays(1).ToString("yyyy/MM/dd") + "') And (ProgramState='" + comboBox3.SelectedValue.ToString() + "') "; }

            }
            else
            {
                if (texDrawDay.Text.ToString() == "")
                { texDrawDay.Text = "1"; }

                if (comboBox3.SelectedValue.ToString() == "2")
                { Condition = "(ProgramTime >= DATEADD(day, -" + Convert.ToInt32(texDrawDay.Text) + ", GETDATE()))"; }
                else
                { Condition = "(ProgramTime >= DATEADD(day, -" + Convert.ToInt32(texDrawDay.Text) + ", GETDATE())) And (ProgramState='" + comboBox3.SelectedValue.ToString() + "') "; }
            }

            INdataGridView1(Condition);
        }


        private void INdataGridView1(string Condition)
        {

            //dataGridView1.SelectAll();
            //dataGridView1.ClearSelection();
            //dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.AllowUserToAddRows = false;

            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.DataSource = GetLogsAllData(Condition);
            dataGridView1.AutoResizeColumns();
        }

        private DataTable GetLogsAllData(string Condition)
        {
            DataTable GetLogsAllDataGo = null;
            DataView dvResult = null;

            using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
            {
                dvResult = query.Kind1SelectTbl2("ProgramKey,ProgramState,ProgramTime,ProgramMotion,ProgramName", "LogsAll", Condition, "ProgramTime", "Desc");
            }

            if (dvResult != null)
            {
                GetLogsAllDataGo = dvResult.Table;
                labCount.Text = dvResult.Count.ToString() + "筆";
            }
            else
            {
                labCount.Text = "";
            }

            return GetLogsAllDataGo;
        }

        private void DoingOK()
        {
            dateTimePicker1.Value = DateTime.Today.AddDays(-3);
            ArrayList data = new ArrayList();
            data.Add(new DictionaryEntry("不拘..", "2"));
            data.Add(new DictionaryEntry("正常(0)", "0"));
            data.Add(new DictionaryEntry("錯誤(1)", "1"));
            comboBox3.DisplayMember = "Key";
            comboBox3.ValueMember = "Value";
            comboBox3.DataSource = data;
        }

        private void texDrawDay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar < 48 | (int)e.KeyChar > 57) & (int)e.KeyChar != 8)
            { e.Handled = true; }
        }

        private void texDrawDay_Click(object sender, EventArgs e)
        {
            radDraw.Checked = true;
        }

        private void dateTimePicker1_DropDown(object sender, EventArgs e)
        {
            radRegion.Checked = true;
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            radRegion.Checked = true;
        }

        private void dateTimePicker1_MouseDown(object sender, MouseEventArgs e)
        {
            radRegion.Checked = true;
        }









    }
}
