using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace PGen.Utils
{
    public static class CompressionHandler
    {

        public static string CompressUnicodeToBase64(string input)
        {
            var compressedBytes = CompressUnicodeToByteStream(input);
            return Convert.ToBase64String(compressedBytes);
        }


        public static string DecompressBase64ToUnicode(string input)
        {
            var bytes = Convert.FromBase64String(input);
            var decompressedBytes = DecompressByteStreamToByteStream(bytes);
            return Encoding.UTF8.GetString(decompressedBytes);

        }

        public static byte[] CompressUnicodeToByteStream(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    CopyTo(msi, gs);
                }
                return mso.ToArray();
            }
        }

      
        public static byte[] DecompressByteStreamToByteStream(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                    {
                        CopyTo(gs, mso);
                    }
                    return mso.ToArray();
                }
        }

        public static string DecompressStreamToString(Stream stream)
        {
            using (var msi = (MemoryStream)stream)
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                    {
                        CopyTo(gs, mso);
                    }
                return DecompressByteStream(mso.ToArray()).ToString();
                }
        }

        private static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }
        
        
        public static byte[] CompressByteStream(byte[] bytes)
        {
            using (MemoryStream memstr = new MemoryStream())
                using (GZipStream gzs = new GZipStream(memstr, CompressionLevel.Optimal, true))
                {
                    gzs.Write(bytes, 0, bytes.Length);
                    return memstr.ToArray();
                }
        }
        
        public static byte[] DecompressByteStream(byte[] gzip)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip),
                                  CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        public static byte[] CompressBase64String(string input)
        {
            byte[] barr = Convert.FromBase64String(input);
            return CompressByteStream(barr);
        }

        public static byte[] DecompressBase64String(string input)
        {
            byte[] barr = Convert.FromBase64String(input);
            return DecompressByteStream(barr);
        }
    }
}