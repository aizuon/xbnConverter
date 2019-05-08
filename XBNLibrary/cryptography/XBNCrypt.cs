using System;
using System.Text;

namespace XBNLibrary.cryptography
{
    public static class XBNCrypt
    {
        private static readonly ASCIIEncoding encoding;

        static XBNCrypt()
          => encoding = new ASCIIEncoding();

        #region Encryption Table
        //First 64 bytes of ASCII table
        private static readonly byte[] XBN_Table_A =
        {
      0xFF, 0xFB, 0xF7, 0xF3, 0xEF, 0xEB, 0xE7, 0xE3, 0xDF, 0xDB, 0xD7, 0xD3,
      0xCF, 0xCB, 0xC7, 0xC3, 0xBF, 0xBB, 0xB7, 0xB3, 0xAF, 0xAB, 0xA7, 0xA3,
      0x9F, 0x9B, 0x97, 0x93, 0x8F, 0x8B, 0x87, 0x83, 0x7F, 0x7B, 0x77, 0x73,
      0x6F, 0x6B, 0x67, 0x63, 0x5F, 0x5B, 0x57, 0x53, 0x4F, 0x4B, 0x47, 0x43,
      0x3F, 0x3B, 0x37, 0x33, 0x2F, 0x2B, 0x27, 0x23, 0x1F, 0x1B, 0x17, 0x13,
      0x0F, 0x0B, 0x07, 0x03
    };

        //Second 64 bytes of ASCII table
        private static readonly byte[] XBN_Table_B =
        {
      0xFE, 0xFA, 0xF6, 0xF2, 0xEE, 0xEA, 0xE6, 0xE2, 0xDE, 0xDA, 0xD6, 0xD2,
      0xCE, 0xCA, 0xC6, 0xC2, 0xBE, 0xBA, 0xB6, 0xB2, 0xAE, 0xAA, 0xA6, 0xA2,
      0x9E, 0x9A, 0x96, 0x92, 0x8E, 0x8A, 0x86, 0x82, 0x7E, 0x7A, 0x76, 0x72,
      0x6E, 0x6A, 0x66, 0x62, 0x5E, 0x5A, 0x56, 0x52, 0x4E, 0x4A, 0x46, 0x42,
      0x3E, 0x3A, 0x36, 0x32, 0x2E, 0x2A, 0x26, 0x22, 0x1E, 0x1A, 0x16, 0x12,
      0x0E, 0x0A, 0x06, 0x02
    };

        //Complete ASCII Table
        private static readonly byte[] ASCII_Table =
        {
      0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F,
      0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F,
      0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F,
      0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F,
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F,
      0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F,
      0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F,
      0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F
    };
        #endregion

        private static TableType getTable(byte value)
          => (value % 2) == 0x00 ? TableType.TableB : TableType.TableA;

        private static TableType getTableTypeByAsciiPosition(int position)
          => position > 0x40 ? TableType.TableB : TableType.TableA;

        public enum TableType
        {
            TableA,
            TableB
        }

        public static string Decrypt(byte @byte)
          => Decrypt(new byte[] { @byte });

        public static string Decrypt(byte[] array)
          => Decrypt(array, 0, array.Length);

        public static string Decrypt(byte[] array, int offset, int length)
        {
            if (array.Length <= 0)
                return "";

            string dec_str = string.Empty;
            for (int i = offset; i < length; i++)
            {
                int position = 0;
                var table = getTable(array[i]);

                switch (table)
                {
                    case TableType.TableA:
                        position = Array.IndexOf(XBN_Table_A, array[i]);
                        if (position <= 0)
                            throw new Exception("Invalid position");

                        dec_str += (char)ASCII_Table[position];
                        break;
                    case TableType.TableB:
                        position = Array.IndexOf(XBN_Table_B, array[i]);
                        if (position <= 0)
                            throw new Exception("Invalid position");

                        dec_str += (char)ASCII_Table[0x40 + position];
                        break;
                    default:
                        throw new Exception("Invalid byte array");
                }
            }

            return dec_str;
        }

        public static byte[] Encrypt(char @char)
          => Encrypt(new char[] { @char });

        public static byte[] Encrypt(char[] chars)
          => Encrypt(new string(chars));

        public static byte[] Encrypt(string str)
        {
            if (string.IsNullOrEmpty(str))
                throw new Exception("String empty");

            byte[] encrypted = new byte[str.Length];

            for (int i = 0; i < str.Length; i++)
            {
                int ascii_position = Array.IndexOf(ASCII_Table, (byte)str[i]);
                if (ascii_position <= 0)
                    throw new Exception("Invalid position");

                var table = getTableTypeByAsciiPosition(ascii_position);
                switch (table)
                {
                    case TableType.TableA:
                        encrypted[i] = XBN_Table_A[ascii_position];
                        break;
                    case TableType.TableB:
                        int newPosition = ascii_position - 0x40;
                        if (newPosition <= 0)
                            throw new Exception("Invalid position");

                        encrypted[i] = XBN_Table_B[newPosition];
                        break;
                    default:
                        throw new Exception("Invalid string");
                }
            }

            return encrypted;
        }
    }
}
