using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Wpf.Security
{
    public static class EncryptText
    {
        public static string Encryptor(string stringToEncrypt, string key)
        {
            if (string.IsNullOrEmpty(stringToEncrypt))
                throw new ArgumentNullException("stringToEncrypt");
            else if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            byte[] saltArray = Encoding.ASCII.GetBytes(key.Length.ToString());
            var pdb = new PasswordDeriveBytes(key, saltArray);
            var RMCrypto = new RijndaelManaged();
            ICryptoTransform encryptor = RMCrypto.CreateEncryptor(pdb.GetBytes(32), pdb.GetBytes(16));

            var streamConnection = new MemoryStream();
            var cryptoStream = new CryptoStream(streamConnection, encryptor, CryptoStreamMode.Write);
            byte[] textArray = Encoding.Unicode.GetBytes(stringToEncrypt);
            cryptoStream.Write(textArray, 0, textArray.Length);

            cryptoStream.Close();
            streamConnection.Close();

            return Convert.ToBase64String(streamConnection.ToArray());
        }

        public static string Decryptor(string stringToDecrypt, string key)
        {
            if (string.IsNullOrEmpty(stringToDecrypt))
                throw new ArgumentNullException("stringToDecrypt");
            else if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            byte[] saltArray = Encoding.ASCII.GetBytes(key.Length.ToString());
            var pdb = new PasswordDeriveBytes(key, saltArray);
            var RMCrypto = new RijndaelManaged();
            ICryptoTransform decryptor = RMCrypto.CreateDecryptor(pdb.GetBytes(32), pdb.GetBytes(16));

            byte[] textArray = Convert.FromBase64String(stringToDecrypt);
            var streamConnection = new MemoryStream(textArray);
            var cryptoStream = new CryptoStream(streamConnection, decryptor, CryptoStreamMode.Read);

            byte[] resultArray = new byte[textArray.Length];
            cryptoStream.Read(resultArray, 0, resultArray.Length);

            cryptoStream.Close();
            streamConnection.Close();

            return Encoding.Unicode.GetString(resultArray, 0, resultArray.Length);
        }
    }
}
