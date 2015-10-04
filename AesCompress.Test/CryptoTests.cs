using NUnit.Framework;

namespace AesCompress.Test
{
    public class CryptoTests
    {
        [Test]
        public void Given_A_String_Should_Decrypt()
        {
            string secret = "Something to hide";
            var key = Crypto.GenerateKeys();
            string encryptedCompressed = Crypto.CompressEncryptString(secret, key);

            Assert.That(secret, Is.Not.EqualTo(encryptedCompressed));

            Assert.That(secret, Is.EqualTo(Crypto.DecompressDecryptString(encryptedCompressed, key)));
        }
    }
}
