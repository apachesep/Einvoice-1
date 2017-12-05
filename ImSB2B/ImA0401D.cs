using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class ImA0401D
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

        //抓 A0401D*.* 的所有檔案
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
                                Console.WriteLine("發票號碼:" + charA[0].ToString());
                                //for (int i = 0; i < charA.Length-1; i++)
                                //{
                                //    Console.WriteLine("Index : {0}, 字串 : {1}", (i + 1), charA[i]);
                                //}

                                using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
                                {
                                    try
                                    {
                                        if (query.Kind1SelectTbl3("A0401DSN", "MInvoiceNumber='" + charA[0].ToString() + "' And DSequenceNumber='" + charA[6].ToString() + "'", "A0401D") == 0)
                                        {
                                            System.Collections.Hashtable data = new System.Collections.Hashtable();
                                            data["MInvoiceNumber"] = charA[0].ToString().Trim();
                                            data["DDescription"] = charA[1].ToString().Trim();
                                            data["DQuantity"] = charA[2].ToString().Trim();
                                            data["DUnit"] = charA[3].ToString().Trim();
                                            data["DUnitPrice"] = charA[4].ToString().Trim();
                                            data["DAmount"] = charA[5].ToString().Trim();
                                            data["DSequenceNumber"] = charA[6].ToString().Trim();
                                            data["DRemark"] = charA[7].ToString().Trim();
                                            data["DRelateNumber"] = charA[8].ToString().Trim();

                                            data["TxFileNmae"] = OkFName.ToString().Trim();
                                            query.InsertDataNonKey("A0401D", data);
                                            data = null;
                                        }
                                        else
                                        {
                                            query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "[MInvoiceNumber:" + charA[0].ToString().Trim() + "][明細排列序號:" + charA[6].ToString().Trim() + "]", (counter + 1).ToString(), 16);

                                            query.GoToSTemp("A0401D", " MInvoiceNumber='" + charA[0].ToString() + "' ");
                                            query.GoToSTemp("A0401H", " MInvoiceNumber='" + charA[0].ToString() + "' ");
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, ex.ToString(), (counter + 1).ToString(), 11);
                                       
                                        query.GoToSTemp("A0401D", " MInvoiceNumber='" + charA[0].ToString() + "' ");
                                        query.GoToSTemp("A0401H", " MInvoiceNumber='" + charA[0].ToString() + "' ");
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

