using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Program
{
    static void Main(string[] args)
    {


        //存證B2B匯入 帶入變數
        //a0401h a0401d a0501 a0601 b0401h b0401d b0501

        if (args.Length == 1)
        {
            string sKind0 = args[0].ToString().ToLower();
            if (sKind0.Length > 0)
            {
                switch (sKind0.ToString())
                {

                    //存證匯入----------------------------------------------------------------------S
                    case "a0401h": //[匯入 A0401 存證B2B 開立發票 Main]
                        ImA0401H a0401h = new ImA0401H();
                        a0401h.Begin(sKind0);
                        break;
                    case "a0401d": //[匯入 A0401 存證B2B 開立發票 Details]
                        ImA0401D a0401d = new ImA0401D();
                        a0401d.Begin(sKind0);
                        break;
                    case "a0501": //[匯入 A0501 存證B2B 作廢發票]
                        ImA0501 a0501 = new ImA0501();
                        a0501.Begin(sKind0);
                        break;
                    case "a0601": //[匯入 A0601 存證B2B  退回(拒收)發票]
                        ImA0601 a0601 = new ImA0601();
                        a0601.Begin(sKind0);
                        break;

                    case "b0401h": //[匯入 B0401 存證B2B  開立折讓證明單/傳送折讓證明單通知 Main]
                        ImB0401H b0401h = new ImB0401H();
                        b0401h.Begin(sKind0);
                        break;
                    case "b0401d": //[匯入 D0401 存證B2B  開立折讓證明單/傳送折讓證明單通知 Details]
                        ImB0401D b0401d = new ImB0401D();
                        b0401d.Begin(sKind0);
                        break;
                    case "b0501": //[匯入 B0501 存證B2B 作廢折讓證明單]
                        ImB0501 b0501 = new ImB0501();
                        b0501.Begin(sKind0);
                        break;
                    //存證匯入----------------------------------------------------------------------E


                    case "budir": //[自動生成資料夾]
                        BuDir budir = new BuDir();
                        budir.Begin(sKind0);
                        break;

                    default:
                        //Console.WriteLine("注意:傳入的變數是不可被解析的...!!");
                        break;
                }
            }
        }
        //Console.ReadLine();
    }
}

