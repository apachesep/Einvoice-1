using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(args.Length.ToString());
        if (args.Length == 3)
        {
            MoveFiles GoMoveFiles = new MoveFiles();
            GoMoveFiles.Begin(args[0].ToString(),args[1].ToString(), args[2].ToString());
        }
    }
}
