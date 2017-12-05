using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Program
{
    static void Main(string[] args)
    {

        if (args.Length == 1)
        {
            //MkPDFaw.exe ALL  //全部生成折讓單PDF
            string MAllowanceNumber = args[0].ToString();
            MPDFaw GOGO = new MPDFaw();
            GOGO.Begin(MAllowanceNumber.ToString());
        }
        //Console.ReadLine();

    }
}

