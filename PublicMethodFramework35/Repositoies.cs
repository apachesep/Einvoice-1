using iTextSharp.text.pdf;
using PublicMethodFramework35.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Xml;

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
                #endregion

                #region 正式區mail server
                SmtpServer = new SmtpClient("lisomail.rinnai.com.tw", 25);
                SmtpServer.Credentials = new System.Net.NetworkCredential(@"einvsrv", "15963");
                #endregion
                if (WorkType == WorkProcessType.Release)
                {
                    SmtpServer.Send(mail);
                }
                strResult = "執行成功!!";
            }
            return strResult;
        }
    }
}
