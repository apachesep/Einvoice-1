using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class CheckFiles
{
    public void Begin(string sCheckWhere)
    {
        string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        foreach (string OkFName in System.IO.Directory.GetFileSystemEntries(sCheckWhere, "*.*"))
        {
            using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
            {
                query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "文字檔資料內容有誤!導致轉檔失敗!請盡速檢查文字檔內容!", "", 51);
            }
        }
    }

}

