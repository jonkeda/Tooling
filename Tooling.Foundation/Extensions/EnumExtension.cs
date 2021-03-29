using System;
using System.Collections.Generic;

namespace Tooling.Extensions
{
    public static class EnumExtension
    {
        public static List<T> Split<T>(string text)
            where T : struct 
        {
            List<T> list = new List<T>();
            if (string.IsNullOrEmpty(text))
            {
                return list;
            }
            string[] words = text.Split(',');
            foreach (string word in words)
            {
                if (Enum.TryParse(word, out T e))
                {
                    list.Add(e);
                }
            }

            return list;
        }

    }
}