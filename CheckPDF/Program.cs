using NSysDB;
using System;

namespace CheckPDF
{
    internal class Program
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        private static void Main(string[] args)
        {
            AllocConsole();
            for (;;)
            {
                using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
                {
                    var queryData1 = query.Kind1SelectTbl2("*", "A0401H", " ExPdfYN <>  'Y' ", "", "");
                    var queryData2 = query.Kind1SelectTbl2("*", "C0401H", " ExPdfYN <>  'Y' ", "", "");
                    var queryData3 = query.Kind1SelectTbl2("*", "B0401H", " ExPdfYN <>  'Y' ", "", "");
                    var queryData4 = query.Kind1SelectTbl2("*", "D0401H", " ExPdfYN <>  'Y' ", "", "");

                    if (queryData1 != null || queryData2 != null || queryData3 != null || queryData4 != null)
                    {
                        var rows1 = queryData1.Table.Rows;
                        var rows2 = queryData2.Table.Rows;
                        var rows3 = queryData3.Table.Rows;
                        var rows4 = queryData4.Table.Rows;
                        if (rows1.Count > 0 || rows2.Count > 0 || rows3.Count > 0 || rows4.Count > 0)
                        {
                            SendMail();
                        }
                    }
                }
                Console.WriteLine("Check Einvoice PDF Has Create,Pls wait 5 seconds......");
                System.Threading.Thread.Sleep(5000);
            }


        }

        private static void SendMail()
        {
            XMLClass oXMLeParamts = new XMLClass();
            string eToWho1 = "juncheng.liu@rinnai.com.tw";
            string eFromWho1 = oXMLeParamts.GetParaXml("eFromWho");
            using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
            {
                query.AutoEMail(eToWho1, "", eFromWho1, "", "PDF未產生，盡速處理");
            }
        }
    }
}