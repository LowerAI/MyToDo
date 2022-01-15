// See https://aka.ms/new-console-template for more information
using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;

using System.Diagnostics;
using System.Diagnostics.Tracing;

/// <summary>
/// 诊断控制台
/// 来自：动手实现一个适用于.NET Core 的诊断工具
/// https://mp.weixin.qq.com/s/TDYxcXKdhGMYRuayhMEQnw
/// 使用方法：
/// 先启动 WebApi 项目，然后在ConsoleApp中先运行相应命令
/// </summary>

if (args.Any())
{
    switch (args[0])
    {
        case "ps": // 获取正在运行的程序列表(dotnet run ps，此时可以得到进程Id)
            PrintProcessStatus();
            break;

        case "runtime": // 获取 GC 信息(dotnet run runtime 13288)
            PrintRuntime(int.Parse(args[1]));
            break;

        case "dump": // 生成Dump文件(dotnet run dump 13288)
            Dump(int.Parse(args[1]));
            break;

        case "trace": // 生成 Trace 文件(dotnet run trace 13288)
            Trace(int.Parse(args[1]));
            break;
    }
}

/// <summary>
/// 获取正在运行的程序列表
/// </summary>
static void PrintProcessStatus()
{
    var processes = DiagnosticsClient.GetPublishedProcesses()
            .Select(Process.GetProcessById)
            .Where(process => process != null);

    foreach (var process in processes)
    {
        Console.WriteLine($"ProcessId: {process.Id}");
        Console.WriteLine($"ProcessName: {process.ProcessName}");
        Console.WriteLine($"StartTime: {process.StartTime}");
        Console.WriteLine($"Threads: {process.Threads.Count}");

        Console.WriteLine();
        Console.WriteLine();
    }
}

/// <summary>
/// 获取 GC 信息
/// <para name="processId">进程Id</para>
/// </summary>
static void PrintRuntime(int processId)
{
    var providers = new List<EventPipeProvider>()
    {
        new ("Microsoft-Windows-DotNETRuntime",EventLevel.Informational, (long)ClrTraceEventParser.Keywords.GC)

    };

    var client = new DiagnosticsClient(processId);
    using (var session = client.StartEventPipeSession(providers, false))
    {
        var source = new EventPipeEventSource(session.EventStream);

        source.Clr.All += (TraceEvent obj) =>
        {
            Console.WriteLine(obj.EventName);
        };

        try
        {
            source.Process();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}

/// <summary>
/// 生成Dump文件
/// <para name="processId">进程Id</para>
/// </summary>
static void Dump(int processId)
{
    var client = new DiagnosticsClient(processId);
    client.WriteDump(DumpType.Normal, @"mydump.dmp", false);
}

/// <summary>
///  生成 Trace 文件
/// <para name="processId">进程Id</para>
/// </summary>
static void Trace(int processId)
{
    var cpuProviders = new List<EventPipeProvider>()
    {
        new EventPipeProvider("Microsoft-Windows-DotNETRuntime", EventLevel.Informational, (long)ClrTraceEventParser.Keywords.Default),
        new EventPipeProvider("Microsoft-DotNETCore-SampleProfiler", EventLevel.Informational, (long)ClrTraceEventParser.Keywords.None)
    };
    var client = new DiagnosticsClient(processId);
    using (var traceSession = client.StartEventPipeSession(cpuProviders))
    {
        Task.Run(async () =>
        {
            using (FileStream fs = new FileStream(@"mytrace.nettrace", FileMode.Create, FileAccess.Write))
            {
                await traceSession.EventStream.CopyToAsync(fs);
            }

        }).Wait(10 * 1000);

        traceSession.Stop();
    }
}