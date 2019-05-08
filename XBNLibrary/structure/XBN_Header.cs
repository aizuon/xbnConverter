using System;
using System.Collections.Generic;
using System.IO;

namespace XBNLibrary.structure
{
    public class XBN_Header
    {
        public byte[] Head { get; internal set; }

        public List<string> Tags { get; set; }
        public List<string> Properties { get; set; }

        public XBN_Header()
        {
            Head = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xCD, 0xCC, 0xCC, 0x3D };

            Tags = new List<string>();
            Properties = new List<string>();
        }

        public static XBN_Header Deserialize(BinaryReader reader)
        {
            if (reader == null)
                throw new Exception("Reader is null");

            byte[] head = reader.ReadBytes(8);
            short tagCount = reader.ReadInt16();

            var tags = new List<string>();
            for (int i = 0; i < tagCount; i++)
                tags.Add(reader.ReadXBNStr());

            short propertyCount = reader.ReadInt16();

            var properties = new List<string>();
            for (int i = 0; i < propertyCount; i++)
                properties.Add(reader.ReadXBNStr());

            return new XBN_Header
            {
                Head = head,
                Tags = tags,
                Properties = properties
            };
        }

        public void Serialize(BinaryWriter writer)
        {
            if (writer == null)
                throw new Exception("Writer is null");

            writer.Write(Head);
            writer.Write((short)Tags.Count);

            for (int i = 0; i < Tags.Count; i++)
                writer.WriteXBNStr(Tags[i]);

            writer.Write((short)Properties.Count);

            for (int i = 0; i < Properties.Count; i++)
                writer.WriteXBNStr(Properties[i]);
        }
    }
}
