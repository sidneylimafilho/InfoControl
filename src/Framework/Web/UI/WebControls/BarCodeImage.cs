using System;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

using InfoControl.Web.UI.WebControls.BarCode;
using InfoControl.Web.UI.WebControls.BarCode.Encoder;

namespace InfoControl.Web.UI.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:BarCodeImage runat=server></{0}:BarCodeImage>")]
    [ToolboxBitmap(typeof(BarCodeImage))]
    [Designer(typeof(BarCodeImageDesigner))]
    public class BarCodeImage : WebControl
    {
        private bool addcheckdigit = true;
        private bool addcheckdigittotext = true;
        private double aspectratio = 1;
        private double barheight = 1000;
        private bool bearerbars = false;
        private bool binaryEncode = true;
        private string bottomcomment = "";
        private double bottomcommentbottommargin = 0;
        private double bottomcommentleftmargin = 0;
        private int datamatrixmodulesize = 0x17;
        private DataMatrixSize datamatrixsize = DataMatrixSize.SIZE_AUTO;
        private EncodationMode encodationmode = EncodationMode.E_AUTO;
        private ImageType imageformat = ImageType.Gif;
        private double leftmargin = 10;
        private int maxcols = 0;
        private int maxrows = 0;
        private double moduleheight = 30;
        private double modulewidth = 10;
        private double narrowbarwidth = 10;
        private Bitmap picture;
        private int ratio = 3;
        private int resolutiondpi = 72;
        private Rotation rotation = Rotation.Clockwise_Zero_Degree;
        private int securitylevel = 9;
        private bool showtext = true;
        private BarType barType = BarType.Code39;
        private string text = "1234567890";
        private TextAlignment textalignment = TextAlignment.Align_Center;
        private double textmargin = 0;
        private bool texttostretch = true;
        private string topcomment = "";
        private double topcommentleftmargin = 0;
        private double topcommenttopmargin = 0;
        private double topmargin = 10;
        private bool truncated = false;

        public BarCodeImage()
        {
            this.Width = new Unit("240px");
            this.Height = new Unit("150px");
            Font.Name = "Times New Roman";
            Font.Size = new FontUnit(10);
            ForeColor = Color.Black;
            BackColor = Color.White;


        }

        protected void CreatePicture()
        {
            int width = (((int)this.Width.Value) * this.resolutiondpi) / 0x60;
            int height = (((int)this.Height.Value) * this.resolutiondpi) / 0x60;
            string encodebinarystr = "";
            string str = this.text;
            string checkStr = "";
            if (this.barType == BarType.PDF417)
            {
                str = this.text;
            }
            if (this.barType == BarType.PDF417)
            {
                encodebinarystr = this.GetPDF417Str(0x60, str);
            }
            else if (this.barType == BarType.DataMatrix)
            {
                encodebinarystr = this.GetDataMatrixStr(str);
            }
            else if (this.barType < BarType.PDF417)
            {
                LinearEncoder encoder = new LinearEncoder();
                encoder.narrowratio = this.ratio;
                if (((this.barType != BarType.Code39) && (this.barType != BarType.Code25)) && ((this.barType != BarType.I25) && (this.barType != BarType.Codabar)))
                {
                    this.addcheckdigit = true;
                }
                string demoStr = null;
                string drawText = null;
                encodebinarystr = encoder.Encoding((int)this.barType, ref str, this.addcheckdigit, out checkStr, out demoStr, out drawText);
            }
            this.picture = new Bitmap(width, height);
            this.picture.SetResolution((float)this.resolutiondpi, (float)this.resolutiondpi);
            Graphics g = Graphics.FromImage(this.picture);
            if (this.barType == BarType.PDF417)
            {
                PDF417Image image = new PDF417Image();
                this.InitPDF417Img(ref this.picture, ref image, encodebinarystr, this.resolutiondpi, width, height);
                image.DrawPDF417(ref g);
            }
            else if (this.barType == BarType.DataMatrix)
            {
                DataMatrixImage datamatriximg = new DataMatrixImage();
                this.InitDataMatrixImg(ref this.picture, ref datamatriximg, encodebinarystr, this.resolutiondpi, width, height);
                datamatriximg.DrawDataMatrix(ref g);
            }
            else if (this.barType < BarType.PDF417)
            {
                LinearImage image3 = new LinearImage();
                this.InitLinearImg(ref this.picture, ref image3, checkStr, str, encodebinarystr, this.resolutiondpi, width, height);
                image3.DrawLinear(ref g);
            }
            g.Dispose();
        }

        private string GetDataMatrixStr(string str)
        {
            DataMatrixEncoder encoder = new DataMatrixEncoder();
            encoder.targetSizeID = (int)this.datamatrixsize;
            encoder.m_nPreferredEncodation = (int)this.encodationmode;
            encoder.binaryEncode = this.binaryEncode;
            return encoder.Encode(str);
        }

        private string GetPDF417Str(int resolutiondpi, string str)
        {
            PDF417Encoder encoder = new PDF417Encoder();
            encoder.aspectRatio = this.aspectratio;
            encoder.codeColumns = this.maxcols;
            encoder.codeRows = this.maxrows;
            encoder.moduleheight = (int)Math.Round((double)((this.moduleheight * resolutiondpi) / 1000));
            encoder.modulewidth = (int)Math.Round((double)((this.modulewidth * resolutiondpi) / 1000));
            encoder.errorLevel = this.securitylevel;
            encoder.isTruncated = this.truncated;
            return encoder.Encode(str);
        }

        private void InitDataMatrixImg(ref Bitmap outputBitmap, ref DataMatrixImage datamatriximg, string encodebinarystr, int resolutiondpi, int width, int height)
        {
            datamatriximg.encodebinarystr = encodebinarystr;
            datamatriximg.backcolor = this.BackColor;
            datamatriximg.forecolor = this.ForeColor;

            FontStyle style = FontStyle.Regular;
            style |= this.Font.Bold ? FontStyle.Bold : 0;
            style |= this.Font.Italic ? FontStyle.Italic : 0;
            style |= this.Font.Strikeout ? FontStyle.Strikeout : 0;
            style |= this.Font.Underline ? FontStyle.Underline : 0;
            datamatriximg.txtFont = new Font(this.Font.Name, (float)this.Font.Size.Unit.Value, style);

            datamatriximg.bottomcomment = this.bottomcomment;
            datamatriximg.topcomment = this.topcomment;
            datamatriximg.moduleheight = (int)Math.Round((double)((this.datamatrixmodulesize * resolutiondpi) / 0x3e8));
            datamatriximg.modulewidth = (int)Math.Round((double)((this.datamatrixmodulesize * resolutiondpi) / 0x3e8));
            datamatriximg.symbolheight = (((int)this.Height.Value) * resolutiondpi) / 0x60;
            datamatriximg.symbolwidth = (((int)this.Width.Value) * resolutiondpi) / 0x60;
            datamatriximg.bottomcommentbottommargin = (int)Math.Round((double)((this.bottomcommentbottommargin * resolutiondpi) / 1000));
            datamatriximg.bottomcommentleftmargin = (int)Math.Round((double)((this.bottomcommentleftmargin * resolutiondpi) / 10000));
            datamatriximg.leftmargin = (int)Math.Round((double)((this.leftmargin * resolutiondpi) / 1000));
            datamatriximg.topmargin = (int)Math.Round((double)((this.topmargin * resolutiondpi) / 1000));
            datamatriximg.topcommentleftmargin = (int)Math.Round((double)((this.topcommentleftmargin * resolutiondpi) / 1000));
            datamatriximg.topcommenttopmargin = (int)Math.Round((double)((this.topcommenttopmargin * resolutiondpi) / 1000));
            switch (this.rotation)
            {
                case Rotation.Clockwise_Zero_Degree:
                    break;

                case Rotation.Clockwise_90_Degree:
                    outputBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    return;

                case Rotation.Clockwise_180_Degree:
                    outputBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    return;

                case Rotation.Clockwise_270_Degree:
                    outputBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;

                default:
                    return;
            }
        }

        private void InitLinearImg(ref Bitmap outputBitmap, ref LinearImage b1, string checkStr, string str, string encodebinarystr, int resolutiondpi, int width, int height)
        {
            b1.encodestr = str;
            b1.symbolwidth = (((((int)this.Width.Value) * 0x60) / 0x3e8) * resolutiondpi) / 0x60;
            b1.symbolheight = (((int)this.Height.Value) * resolutiondpi) / 0x60;
            b1.encodebinarystr = encodebinarystr;
            if (((int)Math.Round((double)((this.narrowbarwidth * resolutiondpi) / 1000))) >= 1)
            {
                b1.narrowbarwidth = (int)Math.Round((double)((this.narrowbarwidth * resolutiondpi) / 1000));
            }
            else
            {
                b1.narrowbarwidth = 1;
            }
            b1.barheight = (int)Math.Round((double)((this.barheight * resolutiondpi) / 1000));
            b1.leftmargin = (int)Math.Round((double)((this.leftmargin * resolutiondpi) / 1000));
            b1.topmargin = (int)Math.Round((double)((this.topmargin * resolutiondpi) / 1000));
            b1.textmargin = (int)Math.Round((double)((this.textmargin * resolutiondpi) / 10000));
            b1.topcommentleftmargin = (int)Math.Round((double)((this.topcommentleftmargin * resolutiondpi) / 1000));
            b1.topcommenttopmargin = (int)Math.Round((double)((this.topcommenttopmargin * resolutiondpi) / 1000));
            b1.bottomcommentleftmargin = (int)Math.Round((double)((this.bottomcommentleftmargin * resolutiondpi) / 10000));
            b1.bottomcommentbottommargin = (int)Math.Round((double)((this.bottomcommentbottommargin * resolutiondpi) / 1000));
            b1.forecolor = this.ForeColor;
            b1.backcolor = this.BackColor;
            b1.symbology = (int)this.barType;
            b1.checkstr = checkStr;
            b1.addcheckdigittotext = this.addcheckdigittotext;
            b1.showtext = this.showtext;
            
            FontStyle style = FontStyle.Regular;
            style |= this.Font.Bold ? FontStyle.Bold : 0;
            style |= this.Font.Italic ? FontStyle.Italic : 0;
            style |= this.Font.Strikeout ? FontStyle.Strikeout : 0;
            style |= this.Font.Underline ? FontStyle.Underline : 0;
            b1.txtFont = new Font(this.Font.Name, (float)this.Font.Size.Unit.Value, style);


            b1.topcomment = this.topcomment;
            b1.bottomcomment = this.bottomcomment;
            b1.resolutiondpi = resolutiondpi;
            b1.texttostretch = this.texttostretch;
            b1.bearerbars = this.bearerbars;
            b1.textalignment = (int)this.textalignment;
            switch (this.rotation)
            {
                case Rotation.Clockwise_Zero_Degree:
                    break;

                case Rotation.Clockwise_90_Degree:
                    outputBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    return;

                case Rotation.Clockwise_180_Degree:
                    outputBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    return;

                case Rotation.Clockwise_270_Degree:
                    outputBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;

                default:
                    return;
            }
        }

        private void InitPDF417Img(ref Bitmap outputBitmap, ref PDF417Image pdf417img, string encodebinarystr, int resolutiondpi, int width, int height)
        {
            pdf417img.encodebinarystr = encodebinarystr;
            pdf417img.backcolor = this.BackColor;
            pdf417img.forecolor = this.ForeColor;


            FontStyle style = FontStyle.Regular;
            style |= this.Font.Bold ? FontStyle.Bold : 0;
            style |= this.Font.Italic ? FontStyle.Italic : 0;
            style |= this.Font.Strikeout ? FontStyle.Strikeout : 0;
            style |= this.Font.Underline ? FontStyle.Underline : 0;
            pdf417img.txtFont = new Font(this.Font.Name, (float)this.Font.Size.Unit.Value, style);


            pdf417img.bottomcomment = this.bottomcomment;
            pdf417img.topcomment = this.topcomment;
            pdf417img.moduleheight = (int)Math.Round((double)(((this.moduleheight * resolutiondpi) * 1) / 1000));
            pdf417img.modulewidth = (int)Math.Round((double)(((this.modulewidth * resolutiondpi) * 1) / 1000));
            pdf417img.symbolheight = (((int)this.Height.Value) * resolutiondpi) / 0x60;
            pdf417img.symbolwidth = (((int)this.Width.Value) * resolutiondpi) / 0x60;
            pdf417img.bottomcommentbottommargin = (int)Math.Round((double)((this.bottomcommentbottommargin * resolutiondpi) / 1000));
            pdf417img.bottomcommentleftmargin = (int)Math.Round((double)((this.bottomcommentleftmargin * resolutiondpi) / 10000));
            pdf417img.leftmargin = (int)Math.Round((double)((this.leftmargin * resolutiondpi) / 1000));
            pdf417img.topmargin = (int)Math.Round((double)((this.topmargin * resolutiondpi) / 1000));
            pdf417img.topcommentleftmargin = (int)Math.Round((double)((this.topcommentleftmargin * resolutiondpi) / 1000));
            pdf417img.topcommenttopmargin = (int)Math.Round((double)((this.topcommenttopmargin * resolutiondpi) / 1000));
            switch (this.rotation)
            {
                case Rotation.Clockwise_Zero_Degree:
                    break;

                case Rotation.Clockwise_90_Degree:
                    outputBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    return;

                case Rotation.Clockwise_180_Degree:
                    outputBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    return;

                case Rotation.Clockwise_270_Degree:
                    outputBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;

                default:
                    return;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string text = "";
            if ((int)this.barType > 0x13)
            {
                text = this.text;
            }
            else
            {
                text = this.text;
            }
            int num = (int)Math.Round((double)((this.datamatrixmodulesize * 96) / 1000));
            if (num <= 0)
            {
                num = 2;
            }
            int num2 = (int)Math.Round((double)((this.modulewidth * 96) / 1000));
            int num3 = (int)Math.Round((double)((this.moduleheight * 96) / 1000));
            if (num2 <= 0)
            {
                num2 = 1;
            }
            if (num3 <= 0)
            {
                num3 = 1;
            }
            int num4 = (int)Math.Round(this.narrowbarwidth * 96 / 1000d);
            int num5 = (int)Math.Round(this.barheight * 96 / 1000d);
            if (num4 <= 0)
            {
                num4 = 1;
            }
            if (num5 <= 0)
            {
                num5 = 10;
            }
            int num6 = (int)Math.Round((double)((this.leftmargin * 96) / 1000));
            int num7 = (int)Math.Round((double)((this.topmargin * 96) / 1000));
            int num8 = (int)Math.Round((double)((this.topcommentleftmargin * 96) / 1000));
            int num9 = (int)Math.Round((double)((this.topcommenttopmargin * 96) / 1000));
            int num10 = (int)Math.Round((double)((this.bottomcommentleftmargin * 96) / 1000));
            int num11 = (int)Math.Round((double)((this.bottomcommentbottommargin * 96) / 1000));
            int num12 = (int)Math.Round((double)((this.textmargin * 96) / 1000));
            string text2 = "";
            for (int i = 0; i < text.Length; i++)
            {
                if ((text[i] >= '\0') && (text[i] < '0'))
                {
                    text2 = text2 + "~0" + ((int)text[i]).ToString();
                }
                else if ((text[i] > '9') && (text[i] < 'A'))
                {
                    text2 = text2 + "~0" + ((int)text[i]).ToString();
                }
                else if ((text[i] > 'Z') && (text[i] < 'a'))
                {
                    text2 = text2 + "~0" + ((int)text[i]).ToString();
                }
                else if (((text[i] > 'z') && (text[i] < '\x00ff')) && (text[i] != '~'))
                {
                    text2 = text2 + "~" + ((int)text[i]).ToString();
                }
                else
                {
                    text2 = text2 + text[i];
                }
            }
            text = text2;
            string text3 = "<img src=\"ImageService.axd?";
            text3 = ((((((((((((((((((((((((((((((((((((((text3 + "symbology=" + ((int)this.barType).ToString()) + "&text=" + text) + "&forecolor=" + this.ForeColor.Name) + "&backcolor=" + this.BackColor.Name) + "&fontname=" + this.Font.Name.ToString()) + "&fontsize=" + this.Font.Size.Unit.Value.ToString()) + "&height=" + this.Height.Value.ToString()) + "&width=" + this.Width.Value.ToString()) + "&leftmargin=" + num6.ToString()) + "&topmargin=" + num7.ToString()) + "&rotation=" + ((int)this.rotation).ToString()) + "&topcomment=" + this.topcomment.ToString()) + "&bottomcomment=" + this.bottomcomment.ToString()) + "&bottomcommentleftmargin=" + num10.ToString()) + "&bottomcommentbottommargin=" + num11.ToString()) + "&topcommenttopmargin=" + num9.ToString()) + "&topcommentleftmargin=" + num8.ToString()) + "&imageformat=" + this.imageformat.ToString()) + "&addcheckdigit=" + this.addcheckdigit.ToString()) + "&ratio=" + this.ratio.ToString()) + "&addcheckdigittotext=" + this.addcheckdigittotext.ToString()) + "&narrowbarwidth=" + num4.ToString()) + "&barheight=" + num5.ToString()) + "&textmargin=" + num12.ToString()) + "&showtext=" + this.showtext.ToString()) + "&bearerbars=" + this.bearerbars.ToString()) + "&texttostretch=" + this.texttostretch.ToString()) + "&textalignment=" + ((int)this.textalignment).ToString()) + "&modulewidth=" + num2.ToString()) + "&moduleheight=" + num3.ToString()) + "&aspectratio=" + this.aspectratio.ToString()) + "&maxcols=" + this.maxcols.ToString()) + "&maxrows=" + this.maxrows.ToString()) + "&securitylevel=" + this.securitylevel.ToString()) + "&truncated=" + this.truncated.ToString()) + "&encodationmode=" + ((int)this.encodationmode).ToString()) + "&datamatrixsize=" + ((int)this.datamatrixsize).ToString()) + "&datamatrixmodulesize=" + num.ToString()) + "\">";
            writer.Write(text3);
        }

        [Bindable(true), Category("Barcode"), DefaultValue(true), Description("True if including the check digit ")]
        public bool AddCheckDigit
        {
            get
            {
                return this.addcheckdigit;
            }
            set
            {
                this.addcheckdigit = value;
            }
        }

        [Description("True if showing check digit in the human readable text"), Bindable(true), Category("Barcode"), DefaultValue(true)]
        public bool AddCheckDigitToText
        {
            get
            {
                return this.addcheckdigittotext;
            }
            set
            {
                this.addcheckdigittotext = value;
            }
        }



        [DefaultValue(0x3e8), Bindable(true), Category("Barcode"), Description("The height of the bar")]
        public double BarHeight
        {
            get
            {
                return this.barheight;
            }
            set
            {
                this.barheight = value;
            }
        }

        [Description("Bearer Bars "), Category("Barcode"), DefaultValue(false), Bindable(true)]
        public bool BearerBars
        {
            get
            {
                return this.bearerbars;
            }
            set
            {
                this.bearerbars = value;
            }
        }

        [Category("Barcode"), Bindable(true), Description("The bottom comment of the barcode")]
        public string BottomComment
        {
            get
            {
                return this.bottomcomment;
            }

            set
            {
                this.bottomcomment = value;
            }
        }

        [Bindable(true), Description("The bottom margin of the bottom comment"), Category("Barcode")]
        public double BottomCommentBottomMargin
        {
            get
            {
                return this.bottomcommentbottommargin;
            }
            set
            {
                this.bottomcommentbottommargin = value;
            }
        }

        [Description("The left margin of the bottom comment"), Bindable(true), Category("Barcode")]
        public double BottomCommentLeftMargin
        {
            get
            {
                return this.bottomcommentleftmargin;
            }
            set
            {
                this.bottomcommentleftmargin = value;
            }
        }

        [DefaultValue(true), Description("encode binary with datamatrix "), Category("Barcode"), Bindable(true)]
        public bool DataMatrixBinaryEncode
        {
            get
            {
                return this.binaryEncode;
            }
            set
            {
                this.binaryEncode = value;
            }
        }

        [Bindable(true), Description("The size of the datamatrix module"), Category("Barcode"), DefaultValue(0x17)]
        public int DataMatrixModuleSize
        {
            get
            {
                return this.datamatrixmodulesize;
            }
            set
            {
                this.datamatrixmodulesize = value;
            }
        }

        [Description("The prefer encode format of the datamatrix"), Bindable(true), Category("Barcode"), DefaultValue(0)]
        public DataMatrixSize DataMatrixSize
        {
            get
            {
                return this.datamatrixsize;
            }
            set
            {
                this.datamatrixsize = value;
            }
        }

        [Bindable(true), DefaultValue(0), Description("The encodation mode of the datamatrix"), Category("Barcode")]
        public EncodationMode EncodationMode
        {
            get
            {
                return this.encodationmode;
            }
            set
            {
                this.encodationmode = value;
            }
        }





        [Category("Barcode"), Bindable(true), DefaultValue(2), Description("The output image format")]
        public ImageType ImageType
        {
            get
            {
                return this.imageformat;
            }
            set
            {
                this.imageformat = value;
            }
        }

        [Description("The left margin of the barcode"), Bindable(true), DefaultValue(200), Category("Barcode")]
        public double LeftMargin
        {
            get
            {
                return this.leftmargin;
            }
            set
            {
                this.leftmargin = value;
            }
        }

        [Bindable(true), Category("Barcode"), DefaultValue(10), Description("The width of the narrow bar")]
        public double NarrowBarWidth
        {
            get
            {
                return this.narrowbarwidth;
            }
            set
            {
                this.narrowbarwidth = value;
            }
        }

        [Description("The ratio of the overall height to width"), Bindable(true), Category("Barcode"), DefaultValue(1)]
        public double PDFAspectRatio
        {
            get
            {
                return this.aspectratio;
            }
            set
            {
                this.aspectratio = value;
            }
        }

        [Bindable(true), Category("Barcode"), DefaultValue(0), Description("the maximum number of codeword columns in a PDF417 symbol")]
        public int PDFMaxCols
        {
            get
            {
                return this.maxcols;
            }
            set
            {
                this.maxcols = value;
            }
        }

        [DefaultValue(0), Bindable(true), Category("Barcode"), Description("the maximum number of codeword rows in a PDF417 symbol")]
        public int PDFMaxRows
        {
            get
            {
                return this.maxrows;
            }
            set
            {
                this.maxrows = value;
            }
        }

        [DefaultValue(30), Bindable(true), Description("The height of the module in a PDF417 symbol"), Category("Barcode")]
        public double PDFModuleHeight
        {
            get
            {
                return this.moduleheight;
            }
            set
            {
                this.moduleheight = value;
            }
        }

        [Category("Barcode"), Bindable(true), DefaultValue(10), Description("The width of the module in a PDF417 symbol")]
        public double PDFModuleWidth
        {
            get
            {
                return this.modulewidth;
            }
            set
            {
                this.modulewidth = value;
            }
        }

        [Bindable(true), Category("Barcode"), DefaultValue(9), Description("The security level of PDF417 symbol")]
        public int PDFSecurityLevel
        {
            get
            {
                return this.securitylevel;
            }
            set
            {
                this.securitylevel = value;
            }
        }

        [Description("True if setting for truncated"), Category("Barcode"), DefaultValue(false), Bindable(true)]
        public bool PDFTruncatedSymbol
        {
            get
            {
                return this.truncated;
            }
            set
            {
                this.truncated = value;
            }
        }

        [Description("The bitmap image of the control"), Category("Barcode"), Bindable(true)]
        public Bitmap Picture
        {
            get
            {
                this.CreatePicture();
                return this.picture;
            }
        }

        [Category("Barcode"), Description("The ratio of width bar to narrow bar"), Bindable(true), DefaultValue(3)]
        public int Ratio
        {
            get
            {
                return this.ratio;
            }
            set
            {
                this.ratio = value;
            }
        }

        [Description("The resolution of the picture and printer"), Category("Barcode"), DefaultValue(300), Bindable(true)]
        public int ResolutionDPI
        {
            get
            {
                return this.resolutiondpi;
            }
            set
            {
                this.resolutiondpi = value;
            }
        }

        [Description("Rotate barcode image in clockwise"), Category("Barcode"), Bindable(true), DefaultValue(0)]
        public Rotation Rotation
        {
            get
            {
                return this.rotation;
            }
            set
            {
                this.rotation = value;
            }
        }

        [DefaultValue(true), Bindable(true), Category("Barcode"), Description("True if showing the human readable text")]
        public bool ShowText
        {
            get
            {
                return this.showtext;
            }
            set
            {
                this.showtext = value;
            }
        }

        [DefaultValue(0), Bindable(true), Description("Select Barcode type"), Category("Barcode")]
        public BarType BarType
        {
            get
            {
                return this.barType;
            }
            set
            {
                this.barType = value;
            }
        }

        [Bindable(true), DefaultValue(0), Description("Human Readable Text Alignment "), Category("Barcode")]
        public TextAlignment TextAlignment
        {
            get
            {
                return this.textalignment;
            }
            set
            {
                this.textalignment = value;
            }
        }

        [Bindable(true), Category("Barcode"), DefaultValue(0), Description("The top margin of the human readable text")]
        public double TextMargin
        {
            get
            {
                return this.textmargin;
            }
            set
            {
                this.textmargin = value;
            }
        }

        [Bindable(true), Description("The message to be encoded"), Category("Barcode"), DefaultValue("1234567890")]
        public string TextToEncode
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }

        [Description(" Stretch human readable text"), Bindable(true), Category("Barcode"), DefaultValue(true)]
        public bool TextToStretch
        {
            get
            {
                return this.texttostretch;
            }
            set
            {
                this.texttostretch = value;
            }
        }

        [DefaultValue("ASP.NET BARCODE CONTROL"), Description("The top comment of the control"), Category("Barcode"), Bindable(true)]
        public string TopComment
        {
            get
            {
                return this.topcomment;
            }
            set
            {
                this.topcomment = value;
            }
        }

        [Description("The left margin of the top comment"), Category("Barcode"), Bindable(true), DefaultValue(0)]
        public double TopCommentLeftMargin
        {
            get
            {
                return this.topcommentleftmargin;
            }
            set
            {
                this.topcommentleftmargin = value;
            }
        }

        [Bindable(true), DefaultValue(0), Description("The top margin of the top comment"), Category("Barcode")]
        public double TopCommentTopMargin
        {
            get
            {
                return this.topcommenttopmargin;
            }
            set
            {
                this.topcommenttopmargin = value;
            }
        }

        [Bindable(true), Category("Barcode"), DefaultValue(200), Description("The top margin of the barcode")]
        public double TopMargin
        {
            get
            {
                return this.topmargin;
            }
            set
            {
                this.topmargin = value;
            }
        }


        public string GetDesignTimeHtml()
        {
            string imageUrl = Page.ClientScript.GetWebResourceUrl(this.GetType(), "InfoControl.Web.UI.WebControls.Boleto.Resources.barra.gif");
            return String.Format("<img src='{0}' />", imageUrl);
        }
    }
}

