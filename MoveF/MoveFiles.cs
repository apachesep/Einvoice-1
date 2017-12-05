using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MoveFiles
{
    public void Begin(string sKind, string sSource, string sToWhere)
    {
        string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        if (sKind == "0")
        { sKind = "*.*"; }
        else
        { sKind = sKind.ToUpper() + "*.*"; }

        foreach (string OkFName in System.IO.Directory.GetFileSystemEntries(sSource, sKind))
        {
            using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
            {
                try
                {
                    System.IO.File.Move(OkFName, OkFName.Replace(sSource, sToWhere));
                    query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, "", "", 99);
                }
                catch (Exception ex)
                {
                    query.GoLogsAll(sPgSN, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, OkFName, ex.ToString(), "", 15);
                }
            }
        }
    }

}
