using System;
using System.IO;

namespace LoggerApp.Api
{
    class Settings
    {
        public static T GetSetting<T>(string key) where T : struct
        {
            var path = $@"C:\temp\elasticloggingdemo\{key}.config";
            if (File.Exists(path))
            {
                var content = File.ReadAllText(path);
                return (T) Convert.ChangeType(content, typeof (T));
            }

            return default(T);
        }

        public static void SetSetting<T>(string key, T value) where T : struct
        {
            var path = $@"C:\temp\elasticloggingdemo\{key}.config";
            
            File.WriteAllText(path, (string) Convert.ChangeType(value, typeof(string)));
        }
    }
}
