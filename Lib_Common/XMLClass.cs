using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;


namespace NSysDB
{
    public class XMLClass
    {

        public string[] GetDbXml()
        {
            string sSystemINI = @"D:\eInvoiceSLN\Release\Xml\eDbConfig.xml";
            System.IO.StreamReader objINIRead = new System.IO.StreamReader(sSystemINI);
            string strTheINI = objINIRead.ReadToEnd();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strTheINI);
            string DbServerName = xmlDoc.SelectSingleNode("/Params/DbServerName").InnerText.ToString();
            string DbName = xmlDoc.SelectSingleNode("/Params/DbName").InnerText.ToString();
            string DbUserID = xmlDoc.SelectSingleNode("/Params/DbUserID").InnerText.ToString();
            string DbUserPassword = xmlDoc.SelectSingleNode("/Params/DbUserPassword").InnerText.ToString();
            string DbTimeOut = xmlDoc.SelectSingleNode("/Params/DbTimeOut").InnerText.ToString();
            string[] GetArray = new string[5] { DbServerName, DbName, DbUserID, DbUserPassword, DbTimeOut };

            return GetArray;
        }

        public string GetParaXml(string XMLNode)
        {
            string sSystemINI = "D:\\eInvoiceSLN\\Release\\Xml\\eParamts.xml";
            System.IO.StreamReader objINIRead = new System.IO.StreamReader(sSystemINI);
            string strTheINI = objINIRead.ReadToEnd();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strTheINI);
            string RGetXMLNode = xmlDoc.SelectSingleNode("/Params/" + XMLNode).InnerText.ToString();

            return RGetXMLNode;
        }

    }
}
