using iTextSharp.text.pdf;
using PublicMethodFramework35.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Xml;
using System.Linq;
using System.IO;

namespace PublicMethodFramework35
{
    public static class Repositoies
    {
        public static WorkProcessType WorkType
        {
            get
            {
                return WorkProcessType.Release;
            }
        }


        public static string GetParaXml(string XMLNode)
        {
            string sSystemINI = "D:\\eInvoiceSLN\\Release\\Xml\\eParamts.xml";
            System.IO.StreamReader objINIRead = new System.IO.StreamReader(sSystemINI);
            string strTheINI = objINIRead.ReadToEnd();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strTheINI);
            string RGetXMLNode = "";
            try
            {
                RGetXMLNode = xmlDoc.SelectSingleNode("/Params/" + XMLNode).InnerText.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RGetXMLNode;
        }

        /// <summary>
        /// 共用pdf字型
        /// </summary>
        /// <returns></returns>
        public static BaseFont BaseFontType()
        {
            string fontPath = @"c:\windows\fonts\pmingliu.ttf";
            var bfChinese = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            return bfChinese;
        }

        /// <summary>
        /// 自動發信
        /// </summary>
        /// <param name="ToWho">千萬別告訴我 你看不懂這些變數</param>
        /// <param name="CcWho"></param>
        /// <param name="FromWho"></param>
        /// <param name="strSubject"></param>
        /// <param name="strBody"></param>
        /// <returns></returns>
        public static string AutoEMail(string ToWho, string CcWho, string FromWho, string strSubject, string strBody)
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
                    ToWho1 = "einvoice@rinnai.com.tw";
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
                { mail.Subject = "電子發票管理平台-自動EMail通報機制 啟動了!![rinnai]"; }

                mail.IsBodyHtml = true;
                mail.Body = "<p>" + strBody + "<p>";
                SmtpClient SmtpServer = null;

                //SmtpServer = new SmtpClient("MS1.cgs.com.tw", 587);

                #region 正式區mail server

                //SmtpServer = new SmtpClient("10.10.1.48", 587);
                //SmtpServer.Credentials = new System.Net.NetworkCredential(@"cgs\einvoice", "cgs23534251");

                #endregion 正式區mail server

                #region 正式區mail server

                SmtpServer = new SmtpClient("lisomail.rinnai.com.tw", 25);
                SmtpServer.Credentials = new System.Net.NetworkCredential(@"einvsrv", "15963");

                #endregion 正式區mail server

                if (WorkType == WorkProcessType.Release)
                {
                    SmtpServer.Send(mail);
                }
                strResult = "執行成功!!";
            }
            return strResult;
        }

        public static List<string> GetPrintEinvoiceNumbersByPrintNo(string printNo)
        {
            List<string> invoiceList = new List<string>();
            Repositoies.SaveMesagesToTextFile("取號開始：" + printNo);
            var data = PrintEinvoiceSqlHandler(printNo);
            Repositoies.SaveMesagesToTextFile("取號結束：" + printNo);

            return data;
        }

        //delete [Rinnai$VAT Print Number] where  [Print No] = '3923852'
        public static void ClearPrintEinvocieDataByPrintNo(string printNo)
        {
            try
            {
                string connectionString = @"Data Source=192.168.1.4;Initial Catalog=NavisionNew;User ID=sa;Password=sasasasa";
                string queryString =
                @" delete[Rinnai$VAT Print Number] where[Print No] = @printNo ";

                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter() { ParameterName = "@printNo", Value = printNo });

                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    using (var cmd = connection.CreateCommand())
                    {
                        connection.Open();
                        cmd.CommandText = queryString;
                        cmd.Parameters.AddRange(sqlParameters.ToArray());
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<string> PrintEinvoiceSqlHandler(string printNo)
        {
            List<string> result = new List<string>();
            try
            {
                string connectionString = @"Data Source=192.168.1.4;Initial Catalog=NavisionNew;User ID=sa;Password=sasasasa";
                string queryString =
                @"  select * from [Rinnai$VAT Print Number] where [Print No] =@printNo  order by [VAT No] ";

                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter() { ParameterName = "@printNo", Value = printNo });
                Repositoies.SaveMesagesToTextFile("取號資料庫連線開始：" + printNo);

                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.AddRange(sqlParameters.ToArray());

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    if (dt.Rows.Count == 0)
                    {
                        Repositoies.SaveMesagesToTextFile("取得筆數0筆：");
                        throw new Exception(string.Format("列印序號：{0} 查無發票號碼!", printNo));
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        result.Add(dt.Rows[i]["VAT No"].ToString());
                    }
                }
                Repositoies.SaveMesagesToTextFile("取號資料庫連線結束：" + printNo);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            result = result.Distinct().ToList();
            return result;
        }

        /// <summary>
        /// 儲存錯誤訊息Log檔(不覆寫)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        public static void SaveMesagesToTextFile(string content)
        {
            string path = @"D:\EinvoiceLog\";
            string fileName = "print_log.txt";
            Exception error = null;
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (!File.Exists(path + fileName))
                {
                    using (StreamWriter sw = File.CreateText(path + fileName))
                    {
                        sw.WriteLine(DateTime.Now);
                        sw.WriteLine(content);
                        sw.WriteLine("-------------------------------------");
                        sw.WriteLine("");
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(path + fileName))
                    {
                        sw.WriteLine(DateTime.Now);
                        sw.WriteLine(content);
                        sw.WriteLine("-------------------------------------");
                        sw.WriteLine("");
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex;
            }
        }
    }
}