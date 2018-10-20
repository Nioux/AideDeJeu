using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AideDeJeu.Tools
{
    public interface INativeAPI
    {
        string GetVersion();
        int GetBuild();
        Task<string> GetDatabasePathAsync(string databaseName);
    }
}
