using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginInterface;
using System.IO;
using System.Security.Cryptography;

namespace RC2_Plugin
{
        [Plugin(PluginType.Cryptography)]
        public class MyPlugin : ICrypography
        {


            public string Name
            {
                get { return "RC2 Chyper Plugin"; }
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
                get { return "RC2"; }
            }

        static string RC2Key = "111111111111111";
        static string RC2IV = "00000000";
        public byte[] Encrypt(byte[] plainTextBytes)
        {
            byte[] Key = System.Text.Encoding.UTF8.GetBytes(RC2Key);
            byte[] IV = System.Text.Encoding.UTF8.GetBytes(RC2IV);
            MemoryStream mStream = new MemoryStream();
            RC2 RC2alg = RC2.Create();
            CryptoStream cStream = new CryptoStream(mStream, RC2alg.CreateEncryptor(Key, IV), CryptoStreamMode.Write);
            cStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cStream.FlushFinalBlock();
            byte[] cyperTextBytes = mStream.ToArray();
            cStream.Close();
            mStream.Close();
            return cyperTextBytes;
        }

        public byte[] Decrypt(byte[] Data)
        {
            byte[] Key = System.Text.Encoding.UTF8.GetBytes(RC2Key);
            byte[] IV = System.Text.Encoding.UTF8.GetBytes(RC2IV);
            MemoryStream mStream = new MemoryStream(Data);
            RC2 RC2alg = RC2.Create();

            CryptoStream cStream = new CryptoStream(mStream, RC2alg.CreateDecryptor(Key, IV), CryptoStreamMode.Read);

            /*StreamReader sReader = new StreamReader(cStream);

            string val = sReader.ReadLine();
            byte[] ret = System.Text.Encoding.UTF8.GetBytes(val);*/
            byte[] ret = new byte[Data.Length];
            int decryptedByteCount = cStream.Read(
                                                    ret,
                                                    0,
                                                    ret.Length);
            //sReader.Close();
            cStream.Close();
            mStream.Close();
            return ret;
        }
    }
}
