using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPlaguemangler.Extensions
{
    public static class ITempDataDictionaryExtensions
    {
        public static void Put<T>(this ITempDataDictionary self, string key, T obj) where T: class
        {
            self[key] = JsonConvert.SerializeObject(obj);
        }

        public static T Get<T>(this ITempDataDictionary self, string key) where T : class
        {
            if (!self.ContainsKey(key))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<T>(self[key] as string);
        }
    }
}
