using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Shared.Logging;
using VRage.Utils;

// Logs messages with format string syntax in them through every PluginLogger method,
// then checks that each one lands in the log file as one line, unchanged.

var messages = new[] { "{", "}", "{0}", "texture {X:512 Y:512}", "{\"json\": [1, 2]}" };

var path = Path.Combine(Path.GetTempPath(), $"PluginLoggerTests-{Environment.ProcessId}.log");
var writer = new StreamWriter(path);

// MyLog.Init needs the game's file system, so set up only what Log() uses
var log = new MyLog(alwaysFlush: true);
var flags = BindingFlags.Instance | BindingFlags.NonPublic;
typeof(MyLog).GetField("m_enabled", flags)!.SetValue(log, true);
typeof(MyLog).GetField("m_streamWriter", flags)!.SetValue(log, writer);
typeof(MyLog).GetField("m_indentsByThread", flags)!.SetValue(log, new Dictionary<int, int>());
MyLog.Default = log;

IPluginLogger logger = new PluginLogger("Test");
var methods = new (string Name, Action<string> Log)[]
{
    ("Trace", m => logger.Trace(m)),
    ("Debug", m => logger.Debug(m)),
    ("Info", m => logger.Info(m)),
    ("Warning", m => logger.Warning(m)),
    ("Error", m => logger.Error(m)),
    ("Critical", m => logger.Critical(m)),
};

foreach (var (name, logMethod) in methods)
foreach (var message in messages)
{
    try
    {
        logMethod($"{name} {message}");
    }
    catch (NullReferenceException)
    {
        // Warning and above go on to the platform's debugger output, which is not set up here
    }
}

writer.Flush();
writer.Close();

// One line per call: "<date> <time> - Thread:   N ->  <Severity>: <message>"
var lines = File.ReadAllLines(path);
File.Delete(path);

var failures = 0;
var index = 0;
foreach (var (name, _) in methods)
foreach (var message in messages)
{
    var expected = $": Test: {name} {message}";
    var line = index < lines.Length ? lines[index] : "(missing)";
    index++;
    if (!line.EndsWith(expected))
    {
        failures++;
        Console.WriteLine($"FAIL {name} '{message}' logged as: {line}");
    }
}

if (lines.Length != index)
{
    failures++;
    Console.WriteLine($"FAIL {lines.Length} lines logged, expected {index}");
}

var total = methods.Length * messages.Length;
Console.WriteLine(failures == 0 ? $"PASS {total}/{total}" : $"FAIL {total - failures}/{total}");
return failures == 0 ? 0 : 1;
