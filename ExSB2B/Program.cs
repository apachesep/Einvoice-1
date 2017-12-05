using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Program
{
    static void Main(string[] args)
    {
        string sKind0 = args[0].ToString().ToLower();
        if (sKind0.Length > 0)
        {
            switch (sKind0.ToString())
            {
                case "a0401":  //生成XML A0401 存證B2B 開立發票
                    ExA0401 a0401 = new ExA0401();
                    a0401.Begin(sKind0);
                    break;
                case "a0501":  //生成XML A0501 存證B2B 作廢發票
                    ExA0501 a0501 = new ExA0501();
                    a0501.Begin(sKind0);
                    break;
                case "a0601":  //生成XML A0601 存證B2B  退回(拒收)發票
                    ExA0601 a0601 = new ExA0601();
                    a0601.Begin(sKind0);
                    break;

                case "b0401":  //生成XML B0401 存證B2B  折讓證明單
                    ExB0401 b0401 = new ExB0401();
                    b0401.Begin(sKind0);
                    break;
                case "b0501":  //生成XML B0501 存證B2B  作廢折讓證明單
                    ExB0501 b0501 = new ExB0501();
                    b0501.Begin(sKind0);
                    break;

                default:

                    break;
            }
        }
    }
}

