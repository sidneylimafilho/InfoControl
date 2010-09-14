namespace InfoControl.Web.UI.WebControls.BarCode.Encoder
{
    using System;
    using System.Drawing;

    public class LinearImage
    {
        public bool addcheckdigittotext;
        public Color backcolor;
        public int barheight;
        public bool bearerbars;
        public string bottomcomment;
        public int bottomcommentbottommargin;
        public int bottomcommentleftmargin;
        public string checkstr;
        public string encodebinarystr;
        public string encodestr;
        public Color forecolor;
        public int leftmargin;
        public int narrowbarwidth;
        public int ratio;
        public int resolutiondpi;
        public int rotation;
        public bool showtext;
        public int symbolheight;
        public int symbology;
        public int symbolwidth;
        public int textalignment;
        public int textmargin;
        public bool texttostretch;
        public string topcomment;
        public int topcommentleftmargin;
        public int topcommenttopmargin;
        public int topmargin;
        public Font txtFont;

        private void DrawBarcodeText(ref Graphics g, string messagetext, int textlength, SolidBrush ForeBrush, int sfheight, bool texttostretch, int textalignment)
        {
            string s = "";
            messagetext = messagetext.Trim();
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            if (texttostretch)
            {
                for (int i = 0; i < messagetext.Length; i++)
                {
                    s = "" + messagetext[i];
                    Rectangle layoutRectangle = new Rectangle(this.leftmargin + ((i * textlength) / messagetext.Length), (this.topmargin + this.barheight) + this.textmargin, textlength / messagetext.Length, 2 * sfheight);
                    g.DrawString(s, this.txtFont, ForeBrush, layoutRectangle, format);
                }
            }
            else
            {
                Rectangle rectangle2 = new Rectangle(this.leftmargin, (this.topmargin + this.barheight) + this.textmargin, textlength, 2 * sfheight);
                switch (textalignment)
                {
                    case 0:
                        format.Alignment = StringAlignment.Near;
                        g.DrawString(messagetext, this.txtFont, ForeBrush, rectangle2, format);
                        return;

                    case 2:
                        format.Alignment = StringAlignment.Far;
                        g.DrawString(messagetext, this.txtFont, ForeBrush, rectangle2, format);
                        return;
                }
                format.Alignment = StringAlignment.Center;
                g.DrawString(messagetext, this.txtFont, ForeBrush, rectangle2, format);
            }
        }

        public void DrawLinear(ref Graphics g)
        {
            int index;
            string s = "";
            string text = "";
            g.Clear(this.backcolor);
            SolidBrush brush = new SolidBrush(this.forecolor);
            int x = this.leftmargin;
            int y = this.topmargin;
            int num4 = 0;
            if ((this.symbology == 8) || (this.symbology == 9))
            {
                this.addcheckdigittotext = false;
            }
            if (((this.encodestr.IndexOf(",") > 0) && (this.symbology > 2)) && (this.symbology < 8))
            {
                s = this.encodestr.Substring(this.encodestr.IndexOf(",") + 1, (this.encodestr.Length - this.encodestr.IndexOf(",")) - 1);
                this.encodestr = this.encodestr.Substring(0, this.encodestr.IndexOf(","));
            }
            if ((this.symbology > 2) && (this.symbology < 8))
            {
                this.addcheckdigittotext = true;
                this.showtext = true;
            }
            if (s.Length > 0)
            {
                this.checkstr = this.checkstr + "," + s;
            }
            if (this.showtext)
            {
                if (this.addcheckdigittotext)
                {
                    text = this.encodestr + this.checkstr;
                }
                else
                {
                    text = this.encodestr;
                }
            }
            else
            {
                text = "     ";
            }
            SizeF ef = g.MeasureString(text, this.txtFont, 0x7fffffff, StringFormat.GenericTypographic);
            int leftmargin = (((this.encodebinarystr.Length * this.narrowbarwidth) - ((int) ef.Width)) / 2) + x;
            if (leftmargin <= this.leftmargin)
            {
                leftmargin = this.leftmargin;
            }
            if (this.encodebinarystr.IndexOf(",") > 0)
            {
                index = this.encodebinarystr.IndexOf(",");
            }
            else
            {
                index = this.encodebinarystr.Length;
            }
            text = text + "            ";
            if (text.Trim().Length > 0)
            {
                string text3;
                string text4;
                string text5;
                string text6;
                switch (this.symbology)
                {
                    case 3:
                        text3 = text.Substring(0, 1);
                        text4 = text.Substring(1, 5);
                        text5 = text.Substring(6, 5);
                        text6 = text.Substring(11, 1);
                        g.DrawString(text3, this.txtFont, brush, (float) this.leftmargin, (y + this.barheight) - (ef.Height / 2f));
                        x += 10 * this.narrowbarwidth;
                        g.DrawString(text4, this.txtFont, brush, (float) (x + (6 * this.narrowbarwidth)), (y + this.barheight) - (ef.Height / 2f));
                        g.DrawString(text5, this.txtFont, brush, (float) (x + (((index / 2) + 5) * this.narrowbarwidth)), (y + this.barheight) - (ef.Height / 2f));
                        g.DrawString(text6, this.txtFont, brush, (float) (x + (index * this.narrowbarwidth)), (y + this.barheight) - (ef.Height / 2f));
                        goto Label_0567;

                    case 4:
                    case 7:
                        text3 = text.Substring(0, 1);
                        text4 = text.Substring(1, 6);
                        text5 = text.Substring(7, 6);
                        g.DrawString(text3, this.txtFont, brush, (float) this.leftmargin, (y + this.barheight) - (ef.Height / 2f));
                        x += 11 * this.narrowbarwidth;
                        g.DrawString(text4, this.txtFont, brush, (float) (x + (3 * this.narrowbarwidth)), (y + this.barheight) - (ef.Height / 2f));
                        g.DrawString(text5, this.txtFont, brush, (float) (x + (((index / 2) + 2) * this.narrowbarwidth)), (y + this.barheight) - (ef.Height / 2f));
                        goto Label_0567;

                    case 5:
                        text4 = text.Substring(0, 4);
                        text5 = text.Substring(4, 4);
                        g.DrawString(text4, this.txtFont, brush, (float) (x + (3 * this.narrowbarwidth)), (y + this.barheight) - (ef.Height / 2f));
                        g.DrawString(text5, this.txtFont, brush, (float) (x + (((index / 2) + 2) * this.narrowbarwidth)), (y + this.barheight) - (ef.Height / 2f));
                        goto Label_0567;

                    case 6:
                        text3 = "0";
                        text4 = text.Substring(0, 6);
                        text6 = text.Substring(6, 1);
                        g.DrawString(text3, this.txtFont, brush, (float) this.leftmargin, (y + this.barheight) - (ef.Height / 2f));
                        x += 11 * this.narrowbarwidth;
                        g.DrawString(text4, this.txtFont, brush, (float) (x + (3 * this.narrowbarwidth)), (y + this.barheight) - (ef.Height / 2f));
                        g.DrawString(text6, this.txtFont, brush, (float) (x + (index * this.narrowbarwidth)), (y + this.barheight) - (ef.Height / 2f));
                        goto Label_0567;
                }
                if (this.symbology != 15)
                {
                    this.DrawBarcodeText(ref g, text, this.encodebinarystr.Length * this.narrowbarwidth, brush, (int) ef.Height, this.texttostretch, this.textalignment);
                }
            }
        Label_0567:
            if (this.symbology == 15)
            {
                this.DrawPostnetImage(ref g, brush);
            }
            else
            {
                for (int i = 0; i < this.encodebinarystr.Length; i++)
                {
                    switch (this.encodebinarystr[i])
                    {
                        case '0':
                            x += this.narrowbarwidth;
                            goto Label_073D;

                        case '1':
                            switch (this.symbology)
                            {
                                case 4:
                                case 7:
                                    goto Label_064E;

                                case 5:
                                    goto Label_0681;

                                case 6:
                                    goto Label_06B4;
                            }
                            goto Label_06D3;

                        case 'Z':
                            y += this.barheight;
                            goto Label_073D;

                        case ',':
                            x += 14 * this.narrowbarwidth;
                            goto Label_073D;

                        default:
                            goto Label_073D;
                    }
                    if (((i < 3) || ((i >= ((index / 2) - 1)) && (i < (((index - 1) / 2) + 3)))) || (i > (index - 4)))
                    {
                        num4 = 0;
                    }
                    else
                    {
                        num4 = ((int) ef.Height) / 2;
                    }
                    goto Label_06D6;
                Label_064E:
                    if (((i < 3) || ((i >= ((index / 2) - 1)) && (i < (((index - 1) / 2) + 3)))) || (i > (index - 4)))
                    {
                        num4 = 0;
                    }
                    else
                    {
                        num4 = ((int) ef.Height) / 2;
                    }
                    goto Label_06D6;
                Label_0681:
                    if (((i < 3) || ((i >= ((index / 2) - 1)) && (i < (((index - 1) / 2) + 3)))) || (i > (index - 4)))
                    {
                        num4 = 0;
                    }
                    else
                    {
                        num4 = ((int) ef.Height) / 2;
                    }
                    goto Label_06D6;
                Label_06B4:
                    if ((i < 3) || (i > (index - 7)))
                    {
                        num4 = 0;
                    }
                    else
                    {
                        num4 = ((int) ef.Height) / 2;
                    }
                    goto Label_06D6;
                Label_06D3:
                    num4 = 0;
                Label_06D6:
                    if (i <= index)
                    {
                        g.FillRectangle(brush, x, y, this.narrowbarwidth, this.barheight - num4);
                    }
                    else
                    {
                        g.FillRectangle(brush, (float) x, y + ef.Height, (float) this.narrowbarwidth, (this.barheight - ef.Height) - (ef.Height / 2f));
                    }
                    x += this.narrowbarwidth;
                Label_073D:;
                }
            }
            if (s.Length > 0)
            {
                g.DrawString(s, this.txtFont, brush, (float) (this.leftmargin + ((index + 0x18) * this.narrowbarwidth)), (float) this.topmargin);
            }
            if (this.bearerbars && ((((this.symbology == 0) || (this.symbology == 1)) || ((this.symbology == 2) || (this.symbology == 11))) || (((this.symbology == 13) || (this.symbology == 8)) || (this.symbology == 9))))
            {
                g.FillRectangle(brush, this.leftmargin, this.topmargin, this.encodebinarystr.Length * this.narrowbarwidth, 3 * this.narrowbarwidth);
                g.FillRectangle(brush, this.leftmargin, this.topmargin + this.barheight, this.encodebinarystr.Length * this.narrowbarwidth, 3 * this.narrowbarwidth);
            }
            g.DrawString(this.topcomment, this.txtFont, brush, (float) this.topcommentleftmargin, (float) this.topcommenttopmargin);

            brush.Dispose();
        }

        private void DrawPostnetImage(ref Graphics g, SolidBrush ForeBrush)
        {
            int width = (0x16 * this.resolutiondpi) / 0x3e8;
            int height = (0x7d * this.resolutiondpi) / 0x3e8;
            int num3 = (50 * this.resolutiondpi) / 0x3e8;
            int num4 = (20 * this.resolutiondpi) / 0x3e8;
            int x = this.leftmargin;
            int y = this.topmargin;
            for (int i = 0; i < this.encodebinarystr.Length; i++)
            {
                switch (this.encodebinarystr[i])
                {
                    case '0':
                        g.FillRectangle(ForeBrush, x, y + (height - num3), width, num3);
                        x = (x + width) + num4;
                        break;

                    case '1':
                        g.FillRectangle(ForeBrush, x, y, width, height);
                        x = (x + width) + num4;
                        break;
                }
            }
        }
    }
}

