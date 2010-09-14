using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using InfoControl.Web.UI.WebControls.BarCode.Encoder;

namespace InfoControl.Web.UI.WebControls.BarCode
{
    public class ImageHandler : IHttpHandler
    {
        private bool addcheckdigit;
        private bool addcheckdigittotext;
        private double aspectratio;
        private Color backcolor;
        private int barheight;
        private bool bearerbars;
        private string bottomcomment;
        private int bottomcommentbottommargin;
        private int bottomcommentleftmargin;
        private int datamatrixmodulesize = 0x17;
        private int datamatrixsize = 0;
        private int encodationmode = 0;
        private string fontname;
        private int fontsize;
        private Color forecolor;
        private string imageformat;
        private int leftmargin;
        private int maxcols;
        private int maxrows;
        private int moduleheight;
        private int modulewidth;
        private int narrowbarwidth;
        private int ratio;
        private int rotation;
        private int securitylevel;
        private bool showtext;
        private int symbolheight;
        private int symbology;
        private int symbolwidth;
        private int textalignment;
        private int textmargin;
        private string texttoencode;
        private bool texttostretch;
        private string topcomment;
        private int topcommentleftmargin;
        private int topcommenttopmargin;
        private int topmargin;
        private bool truncated;

        private Bitmap GetDataMatrixBitmap()
        {
            DataMatrixImage image = new DataMatrixImage();
            image.backcolor = this.backcolor;
            image.forecolor = this.forecolor;
            image.txtFont = new Font(this.fontname, (float) this.fontsize);
            image.moduleheight = this.datamatrixmodulesize;
            image.modulewidth = this.datamatrixmodulesize;
            image.symbolheight = this.symbolheight;
            image.symbolwidth = this.symbolwidth;
            image.bottomcomment = this.bottomcomment;
            image.bottomcommentbottommargin = this.bottomcommentbottommargin;
            image.bottomcommentleftmargin = this.bottomcommentleftmargin;
            image.leftmargin = this.leftmargin;
            image.topmargin = this.topmargin;
            image.topcomment = this.topcomment;
            image.topcommentleftmargin = this.topcommentleftmargin;
            image.topcommenttopmargin = this.topcommenttopmargin;
            image.encodebinarystr = this.GetDataMatrixStr();
            Bitmap bitmap = new Bitmap(this.symbolwidth, this.symbolheight);
            Graphics g = Graphics.FromImage(bitmap);
            image.DrawDataMatrix(ref g);
            g.Dispose();
            return bitmap;
        }

        private string GetDataMatrixStr()
        {
            DataMatrixEncoder encoder = new DataMatrixEncoder();
            encoder.targetSizeID = this.datamatrixsize;
            encoder.m_nPreferredEncodation = this.encodationmode;
            return encoder.Encode(this.texttoencode);
        }

        private Bitmap GetLinearBitmap()
        {
            string checkStr;
            string demoStr;
            string drawText;
            LinearEncoder encoder = new LinearEncoder();
            encoder.narrowratio = this.ratio;
            if (((this.symbology != 0) && (this.symbology != 10)) && ((this.symbology != 11) && (this.symbology != 13)))
            {
                this.addcheckdigit = true;
            }
            string text4 = encoder.Encoding(this.symbology, ref this.texttoencode, this.addcheckdigit, out checkStr, out demoStr, out drawText);
            Bitmap bitmap = new Bitmap(this.symbolwidth, this.symbolheight);
            Graphics g = Graphics.FromImage(bitmap);
            LinearImage image = new LinearImage();
            image.encodebinarystr = text4;
            image.encodestr = this.texttoencode;
            image.symbolwidth = this.symbolwidth;
            image.symbolheight = this.symbolheight;
            image.narrowbarwidth = this.narrowbarwidth;
            image.barheight = this.barheight;
            image.leftmargin = this.leftmargin;
            image.topmargin = this.topmargin;
            image.textmargin = this.textmargin;
            image.forecolor = this.forecolor;
            image.backcolor = this.backcolor;
            image.symbology = this.symbology;
            image.checkstr = checkStr;
            image.addcheckdigittotext = this.addcheckdigittotext;
            image.showtext = this.showtext;
            image.txtFont = new Font(this.fontname, (float) this.fontsize);
            image.topcomment = this.topcomment;
            image.bottomcomment = this.bottomcomment;
            image.topcommenttopmargin = this.topcommenttopmargin;
            image.topcommentleftmargin = this.topcommentleftmargin;
            image.bottomcommentleftmargin = this.bottomcommentleftmargin;
            image.bottomcommentbottommargin = this.bottomcommentbottommargin;
            image.bearerbars = this.bearerbars;
            image.texttostretch = this.texttostretch;
            image.resolutiondpi = 0x60;
            image.textalignment = this.textalignment;
            image.DrawLinear(ref g);
            g.Dispose();
            return bitmap;
        }

        private Bitmap GetPDF417Bitmap()
        {
            PDF417Image image = new PDF417Image();
            image.backcolor = this.backcolor;
            image.forecolor = this.forecolor;
            image.txtFont = new Font(this.fontname, (float) this.fontsize);
            image.moduleheight = this.moduleheight;
            image.modulewidth = this.modulewidth;
            image.symbolheight = this.symbolheight;
            image.symbolwidth = this.symbolwidth;
            image.bottomcomment = this.bottomcomment;
            image.bottomcommentbottommargin = this.bottomcommentbottommargin;
            image.bottomcommentleftmargin = this.bottomcommentleftmargin;
            image.leftmargin = this.leftmargin;
            image.topmargin = this.topmargin;
            image.topcomment = this.topcomment;
            image.topcommentleftmargin = this.topcommentleftmargin;
            image.topcommenttopmargin = this.topcommenttopmargin;
            image.encodebinarystr = this.GetPDF417Str();
            Bitmap bitmap = new Bitmap(this.symbolwidth, this.symbolheight);
            Graphics g = Graphics.FromImage(bitmap);
            image.DrawPDF417(ref g);
            g.Dispose();
            return bitmap;
        }

        private string GetPDF417Str()
        {
            PDF417Encoder encoder = new PDF417Encoder();
            encoder.aspectRatio = this.aspectratio;
            encoder.codeColumns = this.maxcols;
            encoder.codeRows = this.maxrows;
            encoder.moduleheight = this.moduleheight;
            encoder.modulewidth = this.modulewidth;
            encoder.errorLevel = this.securitylevel;
            encoder.isTruncated = this.truncated;
            return encoder.Encode(this.texttoencode);
        }

        public void ProcessRequest(HttpContext context)
        {
            Bitmap dataMatrixBitmap = null;
            this.VarInit(context);
            if (this.symbology == 20)
            {
                dataMatrixBitmap = this.GetPDF417Bitmap();
            }
            if (this.symbology == 0x15)
            {
                dataMatrixBitmap = this.GetDataMatrixBitmap();
            }
            else if (this.symbology < 20)
            {
                dataMatrixBitmap = this.GetLinearBitmap();
            }
            switch (this.rotation)
            {
                case 1:
                    dataMatrixBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;

                case 2:
                    dataMatrixBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;

                case 3:
                    dataMatrixBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
            }
            switch (this.imageformat)
            {
                case "Bmp":
                {
                    ImageFormat bmp = ImageFormat.Bmp;
                    break;
                }
                case "Gif":
                {
                    ImageFormat gif = ImageFormat.Gif;
                    break;
                }
                case "Jpeg":
                {
                    ImageFormat jpeg = ImageFormat.Jpeg;
                    break;
                }
                case "Png":
                {
                    ImageFormat png = ImageFormat.Png;
                    break;
                }
                default:
                {
                    ImageFormat format5 = ImageFormat.Jpeg;
                    break;
                }
            }
            MemoryStream stream = new MemoryStream();
            dataMatrixBitmap.Save(stream, ImageFormat.Jpeg);
            HttpContext.Current.Response.ContentType = "image/" + this.imageformat.ToString();
            stream.WriteTo(HttpContext.Current.Response.OutputStream);
            dataMatrixBitmap.Dispose();
            stream.Close();
        }

        private void VarInit(HttpContext context)
        {
            this.symbology = Convert.ToInt32(context.Request["symbology"]);
            this.rotation = Convert.ToInt32(context.Request["rotation"]);
            this.symbolheight = Convert.ToInt32(context.Request["height"]);
            this.symbolwidth = Convert.ToInt32(context.Request["width"]);
            this.texttoencode = context.Request["text"];
            this.topcomment = context.Request["topcomment"];
            this.bottomcomment = context.Request["bottomcomment"];
            this.leftmargin = Convert.ToInt32(context.Request["leftmargin"]);
            this.topmargin = Convert.ToInt32(context.Request["topmargin"]);
            this.topcommentleftmargin = Convert.ToInt32(context.Request["topcommentleftmargin"]);
            this.topcommenttopmargin = Convert.ToInt32(context.Request["topcommenttopmargin"]);
            this.bottomcommentleftmargin = Convert.ToInt32(context.Request["bottomcommentleftmargin"]);
            this.bottomcommentbottommargin = Convert.ToInt32(context.Request["bottomcommentbottommargin"]);
            this.imageformat = context.Request["imageformat"];
            this.forecolor = Color.FromName(context.Request["forecolor"]);
            this.backcolor = Color.FromName(context.Request["backcolor"]);
            this.fontname = context.Request["fontname"];
            this.fontsize = Convert.ToInt32(context.Request["fontsize"]);
            this.textmargin = Convert.ToInt32(context.Request["textmargin"]);
            this.narrowbarwidth = Convert.ToInt32(context.Request["narrowbarwidth"]);
            this.barheight = Convert.ToInt32(context.Request["barheight"]);
            this.showtext = context.Request["showtext"] == "True";
            this.addcheckdigit = context.Request["addcheckdigit"] == "True";
            this.addcheckdigittotext = context.Request["addcheckdigittotext"] == "True";
            this.ratio = Convert.ToInt32(context.Request["ratio"]);
            this.bearerbars = context.Request["bearerbars"] == "True";
            this.texttostretch = context.Request["texttostretch"] == "True";
            this.textalignment = Convert.ToInt32(context.Request["textalignment"]);
            this.aspectratio = Convert.ToDouble(context.Request["aspectratio"]);
            this.maxcols = Convert.ToInt32(context.Request["maxcols"]);
            this.maxrows = Convert.ToInt32(context.Request["maxrows"]);
            this.securitylevel = Convert.ToInt32(context.Request["securitylevel"]);
            this.truncated = context.Request["truncated"] == "True";
            this.modulewidth = Convert.ToInt32(context.Request["modulewidth"]);
            this.moduleheight = Convert.ToInt32(context.Request["moduleheight"]);
            this.encodationmode = Convert.ToInt32(context.Request["encodationmode"]);
            this.datamatrixsize = Convert.ToInt32(context.Request["datamatrixsize"]);
            this.datamatrixmodulesize = Convert.ToInt32(context.Request["datamatrixmodulesize"]);
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}

