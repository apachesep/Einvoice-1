using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

public class MPDFaw
{
    public void Begin(string MAllowanceNumber = null)
    {
        using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
        {
            //MPDFaw.exe 1706160010  //生成PDF   MAllowanceNumber
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

    public void BeginByNumberList(string identityKey)
    {
        string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
        {


            var b0401List = new List<string>();
            var queryData = query.Kind1SelectTbl2("*", "PRINT_TEMP", " PRINT_METHOD='B0401H' and IDEN_KEY = '" + identityKey + "'", "", "");
            if (queryData != null)
            {
                var rows = queryData.Table.Rows;
                for (int i = 0; i < rows.Count; i++)
                    b0401List.Add(rows[i]["PRINT_EINV_NUM"].ToString());
            }
            if(b0401List.Count>0)
            Console.WriteLine("B0401 產生PDF 開始.");
            int bIndex = 0;
            //B2B折讓證明單 B0401
            foreach (var b0401 in b0401List)
            {
                 bIndex = b0401List.IndexOf(b0401)+1;
                Console.WriteLine(string.Format("B0401 產生PDF 第{0}筆 {1} 折讓證明單開始.", bIndex, b0401));
                query.CallMkPDFaw(b0401);
                Hashtable hashTable = new Hashtable();
                hashTable["MAKE_FILE_DATE"] = DateTime.UtcNow;
                query.UpdateData2("PRINT_TEMP", hashTable, "IDEN_KEY = '" + identityKey + "' AND PRINT_EINV_NUM = " + "'" + b0401 + "'");
                Console.WriteLine(string.Format("B0401 產生PDF 第{0}筆 {1} 折讓證明單結束.", bIndex, b0401));
            }
            if (b0401List.Count > 0)
                Console.WriteLine("B0401 PDF 產生結束 共計:" + bIndex + "筆");


            queryData = new DataView();
            var d0401List = new List<string>();
            queryData = query.Kind1SelectTbl2("*", "PRINT_TEMP", " PRINT_METHOD='D0401H' and IDEN_KEY = '" + identityKey + "'", "", "");
            if (queryData != null)
            {
                var rows = queryData.Table.Rows;
                for (int i = 0; i < rows.Count; i++)
                    d0401List.Add(rows[i]["PRINT_EINV_NUM"].ToString());
            }
            if (d0401List.Count > 0)
                Console.WriteLine("D0401 產生PDF 開始.");
            int dIndex = 0;
            //B2C折讓證明單 D0401H
            foreach (var d0401 in d0401List)
            {
                 dIndex = d0401List.IndexOf(d0401)+1;
                Console.WriteLine(string.Format("D0401 產生PDF 第{0}筆 {1} 折讓證明單開始.", dIndex, d0401));
                query.CallMkPDFaw(d0401);
                Hashtable hashTable = new Hashtable();
                hashTable["MAKE_FILE_DATE"] = DateTime.UtcNow;
                query.UpdateData2("PRINT_TEMP", hashTable, "IDEN_KEY = '" + identityKey + "' AND PRINT_EINV_NUM = " + "'" + d0401 + "'");
                Console.WriteLine(string.Format("D0401 產生PDF 第{0}筆 {1} 折讓證明單結束.", dIndex, d0401));

            }
            if (d0401List.Count > 0)
                Console.WriteLine("D0401 PDF 產生結束 共計:" + dIndex + "筆");


        }
    }
}