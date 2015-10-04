# Aes-Compress
Aes encryption with GZip compression

----
## What is Aes-Compress?
see [Wikipedia AES](https://en.wikipedia.org/wiki/Advanced_Encryption_Standard)

see [Wikipedia GZip](https://en.wikipedia.org/wiki/Gzip)


> Aes-Compress combines GZip compression with Aes encryption. The library has only a few methods.

----
## usage
1. Crypto.GenrateKeys
2. Crypto.CompressEncryptString
3. Crypto.DecompressDecryptString

----

    string secret = "Something to hide";
    var key = Crypto.GenerateKeys();
    string encryptedCompressed = Crypto.CompressEncryptString(secret, key);

    Assert.AreEqual(secret, Crypto.DecompressDecryptString(encryptedCompressed, key);

----
## changelog
* 04-Oct-2015 Initial Commit