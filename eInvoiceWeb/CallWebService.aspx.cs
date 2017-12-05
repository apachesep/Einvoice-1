using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CallWebService : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Call1_Click(object sender, EventArgs e)
    {
        Rinnai.AutoIm SendWS = new Rinnai.AutoIm();
        Label1.Text = SendWS.HelloWorld("123").ToString();
    }

    protected void Call2_Click(object sender, EventArgs e)
    {
        string fXml = @"<?xml version='1.0'?>
        <DOCUMENTDATA>
        <DOCUMENTINFO>
            <InvoNo>Danny Here !!22</InvoNo>
        </DOCUMENTINFO>
        </DOCUMENTDATA>
        ";

        Rinnai.AutoIm SendWS = new Rinnai.AutoIm();
        Label2.Text = SendWS.HelloWorld(fXml).ToString();
    }

    protected void Call3_Click(object sender, EventArgs e)
    {
        //Rinnai.AutoIm SendWS = new Rinnai.AutoIm();
        //Label1.Text = SendWS.HelloWorld("123").ToString();

        //try
        //{

        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        proc.StartInfo.FileName = "C:\\mybatch.bat";
        proc.StartInfo.UseShellExecute = true;
        proc.Start();
        Label3.Text = "5";

        //}
        //catch ()
        //{
        //    //return e.ToString();
        //}
    }
    protected void Call4_Click(object sender, EventArgs e)
    {
        System.Diagnostics.ProcessStartInfo processInfo = new System.Diagnostics.ProcessStartInfo(@"C:\mybatch.bat");
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = false;
        System.Diagnostics.Process process = System.Diagnostics.Process.Start(processInfo);
        process.WaitForExit();
        if (process.ExitCode == 0)
        { // success}
            Label4.Text = "OKOK";
        }
        else
        {
            Label4.Text = process.ExitCode.ToString();
        }
    }
}