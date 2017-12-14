using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class AllXML
{
    public void Begin()
    {
        try
        {
            //程序一 : //生成 B2B XML
            ExA0401 a0401 = new ExA0401();
            a0401.Begin("a0401");

            ExA0501 a0501 = new ExA0501();
            a0501.Begin("a0501");

            ExA0601 a0601 = new ExA0601();
            a0601.Begin("a0601");

            ExB0401 b0401 = new ExB0401();
            b0401.Begin("b0401");

            ExB0501 b0501 = new ExB0501();
            b0501.Begin("b0501");

            //程序二 : //生成 B2C XML
            ExC0401 c0401 = new ExC0401();
            c0401.Begin("c0401");

            ExC0501 c0501 = new ExC0501();
            c0501.Begin("c0501");

            ExC0701 c0701 = new ExC0701();
            c0701.Begin("c0701");

            ExD0401 d0401 = new ExD0401();
            d0401.Begin("d0401");

            ExD0501 d0501 = new ExD0501();
            d0501.Begin("d0501");

        }
        catch (Exception ex)
        {
            string mailBody = string.Format("[電子發票 發票稅別為空值] <br> 錯誤訊息：{0}", ex.Message);
            string eToWho1 = PublicMethodFramework35.Repositoies.GetParaXml("eToWhoRinnai");
            string eFromWho1 = PublicMethodFramework35.Repositoies.GetParaXml("eFromWho");
            PublicMethodFramework35.Repositoies.AutoEMail(eToWho1, "", eFromWho1, "", mailBody);
            throw ex;
        }
    }
}

