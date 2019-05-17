using Serilog;
using System.IO;
using System.Xml.Linq;
using XBNLibrary;
using XBNLibrary.structure;

namespace XBNLoader.Converters
{
    public static class XMLtoXBN
    {
        private static readonly ILogger logger = Log.ForContext(Serilog.Core.Constants.SourceContextPropertyName, nameof(XMLtoXBN));

        public static void Convert(string path)
        {
            logger.Information($"Converting file {path.Replace(Directory.GetCurrentDirectory(), "")}");

            var xml = XDocument.Load(path);

            var xbn = new XBN();

            foreach (var e in xml.Elements())
            {
                if (!xbn.Header.Tags.Contains(e.Name.LocalName))
                    xbn.Header.Tags.Add(e.Name.LocalName);

                xbn.RootElement.Name = e.Name.LocalName;
            }

            foreach (var e2 in xml.Root.Elements())
            {
                if (!xbn.Header.Tags.Contains(e2.Name.LocalName))
                    xbn.Header.Tags.Add(e2.Name.LocalName);

                var xbnElement = new XBN_Element(xbn.Header);
                xbnElement.Name = e2.Name.LocalName;

                foreach (var a in e2.Attributes())
                {
                    xbnElement.Properties.Add(new XBN_Property(xbn.Header, a.Name.LocalName, a.Value));

                    if (!xbn.Header.Properties.Contains(a.Name.LocalName))
                        xbn.Header.Properties.Add(a.Name.LocalName);
                }

                foreach (var e3 in e2.Elements())
                {
                    var child = new XBN_Element(xbn.Header);
                    child.Name = e3.Name.LocalName;

                    foreach (var a2 in e3.Attributes())
                    {
                        child.Properties.Add(new XBN_Property(xbn.Header, a2.Name.LocalName, a2.Value));

                        if (!xbn.Header.Properties.Contains(a2.Name.LocalName))
                            xbn.Header.Properties.Add(a2.Name.LocalName);
                    }

                    if (!xbn.Header.Tags.Contains(e3.Name.LocalName))
                        xbn.Header.Tags.Add(e3.Name.LocalName);

                    xbnElement.Childs.Add(child);
                }

                xbn.RootElement.Elements.Add(xbnElement);
            }

            xbn.Save(path.Replace(".xml", ".xbn"));
        }
    }
}
