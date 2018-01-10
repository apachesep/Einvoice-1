using EinvoiceUnity.Models;
using NSysDB.NTSQL;
using System;
using System.Data;
using System.Text;
using System.Linq;
using NSysDB;

public class Allin
{

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
    }
    public void Begin()
    {
        string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        //程序一 : 移動檔案 D:\ImInputERP To D:\eInvoiceFile\ImInputTXT
        MoveFiles GoMoveFiles = new MoveFiles();
        GoMoveFiles.Begin("0", @"D:\ImInputERP\", @"D:\eInvoiceFile\ImInputTXT\");

        //程序二 : //存證B2B匯入
        ImA0401H a0401h = new ImA0401H();
        a0401h.Begin("a0401h");
        ImA0401D a0401d = new ImA0401D();
        a0401d.Begin("a0401d");

        ImA0501 a0501 = new ImA0501();
        a0501.Begin("a0501");

        ImA0601 a0601 = new ImA0601();
        a0601.Begin("a0601");

        ImB0401H b0401h = new ImB0401H();
        b0401h.Begin("b0401h");
        ImB0401D b0401d = new ImB0401D();
        b0401d.Begin("b0401d");

        ImB0501 b0501 = new ImB0501();
        b0501.Begin("b0501");

        //程序三 : //存證B2C匯入
        ImC0401H c0401h = new ImC0401H();
        c0401h.Begin("c0401h");
        ImC0401D c0401d = new ImC0401D();
        c0401d.Begin("c0401d");

        ImC0501 c0501 = new ImC0501();
        c0501.Begin("c0501");

        ImC0701 c0701 = new ImC0701();
        c0701.Begin("c0701");

        ImD0401H d0401h = new ImD0401H();
        d0401h.Begin("d0401h");
        ImD0401D d0401d = new ImD0401D();
        d0401d.Begin("d0401d");

        ImD0501 d0501 = new ImD0501();
        d0501.Begin("d0501");

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
            nMPDF.Begin("all");
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
            nMPDFaw.Begin("all");
        }
        catch (Exception ex)
        {
            using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
            {
                query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, "[程序五]被中斷", ex.ToString(), "", 61);
            }
        }
    }
}