using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

public class MPDF
{
    public void Begin(string sMInvoiceNumber = null)
    {
        string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
        {
            ////MkPDF.exe ALL  //全部生成PDF  //ExPdfYN=N

            #region 生成B2B 發票PDF

            System.Data.DataView dvResultALLA = query.Kind1SelectTbl2("MInvoiceNumber", "A0401H", "(ExPdfYN IS NULL) OR (ExPdfYN <> 'Y')", "", "");
            if (dvResultALLA != null)
            {
                if (dvResultALLA.Count > 0)
                {
                    for (int i = 0; i < dvResultALLA.Count; i++)
                    {
                        if (Convert.ToString(dvResultALLA.Table.Rows[i][0]).Trim().Length == 10)
                        {
                            //發票證明聯
                            query.CallMkPDFinvoAll(Convert.ToString(dvResultALLA.Table.Rows[i][0]), "1");
                            //發票證明聯補印
                            query.CallMkPDFinvoAll(Convert.ToString(dvResultALLA.Table.Rows[i][0]), "3");
                            //格式二
                            //query.CallMkPDFinvoAll2(Convert.ToString(dvResultALLA.Table.Rows[i][0]));
                        }
                        else
                        {
                            query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, Convert.ToString(dvResultALLA.Table.Rows[i][0]), "[發票號碼不是10馬!!不生成PDF]", "", 41);
                        }
                    }
                }
            }

            #endregion 生成B2B 發票PDF

            #region 生成B2C 發票PDF

            System.Data.DataView dvResultALLC = query.Kind1SelectTbl2("MInvoiceNumber", "C0401H", "(ExPdfYN IS NULL) OR (ExPdfYN <> 'Y')", "", "");
            if (dvResultALLC != null)
            {
                if (dvResultALLC.Count > 0)
                {
                    for (int i = 0; i < dvResultALLC.Count; i++)
                    {
                        if (Convert.ToString(dvResultALLC.Table.Rows[i][0]).Trim().Length == 10)
                        {
                            //發票證明聯
                            query.CallMkPDFinvo(Convert.ToString(dvResultALLC.Table.Rows[i][0]), "1");
                            //發票證明聯補印
                            query.CallMkPDFinvo(Convert.ToString(dvResultALLC.Table.Rows[i][0]), "3");
                        }
                        else
                        {
                            query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, Convert.ToString(dvResultALLC.Table.Rows[i][0]), "[發票號碼不是10馬!!不生成PDF]", "", 41);
                        }
                    }
                }
            }

            #endregion 生成B2C 發票PDF
        }
    }

    public void BeginByNumberList(string identityKey)
    {
        string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
        {
            var a0401List = new List<string>();
            var queryData = query.Kind1SelectTbl2("*", "PRINT_TEMP", " PRINT_METHOD='A0401H' and IDEN_KEY = '" + identityKey + "'", "", "");
            if (queryData != null)
            {
                var rows = queryData.Table.Rows;
                for (int i = 0; i < rows.Count; i++)
                    a0401List.Add(rows[i]["PRINT_EINV_NUM"].ToString());
            }

            #region 生成B2B 發票PDF
            if (a0401List.Count > 0)
                Console.WriteLine("A0401 產生PDF 開始.");
            int aIndex = 0;
            foreach (var a0401 in a0401List)
            {
                aIndex = a0401List.IndexOf(a0401) + 1;
                if (a0401.Trim().Length == 10)
                {
                    try
                    {
                        Console.WriteLine(string.Format("A0401 產生PDF 第{0}筆 {1} 證明聯開始.", aIndex, a0401));
                        //發票證明聯
                        query.CallMkPDFinvoAll(a0401, "1");
                        Console.WriteLine(string.Format("A0401 產生PDF 第{0}筆 {1} 證明聯結束.", aIndex, a0401));
                        Console.WriteLine(string.Format("A0401 產生PDF 第{0}筆 {1} 證明聯補印開始.", aIndex, a0401));

                        //發票證明聯補印
                        query.CallMkPDFinvoAll(a0401, "3");
                        Console.WriteLine(string.Format("A0401 產生PDF 第{0}筆 {1} 證明聯補印結束.", aIndex, a0401));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("A0401 產生PDF發生錯誤 第" + aIndex + "筆： " + ex.Message);
                        throw ex;
                    }

                    //格式二
                    //query.CallMkPDFinvoAll2(Convert.ToString(dvResultALLA.Table.Rows[i][0]));
                }
                else
                {
                    query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, a0401, "[發票號碼不是10碼!!不生成PDF]", "", 41);
                }
            }
            if (a0401List.Count > 0)
                Console.WriteLine("A0401 PDF 產生結束 共計:" + aIndex + "筆");


            foreach (var inv in a0401List)
            {
                Hashtable hashTable = new Hashtable();
                hashTable["MAKE_FILE_DATE"] = DateTime.UtcNow;
                query.UpdateData2("PRINT_TEMP", hashTable, "IDEN_KEY = '" + identityKey + "' AND PRINT_EINV_NUM = " + "'" + inv + "'");
            }

            #endregion 生成B2B 發票PDF

            #region 生成B2C 發票PDF

            queryData = new DataView();
            var c0401List = new List<string>();
            queryData = query.Kind1SelectTbl2("*", "PRINT_TEMP", " PRINT_METHOD='C0401H' and IDEN_KEY = '" + identityKey + "'", "", "");
            if (queryData != null)
            {
                var rows = queryData.Table.Rows;
                for (int i = 0; i < rows.Count; i++)
                    c0401List.Add(rows[i]["PRINT_EINV_NUM"].ToString());
            }
            if (c0401List.Count > 0)
                Console.WriteLine("C0401 產生PDF 開始.");

            int cIndex = 0;
            foreach (var c0401 in c0401List)
            {
                cIndex = c0401List.IndexOf(c0401) + 1;
                if (c0401.Trim().Length == 10)
                {
                    try
                    {
                        //發票證明聯
                        Console.WriteLine(string.Format("C0401 產生PDF 第{0}筆 {1} 證明聯開始.", cIndex, c0401));
                        query.CallMkPDFinvo(c0401, "1");
                        Console.WriteLine(string.Format("C0401 產生PDF 第{0}筆 {1} 證明聯開始.", cIndex, c0401));
                        Console.WriteLine(string.Format("C0401 產生PDF 第{0}筆 {1} 證明聯補印開始.", cIndex, c0401));

                        //發票證明聯補印
                        query.CallMkPDFinvo(c0401, "3");
                        Console.WriteLine(string.Format("C0401 產生PDF 第{0}筆 {1} 證明聯補印結束.", cIndex, c0401));

                        //格式二
                        //query.CallMkPDFinvoAll2(Convert.ToString(dvResultALLA.Table.Rows[i][0]));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("A0401 產生PDF發生錯誤 第" + aIndex + "筆： " + ex.Message);
                        throw ex;
                    }

                }
                else
                {
                    query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, c0401, "[發票號碼不是10碼!!不生成PDF]", "", 41);
                }
            }
            if (c0401List.Count > 0)
                Console.WriteLine("C0401 PDF 產生結束 共計:" + aIndex + "筆");

            foreach (var inv in c0401List)
            {
                Hashtable hashTable = new Hashtable();
                hashTable["MAKE_FILE_DATE"] = DateTime.UtcNow;
                query.UpdateData2("PRINT_TEMP", hashTable, "IDEN_KEY = '" + identityKey + "' AND PRINT_EINV_NUM = " + "'" + inv + "'");
            }

            #endregion 生成B2C 發票PDF
        }
    }
}