namespace InfoControl.Text.HtmlToRtf
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    public class SautinImage
    {
        private string cid = string.Empty;
        private Image img = null;
        private MemoryStream MS;

        public SautinImage(string Cid_, Image Img_)
        {
            MemoryStream stream = new MemoryStream();
            Img_.Save(stream, ImageFormat.Png);
            this.img = Image.FromStream(stream, true);
            this.cid = Cid_;
        }

        public void ClearImage()
        {
            this.MS.Close();
        }

        public string Cid
        {
            get
            {
                return this.cid;
            }
        }

        public Image Img
        {
            get
            {
                return this.img;
            }
        }
    }
}

