using System.IO;

using XBNLibrary.cryptography;

namespace XBNLibrary
{
    public static class Extensions
    {
        public static string ReadXBNStr(this BinaryReader @this)
        {
            int len = @this.ReadUInt16();
            byte[] crypted = @this.ReadBytes(len);

            return XBNCrypt.Decrypt(crypted);
        }

        public static void WriteXBNStr(this BinaryWriter @this, string str)
        {
            byte[] crypted = XBNCrypt.Encrypt(str);

            @this.Write((short)crypted.Length);
            @this.Write(crypted);
        }
    }
}
