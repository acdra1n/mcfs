using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFS.Logging
{
    public abstract class Logger
    {
        public abstract void Log(LogLevel level, string format, params object[] args);
    }
}
