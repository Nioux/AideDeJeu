using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeu
{
    public static class Config
    {
#if CONFIG_HD
        public const string Domain = "HD";
#elif CONFIG_JOA
        public const string Domain = "JoA";
#elif CONFIG_CO
        public const string Domain = "CO";
#else
        public const string Domain = "HD";
#endif
    }
}
