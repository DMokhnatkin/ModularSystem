﻿using System;
using System.IO;
using System.Linq;

namespace ModularSystem.Common.PackedModules.Zip
{
    public class FilePackedModule : ZipPackedModule
    {
        public string FilePath { get; }

        public FilePackedModule(string filePath)
        {
            FilePath = filePath;
        }

        /// <inheritdoc />
        public override Stream OpenWriteStream()
        {
            return File.OpenWrite(FilePath);
        }

        /// <inheritdoc />
        public override Stream OpenReadStream()
        {
            return File.OpenRead(FilePath);
        }

        /// <inheritdoc />
        public override Stream OpenEditStream()
        {
            return File.Open(FilePath, FileMode.OpenOrCreate);
        }

        /// <inheritdoc />
        // Perfomance can be improved by cache
        public override ModuleIdentity ModuleIdentity => ModuleIdentity.Parse(this.ExtractMetaFile().Identity);

        /// <inheritdoc />
        // Perfomance can be improved by cache
        public override ModuleIdentity[] Dependencies => this.ExtractMetaFile().Dependencies.Select(ModuleIdentity.Parse).ToArray();

        /// <inheritdoc />
        // Perfomance can be improved by cache
        public override ModuleType ModuleType => (ModuleType)Enum.Parse(typeof(ModuleType), this.ExtractMetaFile().Type, true);
    }
}
