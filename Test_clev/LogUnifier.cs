using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_clev
{
    public static class LogUnifier
    {
        static List<LogFormatter> formatters = new List<LogFormatter>();
        public static void Register(LogFormatter logFormatter) => formatters.Add(logFormatter);

        public static bool TryFormat(string log, out string result)
        {
            foreach(var formatter in formatters)
            {
                if(formatter.TryFormat(log, out result))
                    return true;
            }
            result = log;
            return false;
        }
    }

    class StandartLog
    {
        public string Date {  get; set; }
        public string Time { get; set; }
        public string LogLevel { get; set; }
        public string CallerName { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            var result = $"{Date}\t{Time}\t{LogLevel}\t{CallerName}\t{Message}";
            return result;
        }

    }

    public abstract class LogFormatter()
    {
        public abstract bool TryFormat(string log, out string result);

        protected string GetLogLevel(string logLevel)
        {
            return logLevel switch
            {
                "INFO" => "INFO",
                "INFORMATION" => "INFO",
                "WARN" => "WARN",
                "WARNING" => "WARN",
                "ERROR" => "ERROR",
                "DEBUG" => "DEBUG",
                _ => string.Empty
            };
        }
    }

    public class LogFormatter1 : LogFormatter
    {
        override public bool TryFormat(string log, out string result)
        {
            result = string.Empty;
            var logParts = log.Split(' ');
            var logItemsExceptMessage = logParts.Take(3).ToArray();

            var date = logItemsExceptMessage[0];
            var time = logItemsExceptMessage[1];

            bool dateTimeIsCorrect = DateTime.TryParseExact(
                $"{date} {time}", 
                "dd.MM.yyyy HH:mm:ss.fff", 
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime dt);

            if (!dateTimeIsCorrect)
                return false;
            date = dt.ToString("yyyy-MM-dd");

            var logLvl = logItemsExceptMessage[2];
            var logLevel = GetLogLevel(logLvl);
            if(string.IsNullOrEmpty(logLevel))
                return false;

            int messageStartPos = date.Length + time.Length + logLvl.Length + 3;
            var message = log.Remove(0, messageStartPos);

            var standartLog = new StandartLog()
            {
                Date = date,
                Time = time,
                LogLevel = logLevel,
                CallerName = "DEFAULT",
                Message = message,
            };

            result = standartLog.ToString();
            return true;
        }
    }

    public class LogFormatter2 : LogFormatter
    {
        override public bool TryFormat(string log, out string result)
        {
            result = string.Empty;
            var logParts = log.Split('|');

            if (logParts.Length != 5 )
                return false;

            var dateTime = logParts[0];
            var logLvl = logParts[1].Trim();
            var callerName = logParts[3];
            var message = logParts[4];


            bool dateTimeIsCorrect = DateTime.TryParseExact(
                dateTime,
                "yyyy-MM-dd HH:mm:ss.ffff",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime dt);

            if(!dateTimeIsCorrect)
                return false;

            var logLevel = GetLogLevel(logLvl);

            if(string.IsNullOrEmpty(logLevel))
                return false;

            var standartLog = new StandartLog()
            {
                Date = dateTime.Remove(10, 14), // remove whitespace + time
                Time = dateTime.Remove(0, 11),  // remove date + whitespase
                LogLevel = logLevel,
                CallerName = "DEFAULT",
                Message = message,
            };

            result = standartLog.ToString();
            return true;
        }
    }
}
