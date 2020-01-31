using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utf8Json;
using Utf8Json.Resolvers;

namespace Helpers
{
    public static class TempDataExtension
    {
        public static void Set<T>(this ITempDataDictionary tempData, string key, T value)
        {
            JsonSerializer.SetDefaultResolver(StandardResolver.AllowPrivateCamelCase);
            tempData[key] = JsonSerializer.ToJsonString(value);
        }
        public static T Get<T>(this ITempDataDictionary tempData, string key)
        {
            tempData.TryGetValue(key, out object value);
            return value == null ? default : JsonSerializer.Deserialize<T>((string)value);
        }

        public static void AddMessage(this ITempDataDictionary tempData, string key, string value)
        {
            var current = tempData.Get<List<string>>(key);
            if (current == default) current = new List<string>();
            current.Add(value);
            tempData.Set(key, current);
        }

        public static List<string> GetMessages(this ITempDataDictionary tempData, string key)
        {
            return tempData.Get<List<string>>(key);
        }
    }
}
