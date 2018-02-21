using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

public class Program
{
    private static void Main(string[] args)
    {
        Exception error = null;
        try
        {
            //using (System.IO.treamWriter file =
            //         new System.IO.StreamWriter(@"D:\GetDirID.txt", true))
            //{
            //    file.WriteLine(dirID);
            //}
            bool isAllIn = args.ToList().Any(o => o == "ALL");
            Allin AllinNow = new Allin();

            Process currentProcess = Process.GetCurrentProcess();
            var currentProcessID = currentProcess.Id;
            var currentProcessName = currentProcess.ProcessName;
            var processInfo = AllinNow.GetProcessInfo(currentProcess);
            Stopwatch watch = new Stopwatch();
            watch.Start();
            //--start
            // 1，ALL 全部發票TXT寫入資料庫以及產生PDF  2，D:\ImInputERP 指定目錄取得TXT 3，D:\ImInputERP 但不取指定目錄
            if (isAllIn)
                AllinNow.Begin("ALL");
            else if (args.Length > 0)
                AllinNow.Begin(args[0]);
            else
                AllinNow.Begin("");

            //--stop
            watch.Stop();
            var totalTime = watch.Elapsed.TotalSeconds.ToString();
            using (System.IO.StreamWriter file =
                     new System.IO.StreamWriter(@"D:\CallAllInLog\log.txt", true))
            {
                StringBuilder sb = new StringBuilder();
                string logMsg = string.Format(
                    @"{0} => ProcessName:{1} UserName:{2} TotalTime:{3} IsAllIn:{4} Parameter:{5}",
                    DateTime.UtcNow.AddHours(8).ToString(),
                    string.Concat(currentProcessID, "-", currentProcessName),
                    string.Join("-", processInfo),
                    totalTime,
                    isAllIn.ToString(),
                    args.Count() > 0 ? args[0] : ""
                    );
                sb.AppendLine(logMsg);
                sb.AppendLine("----------------------------------------------------------");

                file.WriteLine(sb.ToString());
            }
        }
        catch (Exception ex)
        {
            error = ex;
        }
        finally
        {
            if (error != null)
                Console.WriteLine(error.Message);
            else
                Console.WriteLine("success");
        }
    }
}