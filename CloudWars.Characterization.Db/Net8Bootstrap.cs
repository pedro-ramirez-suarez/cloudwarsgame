#if NET8_0_OR_GREATER
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.CompilerServices;

namespace CloudWars.Characterization.Db
{
    internal static class Net8Bootstrap
    {
        [ModuleInitializer]
        internal static void Initialize()
        {
            string configPath = Path.Combine(AppContext.BaseDirectory, "CloudWars.Characterization.Db.dll.config");
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", configPath);

            DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);
        }
    }
}
#endif
