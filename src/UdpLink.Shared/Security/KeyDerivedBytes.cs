using System.Security.Cryptography;
using System.Text;

namespace UdpLink.Shared.Security
{
    public class KeyDerivedBytes
    {
        public static byte[] GetBytes(string key, int size)
        {
            var salt = Encoding.UTF8.GetBytes("does_not_matter");
            using (var derivedBytes = new System.Security.Cryptography.Rfc2898DeriveBytes(key, salt: salt, iterations: 50000, HashAlgorithmName.SHA256))
            {
                return derivedBytes.GetBytes(size);
            }
        }
    }
}
