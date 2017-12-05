using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace NSysDB
{
    public partial class DB
    {
        /// <summary>
        /// 取得Kind1"資料庫連線字串
        /// </summary>
        /// <returns></returns>
        public static string GetKind1ConnString()
        {
            XMLClass oXMLClass = new XMLClass();
            string DataSource = oXMLClass.GetDbXml()[0].ToString();
            string InitialCatalog = oXMLClass.GetDbXml()[1].ToString();
            string UserID = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(oXMLClass.GetDbXml()[2].ToString()));
            string Password = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(oXMLClass.GetDbXml()[3].ToString()));
            string TimeOut = oXMLClass.GetDbXml()[4].ToString();

            return string.Format("Data Source={0};Initial Catalog={1};User ID={2}; Password={3}; Connect Timeout={4}", DataSource, InitialCatalog, UserID, Password, TimeOut);
        }

        /// <summary>
        /// 取得Kind1"資料庫Connection物件
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetKind1Connection()
        {
            return new SqlConnection(GetKind1ConnString());
        }





    }
}
