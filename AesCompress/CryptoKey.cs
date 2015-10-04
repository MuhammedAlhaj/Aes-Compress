using System.Text;

namespace AesCompress
{
    public class CryptoKey
    {
        public byte[] Key { get; set; }

        public byte[] Iv { get; set; }

        public string AnsiKey
        {
            get
            {
                return Encoding.ASCII.GetString(Key);
            }
        }

        public string AnsiIv
        {
            get
            {
                return Encoding.ASCII.GetString(Iv);
            }
        }
    }
}
