using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

public class ExD0501
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
            string sTableInNo = "CancelAllowanceNumber";
            string sTableSN = sKind0up + "SN";
            DataView dvResult1 = query.Kind1SelectTbl2(sTableSN + "," + sTableInNo, sKind0up, "ExXmlYN='N'", sTableInNo, "");
            if (dvResult1 != null)
            {

                for (int i = 0; i < dvResult1.Count; i++)
                {
                    string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string ASN = Convert.ToString(dvResult1.Table.Rows[i][0]);
                    string MINo = Convert.ToString(dvResult1.Table.Rows[i][1]);

                    //開始生成XML
                    query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, "[生成XML發票號碼:]" + MINo, "", "", 1);
                    //提醒二
                    if (query.ExXmlD0501(ASN, MINo, sKind0up, sKind0up, sPgSN) == true)
                    {
                        //最後要更新 ExXmlYN & ExXmlTime
                        System.Collections.Hashtable data = new System.Collections.Hashtable();
                        data["ExXmlYN"] = "Y";
                        //data["ExXmlTime"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        query.UpdateData(sKind0up, data, sTableSN, ASN);
                    }

                    //結束生成XML
                    query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, "[生成XML發票號碼:]" + MINo, "", "", 2);

                }

            }
        }
    }



}

