﻿using System;
using System.IO;
using System.Linq;
using CommandLine;
using ModularSystem.Common;
using ModularSystem.Common.MetaFiles;
using ModularSystem.Common.PackedModules.Zip;
using Newtonsoft.Json.Linq;

namespace PackPlugin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Read();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(PackWithConfigFile);
        }

        public static void PackWithConfigFile(Options o)
        {
            var conf = JObject.Parse(File.ReadAllText(o.ConfigFile));

            if (!Directory.Exists(o.OutputDir))
            {
                Directory.CreateDirectory(o.OutputDir);
            }
            else
            {
                // Clear dir
                Directory.Delete(o.OutputDir, true);
                Directory.CreateDirectory(o.OutputDir);
            }

            var packedModules =
                conf["Modules"]
                .Select(x => x.Value<string>())
                .Select(x => PackHelper.PackModuleToFile(x, $"{MetaFileWrapper.FindInDirectory(x).Identity}.zip"))
                .ToArray();

            var batchPath = Path.Combine(o.OutputDir, $"{conf["BatchName"].Value<string>()}.zip");
            BatchHelper.BatchModulesToFile(packedModules, batchPath);

            // Remove all single module packages (we have batched them in one file)
            foreach (var file in Directory.GetFiles(o.OutputDir))
            {
                if (file != batchPath)
                    File.Delete(file);
            }
        }


    }
}
