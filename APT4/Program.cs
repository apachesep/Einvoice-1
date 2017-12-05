using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace APT4
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string MInvoiceNumberS = "";
            string MInvoiceNumberE = "";

            if (args.Length == 1)
            {
                MInvoiceNumberS = args[0].ToString();
                if (MInvoiceNumberS.Length == 10)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new APT4(MInvoiceNumberS, MInvoiceNumberE));
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
                    Application.Run(new APT4(MInvoiceNumberS, MInvoiceNumberE));
                }
            }
        }
    }
}
