using System;
using System.Collections.Generic;
using System.Text;

namespace DowntownRP.Utilities
{
    public class Generate
    {
        public static string CreateIBANBank()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }
    }
}
