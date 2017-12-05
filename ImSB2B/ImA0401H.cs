using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class ImA0401H
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

        //Console.ReadLine();
    }

    //private int chkint(string chkString)
    //{
    //    int n;
    //    if (int.TryParse(chkString, out n))
    //    {
    //        MessageBox.Show("數字");
    //    }
    //    else
    //    {
    //        MessageBox.Show("非數字");
    //    }
    //}

}

