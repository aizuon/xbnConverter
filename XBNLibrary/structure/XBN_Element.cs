using System;
using System.Collections.Generic;
using System.IO;

namespace XBNLibrary.structure
{
    public class XBN_Element
    {
        private XBN_Header Header { get; set; }

        public string Name { get; set; }
        public List<XBN_Property> Properties { get; set; }
        public List<XBN_Element> Childs { get; set; }

        public short Unk3 { get; set; }

        public XBN_Element(XBN_Header header)
        {
            Header = header;
            Properties = new List<XBN_Property>();
            Childs = new List<XBN_Element>();
        }

        public static XBN_Element Deserialize(BinaryReader reader, XBN_Header header)
        {
            if (reader == null)
                throw new Exception("Reader is null");

            if (header == null)
                throw new Exception("Header is null");

            string name = header.Tags[reader.ReadInt16()];
            short propertyCount = reader.ReadInt16();
            short unk3 = reader.ReadInt16();
            short tagCount = reader.ReadInt16();

            var properties = new List<XBN_Property>();
            for (int i = 0; i < propertyCount; i++)
                properties.Add(XBN_Property.Deserialize(reader, header));

            var elements = new List<XBN_Element>();
            for (int i = 0; i < tagCount; i++)
                elements.Add(XBN_Element.Deserialize(reader, header));

            return new XBN_Element(header)
            {
                Name = name,
                Unk3 = unk3,
                Properties = properties,
                Childs = elements
            };
        }

        public void Serialize(BinaryWriter writer)
        {
            if (writer == null)
                throw new Exception("Writer is null");

            writer.Write((short)Header.Tags.IndexOf(Name));
            writer.Write((short)Properties.Count);
            writer.Write(Unk3);
            writer.Write((short)Childs.Count);

            for (int i = 0; i < Properties.Count; i++)
                Properties[i].Serialize(writer);

            for (int i = 0; i < Childs.Count; i++)
                Childs[i].Serialize(writer);
        }
    }
}
