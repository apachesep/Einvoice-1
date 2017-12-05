using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BuDir
{
    public void Begin(string sKind0)
    {
        ////路徑要做變數@"D:\eInvoiceTXT\ImInput"
        ////sFPath = @"D:\eInvoiceTXT\ImInput";
        ////sFName = @"D:\eInvoiceTXT\ImInput\C0401H2017030501.txt";

        //NSysDB.XMLClass oXMLeParamts = new NSysDB.XMLClass();
        //string sFPathN = oXMLeParamts.GetParaXml("ImTxPaN");
        //string sFPathP = oXMLeParamts.GetParaXml("ImTxPaP");
        //string sFPathY = oXMLeParamts.GetParaXml("ImTxPaY");

        //using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
        //{
        //    query.CreateDIR(sFPathN);
        //    query.CreateDIR(sFPathP);
        //    query.CreateDIR(sFPathY);
        //}

        using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
        {
            query.CreateDIRok("");

            //   string[] sArr;
            //   query.ReturnArr(out sArr);
            //   string sFPathN = sArr[0];
            //   string sFPathP = sArr[1];
            //   string sFPathY = sArr[2];
            //string sPaPartition = sArr[3];


            //Console.WriteLine(sFPathN);
            //Console.ReadLine();
        }
    }
}

