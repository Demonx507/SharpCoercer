using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCoercer.Helpers
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class RandomStringGenerator
    {
        // the characters we?ll allow in the output
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                                      "abcdefghijklmnopqrstuvwxyz" +
                                      "0123456789";

        public static string Generate(int length)
        {
            if (length < 1)
                throw new ArgumentException("Length must be at least 1.", nameof(length));

            // buffer to hold random bytes
            byte[] data = new byte[length];

            // fill with cryptographically strong random bytes
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(data);
            }

            // convert each byte into one of the allowed chars
            var result = new StringBuilder(length);
            int charsLength = _chars.Length;
            foreach (byte b in data)
            {
                result.Append(_chars[b % charsLength]);
            }

            return result.ToString();
        }
    }

}
