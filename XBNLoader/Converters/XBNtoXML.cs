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

                foreach (var e3 in e.Childs)
                {
                    var e4 = new XElement(e3.Name);
                    foreach (var e5 in e3.Properties)
                        e4.SetAttributeValue(e5.Key, e5.Value);

                    element.Add(e4);
                }

                xml.Root.Add(element);
            }

            foreach (var c in xbn.RootElement.Childs)
            {
                var element = new XElement(c.Name);
                foreach (var c2 in c.Properties)
                    element.SetAttributeValue(c2.Key, c2.Value);

                foreach (var c3 in c.Elements)
                {
                    var c4 = new XElement(c3.Name);
                    foreach (var c5 in c3.Properties)
                        c4.SetAttributeValue(c5.Key, c5.Value);

                    foreach (var c6 in c3.Childs)
                    {
                        var c7 = new XElement(c6.Name);
                        foreach (var c8 in c6.Properties)
                            c7.SetAttributeValue(c8.Key, c8.Value);

                        c4.Add(c7);
                    }

                    element.Add(c4);
                }

                xml.Root.Add(element);
            }

            xml.Save(path.Replace(".xbn", ".xml"));
        }
    }
}
