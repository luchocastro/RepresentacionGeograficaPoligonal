using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Hexagon.Model.Models
{
    public class HashHelper
    {
        public static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        public static string sha256Hashed(string ToHashed)
        {
            var ret = "";
            using (SHA256 sha256Hash = SHA256.Create())
            {
                ret = HashHelper.GetHash(sha256Hash, ToHashed);
            }
            return ret;
        }
    }
}
