using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModularSystem.Common.MetaFiles
{
    public class MetaFileWrapper : FileWrapperBase
    {
        public const string DefaultFileName = "meta.json";

        public MetaFileWrapper() : base()
        { }

        public MetaFileWrapper(Stream s) : base(JObject.Load(new JsonTextReader(new StreamReader(s))))
        { }

        public MetaFileWrapper(string file) : base(JObject.Load(new JsonTextReader(new StreamReader(System.IO.File.OpenRead(file)))))
        { }

        /// <summary>
        /// Find meta file in dir, and use it
        /// </summary>
        public static MetaFileWrapper FindInDirectory(string path, string fileName = MetaFileWrapper.DefaultFileName, bool recurrently = false)
        {
            var t = Directory.GetFiles(path, fileName, recurrently ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).FirstOrDefault();
            if (t == null)
                throw new FileNotFoundException($"meta file '{fileName}' was not found in '{path}'");

            return new MetaFileWrapper(t);
        }

        /// <summary>
        /// Create meta file and write to it
        /// </summary>
        /// <param name="path">Path of directory</param>
        /// <param name="fileName">Name of file</param>
        public void CreateAndWrite(string path, string fileName = MetaFileWrapper.DefaultFileName)
        {
            var t = Path.Combine(path, fileName ?? MetaFileWrapper.DefaultFileName);
            Write(t);
        }

        #region Properties

        public const string IdentityKey = "identity";
        public const string DependenciesKey = "dependencies";
        public const string TypeKey = "type";
        public const string StartScriptKey = "start";
        public const string ClientTypesKey = "client_types";

        public string Identity
        {
            get => GetValue<string>(IdentityKey, null);
            set => SetValue(IdentityKey, value);
        }

        public string Type
        {
            get => GetValue<string>(TypeKey, null);
            set => SetValue(TypeKey, value);
        }

        public string[] Dependencies
        {
            get => GetValues<string>(DependenciesKey, null);
            set => SetValues(DependenciesKey, value);
        }

        public string StartScript
        {
            get => GetValue<string>(StartScriptKey, null);
            set => SetValue(StartScriptKey, value);
        }

        /// <see cref="ModularSystem.Common.Modules.Client.IClientModule.ClientTypes"/>
        public string[] ClientTypes
        {
            get => GetValues<string>(ClientTypesKey);
            set => SetValues(ClientTypesKey, value);
        }
        #endregion
    }
}
