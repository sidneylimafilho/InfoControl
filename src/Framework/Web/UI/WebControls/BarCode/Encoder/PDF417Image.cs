namespace InfoControl.Web.UI.WebControls.BarCode.Encoder
{
    using System;
    using System.Drawing;

    public class PDF417Image
    {
        public Color backcolor;
        public string bottomcomment;
        public int bottomcommentbottommargin;
        public int bottomcommentleftmargin;
        public string encodebinarystr;
        public Color forecolor;
        public int leftmargin;
        public int moduleheight;
        public int modulewidth;
        public int symbolheight;
        public int symbolwidth;
        public int textmargin;
        public string topcomment;
        public int topcommentleftmargin;
        public int topcommenttopmargin;
        public int topmargin;
        public Font txtFont;

        public void DrawPDF417(ref Graphics g)
        {
            g.Clear(this.backcolor);
            SolidBrush brush = new SolidBrush(this.forecolor);
            int x = this.leftmargin;
            int y = this.topmargin;
            SizeF ef = g.MeasureString(this.bottomcomment, this.txtFont, 0x7fffffff, StringFormat.GenericTypographic);
            for (int i = 0; i < this.encodebinarystr.Length; i++)
            {
                char ch = this.encodebinarystr[i];
                switch (ch)
                {
                    case '0':
                        x += this.modulewidth;
                        break;

                    case '1':
                        g.FillRectangle(brush, x, y, this.modulewidth, this.moduleheight);
                        x += this.modulewidth;
                        break;

                    default:
                        if (ch == 'Z')
                        {
                            y += this.moduleheight;
                            x = this.leftmargin;
                        }
                        break;
                }
            }
            g.DrawString(this.topcomment, this.txtFont, brush, (float) this.topcommentleftmargin, (float) this.topcommenttopmargin);
            //g.DrawString(" ", this.txtFont, brush, (float) 0f, (float) (this.symbolheight - ef.Height));
            brush.Dispose();
        }
    }
}

