using System;
using System.IO;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;

namespace InfoControl.IO.Compression
{
    /// <summary>
    /// Classe que encapsular� todas as operações de compactação de arquivos em geral
    /// </summary>
    public class ZipFile : IDisposable
    {
        private ZipInputStream _zipInputStream;
        private ZipOutputStream _zipOutputStream;
        private string path;

        public ZipFile(Stream stream)
        {
            _zipOutputStream = new ZipOutputStream(stream);
            _zipInputStream = new ZipInputStream(stream);
        }

        /// <summary>
        /// Classe que encapsular� todas as operações de compactação de arquivos em geral
        /// </summary>
        public ZipFile(string path)
        {
            this.path = path;
        }

        #region IDisposable Members

        public void Dispose()
        {
            _zipInputStream.Dispose();
            _zipOutputStream.Dispose();
        }

        #endregion

        public void AddFile(string fileName, FileInfo file)
        {
            AddStream(fileName, file.OpenRead());
        }

        public void AddStream(string fileName, Stream stream)
        {
            if (_zipOutputStream == null)
                _zipOutputStream = new ZipOutputStream(File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite));

            //
            // Create a CRC value that identifies the file
            //
            var crc = new Crc32();
            crc.Reset();
            crc.Update((int)stream.Length);

            //
            // Create a Zip Entry 
            //
            var zipEntry = new ZipEntry(fileName);
            zipEntry.DateTime = DateTime.Now;
            zipEntry.Size = stream.Length;
            zipEntry.Crc = crc.Value;

            //
            // Attach the Zip Entry in ZipFile
            //
            _zipOutputStream.PutNextEntry(zipEntry);
            Pump(stream, _zipOutputStream);
            _zipOutputStream.CloseEntry();
            _zipOutputStream.Flush();
        }

        public void AddDirectory(string folderName, DirectoryInfo folder)
        {
            foreach (DirectoryInfo subfolder in folder.GetDirectories())
            {
                AddDirectory(folderName + "\\" + subfolder.Name, subfolder);
            }

            foreach (FileInfo file in folder.GetFiles())
            {
                AddFile(folderName + "\\" + file.Name, file);
            }
        }

        public void Save()
        {
            _zipOutputStream.Close();
            _zipInputStream.Close();
        }

        public void Extract(string pathTo)
        {
            pathTo = Path.GetFullPath(pathTo);

            if (_zipInputStream == null)
                _zipInputStream = new ZipInputStream(File.Open(path, FileMode.Open, FileAccess.Read));

            ZipEntry theEntry;
            using (_zipInputStream)
                while ((theEntry = _zipInputStream.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    string fullPath = Path.Combine(pathTo, Path.Combine(directoryName, fileName));

                    var e = new UnZippingEventArgs(fullPath);
                    OnUnZipping(this, e);
                    if (!e.Cancel)
                    {
                        directoryName = Path.GetDirectoryName(fullPath);
                        if (!Directory.Exists(directoryName))
                            Directory.CreateDirectory(directoryName);

                        if (!String.IsNullOrEmpty(fileName))
                            using (FileStream fileStream = File.Create(fullPath))
                                Pump(_zipInputStream, fileStream);

                        OnUnZipped(this, new UnZippedEventArgs(fullPath));
                    }
                }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        internal void Pump(Stream input, Stream output)
        {
            // Como o tamanho � desconhecido então cria um buffer pequeno de 32K para ler o stream aos poucos
            var buffer = new byte[32768];

            int read;

            // Indica quantos bytes conseguiu ler
            while ((read = input.Read(buffer, 0, buffer.Length)) != 0)
            {
                // Joga do buffer para o MemoryStream
                output.Write(buffer, 0, read);
            }
        }

        #region Events

        public event EventHandler<UnZippingEventArgs> UnZipping;
        public event EventHandler<UnZippedEventArgs> UnZipped;

        protected void OnUnZipping(object sender, UnZippingEventArgs e)
        {
            if (UnZipping != null)
                UnZipping(sender, e);
        }

        protected void OnUnZipped(object sender, UnZippedEventArgs e)
        {
            if (UnZipped != null)
                UnZipped(sender, e);
        }

        #endregion
    }

    public class UnZippedEventArgs : EventArgs
    {
        public UnZippedEventArgs(string fullPath)
        {
            FullPath = fullPath;
        }

        public string FullPath { get; private set; }
    }

    public class UnZippingEventArgs : UnZippedEventArgs
    {
        public UnZippingEventArgs(string fullPath)
            : base(fullPath) { }

        public bool Cancel { get; set; }
    }
}