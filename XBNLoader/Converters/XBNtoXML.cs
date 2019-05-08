using Serilog;
using System.IO;
using System.Xml.Linq;
using XBNLibrary;

namespace XBNLoader.Converters
{
    public static class XBNtoXML
    {
        private static readonly ILogger logger = Log.ForContext(Serilog.Core.Constants.SourceContextPropertyName, nameof(XBNtoXML));

        public static void Convert(string path)
        {
            logger.Information($"Converting file {path.Replace(Directory.GetCurrentDirectory(), "")}");

            var xbn = XBN.Load(path);

            var xml = new XDocument(new XElement(xbn.RootElement.Name));
            foreach (var e in xbn.RootElement.Elements)
            {
                var element = new XElement(e.Name);
                foreach (var e2 in e.Properties)
                    element.SetAttributeValue(e2.Key, e2.Value);

                foreach (var e3 in e.Child)
                {
                    var e4 = new XElement(e3.Name);
                    foreach (var e5 in e3.Properties)
                        e4.SetAttributeValue(e5.Key, e5.Value);

                    element.Add(e4);
                }

                xml.Root.Add(element);
            }

            xml.Save(path.Replace(".xbn", ".xml"));
        }
    }
}
