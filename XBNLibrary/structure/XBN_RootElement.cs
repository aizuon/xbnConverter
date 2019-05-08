using System;
using System.Collections.Generic;
using System.IO;

namespace XBNLibrary.structure
{
    public class XBN_RootElement
    {
        private XBN_Header Header { get; set; }

        public string Name { get; set; }

        public short Unk1 { get; set; }
        public short Unk2 { get; set; }

        public List<XBN_RootElement> Childs { get; set; }
        public List<XBN_Property> Properties { get; set; }
        public List<XBN_Element> Elements { get; set; }

        public XBN_RootElement(XBN_Header header)
        {
            Header = header;

            Elements = new List<XBN_Element>();
            Childs = new List<XBN_RootElement>();
            Properties = new List<XBN_Property>();
        }

        public static XBN_RootElement Deserialize(BinaryReader reader, XBN_Header header)
        {
            if (reader == null)
                throw new Exception("Reader is null");

            if (header == null)
                throw new Exception("Header is null");

            string mainTag = header.Tags[reader.ReadInt16()];
            short unk1 = reader.ReadInt16();
            short unk2 = reader.ReadInt16();

            short totalElements = reader.ReadInt16();

            var elements = new List<XBN_Element>();
            var properties = new List<XBN_Property>();
            var childs = new List<XBN_RootElement>();

            if (unk1 > 0)
            {
                for (int i = 0; i < totalElements; i++)
                {
                    var element = XBN_Property.Deserialize(reader, header);
                    properties.Add(element);
                }

                childs.Add(XBN_RootElement.Deserialize(reader, header));
            }
            else
            {
                for (int i = 0; i < totalElements; i++)
                {
                    var element = XBN_Element.Deserialize(reader, header);
                    elements.Add(element);
                }
            }

            return new XBN_RootElement(header)
            {
                Name = mainTag,
                Unk1 = unk1,
                Unk2 = unk2,
                Childs = childs,
                Elements = elements,
                Properties = properties
            };
        }

        public void Serialize(BinaryWriter writer)
        {
            if (writer == null)
                throw new Exception("Writer is null");

            writer.Write((short)Header.Tags.IndexOf(Name));
            writer.Write(Unk1);
            writer.Write(Unk2);
            writer.Write((short)Elements.Count);

            if (Unk1 > 0)
            {
                for (int i = 0; i < Properties.Count; i++)
                    Properties[i].Serialize(writer);

                for (int i = 0; i < Childs.Count; i++)
                    Childs[i].Serialize(writer);
            }
            else
            {
                for (int i = 0; i < Elements.Count; i++)
                    Elements[i].Serialize(writer);
            }
        }
    }
}
