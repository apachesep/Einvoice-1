using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace NSysDB
{
    namespace NTSQL
    {
        /// <summary>
        /// </summary>
        public partial class SQL1
        {

            public void ReturnArr(out string[] arr)
            {
                arr = new string[4];
                NSysDB.XMLClass oXMLeParamts = new NSysDB.XMLClass();
                string sFPathN = oXMLeParamts.GetParaXml("ImTxPaN");
                string sFPathP = oXMLeParamts.GetParaXml("ImTxPaP");
                string sFPathY = oXMLeParamts.GetParaXml("ImTxPaY");
                string sPaPartition = oXMLeParamts.GetParaXml("ImTxPaPartition");

                arr[0] = sFPathN;
                arr[1] = sFPathP;
                arr[2] = sFPathY;
                arr[3] = sPaPartition;
            }

            public void CreateDIRok(string sKind)
            {
                NSysDB.XMLClass oXMLeParamts = new NSysDB.XMLClass();
                string sFPathN = oXMLeParamts.GetParaXml("ImTxPaN");
                string sFPathP = oXMLeParamts.GetParaXml("ImTxPaP");
                string sFPathY = oXMLeParamts.GetParaXml("ImTxPaY");

                string ExXMLPa = oXMLeParamts.GetParaXml("ExXMLPa");

                CreateDIR(sFPathN);
                CreateDIR(sFPathP);
                CreateDIR(sFPathY);

                CreateDIR(ExXMLPa);
            }

            public void CreateDIR(string sfolder)
            {
                System.IO.DirectoryInfo MDir = new System.IO.DirectoryInfo(sfolder);
                MDir.Create();
            }

            public void GoLogsAll(string sProgramKey, string sProgramName, string sProgramMotion, string sEX, string sCount, Int16 sKind, bool sendMail = true)
            {
                Hashtable data = new Hashtable();
                data["ProgramKey"] = sProgramKey.ToString();
                data["ProgramName"] = sProgramName.ToString();
                string sMsn1 = "[注意:]";

                //不發送mail 1,2,99
                switch (sKind)
                {
                    case 1:
                        data["ProgramMotion"] = sProgramMotion.ToString() + " [start]";
                        data["ProgramState"] = "0";
                        break;

                    case 2:
                        data["ProgramMotion"] = sProgramMotion.ToString() + " [End]";
                        data["ProgramState"] = "0";
                        break;

                    case 11:
                        data["ProgramMotion"] = sMsn1 + "[文字檔字串有誤]" + sProgramMotion.ToString() + " [第" + sCount + "筆資料]";
                        data["ProgramState"] = "1";
                        break;

                    case 12:
                        data["ProgramMotion"] = sMsn1 + "[文字檔間隔數不正確]" + sProgramMotion.ToString() + " [第" + sCount + "筆資料]";
                        data["ProgramState"] = "1";
                        break;

                    case 13:
                        data["ProgramMotion"] = sMsn1 + "[移動文字檔發生錯誤]" + sProgramMotion.ToString();
                        data["ProgramState"] = "1";
                        break;

                    case 14:
                        data["ProgramMotion"] = sMsn1 + " [讀取文字檔發生錯誤]" + sProgramMotion.ToString();
                        data["ProgramState"] = "1";
                        break;

                    case 15:
                        data["ProgramMotion"] = sMsn1 + " [移動文字檔發生錯誤]" + sProgramMotion.ToString();
                        data["ProgramState"] = "1";
                        break;

                    case 16:
                        data["ProgramMotion"] = sMsn1 + "[匯入商品細項的文字檔發生錯誤/此商品已存在!!]" + sProgramMotion.ToString() + " [第" + sCount + "筆資料]";
                        data["ProgramState"] = "1";
                        break;

                    case 17:
                        data["ProgramMotion"] = sMsn1 + "[必要欄位沒值!!][DOriginalInvoiceDate][DOriginalInvoiceNumber]" + sProgramMotion.ToString() + " [第" + sCount + "筆資料]";
                        data["ProgramState"] = "1";
                        break;

                    case 23:
                        data["ProgramMotion"] = sMsn1 + "[此張發票 尚未餵入Details資料 暫不生成XML]" + sProgramMotion.ToString() + " [發票號碼:" + sCount + " ]";
                        data["ProgramState"] = "1";
                        break;

                    case 31:
                        data["ProgramMotion"] = sMsn1 + " [列印發票發生錯誤]" + sProgramMotion.ToString();
                        data["ProgramState"] = "1";
                        break;

                    case 32:
                        data["ProgramMotion"] = sMsn1 + " [WebService發生錯誤]" + sProgramMotion.ToString();
                        data["ProgramState"] = "1";
                        break;

                    case 41:
                        data["ProgramMotion"] = sMsn1 + " [生成發票PDF 發生錯誤]" + sProgramMotion.ToString();
                        data["ProgramState"] = "1";
                        break;

                    case 51:
                        data["ProgramMotion"] = sMsn1 + " [請盡速檢查文字檔內容!]" + sProgramMotion.ToString();
                        data["ProgramState"] = "1";
                        break;

                    case 61:
                        data["ProgramMotion"] = sMsn1 + " [Callim內Allin的程序被中斷!]" + sProgramMotion.ToString();
                        data["ProgramState"] = "1";
                        break;

                    case 99:
                        data["ProgramMotion"] = " [成功移動檔案] [" + sProgramMotion.ToString() + "]";
                        data["ProgramState"] = "0";
                        break;

                    default:
                        data["ProgramMotion"] = sMsn1 + " [函式引數錯誤]";
                        data["ProgramState"] = "1";
                        break;
                }

                #region 訊息置換

                data["ProgramMotion"] += sEX;
                InsertDataNonKey("LogsAll", data);
                if (!string.IsNullOrEmpty(sEX))
                    data["ProgramMotion"] = data["ProgramMotion"].ToString().Replace(sEX, "");
                data["SystemExceptionMsg"] = sEX;

                #endregion 訊息置換

                #region 判斷是否送mail

                short[] notSendMailAry = new short[] { 1, 2, 99 };
                if (!notSendMailAry.Contains(sKind))
                {
                    if (!sendMail)
                        data["ProgramState"] = "0";
                }

                #endregion 判斷是否送mail

                if (data["ProgramState"].ToString() != "0")
                {
                    try
                    {
                        StringBuilder htmlContent = new StringBuilder();
                        //var properties = data.GetType().GetProperties();
                        htmlContent.AppendLine(@"<div style=""border:solid black 1px;padding:7px;"">");
                        htmlContent.AppendLine(@"<p style=""font-size:23px;"">系統錯誤：</p>");

                        htmlContent.AppendLine("<p>");
                        htmlContent.AppendLine("錯誤分類：" + sKind);
                        htmlContent.AppendLine("</p>");

                        htmlContent.AppendLine(@"<div style=""border:solid #ccc 1px;padding:10px;"">");

                        int index = 1;
                        foreach (var key in data.Keys)
                        {
                            htmlContent.AppendLine("<p>");
                            htmlContent.AppendLine(string.Format("{0}:{1}", key, data[key]));
                            htmlContent.AppendLine("</p>");

                            index++;
                        }

                        htmlContent.AppendLine("</div>");
                        htmlContent.AppendLine("</div>");

                        XMLClass oXMLeParamts = new XMLClass();
                        string eToWho1 = oXMLeParamts.GetParaXml("eToWho");
                        string eFromWho1 = oXMLeParamts.GetParaXml("eFromWho");
                        AutoEMail(eToWho1, "", eFromWho1, "", htmlContent.ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        data["ProgramMotion"] = sMsn1 + " [SMTP被防守住!!]" + "[" + ex + "]";
                        InsertDataNonKey("LogsAll", data);
                    }
                }

                data = null;
                System.Threading.Thread.Sleep(20);
            }

            /// <summary>
            /// 有流水號的 Table 用的 Insert Function
            /// </summary>
            /// <param name="TableName">Table名稱</param>
            /// <param name="Data"></param>
            /// <returns>影響的資料筆數</returns>
            public int InsertData(string TableName, System.Collections.Hashtable Data)
            {
                object objTemp;
                string strColumn = "", strValue = "";
                int intResult;
                string strSQL = "";
                using (SqlCommand cmd = new SqlCommand())
                {
                    foreach (string strKey in Data.Keys)
                    {
                        strColumn += ", " + strKey;
                        strValue += ", @" + strKey;
                        if (Data[strKey] == null)
                        {
                            objTemp = DBNull.Value;
                        }
                        else
                        {
                            objTemp = Data[strKey];
                        }
                        cmd.Parameters.AddWithValue("@" + strKey, objTemp);
                    }
                    if (strColumn.Substring(0, 2) == ", ")
                    {
                        strColumn = strColumn.Substring(2);
                    }
                    if (strValue.Substring(0, 2) == ", ")
                    {
                        strValue = strValue.Substring(2);
                    }
                    strSQL = "Set NoCount On;Insert Into " + TableName + " (" + strColumn + ") Values (" + strValue + ");";
                    strSQL += "Select @@IDENTITY;";
                    cmd.CommandText = strSQL;
                    intResult = Convert.ToInt32(dbNTSQL.ExecuteScalar(cmd));
                }
                return intResult;
            }

            /// <summary>
            /// 沒有流水號的 Table 用的 Insert Function
            /// </summary>
            /// <param name="TableName">Table名稱</param>
            /// <param name="Data"></param>
            public string InsertDataNonKey(string TableName, System.Collections.Hashtable Data, Exception exp = null)
            {
                string resultMsg = "";
                object objTemp;
                string strColumn = "", strValue = "";
                string strSQL = "";
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        if (exp != null)
                            throw exp;
                        foreach (string strKey in Data.Keys)
                        {
                            strColumn += ", " + strKey;
                            strValue += ", @" + strKey;
                            if (Data[strKey] == null)
                            {
                                objTemp = DBNull.Value;
                            }
                            else
                            {
                                objTemp = Data[strKey];
                            }
                            cmd.Parameters.AddWithValue("@" + strKey, objTemp);
                        }
                        if (strColumn.Substring(0, 2) == ", ")
                        {
                            strColumn = strColumn.Substring(2);
                        }
                        if (strValue.Substring(0, 2) == ", ")
                        {
                            strValue = strValue.Substring(2);
                        }
                        strSQL = "Insert Into " + TableName + " (" + strColumn + ") Values (" + strValue + ");";
                        cmd.CommandText = strSQL;
                        dbNTSQL.ExecuteNonQuery(cmd);
                    }
                    catch (Exception ex)
                    {
                        resultMsg = ex.Message;
                    }
                }
                System.Threading.Thread.Sleep(5);
                return resultMsg;
            }

            /// <summary>
            /// 共用查詢用於一般Table-進階版 線路一
            /// </summary>
            /// <param name="ColumnName">欄位名稱</param>
            /// <param name="TableName">Table名稱</param>
            /// <param name="Condition">條件</param>
            /// <param name="DescAscWho">以誰排序</param>
            /// <param name="DescAsc">如何排序</param>
            /// <returns></returns>
            public DataView Kind1SelectTbl2(string ColumnName, string TableName, string Condition, string DescAscWho, string DescAsc)
            {
                DataView dvResult = null;
                string strSQL = "";
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (DataSet ds = new DataSet())
                    {
                        string ColumnName2 = " * ", Condition2 = "", DescAscWho2 = "", DescAsc2 = "";
                        if (ColumnName != "") ColumnName2 = ColumnName;
                        if (Condition != "") Condition2 = " Where " + Condition;
                        if (DescAsc != "") DescAsc2 = DescAsc;
                        if (DescAscWho != "") DescAscWho2 = " Order By " + DescAscWho + " " + DescAsc2;
                        strSQL = "SELECT " + ColumnName2 + " FROM " + TableName + Condition2 + DescAscWho2;
                        cmd.CommandText = strSQL;
                        if (dbNTSQL.ExecuteQuery(cmd, ds))
                        { dvResult = ds.Tables[0].DefaultView; }
                    }
                }
                return dvResult;
            }

            public int Kind1SelectTbl3(string ColumnName, string Condition, string TableName)
            {
                Int16 igetAnser = 0;
                DataView dvResult = Kind1SelectTbl2(ColumnName, TableName, Condition, "", "");
                if (dvResult != null)
                { igetAnser = 1; }
                dvResult = null;
                return igetAnser;
            }

            /// <summary>
            /// Update Function 適用於多條件做Update
            /// </summary>
            /// <param name="TableName">Table名稱</param>
            /// <param name="Data">Data</param>
            /// <param name="Condition">條件式</param>
            /// <returns>影響的資料筆數</returns>
            public int UpdateData2(string TableName, System.Collections.Hashtable Data, string Condition)
            {
                object objTemp;
                string strSQL = "";
                int intResult;
                using (SqlCommand cmd = new SqlCommand())
                {
                    foreach (string strKey in Data.Keys)
                    {
                        strSQL += ", " + strKey + "=@" + strKey;
                        if (Data[strKey] == null)
                        {
                            objTemp = DBNull.Value;
                        }
                        else
                        {
                            objTemp = Data[strKey];
                        }
                        cmd.Parameters.AddWithValue("@" + strKey, objTemp);
                    }
                    if (strSQL.Substring(0, 2) == ", ")
                    {
                        strSQL = strSQL.Substring(2);
                    }
                    strSQL = "Update " + TableName + " Set " + strSQL + " Where " + Condition;
                    cmd.CommandText = strSQL;
                    intResult = dbNTSQL.ExecuteNonQuery(cmd);
                }
                return intResult;
            }

            /// <summary>
            /// Update Function 適用於單一條件做Update
            /// </summary>
            /// <param name="TableName">Table名稱</param>
            /// <param name="Data">Data</param>
            /// <param name="PKeyName">條件式欄位名稱</param>
            /// <param name="PKeyValue">條件式欄位值</param>
            /// <returns>影響的資料筆數</returns>
            public int UpdateData(string TableName, System.Collections.Hashtable Data, string PKeyName, string PKeyValue)
            {
                int intResult;
                object objTemp;
                string strSQL = "";
                using (SqlCommand cmd = new SqlCommand())
                {
                    foreach (string strKey in Data.Keys)
                    {
                        strSQL += ", " + strKey + "=@" + strKey;
                        if (Data[strKey] == null)
                        {
                            objTemp = DBNull.Value;
                        }
                        else
                        {
                            objTemp = Data[strKey];
                        }
                        cmd.Parameters.AddWithValue("@" + strKey, objTemp);
                    }
                    if (strSQL.Substring(0, 2) == ", ")
                    {
                        strSQL = strSQL.Substring(2);
                    }
                    strSQL = "Update " + TableName + " Set " + strSQL + " Where " + PKeyName + "=@" + PKeyName;
                    cmd.Parameters.AddWithValue("@" + PKeyName, PKeyValue);
                    cmd.CommandText = strSQL;
                    intResult = dbNTSQL.ExecuteNonQuery(cmd);
                }
                return intResult;
            }

            /// <summary>
            /// Delete Function 適用於Delete資料
            /// </summary>
            /// <param name="TableName">TableName</param>
            /// <param name="Condition">條件</param>
            /// <returns></returns>
            public int DeleteData(string TableName, string Condition)
            {
                int intResult;
                string strSQL = "Delete From " + TableName;
                using (SqlCommand cmd = new SqlCommand())
                {
                    if (@Condition != "") strSQL = strSQL + " Where " + @Condition;
                    cmd.CommandText = strSQL;
                    intResult = dbNTSQL.ExecuteNonQuery(cmd);
                }
                return intResult;
            }

            /// <summary>
            /// 自動發信..這是一定要的啦
            /// </summary>
            /// <param name="ToWho">千萬別告訴我 你看不懂這些變數</param>
            /// <param name="CcWho"></param>
            /// <param name="FromWho"></param>
            /// <param name="strSubject"></param>
            /// <param name="strBody"></param>
            /// <returns></returns>
            public string AutoEMail(string ToWho, string CcWho, string FromWho, string strSubject, string strBody)
            {
                string strResult = "自動EMail通報機制 執行失敗!!";
                if (ToWho != "" && strBody != "")
                {
                    MailMessage mail = new MailMessage();

                    if (ToWho.IndexOf("@") != -1)
                    {
                        string[] toa = ToWho.Split(";".ToCharArray());
                        foreach (string to in toa)
                        {
                            mail.To.Add(new MailAddress(to));
                        }
                    }
                    else
                    {
                        string ToWho1 = "";
                        ToWho1 = "einvoice@cgs.com.tw";
                        string[] toa = ToWho1.Split(";".ToCharArray());
                        foreach (string to in toa)
                        {
                            mail.To.Add(new MailAddress(to));
                        }
                    }

                    if (FromWho != "")
                    {
                        string[] tod = FromWho.Split(";".ToCharArray());
                        foreach (string to in tod)
                        {
                            mail.From = new MailAddress(to);
                        }
                    }
                    else
                    {
                        mail.From = new MailAddress("\"電子發票管理平台\"einvoice@rinnai.com.tw");
                    }

                    if (strSubject != "")
                    { mail.Subject = strSubject; }
                    else
                    { mail.Subject = "電子發票管理平台-自動EMail通報機制 啟動了!!"; }

                    mail.IsBodyHtml = true;
                    mail.Body = "<p>" + strBody + "<p>";
                    SmtpClient SmtpServer = null;

                    //SmtpServer = new SmtpClient("MS1.cgs.com.tw", 587);
                    //SmtpServer = new SmtpClient("10.10.1.48", 587);
                    //SmtpServer.Credentials = new System.Net.NetworkCredential(@"cgs\einvoice", "cgs23534251");

                    SmtpServer = new SmtpClient("lisomail.rinnai.com.tw", 25);
                    SmtpServer.Credentials = new System.Net.NetworkCredential(@"einvsrv", "15963");

                    SmtpServer.Send(mail);
                    Console.WriteLine("寄信成功");
                    strResult = "執行成功!!";
                }
                return strResult;
            }

            private string ToL0(string sOldStrring, int AllLength)
            {
                while (sOldStrring.Length < AllLength)
                {
                    sOldStrring = "0" + sOldStrring;
                }
                return sOldStrring;
            }

            private string ToRW(string sOldStrring, int AllLength)
            {
                while (sOldStrring.Length < AllLength)
                {
                    sOldStrring = sOldStrring + " ";
                }
                return sOldStrring;
            }

            /// <summary>
            /// 產生B2B，B2C 發票PDF檔，
            /// </summary>
            /// <param name="MInvoiceNumberS"></param>
            /// <param name="sKind">1=>證明，3=>折讓</param>
            public void CallMkPDFinvoAll(string MInvoiceNumberS, string sKind)
            {
                string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                #region 取資料

                DataView dvResult0401H = null;
                DataView dvResult0401D = null;
                DataView dvResultTemp = null;
                string sMSIdentifier = "";//03553526 總公司(3000)  21543058 桃園所(3910)
                string sMBIdentifier = "";
                string sMBName = ""; //客戶名稱
                string sMBCustomerNumber = "";
                string sMInvoiceDate = "";
                string sReYear = "";
                string sReDay = "";
                string sReMonth = "";
                string sMInvoiceTime = "";
                string sMRandomNumber = "";
                string sATotalAmount = "";
                string sCompanyName = "";
                string ss0401SN = "";
                string sCompanyTel = "";
                string sASalesAmount = "";
                string sAFreeTaxSalesAmount = "";
                string sATaxType = "";
                string sATaxAmount = "";
                string sASalesAmount16 = "";
                string sATotalAmount16 = "";
                string sCompanyKey = "";
                string s0401DCount = "0";
                string sPdfPath = "";
                string sTempMapPath = "";
                string sCOM = "";
                string sTBKind = "";
                string sMRelateNumber = "";
                string sMMainRemark = "";
                dvResult0401H = Kind1SelectTbl2("", "A0401View", "MInvoiceNumber='" + MInvoiceNumberS + "'", "", "");
                if (dvResult0401H != null)
                {
                    sMSIdentifier = dvResult0401H.Table.Rows[0]["MSIdentifier"].ToString();
                    sMBIdentifier = dvResult0401H.Table.Rows[0]["MBIdentifier"].ToString();
                    sMBName = dvResult0401H.Table.Rows[0]["MBName"].ToString();
                    sMBCustomerNumber = dvResult0401H.Table.Rows[0]["MBCustomerNumber"].ToString();
                    sMInvoiceDate = dvResult0401H.Table.Rows[0]["MInvoiceDate"].ToString();
                    sReYear = (Convert.ToInt16(dvResult0401H.Table.Rows[0]["MInvoiceDate"].ToString().Substring(0, 4)) - 1911).ToString();
                    sReDay = dvResult0401H.Table.Rows[0]["MInvoiceDate"].ToString().Substring(4, 4);
                    sMInvoiceTime = dvResult0401H.Table.Rows[0]["MInvoiceTime"].ToString();
                    sMRandomNumber = dvResult0401H.Table.Rows[0]["MRandomNumber"].ToString();
                    sATotalAmount = dvResult0401H.Table.Rows[0]["ATotalAmount"].ToString();
                    sCompanyName = dvResult0401H.Table.Rows[0]["CompanyName"].ToString();
                    ss0401SN = dvResult0401H.Table.Rows[0]["s0401SN"].ToString();
                    sCompanyTel = dvResult0401H.Table.Rows[0]["CompanyTel"].ToString();
                    sAFreeTaxSalesAmount = dvResult0401H.Table.Rows[0]["AFreeTaxSalesAmount"].ToString();
                    sATaxType = dvResult0401H.Table.Rows[0]["ATaxType"].ToString();
                    sATaxAmount = dvResult0401H.Table.Rows[0]["ATaxAmount"].ToString();
                    sASalesAmount = dvResult0401H.Table.Rows[0]["ASalesAmount"].ToString();
                    sASalesAmount16 = ToL0(String.Format("{0:X}", Convert.ToInt64(dvResult0401H.Table.Rows[0]["ASalesAmount"])), 8);
                    sATotalAmount16 = ToL0(String.Format("{0:X}", Convert.ToInt64(dvResult0401H.Table.Rows[0]["ATotalAmount"])), 8);
                    sCompanyKey = dvResult0401H.Table.Rows[0]["CompanyKey"].ToString();
                    sMRelateNumber = dvResult0401H.Table.Rows[0]["MRelateNumber"].ToString();
                    sMMainRemark = dvResult0401H.Table.Rows[0]["MMainRemark"].ToString();
                    dvResult0401D = Kind1SelectTbl2("DDescription,DQuantity,DUnitPrice,DAmount,DSequenceNumber", "A0401D", "MInvoiceNumber='" + MInvoiceNumberS + "'", "DSequenceNumber", "ASC");
                    if (dvResult0401D != null)
                    {
                        s0401DCount = dvResult0401D.Count.ToString();
                    }
                    sTBKind = "A0401H";
                }
                else
                {
                    dvResult0401H = Kind1SelectTbl2("", "C0401View", "MInvoiceNumber='" + MInvoiceNumberS + "'", "", "");
                    if (dvResult0401H != null)
                    {
                        sMSIdentifier = dvResult0401H.Table.Rows[0]["MSIdentifier"].ToString();
                        sMBIdentifier = dvResult0401H.Table.Rows[0]["MBIdentifier"].ToString();
                        sMInvoiceDate = dvResult0401H.Table.Rows[0]["MInvoiceDate"].ToString();
                        sReYear = (Convert.ToInt16(dvResult0401H.Table.Rows[0]["MInvoiceDate"].ToString().Substring(0, 4)) - 1911).ToString();
                        sReDay = dvResult0401H.Table.Rows[0]["MInvoiceDate"].ToString().Substring(4, 4);
                        sMInvoiceTime = dvResult0401H.Table.Rows[0]["MInvoiceTime"].ToString();
                        sMRandomNumber = dvResult0401H.Table.Rows[0]["MRandomNumber"].ToString();
                        sATotalAmount = dvResult0401H.Table.Rows[0]["ATotalAmount"].ToString();
                        sCompanyName = dvResult0401H.Table.Rows[0]["CompanyName"].ToString();
                        ss0401SN = dvResult0401H.Table.Rows[0]["s0401SN"].ToString();
                        sCompanyTel = dvResult0401H.Table.Rows[0]["CompanyTel"].ToString();
                        sAFreeTaxSalesAmount = dvResult0401H.Table.Rows[0]["AFreeTaxSalesAmount"].ToString();
                        sATaxType = dvResult0401H.Table.Rows[0]["ATaxType"].ToString();
                        sATaxAmount = dvResult0401H.Table.Rows[0]["ATaxAmount"].ToString();
                        sASalesAmount = dvResult0401H.Table.Rows[0]["ASalesAmount"].ToString();
                        sASalesAmount16 = ToL0(String.Format("{0:X}", Convert.ToInt64(dvResult0401H.Table.Rows[0]["ASalesAmount"])), 8);
                        sATotalAmount16 = ToL0(String.Format("{0:X}", Convert.ToInt64(dvResult0401H.Table.Rows[0]["ATotalAmount"])), 8);
                        sCompanyKey = dvResult0401H.Table.Rows[0]["CompanyKey"].ToString();

                        dvResult0401D = Kind1SelectTbl2("DDescription,DQuantity,DUnitPrice,DAmount,DSequenceNumber", "C0401D", "MInvoiceNumber='" + MInvoiceNumberS + "'", "DSequenceNumber", "ASC");
                        if (dvResult0401D != null)
                        {
                            s0401DCount = dvResult0401D.Count.ToString();
                        }
                        sTBKind = "C0401H";
                    }
                    else
                    {
                        return;
                    }
                }

                string sATaxTypeN;
                switch (sATaxType)
                {
                    case "1":
                        sATaxTypeN = "TX";
                        break;

                    case "2":
                        sATaxTypeN = "TZ";
                        break;

                    default:
                        sATaxTypeN = "TX";
                        break;
                }

                dvResultTemp = Kind1SelectTbl2("eName", "SParameter", "eKind='eMonths' AND eSN='" + sMInvoiceDate.Substring(4, 2) + "'", "", "");
                if (dvResultTemp != null)
                {
                    sReMonth = Convert.ToString(dvResultTemp.Table.Rows[0][0]);
                }

                if (sKind == "1") //發票證明聯
                {
                    dvResultTemp = Kind1SelectTbl2("eName", "SParameter", "eKind='eMapPaths' AND eSN='1'", "", "");
                    if (dvResultTemp != null)
                    {
                        sPdfPath = Convert.ToString(dvResultTemp.Table.Rows[0][0]);
                    }
                }
                else if (sKind == "3") //發票證明聯補印
                {
                    dvResultTemp = Kind1SelectTbl2("eName", "SParameter", "eKind='eMapPaths' AND eSN='3'", "", "");
                    if (dvResultTemp != null)
                    {
                        sPdfPath = Convert.ToString(dvResultTemp.Table.Rows[0][0]);
                    }
                }

                dvResultTemp = Kind1SelectTbl2("eName", "SParameter", "eKind='eMapPaths' AND eSN='4'", "", "");
                if (dvResultTemp != null)
                {
                    sTempMapPath = Convert.ToString(dvResultTemp.Table.Rows[0][0]);
                }

                dvResultTemp = Kind1SelectTbl2("eName", "SParameter", "eKind='sCOM' AND eSN='1'", "", "");
                if (dvResultTemp != null)
                {
                    sCOM = Convert.ToString(dvResultTemp.Table.Rows[0][0]);
                }

                #endregion 取資料

                #region 生成一維條碼

                string s1dim = "";
                DataView dvResultDim = null;
                dvResultDim = Kind1SelectTbl2("dbo.C0401H.MInvoiceNumber, (SUBSTRING(dbo.C0401H.MInvoiceDate, 1, 4) - 1911) +dbo.SParameter.eMemo + dbo.C0401H.MInvoiceNumber + dbo.C0401H.MRandomNumber AS myMaps ", "dbo.SParameter RIGHT OUTER JOIN dbo.C0401H ON dbo.SParameter.eSN = SUBSTRING(dbo.C0401H.MInvoiceDate, 5, 2) ", "MInvoiceNumber='" + MInvoiceNumberS + "'", "", "");
                if (dvResultDim != null)
                {
                    s1dim = dvResultDim.Table.Rows[0]["myMaps"].ToString();
                }
                else
                {
                    dvResultDim = Kind1SelectTbl2("dbo.A0401H.MInvoiceNumber, Convert(nvarchar,(SUBSTRING(dbo.A0401H.MInvoiceDate, 1, 4) - 1911)) +dbo.SParameter.eMemo +dbo.A0401H.MInvoiceNumber +dbo.A0401H.MRandomNumber AS myMaps", "dbo.SParameter RIGHT OUTER JOIN dbo.A0401H ON dbo.SParameter.eSN = convert(nvarchar, SUBSTRING(dbo.A0401H.MInvoiceDate, 5, 2))", "MInvoiceNumber='" + MInvoiceNumberS + "'", "", "");
                    if (dvResultDim != null)
                    {
                        s1dim = dvResultDim.Table.Rows[0]["myMaps"].ToString();
                    }
                }

                string s1dimFileName = sTempMapPath + MInvoiceNumberS + "Dim" + sPgSN + ".png";
                GoGetCode39(s1dimFileName, s1dim.Trim());

                #endregion 生成一維條碼

                #region 生成QR1

                string s1QR = "";
                s1QR = MInvoiceNumberS + sReYear + sReDay + sMRandomNumber + sASalesAmount16 + sATotalAmount16 + sMBIdentifier + sMSIdentifier + GoAESb64(MInvoiceNumberS + sMRandomNumber, sCompanyKey) + ":**********:" + s0401DCount + ":" + s0401DCount + ":1:";
                string s1qrFileName = sTempMapPath + MInvoiceNumberS + "Q1R" + sPgSN + ".png";
                MkQR4(sPgSN, s1QR, s1qrFileName);

                #endregion 生成QR1

                #region 生成QR2

                string s2QR = "**";
                if (dvResult0401D != null)
                {
                    for (int i = 0; i < dvResult0401D.Count; i++)
                    {
                        s2QR += Convert.ToString(dvResult0401D.Table.Rows[i]["DDescription"]) + ":" + Convert.ToString(dvResult0401D.Table.Rows[i]["DQuantity"]) + ":" + Convert.ToDouble(dvResult0401D.Table.Rows[i]["DUnitPrice"]).ToString("0") + ":";
                    }
                }
                string s2qrFileName = sTempMapPath + MInvoiceNumberS + "Q2R" + sPgSN + ".png";

                MkQR6(sPgSN, s2QR, s2qrFileName);

                #endregion 生成QR2

                #region 發票內容

                #region 發票紙張尺寸設定相關參數 20171107 by俊晨

                //pdf 預設寬度 數字越大越寬
                float defaultDocWidth = 162f;
                //pdf 預設高度 數字越大越高  不含明細
                float defaultDocHeight = sKind == "3" ? 309f : 302f;
                //每次增加一行欲增加數值
                float stepAdd = 7f;
                //每次增加一行欲增加數值 筆明細需增加的高度
                float stepDetailsAdd = 40f;
                // 總計所需高度25f
                float detailsTotalHeight = 43f;

                #region 設定pdf紙張大小 依照資料多寡

                //取客戶名稱 名稱若太長，依行數增加紙張長度
                List<string> sMBNameList = this.ToListCutDownString(sMBName, 12, 12);
                if (sMBNameList.Count > 1)
                {
                    float addPadding = (sMBNameList.Count * stepAdd);
                    defaultDocHeight += addPadding;
                }

                //取明細資料 依明細筆數增加紙張長度 20171114新增判斷多行判斷
                DataView dvResult0401Dbb = null;

                dvResult0401Dbb = Kind1SelectTbl2("DDescription,DQuantity,DUnitPrice,DAmount,DSequenceNumber", "A0401D", "MInvoiceNumber='" + MInvoiceNumberS + "'", "DSequenceNumber", "ASC");
                Dictionary<int, List<string>> sDetailsList = new Dictionary<int, List<string>>();
                if (dvResult0401Dbb.Count > 0)
                {
                    for (int i = 0; i < dvResult0401Dbb.Count; i++)
                    {
                        defaultDocHeight += stepDetailsAdd;
                        string detailsItem = Convert.ToString(dvResult0401Dbb.Table.Rows[i]["DDescription"]);
                        sDetailsList[i] = this.ToListCutDownString(detailsItem, 14, 14);
                        if (sDetailsList[i].Count > 0)
                        {
                            foreach (var item in sDetailsList[i])
                                defaultDocHeight += stepAdd;
                        }
                    }
                }

                //統編條件
                if (sMSIdentifier.Equals("03553526"))
                {
                    defaultDocHeight += stepAdd;
                }

                //客戶條件
                List<string> spStrList = new List<string>();

                if (sMBCustomerNumber == "301127")
                {
                    string spStr =
                        @"
                        本發票之債權已轉讓予玉山銀行，若對本發票所列明細有任何疑問，請立即通知玉山銀行，並請於貨款到期後，逕將應付款項電匯至玉山銀行新竹分行，
                        帳號：0060-440-048000 戶名：台灣林內工業股份有限公司，如以支票付款，應於票面受款人處載明限存入上開戶名及帳號。";
                    spStr = spStr.Trim().Replace(" ", string.Empty).Replace("\r\n", string.Empty);
                    spStrList = this.ToListCutDownString(spStr.Trim(), 14, 14);
                    if (spStrList.Count > 1)
                    {
                        float addPadding = (spStrList.Count * stepAdd);
                        defaultDocHeight += addPadding;
                        defaultDocHeight += 50f;
                    }
                }
                defaultDocHeight += detailsTotalHeight;
                //QRCode TopPosition
                float defaultRightQRCodeTopPosition = defaultDocHeight - 193f;

                #endregion 設定pdf紙張大小 依照資料多寡

                #endregion 發票紙張尺寸設定相關參數 20171107 by俊晨

                //原始設定
                //Document doc = new Document(new iTextSharp.text.Rectangle(162f, 450f), 0, 0, 0, 0);

                string sPDFpath = sPdfPath + MInvoiceNumberS + ".pdf";
                Document doc = new Document();
                doc = new Document(new iTextSharp.text.Rectangle(defaultDocWidth, defaultDocHeight), 0, 0, 0, 0);
                try
                {
                    PdfWriter.GetInstance(doc, new FileStream(sPDFpath, FileMode.Create));
                }
                catch (Exception ex)
                {
                    //檔案被開啟
                    GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, MInvoiceNumberS, "[使用者正在開啟這個DPF !!][" + sPDFpath + "]" + ex.ToString(), "", 41);
                }
                doc.Open();
                //string fontPath = @"c:\windows\fonts\mingliu.ttc,1";
                BaseFont bfChinese = PublicMethodFramework35.Repositoies.BaseFontType();

                iTextSharp.text.Font ChFont4 = new iTextSharp.text.Font(bfChinese, 4);
                iTextSharp.text.Font ChFont6 = new iTextSharp.text.Font(bfChinese, 6);
                iTextSharp.text.Font ChFont7 = new iTextSharp.text.Font(bfChinese, 7);
                iTextSharp.text.Font ChFont8 = new iTextSharp.text.Font(bfChinese, 8);
                iTextSharp.text.Font ChFont8B = new iTextSharp.text.Font(bfChinese, 8, 1);
                iTextSharp.text.Font ChFont9 = new iTextSharp.text.Font(bfChinese, 9);
                iTextSharp.text.Font ChFont9B = new iTextSharp.text.Font(bfChinese, 9, 1);
                iTextSharp.text.Font ChFont10 = new iTextSharp.text.Font(bfChinese, 10);
                iTextSharp.text.Font ChFont11 = new iTextSharp.text.Font(bfChinese, 11);
                iTextSharp.text.Font ChFont11B = new iTextSharp.text.Font(bfChinese, 11, 1);
                iTextSharp.text.Font ChFont12 = new iTextSharp.text.Font(bfChinese, 12);
                iTextSharp.text.Font ChFont13 = new iTextSharp.text.Font(bfChinese, 13);
                iTextSharp.text.Font ChFont14 = new iTextSharp.text.Font(bfChinese, 14);
                iTextSharp.text.Font ChFont15 = new iTextSharp.text.Font(bfChinese, 15);
                iTextSharp.text.Font ChFont15B = new iTextSharp.text.Font(bfChinese, 15, 1);
                iTextSharp.text.Font ChFont16 = new iTextSharp.text.Font(bfChinese, 16);
                iTextSharp.text.Font ChFont16B = new iTextSharp.text.Font(bfChinese, 16, 1);
                iTextSharp.text.Font ChFont17 = new iTextSharp.text.Font(bfChinese, 17);
                iTextSharp.text.Font ChFont17B = new iTextSharp.text.Font(bfChinese, 17, 1);
                iTextSharp.text.Font ChFont18 = new iTextSharp.text.Font(bfChinese, 18);
                iTextSharp.text.Font ChFont18B = new iTextSharp.text.Font(bfChinese, 18, 1);
                iTextSharp.text.Font ChFont19 = new iTextSharp.text.Font(bfChinese, 19);
                iTextSharp.text.Font ChFont19B = new iTextSharp.text.Font(bfChinese, 19, 1);
                iTextSharp.text.Font ChFont20 = new iTextSharp.text.Font(bfChinese, 20);
                iTextSharp.text.Font ChFont20B = new iTextSharp.text.Font(bfChinese, 20, 1);
                iTextSharp.text.Font ChFont21B = new iTextSharp.text.Font(bfChinese, 21, 1);
                iTextSharp.text.Font ChFont22B = new iTextSharp.text.Font(bfChinese, 22, 1);

                Paragraph ParagraphW1 = new Paragraph();
                //補印專用
                Paragraph ParagraphForSkind3 = new Paragraph();
                ParagraphW1.Leading = 23;
                ParagraphW1.Alignment = Element.ALIGN_CENTER;

                Phrase ParagraphWa011 = new Phrase(sCOM, ChFont20B);
                Phrase ParagraphWa014 = new Phrase("         ", ChFont4);
                ParagraphW1.Add(ParagraphWa011);
                ParagraphW1.Add(ParagraphWa014);
                ParagraphW1.Add("\n");

                string sProveName10 = "電子發票證明聯";
                string sProveName11 = "補印";
                if (sKind == "1")
                {
                    Phrase ParagraphWa021 = new Phrase(sProveName10, ChFont19B);
                    Phrase ParagraphWa022 = new Phrase("            ", ChFont4);
                    ParagraphW1.Add(ParagraphWa021);
                    ParagraphW1.Add(ParagraphWa022);
                }
                else if (sKind == "3")
                {
                    Phrase ParagraphWa021 = new Phrase(sProveName10, ChFont16B);
                    Phrase ParagraphWa022 = new Phrase(sProveName11, ChFont16B);
                    //無意義的空白 註解
                    //Phrase ParagraphWa023 = new Phrase("               ", ChFont4);

                    //new add by 20180108
                    ParagraphForSkind3.Add(ParagraphWa021);
                    ParagraphForSkind3.Add(ParagraphWa022);
                    //ParagraphW1.Add(ParagraphWa022);
                    ParagraphForSkind3.Alignment = iTextSharp.text.Image.LEFT_ALIGN;
                    //右邊QRCODE 若有需要在調整
                    //defaultRightQRCodeTopPosition += 7f;
                    //無意義的空白 註解
                    //ParagraphW1.Add(ParagraphWa023);
                    //與標題兼間距
                    ParagraphForSkind3.Leading = 23;
                }

                doc.Add(ParagraphW1);
                doc.Add(ParagraphForSkind3);

                doc.Add(new Paragraph(3f, "\n", ChFont9B));

                Paragraph ParagraphW2 = new Paragraph();
                ParagraphW2.Leading = 17;
                ParagraphW2.Alignment = Element.ALIGN_CENTER;

                string sYM = sReYear + "年" + sReMonth;
                Phrase ParagraphWa031 = new Phrase(sYM, ChFont18B);
                Phrase ParagraphWa032 = new Phrase("      ", ChFont4);
                ParagraphW2.Add(ParagraphWa031);
                ParagraphW2.Add(ParagraphWa032);
                ParagraphW2.Add("\n");

                string sMInvoiceNumber = MInvoiceNumberS.Substring(0, 2) + "-" + MInvoiceNumberS.Substring(2, 8);
                Phrase ParagraphWa041 = new Phrase(sMInvoiceNumber, ChFont18B);
                Phrase ParagraphWa042 = new Phrase("      ", ChFont4);
                ParagraphW2.Add(ParagraphWa041);
                ParagraphW2.Add(ParagraphWa042);
                ParagraphW2.Add("\n");
                doc.Add(ParagraphW2);

                string sYMDhms = sMInvoiceDate.Substring(0, 4) + "-" + sMInvoiceDate.Substring(4, 2) + "-" + sMInvoiceDate.Substring(6, 2) + " " + sMInvoiceTime + "  格式 25";
                doc.Add(new Paragraph(10f, sYMDhms, ChFont9B));
                string sMRandomNumberA = "隨機碼:" + sMRandomNumber + "             " + "總計:" + sATotalAmount;
                doc.Add(new Paragraph(10f, sMRandomNumberA, ChFont9B));

                string sMSIdentifierB = "";
                if (sMBIdentifier == "0000000000")
                { sMSIdentifierB = "賣方:" + sMSIdentifier; }
                else
                { sMSIdentifierB = "賣方:" + sMSIdentifier + "          " + "買方:" + sMBIdentifier; }

                doc.Add(new Paragraph(10f, sMSIdentifierB, ChFont9B));

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(s1dimFileName);
                image.Alignment = iTextSharp.text.Image.LEFT_ALIGN;
                image.ScaleToFit(144f, 150f);

                doc.Add(image);

                iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(s1qrFileName);
                image1.Alignment = iTextSharp.text.Image.LEFT_ALIGN;
                image1.ScaleToFit(55f, 55f);

                doc.Add(image1);

                iTextSharp.text.Image image2 = iTextSharp.text.Image.GetInstance(s2qrFileName);
                //原始設定 image2.SetAbsolutePosition(80f, 257f);
                //紙張加長 數字要相對加大
                image2.SetAbsolutePosition(80f, defaultRightQRCodeTopPosition);
                image2.ScaleToFit(55f, 55f);
                doc.Add(image2);

                Paragraph ParagraphW9 = new Paragraph();
                ParagraphW9.Leading = 10;
                ParagraphW9.Alignment = Element.ALIGN_LEFT;
                Phrase ParagraphWa091 = new Phrase(sCompanyName + "   序:" + ss0401SN, ChFont9B);
                Phrase ParagraphWa091n = new Phrase("\n", ChFont9B);
                Phrase ParagraphWa092 = new Phrase("退貨憑電子發票證明聯正本辦理", ChFont9B);
                Phrase ParagraphWa093n = new Phrase("\n", ChFont9B);
                Phrase ParagraphWa0931n = new Phrase("\n", ChFont9B);
                Phrase ParagraphWa094 = new Phrase("--------------------------------------------", ChFont9B);
                Phrase ParagraphWa0941 = new Phrase("\n", ChFont9B);
                ParagraphW9.Add(ParagraphWa091);
                ParagraphW9.Add(ParagraphWa091n);
                ParagraphW9.Add(ParagraphWa092);
                ParagraphW9.Add(ParagraphWa093n);
                ParagraphW9.Add(ParagraphWa0931n);
                ParagraphW9.Add(ParagraphWa094);
                ParagraphW9.Add(ParagraphWa0941);
                doc.Add(ParagraphW9);

                //加入明細------S

                //取資料
                string s0401DCountAll = "0";

                dvResult0401Dbb = Kind1SelectTbl2("DDescription,DQuantity,DUnitPrice,DAmount,DSequenceNumber", "A0401D", "MInvoiceNumber='" + MInvoiceNumberS + "'", "DSequenceNumber", "ASC");
                if (dvResult0401Dbb != null)
                {
                    s0401DCountAll = dvResult0401Dbb.Count.ToString();
                }
                else
                    throw new Exception("發票：" + MInvoiceNumberS + " 查無明細 無法產生PDF");

                #endregion 發票內容

                #region 明細內容

                if (sMBName != "")
                {
                    //--加上字數判斷 20170921 by 俊晨
                    doc.Add(new Paragraph(10f, "買方客戶:" + sMBCustomerNumber + "　" + sMMainRemark + "\n", ChFont9B));
                    foreach (var s in sMBNameList)
                        doc.Add(new Paragraph(10f, s + "\n", ChFont9B));
                }

                doc.Add(new Paragraph(10f, "\n" + sCompanyName + "    " + sCompanyTel, ChFont9B));
                doc.Add(new Paragraph(10f, "發票號碼:" + MInvoiceNumberS + "\n\n", ChFont9B));

                //if (dvResult0401Dbb != null)
                //{
                //    //for (int i = 0; i < dvResult0401Dbb.Count; i++)
                //    //{
                //    foreach (var di in sDetailsList as Dictionary<int, List<string>>)
                //    {
                //        foreach (var itemStr in di.Value as List<string>)
                //        {
                //            doc.Add(new Paragraph(10f, itemStr + "\n", ChFont9B));
                //        }
                //        doc.Add(new Paragraph(10f, "單價 : " + Convert.ToDouble(dvResult0401Dbb.Table.Rows[di.Key]["DUnitPrice"]).ToString("0.00") + "  數量 : " + Convert.ToString(dvResult0401Dbb.Table.Rows[di.Key]["DQuantity"]) + "  金額 : " + Convert.ToString(dvResult0401Dbb.Table.Rows[di.Key]["DAmount"]) + "\n", ChFont9B));
                //    }
                //    //}
                //}

                if (dvResult0401Dbb != null)
                {
                    doc.Add(new Paragraph(10f, "品名     單價     數量    金額 :(未稅)\n", ChFont9B));
                    //for (int i = 0; i < dvResult0401Dbb.Count; i++)
                    //{
                    foreach (var di in sDetailsList as Dictionary<int, List<string>>)
                    {
                        foreach (var itemStr in di.Value as List<string>)
                        {
                            doc.Add(new Paragraph(12f, itemStr + "\n", ChFont9B));
                        }
                        var uniPrice = Convert.ToDouble(dvResult0401Dbb.Table.Rows[di.Key]["DUnitPrice"]).ToString("0.00");
                        var quantity = Convert.ToDouble(dvResult0401Dbb.Table.Rows[di.Key]["DQuantity"]).ToString();
                        var amount = Convert.ToDouble(dvResult0401Dbb.Table.Rows[di.Key]["DAmount"]).ToString();

                        uniPrice = AddEmptyStringBefore(22, uniPrice);
                        quantity = AddEmptyStringBefore(13, quantity);
                        amount = AddEmptyStringBefore((amount.Length == 1 ? 11 : 13), amount);
                        //doc.Add(new Paragraph(10f, "單價 : " + Convert.ToDouble(dvResult0401Dbb.Table.Rows[di.Key]["DUnitPrice"]).ToString("0.00") + "  數量 : " + Convert.ToString(dvResult0401Dbb.Table.Rows[di.Key]["DQuantity"]) + "  金額 : " + Convert.ToString(dvResult0401Dbb.Table.Rows[di.Key]["DAmount"]) + "\n", ChFont9B));
                        string pirceInfo = string.Concat(uniPrice, quantity, amount);
                        doc.Add(new Paragraph(10f, pirceInfo, ChFont9B));
                    }
                    //}
                }

                doc.Add(new Paragraph(10f, "應稅額:" + sASalesAmount + " 免稅額:" + sAFreeTaxSalesAmount + " 稅額:" + sATaxAmount + "\n", ChFont9B));
                doc.Add(new Paragraph(14f, "總計 $" + sATotalAmount + "\n", ChFont9B));
                doc.Add(new Paragraph(14f, "共 " + dvResult0401Dbb.Count.ToString() + "項  合計 $" + sATotalAmount + "   (" + sATaxTypeN + ")" + "\n", ChFont11B));
                doc.Add(new Paragraph(10f, "\n", ChFont9B));

                #region 特定客戶代號 加上指定字串 by geroge 20171031 俊晨

                if (sMSIdentifier.Equals("03553526"))
                {
                    doc.Add(new Paragraph(10f, string.Concat("訂單號碼:", sMRelateNumber), ChFont9B));
                    doc.Add(new Paragraph(10f, "\n", ChFont9B));
                }

                if (sMBCustomerNumber == "301127")
                {
                    doc.Add(new Paragraph(10f, "備註:\n", ChFont9B));
                    foreach (var s in spStrList)
                        doc.Add(new Paragraph(10f, s + "\n", ChFont9B));
                    doc.Add(new Paragraph(10f, "\n", ChFont9B));
                }

                #endregion 特定客戶代號 加上指定字串 by geroge 20171031 俊晨

                #region 活動公告

                DataView dvResultTemp2 = Kind1SelectTbl2("eName", "SParameter", "eKind='eActions'", "eSN", "ASC");
                if (dvResultTemp2 != null)
                {
                    for (int i = 0; i < dvResultTemp2.Count; i++)
                    {
                        doc.Add(new Paragraph(14f, Convert.ToString(dvResultTemp2.Table.Rows[i][0]) + "\n", ChFont11B));
                    }
                }

                #endregion 活動公告

                //加入明細------E

                doc.NewPage();
                doc.Close();
                doc.Dispose();

                #endregion 明細內容

                File.Delete(s1dimFileName);
                File.Delete(s1qrFileName);
                File.Delete(s2qrFileName);

                CallExPdfYN(MInvoiceNumberS, sTBKind, "1");
            }

            /// <summary>
            /// 產生B2C 發票PDF檔
            /// </summary>
            /// <param name="MInvoiceNumberS"></param>
            /// <param name="sKind">1=>證明，2=>折讓</param>
            public void CallMkPDFinvo(string MInvoiceNumberS, string sKind)
            {
                string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                #region 取資料

                DataView dvResult0401H = null;
                DataView dvResult0401D = null;
                DataView dvResultTemp = null;
                string sMSIdentifier = "";
                string sMBIdentifier = "";
                string sMBName = "";
                string sMInvoiceDate = "";
                string sReYear = "";
                string sReDay = "";
                string sReMonth = "";
                string sMInvoiceTime = "";
                string sMRandomNumber = "";
                string sATotalAmount = "";
                string sCompanyName = "";
                string ss0401SN = "";
                string sCompanyTel = "";
                string sASalesAmount = "";
                string sAFreeTaxSalesAmount = "";
                string sATaxType = "";
                string sATaxAmount = "";
                string sASalesAmount16 = "";
                string sATotalAmount16 = "";
                string sCompanyKey = "";
                string s0401DCount = "0";
                string sPdfPath = "";
                string sTempMapPath = "";
                string sCOM = "";
                string sTBKind = "";
                dvResult0401H = Kind1SelectTbl2("", "A0401View", "MInvoiceNumber='" + MInvoiceNumberS + "'", "", "");
                if (dvResult0401H != null)
                {
                    sMSIdentifier = dvResult0401H.Table.Rows[0]["MSIdentifier"].ToString();
                    sMBIdentifier = dvResult0401H.Table.Rows[0]["MBIdentifier"].ToString();
                    sMInvoiceDate = dvResult0401H.Table.Rows[0]["MInvoiceDate"].ToString();
                    sReYear = (Convert.ToInt16(dvResult0401H.Table.Rows[0]["MInvoiceDate"].ToString().Substring(0, 4)) - 1911).ToString();
                    sReDay = dvResult0401H.Table.Rows[0]["MInvoiceDate"].ToString().Substring(4, 4);
                    sMInvoiceTime = dvResult0401H.Table.Rows[0]["MInvoiceTime"].ToString();
                    sMRandomNumber = dvResult0401H.Table.Rows[0]["MRandomNumber"].ToString();
                    sATotalAmount = dvResult0401H.Table.Rows[0]["ATotalAmount"].ToString();
                    sCompanyName = dvResult0401H.Table.Rows[0]["CompanyName"].ToString();
                    ss0401SN = dvResult0401H.Table.Rows[0]["s0401SN"].ToString();
                    sCompanyTel = dvResult0401H.Table.Rows[0]["CompanyTel"].ToString();
                    sAFreeTaxSalesAmount = dvResult0401H.Table.Rows[0]["AFreeTaxSalesAmount"].ToString();
                    sATaxType = dvResult0401H.Table.Rows[0]["ATaxType"].ToString();
                    sATaxAmount = dvResult0401H.Table.Rows[0]["ATaxAmount"].ToString();
                    sASalesAmount = dvResult0401H.Table.Rows[0]["ASalesAmount"].ToString();
                    sASalesAmount16 = ToL0(String.Format("{0:X}", Convert.ToInt64(dvResult0401H.Table.Rows[0]["ASalesAmount"])), 8);
                    sATotalAmount16 = ToL0(String.Format("{0:X}", Convert.ToInt64(dvResult0401H.Table.Rows[0]["ATotalAmount"])), 8);
                    sCompanyKey = dvResult0401H.Table.Rows[0]["CompanyKey"].ToString();

                    dvResult0401D = Kind1SelectTbl2("DDescription,DQuantity,DUnitPrice,DAmount,DSequenceNumber", "A0401D", "MInvoiceNumber='" + MInvoiceNumberS + "'", "DSequenceNumber", "ASC");
                    if (dvResult0401D != null)
                    {
                        s0401DCount = dvResult0401D.Count.ToString();
                    }
                    sTBKind = "A0401H";
                }
                else
                {
                    dvResult0401H = Kind1SelectTbl2("", "C0401View", "MInvoiceNumber='" + MInvoiceNumberS + "'", "", "");
                    if (dvResult0401H != null)
                    {
                        sMBName = dvResult0401H.Table.Rows[0]["MBName"].ToString();
                        sMSIdentifier = dvResult0401H.Table.Rows[0]["MSIdentifier"].ToString();
                        sMBIdentifier = dvResult0401H.Table.Rows[0]["MBIdentifier"].ToString();
                        sMInvoiceDate = dvResult0401H.Table.Rows[0]["MInvoiceDate"].ToString();
                        sReYear = (Convert.ToInt16(dvResult0401H.Table.Rows[0]["MInvoiceDate"].ToString().Substring(0, 4)) - 1911).ToString();
                        sReDay = dvResult0401H.Table.Rows[0]["MInvoiceDate"].ToString().Substring(4, 4);
                        sMInvoiceTime = dvResult0401H.Table.Rows[0]["MInvoiceTime"].ToString();
                        sMRandomNumber = dvResult0401H.Table.Rows[0]["MRandomNumber"].ToString();
                        sATotalAmount = dvResult0401H.Table.Rows[0]["ATotalAmount"].ToString();
                        sCompanyName = dvResult0401H.Table.Rows[0]["CompanyName"].ToString();
                        ss0401SN = dvResult0401H.Table.Rows[0]["s0401SN"].ToString();
                        sCompanyTel = dvResult0401H.Table.Rows[0]["CompanyTel"].ToString();
                        sAFreeTaxSalesAmount = dvResult0401H.Table.Rows[0]["AFreeTaxSalesAmount"].ToString();
                        sATaxType = dvResult0401H.Table.Rows[0]["ATaxType"].ToString();
                        sATaxAmount = dvResult0401H.Table.Rows[0]["ATaxAmount"].ToString();
                        sASalesAmount = dvResult0401H.Table.Rows[0]["ASalesAmount"].ToString();
                        sASalesAmount16 = ToL0(String.Format("{0:X}", Convert.ToInt64(dvResult0401H.Table.Rows[0]["ASalesAmount"])), 8);
                        sATotalAmount16 = ToL0(String.Format("{0:X}", Convert.ToInt64(dvResult0401H.Table.Rows[0]["ATotalAmount"])), 8);
                        sCompanyKey = dvResult0401H.Table.Rows[0]["CompanyKey"].ToString();

                        dvResult0401D = Kind1SelectTbl2("DDescription,DQuantity,DUnitPrice,DAmount,DSequenceNumber", "C0401D", "MInvoiceNumber='" + MInvoiceNumberS + "'", "DSequenceNumber", "ASC");
                        if (dvResult0401D != null)
                        {
                            s0401DCount = dvResult0401D.Count.ToString();
                        }
                        sTBKind = "C0401H";
                    }
                    else
                    {
                        return;
                    }
                }

                string sATaxTypeN = GetTaxTypeCodeByTaxTypeID(sATaxType);

                dvResultTemp = Kind1SelectTbl2("eName", "SParameter", "eKind='eMonths' AND eSN='" + sMInvoiceDate.Substring(4, 2) + "'", "", "");
                if (dvResultTemp != null)
                {
                    sReMonth = Convert.ToString(dvResultTemp.Table.Rows[0][0]);
                }

                if (sKind == "1") //發票證明聯
                {
                    dvResultTemp = Kind1SelectTbl2("eName", "SParameter", "eKind='eMapPaths' AND eSN='1'", "", "");
                    if (dvResultTemp != null)
                    {
                        sPdfPath = Convert.ToString(dvResultTemp.Table.Rows[0][0]);
                    }
                }
                else if (sKind == "3") //發票證明聯補印
                {
                    dvResultTemp = Kind1SelectTbl2("eName", "SParameter", "eKind='eMapPaths' AND eSN='3'", "", "");
                    if (dvResultTemp != null)
                    {
                        sPdfPath = Convert.ToString(dvResultTemp.Table.Rows[0][0]);
                    }
                }

                dvResultTemp = Kind1SelectTbl2("eName", "SParameter", "eKind='eMapPaths' AND eSN='4'", "", "");
                if (dvResultTemp != null)
                {
                    sTempMapPath = Convert.ToString(dvResultTemp.Table.Rows[0][0]);
                }

                dvResultTemp = Kind1SelectTbl2("eName", "SParameter", "eKind='sCOM' AND eSN='1'", "", "");
                if (dvResultTemp != null)
                {
                    sCOM = Convert.ToString(dvResultTemp.Table.Rows[0][0]);
                }

                #endregion 取資料

                #region 生成一維條碼

                string s1dim = "";
                DataView dvResultDim = null;
                dvResultDim = Kind1SelectTbl2("dbo.C0401H.MInvoiceNumber,(CONVERT(nvarchar(50), SUBSTRING(dbo.C0401H.MInvoiceDate, 1, 4) - 1911) +dbo.SParameter.eMemo + dbo.C0401H.MInvoiceNumber + dbo.C0401H.MRandomNumber)  AS myMaps", "dbo.SParameter RIGHT OUTER JOIN dbo.C0401H ON dbo.SParameter.eSN = SUBSTRING(dbo.C0401H.MInvoiceDate, 5, 2)", "MInvoiceNumber='" + MInvoiceNumberS + "'", "", "");

                if (dvResultDim != null)
                {
                    s1dim = dvResultDim.Table.Rows[0]["myMaps"].ToString();
                }
                else
                {
                    dvResultDim = Kind1SelectTbl2("dbo.A0401H.MInvoiceNumber, (CONVERT(nvarchar(50), SUBSTRING(dbo.A0401H.MInvoiceDate, 1, 4) - 1911) +dbo.SParameter.eMemo + dbo.A0401H.MInvoiceNumber +dbo.A0401H.MRandomNumber)  AS myMaps", "dbo.SParameter RIGHT OUTER JOIN dbo.A0401H ON dbo.SParameter.eSN = SUBSTRING(dbo.A0401H.MInvoiceDate, 5, 2)", "MInvoiceNumber='" + MInvoiceNumberS + "'", "", "");
                    if (dvResultDim != null)
                    {
                        s1dim = dvResultDim.Table.Rows[0]["myMaps"].ToString();
                    }
                }

                string s1dimFileName = sTempMapPath + MInvoiceNumberS + "Dim" + sPgSN + ".png";
                GoGetCode39(s1dimFileName, s1dim.Trim());

                #endregion 生成一維條碼

                #region 生成QR1

                string s1QR = "";
                s1QR = MInvoiceNumberS + sReYear + sReDay + sMRandomNumber + sASalesAmount16 + sATotalAmount16 + sMBIdentifier + sMSIdentifier + GoAESb64(MInvoiceNumberS + sMRandomNumber, sCompanyKey) + ":**********:" + s0401DCount + ":" + s0401DCount + ":1:";
                string s1qrFileName = sTempMapPath + MInvoiceNumberS + "Q1R" + sPgSN + ".png";
                MkQR4(sPgSN, s1QR, s1qrFileName);

                #endregion 生成QR1

                #region 生成QR2

                string s2QR = "**";
                if (dvResult0401D != null)
                {
                    for (int i = 0; i < dvResult0401D.Count; i++)
                    {
                        s2QR += Convert.ToString(dvResult0401D.Table.Rows[i]["DDescription"]) + ":" + Convert.ToString(dvResult0401D.Table.Rows[i]["DQuantity"]) + ":" + Convert.ToDouble(dvResult0401D.Table.Rows[i]["DUnitPrice"]).ToString("0") + ":";
                    }
                }
                string s2qrFileName = sTempMapPath + MInvoiceNumberS + "Q2R" + sPgSN + ".png";
                //MkQR4(sPgSN, s2QR, s2qrFileName);
                MkQR6(sPgSN, s2QR, s2qrFileName);

                #endregion 生成QR2

                #region 發票內容

                Document doc = new Document(new iTextSharp.text.Rectangle(162f, 225f), 0, 0, 0, 0);
                string sPDFpath = sPdfPath + MInvoiceNumberS + ".pdf";
                try
                {
                    PdfWriter.GetInstance(doc, new FileStream(sPDFpath, FileMode.Create));
                }
                catch (Exception ex)
                {
                    //檔案被開啟
                    GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, MInvoiceNumberS, "[使用者正在開啟這個DPF !!][" + sPDFpath + "]" + ex.ToString(), "", 41);
                }
                doc.Open();
                BaseFont bfChinese = PublicMethodFramework35.Repositoies.BaseFontType();
                iTextSharp.text.Font ChFont4 = new iTextSharp.text.Font(bfChinese, 4);
                iTextSharp.text.Font ChFont6 = new iTextSharp.text.Font(bfChinese, 6);
                iTextSharp.text.Font ChFont7 = new iTextSharp.text.Font(bfChinese, 7);
                iTextSharp.text.Font ChFont8 = new iTextSharp.text.Font(bfChinese, 8);
                iTextSharp.text.Font ChFont8B = new iTextSharp.text.Font(bfChinese, 8, 1);
                iTextSharp.text.Font ChFont9 = new iTextSharp.text.Font(bfChinese, 9);
                iTextSharp.text.Font ChFont9B = new iTextSharp.text.Font(bfChinese, 9, 1);
                iTextSharp.text.Font ChFont10 = new iTextSharp.text.Font(bfChinese, 10);
                iTextSharp.text.Font ChFont11 = new iTextSharp.text.Font(bfChinese, 11);
                iTextSharp.text.Font ChFont12 = new iTextSharp.text.Font(bfChinese, 12);
                iTextSharp.text.Font ChFont13 = new iTextSharp.text.Font(bfChinese, 13);
                iTextSharp.text.Font ChFont14 = new iTextSharp.text.Font(bfChinese, 14);
                iTextSharp.text.Font ChFont15 = new iTextSharp.text.Font(bfChinese, 15);
                iTextSharp.text.Font ChFont15B = new iTextSharp.text.Font(bfChinese, 15, 1);
                iTextSharp.text.Font ChFont16 = new iTextSharp.text.Font(bfChinese, 16);
                iTextSharp.text.Font ChFont16B = new iTextSharp.text.Font(bfChinese, 16, 1);
                iTextSharp.text.Font ChFont17 = new iTextSharp.text.Font(bfChinese, 17);
                iTextSharp.text.Font ChFont17B = new iTextSharp.text.Font(bfChinese, 17, 1);
                iTextSharp.text.Font ChFont18 = new iTextSharp.text.Font(bfChinese, 18);
                iTextSharp.text.Font ChFont18B = new iTextSharp.text.Font(bfChinese, 18, 1);
                iTextSharp.text.Font ChFont19 = new iTextSharp.text.Font(bfChinese, 19);
                iTextSharp.text.Font ChFont19B = new iTextSharp.text.Font(bfChinese, 19, 1);
                iTextSharp.text.Font ChFont20 = new iTextSharp.text.Font(bfChinese, 20);
                iTextSharp.text.Font ChFont20B = new iTextSharp.text.Font(bfChinese, 20, 1);
                iTextSharp.text.Font ChFont21B = new iTextSharp.text.Font(bfChinese, 21, 1);
                iTextSharp.text.Font ChFont22B = new iTextSharp.text.Font(bfChinese, 22, 1);

                Paragraph ParagraphW1 = new Paragraph();
                ParagraphW1.Leading = 23;
                ParagraphW1.Alignment = Element.ALIGN_CENTER;

                Phrase ParagraphWa011 = new Phrase(sCOM, ChFont20B);
                Phrase ParagraphWa014 = new Phrase("         ", ChFont4);
                ParagraphW1.Add(ParagraphWa011);
                ParagraphW1.Add(ParagraphWa014);
                ParagraphW1.Add("\n");

                string sProveName10 = "電子發票證明聯";
                string sProveName11 = "補印";
                if (sKind == "1")
                {
                    Phrase ParagraphWa021 = new Phrase(sProveName10, ChFont19B);
                    Phrase ParagraphWa022 = new Phrase("            ", ChFont4);
                    ParagraphW1.Add(ParagraphWa021);
                    ParagraphW1.Add(ParagraphWa022);
                }
                else if (sKind == "3")
                {
                    Phrase ParagraphWa021 = new Phrase(sProveName10, ChFont16B);
                    Phrase ParagraphWa022 = new Phrase(sProveName11, ChFont16B);
                    Phrase ParagraphWa023 = new Phrase("               ", ChFont4);
                    ParagraphW1.Add(ParagraphWa021);
                    ParagraphW1.Add(ParagraphWa022);
                    ParagraphW1.Add(ParagraphWa023);
                }

                doc.Add(ParagraphW1);

                doc.Add(new Paragraph(3f, "\n", ChFont9B));

                Paragraph ParagraphW2 = new Paragraph();
                ParagraphW2.Leading = 17;
                ParagraphW2.Alignment = Element.ALIGN_CENTER;

                string sYM = sReYear + "年" + sReMonth;
                Phrase ParagraphWa031 = new Phrase(sYM, ChFont18B);
                Phrase ParagraphWa032 = new Phrase("      ", ChFont4);
                ParagraphW2.Add(ParagraphWa031);
                ParagraphW2.Add(ParagraphWa032);
                ParagraphW2.Add("\n");

                string sMInvoiceNumber = MInvoiceNumberS.Substring(0, 2) + "-" + MInvoiceNumberS.Substring(2, 8);
                Phrase ParagraphWa041 = new Phrase(sMInvoiceNumber, ChFont18B);
                Phrase ParagraphWa042 = new Phrase("      ", ChFont4);
                ParagraphW2.Add(ParagraphWa041);
                ParagraphW2.Add(ParagraphWa042);
                ParagraphW2.Add("\n");
                doc.Add(ParagraphW2);

                string sYMDhms = sMInvoiceDate.Substring(0, 4) + "-" + sMInvoiceDate.Substring(4, 2) + "-" + sMInvoiceDate.Substring(6, 2) + " " + sMInvoiceTime;
                doc.Add(new Paragraph(10f, sYMDhms, ChFont9B));
                string sMRandomNumberA = "隨機碼:" + sMRandomNumber + "             " + "總計:" + sATotalAmount;
                doc.Add(new Paragraph(10f, sMRandomNumberA, ChFont9B));

                string sMSIdentifierB = "";
                if (sMBIdentifier == "0000000000")
                { sMSIdentifierB = "賣方:" + sMSIdentifier; }
                else
                { sMSIdentifierB = "賣方:" + sMSIdentifier + "          " + "買方:" + sMBIdentifier; }

                doc.Add(new Paragraph(10f, sMSIdentifierB, ChFont9B));

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(s1dimFileName);
                image.Alignment = iTextSharp.text.Image.LEFT_ALIGN;
                image.ScaleToFit(144f, 150f);
                doc.Add(image);

                iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(s1qrFileName);
                image1.Alignment = iTextSharp.text.Image.LEFT_ALIGN;
                image1.ScaleToFit(55f, 55f);

                doc.Add(image1);

                iTextSharp.text.Image image2 = iTextSharp.text.Image.GetInstance(s2qrFileName);
                image2.SetAbsolutePosition(80f, 32f);
                image2.ScaleToFit(55f, 55f);
                doc.Add(image2);

                Paragraph ParagraphW9 = new Paragraph();
                ParagraphW9.Leading = 10;
                ParagraphW9.Alignment = Element.ALIGN_LEFT;
                Phrase ParagraphWa091 = new Phrase(sCompanyName + "   序:" + ss0401SN, ChFont9B);
                Phrase ParagraphWa091n = new Phrase("\n", ChFont9B);
                Phrase ParagraphWa092 = new Phrase("退貨憑電子發票證明聯正本辦理", ChFont9B);
                ParagraphW9.Add(ParagraphWa091);
                ParagraphW9.Add(ParagraphWa091n);
                ParagraphW9.Add(ParagraphWa092);
                doc.Add(ParagraphW9);

                doc.NewPage();

                doc.Close();
                doc.Dispose();

                if (sKind == "1")
                {
                    //發票明細聯
                    CallMkPDFinvoD(sMBName, MInvoiceNumberS, sCompanyName, sCompanyTel, sATotalAmount, sASalesAmount, sAFreeTaxSalesAmount, sATaxAmount, sATaxTypeN);
                }

                #endregion 發票內容

                File.Delete(s1dimFileName);
                File.Delete(s1qrFileName);
                File.Delete(s2qrFileName);

                CallExPdfYN(MInvoiceNumberS, sTBKind, "1");
            }

            public void CallExPdfYN(string MInvoiceNumberS, string sTBKind, string sCuKind)
            {
                System.Collections.Hashtable data = new System.Collections.Hashtable();
                data["ExPdfYN"] = "Y";
                if (sCuKind == "1")
                {
                    UpdateData(sTBKind, data, "MInvoiceNumber", MInvoiceNumberS);
                }
                else
                {
                    UpdateData(sTBKind, data, "MAllowanceNumber", MInvoiceNumberS);
                }
            }

            public void CallMkPDFinvoD(string sMBName, string MInvoiceNumberS, string sCompanyName, string sCompanyTel, string sATotalAmount, string sASalesAmount, string sAFreeTaxSalesAmount, string sATaxAmount, string sATaxTypeN)
            {
                string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                #region 取資料

                DataView dvResult0401D = null;
                DataView dvResultTemp = null;
                string s0401DCount = "0";
                string sPdfPath = "";

                dvResult0401D = Kind1SelectTbl2("DDescription,DQuantity,DUnitPrice,DAmount,DSequenceNumber", "A0401D", "MInvoiceNumber='" + MInvoiceNumberS + "'", "DSequenceNumber", "ASC");
                if (dvResult0401D != null)
                {
                    s0401DCount = dvResult0401D.Count.ToString();
                }
                else
                {
                    dvResult0401D = Kind1SelectTbl2("DDescription,DQuantity,DUnitPrice,DAmount,DSequenceNumber", "C0401D", "MInvoiceNumber='" + MInvoiceNumberS + "'", "DSequenceNumber", "ASC");
                    if (dvResult0401D != null)
                    {
                        s0401DCount = dvResult0401D.Count.ToString();
                    }
                    else
                        throw new Exception("發票：" + MInvoiceNumberS + " 無法取得明細內容");
                }

                dvResultTemp = Kind1SelectTbl2("eName", "SParameter", "eKind='eMapPaths' AND eSN='2'", "", "");
                if (dvResultTemp != null)
                {
                    sPdfPath = Convert.ToString(dvResultTemp.Table.Rows[0][0]);
                }

                #endregion 取資料

                #region 明細內容

                #region 發票紙張尺寸設定相關參數 20171107 by俊晨

                //pdf 預設寬度 數字越大越寬
                float defaultDocWidth = 162f;
                //pdf 預設高度 數字越大越高  不含明細
                float defaultDocHeight = 255f;
                //每次增加一行欲增加數值
                float stepAdd = 7f;
                //每次增加一行欲增加數值 筆明細需增加的高度
                float stepDetailsAdd = 40f;
                // 總計所需高度25f
                float detailsTotalHeight = 43f;

                #region 設定pdf紙張大小 依照資料多寡

                //取明細資料 依明細筆數增加紙張長度 20171114新增判斷多行判斷
                Dictionary<int, List<string>> sDetailsList = new Dictionary<int, List<string>>();
                if (dvResult0401D.Count > 0)
                {
                    for (int i = 0; i < dvResult0401D.Count; i++)
                    {
                        defaultDocHeight += stepDetailsAdd;
                        string detailsItem = Convert.ToString(dvResult0401D.Table.Rows[i]["DDescription"]);
                        sDetailsList[i] = this.ToListCutDownString(detailsItem, 12, 12);
                        if (sDetailsList[i].Count > 0)
                        {
                            foreach (var item in sDetailsList[i])
                                defaultDocHeight += stepAdd;
                        }
                    }
                }

                defaultDocHeight += detailsTotalHeight;
                defaultDocHeight = 255f;

                #endregion 設定pdf紙張大小 依照資料多寡

                #endregion 發票紙張尺寸設定相關參數 20171107 by俊晨

                //Document doc = new Document(new iTextSharp.text.Rectangle(162f, 225f), 0, 0, 0, 0);
                Document doc = new Document(new iTextSharp.text.Rectangle(defaultDocWidth, defaultDocHeight), 0, 0, 0, 0);
                string sPDFpath = sPdfPath + MInvoiceNumberS + ".pdf";
                try
                {
                    PdfWriter.GetInstance(doc, new FileStream(sPDFpath, FileMode.Create));
                }
                catch (Exception ex)
                {
                    //檔案被開啟
                    GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, MInvoiceNumberS, "[使用者正在開啟這個DPF !!][" + sPDFpath + "]" + ex.ToString(), "", 41);
                }
                doc.Open();
                BaseFont bfChinese = PublicMethodFramework35.Repositoies.BaseFontType();
                iTextSharp.text.Font ChFont4 = new iTextSharp.text.Font(bfChinese, 4);
                iTextSharp.text.Font ChFont6 = new iTextSharp.text.Font(bfChinese, 6);
                iTextSharp.text.Font ChFont7 = new iTextSharp.text.Font(bfChinese, 7);
                iTextSharp.text.Font ChFont8 = new iTextSharp.text.Font(bfChinese, 8);
                iTextSharp.text.Font ChFont8B = new iTextSharp.text.Font(bfChinese, 8, 1);
                iTextSharp.text.Font ChFont9 = new iTextSharp.text.Font(bfChinese, 9);
                iTextSharp.text.Font ChFont9B = new iTextSharp.text.Font(bfChinese, 9, 1);
                iTextSharp.text.Font ChFont10 = new iTextSharp.text.Font(bfChinese, 10);
                iTextSharp.text.Font ChFont11 = new iTextSharp.text.Font(bfChinese, 11);
                iTextSharp.text.Font ChFont11B = new iTextSharp.text.Font(bfChinese, 11, 1);
                iTextSharp.text.Font ChFont12 = new iTextSharp.text.Font(bfChinese, 12);
                iTextSharp.text.Font ChFont12B = new iTextSharp.text.Font(bfChinese, 12, 1);
                iTextSharp.text.Font ChFont13 = new iTextSharp.text.Font(bfChinese, 13);
                iTextSharp.text.Font ChFont14 = new iTextSharp.text.Font(bfChinese, 14);
                iTextSharp.text.Font ChFont14B = new iTextSharp.text.Font(bfChinese, 14, 1);
                iTextSharp.text.Font ChFont15 = new iTextSharp.text.Font(bfChinese, 15);
                iTextSharp.text.Font ChFont15B = new iTextSharp.text.Font(bfChinese, 15, 1);
                iTextSharp.text.Font ChFont16 = new iTextSharp.text.Font(bfChinese, 16);
                iTextSharp.text.Font ChFont16B = new iTextSharp.text.Font(bfChinese, 16, 1);
                iTextSharp.text.Font ChFont17 = new iTextSharp.text.Font(bfChinese, 17);
                iTextSharp.text.Font ChFont17B = new iTextSharp.text.Font(bfChinese, 17, 1);
                iTextSharp.text.Font ChFont18 = new iTextSharp.text.Font(bfChinese, 18);
                iTextSharp.text.Font ChFont18B = new iTextSharp.text.Font(bfChinese, 18, 1);
                iTextSharp.text.Font ChFont19 = new iTextSharp.text.Font(bfChinese, 19);
                iTextSharp.text.Font ChFont19B = new iTextSharp.text.Font(bfChinese, 19, 1);
                iTextSharp.text.Font ChFont20 = new iTextSharp.text.Font(bfChinese, 20);
                iTextSharp.text.Font ChFont20B = new iTextSharp.text.Font(bfChinese, 20, 1);
                iTextSharp.text.Font ChFont21B = new iTextSharp.text.Font(bfChinese, 21, 1);
                iTextSharp.text.Font ChFont22B = new iTextSharp.text.Font(bfChinese, 22, 1);

                if (!string.IsNullOrEmpty(sMBName) && !sMBName.Equals("0000"))
                {
                    doc.Add(new Paragraph(10f, "買方客戶：" + "\n\n", ChFont9B));
                    doc.Add(new Paragraph(10f, sMBName + "\n\n", ChFont9B));
                }

                doc.Add(new Paragraph(10f, sCompanyName + "    " + sCompanyTel, ChFont9B));
                doc.Add(new Paragraph(10f, "發票號碼:" + MInvoiceNumberS + "\n\n", ChFont9B));

                //old
                //if (dvResult0401D != null)
                //{
                //    for (int i = 0; i < dvResult0401D.Count; i++)
                //    {
                //        //doc.Add(new Paragraph(10f, Convert.ToString(dvResult0401D.Table.Rows[i]["DDescription"]) + "\n", ChFont9B));
                //        //doc.Add(new Paragraph(10f, "金額 : " + Convert.ToDouble(dvResult0401D.Table.Rows[i]["DUnitPrice"]).ToString("0") + "  數量 : " + Convert.ToString(dvResult0401D.Table.Rows[i]["DQuantity"]) + "\n", ChFont9B));
                //        doc.Add(new Paragraph(10f, Convert.ToString(dvResult0401D.Table.Rows[i]["DDescription"]) + "\n", ChFont9B));
                //        doc.Add(new Paragraph(10f, "單價 : " + Convert.ToDouble(dvResult0401D.Table.Rows[i]["DUnitPrice"]).ToString("0.00") + "  數量 : " + Convert.ToString(dvResult0401D.Table.Rows[i]["DQuantity"]) + "  金額 : " + Convert.ToString(dvResult0401D.Table.Rows[i]["DAmount"]) + "\n", ChFont9B));
                //    }
                //}

                if (dvResult0401D != null)
                {
                    doc.Add(new Paragraph(10f, "品名     單價     數量    金額 :(含稅)\n", ChFont9B));
                    //for (int i = 0; i < dvResult0401Dbb.Count; i++)
                    //{
                    foreach (var di in sDetailsList as Dictionary<int, List<string>>)
                    {
                        foreach (var itemStr in di.Value as List<string>)
                        {
                            doc.Add(new Paragraph(12f, itemStr + "\n", ChFont9B));
                        }
                        var uniPrice = Convert.ToDouble(dvResult0401D.Table.Rows[di.Key]["DUnitPrice"]).ToString("0.00");
                        var quantity = Convert.ToDouble(dvResult0401D.Table.Rows[di.Key]["DQuantity"]).ToString();
                        var amount = Convert.ToDouble(dvResult0401D.Table.Rows[di.Key]["DAmount"]).ToString();

                        uniPrice = AddEmptyStringBefore(22, uniPrice);
                        quantity = AddEmptyStringBefore(13, quantity);
                        amount = AddEmptyStringBefore((amount.Length == 1 ? 11 : 13), amount);
                        //doc.Add(new Paragraph(10f, "單價 : " + Convert.ToDouble(dvResult0401Dbb.Table.Rows[di.Key]["DUnitPrice"]).ToString("0.00") + "  數量 : " + Convert.ToString(dvResult0401Dbb.Table.Rows[di.Key]["DQuantity"]) + "  金額 : " + Convert.ToString(dvResult0401Dbb.Table.Rows[di.Key]["DAmount"]) + "\n", ChFont9B));
                        string pirceInfo = string.Concat(uniPrice, quantity, amount);
                        doc.Add(new Paragraph(10f, pirceInfo, ChFont9B));
                    }
                    //}
                }

                //doc.Add(new Paragraph(14f, "共 " + dvResult0401D.Count.ToString() + "項  合計 $" + sATotalAmount + "\n", ChFont11B));
                //doc.Add(new Paragraph(10f, "應稅額:" + sASalesAmount + " 免稅額:" + sAFreeTaxSalesAmount + " 稅額:" + sATaxAmount + "\n", ChFont9B));
                //doc.Add(new Paragraph(10f, "\n", ChFont9B));

                doc.Add(new Paragraph(10f, "應稅額:" + sASalesAmount + " 免稅額:" + sAFreeTaxSalesAmount + " 稅額:" + sATaxAmount + "\n", ChFont9B));
                doc.Add(new Paragraph(14f, "總計 $" + sATotalAmount + "\n", ChFont11B));
                doc.Add(new Paragraph(14f, "共 " + dvResult0401D.Count.ToString() + "項  合計 $" + sATotalAmount + "   (" + sATaxTypeN + ")" + "\n", ChFont9B));
                doc.Add(new Paragraph(10f, "\n", ChFont9B));

                DataView dvResultTemp2 = Kind1SelectTbl2("eName", "SParameter", "eKind='eActions'", "eSN", "ASC");
                if (dvResultTemp2 != null)
                {
                    for (int i = 0; i < dvResultTemp2.Count; i++)
                    {
                        doc.Add(new Paragraph(14f, Convert.ToString(dvResultTemp2.Table.Rows[i][0]) + "\n", ChFont11B));
                    }
                }

                doc.NewPage();
                doc.Close();
                doc.Dispose();

                #endregion 明細內容
            }

            public string GoAESb64(string plainText, string AESKey)
            {
                byte[] bytes = Encoding.Default.GetBytes(plainText);
                ICryptoTransform transform = new RijndaelManaged
                {
                    KeySize = 0x80,
                    Key = this.conHexToByte(AESKey),
                    BlockSize = 0x80,
                    IV = Convert.FromBase64String("Dt8lyToo17X/XkXaQvihuA==")
                }.CreateEncryptor();
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
                stream2.Write(bytes, 0, bytes.Length);
                stream2.FlushFinalBlock();
                stream2.Close();
                return Convert.ToBase64String(stream.ToArray());
            }

            private byte[] conHexToByte(string hexString)
            {
                byte[] buffer = new byte[hexString.Length / 2];
                int index = 0;
                for (int i = 0; i < hexString.Length; i += 2)
                {
                    int num3 = Convert.ToInt32(hexString.Substring(i, 2), 0x10);
                    buffer[index] = BitConverter.GetBytes(num3)[0];
                    index++;
                }
                return buffer;
            }

            private void GoGetCode39(string sMapPath, string strSource)
            {
                Bitmap b = GetCode39(strSource);
                b.Save(sMapPath, System.Drawing.Imaging.ImageFormat.Png);
            }

            private Bitmap GetCode39(string strSource)
            {
                int x = 0;
                int y = 0;
                int BarCodeHeight = 30;
                int WidLength = 2;
                int NarrowLength = 1;
                int intSourceLength = strSource.Length;
                string strEncode = "010010100";
                string AlphaBet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%*";

                string[] Code39 = //Code39的各字母對應碼
             {
/**//* 0 */ "000110100",
/**//* 1 */ "100100001",
/**//* 2 */ "001100001",
/**//* 3 */ "101100000",
/**//* 4 */ "000110001",
/**//* 5 */ "100110000",
/**//* 6 */ "001110000",
/**//* 7 */ "000100101",
/**//* 8 */ "100100100",
/**//* 9 */ "001100100",
/**//* A */ "100001001",
/**//* B */ "001001001",
/**//* C */ "101001000",
/**//* D */ "000011001",
/**//* E */ "100011000",
/**//* F */ "001011000",
/**//* G */ "000001101",
/**//* H */ "100001100",
/**//* I */ "001001100",
/**//* J */ "000011100",
/**//* K */ "100000011",
/**//* L */ "001000011",
/**//* M */ "101000010",
/**//* N */ "000010011",
/**//* O */ "100010010",
/**//* P */ "001010010",
/**//* Q */ "000000111",
/**//* R */ "100000110",
/**//* S */ "001000110",
/**//* T */ "000010110",
/**//* U */ "110000001",
/**//* V */ "011000001",
/**//* W */ "111000000",
/**//* X */ "010010001",
/**//* Y */ "110010000",
/**//* Z */ "011010000",
/**//* - */ "010000101",
/**//* . */ "110000100",
/**//*' '*/ "011000100",
/**//* $ */ "010101000",
/**//* / */ "010100010",
/**//* + */ "010001010",
/**//* % */ "000101010",
/**//* * */ "010010100"
};

                strSource = strSource.ToUpper();

                //實作圖片
                Bitmap objBitmap = new Bitmap(((WidLength * 3 + NarrowLength * 7) * (intSourceLength + 2)) + (x * 2), BarCodeHeight + (y * 2));
                Graphics objGraphics = Graphics.FromImage(objBitmap);
                //宣告GDI+繪圖介面
                //填上底色
                objGraphics.FillRectangle(Brushes.White, 0, 0, objBitmap.Width, objBitmap.Height);

                for (int i = 0; i < intSourceLength; i++)
                {
                    //檢查是否有非法字元
                    //if (AlphaBet.IndexOf(strSource[i]) == -1 || strSource[i] == '*')
                    if (AlphaBet.IndexOf(strSource[i]) == -1)
                    {
                        objGraphics.DrawString("含有非法字元", SystemFonts.DefaultFont, Brushes.Red, x, y);
                        return objBitmap;
                    }

                    //查表編碼
                    strEncode = string.Format("{0}0{1}", strEncode,
                    Code39[AlphaBet.IndexOf(strSource[i])]);
                }

                strEncode = string.Format("{0}0010010100", strEncode); //補上結束符號 *

                int intEncodeLength = strEncode.Length; //編碼後長度
                int intBarWidth;
                for (int i = 0; i < intEncodeLength; i++) //依碼畫出Code39 BarCode
                {
                    intBarWidth = strEncode[i] == '1' ? WidLength : NarrowLength;
                    objGraphics.FillRectangle(i % 2 == 0 ? Brushes.Black : Brushes.White,
                    x, y, intBarWidth, BarCodeHeight);
                    x += intBarWidth;
                }

                return objBitmap;
            }

            public void GetCode394(string filePath, string strSource)
            {
                EncodingOptions encodeOption = new EncodingOptions();
                encodeOption.Height = 32;
                encodeOption.Width = 140;
                ZXing.BarcodeWriter wr = new BarcodeWriter();
                wr.Options = encodeOption;
                wr.Format = BarcodeFormat.CODE_39;
                Bitmap img = wr.Write(strSource);
                img.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            }

            public void MkQR4(string sPgSN, string content, string sMapPath)
            {
                ZXing.QrCode.QrCodeEncodingOptions qrEncodeOption = new ZXing.QrCode.QrCodeEncodingOptions();
                qrEncodeOption.CharacterSet = "UTF-8";

                qrEncodeOption.Height = 75;
                qrEncodeOption.Width = 75;
                qrEncodeOption.Margin = 1;

                qrEncodeOption.Margin = 0;
                ZXing.BarcodeWriter wr = new BarcodeWriter();
                wr.Format = BarcodeFormat.QR_CODE;
                wr.Options = qrEncodeOption;
                Bitmap img = wr.Write(content);
                img.Save(sMapPath, System.Drawing.Imaging.ImageFormat.Png);
                //return sMap;
            }

            public void MkQR6(string sPgSN, string content, string sMapPath)
            {
                Bitmap img = ToBitmap(content, 75, 75);
                img.Save(sMapPath, System.Drawing.Imaging.ImageFormat.Png);
            }

            public static Bitmap ToBitmap(string content, int width = 400, int height = 400)
            {
                var hints = new Dictionary<EncodeHintType, object> { { EncodeHintType.CHARACTER_SET, "UTF-8" } };

                var matrix = new MultiFormatWriter().encode(content, BarcodeFormat.QR_CODE, width, height, hints);

                matrix = CutWhiteBorder2(matrix);
                var barcodeWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,

                    Options = new QrCodeEncodingOptions
                    {
                        Margin = 0,
                        Height = height,
                        Width = width
                    }
                };

                barcodeWriter.Options.Width = matrix.Width;
                barcodeWriter.Options.Height = matrix.Height;
                var codeImage = barcodeWriter.Write(matrix);
                return codeImage;
            }

            private static BitMatrix CutWhiteBorder2(BitMatrix matrix)
            {
                int[] rec = matrix.getEnclosingRectangle();
                int resWidth = rec[2] + 1;
                int resHeight = rec[3] + 1;
                BitMatrix resMatrix = new BitMatrix(resWidth + 1, resHeight + 1);
                resMatrix.clear();
                for (int i = 0; i < resWidth; i++)
                {
                    for (int j = 0; j < resHeight; j++)
                    {
                        if (matrix[i + rec[0], j + rec[1]])
                        {
                            resMatrix.flip(i + 1, j + 1);
                        }
                    }
                }
                return resMatrix;
            }

            public string DoAllInOne()
            {
                return "test";
            }

            public void CallMkPDFaw(string MAllowanceNumber)
            {
                string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                #region 取資料

                DataView dvResult0401H = null;
                DataView dvResult0401D = null;
                DataView dvResultTemp = null;
                string MSIdentifier = "";
                string MBIdentifier = "";
                string MAllowanceDate = "";
                string MSName = "";
                string MBName = "";
                string s0401DCount = "0";
                string sPdfPath = "";
                string sTBKind = "";

                dvResult0401H = Kind1SelectTbl2("", "B0401H", "MAllowanceNumber='" + MAllowanceNumber + "'", "", "");
                if (dvResult0401H != null)
                {
                    MSIdentifier = dvResult0401H.Table.Rows[0]["MSIdentifier"].ToString();
                    MBIdentifier = dvResult0401H.Table.Rows[0]["MBIdentifier"].ToString();
                    MAllowanceDate = dvResult0401H.Table.Rows[0]["MAllowanceDate"].ToString();
                    MSName = dvResult0401H.Table.Rows[0]["MSName"].ToString();
                    MBName = dvResult0401H.Table.Rows[0]["MBName"].ToString();

                    dvResult0401D = Kind1SelectTbl2("", "B0401D", "MAllowanceNumber='" + MAllowanceNumber + "'", "DAllowanceSequenceNumber", "ASC");
                    if (dvResult0401D != null)
                    {
                        s0401DCount = dvResult0401D.Count.ToString();
                    }
                    sTBKind = "B0401H";
                }
                else
                {
                    dvResult0401H = Kind1SelectTbl2("", "D0401H", "MAllowanceNumber='" + MAllowanceNumber + "'", "", "");
                    if (dvResult0401H != null)
                    {
                        MSIdentifier = dvResult0401H.Table.Rows[0]["MSIdentifier"].ToString();
                        MBIdentifier = dvResult0401H.Table.Rows[0]["MBIdentifier"].ToString();
                        MAllowanceDate = dvResult0401H.Table.Rows[0]["MAllowanceDate"].ToString();
                        MSName = dvResult0401H.Table.Rows[0]["MSName"].ToString().Trim();
                        MBName = dvResult0401H.Table.Rows[0]["MBName"].ToString().Trim();
                        dvResult0401D = Kind1SelectTbl2("", "D0401D", "MAllowanceNumber='" + MAllowanceNumber + "'", "DAllowanceSequenceNumber", "ASC");
                        if (dvResult0401D != null)
                        {
                            s0401DCount = dvResult0401D.Count.ToString();
                        }
                        sTBKind = "D0401H";
                    }
                    else
                    {
                        //return;
                    }
                }

                //銷貨退回折讓
                dvResultTemp = Kind1SelectTbl2("eName", "SParameter", "eKind='eMapPaths' AND eSN='5'", "", "");
                if (dvResultTemp != null)
                {
                    sPdfPath = Convert.ToString(dvResultTemp.Table.Rows[0][0]);
                }

                #endregion 取資料

                #region 銷貨退回折讓內容

                Document doc = new Document(new iTextSharp.text.Rectangle(162f, 600f), 0, 0, 0, 0);
                string sPDFpath = sPdfPath + MAllowanceNumber + ".pdf";
                try
                {
                    PdfWriter.GetInstance(doc, new FileStream(sPDFpath, FileMode.Create));
                }
                catch (Exception ex)
                {
                    //檔案被開啟
                    GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, MAllowanceNumber, "[使用者正在開啟這個DPF !!][" + sPDFpath + "]" + ex.ToString(), "", 41);
                }

                try
                {
                    doc.Open();
                    BaseFont bfChinese = PublicMethodFramework35.Repositoies.BaseFontType(); iTextSharp.text.Font ChFont4 = new iTextSharp.text.Font(bfChinese, 4);
                    iTextSharp.text.Font ChFont6 = new iTextSharp.text.Font(bfChinese, 6);
                    iTextSharp.text.Font ChFont7 = new iTextSharp.text.Font(bfChinese, 7);
                    iTextSharp.text.Font ChFont8 = new iTextSharp.text.Font(bfChinese, 8);
                    iTextSharp.text.Font ChFont8B = new iTextSharp.text.Font(bfChinese, 8, 1);
                    iTextSharp.text.Font ChFont9 = new iTextSharp.text.Font(bfChinese, 9);
                    iTextSharp.text.Font ChFont9B = new iTextSharp.text.Font(bfChinese, 9, 1);
                    iTextSharp.text.Font ChFont10 = new iTextSharp.text.Font(bfChinese, 10);
                    iTextSharp.text.Font ChFont11 = new iTextSharp.text.Font(bfChinese, 11);
                    iTextSharp.text.Font ChFont12 = new iTextSharp.text.Font(bfChinese, 12);
                    iTextSharp.text.Font ChFont12B = new iTextSharp.text.Font(bfChinese, 12, 1);
                    iTextSharp.text.Font ChFont13 = new iTextSharp.text.Font(bfChinese, 13);
                    iTextSharp.text.Font ChFont14 = new iTextSharp.text.Font(bfChinese, 14);
                    iTextSharp.text.Font ChFont15 = new iTextSharp.text.Font(bfChinese, 15);
                    iTextSharp.text.Font ChFont15B = new iTextSharp.text.Font(bfChinese, 15, 1);
                    iTextSharp.text.Font ChFont16 = new iTextSharp.text.Font(bfChinese, 16);
                    iTextSharp.text.Font ChFont16B = new iTextSharp.text.Font(bfChinese, 16, 1);
                    iTextSharp.text.Font ChFont17 = new iTextSharp.text.Font(bfChinese, 17);
                    iTextSharp.text.Font ChFont17B = new iTextSharp.text.Font(bfChinese, 17, 1);
                    iTextSharp.text.Font ChFont18 = new iTextSharp.text.Font(bfChinese, 18);
                    iTextSharp.text.Font ChFont18B = new iTextSharp.text.Font(bfChinese, 18, 1);
                    iTextSharp.text.Font ChFont19 = new iTextSharp.text.Font(bfChinese, 19);
                    iTextSharp.text.Font ChFont19B = new iTextSharp.text.Font(bfChinese, 19, 1);
                    iTextSharp.text.Font ChFont20 = new iTextSharp.text.Font(bfChinese, 20);
                    iTextSharp.text.Font ChFont20B = new iTextSharp.text.Font(bfChinese, 20, 1);
                    iTextSharp.text.Font ChFont21B = new iTextSharp.text.Font(bfChinese, 21, 1);
                    iTextSharp.text.Font ChFont22B = new iTextSharp.text.Font(bfChinese, 22, 1);

                    Paragraph ParagraphW1 = new Paragraph();
                    ParagraphW1.Leading = 18;
                    ParagraphW1.Alignment = Element.ALIGN_LEFT;
                    Phrase ParagraphWa011 = new Phrase("電子發票銷貨退回", ChFont18B);
                    ParagraphW1.Add(ParagraphWa011);
                    ParagraphW1.Add("\n");
                    Phrase ParagraphWa012 = new Phrase("、進貨退出或折讓", ChFont18B);
                    ParagraphW1.Add(ParagraphWa012);

                    doc.Add(ParagraphW1);

                    Paragraph ParagraphW2 = new Paragraph();
                    ParagraphW2.Leading = 18;
                    ParagraphW2.Alignment = Element.ALIGN_CENTER;
                    Phrase ParagraphWa013 = new Phrase("證明單證明聯", ChFont18B);
                    Phrase ParagraphWa014 = new Phrase("         ", ChFont4);
                    ParagraphW2.Add(ParagraphWa013);
                    ParagraphW2.Add(ParagraphWa014);
                    ParagraphW2.Add("\n");

                    doc.Add(ParagraphW2);
                    doc.Add(new Paragraph(3f, "\n\n", ChFont9B));

                    Paragraph ParagraphW3 = new Paragraph();
                    ParagraphW3.Leading = 18;
                    ParagraphW3.Alignment = Element.ALIGN_CENTER;
                    string sMAllowanceDate = MAllowanceDate.Substring(0, 4) + "-" + MAllowanceDate.Substring(4, 2) + "-" + MAllowanceDate.Substring(6, 2);
                    Phrase ParagraphWa015 = new Phrase(sMAllowanceDate, ChFont18B);
                    Phrase ParagraphWa016 = new Phrase("         ", ChFont4);
                    ParagraphW3.Add(ParagraphWa015);
                    ParagraphW3.Add(ParagraphWa016);

                    doc.Add(ParagraphW3);
                    doc.Add(new Paragraph(3f, "\n\n", ChFont9B));

                    iTextSharp.text.Font ChFontW01 = ChFont12;

                    doc.Add(new Paragraph(3f, "\n", ChFontW01));

                    doc.Add(new Paragraph(10f, "折讓單號碼 : " + MAllowanceNumber, ChFontW01));
                    doc.Add(new Paragraph(3f, "\n", ChFontW01));
                    doc.Add(new Paragraph(3f, "\n", ChFontW01));

                    doc.Add(new Paragraph(10f, "賣方統編 : " + MSIdentifier, ChFontW01));
                    doc.Add(new Paragraph(3f, "\n", ChFontW01));
                    doc.Add(new Paragraph(3f, "\n", ChFontW01));

                    if (MSName.Length > 15)
                    {
                        doc.Add(new Paragraph(10f, "賣方名稱 : " + MSName.Substring(0, 6), ChFont9B));
                        doc.Add(new Paragraph(3f, "\n", ChFont9B));
                        doc.Add(new Paragraph(10f, MSName.Substring(6, MSName.Length - 6), ChFont9B));
                        doc.Add(new Paragraph(3f, "\n", ChFont9B));
                    }
                    else
                    {
                        doc.Add(new Paragraph(10f, "賣方名稱 : ", ChFont9B));
                        doc.Add(new Paragraph(3f, "\n", ChFont9B));
                        doc.Add(new Paragraph(10f, MSName, ChFont9B));
                        doc.Add(new Paragraph(3f, "\n", ChFont9B));
                    }

                    if (MBIdentifier == "0000000000")
                    { MBIdentifier = ""; }
                    doc.Add(new Paragraph(3f, "\n", ChFontW01));
                    doc.Add(new Paragraph(3f, "\n", ChFontW01));
                    doc.Add(new Paragraph(10f, "買方統編 : " + MBIdentifier, ChFontW01));
                    doc.Add(new Paragraph(3f, "\n", ChFontW01));

                    doc.Add(new Paragraph(10f, "買方名稱 : ", ChFont9B));
                    doc.Add(new Paragraph(3f, "\n", ChFont9B));

                    string s1 = MBName;
                    Int16 j2 = 16;//第1行
                    if (s1.Length > j2)
                    {
                        Int16 j1 = 16;//第2行以後

                        if (s1.Length > j2)
                        {
                            string s3 = s1.Substring(0, j2);
                            string s4 = s3;
                            while (s3.Length < (s4.Length + (j1 - s4.Length % j1)))
                            { s3 = s3 + " "; }
                            s1 = s3 + s1.Substring(j2, s1.Length - j2);
                        }
                        string s2 = s1;
                        while (s1.Length < (s2.Length + (j1 - s2.Length % j1)))
                        { s1 = s1 + " "; }

                        int k = 0;
                        while (k < s1.Length / j1)
                        {
                            doc.Add(new Paragraph(10f, s1.Substring(k * j1, j1 - s1.Length % j1).Trim(), ChFont9B));
                            k += 1;
                        }
                    }
                    else
                    {
                        if (MBName == "0000")
                        { MBName = ""; }
                        doc.Add(new Paragraph(10f, MBName, ChFont9B));
                    }

                    //if (MBName.Length > 15)
                    //{
                    //    doc.Add(new Paragraph(10f, "買方名稱 : " + MBName.Substring(0, 6), ChFont9B));
                    //    doc.Add(new Paragraph(3f, "\n", ChFont9B));
                    //    doc.Add(new Paragraph(10f, MBName.Substring(6, MBName.Length - 6), ChFont9B));

                    //}
                    //else
                    //{
                    //    if (MBName == "0000")
                    //    { MBName = ""; }
                    //    doc.Add(new Paragraph(10f, "買方名稱 : ", ChFont9B));
                    //    doc.Add(new Paragraph(3f, "\n", ChFont9B));
                    //    doc.Add(new Paragraph(10f, MBName, ChFont9B));

                    //}

                    doc.Add(new Paragraph(3f, "\n", ChFont9B));

                    doc.Add(new Paragraph(3f, "\n", ChFontW01));
                    doc.Add(new Paragraph(3f, "\n", ChFontW01));
                    doc.Add(new Paragraph(3f, "\n", ChFontW01));



                    if (dvResult0401D != null)
                    {
                        Int64 iDAmount = 0;
                        Int64 iDTax = 0;
                        //B2B折讓單  顯示單價、未稅、稅額、含稅
                        if (sTBKind == "B0401H")
                        {
                            for (int i = 0; i < dvResult0401D.Count; i++)
                            {
                                string sDTaxType = Convert.ToString(dvResult0401D.Table.Rows[i]["DTaxType"]);
                                string sDTaxTypeN = GetTaxTypeCodeByTaxTypeID(sDTaxType);

                                int sDQuantity = Convert.ToInt32(dvResult0401D.Table.Rows[i]["DQuantity"]);
                                string sDOriginalInvoiceDate = dvResult0401D.Table.Rows[i]["DOriginalInvoiceDate"].ToString().Substring(0, 4) + "-" + dvResult0401D.Table.Rows[i]["DOriginalInvoiceDate"].ToString().Substring(4, 2) + "-" + dvResult0401D.Table.Rows[i]["DOriginalInvoiceDate"].ToString().Substring(6, 2);
                                string sDunitPrice = Convert.ToDouble(dvResult0401D.Table.Rows[i]["DUnitPrice"]).ToString("0.00");
                                string sDTax = Convert.ToInt64(dvResult0401D.Table.Rows[i]["DTax"]).ToString();
                                string sDAmount = Convert.ToInt64(dvResult0401D.Table.Rows[i]["DAmount"]).ToString();

                                doc.Add(new Paragraph(10f, "發票號碼 : " + Convert.ToString(dvResult0401D.Table.Rows[i]["DOriginalInvoiceNumber"]) + "\n", ChFont12B));
                                doc.Add(new Paragraph(3f, "\n", ChFont9B));
                                doc.Add(new Paragraph(3f, "\n", ChFont9B));
                                doc.Add(new Paragraph(10f, "發票開立日期 : " + sDOriginalInvoiceDate + "\n", ChFont12B));
                                doc.Add(new Paragraph(3f, "\n", ChFont9B));
                                doc.Add(new Paragraph(10f, Convert.ToString(dvResult0401D.Table.Rows[i]["DOriginalDescription"]) + "\n", ChFont9B));
                                //增加顯示單價、未稅、稅額、含稅
                                string centerFirstEmpty = "               ";
                                string centerSecondEmpty = "                ";
                                int ssDunitPriceLength = sDunitPrice.Length;
                                int sDAmountLength = sDAmount.Length;

                                int emptyFirstLength = (centerFirstEmpty.Length - ssDunitPriceLength) + 1;
                                int emptySecondLength = (centerSecondEmpty.Length - sDAmountLength) + 1;
                                string setFirstEmpty = "";
                                string setSecondEmpty = "";
                                for (int j = 0; j < emptyFirstLength; j++)
                                {
                                    setFirstEmpty += " ";
                                }
                                for (int j = 0; j < emptySecondLength; j++)
                                {
                                    setSecondEmpty += " ";
                                }

                                //doc.Add(new Paragraph(10f, "數量、     單價、         未稅 \n", ChFont9B));
                                doc.Add(new Paragraph(10f, "   " + sDQuantity + setFirstEmpty + sDunitPrice + setSecondEmpty + sDAmount + "\n", ChFont9B));
                                doc.Add(new Paragraph(3f, "\n", ChFont9B));

                                //doc.Add(new Paragraph(10f, "稅額 \n", ChFont9B));
                                doc.Add(new Paragraph(10f, "稅額 " + sDTax + "        " + (Convert.ToDouble(sDAmount) + Convert.ToDouble(sDTax)).ToString() + "  " + sDTaxTypeN + "\n", ChFont9B));


                                doc.Add(new Paragraph(3f, "\n", ChFont9B));
                                doc.Add(new Paragraph(3f, "\n", ChFont9B));
                                //單價
                                iDAmount = iDAmount + Convert.ToInt64(dvResult0401D.Table.Rows[i]["DAmount"]);
                                //稅額
                                iDTax = iDTax + Convert.ToInt64(dvResult0401D.Table.Rows[i]["DTax"]);
                            }
                            doc.Add(new Paragraph(3f, "\n", ChFont9B));
                            doc.Add(new Paragraph(3f, "\n", ChFont9B));

                            doc.Add(new Paragraph(10f, "總    計 : " + (iDAmount + iDTax).ToString(), ChFont9B));
                            doc.Add(new Paragraph(3f, "\n", ChFont9B));
                            doc.Add(new Paragraph(10f, "未稅金額 : " + iDAmount.ToString(), ChFont9B));
                            doc.Add(new Paragraph(3f, "\n", ChFont9B));
                            doc.Add(new Paragraph(10f, "稅    額 : " + iDTax.ToString(), ChFont9B));
                            doc.Add(new Paragraph(3f, "\n", ChFont9B));

                        }
                        else
                        {
                            for (int i = 0; i < dvResult0401D.Count; i++)
                            {
                                string sDTaxType = Convert.ToString(dvResult0401D.Table.Rows[i]["DTaxType"]);
                                string sDTaxTypeN = GetTaxTypeCodeByTaxTypeID(sDTaxType);

                                string sDOriginalInvoiceDate = dvResult0401D.Table.Rows[i]["DOriginalInvoiceDate"].ToString().Substring(0, 4) + "-" + dvResult0401D.Table.Rows[i]["DOriginalInvoiceDate"].ToString().Substring(4, 2) + "-" + dvResult0401D.Table.Rows[i]["DOriginalInvoiceDate"].ToString().Substring(6, 2);
                                string sDQuantity = Convert.ToDouble(dvResult0401D.Table.Rows[i]["DUnitPrice"]).ToString("0.00");
                                doc.Add(new Paragraph(10f, "發票號碼 : " + Convert.ToString(dvResult0401D.Table.Rows[i]["DOriginalInvoiceNumber"]) + "\n", ChFont12B));
                                doc.Add(new Paragraph(3f, "\n", ChFont9B));
                                doc.Add(new Paragraph(3f, "\n", ChFont9B));
                                doc.Add(new Paragraph(10f, "發票開立日期 : " + sDOriginalInvoiceDate + "\n", ChFont12B));
                                doc.Add(new Paragraph(3f, "\n", ChFont9B));
                                doc.Add(new Paragraph(10f, Convert.ToString(dvResult0401D.Table.Rows[i]["DOriginalDescription"]) + "\n", ChFont9B));
                                doc.Add(new Paragraph(10f, "    " + Convert.ToString(dvResult0401D.Table.Rows[i]["DQuantity"]) + "    " + sDQuantity + "    " + Convert.ToString(dvResult0401D.Table.Rows[i]["DAmount"]) + "  " + sDTaxTypeN + "\n", ChFont9B));
                                doc.Add(new Paragraph(3f, "\n", ChFont9B));
                                doc.Add(new Paragraph(3f, "\n", ChFont9B));
                                iDAmount = iDAmount + Convert.ToInt64(dvResult0401D.Table.Rows[i]["DAmount"]);
                                iDTax = iDTax + Convert.ToInt64(dvResult0401D.Table.Rows[i]["DTax"]);
                            }
                            doc.Add(new Paragraph(3f, "\n", ChFont9B));
                            doc.Add(new Paragraph(3f, "\n", ChFont9B));

                            doc.Add(new Paragraph(10f, "總    計 : " + (iDAmount + iDTax).ToString(), ChFont9B));
                            doc.Add(new Paragraph(3f, "\n", ChFont9B));
                            doc.Add(new Paragraph(10f, "未稅金額 : " + iDAmount.ToString(), ChFont9B));
                            doc.Add(new Paragraph(3f, "\n", ChFont9B));
                            doc.Add(new Paragraph(10f, "稅    額 : " + iDTax.ToString(), ChFont9B));
                            doc.Add(new Paragraph(3f, "\n", ChFont9B));
                        }
                    }
                    else
                        throw new Exception(sTBKind + "：" + MAllowanceNumber + " 產生PDF時 查無明細資料 ");

                    doc.Add(new Paragraph(3f, "\n", ChFont9B));
                    doc.Add(new Paragraph(3f, "\n", ChFont9B));
                    doc.Add(new Paragraph(3f, "\n", ChFont9B));
                    doc.Add(new Paragraph(10f, "簽收人 : " + "\n", ChFont18B));
                    doc.Add(new Paragraph(36f, " " + "\n", ChFont18B));
                    doc.Add(new Paragraph(36f, " " + "\n", ChFont18B));
                    doc.Add(new Paragraph(36f, "." + "\n", ChFont18B));

                    doc.NewPage();
                    doc.Close();
                    doc.Dispose();

                    CallExPdfYN(MAllowanceNumber, sTBKind, "2");
                }
                catch (Exception ex)
                {
                    GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, MAllowanceNumber, "[MAllowanceDate 資料來源不正確 !!][" + sPDFpath + "]" + ex.ToString(), "", 41);
                }

                #endregion 銷貨退回折讓內容
            }

            public void MkDailyRrport()
            {
                MkDailyRrport1("Y");
                MkDailyRrport1("N");
            }

            public void MkDailyRrport1(string sStat)
            {
                string sPgSN = DateTime.Now.ToString("yyyyMMdd");
                string strTXT = "", strPath = "";
                DataView dvResultD = null;

                dvResultD = Kind1SelectTbl2("eName", "SParameter", "eKind='eMapPaths' AND eSN='4'", "", "");
                if (dvResultD != null)
                {
                    strPath = Convert.ToString(dvResultD.Table.Rows[0][0]);
                    dvResultD = null;
                }

                dvResultD = Kind1SelectTbl2("MInvoiceNumber", "C0401H", "(DATEDIFF(DAY, DataTime, GETDATE()) = 1) and ExXmlYN='" + sStat + "'", "MInvoiceNumber", "");
                if (dvResultD != null)
                {
                    for (int i = 0; i < dvResultD.Count; i++)
                    { strTXT += "C0401;" + Convert.ToString(dvResultD.Table.Rows[i][0]) + Environment.NewLine; }
                    dvResultD = null;
                }

                dvResultD = Kind1SelectTbl2("CancelInvoiceNumber", "C0501", "(DATEDIFF(DAY, DataTime, GETDATE()) = 1) and ExXmlYN='" + sStat + "'", "CancelInvoiceNumber", "");
                if (dvResultD != null)
                {
                    for (int i = 0; i < dvResultD.Count; i++)
                    { strTXT += "C0501;" + Convert.ToString(dvResultD.Table.Rows[i][0]) + Environment.NewLine; }
                    dvResultD = null;
                }

                dvResultD = Kind1SelectTbl2("VoidInvoiceNumber", "C0701", "(DATEDIFF(DAY, DataTime, GETDATE()) = 1) and ExXmlYN='" + sStat + "'", "VoidInvoiceNumber", "");
                if (dvResultD != null)
                {
                    for (int i = 0; i < dvResultD.Count; i++)
                    { strTXT += "C0701;" + Convert.ToString(dvResultD.Table.Rows[i][0]) + Environment.NewLine; }
                    dvResultD = null;
                }

                dvResultD = Kind1SelectTbl2("MAllowanceNumber", "D0401H", "(DATEDIFF(DAY, DataTime, GETDATE()) = 1) and ExXmlYN='" + sStat + "'", "MAllowanceNumber", "");
                if (dvResultD != null)
                {
                    for (int i = 0; i < dvResultD.Count; i++)
                    { strTXT += "D0401;" + Convert.ToString(dvResultD.Table.Rows[i][0]) + Environment.NewLine; }
                    dvResultD = null;
                }

                dvResultD = Kind1SelectTbl2("CancelAllowanceNumber", "D0501", "(DATEDIFF(DAY, DataTime, GETDATE()) = 1) and ExXmlYN='" + sStat + "'", "CancelAllowanceNumber", "");
                if (dvResultD != null)
                {
                    for (int i = 0; i < dvResultD.Count; i++)
                    { strTXT += "D0501;" + Convert.ToString(dvResultD.Table.Rows[i][0]) + Environment.NewLine; }
                    dvResultD = null;
                }

                dvResultD = Kind1SelectTbl2("MInvoiceNumber", "A0401H", "(DATEDIFF(DAY, DataTime, GETDATE()) = 1) and ExXmlYN='" + sStat + "'", "MInvoiceNumber", "");
                if (dvResultD != null)
                {
                    for (int i = 0; i < dvResultD.Count; i++)
                    { strTXT += "A0401;" + Convert.ToString(dvResultD.Table.Rows[i][0]) + Environment.NewLine; }
                    dvResultD = null;
                }

                dvResultD = Kind1SelectTbl2("MAllowanceNumber", "B0401H", "(DATEDIFF(DAY, DataTime, GETDATE()) = 1) and ExXmlYN='" + sStat + "'", "MAllowanceNumber", "");
                if (dvResultD != null)
                {
                    for (int i = 0; i < dvResultD.Count; i++)
                    { strTXT += "B0401;" + Convert.ToString(dvResultD.Table.Rows[i][0]) + Environment.NewLine; }
                    dvResultD = null;
                }

                dvResultD = Kind1SelectTbl2("CancelInvoiceNumber", "A0501", "(DATEDIFF(DAY, DataTime, GETDATE()) = 1) and ExXmlYN='" + sStat + "'", "CancelInvoiceNumber", "");
                if (dvResultD != null)
                {
                    for (int i = 0; i < dvResultD.Count; i++)
                    { strTXT += "A0501;" + Convert.ToString(dvResultD.Table.Rows[i][0]) + Environment.NewLine; }
                    dvResultD = null;
                }

                dvResultD = Kind1SelectTbl2("CancelAllowanceNumber", "B0501", "(DATEDIFF(DAY, DataTime, GETDATE()) = 1) and ExXmlYN='" + sStat + "'", "CancelAllowanceNumber", "");
                if (dvResultD != null)
                {
                    for (int i = 0; i < dvResultD.Count; i++)
                    { strTXT += "B0501;" + Convert.ToString(dvResultD.Table.Rows[i][0]) + Environment.NewLine; }
                    dvResultD = null;
                }

                dvResultD = Kind1SelectTbl2("RejectInvoiceNumber", "A0601", "(DATEDIFF(DAY, DataTime, GETDATE()) = 1) and ExXmlYN='" + sStat + "'", "RejectInvoiceNumber", "");
                if (dvResultD != null)
                {
                    for (int i = 0; i < dvResultD.Count; i++)
                    { strTXT += "A0601;" + Convert.ToString(dvResultD.Table.Rows[i][0]) + Environment.NewLine; }
                    dvResultD = null;
                }

                StreamWriter sw = new StreamWriter(strPath + sPgSN + sStat + ".TXT");
                sw.Write(strTXT);
                sw.Close();
            }

            public void GoToSTemp(string STempKind, string STempNo)
            {
                if (STempKind != "" && STempNo != "")
                {
                    System.Collections.Hashtable data = new System.Collections.Hashtable();
                    data["STempKind"] = STempKind.ToString();
                    data["STempNo"] = STempNo.ToString();
                    InsertDataNonKey("STemp", data);
                    data = null;
                }
            }

            //public void CallMkPDFinvoAll2(string MInvoiceNumberS)
            //{
            //    string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            //    #region 取資料

            //    DataView dvResult0401H = null;
            //    DataView dvResult0401D = null;
            //    DataView dvResultTemp = null;
            //    string sMSIdentifier = "";//03553526 總公司(3000)  21543058 桃園所(3910)
            //    string sMBIdentifier = "";
            //    string sMBName = "";
            //    string sMBAddress = "";

            //    string sMInvoiceDate = "";
            //    string sReYear = "";
            //    string sReDay = "";
            //    //string sReMonth = "";
            //    string sMInvoiceTime = "";
            //    string sMRandomNumber = "";
            //    string sATotalAmount = "";
            //    string sCompanyName = "";
            //    string ss0401SN = "";
            //    string sCompanyTel = "";
            //    string sASalesAmount = "";
            //    string sAFreeTaxSalesAmount = "";
            //    string sATaxType = "";
            //    string sATaxAmount = "";
            //    string sASalesAmount16 = "";
            //    string sATotalAmount16 = "";
            //    string sCompanyKey = "";
            //    string s0401DCount = "0";
            //    string sPdfPath = "";

            //    string sCOM = "";
            //    string sTBKind = "";

            //    dvResult0401H = Kind1SelectTbl2("", "A0401View", "MInvoiceNumber='" + MInvoiceNumberS + "'", "", "");
            //    if (dvResult0401H != null)
            //    {
            //        sMSIdentifier = dvResult0401H.Table.Rows[0]["MSIdentifier"].ToString();
            //        sMBIdentifier = dvResult0401H.Table.Rows[0]["MBIdentifier"].ToString();
            //        sMBName = dvResult0401H.Table.Rows[0]["MBName"].ToString();
            //        sMBAddress = dvResult0401H.Table.Rows[0]["MBAddress"].ToString();
            //        sMInvoiceDate = dvResult0401H.Table.Rows[0]["MInvoiceDate"].ToString();
            //        sReYear = (Convert.ToInt16(dvResult0401H.Table.Rows[0]["MInvoiceDate"].ToString().Substring(0, 4)) - 1911).ToString();
            //        sReDay = dvResult0401H.Table.Rows[0]["MInvoiceDate"].ToString().Substring(4, 4);
            //        sMInvoiceTime = dvResult0401H.Table.Rows[0]["MInvoiceTime"].ToString();
            //        sMRandomNumber = dvResult0401H.Table.Rows[0]["MRandomNumber"].ToString();
            //        sATotalAmount = dvResult0401H.Table.Rows[0]["ATotalAmount"].ToString();
            //        sCompanyName = dvResult0401H.Table.Rows[0]["CompanyName"].ToString();
            //        ss0401SN = dvResult0401H.Table.Rows[0]["s0401SN"].ToString();
            //        sCompanyTel = dvResult0401H.Table.Rows[0]["CompanyTel"].ToString();
            //        sAFreeTaxSalesAmount = dvResult0401H.Table.Rows[0]["AFreeTaxSalesAmount"].ToString();
            //        sATaxType = dvResult0401H.Table.Rows[0]["ATaxType"].ToString();
            //        sATaxAmount = dvResult0401H.Table.Rows[0]["ATaxAmount"].ToString();
            //        sASalesAmount = dvResult0401H.Table.Rows[0]["ASalesAmount"].ToString();
            //        sASalesAmount16 = ToL0(String.Format("{0:X}", Convert.ToInt64(dvResult0401H.Table.Rows[0]["ASalesAmount"])), 8);
            //        sATotalAmount16 = ToL0(String.Format("{0:X}", Convert.ToInt64(dvResult0401H.Table.Rows[0]["ATotalAmount"])), 8);
            //        sCompanyKey = dvResult0401H.Table.Rows[0]["CompanyKey"].ToString();

            //        dvResult0401D = Kind1SelectTbl2("DDescription,DQuantity,DUnitPrice,DAmount,DSequenceNumber", "A0401D", "MInvoiceNumber='" + MInvoiceNumberS + "'", "DSequenceNumber", "ASC");
            //        if (dvResult0401D != null)
            //        {
            //            s0401DCount = dvResult0401D.Count.ToString();
            //        }
            //        sTBKind = "A0401H";
            //    }
            //    else
            //    {
            //        dvResult0401H = Kind1SelectTbl2("", "C0401View", "MInvoiceNumber='" + MInvoiceNumberS + "'", "", "");
            //        if (dvResult0401H != null)
            //        {
            //            sMSIdentifier = dvResult0401H.Table.Rows[0]["MSIdentifier"].ToString();
            //            sMBIdentifier = dvResult0401H.Table.Rows[0]["MBIdentifier"].ToString();
            //            sMBName = dvResult0401H.Table.Rows[0]["MBName"].ToString();
            //            sMBAddress = dvResult0401H.Table.Rows[0]["MBAddress"].ToString();
            //            sMInvoiceDate = dvResult0401H.Table.Rows[0]["MInvoiceDate"].ToString();
            //            sReYear = (Convert.ToInt16(dvResult0401H.Table.Rows[0]["MInvoiceDate"].ToString().Substring(0, 4)) - 1911).ToString();
            //            sReDay = dvResult0401H.Table.Rows[0]["MInvoiceDate"].ToString().Substring(4, 4);
            //            sMInvoiceTime = dvResult0401H.Table.Rows[0]["MInvoiceTime"].ToString();
            //            sMRandomNumber = dvResult0401H.Table.Rows[0]["MRandomNumber"].ToString();
            //            sATotalAmount = dvResult0401H.Table.Rows[0]["ATotalAmount"].ToString();
            //            sCompanyName = dvResult0401H.Table.Rows[0]["CompanyName"].ToString();
            //            ss0401SN = dvResult0401H.Table.Rows[0]["s0401SN"].ToString();
            //            sCompanyTel = dvResult0401H.Table.Rows[0]["CompanyTel"].ToString();
            //            sAFreeTaxSalesAmount = dvResult0401H.Table.Rows[0]["AFreeTaxSalesAmount"].ToString();
            //            sATaxType = dvResult0401H.Table.Rows[0]["ATaxType"].ToString();
            //            sATaxAmount = dvResult0401H.Table.Rows[0]["ATaxAmount"].ToString();
            //            sASalesAmount = dvResult0401H.Table.Rows[0]["ASalesAmount"].ToString();
            //            sASalesAmount16 = ToL0(String.Format("{0:X}", Convert.ToInt64(dvResult0401H.Table.Rows[0]["ASalesAmount"])), 8);
            //            sATotalAmount16 = ToL0(String.Format("{0:X}", Convert.ToInt64(dvResult0401H.Table.Rows[0]["ATotalAmount"])), 8);
            //            sCompanyKey = dvResult0401H.Table.Rows[0]["CompanyKey"].ToString();

            //            dvResult0401D = Kind1SelectTbl2("DDescription,DQuantity,DUnitPrice,DAmount,DSequenceNumber", "C0401D", "MInvoiceNumber='" + MInvoiceNumberS + "'", "DSequenceNumber", "ASC");
            //            if (dvResult0401D != null)
            //            {
            //                s0401DCount = dvResult0401D.Count.ToString();
            //            }
            //            sTBKind = "C0401H";
            //        }
            //        else
            //        {
            //            return;
            //        }
            //    }

            //    string sATaxTypeN;
            //    switch (sATaxType)
            //    {
            //        case "1":
            //            sATaxTypeN = "TX";
            //            break;

            //        case "2":
            //            sATaxTypeN = "TZ";
            //            break;

            //        default:
            //            sATaxTypeN = "TX";
            //            break;
            //    }

            //    dvResultTemp = Kind1SelectTbl2("eName", "SParameter", "eKind='eMapPaths' AND eSN='6'", "", "");
            //    if (dvResultTemp != null)
            //    {
            //        sPdfPath = Convert.ToString(dvResultTemp.Table.Rows[0][0]);
            //    }

            //    dvResultTemp = Kind1SelectTbl2("eName", "SParameter", "eKind='sCOM' AND eSN='1'", "", "");
            //    if (dvResultTemp != null)
            //    {
            //        sCOM = Convert.ToString(dvResultTemp.Table.Rows[0][0]);
            //    }

            //    #endregion 取資料

            //    #region 發票內容

            //    Document doc = new Document(PageSize.A4, 50, 50, 50, 50);
            //    string sPDFpath = sPdfPath + MInvoiceNumberS + ".pdf";
            //    try
            //    {
            //        PdfWriter.GetInstance(doc, new FileStream(sPDFpath, FileMode.Create));
            //    }
            //    catch (Exception ex)
            //    {
            //        //檔案被開啟
            //        GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, MInvoiceNumberS, "[使用者正在開啟這個DPF !!][" + sPDFpath + "]" + ex.ToString(), "", 41);
            //    }
            //    doc.Open();
            //    BaseFont bfChinese = PublicMethodFramework35.Repositoies.BaseFontType();
            //    iTextSharp.text.Font ChFont4 = new iTextSharp.text.Font(bfChinese, 4);
            //    iTextSharp.text.Font ChFont6 = new iTextSharp.text.Font(bfChinese, 6);
            //    iTextSharp.text.Font ChFont7 = new iTextSharp.text.Font(bfChinese, 7);
            //    iTextSharp.text.Font ChFont8 = new iTextSharp.text.Font(bfChinese, 8);
            //    iTextSharp.text.Font ChFont8B = new iTextSharp.text.Font(bfChinese, 8, 1);
            //    iTextSharp.text.Font ChFont9 = new iTextSharp.text.Font(bfChinese, 9);
            //    iTextSharp.text.Font ChFont9B = new iTextSharp.text.Font(bfChinese, 9, 1);
            //    iTextSharp.text.Font ChFont10 = new iTextSharp.text.Font(bfChinese, 10);
            //    iTextSharp.text.Font ChFont11 = new iTextSharp.text.Font(bfChinese, 11);
            //    iTextSharp.text.Font ChFont11B = new iTextSharp.text.Font(bfChinese, 11, 1);
            //    iTextSharp.text.Font ChFont12 = new iTextSharp.text.Font(bfChinese, 12);
            //    iTextSharp.text.Font ChFont12B = new iTextSharp.text.Font(bfChinese, 12, 1);
            //    iTextSharp.text.Font ChFont13 = new iTextSharp.text.Font(bfChinese, 13);
            //    iTextSharp.text.Font ChFont14 = new iTextSharp.text.Font(bfChinese, 14);
            //    iTextSharp.text.Font ChFont14B = new iTextSharp.text.Font(bfChinese, 14, 1);
            //    iTextSharp.text.Font ChFont15 = new iTextSharp.text.Font(bfChinese, 15);
            //    iTextSharp.text.Font ChFont15B = new iTextSharp.text.Font(bfChinese, 15, 1);
            //    iTextSharp.text.Font ChFont16 = new iTextSharp.text.Font(bfChinese, 16);
            //    iTextSharp.text.Font ChFont16B = new iTextSharp.text.Font(bfChinese, 16, 1);
            //    iTextSharp.text.Font ChFont17 = new iTextSharp.text.Font(bfChinese, 17);
            //    iTextSharp.text.Font ChFont17B = new iTextSharp.text.Font(bfChinese, 17, 1);
            //    iTextSharp.text.Font ChFont18 = new iTextSharp.text.Font(bfChinese, 18);
            //    iTextSharp.text.Font ChFont18B = new iTextSharp.text.Font(bfChinese, 18, 1);
            //    iTextSharp.text.Font ChFont19 = new iTextSharp.text.Font(bfChinese, 19);
            //    iTextSharp.text.Font ChFont19B = new iTextSharp.text.Font(bfChinese, 19, 1);
            //    iTextSharp.text.Font ChFont20 = new iTextSharp.text.Font(bfChinese, 20);
            //    iTextSharp.text.Font ChFont20B = new iTextSharp.text.Font(bfChinese, 20, 1);
            //    iTextSharp.text.Font ChFont21B = new iTextSharp.text.Font(bfChinese, 21, 1);
            //    iTextSharp.text.Font ChFont22B = new iTextSharp.text.Font(bfChinese, 22, 1);

            //    Paragraph ParagraphW1 = new Paragraph();
            //    ParagraphW1.Leading = 23;
            //    ParagraphW1.Alignment = Element.ALIGN_CENTER;

            //    Phrase ParagraphWa011 = new Phrase(sCOM, ChFont14B);
            //    Phrase ParagraphWa014 = new Phrase("         ", ChFont4);
            //    ParagraphW1.Add(ParagraphWa011);
            //    ParagraphW1.Add(ParagraphWa014);
            //    ParagraphW1.Add("\n");

            //    string sProveName10 = "電子發票證明聯";
            //    Phrase ParagraphWa021 = new Phrase(sProveName10, ChFont14B);
            //    Phrase ParagraphWa022 = new Phrase("            ", ChFont4);
            //    ParagraphW1.Add(ParagraphWa021);
            //    ParagraphW1.Add(ParagraphWa022);

            //    doc.Add(ParagraphW1);

            //    doc.Add(new Paragraph(3f, "\n", ChFont9B));

            //    Paragraph ParagraphW2 = new Paragraph();
            //    ParagraphW2.Leading = 17;
            //    ParagraphW2.Alignment = Element.ALIGN_CENTER;

            //    //string sMInvoiceNumber = MInvoiceNumberS.Substring(0, 2) + "-" + MInvoiceNumberS.Substring(2, 8);
            //    string sYMDhms2 = sMInvoiceDate.Substring(0, 4) + "-" + sMInvoiceDate.Substring(4, 2) + "-" + sMInvoiceDate.Substring(6, 2);
            //    Phrase ParagraphWa041 = new Phrase(sYMDhms2, ChFont10);
            //    Phrase ParagraphWa042 = new Phrase("      ", ChFont4);
            //    ParagraphW2.Add(ParagraphWa041);
            //    ParagraphW2.Add(ParagraphWa042);
            //    ParagraphW2.Add("\n");
            //    doc.Add(ParagraphW2);

            //    //string sYMDhms = sMInvoiceDate.Substring(0, 4) + "-" + sMInvoiceDate.Substring(4, 2) + "-" + sMInvoiceDate.Substring(6, 2) + " " + sMInvoiceTime + "  格式 25";
            //    string sYMDhms = "發票號碼 : " + MInvoiceNumberS.Substring(0, 2) + "-" + MInvoiceNumberS.Substring(2, 8) + "                                                                                                                                 格式 : 25";
            //    doc.Add(new Paragraph(10f, sYMDhms, ChFont10));

            //    doc.Add(new Paragraph(10f, "買        方 : " + sMBName, ChFont10));
            //    doc.Add(new Paragraph(10f, "統一編號 : " + sMBIdentifier, ChFont10));
            //    doc.Add(new Paragraph(10f, "地        址 : " + sMBAddress, ChFont10));
            //    doc.Add(new Paragraph(10f, "\n", ChFont9B));

            //    //string sMRandomNumberA = "隨機碼:" + sMRandomNumber + "             " + "總計:" + sATotalAmount;
            //    //doc.Add(new Paragraph(10f, sMRandomNumberA, ChFont9B));

            //    //string sMSIdentifierB = "";
            //    //if (sMBIdentifier == "0000000000")
            //    //{ sMSIdentifierB = "賣方:" + sMSIdentifier; }
            //    //else
            //    //{ sMSIdentifierB = "賣方:" + sMSIdentifier + "          " + "買方:" + sMBIdentifier; }

            //    //doc.Add(new Paragraph(10f, sMSIdentifierB, ChFont9B));

            //    //PdfPTable ntable = new PdfPTable(new float[] { 1,1,1,1,1,1,1,2,3});
            //    PdfPTable table = new PdfPTable(new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 4 });
            //    table.TotalWidth = 500;
            //    table.LockedWidth = true;

            //    // ntable.AddCell();
            //    PdfPCell itemname01 = new PdfPCell(new Phrase("品名", ChFont10));
            //    itemname01.Colspan = 5;
            //    itemname01.HorizontalAlignment = Element.ALIGN_CENTER;
            //    table.AddCell(itemname01);

            //    PdfPCell itemname02 = new PdfPCell(new Phrase("數量", ChFont10));
            //    itemname02.Colspan = 2;
            //    itemname02.HorizontalAlignment = Element.ALIGN_CENTER;
            //    table.AddCell(itemname02);

            //    PdfPCell itemname03 = new PdfPCell(new Phrase("單價", ChFont10));
            //    itemname03.Colspan = 2;
            //    itemname03.HorizontalAlignment = Element.ALIGN_CENTER;
            //    table.AddCell(itemname03);

            //    PdfPCell itemname04 = new PdfPCell(new Phrase("金額", ChFont10));
            //    itemname04.Colspan = 1;
            //    itemname04.HorizontalAlignment = Element.ALIGN_CENTER;
            //    table.AddCell(itemname04);

            //    PdfPCell itemname05 = new PdfPCell(new Phrase("備註", ChFont10));
            //    itemname05.Colspan = 1;
            //    itemname05.HorizontalAlignment = Element.ALIGN_CENTER;
            //    table.AddCell(itemname05);

            //    for (int i = 1; i <= 3; i++)
            //    {
            //        PdfPCell b1 = new PdfPCell(new Phrase("TestName" + i.ToString() + "C1", ChFont10));
            //        b1.Colspan = 5;
            //        b1.UseBorderPadding = false;
            //        b1.BorderWidth = 0;
            //        b1.BorderWidthLeft = 0.5f;
            //        b1.BorderWidthRight = 0.5f;
            //        table.AddCell(b1);

            //        PdfPCell b2 = new PdfPCell(new Phrase("TestC" + i.ToString() + "C2", ChFont10));
            //        b2.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        b2.Colspan = 2;
            //        b2.BorderWidth = 0;
            //        b2.BorderWidthRight = 0.5f;
            //        table.AddCell(b2);

            //        PdfPCell b3 = new PdfPCell(new Phrase("TeatS" + i.ToString() + "C3", ChFont10));
            //        b3.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        b3.Colspan = 2;
            //        b3.BorderWidth = 0;
            //        b3.BorderWidthRight = 0.5f;
            //        table.AddCell(b3);

            //        //table.AddCell("Cell" + i.ToString() + "c");
            //        //table.AddCell("Cell" + i.ToString() + "a");
            //        //table.AddCell("Cell" + i.ToString() + "b");
            //        //table.AddCell("Cell" + i.ToString() + "c");
            //        PdfPCell b4 = new PdfPCell(new Phrase("TeatT" + i.ToString() + "C4", ChFont10));
            //        b4.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        b4.BorderWidth = 0;
            //        b4.BorderWidthRight = 0.5f;
            //        table.AddCell(b4);

            //        PdfPCell b5 = new PdfPCell(new Phrase(" ", ChFont10));
            //        //b4.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        b5.BorderWidth = 0;
            //        b5.BorderWidthRight = 0.5f;
            //        table.AddCell(b5);
            //        //table.AddCell("Cell" + i.ToString() + " ");
            //    }

            //    //PdfPTable nested = new PdfPTable(1);
            //    //nested.AddCell("Nested Row 1");

            //    //nested.AddCell("Nested Row 2");

            //    //nested.AddCell("Nested Row 3");

            //    //PdfPCell nesthousing = new PdfPCell(nested);

            //    //nesthousing.Padding = 0f;

            //    //table.AddCell(nesthousing);

            //    //PdfPCell C99 = new PdfPCell(new Phrase(" ", ChFont10));
            //    //C99.Colspan = 11;
            //    //table.AddCell(C99);

            //    PdfPCell bottom1 = new PdfPCell(new Phrase("銷售總額合計", ChFont10));
            //    bottom1.Colspan = 9;
            //    table.AddCell(bottom1);
            //    PdfPCell bottom2 = new PdfPCell(new Phrase("1800", ChFont10));
            //    bottom2.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    bottom2.Colspan = 1;
            //    table.AddCell(bottom2);
            //    PdfPCell bottom3 = new PdfPCell(new Phrase("營業人蓋統一發票專用章", ChFont10));
            //    bottom3.Colspan = 1;
            //    table.AddCell(bottom3);

            //    //PdfPTable ptb = new PdfPTable(7);
            //    //float[] widths = new float[] { 150, 150, 150, 150, 150, 150, 150 };
            //    ////widths.Padding = 0f;
            //    //ptb.SetWidths(widths);
            //    //ptb.AddCell(new Phrase("a1", ChFont10));
            //    //ptb.AddCell(new Phrase("a2", ChFont10));
            //    //ptb.AddCell(new Phrase("a3", ChFont10));
            //    //ptb.AddCell(new Phrase("a4", ChFont10));
            //    //ptb.AddCell(new Phrase("a5", ChFont10));
            //    //ptb.AddCell(new Phrase("a6", ChFont10));
            //    //ptb.AddCell(new Phrase("a7", ChFont10));
            //    //table.AddCell(ptb);

            //    PdfPTable nested = new PdfPTable(7);
            //    Int32 a1 = 30;
            //    float[] widths = new float[] { a1, a1, a1, a1, a1, a1, a1 };
            //    nested.SetWidths(widths);
            //    nested.AddCell("1");
            //    nested.AddCell("2");
            //    nested.AddCell("3");
            //    nested.AddCell("4");
            //    nested.AddCell("5");
            //    nested.AddCell("6");
            //    nested.AddCell("7");
            //    PdfPCell nesthousing = new PdfPCell(nested);
            //    nesthousing.Padding = 0f;
            //    table.AddCell(nesthousing);

            //    PdfPCell bottom21 = new PdfPCell(new Phrase(" ", ChFont10));
            //    bottom21.Colspan = 8;
            //    table.AddCell(bottom21);

            //    //PdfPTable ptb = new PdfPTable(7);
            //    //float[] widths = new float[] { 150, 150, 150, 150, 150, 150, 150 };
            //    ////widths.Padding = 0f;
            //    //ptb.SetWidths(widths);
            //    //ptb.AddCell(new Phrase("a1", ChFont10));
            //    //ptb.AddCell(new Phrase("a2", ChFont10));
            //    //ptb.AddCell(new Phrase("a3", ChFont10));
            //    //ptb.AddCell(new Phrase("a4", ChFont10));
            //    //ptb.AddCell(new Phrase("a5", ChFont10));
            //    //ptb.AddCell(new Phrase("a6", ChFont10));
            //    //ptb.AddCell(new Phrase("a7", ChFont10));
            //    //table.AddCell(ptb);

            //    PdfPCell bottom22 = new PdfPCell(new Phrase(" 1", ChFont10));
            //    bottom22.Colspan = 1;
            //    table.AddCell(bottom22);
            //    PdfPCell bottom23 = new PdfPCell(new Phrase("OOXXOOX", ChFont10));
            //    bottom23.Colspan = 1;
            //    table.AddCell(bottom23);

            //    doc.Add(table);

            //    //Paragraph ParagraphW9 = new Paragraph();
            //    //ParagraphW9.Leading = 10;
            //    //ParagraphW9.Alignment = Element.ALIGN_LEFT;
            //    //Phrase ParagraphWa091 = new Phrase(sCompanyName + "   序:" + ss0401SN, ChFont9B);
            //    //Phrase ParagraphWa091n = new Phrase("\n", ChFont9B);
            //    //Phrase ParagraphWa092 = new Phrase("退貨憑電子發票證明聯正本辦理", ChFont9B);
            //    //Phrase ParagraphWa093n = new Phrase("\n", ChFont9B);
            //    //Phrase ParagraphWa0931n = new Phrase("\n", ChFont9B);
            //    //Phrase ParagraphWa094 = new Phrase("--------------------------------------------", ChFont9B);
            //    //Phrase ParagraphWa0941 = new Phrase("\n", ChFont9B);
            //    //ParagraphW9.Add(ParagraphWa091);
            //    //ParagraphW9.Add(ParagraphWa091n);
            //    //ParagraphW9.Add(ParagraphWa092);
            //    //ParagraphW9.Add(ParagraphWa093n);
            //    //ParagraphW9.Add(ParagraphWa0931n);
            //    //ParagraphW9.Add(ParagraphWa094);
            //    //ParagraphW9.Add(ParagraphWa0941);
            //    //doc.Add(ParagraphW9);

            //    ////加入明細------S

            //    ////取資料
            //    //DataView dvResult0401Dbb = null;
            //    //string s0401DCountAll = "0";

            //    //dvResult0401Dbb = Kind1SelectTbl2("DDescription,DQuantity,DUnitPrice,DAmount,DSequenceNumber", "A0401D", "MInvoiceNumber='" + MInvoiceNumberS + "'", "DSequenceNumber", "ASC");
            //    //if (dvResult0401Dbb != null)
            //    //{
            //    //    s0401DCountAll = dvResult0401Dbb.Count.ToString();
            //    //}

            //    //#endregion

            //    //#region 明細內容

            //    //doc.Add(new Paragraph(10f, sCompanyName + "    " + sCompanyTel, ChFont9B));
            //    //doc.Add(new Paragraph(10f, "發票號碼:" + MInvoiceNumberS + "\n\n", ChFont9B));

            //    //if (dvResult0401Dbb != null)
            //    //{
            //    //    for (int i = 0; i < dvResult0401Dbb.Count; i++)
            //    //    {
            //    //        doc.Add(new Paragraph(10f, Convert.ToString(dvResult0401Dbb.Table.Rows[i]["DDescription"]) + "\n", ChFont9B));
            //    //        doc.Add(new Paragraph(10f, "單價 : " + Convert.ToDouble(dvResult0401Dbb.Table.Rows[i]["DUnitPrice"]).ToString("0") + "  數量 : " + Convert.ToString(dvResult0401Dbb.Table.Rows[i]["DQuantity"]) + "  金額 : " + Convert.ToString(dvResult0401Dbb.Table.Rows[i]["DAmount"]) + "\n", ChFont9B));
            //    //    }
            //    //}

            //    //doc.Add(new Paragraph(14f, "共 " + dvResult0401Dbb.Count.ToString() + "項  合計 $" + sATotalAmount + "   (" + sATaxTypeN + ")" + "\n", ChFont11B));
            //    //doc.Add(new Paragraph(10f, "應稅額:" + sASalesAmount + " 免稅額:" + sAFreeTaxSalesAmount + " 稅額:" + sATaxAmount + "\n", ChFont9B));
            //    //doc.Add(new Paragraph(14f, "總計 $" + sATotalAmount + "\n", ChFont11B));
            //    //doc.Add(new Paragraph(10f, "\n", ChFont9B));

            //    //DataView dvResultTemp2 = Kind1SelectTbl2("eName", "SParameter", "eKind='eActions'", "eSN", "ASC");
            //    //if (dvResultTemp2 != null)
            //    //{
            //    //    for (int i = 0; i < dvResultTemp2.Count; i++)
            //    //    {
            //    //        doc.Add(new Paragraph(14f, Convert.ToString(dvResultTemp2.Table.Rows[i][0]) + "\n", ChFont11B));
            //    //    }
            //    //}

            //    ////加入明細------E

            //    doc.NewPage();
            //    doc.Close();

            //    #endregion 發票內容

            //    //CallExPdfYN2(MInvoiceNumberS, sTBKind, "1");
            //}

            public void CallExPdfYN2(string MInvoiceNumberS, string sTBKind, string sCuKind)
            {
                System.Collections.Hashtable data = new System.Collections.Hashtable();
                data["ExPdf2YN"] = "Y";
                if (sCuKind == "1")
                {
                    UpdateData(sTBKind, data, "MInvoiceNumber", MInvoiceNumberS);
                }
                else
                {
                    UpdateData(sTBKind, data, "MAllowanceNumber", MInvoiceNumberS);
                }
            }

            #region function area by 俊晨

            /// <summary>
            /// 判斷字數，依指定的不同長度截斷存入List，可設定第一組List為不同長度
            /// </summary>
            /// <param name="str"></param>
            /// <param name="conditionFirstCount">第一組List指定長度</param>
            /// <param name="conditionLastCount">第二組(含)以後的List指定長度，預設戴第一組設定長度</param>
            /// <returns></returns>
            public List<string> ToListCutDownString(string str, int conditionFirstCount, int? conditionLastCount = null)
            {
                conditionLastCount = conditionLastCount == null ? (2 * conditionFirstCount) : (conditionLastCount * 2);
                conditionFirstCount = (2 * conditionFirstCount);
                List<string> resultStrList = new List<string>();

                #region 切出第一組

                int addStrLength = 0;
                foreach (var s in str)
                    addStrLength += GetStringLengthByByte(s.ToString());

                if (addStrLength <= conditionFirstCount)
                {
                    resultStrList.Add(str);
                    return resultStrList;
                }
                int iStr = 0;
                List<string> tempStr = new List<string>();
                str.All(a =>
                {
                    bool b = true;
                    iStr += GetStringLengthByByte(a.ToString());
                    if (iStr <= conditionFirstCount)
                        tempStr.Add(a.ToString());
                    else
                        b = false;
                    return b;
                }
                );
                resultStrList.Add(string.Join(string.Empty, tempStr));

                #endregion 切出第一組

                //取剩餘字串
                string remainingStr = str.Substring(string.Join(string.Empty, tempStr).Length, str.Length - string.Join(string.Empty, tempStr).Length);

                #region 切第二組以後跑遞回

                resultStrList.AddRange(ToListCutDownString(remainingStr, (int)conditionLastCount));

                #endregion 切第二組以後跑遞回

                return resultStrList;
            }

            /// <summary>
            /// 判斷字數，依指定長度截斷並存入List
            /// </summary>
            /// <param name="str">指定字串</param>
            /// <param name="count">限制截斷長度(獨立呼叫須*2)</param>
            /// <returns></returns>
            public List<string> ToListCutDownString(string str, int count)
            {
                int addStrLength = 0;
                List<string> result = new List<string>();

                List<string> temp = new List<string>();
                foreach (var s in str)
                {
                    int sLen = GetStringLengthByByte(s.ToString());
                    addStrLength += sLen;
                    if (addStrLength <= count)
                        temp.Add(s.ToString());
                }
                result.Add(string.Join(string.Empty, temp));
                string remainingStr = str.Substring(string.Join(string.Empty, temp).Length, str.Length - string.Join(string.Empty, temp).Length);
                if (remainingStr.Length > 0)
                    result.AddRange(ToListCutDownString(remainingStr, count));
                return result;
            }

            /// <summary>
            /// 取字元byte長度
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            public int GetStringLengthByByte(string str)
            {
                byte[] tIntByte = Encoding.Default.GetBytes(str.ToString());
                return tIntByte.Length;
            }

            /// <summary>
            /// 取發票稅額種類代碼 2017-09-27 法令更版修改by 俊晨 (Denny告知)
            /// </summary>
            /// <param name="taxTypeID"></param>
            /// <returns></returns>
            public string GetTaxTypeCodeByTaxTypeID(string taxTypeID)
            {
                string taxTypeCode = string.Empty;
                switch (taxTypeID)
                {
                    case "1":
                        taxTypeCode = "TX";
                        break;

                    case "2":
                        taxTypeCode = "TZ";
                        break;

                    default:
                        taxTypeCode = "0";
                        break;
                }
                return taxTypeCode;
            }

            /// <summary>
            /// 字串指定長度補空白
            /// </summary>
            /// <param name="totalLength"></param>
            /// <param name="str"></param>
            /// <returns></returns>
            public string AddEmptyStringBefore(int totalLength, string str)
            {
                var strLength = (str.Length * 2);
                var remainder = totalLength - strLength;
                List<string> emptyList = new List<string>();
                if (remainder > 0)
                {
                    for (int j = 0; j < remainder; j++)
                        emptyList.Add(" ");
                }
                return string.Concat(string.Join("", emptyList), str);
            }

            #endregion function area by 俊晨
        }
    }
}