using AideDeJeu.Services;
using AideDeJeuLib.Spells;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace AideDeJeuCmd
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var documentsDirectoryPath = @"C:\Users\yanma\Documents\Visual Studio 2017\Projects\AideDeJeu\Data\database.db"; // Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            ItemDatabaseHelper helper = new ItemDatabaseHelper(documentsDirectoryPath);
            //var items = await helper.GetSpellsAsync(classe: "", niveauMin: "0", niveauMax: "9", ecole: "", rituel: "", source: "(SRD)");
            var items = await helper.GetMonstersAsync(category: "", type: "", minPower: " 0 (0 PX)", maxPower: " 30 (155000 PX)", size: "", legendary: "", source: "(SRD)");

            foreach (var item in items)
            {

            }
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(items.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, items);
            stream.Seek(0, SeekOrigin.Begin);
            string text = await new StreamReader(stream).ReadToEndAsync();
            Console.WriteLine("Hello World!");
        }
    }
}
