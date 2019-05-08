using Serilog;
using Serilog.Core;
using System;
using System.IO;
using System.Linq;
using XBNLoader.Converters;

namespace XBNLoader
{
    public static class Program
    {
        static void Main()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Async(w => w.Console(outputTemplate: "[{Level:u3} {SourceContext}] {Message}{NewLine}{Exception}"))
                .MinimumLevel.Verbose()
                .CreateLogger();
            var logger = Log.ForContext(Constants.SourceContextPropertyName, "Main");

            var xbns = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.*", SearchOption.AllDirectories)
                .Where(s => (Path.GetExtension(s) == ".xbn"));
            foreach (string xbn in xbns)
                XBNtoXML.Convert(xbn);

            //XMLtoXBN.Convert(Path.Combine(Directory.GetCurrentDirectory(), @"XBN\Actions.xml"));

            logger.Information("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
