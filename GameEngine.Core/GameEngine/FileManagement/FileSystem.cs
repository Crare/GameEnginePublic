﻿using System.IO;
using System.Text.Json;

namespace GameEngine.Core.GameEngine.FileManagement
{
    public static class FileSystem
    {
        public static void SaveAsJson<T>(string fileName, T content)
        {
            File.WriteAllText(fileName, JsonSerializer.Serialize(content));
        }

        public static T LoadFromJson<T>(string fileName)
        {
            if (File.Exists(fileName))
            {
                return JsonSerializer.Deserialize<T>(File.ReadAllText(fileName));
            }
            return default(T);
        }
    }
}
