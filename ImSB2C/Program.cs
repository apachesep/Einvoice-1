using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Program
{
    static void Main(string[] args)
    {

        //存證B2C匯入 帶入變數
        //c0401h c0401d c0501 c0701 d0401h d0401d d0501

        if (args.Length == 1)
        {
            string sKind0 = args[0].ToString().ToLower();
            if (sKind0.Length > 0)
            {
                switch (sKind0.ToString())
                {

                    //存證匯入----------------------------------------------------------------------S
                    case "c0401h": //[匯入 C0401 存證B2C 開立發票 Main]
                        ImC0401H c0401h = new ImC0401H();
                        c0401h.Begin(sKind0);
                        break;
                    case "c0401d": //[匯入 C0401 存證B2C 開立發票 Details]
                        ImC0401D c0401d = new ImC0401D();
                        c0401d.Begin(sKind0);
                        break;

                    case "c0501": //[匯入 C0501 存證B2C 作廢發票]
                        ImC0501 c0501 = new ImC0501();
                        c0501.Begin(sKind0);
                        break;

                    case "c0701": //[匯入 C0701 存證B2C  註銷發票]
                        ImC0701 c0701 = new ImC0701();
                        c0701.Begin(sKind0);
                        break;

                    case "d0401h": //[匯入 D0401 存證B2C 開立折讓證明單/傳送折讓證明單通知 Main]
                        ImD0401H d0401h = new ImD0401H();
                        d0401h.Begin(sKind0);
                        break;
                    case "d0401d": //[匯入 D0401 存證B2C 開立折讓證明單/傳送折讓證明單通知 Details]
                        ImD0401D d0401d = new ImD0401D();
                        d0401d.Begin(sKind0);
                        break;
                    case "d0501": //[匯入 D0501 存證B2C 作廢折讓證明單]
                        ImD0501 d0501 = new ImD0501();
                        d0501.Begin(sKind0);
                        break;
                    //存證匯入----------------------------------------------------------------------E


                    case "budir": //[自動生成資料夾]
                        BuDir budir = new BuDir();
                        budir.Begin(sKind0);
                        break;

                    default:
                        Console.WriteLine("注意:傳入的變數是不可被解析的...!!");
                        break;
                }
            }
        }
        //Console.ReadLine();

    }
}

