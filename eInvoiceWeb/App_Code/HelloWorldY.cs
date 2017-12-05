using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// HelloWorldY 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
// [System.Web.Script.Services.ScriptService]
public class HelloWorldY : System.Web.Services.WebService
{

    public HelloWorldY()
    {

        //如果使用設計的元件，請取消註解下列一行
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld(string sHelloWorldY)
    {

        using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
        {
            // query.GoLogsAll("HelloWorldY", "HelloWorldY已被觸發了喔!!", Convert.ToString(sHelloWorldY), "", "", 1);
            query.GoLogsAll("HelloWorldY", "HelloWorldY已被觸發了喔!!", "", "", "", 1);
        }


        try
        {
    
             using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
            {
                query.GoLogsAll("HelloWorldY", "HelloWorldY已被觸發了喔!!",Convert.ToString(sHelloWorldY), "", "", 1);

            }
        }
        catch (Exception ex)
        {
            using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
            {
                query.GoLogsAll("HelloWorldY", "HelloWorldY已被觸發了喔!!", ex.ToString(), "", "", 1);
            }
        }



        string a123;
        if (sHelloWorldY == null)
        { a123 = "GG"; }
        else
        { a123 = "YY"; }

        return a123;
        // return sHelloWorldY;
        //return "已被觸發了";
    }

}
