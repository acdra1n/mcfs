using DokanNet;
using DokanNet.Logging;
using MCFS.Caching;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MCFS
{
    /// <summary>
    /// Memory Cached File System (MCFS) entry point.
    /// </summary>
    class MCFSProg
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Memory Cached File System (MCFS). Copyright (C) acdra1n 2020.\n");
            Console.ResetColor();

            Dokan.Mount(new MCFSDrv(new MCFSParams()
            {
                TargetDataLocation = Environment.CurrentDirectory + "\\fsroot",
                VolumeLabel = "mcfstest",
                Logger = new Logging.ConsoleLogger()
            }), "K:\\", DokanOptions.FixedDrive, 5, new Logger((a,b)=> { }, (a,b)=> { }, (a,b)=> { }, (a,b)=> { }, (a,b)=> { }));
        }
    }
}
