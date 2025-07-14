using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Test_clev
{
    internal class Program
    {
        static void Main()
        {
            string s = "aabccde";
            var compressed = s.Compress();
            var decompressed = compressed.Decompress();

            LogUnifier.Register(new LogFormatter1());
            LogUnifier.Register(new LogFormatter2());

            if(File.Exists("All_Logs.txt"))
            {
                Console.WriteLine("Файл \"All_Logs.txt\" с логами не найден.");
                Console.Read();
                return;
            }

            List<string> log = new List<string>();
            using(var reader = new StreamReader("All_Logs.txt"))
            {
                string? line;
                while((line = reader.ReadLine()) != null)
                {
                    log.Add(line);
                }
            }

            var maskedLog = log.Select(l =>
            {
                bool isCorrect = LogUnifier.TryFormat(l, out string newLog);
                return new
                {
                    IsCorrect = isCorrect,
                    Log = newLog
                };
            });

            string[] correctLogs = maskedLog.Where(l=>l.IsCorrect)
                                            .Select(l=>l.Log)
                                            .ToArray();

            using(var writer = new StreamWriter("StandartLog.txt"))
                writer.WriteLine(correctLogs);

            string[] wrongLogs = maskedLog.Where(l => !l.IsCorrect)
                                .Select(l => l.Log)
                                .ToArray();

            using(var writer = new StreamWriter("Problem.txt"))
                writer.WriteLine(correctLogs);

        }
    }
}
