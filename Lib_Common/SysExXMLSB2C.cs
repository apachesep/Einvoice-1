using System;
using System.Data;
using System.IO;

namespace NSysDB
{
    namespace NTSQL
    {
        /// <summary>
        /// </summary>
        public partial class SQL1
        {
            //存證B2C-----S
            public bool ExXmlC0401(string C0401SN, string MInvoiceNumber, string sKind0up, string sKind0upAll, string sPgSN)
            {
                //要有Details才生成XML
                DataView dvResultD = Kind1SelectTbl2("", "C0401D", "MInvoiceNumber='" + MInvoiceNumber + "'", "DSequenceNumber", "");
                Console.WriteLine(MInvoiceNumber);
                if (dvResultD != null)
                {
                    NSysDB.XMLClass oXMLeParamts = new NSysDB.XMLClass();
                    string sFPathN = oXMLeParamts.GetParaXml("ExXMLPa");

                    DataView dvResultM = Kind1SelectTbl2("", sKind0upAll, "C0401SN='" + C0401SN + "'", "", "");
                    bool validInvoiceDate = ValidIsDate(Convert.ToString(dvResultM.Table.Rows[0]["MInvoiceDate"]));
                    if (!validInvoiceDate)
                    {
                        GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, sKind0up, "發票日期過早，不上傳至turnkey，日期：" + Convert.ToString(dvResultM.Table.Rows[0]["MInvoiceDate"]), MInvoiceNumber, 23);
                        return false;
                    }
                    string strXMLAll = "", strXMLM = "", strXMLD = "", strXMLA = "";
                    strXMLM = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine
                          //+ "<Invoice xmlns=\"urn:GEINV:eInvoiceMessage:C0401:3.1\"/>" + Environment.NewLine
                          //+ "<Invoice xmlns=\"urn:GEINV:eInvoiceMessage:C0401:3.1\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" targetNamespace=\"urn:GEINV:eInvoiceMessage:C0401:3.1\" elementFormDefault=\"qualified\" attributeFormDefault=\"unqualified\">" + Environment.NewLine
                          + "<Invoice xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"urn:GEINV:eInvoiceMessage:C0401:3.1\" xsi:schemaLocation=\"urn:GEINV:eInvoiceMessage:C0401:3.1 C0401.xsd\" >" + Environment.NewLine
                          //+ "<!-- 開立發票 C0401-->" + Environment.NewLine
                          + "<Main>" + Environment.NewLine
                          //+ "<!-- 發票號碼 -->" + Environment.NewLine
                          + "<InvoiceNumber>" + Convert.ToString(dvResultM.Table.Rows[0]["MInvoiceNumber"]) + "</InvoiceNumber>" + Environment.NewLine
                          //+ "<!-- 發票日期 -->" + Environment.NewLine
                          + "<InvoiceDate>" + Convert.ToString(dvResultM.Table.Rows[0]["MInvoiceDate"]) + "</InvoiceDate>" + Environment.NewLine
                          //+ "<!-- 發票時間 -->" + Environment.NewLine
                          + "<InvoiceTime>" + Convert.ToString(dvResultM.Table.Rows[0]["MInvoiceTime"]) + "</InvoiceTime>" + Environment.NewLine
                          //+ "<!-- 賣方 -->" + Environment.NewLine
                          + "<Seller>" + Environment.NewLine
                          //+ "<!-- 賣方 營業人統一編號 -->" + Environment.NewLine
                          + "<Identifier>" + Convert.ToString(dvResultM.Table.Rows[0]["MSIdentifier"]) + "</Identifier>" + Environment.NewLine
                          //+ "<!-- 賣方 營業人名稱 -->" + Environment.NewLine
                          + "<Name>" + Convert.ToString(dvResultM.Table.Rows[0]["MSName"]) + "</Name>" + Environment.NewLine
                          //+ "<!-- 賣方 營業人地址 -->" + Environment.NewLine 20171116 append by 俊晨
                          + "<Address>" + Convert.ToString(dvResultM.Table.Rows[0]["MSAddress"]) + "</Address>" + Environment.NewLine
                          //+ "<!-- 賣方 營業人負責人姓名 -->" + Environment.NewLine 20171116 append by 俊晨
                          + "<PersonInCharge>" + Convert.ToString(dvResultM.Table.Rows[0]["MSPersonInCharge"]) + "</PersonInCharge>" + Environment.NewLine

                    #region 因稅籍檔並未有電話與傳真 自我檢測時至將會無法通過 先註解 需要再解除註解 20171116 append by 俊晨

                          /*
                          //+ "<!-- 賣方 營業人電話 -->" + Environment.NewLine 20171116 append by 俊晨
                          + "<TelephoneNumber>" + Convert.ToString(dvResultM.Table.Rows[0]["MSTelephoneNumber"]) + "</TelephoneNumber>" + Environment.NewLine
                          //+ "<!-- 賣方 營業人傳真 -->" + Environment.NewLine 20171116 append by 俊晨
                          + "<FacsimileNumber>" + Convert.ToString(dvResultM.Table.Rows[0]["MSFacsimileNumber"]) + "</FacsimileNumber>" + Environment.NewLine
                          */

                    #endregion 因稅籍檔並未有電話與傳真 自我檢測時至將會無法通過 先註解 需要再解除註解 20171116 append by 俊晨

                          + "</Seller>" + Environment.NewLine
                          //+ "<!-- 買方 -->" + Environment.NewLine
                          + "<Buyer>" + Environment.NewLine
                          //+ "<!-- 買方-營業人統一編號  B2C買方則填入10個0 -->" + Environment.NewLine 20171116 append by 俊晨 append實際統編，非10個0
                          + "<Identifier>" + Convert.ToString(dvResultM.Table.Rows[0]["MBIdentifier"]) + "</Identifier>" + Environment.NewLine
                          //+ "<!-- 買方-營業人名稱  B2C買方則填入4個0 -->" + Environment.NewLine 20171116 append by 俊晨 append實際營業人名稱，非4個0
                          + "<Name>" + Convert.ToString(dvResultM.Table.Rows[0]["MBName"]) + "</Name>" + Environment.NewLine
                          + "</Buyer>" + Environment.NewLine
                          //+ "<!-- 發票類別 -->" + Environment.NewLine
                          + "<InvoiceType>" + Convert.ToString(dvResultM.Table.Rows[0]["MInvoiceType"]) + "</InvoiceType>" + Environment.NewLine
                          //+ "<!-- 捐贈註記 -->" + Environment.NewLine
                          + "<DonateMark><![CDATA[" + Convert.ToString(dvResultM.Table.Rows[0]["MDonateMark"]) + "]]></DonateMark>" + Environment.NewLine
                          //+ "<!-- 載具類別號碼 -->" + Environment.NewLine 20171116 append by 俊晨
                          + "<CarrierType>" + Convert.ToString(dvResultM.Table.Rows[0]["MCarrierType"]) + "</CarrierType>" + Environment.NewLine
                          //+ "<!-- 載具顯碼id -->" + Environment.NewLine 20171116 append by 俊晨
                          + "<CarrierId1>" + Convert.ToString(dvResultM.Table.Rows[0]["MCarrierId1"]) + "</CarrierId1>" + Environment.NewLine
                          //+ "<!-- 載具隱碼id -->" + Environment.NewLine 20171116 append by 俊晨
                          + "<CarrierId2>" + Convert.ToString(dvResultM.Table.Rows[0]["MCarrierId2"]) + "</CarrierId2>" + Environment.NewLine
                          //+ "<!-- 發票捐贈對象 -->" + Environment.NewLine 20171116 append by 俊晨
                          + "<NPOBAN>" + Convert.ToString(dvResultM.Table.Rows[0]["MNPOBAN"]) + "</NPOBAN>" + Environment.NewLine
                          //+ "<!-- 發票檢查碼 -->" + Environment.NewLine 20171116 append by 俊晨
                          + "<CheckNumber>" + Convert.ToString(dvResultM.Table.Rows[0]["MCheckNumber"]) + "</CheckNumber>" + Environment.NewLine
                          //+ "<!-- BC紙本電子發票已列印註記 -->" + Environment.NewLine
                          + "<PrintMark>" + Convert.ToString(dvResultM.Table.Rows[0]["MPrintMark"]) + "</PrintMark>" + Environment.NewLine

                          //+ "<!-- BC發票防偽隨機碼 -->" + Environment.NewLine
                          + "<RandomNumber>" + Convert.ToString(dvResultM.Table.Rows[0]["MRandomNumber"]) + "</RandomNumber>" + Environment.NewLine
                          + "</Main>" + Environment.NewLine
                          + "<Details>" + Environment.NewLine;
                    int sumAmount = 0;
                    for (int i = 0; i < dvResultD.Count; i++)
                    {
                        strXMLD += "<ProductItem>" + Environment.NewLine
                            //+ "<!-- 品名 -->" + Environment.NewLine
                            + "<Description>" + Convert.ToString(dvResultD.Table.Rows[i]["DDescription"]) + "</Description>" + Environment.NewLine
                            //+ "<!-- 數量 -->" + Environment.NewLine
                            + "<Quantity>" + Convert.ToString(dvResultD.Table.Rows[i]["DQuantity"]) + "</Quantity>" + Environment.NewLine
                            //+ "<!-- 單價 -->" + Environment.NewLine
                            + "<UnitPrice>" + Convert.ToString(dvResultD.Table.Rows[i]["DUnitPrice"]) + "</UnitPrice>" + Environment.NewLine
                            //+ "<!-- 金額 -->" + Environment.NewLine
                            + "<Amount>" + Convert.ToString(dvResultD.Table.Rows[i]["DAmount"]) + "</Amount>" + Environment.NewLine
                            //+ "<!-- 明細排列序號 -->" + Environment.NewLine
                            + "<SequenceNumber>" + Convert.ToString(dvResultD.Table.Rows[i]["DSequenceNumber"]) + "</SequenceNumber>" + Environment.NewLine
                            + "</ProductItem>" + Environment.NewLine;
                        sumAmount += Convert.ToInt32(dvResultD.Table.Rows[i]["DAmount"]);
                    }

                    #region 判斷金額總計是否相符

                    int totalAmount = (Convert.ToInt32(dvResultM.Table.Rows[0]["ATotalAmount"]));
                    if ((sumAmount + Convert.ToInt32(dvResultM.Table.Rows[0]["ATaxAmount"])) != totalAmount)
                        throw new Exception(
                            string.Format(
                                @"金額總計與明細不相符，停止XML生成作業<br/>
                                {0} {1}：

                                明細金額：{2} <br/>
                                稅額：{3} <br/>
                                加總金額： {4}",
                            sKind0up, Convert.ToString(dvResultM.Table.Rows[0]["MInvoiceNumber"]), sumAmount, Convert.ToInt32(dvResultM.Table.Rows[0]["ATaxAmount"]), totalAmount));

                    #endregion 判斷金額總計是否相符

                    if (string.IsNullOrEmpty(Convert.ToString(dvResultM.Table.Rows[0]["ATaxType"])))
                        throw new Exception(sKind0up + "：" + Convert.ToString(dvResultM.Table.Rows[0]["MInvoiceNumber"]) + "發票稅別為空值，停止XML生成作業。");

                    strXMLA = "</Details>" + Environment.NewLine
                   + "<Amount>" + Environment.NewLine
                   //+ "<!-- 應稅銷售額合計(含稅新台幣)B2B:未稅  B2C:內含稅 -->" + Environment.NewLine
                   + "<SalesAmount>" + Convert.ToString(dvResultM.Table.Rows[0]["ASalesAmount"]) + "</SalesAmount>" + Environment.NewLine
                   //+ "<!-- 免銷售額合計(新台幣) -->" + Environment.NewLine
                   + "<FreeTaxSalesAmount>" + Convert.ToString(dvResultM.Table.Rows[0]["AFreeTaxSalesAmount"]) + "</FreeTaxSalesAmount>" + Environment.NewLine
                   //+ "<!-- 零銷售額合計(新台幣) -->" + Environment.NewLine
                   + "<ZeroTaxSalesAmount>" + Convert.ToString(dvResultM.Table.Rows[0]["AZeroTaxSalesAmount"]) + "</ZeroTaxSalesAmount>" + Environment.NewLine
                   //+ "<!-- 課稅別 -->" + Environment.NewLine
                   + "<TaxType>" + Convert.ToString(dvResultM.Table.Rows[0]["ATaxType"]) + "</TaxType>" + Environment.NewLine
                   //+ "<!-- 稅率 -->" + Environment.NewLine
                   + "<TaxRate>" + Convert.ToString(dvResultM.Table.Rows[0]["ATaxRate"]) + "</TaxRate>" + Environment.NewLine
                   //+ "<!-- 營業稅 -->" + Environment.NewLine
                   + "<TaxAmount>" + Convert.ToString(dvResultM.Table.Rows[0]["ATaxAmount"]) + "</TaxAmount>" + Environment.NewLine
                   //+ "<!-- 總計 -->" + Environment.NewLine
                   + "<TotalAmount>" + Convert.ToString(dvResultM.Table.Rows[0]["ATotalAmount"]) + "</TotalAmount>" + Environment.NewLine
                   //+ "<!-- 扣抵金額 -->" + Environment.NewLine
                   + "<DiscountAmount>" + Convert.ToString(dvResultM.Table.Rows[0]["ADiscountAmount"]) + "</DiscountAmount>" + Environment.NewLine
                   //+ "<!-- 原幣金額 -->" + Environment.NewLine
                   + "<OriginalCurrencyAmount>" + Convert.ToString(dvResultM.Table.Rows[0]["AOriginalCurrencyAmount"]) + "</OriginalCurrencyAmount>" + Environment.NewLine
                   //+ "<!-- 匯率 -->" + Environment.NewLine
                   + "<ExchangeRate>" + Convert.ToString(dvResultM.Table.Rows[0]["AExchangeRate"]) + "</ExchangeRate>" + Environment.NewLine
                   //+ "<!-- 幣別 -->" + Environment.NewLine
                   + "<Currency>" + Convert.ToString(dvResultM.Table.Rows[0]["ACurrency"]) + "</Currency>" + Environment.NewLine
                   + "</Amount>" + Environment.NewLine
                   + "</Invoice>";

                    strXMLAll = strXMLM + strXMLD + strXMLA;
                    //Console.WriteLine(strXMLAll);
                    StreamWriter sw = new StreamWriter(sFPathN + sKind0up + "_" + MInvoiceNumber + ".XML");
                    sw.Write(strXMLAll);
                    sw.Close();

                    return true;
                }
                else
                {
                    GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, sKind0up, "", MInvoiceNumber, 23);
                    return false;
                }
            }

            public bool ExXmlC0501(string ASN, string MInvoiceNumber, string sKind0up, string sKind0upAll, string sPgSN)
            {
                NSysDB.XMLClass oXMLeParamts = new NSysDB.XMLClass();
                string sFPathN = oXMLeParamts.GetParaXml("ExXMLPa");

                DataView dvResultM = Kind1SelectTbl2("", sKind0upAll, "C0501SN='" + ASN + "'", "", "");
                bool validInvoiceDate = ValidIsDate(Convert.ToString(dvResultM.Table.Rows[0]["InvoiceDate"]));
                if (!validInvoiceDate)
                {
                    GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, sKind0up, "原發票日期過早，不上傳至turnkey，日期：" + Convert.ToString(dvResultM.Table.Rows[0]["InvoiceDate"]), MInvoiceNumber, 23);
                    return false;
                }

                bool validInvoiceCancelDate = ValidIsDate(Convert.ToString(dvResultM.Table.Rows[0]["CancelDate"]));
                if (!validInvoiceCancelDate)
                {
                    GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, sKind0up, "發票作廢日期過早，不上傳至turnkey，日期：" + Convert.ToString(dvResultM.Table.Rows[0]["CancelDate"]), MInvoiceNumber, 23);
                    return false;
                }

                if (dvResultM != null)
                {
                    string strXMLAll = "";
                    strXMLAll = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine
                          //+ "<Invoice xmlns=\"urn:GEINV:eInvoiceMessage:C0401:3.1\"/>" + Environment.NewLine
                          //+ "<Invoice xmlns=\"urn:GEINV:eInvoiceMessage:A0501:3.1\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" targetNamespace=\"urn:GEINV:eInvoiceMessage:A0501:3.1\" elementFormDefault=\"qualified\" attributeFormDefault=\"unqualified\">" + Environment.NewLine
                          + "<CancelInvoice xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"urn:GEINV:eInvoiceMessage:C0501:3.1\" xsi:schemaLocation=\"urn:GEINV:eInvoiceMessage:C0501:3.1 C0501.xsd\" >" + Environment.NewLine
                          //+ "<!-- 作廢發票號碼 C0501-->" + Environment.NewLine
                          + "<CancelInvoiceNumber>" + Convert.ToString(dvResultM.Table.Rows[0]["CancelInvoiceNumber"]) + "</CancelInvoiceNumber>" + Environment.NewLine
                          //+ "<!-- 發票日期 -->" + Environment.NewLine
                          + "<InvoiceDate>" + Convert.ToString(dvResultM.Table.Rows[0]["InvoiceDate"]) + "</InvoiceDate>" + Environment.NewLine
                          //+ "<!-- 買方統一編號 -->" + Environment.NewLine
                          + "<BuyerId>" + Convert.ToString(dvResultM.Table.Rows[0]["BuyerId"]) + "</BuyerId>" + Environment.NewLine
                          //+ "<!-- 賣方統一編號 -->" + Environment.NewLine
                          + "<SellerId>" + Convert.ToString(dvResultM.Table.Rows[0]["SellerId"]) + "</SellerId>" + Environment.NewLine
                          //+ "<!-- 發票作廢日期 -->" + Environment.NewLine
                          + "<CancelDate>" + Convert.ToString(dvResultM.Table.Rows[0]["CancelDate"]) + "</CancelDate>" + Environment.NewLine
                          //+ "<!-- 發票作廢時間 -->" + Environment.NewLine
                          + "<CancelTime>" + Convert.ToString(dvResultM.Table.Rows[0]["CancelTime"]) + "</CancelTime>" + Environment.NewLine
                          //+ "<!-- 發票作廢原因 -->" + Environment.NewLine
                          + "<CancelReason><![CDATA[" + Convert.ToString(dvResultM.Table.Rows[0]["CancelReason"]) + "]]></CancelReason>" + Environment.NewLine
                          //+ "<!-- 專案作廢核准文號 -->" + Environment.NewLine
                          + "<ReturnTaxDocumentNumber>" + Convert.ToString(dvResultM.Table.Rows[0]["ReturnTaxDocumentNumber"]) + "</ReturnTaxDocumentNumber>" + Environment.NewLine
                          + "</CancelInvoice>" + Environment.NewLine;

                    StreamWriter sw = new StreamWriter(sFPathN + sKind0up + "_" + MInvoiceNumber + ".XML");
                    sw.Write(strXMLAll);
                    sw.Close();

                    return true;
                }
                else
                {
                    return false;
                }
            }

            public bool ExXmlC0701(string ASN, string MInvoiceNumber, string sKind0up, string sKind0upAll, string sPgSN)
            {
                NSysDB.XMLClass oXMLeParamts = new NSysDB.XMLClass();
                string sFPathN = oXMLeParamts.GetParaXml("ExXMLPa");

                DataView dvResultM = Kind1SelectTbl2("", sKind0upAll, "C0701SN='" + ASN + "'", "", "");
                bool validInvoiceDate = ValidIsDate(Convert.ToString(dvResultM.Table.Rows[0]["InvoiceDate"]));
                if (!validInvoiceDate)
                {
                    GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, sKind0up, "原發票日期過早，不上傳至turnkey，日期：" + Convert.ToString(dvResultM.Table.Rows[0]["InvoiceDate"]), MInvoiceNumber, 23);
                    return false;
                }
                bool validInvoiceCancelDate = ValidIsDate(Convert.ToString(dvResultM.Table.Rows[0]["VoidDate"]));
                if (!validInvoiceCancelDate)
                {
                    GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, sKind0up, "註銷日期過早，不上傳至turnkey，日期：" + Convert.ToString(dvResultM.Table.Rows[0]["VoidDate"]), MInvoiceNumber, 23);
                    return false;
                }

                if (dvResultM != null)
                {
                    string strXMLAll = "";
                    strXMLAll = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine
                          //+ "<Invoice xmlns=\"urn:GEINV:eInvoiceMessage:C0401:3.1\"/>" + Environment.NewLine
                          //+ "<Invoice xmlns=\"urn:GEINV:eInvoiceMessage:A0601:3.1\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" targetNamespace=\"urn:GEINV:eInvoiceMessage:A0601:3.1\" elementFormDefault=\"qualified\" attributeFormDefault=\"unqualified\">" + Environment.NewLine
                          + "<VoidInvoice xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"urn:GEINV:eInvoiceMessage:C0701:3.1\" xsi:schemaLocation=\"urn:GEINV:eInvoiceMessage:C0701:3.1 C0701.xsd\" >" + Environment.NewLine
                          //+ "<!-- 註銷發票號碼 -->" + Environment.NewLine
                          + "<VoidInvoiceNumber>" + Convert.ToString(dvResultM.Table.Rows[0]["VoidInvoiceNumber"]) + "</VoidInvoiceNumber>" + Environment.NewLine
                          //+ "<!-- 發票開立日期 -->" + Environment.NewLine
                          + "<InvoiceDate>" + Convert.ToString(dvResultM.Table.Rows[0]["InvoiceDate"]) + "</InvoiceDate>" + Environment.NewLine
                          //+ "<!-- 買方統一編號 -->" + Environment.NewLine
                          + "<BuyerId>" + Convert.ToString(dvResultM.Table.Rows[0]["BuyerId"]) + "</BuyerId>" + Environment.NewLine
                          //+ "<!-- 賣方統一編號 -->" + Environment.NewLine
                          + "<SellerId>" + Convert.ToString(dvResultM.Table.Rows[0]["SellerId"]) + "</SellerId>" + Environment.NewLine
                          //+ "<!-- 註銷日期 -->" + Environment.NewLine
                          + "<VoidDate>" + Convert.ToString(dvResultM.Table.Rows[0]["VoidDate"]) + "</VoidDate>" + Environment.NewLine
                          //+ "<!-- 註銷時間 -->" + Environment.NewLine
                          + "<VoidTime>" + Convert.ToString(dvResultM.Table.Rows[0]["VoidTime"]) + "</VoidTime>" + Environment.NewLine
                          //+ "<!-- 註銷原因 -->" + Environment.NewLine
                          + "<VoidReason><![CDATA[" + Convert.ToString(dvResultM.Table.Rows[0]["VoidReason"]) + "]]></VoidReason>" + Environment.NewLine

                          + "</VoidInvoice>" + Environment.NewLine;

                    StreamWriter sw = new StreamWriter(sFPathN + sKind0up + "_" + MInvoiceNumber + ".XML");
                    sw.Write(strXMLAll);
                    sw.Close();

                    return true;
                }
                else
                {
                    return false;
                }
            }

            public bool ExXmlD0401(string D0401SN, string MInvoiceNumber, string sKind0up, string sKind0upAll, string sPgSN)
            {
                //要有Details才生成XML
                DataView dvResultD = Kind1SelectTbl2("", "D0401D", "MAllowanceNumber='" + MInvoiceNumber + "'", "DAllowanceSequenceNumber", "");


                Console.WriteLine(MInvoiceNumber);
                if (dvResultD != null)
                {
                    NSysDB.XMLClass oXMLeParamts = new NSysDB.XMLClass();
                    string sFPathN = oXMLeParamts.GetParaXml("ExXMLPa");

                    DataView dvResultM = Kind1SelectTbl2("", sKind0upAll, "D0401SN='" + D0401SN + "'", "", "");
                    bool validInvoiceDate = ValidIsDate(Convert.ToString(dvResultM.Table.Rows[0]["MAllowanceDate"]));
                    if (!validInvoiceDate)
                    {
                        GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, sKind0up, "折讓證明單日期過早，不上傳至turnkey，日期：" + Convert.ToString(dvResultM.Table.Rows[0]["MAllowanceDate"]), MInvoiceNumber, 23);
                        return false;
                    }
                    string strXMLAll = "", strXMLM = "", strXMLD = "", strXMLA = "";
                    strXMLM = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine
                          //+ "<Invoice xmlns=\"urn:GEINV:eInvoiceMessage:C0401:3.1\"/>" + Environment.NewLine
                          //+ "<Invoice xmlns=\"urn:GEINV:eInvoiceMessage:B0401:3.1\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" targetNamespace=\"urn:GEINV:eInvoiceMessage:B0401:3.1\" elementFormDefault=\"qualified\" attributeFormDefault=\"unqualified\">" + Environment.NewLine
                          + "<Allowance xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"urn:GEINV:eInvoiceMessage:D0401:3.1\" xsi:schemaLocation=\"urn:GEINV:eInvoiceMessage:D0401:3.1 D0401.xsd\" >" + Environment.NewLine
                          //+ "<!-- 開立折讓證明單/傳送折讓證明單 D0401-->" + Environment.NewLine
                          + "<Main>" + Environment.NewLine
                          //+ "<!-- 折讓證明單號碼 -->" + Environment.NewLine
                          + "<AllowanceNumber>" + Convert.ToString(dvResultM.Table.Rows[0]["MAllowanceNumber"]) + "</AllowanceNumber>" + Environment.NewLine
                          //+ "<!-- 折讓證明單日期 -->" + Environment.NewLine
                          + "<AllowanceDate>" + Convert.ToString(dvResultM.Table.Rows[0]["MAllowanceDate"]) + "</AllowanceDate>" + Environment.NewLine
                          //+ "<!-- 賣方 -->" + Environment.NewLine
                          + "<Seller>" + Environment.NewLine
                          //+ "<!-- 賣方 營業人統一編號 -->" + Environment.NewLine
                          + "<Identifier>" + Convert.ToString(dvResultM.Table.Rows[0]["MSIdentifier"]) + "</Identifier>" + Environment.NewLine
                          //+ "<!-- 賣方 營業人名稱 -->" + Environment.NewLine
                          + "<Name>" + Convert.ToString(dvResultM.Table.Rows[0]["MSName"]) + "</Name>" + Environment.NewLine
                          + "</Seller>" + Environment.NewLine
                          //+ "<!-- 買方 -->" + Environment.NewLine
                          + "<Buyer>" + Environment.NewLine
                          //+ "<!-- B2B 買方-營業人統一編號  B2C買方則填入10個0 -->" + Environment.NewLine
                          + "<Identifier>" + Convert.ToString(dvResultM.Table.Rows[0]["MBIdentifier"]) + "</Identifier>" + Environment.NewLine
                          //+ "<!-- B2B 買方-營業人名稱  B2C買方則填入4個0 -->" + Environment.NewLine
                          + "<Name>" + Convert.ToString(dvResultM.Table.Rows[0]["MBName"]) + "</Name>" + Environment.NewLine
                          + "</Buyer>" + Environment.NewLine
                          //+ "<!-- 折讓種類 -->" + Environment.NewLine
                          + "<AllowanceType>" + Convert.ToString(dvResultM.Table.Rows[0]["MAllowanceType"]) + "</AllowanceType>" + Environment.NewLine
                          + "</Main>" + Environment.NewLine
                          + "<Details>" + Environment.NewLine;

                    for (int i = 0; i < dvResultD.Count; i++)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(dvResultD.Table.Rows[i]["DTaxType"])))
                            throw new Exception(sKind0up + "：" + Convert.ToString(dvResultM.Table.Rows[0]["MAllowanceNumber"]) + "發票稅別為空值，停止XML生成作業。");

                        strXMLD += "<ProductItem>" + Environment.NewLine
                            //+ "<!-- 原發票日期 -->" + Environment.NewLine
                            + "<OriginalInvoiceDate>" + Convert.ToString(dvResultD.Table.Rows[i]["DOriginalInvoiceDate"]) + "</OriginalInvoiceDate>" + Environment.NewLine
                            //+ "<!-- 原發票號碼 -->" + Environment.NewLine
                            + "<OriginalInvoiceNumber>" + Convert.ToString(dvResultD.Table.Rows[i]["DOriginalInvoiceNumber"]) + "</OriginalInvoiceNumber>" + Environment.NewLine
                            //+ "<!-- 原品名 -->" + Environment.NewLine
                            + "<OriginalDescription>" + Convert.ToString(dvResultD.Table.Rows[i]["DOriginalDescription"]) + "</OriginalDescription>" + Environment.NewLine
                            //+ "<!-- 數量 -->" + Environment.NewLine
                            + "<Quantity>" + Convert.ToString(dvResultD.Table.Rows[i]["DQuantity"]) + "</Quantity>" + Environment.NewLine
                            //+ "<!-- 單價 -->" + Environment.NewLine
                            + "<UnitPrice>" + Convert.ToString(dvResultD.Table.Rows[i]["DUnitPrice"]) + "</UnitPrice>" + Environment.NewLine
                            //+ "<!-- 金額(不含稅之進貨額) -->" + Environment.NewLine
                            + "<Amount>" + Convert.ToString(dvResultD.Table.Rows[i]["DAmount"]) + "</Amount>" + Environment.NewLine
                            //+ "<!-- 營業稅額 -->" + Environment.NewLine
                            + "<Tax>" + Convert.ToString(dvResultD.Table.Rows[i]["DTax"]) + "</Tax>" + Environment.NewLine
                            //+ "<!-- 折讓證明單明細排列序號 -->" + Environment.NewLine
                            + "<AllowanceSequenceNumber>" + Convert.ToString(dvResultD.Table.Rows[i]["DAllowanceSequenceNumber"]) + "</AllowanceSequenceNumber>" + Environment.NewLine
                            //+ "<!-- 課稅別 -->" + Environment.NewLine
                            + "<TaxType>" + Convert.ToString(dvResultD.Table.Rows[i]["DTaxType"]) + "</TaxType>" + Environment.NewLine
                            + "</ProductItem>" + Environment.NewLine;
                    }

                    strXMLA = "</Details>" + Environment.NewLine
                   + "<Amount>" + Environment.NewLine
                   //+ "<!-- 營業稅額合計 -->" + Environment.NewLine
                   + "<TaxAmount>" + Convert.ToString(dvResultM.Table.Rows[0]["ATaxAmount"]) + "</TaxAmount>" + Environment.NewLine
                   //+ "<!-- 金額(不含稅之進貨額)合計 -->" + Environment.NewLine
                   + "<TotalAmount>" + Convert.ToString(dvResultM.Table.Rows[0]["ATotalAmount"]) + "</TotalAmount>" + Environment.NewLine
                   + "</Amount>" + Environment.NewLine
                   + "</Allowance>";

                    strXMLAll = strXMLM + strXMLD + strXMLA;
                    //Console.WriteLine(strXMLAll);
                    StreamWriter sw = new StreamWriter(sFPathN + sKind0up + "_" + MInvoiceNumber + ".XML");
                    sw.Write(strXMLAll);
                    sw.Close();

                    return true;
                }
                else
                {
                    GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, sKind0up, "", MInvoiceNumber, 23);
                    return false;
                }
            }

            public bool ExXmlD0501(string ASN, string MInvoiceNumber, string sKind0up, string sKind0upAll, string sPgSN)
            {
                NSysDB.XMLClass oXMLeParamts = new NSysDB.XMLClass();
                string sFPathN = oXMLeParamts.GetParaXml("ExXMLPa");

                DataView dvResultM = Kind1SelectTbl2("", sKind0upAll, "D0501SN='" + ASN + "'", "", "");
                bool validInvoiceDate = ValidIsDate(Convert.ToString(dvResultM.Table.Rows[0]["AllowanceDate"]));
                if (!validInvoiceDate)
                {
                    GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, sKind0up, "原折讓證明單日期過早，不上傳至turnkey，日期：" + Convert.ToString(dvResultM.Table.Rows[0]["AllowanceDate"]), MInvoiceNumber, 23);
                    return false;
                }
                bool validInvoiceCancelDate = ValidIsDate(Convert.ToString(dvResultM.Table.Rows[0]["CancelDate"]));
                if (!validInvoiceCancelDate)
                {
                    GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, sKind0up, "折讓證明單作廢日期過早，不上傳至turnkey，日期：" + Convert.ToString(dvResultM.Table.Rows[0]["CancelDate"]), MInvoiceNumber, 23);
                    return false;
                }

                if (dvResultM != null)
                {
                    string strXMLAll = "";
                    strXMLAll = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine
                          //+ "<Invoice xmlns=\"urn:GEINV:eInvoiceMessage:C0401:3.1\"/>" + Environment.NewLine
                          //+ "<Invoice xmlns=\"urn:GEINV:eInvoiceMessage:A0501:3.1\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" targetNamespace=\"urn:GEINV:eInvoiceMessage:A0501:3.1\" elementFormDefault=\"qualified\" attributeFormDefault=\"unqualified\">" + Environment.NewLine
                          + "<CancelAllowance xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"urn:GEINV:eInvoiceMessage:D0501:3.1\" xsi:schemaLocation=\"urn:GEINV:eInvoiceMessage:D0501:3.1 D0501.xsd\" >" + Environment.NewLine
                          //+ "<!-- 作廢折讓證明單號碼 D0501-->" + Environment.NewLine
                          + "<CancelAllowanceNumber>" + Convert.ToString(dvResultM.Table.Rows[0]["CancelAllowanceNumber"]) + "</CancelAllowanceNumber>" + Environment.NewLine
                          //+ "<!-- 折讓證明單日期 -->" + Environment.NewLine
                          + "<AllowanceDate>" + Convert.ToString(dvResultM.Table.Rows[0]["AllowanceDate"]) + "</AllowanceDate>" + Environment.NewLine
                          //+ "<!-- 買方統一編號 -->" + Environment.NewLine
                          + "<BuyerId>" + Convert.ToString(dvResultM.Table.Rows[0]["BuyerId"]) + "</BuyerId>" + Environment.NewLine
                          //+ "<!-- 賣方統一編號 -->" + Environment.NewLine
                          + "<SellerId>" + Convert.ToString(dvResultM.Table.Rows[0]["SellerId"]) + "</SellerId>" + Environment.NewLine
                          //+ "<!-- 折讓證明單作廢日期 -->" + Environment.NewLine
                          + "<CancelDate>" + Convert.ToString(dvResultM.Table.Rows[0]["CancelDate"]) + "</CancelDate>" + Environment.NewLine
                          //+ "<!-- 折讓證明單作廢時間 -->" + Environment.NewLine
                          + "<CancelTime>" + Convert.ToString(dvResultM.Table.Rows[0]["CancelTime"]) + "</CancelTime>" + Environment.NewLine
                          //+ "<!-- 折讓證明單作廢原因 -->" + Environment.NewLine
                          + "<CancelReason><![CDATA[" + Convert.ToString(dvResultM.Table.Rows[0]["CancelReason"]) + "]]></CancelReason>" + Environment.NewLine

                          + "</CancelAllowance>" + Environment.NewLine;

                    StreamWriter sw = new StreamWriter(sFPathN + sKind0up + "_" + MInvoiceNumber + ".XML");
                    sw.Write(strXMLAll);
                    sw.Close();

                    return true;
                }
                else
                {
                    return false;
                }
            }

            //存證B2C-----E
        }
    }
}