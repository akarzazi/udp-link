namespace UdpLink.Shared.Data
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    [Serializable]
    public class Payload
    {
        public const int VectorSize = 16;

        public byte[] Vector { get; set; }
        public byte[] EncryptedData { get; set; }

        public byte[] Serialize()
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, this);
                return ms.ToArray();
            }
        }

        public static Payload Deserialize(byte[] data)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(data, 0, data.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                return (Payload)binForm.Deserialize(memStream);
            }
        }
    }
}