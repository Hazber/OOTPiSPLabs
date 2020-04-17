using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using PluginInterface;

namespace Cryptography_Plugin
{
    [Plugin(PluginType.Cryptography)]
    public class MyPlugin:ICrypography
    {
       

        public string Name
        {
            get { return "AES256 Chyper Plugin"; }
        }

        public string Version
        {
            get { return "1.0.0"; }
        }

        public string Author
        {
            get { return "Me"; }
        }

        public string ChyperName
        {
            get { return "AES256"; }
        }

        const string initVector = "@1B2c3D4e5F6g7H8";     // Must be 16 bytes
        const string passPhrase = "Pas5pr@se";            // Any string
        const string saltValue = "s@1tValue";             // Any string
        const string hashAlgorithm = "SHA1";              // Can also be "MD5", "SHA1" is stronger
        const int passwordIterations = 2;                 // Can be any number, usually 1 or 2       
        const int keySize = 256;                          // Allowed values: 192, 128 or 256
        private static string randomKeyText = "챼\u07bbﰾṦ챻챰籶❠⨘뜱湋驴礉";
       

        static byte[] GetBytes(string randomKeyText)
        {
            byte[] bytes = new byte[randomKeyText.Length * sizeof(char)];
            System.Buffer.BlockCopy(randomKeyText.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public byte[] Encrypt(byte[] plainTextBytes)
        {
            byte[] chipherTextBytes = null;
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            //byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            /*PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            passPhrase,
                                                            saltValueBytes,
                                                            hashAlgorithm,
                                                            passwordIterations);
            byte[] keyBytes = password.GetBytes(keySize / 8);
            randomKeyText = GetString(keyBytes);*/
            byte[] keyBytes = GetBytes(randomKeyText);//
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
                                                             keyBytes,
                                                             initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(
                                                    memoryStream,
                                                    encryptor,
                                                    CryptoStreamMode.Write);

            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();

            chipherTextBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();
            return chipherTextBytes;
        }

        public byte[] Decrypt(byte[] chipherTextBytes)
        {
            byte[] plainTextBytes = null;
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            byte[] keyBytes = GetBytes(randomKeyText);
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                                                             keyBytes,
                                                             initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(chipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                          decryptor,
                                                          CryptoStreamMode.Read);
            plainTextBytes = new byte[chipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(
                                                    plainTextBytes,
                                                    0,
                                                    plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return plainTextBytes;
        }
    }
}
