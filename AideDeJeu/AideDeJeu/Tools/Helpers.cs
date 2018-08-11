using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace AideDeJeu.Tools
{
    public static class Helpers
    {
        public static T GetResourceObject<T>(string resourceName) where T : class
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            var assembly = typeof(Helpers).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                return serializer.ReadObject(stream) as T;
            }
        }

        public static IEnumerable<string> GetResourceNames()
        {
            var assembly = typeof(Helpers).GetTypeInfo().Assembly;
            return assembly.GetManifestResourceNames();
        }

        public static async Task<string> GetResourceStringAsync(string resourceName)
        {
            var assembly = typeof(Helpers).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (var sr = new StreamReader(stream))
                    {
                        return await sr.ReadToEndAsync();
                    }
                }
                return null;
            }
        }

        public static string GetResourceString(string resourceName)
        {
            var assembly = typeof(Helpers).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (var sr = new StreamReader(stream))
                    {
                        return sr.ReadToEnd();
                    }
                }
                return null;
            }
        }

        public static async Task<string> GetStringFromUrl(string url)
        {
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync(url);
            }
        }

        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        public static string Capitalize(string text)
        {
            return string.Concat(text.Take(1)).ToUpper() + string.Concat(text.Skip(1)).ToString().ToLower();
        }

        public static string IdFromName(string name)
        {
            string id = string.Empty;
            foreach(var c in name)
            {
                if(c >= 'A' && c <= 'Z')
                {
                    id += c.ToString().ToLower();
                }
                else if(c == ' ')
                {
                    id += '-';
                }
                else if(c== '\'' || c == '/' || c == '(' || c ==')' || c == ':')
                {
                    // vide
                }
                else
                {
                    id += c;
                }
            }
            return id;
            //return name.ToLower().Replace(" ", "-").Replace("\'","").Replace("/","");
            //return RemoveDiacritics(name.ToLower().Replace(" ", "-").Replace("\'", ""));
        }

        public static string OldIdFromName(string name)
        {
            //return name.ToLower().Replace(" ", "-").Replace("\'", "").Replace("/", "");
            return RemoveDiacritics(name.ToLower().Replace(" ", "-")); //.Replace("\'", ""));
        }

        public static string Simplify(this string text)
        {
            if (text == null) return null;
            text = RemoveDiacritics(text).ToLower();
            var ntext = string.Empty;
            foreach(var c in text)
            {
                if (c >= 'a' && c <= 'z')
                {
                    ntext += c;
                }
            }
            return ntext;
        }

    }
}
