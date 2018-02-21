using EinvoiceUnity.Models;
using EinvoiceUnity.repositories;
using NSysDB.NTSQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ImC0401D
{
    private string m_ProcessName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
    public string ProcessName { get { return m_ProcessName; } set { m_ProcessName = value; } }

    public void Begin(string sKind0)
    {
        //try
        //{
        string[] sArr;
        using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
        { query.ReturnArr(out sArr); }
        string sFPathN = sArr[0];
        string sFPathP = sArr[1];
        string sFPathY = sArr[2];
        string sPaPartition = sArr[3];
        //Console.WriteLine(sFPathN);

        ////路徑要做變數@"D:\eInvoiceTXT\ImInput"
        ////sFPath = @"D:\eInvoiceTXT\ImInput";
        ////sFName = @"D:\eInvoiceTXT\ImInput\C0401H2017030501.txt";

        //NSysDB.XMLClass oXMLeParamts = new NSysDB.XMLClass();
        //string sFPathN = oXMLeParamts.GetParaXml("ImTxPaN");
        //string sFPathP = oXMLeParamts.GetParaXml("ImTxPaP");
        //string sFPathY = oXMLeParamts.GetParaXml("ImTxPaY");
        ////using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
        ////{
        ////    query.CreateDIR(sFPathN);
        ////    query.CreateDIR(sFPathP);
        ////    query.CreateDIR(sFPathY);
        ////}
        ////文字分隔符號
        //string sPaPartition = oXMLeParamts.GetParaXml("ImTxPaPartition");
        ////sFName = oXMLeParamts.GetParaXml("ImTxPaN") + "C0401H2017030501.txt";
        ////Console.WriteLine(sFPathY);

        //抓 C0401D*.* 的所有檔案
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
                                        if (query.Kind1SelectTbl3("C0401DSN", "MInvoiceNumber='" + charA[0].ToString() + "' And DSequenceNumber='" + charA[6].ToString() + "'", "C0401D") == 0)
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
                                            query.InsertDataNonKey("C0401D", data);
                                            data = null;
                                        }
                                        else
                                        {
                                            query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "[MInvoiceNumber:" + charA[0].ToString().Trim() + "][DSequenceNumber:" + charA[6].ToString().Trim() + "]", (counter + 1).ToString(), 16);

                                            query.GoToSTemp("C0401D", " MInvoiceNumber='" + charA[0].ToString() + "' ");
                                            query.GoToSTemp("C0401H", " MInvoiceNumber='" + charA[0].ToString() + "' ");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, ex.ToString(), (counter + 1).ToString(), 11);

                                        query.GoToSTemp("C0401D", " MInvoiceNumber='" + charA[0].ToString() + "' ");
                                        query.GoToSTemp("C0401H", " MInvoiceNumber='" + charA[0].ToString() + "' ");
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

    private void ValidDetailsHasError(ErrorInfoModel errorInfo)
    {
        var detailsError = errorInfo.ErrorBuffer.Where(o => o.Key == "C0401D").ToList();
        if (detailsError.Count > 0)
        {
            using (var sqlAdapter = new SQL1())
            {
                foreach (var error in detailsError)
                {
                    var detail = error.Value.Details.First();

                    string index = (error.Value.Details.IndexOf(detail) + 1).ToString();
                    sqlAdapter.GoLogsAll(error.Key, ProcessName, detail.SourceFile, detail.ErrorMessage, index, detail.ErrorLevel, false);
                    if (detail.ErrorGroupKey == 1 || detail.ErrorGroupKey == 3)
                        continue;
                    sqlAdapter.GoToSTemp("C0401D", " MInvoiceNumber='" + detail.EinvoiceNumber + "' ");
                    sqlAdapter.GoToSTemp("C0401H", " MInvoiceNumber='" + detail.EinvoiceNumber + "' ");
                }
            }
        }
    }

    public void Begin2(string path, string sKind0, ErrorInfoModel errorInfo, string identityKey)
    {
        if (!string.IsNullOrEmpty(sKind0))
            sKind0 = sKind0.ToUpper();
        string[] sArr;
        using (SQL1 sqlAdapter = new SQL1())
        { sqlAdapter.ReturnArr(out sArr); }
        string sPaPartition = sArr[3];
        using (SQL1 sqlAdapter = new SQL1())
        {
            List<EinvoiceC0401DTemp> tempData = new List<EinvoiceC0401DTemp>();
            var query = sqlAdapter.Kind1SelectTbl2("*", "FILE_TEMP", " EINVOICE_TP='C0401D' and IDENT_KEY = '" + identityKey + "'", "", "");
            if (query != null)
            {
                var rows = query.Table.Rows;
                for (int i = 0; i < rows.Count; i++)
                {
                    tempData.Add(new EinvoiceC0401DTemp()
                    {
                        EinvoiceContent = rows[i]["FILE_CONTENT"].ToString(),
                        EinvoiceFIlePath = rows[i]["FILE_NM"].ToString(),
                    });
                }
            }
            else
                return;
            string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            sqlAdapter.GoLogsAll(sPgSN, ProcessName, "FILE_TEMP", "", "", 1);
            //抓 C0401D*.* 的所有檔案
            foreach (var data in tempData)
            {
                int index = tempData.IndexOf(data) + 1;
                string sourceFile = data.EinvoiceFIlePath;
                string einvoiceNumber = string.Empty;
                string einvoiceDescription = string.Empty;
                string errorMsg = string.Empty;

                try
                {
                    string line = "";
                    List<Hashtable> einvoiceDataList = new List<Hashtable>();

                    line = data.EinvoiceContent;
                    if (line.Trim() != "")
                    {
                        string[] CutS = { sPaPartition };
                        string[] charA = line.Split(CutS, StringSplitOptions.None);
                        einvoiceNumber = charA[0];
                        einvoiceDescription = charA[1];
                        //字串尾要分號//共10個分號
                        if (charA.Length == 10)
                        {
                            #region 檢查Head有無寫入資料 有的話不寫入明細

                            var chkHeadHasError = EinvoiceRepository.CheckHeadHasError(sKind0, einvoiceNumber, errorInfo);

                            if (chkHeadHasError)
                            {
                                errorMsg = "[正式][" + sKind0 + "]" + einvoiceNumber + "[發票Head寫入時有錯，明細資料不寫入]";
                                EinvoiceRepository.AddEinvoiceToErrorBuffer(sKind0, einvoiceNumber, errorMsg, 1, 51, ref errorInfo, ProcessName, sourceFile, einvoiceDescription);
                                continue;
                            }

                            #endregion 檢查Head有無寫入資料 有的話不寫入明細

                            if (sqlAdapter.Kind1SelectTbl3("C0401DSN", "MInvoiceNumber='" + charA[0].ToString() + "' And DSequenceNumber='" + charA[6].ToString() + "'", "C0401D") == 0)
                            {
                                Console.WriteLine(string.Format("{0} 寫入正式資料庫 第{1}筆 發票號碼:{2}開始.", sKind0, index, einvoiceNumber));


                                Hashtable hashTable = new System.Collections.Hashtable();
                                hashTable["MInvoiceNumber"] = charA[0].ToString().Trim();
                                hashTable["DDescription"] = charA[1].ToString().Trim();
                                hashTable["DQuantity"] = charA[2].ToString().Trim();
                                hashTable["DUnit"] = charA[3].ToString().Trim();
                                hashTable["DUnitPrice"] = charA[4].ToString().Trim();
                                hashTable["DAmount"] = charA[5].ToString().Trim();
                                hashTable["DSequenceNumber"] = charA[6].ToString().Trim();
                                hashTable["DRemark"] = charA[7].ToString().Trim();
                                hashTable["DRelateNumber"] = charA[8].ToString().Trim();
                                hashTable["TxFileNmae"] = sourceFile;

                                string insertMsg = sqlAdapter.InsertDataNonKey("C0401D", hashTable);

                                #region 寫入有錯誤之處理

                                if (!string.IsNullOrEmpty(insertMsg))
                                {
                                    errorMsg = "[正式][" + sKind0 + "]" + einvoiceNumber + "[TXT寫入正式資料庫發生錯誤，資料不寫入]" + insertMsg;
                                    EinvoiceRepository.AddEinvoiceToErrorBuffer(sKind0, einvoiceNumber, errorMsg, 2, 51, ref errorInfo, ProcessName, sourceFile, einvoiceDescription);
                                }
                                else
                                    Console.WriteLine(string.Format("{0} 寫入正式資料庫 第{1}筆 發票號碼:{2}結束.", sKind0, index, einvoiceNumber));

                                #endregion 寫入有錯誤之處理
                            }
                            else
                            {
                                errorMsg = "[MInvoiceNumber:" + charA[0].ToString().Trim() + "][明細排列序號:" + charA[6].ToString().Trim() + "]";
                                EinvoiceRepository.AddEinvoiceToErrorBuffer(sKind0, einvoiceNumber, errorMsg + "[匯入商品細項的文字檔發生錯誤/此商品已存在!!]", 3, 16, ref errorInfo, ProcessName, sourceFile, einvoiceDescription);
                            }
                        }
                        else
                        {
                            errorMsg = "[正式][發票號碼:" + einvoiceNumber + "][字串尾要分號，共10個分號]";
                            EinvoiceRepository.AddEinvoiceToErrorBuffer(sKind0, einvoiceNumber, errorMsg, 4, 12, ref errorInfo, ProcessName, sourceFile, einvoiceDescription);
                        }
                    }
                    else
                    {
                        errorMsg = "[正式][發票號碼:" + einvoiceNumber + "][讀取的資料內容為空白]";
                        EinvoiceRepository.AddEinvoiceToErrorBuffer(sKind0, einvoiceNumber, errorMsg, 4, 12, ref errorInfo, ProcessName, sourceFile, einvoiceDescription);
                    }
                }
                catch (Exception ex)
                {
                    errorMsg = "[正式][未知錯誤]";
                    EinvoiceRepository.AddEinvoiceToErrorBuffer(sKind0, einvoiceNumber, errorMsg, 5, 15, ref errorInfo, ProcessName, sourceFile, einvoiceDescription);
                }
            }

            sqlAdapter.GoLogsAll(sPgSN, ProcessName, "FILE_TEMP", "", "", 2);
            ValidDetailsHasError(errorInfo);
        }
    }
}

internal class EinvoiceC0401DTemp
{
    public string EinvoiceContent { get; set; }
    public string EinvoiceFIlePath { get; set; }
}