using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFS.Logging
{
    public class NullLogger : Logger
    {
        public override void Log(LogLevel level, string format, params object[] args)
        {
            
        }
    }
}
