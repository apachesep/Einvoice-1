using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MPDFaw
{

    public void Begin(string MAllowanceNumber)
    {

        using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
        {
            //MPDFaw.exe 1706160010  //生成PDF   MAllowanceNumber
            //if (MAllowanceNumber.Length > 1)
            //{
                ////MPDFaw.exe ALL  //全部生成PDF  //ExPdfYN=N
                System.Data.DataView dvResultALLA = query.Kind1SelectTbl2("MAllowanceNumber", "B0401H", "(ExPdfYN IS NULL) OR (ExPdfYN <> 'Y')", "", "");
                //Console.WriteLine(MAllowanceNumber);

                if (dvResultALLA != null)
                {
                    if (dvResultALLA.Count > 0)
                    {
                        for (int i = 0; i < dvResultALLA.Count; i++)
                        {
                            //B2B折讓證明單 B0401
                            query.CallMkPDFaw(Convert.ToString(dvResultALLA.Table.Rows[i][0]));
                        }
                    }
                }

                System.Data.DataView dvResultALLC = query.Kind1SelectTbl2("MAllowanceNumber", "D0401H", "(ExPdfYN IS NULL) OR (ExPdfYN <> 'Y')", "", "");
                if (dvResultALLC != null)
                {
                    if (dvResultALLC.Count > 0)
                    {
                        for (int i = 0; i < dvResultALLC.Count; i++)
                        {
                            //B2C折讓證明單 D0401
                            query.CallMkPDFaw(Convert.ToString(dvResultALLC.Table.Rows[i][0]));
                        }
                    }
                }
            //}
        }
    }

}
