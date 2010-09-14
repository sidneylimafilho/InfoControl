using System;
using System.IO;
using System.Text;
using System.Collections;

namespace InfoControl.Net.Mail
{
    /// <summary>
    /// Byte[] line parser.
    /// </summary>
    public class StreamHelper
    {
        private Stream m_StrmSource = null;
        Stream stream;
        System.Text.Encoding encoding;
        StreamReader reader;
        StreamWriter writer;



        public StreamHelper(Stream str, System.Text.Encoding enc)
        {
            stream = str;
            encoding = enc ?? System.Text.Encoding.Default;
            reader = new StreamReader(str, encoding);
            writer = new StreamWriter(str, encoding);
        }


        #region function ReadLine

        /// <summary>
        /// Reads byte[] line from stream.
        /// </summary>
        /// <returns>Return null if end of stream reached.</returns>
        public byte[] ReadLine()
        {
            MemoryStream strmLineBuf = new MemoryStream();
            byte prevByte = 0;

            int currByteInt = m_StrmSource.ReadByte();
            while (currByteInt > -1)
            {
                strmLineBuf.WriteByte((byte)currByteInt);

                // Line found
                if ((prevByte == (byte)'\r' && (byte)currByteInt == (byte)'\n'))
                {
                    strmLineBuf.SetLength(strmLineBuf.Length - 2); // Remove <CRLF>

                    return strmLineBuf.ToArray();
                }

                // Store byte
                prevByte = (byte)currByteInt;

                // Read next byte
                currByteInt = m_StrmSource.ReadByte();
            }

            // Line isn't terminated with <CRLF> and has some bytes left, return them.
            if (strmLineBuf.Length > 0)
            {
                return strmLineBuf.ToArray();
            }

            return null;
        }

        public string ReadToEnd()
        {
            return reader.ReadLine();
        }

        #endregion
        public void Write(string text)
        {
            byte[] bts = encoding.GetBytes(text);
            stream.Write(bts, 0, bts.Length);
        }
    }
}
