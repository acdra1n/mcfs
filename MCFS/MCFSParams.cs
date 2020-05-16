using MCFS.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCFS
{
    public class MCFSParams
    {
        public string TargetDataLocation { get; set; }
        public long AllocMax { get; set; } = 1024 * 1024 * 1024; // 1 Gigabyte max memory alloc
        public string[] CacheAutoStore { get; set; } = new string[] { ".exe", ".dll" };
        public string VolumeLabel { get; set; }
        public Logger Logger { get; set; } = new NullLogger();
    }
}
