using System;
using System.IO;
using System.IO.Compression;
using ModularSystem.Communication.Data.Dto;
using Newtonsoft.Json;

namespace ModularSystem.Communication.Data.Files
{
    /// <summary>
    /// Allows write module dtos to directories
    /// </summary>
    public static class ModuleDtoFileSystem
    {
        private const string ConfigFileName = "conf.json";
        private const string DataDirectoryName = "data";

        public static void WriteToDirectory(this ModuleDto dto, string path)
        {
            Directory.CreateDirectory(path);

            using (StreamWriter wr = new StreamWriter(File.Create(Path.Combine(path, ConfigFileName))))
            {
                var t = new JsonSerializer
                {
                    Formatting = Formatting.Indented
                };
                t.Serialize(wr, dto.ModuleInfo);
            }

            var dataDirInfo = Directory.CreateDirectory(Path.Combine(path, DataDirectoryName));
            new ZipArchive(new MemoryStream(dto.Data)).ExtractToDirectory(dataDirInfo.FullName);
        }

        public static ModuleDto ReadFromDirectory(string path)
        {
            var confPath = Path.Combine(path, ConfigFileName);
            var dataPath = Path.Combine(path, DataDirectoryName);

            ModuleInfoDto moduleInfoDto;
            // Read config.json to moduleInfoDto
            using (JsonReader sr = new JsonTextReader(new StreamReader(File.OpenRead(confPath))))
            {
                var t = new JsonSerializer
                {
                    Formatting = Formatting.Indented
                };
                moduleInfoDto = t.Deserialize<ModuleInfoDto>(sr);
            }

            var tmpZipPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.zip");
            ZipFile.CreateFromDirectory(dataPath, tmpZipPath);
            byte[] data = File.ReadAllBytes(tmpZipPath);
            File.Delete(tmpZipPath);

            return new ModuleDto
            {
                ModuleInfo = moduleInfoDto,
                Data = data
            };
        }
    }
}
