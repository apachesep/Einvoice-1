using System;
using System.Configuration;
using System.Security;
using System.Text;
using System.Web;

public partial class AutoIm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string result = string.Empty;
        try
        {
            var dirID = Request.QueryString["dirId"];
            using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(@"D:\CallAllInLog\erpDirID.txt", true))
            {
                StringBuilder sb = new StringBuilder();
                string logMsg = string.Format(@"{0} => DirID:{1}", DateTime.UtcNow.AddHours(8), dirID);
                sb.AppendLine(logMsg);
                sb.AppendLine("----------------------------------------------------------");
                file.WriteLine(sb.ToString());
            }
            if (dirID == null)
                result = GoAutoIm();
            else
                result = GoAutoIm(dirID.ToString());
            result = result.Replace("\r\n","<br>");
        }
        catch (Exception ex)
        {
            result = "網頁呼叫失敗：" + ex.Message + "<br>";
            return;
        }
        Response.Write(result);
    }

    protected string GoAutoIm(string dirID = null)
    {
        string writeMsg = string.Empty;
        try
        {
            // 要執行的檔案名稱(必要時需要加讓路徑)
            //string fileName = @"D:\eInvoiceSLN\Release\Callim.exe";
            string fileName = @"D:\eInvoiceSLN\Release\Callim.exe";
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
            System.Diagnostics.ProcessStartInfo processInfo = new System.Diagnostics.ProcessStartInfo(fileName);
            if (dirID != null)
                processInfo.Arguments = dirID;

            processInfo.RedirectStandardOutput = true;
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(processInfo);
            string resultMsg = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Dispose();
            processInfo = null;

            if (dirID == null)
                writeMsg = string.Format("Callim.exe 執行結果：{0}", resultMsg);
            else
                writeMsg = string.Format("Callim.exe 執行結果：{0} 資料夾名稱：{1}", resultMsg, dirID);
        }
        catch (Exception ex)
        {
            if (dirID == null)
                writeMsg = string.Format("Callim.exe 執行行失敗。<br>{0}<br>", ex.Message);
            else
                writeMsg = string.Format("Callim.exe 執行行失敗。<br>{0}<br>資料夾名稱：{1}", ex.Message, dirID);
        }
        return writeMsg;
    }
}