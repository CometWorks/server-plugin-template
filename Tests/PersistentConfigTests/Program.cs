using System;
using System.IO;
using Shared.Config;
using Shared.Logging;
using VRage.Utils;

MyLog.Default = new MyLog();
var appData = Directory.CreateTempSubdirectory("PersistentConfigTests-");
var path = Path.Combine(appData.FullName, "Storage", "TestPlugin.cfg");
var log = new PluginLogger("Test");

try
{
    using (var config = PersistentConfig<PluginConfig>.Load(log, path))
    {
        GC.SuppressFinalize(config);
        if (!config.IsNewDeployment || !File.Exists(path))
            throw new InvalidOperationException(
                "Load did not write the default config to Storage."
            );
    }

    using (var config = PersistentConfig<PluginConfig>.Load(log, path))
    {
        GC.SuppressFinalize(config);
        if (config.IsNewDeployment || !config.Data.Enabled)
            throw new InvalidOperationException("Load did not read back the config from Storage.");
    }

    Console.WriteLine("PASS: created Storage, wrote the default config, and loaded it again.");
}
finally
{
    appData.Delete(recursive: true);
}
