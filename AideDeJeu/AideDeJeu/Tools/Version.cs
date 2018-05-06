using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeu.Tools
{
    public interface IAppVersion
    {
        string GetVersion();
        int GetBuild();
    }
}
