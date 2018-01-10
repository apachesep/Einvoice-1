using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Management;
using System.Windows.Forms;

namespace APT4
{
    public partial class APT4 : Form
    {
        private string MInvoiceNumberS;
        private string MInvoiceNumberE;
        private List<string> EinvoiceList { get; set; }

        public APT4(string eMInvoiceNumberS, string eMInvoiceNumberE)
        {
            InitializeComponent();
            MInvoiceNumberS = eMInvoiceNumberS;
            MInvoiceNumberE = eMInvoiceNumberE;
        }
        public APT4(List<string> envoiceList)
        {
            InitializeComponent();
            EinvoiceList = envoiceList;
        }
        private void butCheck_Click(object sender, EventArgs e)
        {
            if (comPrinter.SelectedItem.ToString() != "請選擇印表機")
            {

                if (EinvoiceList.Count > 0)
                {
                    foreach (var einvoice in EinvoiceList)
                    {
                        System.Threading.Thread.Sleep(2000);
                        PrintPDF4(einvoice, comPrinter.SelectedItem.ToString());
                    }
                }

                //if (MInvoiceNumberE != "")
                //{
                //    //多筆
                //    for (Int64 i = Convert.ToInt64(MInvoiceNumberS.Substring(6, 4)); i <= Convert.ToInt64(MInvoiceNumberE.Substring(6, 4)); i++)
                //    {
                //        try
                //        {
                //            PrintPDF4(MInvoiceNumberS.Substring(0, 6) + i.ToString(), comPrinter.SelectedItem.ToString());
                //        }
                //        catch
                //        {
                //        }
                //    }
                //}
                //else
                //{
                //    //單筆
                //    PrintPDF4(MInvoiceNumberS, comPrinter.SelectedItem.ToString());
                //}

                Application.Exit();
            }
            else
            { labPrint.Text = "請選擇印表機!!"; }
        }

        private void PrintPDF4(string File, string sPrint)
        {
            string sPathT1 = @"\\einvoice\eInvoiceFile\eInvoiceF\4\";
            string sPfile = File + ".pdf";
            string sPath1 = sPathT1 + File + ".pdf";

            try
            {
                if (System.IO.File.Exists(sPath1))
                {
                    PrintPDFGOGO(sPath1, sPrint, sPfile);
                    PrintPDFGOGO(sPath1, sPrint, sPfile);
                }
            }
            catch
            {
            }
        }

        private void PrintPDFGOGO(string File, string sPrint, string sPfile)
        {
            Process p = new Process();
            p.StartInfo.FileName = "AcroRd32.exe";
            p.StartInfo.Arguments = (" /t \"" + (File + "\" \"" + sPrint + "\""));
            p.Start();
            //p.WaitForExit(3000);
            p.WaitForInputIdle();
            var timeOut = DateTime.Now.AddSeconds(30);
            bool printing = false; //是否開始列印

            while (DateTime.Now.CompareTo(timeOut) < 0)
            {
                if (!printing)
                {
                    if (GetPrintJobsCollection(sPfile, sPrint))
                    {
                        printing = true;
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                else
                {
                    if (!GetPrintJobsCollection(sPfile, sPrint))
                    { break; }
                }
            }
            p.Close();
            p.Dispose();

            try
            {
                Process[] workers = Process.GetProcessesByName("AcroRd32");
                KillAcroRd32(workers);
            }
            catch (Exception)
            {
            }
        }

        private void KillAcroRd32(Process[] workers)
        {
            foreach (Process worker in workers)
            {
                if (workers.Count() == 0)
                    return;
                worker.Kill();
                workers = Process.GetProcessesByName("AcroRd32");
                KillAcroRd32(workers);
                worker.WaitForExit();
                worker.Close();
                worker.Dispose();
            }
        }

        public bool GetPrintJobsCollection(string printFile, string printerName)
        {
            bool sturn = false;
            string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            try
            {
                string searchQuery = "SELECT * FROM Win32_PrintJob";
                ManagementObjectSearcher searchPrintJobs = new ManagementObjectSearcher(searchQuery);
                ManagementObjectCollection prntJobCollection = searchPrintJobs.Get();

                foreach (ManagementObject prntJob in prntJobCollection)
                {
                    String jobName = prntJob.Properties["Name"].Value.ToString();
                    char[] splitArr = new char[1];
                    splitArr[0] = Convert.ToChar(",");
                    string prnName = jobName.Split(splitArr)[0];
                    string documentName = prntJob.Properties["Document"].Value.ToString();
                    PublicMethodFramework35.Repositoies.SaveMesagesToTextFile("列印文件名稱：" + prnName + "   -   " + documentName);


                    if (String.Compare(prnName, printerName, true) == 0)
                    {
                        if (printFile == documentName.ToString())
                        {
                            sturn = true;
                        }
                    }
                }
            }
            catch
            {
            }
            return sturn;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            labMInvoiceNumberS.Text = MInvoiceNumberS;
            PrintDocument printDoc = new PrintDocument();
            String sDefaultPrinter = printDoc.PrinterSettings.PrinterName;  // 取得預設的印表機名稱
            comPrinter.Items.Insert(0, "請選擇印表機");
            foreach (String strPrinter in PrinterSettings.InstalledPrinters)
            {
                if (strPrinter.IndexOf("W") >= 0 && strPrinter.IndexOf("P") >= 0 && strPrinter.IndexOf("8") >= 0 && strPrinter.IndexOf("1") >= 0 && strPrinter.IndexOf("0") >= 0)
                {
                    comPrinter.Items.Add(strPrinter);
                }
            }
            comPrinter.SelectedIndex = 0;
        }
    }
}