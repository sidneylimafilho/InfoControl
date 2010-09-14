using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace InfoControl.IO.Compression
{
    public static class StringExtensions
    {
        #region Compression

        public static string Compress(this string identifier)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(identifier);
            var compressedStream = new MemoryStream();

            using (var compactor = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                compactor.Write(buffer, 0, buffer.Length);
                compactor.Close();
            }

            return Convert.ToBase64String(compressedStream.ToArray());
        }

        public static string Decompress(this string identifier)
        {
            var input = new MemoryStream(Convert.FromBase64String(identifier));
            var output = new MemoryStream();

            using (var decompactor = new GZipStream(input, CompressionMode.Decompress))
            {
                // Indica quantos bytes conseguiu ler
                int read;
                var buffer = new byte[32768];
                while ((read = decompactor.Read(buffer, 0, buffer.Length)) != 0)
                {
                    // Joga do buffer para o MemoryStream
                    output.Write(buffer, 0, read);
                }
            }

            return Encoding.UTF8.GetString(output.ToArray(), 0, (int) output.Length);
        }

        #endregion
    }
}