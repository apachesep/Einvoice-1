using System;
using System.Windows.Forms;

namespace APT
{
    internal static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            //args = new string[] { "BW12213612" };

            #region 列印發票
            try
            {
                if (args != null && args.Length != 0)
                {
                    PublicMethodFramework35.Repositoies.SaveMesagesToTextFile("程式進入：" + args[0]);
                    var printData = PublicMethodFramework35.Repositoies.GetPrintEinvoiceNumbersByPrintNo(args[0]);
                    PublicMethodFramework35.Repositoies.SaveMesagesToTextFile("取得發票：" + string.Join(",", printData.ToArray()));
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    PublicMethodFramework35.Repositoies.SaveMesagesToTextFile("列印開始：" + args[0]);
                    Application.Run(new APT(printData));
                    PublicMethodFramework35.Repositoies.SaveMesagesToTextFile("列印完畢：" + args[0]);
                    PublicMethodFramework35.Repositoies.SaveMesagesToTextFile("清除資料開始：" + args[0]);
                    PublicMethodFramework35.Repositoies.ClearPrintEinvocieDataByPrintNo(args[0]);
                    PublicMethodFramework35.Repositoies.SaveMesagesToTextFile("清除資料完畢：" + args[0]);
                }
                else if (args.Length > 1)
                {
                    string mailBody = string.Format("[電子發票] <br> 錯誤訊息：{0}", "列印傳入資訊錯誤ID大於1筆");
                    string eToWho1 = PublicMethodFramework35.Repositoies.GetParaXml("eToWhoRinnai");
                    string eFromWho1 = PublicMethodFramework35.Repositoies.GetParaXml("eFromWho");
                    PublicMethodFramework35.Repositoies.AutoEMail(eToWho1, "", eFromWho1, "", mailBody);
                    PublicMethodFramework35.Repositoies.SaveMesagesToTextFile("錯誤訊息：" + mailBody);

                }
            }
            catch (Exception ex)
            {
                string mailBody = string.Format("[電子發票] <br> 錯誤訊息：{0} 列印發票出現未知錯誤：{1} ", args[0], ex.Message);
                string eToWho1 = PublicMethodFramework35.Repositoies.GetParaXml("eToWhoRinnai");
                string eFromWho1 = PublicMethodFramework35.Repositoies.GetParaXml("eFromWho");
                PublicMethodFramework35.Repositoies.AutoEMail(eToWho1, "", eFromWho1, "", mailBody);
                PublicMethodFramework35.Repositoies.SaveMesagesToTextFile("錯誤訊息：" + mailBody);
                throw ex;
            }
        }

        #endregion 列印發票

        //string MInvoiceNumberS = "";
        //string MInvoiceNumberE = "";

        //if (args.Length == 1)
        //{
        //    MInvoiceNumberS = args[0].ToString();
        //    if (MInvoiceNumberS.Length == 10)
        //    {
        //        Application.EnableVisualStyles();
        //        Application.SetCompatibleTextRenderingDefault(false);
        //        Application.Run(new APT(MInvoiceNumberS, MInvoiceNumberE));
        //    }
        //}
        //if (args.Length == 2)
        //{
        //    MInvoiceNumberS = args[0].ToString();
        //    MInvoiceNumberE = args[1].ToString();
        //    if (MInvoiceNumberS.Length == 10 && MInvoiceNumberS.Length == 10)
        //    {
        //        Application.EnableVisualStyles();
        //        Application.SetCompatibleTextRenderingDefault(false);
        //        Application.Run(new APT(MInvoiceNumberS, MInvoiceNumberE));
        //    }
        //}
    }
}