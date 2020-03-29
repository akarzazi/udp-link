using System;
using System.Collections.Generic;
using System.Text;
using UdpLink.Shared.Data;
using UdpLink.Shared.Security;

namespace UdpLink.Shared.Helpers
{
    public class PayloadHelper
    {
        public static Payload CreatePayload(byte[] rawBytes, string secret)
        {
            var keyBytes = KeyDerivedBytes.GetBytes(secret, AesEncryptor.KeySize);
            var encryptedCommand = AesEncryptor.Encrypt(rawBytes, keyBytes, out byte[] vector);

            return new Payload()
            {
                Vector = vector,
                EncryptedData = encryptedCommand,
            };
        }

        public static byte[] CreatePayloadBytes(byte[] rawBytes, string secret)
        {
            return CreatePayload(rawBytes, secret).Serialize();
        }

        public static byte[] DecodePayload(Payload payload, string secret)
        {
            var keyBytes = KeyDerivedBytes.GetBytes(secret, AesEncryptor.KeySize);
            return AesEncryptor.Decrypt(payload.EncryptedData, keyBytes, payload.Vector);
        }

        public static byte[] DecodePayloadBytes(byte[] encodedBytes, string secret)
        {
            var payload = Payload.Deserialize(encodedBytes);
            return DecodePayload(payload, secret);


        }
    }
}
