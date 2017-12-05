using System;
using System.Configuration;
using System.Security;

public partial class AutoIm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            GoAutoIm();
        }
        catch (Exception ex)
        {
            Response.Write("2，網頁呼叫失敗：" + ex.Message + "<br>");
            return;
        }
        Response.Write("2，網頁呼叫已完成。");
        return;
    }

    protected void GoAutoIm()
    {
        try
        {

            // 要執行的檔案名稱(必要時需要加讓路徑)
            string fileName = @"D:\eInvoiceSLN\Callim\bin\Debug\Callim.exe";
            //string fileName = @"D:\copyTemp\Callim\bin\Debug\Callim.exe";
            // 指定要執行程式的使用者名稱
            string userName = ConfigurationManager.AppSettings["ProcessUserName"];
            string userPwd = ConfigurationManager.AppSettings["ProcessUserPwd"];
            SecureString securePwd = new SecureString();
            foreach (var c in userPwd)
                securePwd.AppendChar(c);
            //System.Diagnostics.ProcessStartInfo processInfo1 = new System.Diagnostics.ProcessStartInfo(@"D:\eInvoiceSLN\BAT\9Callim.bat");
            //System.Diagnostics.Process process1 = System.Diagnostics.Process.Start(processInfo1);
            //System.Diagnostics.ProcessStartInfo processInfo1 = new System.Diagnostics.ProcessStartInfo(@"D:\eInvoiceSLN\Release\Callim.exe");
            System.Diagnostics.ProcessStartInfo processInfo1 = new System.Diagnostics.ProcessStartInfo(fileName);
            //processInfo1.UserName = userName;
            //processInfo1.Password = securePwd;
            //processInfo1.Domain = "rinnai";
            processInfo1.CreateNoWindow = true;
            processInfo1.UseShellExecute = false;
            System.Diagnostics.Process process1 = System.Diagnostics.Process.Start(processInfo1);
            process1.WaitForExit();
            process1.Dispose();
            processInfo1 = null;
        }
        catch (Exception ex)
        {
            Response.Write("1，Callim.exe 執行行失敗。<br>" + ex.Message + "<br>");
            return;
        }
        Response.Write("1，Callim.exe 成功執行。<br>");
    }
}