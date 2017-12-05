using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {

        if (args.Length == 1)
        {
            string sMInvoiceNumber = args[0].ToString();
            MPDF GOGO = new MPDF();
            GOGO.Begin(sMInvoiceNumber.ToString());

            //MkPDF.exe NV41099654  //單筆生成PDF
            //MkPDF.exe ALL  //全部生成PDF  //ExPdfYN=N
        }
        //MPDF GOGO2 = new MPDF();
        //GOGO2.Begin("all");

    }
}

