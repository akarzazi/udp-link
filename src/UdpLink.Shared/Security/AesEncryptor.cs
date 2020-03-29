using System.Security.Cryptography;

namespace UdpLink.Shared.Security
{
    public class AesEncryptor
    {
        public const int KeySize = 32;

        public static byte[] Encrypt(byte[] data, byte[] key, out byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Key = key;
                aes.GenerateIV(); // Ensure we use a new IV for each encryption

                using (var cryptoTransform = aes.CreateEncryptor())
                {
                    iv = aes.IV;
                    return cryptoTransform.TransformFinalBlock(data, 0, data.Length);
                }
            }
        }

        public static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC; // same as for encryption

                using (var cryptoTransform = aes.CreateDecryptor())
                {
                    return cryptoTransform.TransformFinalBlock(data, 0, data.Length);
                }
            }
        }
    }
}
