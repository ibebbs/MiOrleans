using System;
using System.Security.Cryptography;
using System.Text;

namespace MiOrleans.Common
{
    public class StringToGuidConverter
    {
        public static StringToGuidConverter Default = new StringToGuidConverter();

        public Guid Convert(string value)
        {
            MD5 hasher = MD5.Create();

            byte[] data = hasher.ComputeHash(Encoding.Default.GetBytes(value));

            return new Guid(data);
        }
    }
}
