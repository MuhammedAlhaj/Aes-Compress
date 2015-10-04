using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace AesCompress
{
    public class Crypto
    {
        public static CryptoKey GenerateKeys()
        {
            using (AesCryptoServiceProvider desCrypto = new AesCryptoServiceProvider())
            {
                return new CryptoKey { Key = desCrypto.Key, Iv = desCrypto.IV };
            }
        }
        
        public static string CompressEncryptString(string textToEncrypt, CryptoKey key)
        {
            if (string.IsNullOrEmpty(textToEncrypt))
            {
                return string.Empty;
            }

            textToEncrypt = Compress(textToEncrypt);

            using (AesCryptoServiceProvider des = new AesCryptoServiceProvider { Key = key.Key, IV = key.Iv })
            using (ICryptoTransform desencrypt = des.CreateEncryptor())
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, desencrypt, CryptoStreamMode.Write))
                using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(textToEncrypt);
                }

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        private static string Compress(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    zip.Write(buffer, 0, buffer.Length);
                }

                ms.Position = 0;

                byte[] compressed = new byte[ms.Length];
                ms.Read(compressed, 0, compressed.Length);

                byte[] gzBuffer = new byte[compressed.Length + 4];
                Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
                Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
                return Convert.ToBase64String(gzBuffer);
            }
        }

        public static string DecompressDecryptString(string textToDecrypt, CryptoKey key)
        {
            if (string.IsNullOrEmpty(textToDecrypt))
            {
                return string.Empty;
            }

            string compressedText;

            byte[] bytes = Convert.FromBase64String(textToDecrypt);

            using (AesCryptoServiceProvider des = new AesCryptoServiceProvider { Key = key.Key, IV = key.Iv })
            using (ICryptoTransform desdecrypt = des.CreateDecryptor())
            using (MemoryStream msDecrypt = new MemoryStream(bytes))
            using (CryptoStream cryptoStream = new CryptoStream(msDecrypt, desdecrypt, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new StreamReader(cryptoStream))
            {
                compressedText = srDecrypt.ReadToEnd();
            }

            return Decompress(compressedText);
        }

        private static string Decompress(string compressedText)
        {
            byte[] gzBuffer = Convert.FromBase64String(compressedText);

            using (MemoryStream ms = new MemoryStream())
            {
                int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

                byte[] buffer = new byte[msgLength];

                ms.Position = 0;
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    zip.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }
    }
}
