using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

public class ExA0401
{
    public void Begin(string sKind0)
    {
        //GoAC0401("A0401", sPgSN);
        //GoAC0401("C0401", sPgSN);

        //GoXmlC0401("C0401", sPgSN);

        GoXml(sKind0.ToUpper());
    }

    //protected void GoXmlC0401(string GoXML, string sPgSN)
    protected void GoXml(string sKind0up)
    {
        using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
        {
            //提醒一
            string sTableInNo = "MInvoiceNumber";
            string sKind0upAll = sKind0up + "H";
            string sTableSN = sKind0up + "SN";
            //DataView dvResult1 = query.Kind1SelectTbl2("AC0401SN,MInvoiceNumber", "AC0401", "ACKind='" + GoXML + "' And ExXmlYN='N'", "MInvoiceNumber", "");
            //DataView dvResult1 = query.Kind1SelectTbl2("A0401SN,MInvoiceNumber", "A0401H", "ACKind='" + sKind0upAll + "' And ExXmlYN='N'", "MInvoiceNumber", "");
            //DataView dvResult1 = query.Kind1SelectTbl2("A0401SN,MInvoiceNumber", "A0401H", "ExXmlYN='N' ", "MInvoiceNumber", "");
            //DataView dvResult1 = query.Kind1SelectTbl2(sTableSN + ",MInvoiceNumber", "A0401H", "ExXmlYN='N' ", "MInvoiceNumber", "");
            //DataView dvResult1 = query.Kind1SelectTbl2(sTableSN + ",MInvoiceNumber", sKind0upAll, "ExXmlYN='N' ", "MInvoiceNumber", "");
            DataView dvResult1 = query.Kind1SelectTbl2(sTableSN + "," + sTableInNo, sKind0upAll, "ExXmlYN='N' ", sTableInNo, "");
            if (dvResult1 != null)
            {

                for (int i = 0; i < dvResult1.Count; i++)
                {
                    string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    //string A0401SN = Convert.ToString(dvResult1.Table.Rows[i][0]);
                    string ASN = Convert.ToString(dvResult1.Table.Rows[i][0]);
                    string MInvoiceNumber = Convert.ToString(dvResult1.Table.Rows[i][1]);

                    //開始生成XML
                    query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, "[生成XML發票號碼:]" + MInvoiceNumber, "", "", 1);

                    //bool ExAC0401ok = query.ExAC0401(AC0401SN, MInvoiceNumber, GoXML, sPgSN);
                    //bool ExXmlA0401ok = query.ExXmlA0401(A0401SN, MInvoiceNumber, sKind0up, sKind0upAll, sPgSN);
                    //bool ExXmlok = query.ExXmlA0401(ASN, MInvoiceNumber, sKind0up, sKind0upAll, sPgSN);
                    //提醒二
                    if (query.ExXmlA0401(ASN, MInvoiceNumber, sKind0up, sKind0upAll, sPgSN) == true)
                    {
                        //最後要更新 A0401H/ExXmlYN & ExXmlTime
                        System.Collections.Hashtable data = new System.Collections.Hashtable();
                        data["ExXmlYN"] = "Y";
                        //data["ExXmlTime"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        //query.UpdateData("A0401H", data, "A0401SN", A0401SN);
                        //query.UpdateData(sKind0upAll, data, sTableSN, A0401SN);
                        query.UpdateData(sKind0upAll, data, sTableSN, ASN);
                    }

                    //結束生成XML
                    query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, "[生成XML發票號碼:]" + MInvoiceNumber, "", "", 2);

                }

            }
        }
    }

}

