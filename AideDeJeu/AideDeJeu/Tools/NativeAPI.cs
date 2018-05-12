using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeu.Tools
{
    public interface INativeAPI
    {
        string GetVersion();
        int GetBuild();
        string GetDatabasePath(string databaseName);
    }
}
