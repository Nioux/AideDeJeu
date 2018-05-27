using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;

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
    }
}
