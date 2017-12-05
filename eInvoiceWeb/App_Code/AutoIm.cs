using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Threading;

/// <summary>
/// AutoIm 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
// [System.Web.Script.Services.ScriptService]
public class AutoIm : System.Web.Services.WebService
{

    public AutoIm()
    {

        //如果使用設計的元件，請取消註解下列一行
        //InitializeComponent(); 

    }

    [WebMethod]

    public string HelloWorld()
    {
        GoHelloWorld();
        return "收到了!!  Danny!!";
    }

    protected void GoHelloWorld()
    {
        System.Diagnostics.ProcessStartInfo processInfo1 = new System.Diagnostics.ProcessStartInfo(@"D:\eInvoiceSLN\BAT\9Callim.bat");
        processInfo1.CreateNoWindow = true;
        processInfo1.UseShellExecute = false;
        System.Diagnostics.Process process1 = System.Diagnostics.Process.Start(processInfo1);
        process1.WaitForExit();
    }


//    public string HelloWorld(string  InvoNoXML)
//    {
//        string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");




//        using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
//        {
//            //  query.GoLogsAll(sPgSN, "111111WebService_AutoIm已被觸發了喔!!", Convert.ToString( InvoNoXML), "", "", 1);
//            //query.GoLogsAll(sPgSN, "333WebService_AutoIm已被觸發了喔!!", InvoNoXML.GetType().ToString(), "", "", 1);
//            query.GoLogsAll(sPgSN, "333WebService_AutoIm已被觸發了喔!!", "aa", "", "", 1);
//        }





//        return InvoNoXML;


//        //string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");


//        sPgSN = "AAA" + InvoNoXML + "BBB" + sPgSN;
//        //using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
//        //{
//        //    query.GoLogsAll(sPgSN, "WebService_AutoIm已被觸發了喔!!", "test", "", "", 1);
//        //}
//        //return sPgSN;

//        string fXml = @"<?xml version='1.0'?>
//        <DOCUMENTDATA>
//        <DOCUMENTINFO>
//            <InvoNo>Danny Here</InvoNo>
//        </DOCUMENTINFO>
//        </DOCUMENTDATA>
//        ";

//        //fXml = fXml; //假字串
//        fXml = InvoNoXML; //真字串

//        string sInvoNo = "0";
//        try
//        {
//            XmlDocument xmlDoc = new XmlDocument();
//            xmlDoc.LoadXml(fXml);
//            XmlNode xmlNode = xmlDoc.SelectSingleNode("DOCUMENTDATA/DOCUMENTINFO");
//            sInvoNo = xmlNode.SelectSingleNode("InvoNo").InnerText.ToString();
//        }
//        catch
//        { sInvoNo = "1"; }


//        //Rinnai.AutoIm SendWS = new Rinnai.AutoIm();
//        //Label1.Text = SendWS.HelloWorld("123").ToString();


//        System.Diagnostics.ProcessStartInfo processInfo1 = new System.Diagnostics.ProcessStartInfo(@"D:\eInvoiceSLN\BAT\1MoveTXTIn.bat");
//        processInfo1.CreateNoWindow = true;
//        processInfo1.UseShellExecute = false;
//        System.Diagnostics.Process process1 = System.Diagnostics.Process.Start(processInfo1);
//        process1.WaitForExit();
//        Thread.Sleep(2000);
//        System.Diagnostics.ProcessStartInfo processInfo2 = new System.Diagnostics.ProcessStartInfo(@"D:\eInvoiceSLN\BAT\A2All.bat");
//        processInfo2.CreateNoWindow = true;
//        processInfo2.UseShellExecute = false;
//        System.Diagnostics.Process process2 = System.Diagnostics.Process.Start(processInfo2);
//        process2.WaitForExit();
//        Thread.Sleep(2000);
//        System.Diagnostics.ProcessStartInfo processInfo3 = new System.Diagnostics.ProcessStartInfo(@"D:\eInvoiceSLN\BAT\A3All.bat");
//        processInfo3.CreateNoWindow = true;
//        processInfo3.UseShellExecute = false;
//        System.Diagnostics.Process process3 = System.Diagnostics.Process.Start(processInfo3);
//        process3.WaitForExit();


//        return sInvoNo;
//    }






}
