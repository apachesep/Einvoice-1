using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Base64S
{
    public partial class Base64S : Form
    {
        public Base64S()
        {
            InitializeComponent();
        }

        private void but_S_to_64_Click(object sender, EventArgs e)
        {
            byte[] byt = System.Text.Encoding.UTF8.GetBytes(textBox1.Text);
            textBox2.Text = Convert.ToBase64String(byt);
            textBox3.Text = "";
        }

        private void but_64_to_S_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] b = Convert.FromBase64String(textBox2.Text);
                textBox3.Text = System.Text.Encoding.UTF8.GetString(b);
            }
            catch (Exception ex)
            {
                textBox3.Text = ex.Message;
            }
        }

        private void but_Close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Base64S_Load(object sender, EventArgs e)
        {

        }

        private string ToL0(string sOldStrring, int AllLength)
        {



            //if (sOldStrring.Length < AllLength)
            //{
            //    int k = AllLength - sOldStrring.Length;
            //    //for (int i = 1; i <= AllLength - sOldStrring.Length; i++)
            //     for (int i = 1; i <= k; i++)
            //    {
            //        sOldStrring =  sOldStrring+"0";
            //        //tex16.Text += sOldStrring;
            //    }
            //}

            while (sOldStrring.Length < AllLength)
            {
                sOldStrring = "0" + sOldStrring;
            }

            return sOldStrring;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            string s1 = "0123456789012345678901234567890123456789";
            Int16 j1 = 15;
            Int16 j2 = 6;
            if (s1.Length > j2)
            {
                string s3 = s1.Substring(0, j2);
                string s4 = s3;
                while (s3.Length < (s4.Length + (j1 - s4.Length % j1)))
                { s3 = s3 + " "; }
                s1 = s3 + s1.Substring(j2, s1.Length - j2);
            }
            string s2 = s1;
            while (s1.Length < (s2.Length + (j1 - s2.Length % j1)))
            { s1 = s1 + " "; }

            int i = 0;
            while (i < s1.Length / j1)
            {
                label1.Text += s1.Substring(i * j1, j1 - s1.Length % j1).Trim() + "\r\n";
                i += 1;
            }

        }



    }
}
