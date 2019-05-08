using System;
using System.IO;

namespace XBNLibrary.structure
{
    public class XBN_Property
    {
        private XBN_Header Header { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }

        public XBN_Property(XBN_Header header)
        {
            Header = header;
        }

        public XBN_Property(XBN_Header header, string key, string value)
          : this(header)
        {
            Key = key;
            Value = value;
        }

        public static XBN_Property Deserialize(BinaryReader reader, XBN_Header header)
        {
            if (reader == null)
                throw new Exception("Reader is null");

            if (header == null)
                throw new Exception("Header is null");

            string propertyKey = header.Properties[reader.ReadInt16()];
            string propertyValue = reader.ReadXBNStr();

            return new XBN_Property(header, propertyKey, propertyValue);
        }

        public void Serialize(BinaryWriter writer)
        {
            if (writer == null)
                throw new Exception("Writer is null");

            writer.Write((short)Header.Properties.IndexOf(Key));
            writer.WriteXBNStr(Value);
        }
    }
}
