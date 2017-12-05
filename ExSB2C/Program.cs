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
                case "c0401":
                    ExC0401 c0401 = new ExC0401();
                    c0401.Begin(sKind0);
                    break;

                case "c0501":
                    ExC0501 c0501 = new ExC0501();
                    c0501.Begin(sKind0);
                    break;

                case "c0701":
                    ExC0701 c0701 = new ExC0701();
                    c0701.Begin(sKind0);
                    break;

                case "d0401":
                    ExD0401 d0401 = new ExD0401();
                    d0401.Begin(sKind0);
                    break;

                case "d0501":
                    ExD0501 d0501 = new ExD0501();
                    d0501.Begin(sKind0);
                    break;

                default:

                    break;
            }
        }
    }
}

