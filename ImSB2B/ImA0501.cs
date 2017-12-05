using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class ImA0501
{
    public void Begin(string sKind0)
    {
        string[] sArr;
        using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
        { query.ReturnArr(out sArr); }
        string sFPathN = sArr[0];
        string sFPathP = sArr[1];
        string sFPathY = sArr[2];
        string sPaPartition = sArr[3];
        //Console.WriteLine(sFPathN);

        //抓 A0501*.* 的所有檔案
        foreach (string OkFName in System.IO.Directory.GetFileSystemEntries(sFPathN, sKind0 + "*.*"))
        {
            Console.WriteLine("檔案名稱1:" + OkFName);
            string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            try
            {
                System.IO.File.Move(OkFName, OkFName.Replace(sFPathN, sFPathP));
                string OkFNameP = OkFName.Replace(sFPathN, sFPathP);

                string line = "";
                int counter = 0;

                using (System.IO.StreamReader txtFile = new System.IO.StreamReader(OkFNameP, System.Text.Encoding.Default))
                {
                    Console.WriteLine("檔案名稱2:" + OkFNameP);

                    using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
                    {
                        //開始匯入
                        query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "", "", 1);
                    }

                    while ((line = txtFile.ReadLine()) != null)
                    {
                        if (line.Trim() != "")
                        {
                            //Console.WriteLine(line);
                            string[] CutS = { sPaPartition };
                            string[] charA = line.Split(CutS, StringSplitOptions.None);

                            //字串尾要分號//共10個分號
                            if (charA.Length == 10)
                            {
                                Console.WriteLine("作廢發票號碼:" + charA[0].ToString());
                                //for (int i = 0; i < charA.Length-1; i++)
                                //{
                                //    Console.WriteLine("Index : {0}, 字串 : {1}", (i + 1), charA[i]);
                                //}

                                using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
                                {
                                    try
                                    {
                                        if (query.Kind1SelectTbl3("A0501SN", "CancelInvoiceNumber='" + charA[0].ToString() + "'", "A0501") == 0)
                                        {
                                            System.Collections.Hashtable data = new System.Collections.Hashtable();

                                            data["CancelInvoiceNumber"] = charA[0].ToString().Trim();
                                            data["InvoiceDate"] = charA[1].ToString().Trim();
                                            data["BuyerId"] = charA[2].ToString().Trim();
                                            data["SellerId"] = charA[3].ToString().Trim();
                                            data["CancelDate"] = charA[4].ToString().Trim();

                                            data["CancelTime"] = charA[5].ToString().Trim();
                                            data["CancelReason"] = charA[6].ToString().Trim();
                                            data["ReturnTaxDocumentNumber"] = charA[7].ToString().Trim();
                                            data["Remark"] = charA[8].ToString().Trim();

                                            data["TxFileNmae"] = OkFName.ToString().Trim();
                                            query.InsertDataNonKey("A0501", data);
                                            data = null;
                                        }
                                        else
                                        { query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "[作廢發票號碼:" + charA[0].ToString() + "][作廢發票號碼已存在!!", (counter + 1).ToString(), 11); }
                                    }
                                    catch (Exception ex)
                                    {
                                        query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, ex.ToString(), (counter + 1).ToString(), 11);
                                    }
                                }


                            }
                            else
                            {
                                using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
                                {
                                    query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "", (counter + 1).ToString(), 12);
                                }

                            }

                            Console.WriteLine("間隔數:" + charA.Length.ToString());
                            counter++;
                        }
                    }

                    using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
                    {
                        //結束匯入
                        query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "", "", 2);
                    }

                }
                Console.WriteLine("筆數:" + counter.ToString());

                System.IO.File.Move(OkFNameP, OkFNameP.Replace(sFPathP, sFPathY).Replace(".txt", "_" + sPgSN + ".txt"));
            }
            catch (Exception ex)
            {
                using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
                {
                    query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "此檔案已存於:" + sFPathP + " [" + ex + "]", "", 13);
                }
            }

            //try
            //{
            //    //檔案已存在的FileMove
            //    Console.WriteLine(OkFName);
            //    Console.WriteLine(OkFName.Replace(sFPathN, sFPathY));

            //    System.IO.File.Move(OkFName, OkFName.Replace(sFPathN, sFPathY) + sPgSN);
            //    //Exception未處理,檔案已存在時，無法建立該檔案。
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("檔案已存在!!");
            //    using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
            //    {
            //        query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, ex.ToString(), "", 13);
            //    }
            //}

        }

        //}
        //catch (Exception ex)
        //{
        //    using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
        //    {
        //        // query.GoException(sPgSN, ex.ToString(), "[" + GetType().Assembly.Location + "] [" + System.Reflection.MethodInfo.GetCurrentMethod().ToString() + "]");
        //        query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, "", ex.ToString(), "", 14);
        //    }
        //}


    }
}

