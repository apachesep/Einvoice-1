using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Program
{
    static void Main(string[] args)
    {
        try
        {
            Allin AllinNow = new Allin();
            AllinNow.Begin();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
}

