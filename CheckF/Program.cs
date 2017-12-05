using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(args.Length.ToString());
        if (args.Length == 1)
        {
            CheckFiles GoCheckFiles = new CheckFiles();
            GoCheckFiles.Begin(args[0].ToString());
        }
    }
}

