using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Core.Hashes.Cryptography;
using System.Security.Cryptography;
using CafeLibrary;
using ByamlExt.Byaml;

namespace SampleMapEditor
{
    namespace Sead
    {
        internal class Random
        {
            private uint[] internalData;

            public Random(uint seed)
            {
                internalData = new uint[4];
                internalData[0] = 1812433253 * (seed ^ (seed >> 30)) + 1;
                internalData[1] = 1812433253 * (internalData[0] ^ (internalData[0] >> 30)) + 2;
                internalData[2] = 1812433253 * (internalData[1] ^ (internalData[1] >> 30)) + 3;
                internalData[3] = 1812433253 * (internalData[2] ^ (internalData[2] >> 30)) + 4;
            }

            public Random(uint seedOne, uint seedTwo, uint seedThree, uint seedFour)
            {
                internalData = new uint[] { seedOne, seedTwo, seedThree, seedFour };
            }

            public uint GetUInt32()
            {
                uint v1;
                uint v2;
                uint v3;

                v1 = internalData[0] ^ (internalData[0] << 11);
                internalData[0] = internalData[1];
                v2 = internalData[3];
                v3 = v1 ^ (v1 >> 8) ^ v2 ^ (v2 >> 19);
                internalData[1] = internalData[2];
                internalData[2] = v2;
                internalData[3] = v3;

                return v3;
            }
        }
    }

    public static class Nisasyst
    {
        private static string KeyMaterialString = "e413645fa69cafe34a76192843e48cbd691d1f9fba87e8a23d40e02ce13b0d534d10301576f31bc70b763a60cf07149cfca50e2a6b3955b98f26ca84a5844a8aeca7318f8d7dba406af4e45c4806fa4d7b736d51cceaaf0e96f657bb3a8af9b175d51b9bddc1ed475677260f33c41ddbc1ee30b46c4df1b24a25cf7cb6019794";
        private static char[] KeyMaterial = KeyMaterialString.ToCharArray();

        public static bool IsEncrypted(byte[] data)
        {
            var footer = Encoding.UTF8.GetString(data, data.Length - 8, 8);
            return footer == "nisasyst";
        }

        public static bool IsEncrypted(Stream stream)
        {
            var curPos = stream.Position;
            //stream.Position = stream.Length - 8;
            stream.Seek(-8, SeekOrigin.End);
            byte[] data = new byte[8];
            stream.Read(data, 0, 8);
            string footer = Encoding.UTF8.GetString(data);
            return footer == "nisasyst";
        }

        //public static BYAML DecryptByaml(SARC.SarcEntry entry)
        public static BymlFileData DecryptByaml(SARC.FileEntry entry)
        {
            UInt32 crc = Crc32.Compute(entry.FileName);
            Console.WriteLine($"Crc of \"{entry.FileName}\" is {crc.ToString("X")}");

            Sead.Random seadRandom = new Sead.Random(crc);

            // Create the encryption key and IV
            byte[] encryptionKey = CreateSequence(seadRandom);
            byte[] iv = CreateSequence(seadRandom);

            byte[] decData = new byte[entry.FileData.Length - 8];

            // Calculate AES
            using (MemoryStream ms = new MemoryStream())
            using (AesManaged cryptor = new AesManaged())
            {
                cryptor.Mode = CipherMode.CBC;
                cryptor.Padding = PaddingMode.PKCS7;
                cryptor.KeySize = 128;
                cryptor.BlockSize = 128;


                using (CryptoStream cryptoStream = new CryptoStream(ms, cryptor.CreateDecryptor(encryptionKey, iv), CryptoStreamMode.Write))
                {
                    // Seek to the beginning of the file
                    //using (MemoryStream memStream = new MemoryStream(entry.FileData))
                    using (MemoryStream memStream = (MemoryStream)entry.FileData)
                    {
                        // Copy the encrypted data
                        CopyStream(memStream, cryptoStream, (int)memStream.Length - 8);
                    }
                }

                decData = ms.ToArray();
            }

            //entry.FileData = decData;

            //BYAML byml = new BYAML();
            BymlFileData byml;
            using (MemoryStream memoryStream = new MemoryStream(decData))
            {
                //byml.Load(memoryStream);
                byml = ByamlFile.LoadN(memoryStream);
            }
            return byml;
        }

        private static byte[] CreateSequence(Sead.Random random)
        {
            // Create byte array
            byte[] sequence = new byte[16];

            // Create each byte
            for (int i = 0; i < sequence.Length; i++)
            {
                // Create empty byte string
                string byteString = "";

                // Get characters from key material
                byteString += KeyMaterial[random.GetUInt32() >> 24];
                byteString += KeyMaterial[random.GetUInt32() >> 24];

                // Parse the resulting byte
                sequence[i] = Convert.ToByte(byteString, 16);
            }

            // Return the sequence
            return sequence;
        }

        private static void CopyStream(Stream input, Stream output, int bytes)
        {
            byte[] buffer = new byte[32768];
            int read;
            while (bytes > 0 &&
                   (read = input.Read(buffer, 0, Math.Min(buffer.Length, bytes))) > 0)
            {
                output.Write(buffer, 0, read);
                bytes -= read;
            }
        }
    }
}
