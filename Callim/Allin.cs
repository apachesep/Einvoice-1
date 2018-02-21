using EinvoiceUnity.Models;
using NSysDB;
using NSysDB.NTSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;

public class Allin
{
    public Queue<Process> m_processQueue = new Queue<Process>();
    public Queue<Process> ProcessQueue { get { return m_processQueue; } set { m_processQueue = value; } }

    /// <summary>
    /// 排隊入口
    /// </summary>
    /// <param name="workers"></param>
    /// <returns></returns>
    private bool QueueEntrance(List<Process> workers, Process currentProcess)
    {
        string msg = "目前使用者的處理程序為：" + currentProcess.Id + "，\n目前Queue有" + workers.Count + "筆在等待\n";
        foreach (var item in ProcessQueue)
        {
            int index = workers.ToList().IndexOf(item) + 1;
            msg += string.Format("第{0}筆，處理程序為：{1}，name：{2}\n", index, item.Id, item.ProcessName);
        }
        Console.WriteLine(msg);
        bool result = true;
        try
        {
            //將自己排進去
            if (ProcessQueue.Count == 0)
            {
                workers.All(a => { ProcessQueue.Enqueue(a); return true; });
                ProcessQueue.Enqueue(currentProcess);
            }
            else
                ProcessQueue.Enqueue(currentProcess);
            //消化排隊
            while (ProcessQueue.Count != 0)
            {
                var process = ProcessQueue.Dequeue();
                var processInso = GetProcessInfo(process);
                var id = process.Id;
                Console.WriteLine("目前Queue數量：" + ProcessQueue.Count + "\n");
                var chkHasInTask = (Process.GetProcesses().Where(o => o.Id == id).ToList().Count > 0);
                if (!chkHasInTask)
                    continue;
                Console.WriteLine(string.Format(
                      "等待執行緒ID：{0}\n name：{1}\n startTime：{2}\n user：{3}\n",
                       id, process.ProcessName, process.StartTime, string.Join(",", processInso)));
                //排到自己就跳出迴圈執行
                if (id == currentProcess.Id)
                    break;

                process.WaitForExit();
            }
        }
        catch (Exception ex)
        {
            result = false;
        }
        return result;
    }

    public string[] GetProcessInfo(Process singlePro)
    {
        ObjectQuery objQuery = new ObjectQuery("Select * From Win32_Process where ProcessId='" + singlePro.Id + "'");

        ManagementObjectSearcher mos = new ManagementObjectSearcher(objQuery);

        string processOwner = "";
        string[] s = new string[] { };
        foreach (ManagementObject mo in mos.Get())
        {
            s = new string[2];

            mo.InvokeMethod("GetOwner", (object[])s);

            processOwner = s[0].ToString();

            break;
        }
        return s;
    }

    /// <summary>
    /// 判斷是否可直接執行處理程序
    /// </summary>
    /// <returns></returns>
    private bool ReadyForAdmission()
    {
        Process currentProcess = Process.GetCurrentProcess();
        var currentProcessID = currentProcess.Id;
        List<Process> workers = Process.GetProcesses()
            .Where(o => o.Id != currentProcessID && o.ProcessName == "Callim")
            .OrderBy(o => o.StartTime)
            .ToList();
        if (workers.Count() == 0)
            return true;
        //有一個正在執行就排隊
        return QueueEntrance(workers, currentProcess);
    }

    /// <summary>
    /// 處理錯誤通知mail
    /// </summary>
    /// <param name="errorInfo"></param>
    private void SendErrorEmail(ErrorInfoModel errorInfo)
    {
        SQL1 sqlAdaper = new SQL1();
        StringBuilder htmlContent = new StringBuilder();

        string propertiesHtml = string.Empty;
        var groupData = errorInfo.ErrorBuffer.OrderBy(o => o.Key)
            .GroupBy(o => o.Key, o => o.Value).ToList();

        //分出A0401H，C0401H
        foreach (var group in groupData)
        {
            var dataTotalCount = group.Sum(s => s.Details.Count);
            string einvoiceType = group.Key;
            htmlContent.AppendLine(@"<div style=""border:solid black 1px;padding:7px;"">");
            htmlContent.AppendLine(@"<p style=""font-size:23px;"">");
            htmlContent.AppendLine("發票處理程序：" + einvoiceType + " 資料筆數：" + dataTotalCount);
            htmlContent.AppendLine("<p>");

            var errorsGroup = group.First().Details.GroupBy(g => g.ErrorGroupKey).Select(s => s);
            //分出錯誤群組
            foreach (var error in errorsGroup)
            {
                var erKey = error.Key;
                htmlContent.AppendLine("<p>");
                htmlContent.AppendLine("錯誤分類：" + erKey);
                htmlContent.AppendLine("<p>");
                int dataIndex = 1;
                foreach (var er in error)
                {
                    htmlContent.AppendLine(@"<div style=""border:solid #ccc 1px;padding:3px;"">");
                    htmlContent.AppendLine("<p>");
                    htmlContent.AppendLine("項目：" + dataIndex);
                    htmlContent.AppendLine("<p>");

                    htmlContent.AppendLine("<p>");
                    htmlContent.AppendLine("錯誤發票：" + er.EinvoiceNumber);
                    htmlContent.AppendLine("<p>");

                    htmlContent.AppendLine("<p>");
                    htmlContent.AppendLine("錯誤訊息：" + er.ErrorMessage);
                    htmlContent.AppendLine("<p>");

                    htmlContent.AppendLine("<p>");
                    htmlContent.AppendLine("其他訊息：" + er.OtherMessage);
                    htmlContent.AppendLine("<p>");

                    htmlContent.AppendLine("<p>");
                    htmlContent.AppendLine("處理程序：" + er.ProcessName);
                    htmlContent.AppendLine("<p>");
                    htmlContent.AppendLine("</div>");

                    dataIndex++;
                }
            }
            htmlContent.AppendLine("</div>");
        }
        var viewHtml = htmlContent.ToString();
        XMLClass oXMLeParamts = new XMLClass();
        string eToWho1 = oXMLeParamts.GetParaXml("eToWho");
        string eFromWho1 = oXMLeParamts.GetParaXml("eFromWho");
        sqlAdaper.AutoEMail(eToWho1, "", eFromWho1, "", htmlContent.ToString());
        sqlAdaper.Dispose();
    }

    private ErrorInfoModel m_errorInfo = new ErrorInfoModel();
    public ErrorInfoModel ErrorInfo { get { return m_errorInfo; } set { m_errorInfo = value; } }

    //public void Begin(bool isAllIn = false)
    //{
    //    MoveFiles GoMoveFiles = new MoveFiles();
    //    string identityKey = GoMoveFiles.GetIdentityKey();
    //    string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

    //    //程序一: 移動檔案 D:\ImInputERP To D:\eInvoiceFile\ImInputTXT
    //    GoMoveFiles.Begin("0", @"D:\ImInputERP\", @"D:\eInvoiceFile\ImInputTXT\", true);

    //    //程序二: //存證B2B匯入
    //    ImA0401H a0401h = new ImA0401H();
    //    a0401h.Begin("a0401h", identityKey);
    //    ImA0401D a0401d = new ImA0401D();
    //    a0401d.Begin("a0401d");

    //    ImA0501 a0501 = new ImA0501();
    //    a0501.Begin("a0501");

    //    ImA0601 a0601 = new ImA0601();
    //    a0601.Begin("a0601");

    //    ImB0401H b0401h = new ImB0401H();
    //    b0401h.Begin("b0401h", identityKey);
    //    ImB0401D b0401d = new ImB0401D();
    //    b0401d.Begin("b0401d");

    //    ImB0501 b0501 = new ImB0501();
    //    b0501.Begin("b0501");

    //    //程序三 : //存證B2C匯入
    //    ImC0401H c0401h = new ImC0401H();
    //    c0401h.Begin("c0401h", identityKey);
    //    ImC0401D c0401d = new ImC0401D();
    //    c0401d.Begin("c0401d");

    //    ImC0501 c0501 = new ImC0501();
    //    c0501.Begin("c0501");

    //    ImC0701 c0701 = new ImC0701();
    //    c0701.Begin("c0701");

    //    ImD0401H d0401h = new ImD0401H();
    //    d0401h.Begin("d0401h", identityKey);
    //    ImD0401D d0401d = new ImD0401D();
    //    d0401d.Begin("d0401d");

    //    ImD0501 d0501 = new ImD0501();
    //    d0501.Begin("d0501");

    //    //程序3-5
    //    try
    //    {
    //        using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
    //        {
    //            DataView dvResult1 = query.Kind1SelectTbl2("STempKind,STempNo", "STemp", "", "", "STemp");
    //            if (dvResult1 != null)
    //            {
    //                for (int i = 0; i < dvResult1.Count; i++)
    //                {
    //                    query.DeleteData(Convert.ToString(dvResult1.Table.Rows[i][0]), Convert.ToString(dvResult1.Table.Rows[i][1]));
    //                }
    //                query.DeleteData("STemp", "");
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
    //        {
    //            query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, "[程序3-5]被中斷", ex.ToString(), "", 61);
    //        }
    //    }

    //    //程序四 : //產生發票PDF
    //    try
    //    {
    //        MPDF nMPDF = new MPDF();
    //        if (isAllIn)
    //            nMPDF.Begin("ALL");
    //        else
    //            nMPDF.BeginByNumberList(identityKey);
    //    }
    //    catch (Exception ex)
    //    {
    //        using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
    //        {
    //            query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, "[程序四]被中斷", ex.ToString(), "", 61);
    //        }
    //    }

    //    //程序五 : //產生折讓單PDF
    //    try
    //    {
    //        MPDFaw nMPDFaw = new MPDFaw();
    //        if (isAllIn)
    //            nMPDFaw.Begin("ALL");
    //        else
    //            nMPDFaw.BeginByNumberList(identityKey);
    //    }
    //    catch (Exception ex)
    //    {
    //        using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
    //        {
    //            query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, "[程序五]被中斷", ex.ToString(), "", 61);
    //        }
    //    }


    //}

    public void Begin(string parameter)
    {
        //var isContinue = ReadyForAdmission();
        //if (!isContinue)
        //    return;
        Dictionary<string, List<string>> pdfEinvoiceNumList = new Dictionary<string, List<string>>();
        string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        MoveFiles GoMoveFiles = new MoveFiles();
        //1-取得識別碼
        string identityKey = GoMoveFiles.GetIdentityKey();

        try
        {

            //2-設定要處理的發票類別
            List<string> getEinvoiceTypes = new List<string>() { "A0401", "A0501", "A0601", "B0401", "B0501", "C0401", "D0501", "D0401", "C0501", "C0701" };

            //3-將檔案移動至暫存資料夾，並取得暫存資料夾完整路徑
            string virtualTempDir = GoMoveFiles.MoveToTempDatabase(getEinvoiceTypes, parameter, true);
            //if (string.IsNullOrEmpty(virtualTempDir))
            //{
            //    return;
            //}

            //4-寫入暫存資料庫
            GoMoveFiles.WriteDataHandler(virtualTempDir, "A0401H", identityKey);
            GoMoveFiles.WriteDataHandler(virtualTempDir, "A0401D", identityKey);
            GoMoveFiles.WriteDataHandler(virtualTempDir, "B0401H", identityKey);
            GoMoveFiles.WriteDataHandler(virtualTempDir, "B0401D", identityKey);

            GoMoveFiles.WriteDataHandler(virtualTempDir, "C0401H", identityKey);
            GoMoveFiles.WriteDataHandler(virtualTempDir, "C0401D", identityKey);
            GoMoveFiles.WriteDataHandler(virtualTempDir, "D0401H", identityKey);
            GoMoveFiles.WriteDataHandler(virtualTempDir, "D0401D", identityKey);

            GoMoveFiles.WriteDataHandler(virtualTempDir, "A0501", identityKey);
            GoMoveFiles.WriteDataHandler(virtualTempDir, "A0601", identityKey);
            GoMoveFiles.WriteDataHandler(virtualTempDir, "B0501", identityKey);
            GoMoveFiles.WriteDataHandler(virtualTempDir, "C0501", identityKey);
            GoMoveFiles.WriteDataHandler(virtualTempDir, "C0701", identityKey);
            GoMoveFiles.WriteDataHandler(virtualTempDir, "D0501", identityKey);

            //5-從暫存寫入正式資料庫

            ImA0401H a0401h = new ImA0401H();
            a0401h.Begin2(virtualTempDir, "A0401H", ErrorInfo, identityKey);

            ImA0401D a0401d = new ImA0401D();
            a0401d.Begin2(virtualTempDir, "A0401D", ErrorInfo, identityKey);

            ImB0401H b0401h = new ImB0401H();
            b0401h.Begin2(virtualTempDir, "B0401H", ErrorInfo, identityKey);

            ImB0401D b0401d = new ImB0401D();
            b0401d.Begin2(virtualTempDir, "B0401D", ErrorInfo, identityKey);

            ImC0401H c0401h = new ImC0401H();
            c0401h.Begin2(virtualTempDir, "C0401H", ErrorInfo, identityKey);

            ImC0401D c0401d = new ImC0401D();
            c0401d.Begin2(virtualTempDir, "C0401D", ErrorInfo, identityKey);

            ImD0401H d0401h = new ImD0401H();
            d0401h.Begin2(virtualTempDir, "D0401H", ErrorInfo, identityKey);

            ImD0401D d0401d = new ImD0401D();
            d0401d.Begin2(virtualTempDir, "D0401D", ErrorInfo, identityKey);

            ImA0501 a0501 = new ImA0501();
            a0501.Begin2(virtualTempDir, "A0501", ErrorInfo, identityKey);

            ImA0601 a0601 = new ImA0601();
            a0601.Begin2(virtualTempDir, "A0601", ErrorInfo, identityKey);

            ImB0501 b0501 = new ImB0501();
            b0501.Begin2(virtualTempDir, "B0501", ErrorInfo, identityKey);

            ImC0501 c0501 = new ImC0501();
            c0501.Begin2(virtualTempDir, "C0501", ErrorInfo, identityKey);

            ImC0701 c0701 = new ImC0701();
            c0701.Begin2(virtualTempDir, "C0701", ErrorInfo, identityKey);

            ImD0501 d0501 = new ImD0501();
            d0501.Begin2(virtualTempDir, "D0501", ErrorInfo, identityKey);

            //6-錯誤發送通知
            if (ErrorInfo.ErrorBuffer.Count > 0)
                SendErrorEmail(ErrorInfo);

            //7-沒有給目錄就跑原來的Allin
            //if (string.IsNullOrEmpty(dirID))
            //    GoMoveFiles.Begin("0", @"D:\ImInputERP\", @"D:\eInvoiceFile\ImInputTXT\");
            //else
            //    GoMoveFiles.Begin("0", @"D:\ImInputERP\" + dirID, @"D:\eInvoiceFile\ImInputTXT\");
        }
        catch (Exception ex)
        {
            using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
            {
                query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, "[程序 Allin]被中斷", ex.ToString(), "", 61);
            }
        }

        //程序3-5
        try
        {
            using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
            {
                DataView dvResult1 = query.Kind1SelectTbl2("STempKind,STempNo", "STemp", "", "", "STemp");
                if (dvResult1 != null)
                {
                    for (int i = 0; i < dvResult1.Count; i++)
                    {
                        query.DeleteData(Convert.ToString(dvResult1.Table.Rows[i][0]), Convert.ToString(dvResult1.Table.Rows[i][1]));
                    }
                    query.DeleteData("STemp", "");
                }
            }
        }
        catch (Exception ex)
        {
            using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
            {
                query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, "[程序3-5]被中斷", ex.ToString(), "", 61);
            }
        }

        //程序四 : //產生發票PDF
        try
        {
            MPDF nMPDF = new MPDF();
            if (parameter.Equals("ALL"))
                nMPDF.Begin();
            else
                nMPDF.BeginByNumberList(identityKey);
        }
        catch (Exception ex)
        {
            using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
            {
                query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, "[程序四]被中斷", ex.ToString(), "", 61);
            }
        }

        //程序五 : //產生折讓單PDF
        try
        {
            MPDFaw nMPDFaw = new MPDFaw();
            if (parameter.Equals("ALL"))
                nMPDFaw.Begin();
            else
                nMPDFaw.BeginByNumberList(identityKey);
        }
        catch (Exception ex)
        {
            using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
            {
                query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, "[程序五]被中斷", ex.ToString(), "", 61);
            }
            Console.WriteLine("發生未知錯誤：" + ex.Message);

        }
    }
}