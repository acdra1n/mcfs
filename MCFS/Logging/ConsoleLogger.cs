using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFS.Logging
{
    public class ConsoleLogger : Logger
    {
        public override void Log(LogLevel level, string format, params object[] args)
        {
            switch(level)
            {
                case LogLevel.INFO:
                    Console.WriteLine("{0} INFO: {1}", DateTime.Now, string.Format(format, args));
                    break;
                case LogLevel.WARN:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("{0} WARN: {1}", DateTime.Now, string.Format(format, args));
                    Console.ResetColor();
                    break;
                case LogLevel.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0} ERROR: {1}", DateTime.Now, string.Format(format, args));
                    Console.ResetColor();
                    break;
                case LogLevel.FATAL:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0} [!] FATAL ERROR [!]: {1}", DateTime.Now, string.Format(format, args));
                    Console.ResetColor();
                    break;
            }
        }
    }
}
