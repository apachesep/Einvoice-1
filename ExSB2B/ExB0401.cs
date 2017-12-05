using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

public class ExB0401
{
    public void Begin(string sKind0)
    {
        GoXml(sKind0.ToUpper());
    }

    protected void GoXml(string sKind0up)
    {
        using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
        {
            //提醒一
            string sTableInNo = "MAllowanceNumber";
            string sKind0upAll = sKind0up + "H";
            string sTableSN = sKind0up + "SN";
            DataView dvResult1 = query.Kind1SelectTbl2(sTableSN + "," + sTableInNo, sKind0upAll, "ExXmlYN='N' ", sTableInNo, "");
            if (dvResult1 != null)
            {

                for (int i = 0; i < dvResult1.Count; i++)
                {
                    string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string ASN = Convert.ToString(dvResult1.Table.Rows[i][0]);
                    string MInvoiceNumber = Convert.ToString(dvResult1.Table.Rows[i][1]);

                    //開始生成XML
                    query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, "[生成XML發票號碼:]" + MInvoiceNumber, "", "", 1);
                    //提醒二
                    if (query.ExXmlB0401(ASN, MInvoiceNumber, sKind0up, sKind0upAll, sPgSN) == true)
                    {
                        //最後要更新 ExXmlYN & ExXmlTime
                        System.Collections.Hashtable data = new System.Collections.Hashtable();
                        data["ExXmlYN"] = "Y";
                        //data["ExXmlTime"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        query.UpdateData(sKind0upAll, data, sTableSN, ASN);
                    }

                    //結束生成XML
                    query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, "[生成XML發票號碼:]" + MInvoiceNumber, "", "", 2);

                }

            }
        }
    }

}

