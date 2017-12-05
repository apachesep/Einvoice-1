using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MPDF
{
    public void Begin(string sMInvoiceNumber)
    {
        string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
        {
            //MkPDF.exe NV41099654  //單筆生成PDF
            sMInvoiceNumber = sMInvoiceNumber.ToUpper();
            if (sMInvoiceNumber.Length == 10)
            {
                //發票證明聯
                query.CallMkPDFinvo(sMInvoiceNumber, "1");
                //發票證明聯補印
                query.CallMkPDFinvo(sMInvoiceNumber, "3");
                //格式二
                //query.CallMkPDFinvoAll2(sMInvoiceNumber);

            }
            else
            {
                ////MkPDF.exe ALL  //全部生成PDF  //ExPdfYN=N
                #region 生成B2B 發票PDF

                System.Data.DataView dvResultALLA = query.Kind1SelectTbl2("MInvoiceNumber", "A0401H", "(ExPdfYN IS NULL) OR (ExPdfYN <> 'Y')", "", "");
                if (dvResultALLA != null)
                {
                    if (dvResultALLA.Count > 0)
                    {
                        for (int i = 0; i < dvResultALLA.Count; i++)
                        {

                            if (Convert.ToString(dvResultALLA.Table.Rows[i][0]).Trim().Length == 10)
                            {
                                //發票證明聯
                                query.CallMkPDFinvoAll(Convert.ToString(dvResultALLA.Table.Rows[i][0]), "1");
                                //發票證明聯補印
                                query.CallMkPDFinvoAll(Convert.ToString(dvResultALLA.Table.Rows[i][0]), "3");
                                //格式二
                                //query.CallMkPDFinvoAll2(Convert.ToString(dvResultALLA.Table.Rows[i][0]));
                            }
                            else
                            {
                                query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, Convert.ToString(dvResultALLA.Table.Rows[i][0]), "[發票號碼不是10馬!!不生成PDF]", "", 41);
                            }

                        }
                    }
                }
                #endregion

                #region 生成B2C 發票PDF

                System.Data.DataView dvResultALLC = query.Kind1SelectTbl2("MInvoiceNumber", "C0401H", "(ExPdfYN IS NULL) OR (ExPdfYN <> 'Y')", "", "");
                if (dvResultALLC != null)
                {
                    if (dvResultALLC.Count > 0)
                    {
                        for (int i = 0; i < dvResultALLC.Count; i++)
                        {
                            if (Convert.ToString(dvResultALLC.Table.Rows[i][0]).Trim().Length == 10)
                            {
                                //發票證明聯
                                query.CallMkPDFinvo(Convert.ToString(dvResultALLC.Table.Rows[i][0]), "1");
                                //發票證明聯補印
                                query.CallMkPDFinvo(Convert.ToString(dvResultALLC.Table.Rows[i][0]), "3");
                            }
                            else
                            {
                                query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, Convert.ToString(dvResultALLC.Table.Rows[i][0]), "[發票號碼不是10馬!!不生成PDF]", "", 41);
                            }
                        }
                    }
                }
                #endregion
            }
        }
    }

}

