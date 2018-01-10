using System;
using System.Windows.Forms;

namespace APT2
{
    internal static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            //    #region 列印發票

            //    try
            //    {
            //        if (args != null && args.Length != 0)
            //        {
            //            var printData = PublicMethodFramework35.Repositoies.GetPrintEinvoiceNumbersByPrintNo(args[0]);
            //            Application.EnableVisualStyles();
            //            Application.SetCompatibleTextRenderingDefault(false);
            //            Application.Run(new APT2(printData));
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
            string MInvoiceNumberE = "";

            if (args.Length == 1)
            {
                MInvoiceNumberS = args[0].ToString();
                if (MInvoiceNumberS.Length == 10)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new APT2(MInvoiceNumberS, MInvoiceNumberE));
                }
            }
            if (args.Length == 2)
            {
                MInvoiceNumberS = args[0].ToString();
                MInvoiceNumberE = args[1].ToString();
                if (MInvoiceNumberS.Length == 10 && MInvoiceNumberS.Length == 10)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new APT2(MInvoiceNumberS, MInvoiceNumberE));
                }
            }
        }
    }
}