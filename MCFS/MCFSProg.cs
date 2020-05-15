using DokanNet;
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
            MCFSCacheManager cm = new MCFSCacheManager(1024 * 1024 * 1024);
            MemoryStream ms = cm.CreateStream("\\test.txt", 1024);
            TextWriter tw = new StreamWriter(ms);
            tw.WriteLine("test text.");
            tw.Flush();
            ms.Close();

            Console.WriteLine(cm.file_refs[0]);
            Console.WriteLine(cm.data_store[0].Length);
            Console.WriteLine(cm.IsCached("\\test.txt"));
            Console.WriteLine(cm.IndexOf("\\test.txt"));
            Console.ReadKey();

            return;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Memory Cached File System (MCFS). Copyright (C) acdra1n 2020.\n");
            Console.ResetColor();
            Dokan.Mount(new MCFSDrv(new MCFSParams()
            {
                TargetDataLocation = Environment.CurrentDirectory + "\\fsroot",
                VolumeLabel = "mcfstest"
            }), "N:\\", DokanOptions.FixedDrive, 4, null);
        }
    }
}
