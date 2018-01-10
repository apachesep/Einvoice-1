using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace APT3
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //    #region 列印發票
            //    try
            //    {
            //        if (args != null && args.Length != 0)
            //        {
            //            var printData = PublicMethodFramework35.Repositoies.GetPrintEinvoiceNumbersByPrintNo(args[0]);
            //            Application.EnableVisualStyles();
            //            Application.SetCompatibleTextRenderingDefault(false);
            //            Application.Run(new APT3(printData));
            //            PublicMethodFramework35.Repositoies.ClearPrintEinvocieDataByPrintNo(args[0]);
            //        }
            //        else if (args.Length > 1)
            //        {
            //            string mailBody = string.Format("[電子發票] <br> 錯誤訊息：{0}", "列印傳入資訊錯誤ID大於1筆");
            //            string eToWho1 = PublicMethodFramework35.Repositoies.GetParaXml("eToWhoRinnai");
            //            string eFromWho1 = PublicMethodFramework35.Repositoies.GetParaXml("eFromWho");
            //            PublicMethodFramework35.Repositoies.AutoEMail(eToWho1, "", eFromWho1, "", mailBody);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        string mailBody = string.Format("[電子發票] <br> 錯誤訊息：{0}", "列印發票出現未知錯誤");
            //        string eToWho1 = PublicMethodFramework35.Repositoies.GetParaXml("eToWhoRinnai");
            //        string eFromWho1 = PublicMethodFramework35.Repositoies.GetParaXml("eFromWho");
            //        PublicMethodFramework35.Repositoies.AutoEMail(eToWho1, "", eFromWho1, "", mailBody);
            //        throw ex;
            //    }
            //}

            //#endregion 列印發票

            string MInvoiceNumberS = "";
            if (args.Length == 1)
            {
                MInvoiceNumberS = args[0].ToString();
                if (MInvoiceNumberS.Length == 10)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new APT3(MInvoiceNumberS));
                }
            }
        }
    }
}


