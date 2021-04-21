using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace SymmetricEncryptingH4.Service
{
    public class EncryptionService
    {
        public SymmetricAlgorithm Algorithm { get; private set; }

        public string KeyAsBase64
        {
            get
            {
                return Convert.ToBase64String(Algorithm.Key);
            }
        }

        public string IVAsBase64
        {
            get
            {
                return Convert.ToBase64String(Algorithm.IV);
            }
        }


        public EncryptionService(HashingAlgorithm hashingAlgorithm)
        {
            switch (hashingAlgorithm)
            {
                case HashingAlgorithm.DES:
                    Algorithm = DES.Create();
                    break;
                case HashingAlgorithm.TripleDES:
                    Algorithm = TripleDES.Create();
                    break;
                case HashingAlgorithm.AES:
                    Algorithm = Aes.Create();
                    break;
                default:
                    break;
            }
            Algorithm.GenerateKey();
            Algorithm.GenerateIV();
        }


        public byte[] Encrypt(byte[] message)
        {
            
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms,Algorithm.CreateEncryptor(),CryptoStreamMode.Write);

            cs.Write(message, 0, message.Length);
            cs.Close();
            cs.Dispose();

            byte[] encryptedMessage = ms.ToArray();

            ms.Dispose();
            return encryptedMessage;

        }

        public byte[] Decrypt(byte[] message)
        {
            byte[] plainText = new byte[message.Length];


            MemoryStream ms = new MemoryStream(message);
            CryptoStream cs = new CryptoStream(ms, Algorithm.CreateDecryptor(), CryptoStreamMode.Read);

            cs.Read(plainText, 0, message.Length);
            cs.Close();
            cs.Dispose();

            ms.Dispose();

            return plainText;

        }

    }
}
