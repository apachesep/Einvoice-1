using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {


        if (Request.QueryString["GGYY"] != null)
        {
            using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
            {
                // query.GoLogsAll("HelloWorldY", "HelloWorldY已被觸發了喔!!", Convert.ToString(sHelloWorldY), "", "", 1);
                query.GoLogsAll("HelloWorldY", "HelloWorldY已被觸發了喔!!", Request.QueryString["GGYY"].ToString(), "", "", 1);
            }
        }
    }
}