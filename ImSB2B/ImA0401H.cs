using EinvoiceUnity.Models;
using EinvoiceUnity.repositories;
using NSysDB.NTSQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class ImA0401H
{
    /// <summary>
    /// 驗證發票內容
    /// </summary>
    /// <param name="charA"></param>
    /// <returns></returns>
    private Dictionary<bool, string> ValidInvoiceData(string[] charA)
    {
        int[] parserIndexAry = new int[] { 12, 13, 36, 37, 38, 41, 42, 43 };
        List<string> validData = new List<string>();

        foreach (var a in parserIndexAry)
            validData.Add(charA[a]);
        Dictionary<bool, string> errorBuffer = new Dictionary<bool, string>();
        string errorMsgTitle = "[發票號碼: " + charA[0].ToString() + "]";
        int index = 0;
        foreach (var data in validData)
        {
            if (errorBuffer.Count > 0 && errorBuffer.Keys.First() == false)
                break;
            //int index = validData.IndexOf(data);

            switch (index)
            {
                case 0:
                    if (data.Equals("0000000000"))
                        errorBuffer.Add(false, errorMsgTitle += "[買方營業人統一編號須<>0000000000]");
                    break;

                case 1:
                    if (data.Equals("0000"))
                        errorBuffer.Add(false, errorMsgTitle += "[買方營業人名稱須<>0000]");
                    break;

                default:
                    int n;
                    bool isInt = int.TryParse(data.Trim(), out n);
                    if (!(isInt && n >= 0))
                    {
                        switch (index)
                        {
                            case 2:
                                errorBuffer.Add(false, errorMsgTitle += "[ASalesAmount要>=0且不能有小數]");
                                break;

                            case 3:
                                errorBuffer.Add(false, errorMsgTitle += "[AFreeTaxSalesAmount要>=0且不能有小數]");
                                break;

                            case 4:
                                errorBuffer.Add(false, errorMsgTitle += "[AZeroTaxSalesAmount要>=0且不能有小數]");
                                break;

                            case 5:
                                errorBuffer.Add(false, errorMsgTitle += "[ATaxAmount要>=0且不能有小數]");
                                break;

                            case 6:
                                errorBuffer.Add(false, errorMsgTitle += "[ATotalAmount要>=0且不能有小數]");
                                break;

                            case 7:
                                errorBuffer.Add(false, errorMsgTitle += "[ADiscountAmount要>=0且不能有小數]");
                                break;

                            default:
                                break;
                        }
                    }
                    break;
            }
            index++;
        }
        return errorBuffer;
    }

    private Hashtable SetEinvoiceToHashtable(string[] charA, string sourceFile)
    {
        Hashtable data = new Hashtable();

        try
        {
            data["MInvoiceNumber"] = charA[0].ToString().Trim();
            data["MInvoiceDate"] = charA[1].ToString().Trim();
            data["MInvoiceTime"] = charA[2].ToString().Trim();

            data["MSIdentifier"] = charA[3].ToString().Trim();
            data["MSName"] = charA[4].ToString().Trim();
            data["MSAddress"] = charA[5].ToString().Trim();
            data["MSPersonInCharge"] = charA[6].ToString().Trim();
            data["MSTelephoneNumber"] = charA[7].ToString().Trim();
            data["MSFacsimileNumber"] = charA[8].ToString().Trim();
            data["MSEmailAddress"] = charA[9].ToString().Trim();
            data["MSCustomerNumber"] = charA[10].ToString().Trim();
            data["MSRoleRemark"] = charA[11].ToString().Trim();

            data["MBIdentifier"] = charA[12].ToString().Trim();
            data["MBName"] = charA[13].ToString().Trim();
            data["MBAddress"] = charA[14].ToString().Trim();
            data["MBPersonInCharge"] = charA[15].ToString().Trim();
            data["MBTelephoneNumber"] = charA[16].ToString().Trim();
            data["MBFacsimileNumber"] = charA[17].ToString().Trim();
            data["MBEmailAddress"] = charA[18].ToString().Trim();
            data["MBCustomerNumber"] = charA[19].ToString().Trim();
            data["MBRoleRemark"] = charA[20].ToString().Trim();
            data["MCheckNumber"] = charA[21].ToString().Trim();
            data["MBuyerRemark"] = charA[22].ToString().Trim();
            data["MMainRemark"] = charA[23].ToString().Trim();
            data["MCustomsClearanceMark"] = charA[24].ToString().Trim();
            data["MCategory"] = charA[25].ToString().Trim();
            data["MRelateNumber"] = charA[26].ToString().Trim();
            data["MInvoiceType"] = charA[27].ToString().Trim();
            data["MGroupMark"] = charA[28].ToString().Trim();
            data["MDonateMark"] = charA[29].ToString().Trim();
            data["MCarrierType"] = charA[30].ToString().Trim();
            data["MCarrierId1"] = charA[31].ToString().Trim();
            data["MCarrierId2"] = charA[32].ToString().Trim();
            data["MPrintMark"] = charA[33].ToString().Trim();
            data["MNPOBAN"] = charA[34].ToString().Trim();
            data["MRandomNumber"] = charA[35].ToString().Trim();

            data["ASalesAmount"] = charA[36].ToString().Trim();
            data["AFreeTaxSalesAmount"] = charA[37].ToString().Trim();
            data["AZeroTaxSalesAmount"] = charA[38].ToString().Trim();
            data["ATaxType"] = charA[39].ToString().Trim();
            data["ATaxRate"] = charA[40].ToString().Trim();
            data["ATaxAmount"] = charA[41].ToString().Trim();
            data["ATotalAmount"] = charA[42].ToString().Trim();
            data["ADiscountAmount"] = charA[43].ToString().Trim();

            data["AOriginalCurrencyAmount"] = charA[44].ToString().Trim();
            data["AExchangeRate"] = charA[45].ToString().Trim();
            data["ACurrency"] = charA[46].ToString().Trim();

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
        string[] sArr;
        using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
        { query.ReturnArr(out sArr); }
        string sFPathN = sArr[0];
        string sFPathP = sArr[1];
        string sFPathY = sArr[2];
        string sPaPartition = sArr[3];
        //Console.WriteLine(sFPathN);
        string sKind0UpperCase = sKind0.ToUpper();
        //抓 A0401H*.* 的所有檔案
        foreach (string OkFName in System.IO.Directory.GetFileSystemEntries(sFPathN, sKind0 + "*.*"))
        {
            Console.WriteLine("檔案名稱1:" + OkFName);
            string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            //try
            //{
            System.IO.File.Move(OkFName, OkFName.Replace(sFPathN, sFPathP));
            string OkFNameP = OkFName.Replace(sFPathN, sFPathP);

            string line = "";
            int counter = 0;

            using (System.IO.StreamReader txtFile = new System.IO.StreamReader(OkFNameP, System.Text.Encoding.Default))
            {
                Console.WriteLine("檔案名稱2:" + OkFNameP);

                using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
                {
                    //Log_開始匯入
                    query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "", "", 1);
                }



                while ((line = txtFile.ReadLine()) != null)
                {
                    if (line.Trim() != "")
                    {

                        //Console.WriteLine(line);

                        //string[] charA = line.Split(';');
                        //string[] CutS = { ";" };
                        string[] CutS = { sPaPartition };
                        string[] charA = line.Split(CutS, StringSplitOptions.None);

                        //字串尾要分號//共48個分號
                        if (charA.Length == 48)
                        {
                            Console.WriteLine("發票號碼:" + charA[0].ToString());
                            //for (int i = 0; i < charA.Length-1; i++)
                            //{
                            //    Console.WriteLine("Index : {0}, 字串 : {1}", (i + 1), charA[i]);
                            //}

                            using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
                            {
                                //try
                                //{

                                if (query.Kind1SelectTbl3("A0401SN", "MInvoiceNumber='" + charA[0].ToString() + "'", "A0401H") == 0)
                                {

                                    if (charA[12].ToString().Trim() != "0000000000")
                                    {
                                        if (charA[13].ToString().Trim() != "0000")
                                        {
                                            int n;
                                            if (int.TryParse(charA[36].ToString().Trim(), out n))
                                            {
                                                if (int.TryParse(charA[37].ToString().Trim(), out n))
                                                {
                                                    if (int.TryParse(charA[38].ToString().Trim(), out n))
                                                    {
                                                        if (int.TryParse(charA[41].ToString().Trim(), out n))
                                                        {
                                                            if (int.TryParse(charA[42].ToString().Trim(), out n))
                                                            {
                                                                if (int.TryParse(charA[43].ToString().Trim(), out n))
                                                                {

                                                                    System.Collections.Hashtable data = new System.Collections.Hashtable();

                                                                    //if (charA[12].ToString().Trim() == "0000000000")
                                                                    //{ data["ACKind"] = "C0401H"; }
                                                                    //else
                                                                    //{ data["ACKind"] = "A0401H"; }
                                                                    //data["ACKind"] = sKind0.ToUpper().Substring(0, 6);

                                                                    data["MInvoiceNumber"] = charA[0].ToString().Trim();
                                                                    data["MInvoiceDate"] = charA[1].ToString().Trim();
                                                                    data["MInvoiceTime"] = charA[2].ToString().Trim();

                                                                    data["MSIdentifier"] = charA[3].ToString().Trim();
                                                                    data["MSName"] = charA[4].ToString().Trim();
                                                                    data["MSAddress"] = charA[5].ToString().Trim();
                                                                    data["MSPersonInCharge"] = charA[6].ToString().Trim();
                                                                    data["MSTelephoneNumber"] = charA[7].ToString().Trim();
                                                                    data["MSFacsimileNumber"] = charA[8].ToString().Trim();
                                                                    data["MSEmailAddress"] = charA[9].ToString().Trim();
                                                                    data["MSCustomerNumber"] = charA[10].ToString().Trim();
                                                                    data["MSRoleRemark"] = charA[11].ToString().Trim();

                                                                    data["MBIdentifier"] = charA[12].ToString().Trim();
                                                                    data["MBName"] = charA[13].ToString().Trim();
                                                                    data["MBAddress"] = charA[14].ToString().Trim();
                                                                    data["MBPersonInCharge"] = charA[15].ToString().Trim();
                                                                    data["MBTelephoneNumber"] = charA[16].ToString().Trim();
                                                                    data["MBFacsimileNumber"] = charA[17].ToString().Trim();
                                                                    data["MBEmailAddress"] = charA[18].ToString().Trim();
                                                                    data["MBCustomerNumber"] = charA[19].ToString().Trim();
                                                                    data["MBRoleRemark"] = charA[20].ToString().Trim();

                                                                    data["MCheckNumber"] = charA[21].ToString().Trim();
                                                                    data["MBuyerRemark"] = charA[22].ToString().Trim();
                                                                    data["MMainRemark"] = charA[23].ToString().Trim();
                                                                    data["MCustomsClearanceMark"] = charA[24].ToString().Trim();
                                                                    data["MCategory"] = charA[25].ToString().Trim();
                                                                    data["MRelateNumber"] = charA[26].ToString().Trim();
                                                                    data["MInvoiceType"] = charA[27].ToString().Trim();
                                                                    data["MGroupMark"] = charA[28].ToString().Trim();
                                                                    data["MDonateMark"] = charA[29].ToString().Trim();
                                                                    data["MCarrierType"] = charA[30].ToString().Trim();
                                                                    data["MCarrierId1"] = charA[31].ToString().Trim();
                                                                    data["MCarrierId2"] = charA[32].ToString().Trim();
                                                                    data["MPrintMark"] = charA[33].ToString().Trim();
                                                                    data["MNPOBAN"] = charA[34].ToString().Trim();
                                                                    data["MRandomNumber"] = charA[35].ToString().Trim();

                                                                    data["ASalesAmount"] = charA[36].ToString().Trim();
                                                                    data["AFreeTaxSalesAmount"] = charA[37].ToString().Trim();
                                                                    data["AZeroTaxSalesAmount"] = charA[38].ToString().Trim();
                                                                    data["ATaxType"] = charA[39].ToString().Trim();
                                                                    data["ATaxRate"] = charA[40].ToString().Trim();
                                                                    data["ATaxAmount"] = charA[41].ToString().Trim();
                                                                    data["ATotalAmount"] = charA[42].ToString().Trim();
                                                                    data["ADiscountAmount"] = charA[43].ToString().Trim();

                                                                    data["AOriginalCurrencyAmount"] = charA[44].ToString().Trim();
                                                                    data["AExchangeRate"] = charA[45].ToString().Trim();
                                                                    data["ACurrency"] = charA[46].ToString().Trim();

                                                                    data["TxFileNmae"] = OkFName.ToString().Trim();
                                                                    query.InsertDataNonKey("A0401H", data);
                                                                    data = null;
                                                                    if (!pdfNumList.ContainsKey(sKind0UpperCase))
                                                                        pdfNumList[sKind0UpperCase] = new List<string>();
                                                                    pdfNumList[sKind0UpperCase].Add(charA[0].ToString());
                                                                }
                                                                else
                                                                { query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "[發票號碼:" + charA[0].ToString() + "][ADiscountAmount要>=0且不能有小數]", (counter + 1).ToString(), 11); }
                                                            }
                                                            else
                                                            { query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "[發票號碼:" + charA[0].ToString() + "][ATotalAmount要>=0且不能有小數]", (counter + 1).ToString(), 11); }
                                                        }
                                                        else
                                                        { query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "[發票號碼:" + charA[0].ToString() + "][ATaxAmount要>=0且不能有小數]", (counter + 1).ToString(), 11); }
                                                    }
                                                    else
                                                    { query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "[發票號碼:" + charA[0].ToString() + "][AZeroTaxSalesAmount要>=0且不能有小數]", (counter + 1).ToString(), 11); }
                                                }
                                                else
                                                { query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "[發票號碼:" + charA[0].ToString() + "][AFreeTaxSalesAmount要>=0且不能有小數]", (counter + 1).ToString(), 11); }
                                            }
                                            else
                                            { query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "[發票號碼:" + charA[0].ToString() + "][ASalesAmount要>=0且不能有小數]", (counter + 1).ToString(), 11); }
                                        }
                                        else
                                        { query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "[發票號碼:" + charA[0].ToString() + "][買方營業人名稱須<>0000]", (counter + 1).ToString(), 11); }
                                    }
                                    else
                                    { query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "[發票號碼:" + charA[0].ToString() + "][買方營業人統一編號須<>0000000000]", (counter + 1).ToString(), 11); }
                                }
                                else
                                { query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "[發票號碼:" + charA[0].ToString() + "][此發票號碼已存在!!", (counter + 1).ToString(), 11); }
                                //}
                                //catch (Exception ex)
                                //{
                                //    query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "[發票號碼:" + charA[0].ToString() + "][字串有例外錯誤!!]" + ex.ToString(), (counter + 1).ToString(), 11);
                                //}
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

                //結束匯入
                using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
                {
                    query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "", "", 2);
                }


            }

            Console.WriteLine("筆數:" + counter.ToString());


            System.IO.File.Move(OkFNameP, OkFNameP.Replace(sFPathP, sFPathY).Replace(".txt", "_" + sPgSN + ".txt"));

            //}
            //catch (Exception ex)
            //{
            //    using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
            //    {
            //        query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "此檔案已存於:" + sFPathP + " [" + ex + "]", "", 13);
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
        //Console.ReadLine();
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
            List<EinvoiceA0401HTemp> tempData = new List<EinvoiceA0401HTemp>();
            var query = sqlAdapter.Kind1SelectTbl2("*", "FILE_TEMP", " EINVOICE_TP='A0401H' and IDENT_KEY = '" + identityKey + "'", "", "");
            if (query != null)
            {
                var rows = query.Table.Rows;
                for (int i = 0; i < rows.Count; i++)
                {
                    tempData.Add(new EinvoiceA0401HTemp()
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
                        //字串尾要分號//共48個分號
                        if (charA.Length == 48)
                        {
                            if (sqlAdapter.Kind1SelectTbl3("A0401SN", "MInvoiceNumber='" + charA[0].ToString() + "'", "A0401H") == 0)
                            {
                                Console.WriteLine(string.Format("{0} 寫入正式資料庫 第{1}筆 發票號碼:{2}開始.", sKind0, index, einvoiceNumber));
                                Dictionary<bool, string> validErrorBuffer = ValidInvoiceData(charA);

                                if (validErrorBuffer.Count > 0 && validErrorBuffer.Keys.First() == false)
                                {
                                    errorMsg = validErrorBuffer.Values.First() + "[正式]驗證失敗,資料不寫入";
                                    sqlAdapter.GoLogsAll(sPgSN, ProcessName, sourceFile, errorMsg, index.ToString(), 11, false);
                                    EinvoiceRepository.AddEinvoiceToErrorBuffer(sKind0, einvoiceNumber, errorMsg, 1, 11, ref errorInfo, ProcessName);
                                    continue;
                                }
                                Hashtable hashData = SetEinvoiceToHashtable(charA, sourceFile);
                                string insertMsg = sqlAdapter.InsertDataNonKey("A0401H", hashData);

                                #region 寫入有錯誤之處理

                                if (!string.IsNullOrEmpty(insertMsg))
                                {
                                    errorMsg = "[正式][" + sKind0 + "]" + einvoiceNumber + "[TXT寫入正式資料庫發生錯誤，資料不寫入]" + insertMsg;
                                    sqlAdapter.GoLogsAll(sPgSN, ProcessName, sourceFile, errorMsg, index.ToString(), 51, false);
                                    EinvoiceRepository.AddEinvoiceToErrorBuffer(sKind0, einvoiceNumber, errorMsg, 2, 51, ref errorInfo, ProcessName);
                                    Console.WriteLine(errorMsg);
                                }
                                else
                                {
                                    if (!pdfNumList.ContainsKey(sKind0))
                                        pdfNumList[sKind0] = new List<string>();
                                    pdfNumList[sKind0].Add(einvoiceNumber);
                                    Console.WriteLine(string.Format("{0} 寫入正式資料庫 第{1}筆 發票號碼:{2}完成.", sKind0, index, einvoiceNumber));
                                }

                                #endregion 寫入有錯誤之處理
                            }
                            else
                            {
                                errorMsg = "[正式][發票號碼:" + einvoiceNumber + "][此發票號碼已存在,資料不寫入]";
                                sqlAdapter.GoLogsAll(sPgSN, ProcessName, sourceFile, errorMsg, index.ToString(), 11, false);
                                EinvoiceRepository.AddEinvoiceToErrorBuffer(sKind0, einvoiceNumber, errorMsg, 3, 11, ref errorInfo, ProcessName);
                            }
                        }
                        else
                        {
                            errorMsg = "[正式][發票號碼:" + einvoiceNumber + "][字串尾要分號，共48個分號]";
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

internal class EinvoiceA0401HTemp
{
    public string EinvoiceContent { get; set; }
    public string EinvoiceFIlePath { get; set; }
}

//while ((line = txtFile.ReadLine()) != null)
//{
//    if (line.Trim() != "")
//    {
//        string[] CutS = { sPaPartition };
//        string[] charA = line.Split(CutS, StringSplitOptions.None);

//        //字串尾要分號//共48個分號
//        if (charA.Length == 48)
//        {
//            Console.WriteLine("發票號碼:" + charA[0].ToString());

//            using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
//            {
//                //try
//                //{
//                if (query.Kind1SelectTbl3("A0401SN", "MInvoiceNumber='" + charA[0].ToString() + "'", "A0401H") == 0)
//                {
//                    Dictionary<bool, string> errorBuffer = ValidInvoiceData(charA);
//                    if (errorBuffer.Count > 0 && errorBuffer.Keys.First() == false)
//                        break;

//                    if (charA[12].ToString().Trim() != "0000000000")
//                    {
//                        if (charA[13].ToString().Trim() != "0000")
//                        {
//                            int n;
//                            if (int.TryParse(charA[36].ToString().Trim(), out n))
//                            {
//                                if (int.TryParse(charA[37].ToString().Trim(), out n))
//                                {
//                                    if (int.TryParse(charA[38].ToString().Trim(), out n))
//                                    {
//                                        if (int.TryParse(charA[41].ToString().Trim(), out n))
//                                        {
//                                            if (int.TryParse(charA[42].ToString().Trim(), out n))
//                                            {
//                                                if (int.TryParse(charA[43].ToString().Trim(), out n))
//                                                {
//                                                    System.Collections.Hashtable data = new System.Collections.Hashtable();

//                                                    //if (charA[12].ToString().Trim() == "0000000000")
//                                                    //{ data["ACKind"] = "C0401H"; }
//                                                    //else
//                                                    //{ data["ACKind"] = "A0401H"; }
//                                                    //data["ACKind"] = sKind0.ToUpper().Substring(0, 6);

//                                                    data["MInvoiceNumber"] = charA[0].ToString().Trim();
//                                                    data["MInvoiceDate"] = charA[1].ToString().Trim();
//                                                    data["MInvoiceTime"] = charA[2].ToString().Trim();

//                                                    data["MSIdentifier"] = charA[3].ToString().Trim();
//                                                    data["MSName"] = charA[4].ToString().Trim();
//                                                    data["MSAddress"] = charA[5].ToString().Trim();
//                                                    data["MSPersonInCharge"] = charA[6].ToString().Trim();
//                                                    data["MSTelephoneNumber"] = charA[7].ToString().Trim();
//                                                    data["MSFacsimileNumber"] = charA[8].ToString().Trim();
//                                                    data["MSEmailAddress"] = charA[9].ToString().Trim();
//                                                    data["MSCustomerNumber"] = charA[10].ToString().Trim();
//                                                    data["MSRoleRemark"] = charA[11].ToString().Trim();

//                                                    data["MBIdentifier"] = charA[12].ToString().Trim();
//                                                    data["MBName"] = charA[13].ToString().Trim();
//                                                    data["MBAddress"] = charA[14].ToString().Trim();
//                                                    data["MBPersonInCharge"] = charA[15].ToString().Trim();
//                                                    data["MBTelephoneNumber"] = charA[16].ToString().Trim();
//                                                    data["MBFacsimileNumber"] = charA[17].ToString().Trim();
//                                                    data["MBEmailAddress"] = charA[18].ToString().Trim();
//                                                    data["MBCustomerNumber"] = charA[19].ToString().Trim();
//                                                    data["MBRoleRemark"] = charA[20].ToString().Trim();
//                                                    throw new Exception("test move file error");
//                                                    data["MCheckNumber"] = charA[21].ToString().Trim();
//                                                    data["MBuyerRemark"] = charA[22].ToString().Trim();
//                                                    data["MMainRemark"] = charA[23].ToString().Trim();
//                                                    data["MCustomsClearanceMark"] = charA[24].ToString().Trim();
//                                                    data["MCategory"] = charA[25].ToString().Trim();
//                                                    data["MRelateNumber"] = charA[26].ToString().Trim();
//                                                    data["MInvoiceType"] = charA[27].ToString().Trim();
//                                                    data["MGroupMark"] = charA[28].ToString().Trim();
//                                                    data["MDonateMark"] = charA[29].ToString().Trim();
//                                                    data["MCarrierType"] = charA[30].ToString().Trim();
//                                                    data["MCarrierId1"] = charA[31].ToString().Trim();
//                                                    data["MCarrierId2"] = charA[32].ToString().Trim();
//                                                    data["MPrintMark"] = charA[33].ToString().Trim();
//                                                    data["MNPOBAN"] = charA[34].ToString().Trim();
//                                                    data["MRandomNumber"] = charA[35].ToString().Trim();

//                                                    data["ASalesAmount"] = charA[36].ToString().Trim();
//                                                    data["AFreeTaxSalesAmount"] = charA[37].ToString().Trim();
//                                                    data["AZeroTaxSalesAmount"] = charA[38].ToString().Trim();
//                                                    data["ATaxType"] = charA[39].ToString().Trim();
//                                                    data["ATaxRate"] = charA[40].ToString().Trim();
//                                                    data["ATaxAmount"] = charA[41].ToString().Trim();
//                                                    data["ATotalAmount"] = charA[42].ToString().Trim();
//                                                    data["ADiscountAmount"] = charA[43].ToString().Trim();

//                                                    data["AOriginalCurrencyAmount"] = charA[44].ToString().Trim();
//                                                    data["AExchangeRate"] = charA[45].ToString().Trim();
//                                                    data["ACurrency"] = charA[46].ToString().Trim();

//                                                    data["TxFileNmae"] = sourceFile.ToString().Trim();
//                                                    query.InsertDataNonKey("A0401H", data);
//                                                    data = null;
//                                                    isSuccess = true;
//                                                    System.Threading.Thread.Sleep(100);
//                                                }
//                                                else
//                                                { query.GoLogsAll(sPgSN, ProcessName, sourceFile, "[發票號碼:" + charA[0].ToString() + "][ADiscountAmount要>=0且不能有小數]", (counter + 1).ToString(), 11); }
//                                            }
//                                            else
//                                            { query.GoLogsAll(sPgSN, ProcessName, sourceFile, "[發票號碼:" + charA[0].ToString() + "][ATotalAmount要>=0且不能有小數]", (counter + 1).ToString(), 11); }
//                                        }
//                                        else
//                                        { query.GoLogsAll(sPgSN, ProcessName, sourceFile, "[發票號碼:" + charA[0].ToString() + "][ATaxAmount要>=0且不能有小數]", (counter + 1).ToString(), 11); }
//                                    }
//                                    else
//                                    { query.GoLogsAll(sPgSN, ProcessName, sourceFile, "[發票號碼:" + charA[0].ToString() + "][AZeroTaxSalesAmount要>=0且不能有小數]", (counter + 1).ToString(), 11); }
//                                }
//                                else
//                                { query.GoLogsAll(sPgSN, ProcessName, sourceFile, "[發票號碼:" + charA[0].ToString() + "][AFreeTaxSalesAmount要>=0且不能有小數]", (counter + 1).ToString(), 11); }
//                            }
//                            else
//                            { query.GoLogsAll(sPgSN, ProcessName, sourceFile, "[發票號碼:" + charA[0].ToString() + "][ASalesAmount要>=0且不能有小數]", (counter + 1).ToString(), 11); }
//                        }
//                        else
//                        { query.GoLogsAll(sPgSN, ProcessName, sourceFile, "[發票號碼:" + charA[0].ToString() + "][買方營業人名稱須<>0000]", (counter + 1).ToString(), 11); }
//                    }
//                    else
//                    { query.GoLogsAll(sPgSN, ProcessName, sourceFile, "[發票號碼:" + charA[0].ToString() + "][買方營業人統一編號須<>0000000000]", (counter + 1).ToString(), 11); }
//                }
//                else
//                {
//                    isSuccess = false;
//                    query.GoLogsAll(sPgSN, ProcessName, sourceFile, "[發票號碼:" + charA[0].ToString() + "][此發票號碼已存在!!", (counter + 1).ToString(), 11);
//                }
//                //}
//                //catch (Exception ex)
//                //{
//                //    query.GoLogsAll(sPgSN,ProcessName, OkFName, "[發票號碼:" + charA[0].ToString() + "][字串有例外錯誤!!]" + ex.ToString(), (counter + 1).ToString(), 11);
//                //}
//            }
//        }
//        else
//        {
//            using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
//            {
//                query.GoLogsAll(sPgSN, ProcessName, sourceFile, "", (counter + 1).ToString(), 12);
//            }
//        }

//        Console.WriteLine("間隔數:" + charA.Length.ToString());

//        counter++;
//    }
//}