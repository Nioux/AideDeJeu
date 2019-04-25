using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AideDeJeu.Tools
{
    public interface INativeAPI
    {
        string GetVersion();
        int GetBuild();
        Task<string> GetDatabasePathAsync(string databaseName);
        Task SaveStreamAsync(string filename, Stream stream);
        Stream CreateStream(string filename);
        //void OpenFileByName(string fileName);
        Task LaunchFileAsync(string title, string message, string filePath);
    }
}
