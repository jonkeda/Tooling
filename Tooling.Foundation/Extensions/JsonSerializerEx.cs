using System;
using System.IO;
using Newtonsoft.Json;
using Tooling.Mio;

namespace Tooling.Extensions
{
    public static class JsonSerializerEx
    {
        public static T LoadJson<T>(this VirtualFile file)
            where T : class
        {
            try
            {
                using (Stream stream = file.OpenRead())
                using (StreamReader sr = new StreamReader(stream) )
                {
                    JsonSerializer xs = new JsonSerializer();
                    return xs.Deserialize(sr, typeof(T)) as T;
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }
            return default(T);
        }

        public static void SaveJson<T>(this T value, VirtualFile file) 
            where T : class
        {
            SaveJson(file, value);
        }

        public static void SaveJson<T>(this VirtualFile file, T value)
            where T : class
        {
            try
            {
                using (Stream stream = file.OpenCreate())
                using (StreamWriter sr = new StreamWriter(stream))
                {
                    JsonSerializer xs = new JsonSerializer();
                    xs.Serialize(sr, value, typeof(T));
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }

    }
}