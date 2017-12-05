using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BuDir
{
    public void Begin(string sKind0)
    {
        using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
        { query.CreateDIRok(""); }
    }
}

