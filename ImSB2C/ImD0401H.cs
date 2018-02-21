using EinvoiceUnity.Models;
using EinvoiceUnity.repositories;
using NSysDB.NTSQL;
using System;
using System.Collections;
using System.Collections.Generic;

public class ImD0401H
{
    private Hashtable SetEinvoiceToHashtable(string[] charA, string sourceFile)
    {
        Hashtable data = new Hashtable();

        try
        {
            data["MAllowanceNumber"] = charA[0].ToString().Trim();
            data["MAllowanceDate"] = charA[1].ToString().Trim();

            data["MSIdentifier"] = charA[2].ToString().Trim();
            data["MSName"] = charA[3].ToString().Trim();
            data["MSAddress"] = charA[4].ToString().Trim();
            data["MSPersonInCharge"] = charA[5].ToString().Trim();
            data["MSTelephoneNumber"] = charA[6].ToString().Trim();
            data["MSFacsimileNumber"] = charA[7].ToString().Trim();
            data["MSEmailAddress"] = charA[8].ToString().Trim();
            data["MSCustomerNumber"] = charA[9].ToString().Trim();
            data["MSRoleRemark"] = charA[10].ToString().Trim();

            data["MBIdentifier"] = charA[11].ToString().Trim();
            data["MBName"] = charA[12].ToString().Trim();
            data["MBAddress"] = charA[13].ToString().Trim();
            data["MBPersonInCharge"] = charA[14].ToString().Trim();
            data["MBTelephoneNumber"] = charA[15].ToString().Trim();
            data["MBFacsimileNumber"] = charA[16].ToString().Trim();
            data["MBEmailAddress"] = charA[17].ToString().Trim();
            data["MBCustomerNumber"] = charA[18].ToString().Trim();
            data["MBRoleRemark"] = charA[19].ToString().Trim();

            data["MAllowanceType"] = charA[20].ToString().Trim();
            data["ATaxAmount"] = charA[21].ToString().Trim();
            data["ATotalAmount"] = charA[22].ToString().Trim();
            data["TxFileNmae"] = sourceFile;
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return data;
    }

    private string m_ProcessName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
    public string ProcessName { get { return m_ProcessName; } set { m_ProcessName = value; } }

    public void Begin(string sKind0, string identityKey)
    {
        Dictionary<string, List<string>> pdfNumList = new Dictionary<string, List<string>>();

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
        string sKind0UpperCase = sKind0.ToUpper();

        //抓 B0401H*.* 的所有檔案
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

                            //字串尾要分號//共24個分號
                            if (charA.Length == 24)
                            {
                                Console.WriteLine("折讓證明單號碼:" + charA[0].ToString());
                                //for (int i = 0; i < charA.Length-1; i++)
                                //{
                                //    Console.WriteLine("Index : {0}, 字串 : {1}", (i + 1), charA[i]);
                                //}

                                using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
                                {
                                    try
                                    {
                                        if (query.Kind1SelectTbl3("D0401SN", "MAllowanceNumber='" + charA[0].ToString() + "'", "D0401H") == 0)
                                        {
                                            System.Collections.Hashtable data = new System.Collections.Hashtable();

                                            data["MAllowanceNumber"] = charA[0].ToString().Trim();
                                            data["MAllowanceDate"] = charA[1].ToString().Trim();

                                            data["MSIdentifier"] = charA[2].ToString().Trim();
                                            data["MSName"] = charA[3].ToString().Trim();
                                            data["MSAddress"] = charA[4].ToString().Trim();
                                            data["MSPersonInCharge"] = charA[5].ToString().Trim();
                                            data["MSTelephoneNumber"] = charA[6].ToString().Trim();
                                            data["MSFacsimileNumber"] = charA[7].ToString().Trim();
                                            data["MSEmailAddress"] = charA[8].ToString().Trim();
                                            data["MSCustomerNumber"] = charA[9].ToString().Trim();
                                            data["MSRoleRemark"] = charA[10].ToString().Trim();

                                            data["MBIdentifier"] = charA[11].ToString().Trim();
                                            data["MBName"] = charA[12].ToString().Trim();
                                            data["MBAddress"] = charA[13].ToString().Trim();
                                            data["MBPersonInCharge"] = charA[14].ToString().Trim();
                                            data["MBTelephoneNumber"] = charA[15].ToString().Trim();
                                            data["MBFacsimileNumber"] = charA[16].ToString().Trim();
                                            data["MBEmailAddress"] = charA[17].ToString().Trim();
                                            data["MBCustomerNumber"] = charA[18].ToString().Trim();
                                            data["MBRoleRemark"] = charA[19].ToString().Trim();

                                            data["MAllowanceType"] = charA[20].ToString().Trim();
                                            data["ATaxAmount"] = charA[21].ToString().Trim();
                                            data["ATotalAmount"] = charA[22].ToString().Trim();

                                            data["TxFileNmae"] = OkFName.ToString().Trim();
                                            query.InsertDataNonKey("D0401H", data);
                                            data = null;
                                            if (!pdfNumList.ContainsKey(sKind0UpperCase))
                                                pdfNumList[sKind0UpperCase] = new List<string>();
                                            pdfNumList[sKind0UpperCase].Add(charA[0].ToString().Trim());
                                        }
                                        else
                                        { query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "[折讓證明單號碼:" + charA[0].ToString() + "][此折讓證明單號碼已存在!!", (counter + 1).ToString(), 11); }
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
        using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
        {
            foreach (var inv in pdfNumList)
            {
                foreach (var item in inv.Value)
                {
                    Hashtable hashTable = new Hashtable();
                    hashTable["PRINT_METHOD"] = inv.Key;
                    hashTable["PRINT_EINV_NUM"] = item;
                    //hashTable["MAKE_FILE_DATE"] = DateTime.UtcNow;
                    hashTable["WRITE_DATE"] = DateTime.UtcNow;
                    hashTable["IDEN_KEY"] = identityKey;
                    string insertMsg = query.InsertDataNonKey("PRINT_TEMP", hashTable);
                }
            }
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

    public void Begin2(string path, string sKind0, ErrorInfoModel errorInfo, string identityKey)
    {
        Dictionary<string, List<string>> pdfNumList = new Dictionary<string, List<string>>();

        if (!string.IsNullOrEmpty(sKind0))
            sKind0 = sKind0.ToUpper();
        string[] sArr;
        using (SQL1 sqlAdapter = new SQL1())
        { sqlAdapter.ReturnArr(out sArr); }

        string sPaPartition = sArr[3];
        using (var sqlAdapter = new SQL1())
        {
            List<EinvoiceD0401HTemp> tempData = new List<EinvoiceD0401HTemp>();
            var query = sqlAdapter.Kind1SelectTbl2("*", "FILE_TEMP", " EINVOICE_TP='D0401H' and IDENT_KEY = '" + identityKey + "'", "", "");
            if (query != null)
            {
                var rows = query.Table.Rows;
                for (int i = 0; i < rows.Count; i++)
                {
                    tempData.Add(new EinvoiceD0401HTemp()
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
            foreach (var data in tempData)
            {
                int index = tempData.IndexOf(data) + 1;
                string sourceFile = data.EinvoiceFIlePath;
                string einvoiceNumber = string.Empty;
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
                        einvoiceNumber = charA[0].ToUpper();
                        //字串尾要分號//共24個分號
                        if (charA.Length == 24)
                        {
                            if (sqlAdapter.Kind1SelectTbl3("D0401SN", "MAllowanceNumber='" + charA[0].ToString() + "'", "D0401H") == 0)
                            {
                                Console.WriteLine(string.Format("{0} 寫入正式資料庫 第{1}筆 發票號碼:{2}開始.", sKind0, index, einvoiceNumber));

                                Hashtable hashData = SetEinvoiceToHashtable(charA, sourceFile);
                                string insertMsg = sqlAdapter.InsertDataNonKey("D0401H", hashData);

                                #region 寫入有錯誤之處理

                                if (!string.IsNullOrEmpty(insertMsg))
                                {
                                    errorMsg = "[正式][" + sKind0 + "]" + einvoiceNumber + "[TXT寫入正式資料庫發生錯誤，資料不寫入]" + insertMsg;
                                    sqlAdapter.GoLogsAll(sPgSN, ProcessName, sourceFile, errorMsg, index.ToString(), 51, false);
                                    EinvoiceRepository.AddEinvoiceToErrorBuffer(sKind0, einvoiceNumber, errorMsg, 2, 51, ref errorInfo, ProcessName);
                                }
                                else
                                {
                                    if (!pdfNumList.ContainsKey(sKind0))
                                        pdfNumList[sKind0] = new List<string>();
                                    pdfNumList[sKind0].Add(einvoiceNumber);
                                    Console.WriteLine(string.Format("{0} 寫入正式資料庫 第{1}筆 發票號碼:{2}結束.", sKind0, index, einvoiceNumber));
                                }

                                #endregion 寫入有錯誤之處理
                            }
                            else
                            {
                                errorMsg = "[正式][折讓證明單號碼:" + einvoiceNumber + "][此折讓證明單號碼已存在,資料不寫入]";
                                sqlAdapter.GoLogsAll(sPgSN, ProcessName, sourceFile, errorMsg, index.ToString(), 11, false);
                                EinvoiceRepository.AddEinvoiceToErrorBuffer(sKind0, einvoiceNumber, errorMsg, 3, 11, ref errorInfo, ProcessName);
                            }
                        }
                        else
                        {
                            errorMsg = "[正式][折讓證明單號碼:" + einvoiceNumber + "][字串尾要分號，共24個分號]";
                            sqlAdapter.GoLogsAll(sPgSN, ProcessName, sourceFile, "[正式]", index.ToString(), 12);
                            EinvoiceRepository.AddEinvoiceToErrorBuffer(sKind0, einvoiceNumber, errorMsg, 4, 12, ref errorInfo, ProcessName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorMsg = "[正式][未知錯誤]";
                    sqlAdapter.GoLogsAll(sPgSN, ProcessName, sourceFile, errorMsg + ex.ToString(), "", 15, false);
                    EinvoiceRepository.AddEinvoiceToErrorBuffer(sKind0, einvoiceNumber, errorMsg, 5, 15, ref errorInfo, ProcessName);
                }
            }
            sqlAdapter.GoLogsAll(sPgSN, ProcessName, "FILE_TEMP", "", "", 2);
            foreach (var inv in pdfNumList)
            {
                foreach (var item in inv.Value)
                {
                    Hashtable hashTable = new Hashtable();
                    hashTable["PRINT_METHOD"] = inv.Key;
                    hashTable["PRINT_EINV_NUM"] = item;
                    //hashTable["MAKE_FILE_DATE"] = DateTime.UtcNow;
                    hashTable["WRITE_DATE"] = DateTime.UtcNow;
                    hashTable["IDEN_KEY"] = identityKey;
                    string insertMsg = sqlAdapter.InsertDataNonKey("PRINT_TEMP", hashTable);
                }
            }
        }
    }
}

internal class EinvoiceD0401HTemp
{
    public string EinvoiceContent { get; set; }
    public string EinvoiceFIlePath { get; set; }
}