using System;
using System.IO;
using XBNLibrary.structure;

namespace XBNLibrary
{
    public class XBN
    {
        public XBN_Header Header { get; set; }
        public XBN_RootElement RootElement { get; set; }

        public XBN()
        {
            Header = new XBN_Header();
            RootElement = new XBN_RootElement(Header);
        }

        public static XBN Load(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var br = new BinaryReader(fs))
            {
                var header = XBN_Header.Deserialize(br);
                var element = XBN_RootElement.Deserialize(br, header);

                return new XBN
                {
                    Header = header,
                    RootElement = element
                };
            }
        }

        public void Save(string path)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            using (var bw = new BinaryWriter(fs))
            {
                if (Header == null)
                    throw new Exception("Header is null");

                if (RootElement == null)
                    throw new Exception("Element is null");

                Header.Serialize(bw);
                RootElement.Serialize(bw);
            }
        }
    }
}
