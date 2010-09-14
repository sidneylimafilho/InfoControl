namespace InfoControl.Web.UI.WebControls.BarCode.Encoder
{
    using System;
    using System.Collections;

    public class DataMatrixEncoder
    {
        public bool binaryEncode;
        private int datacols;
        private int datarows;
        private int E_ASCII = 1;
        private int E_AUTO = 0;
        private int E_BASE256 = 6;
        private int E_C40 = 2;
        private int E_EDIFACT = 5;
        private int E_NONE = -1;
        private int E_TEXT = 3;
        private int E_X12 = 4;
        private static int[][] ECC200Size = new int[][] { 
            new int[] { 10, 10, 8, 8, 1, 8, 8, 3, 5, 3, 5, 1 }, new int[] { 12, 12, 10, 10, 1, 10, 10, 5, 7, 5, 7, 1 }, new int[] { 14, 14, 12, 12, 1, 12, 12, 8, 10, 8, 10, 1 }, new int[] { 0x10, 0x10, 14, 14, 1, 14, 14, 12, 12, 12, 12, 1 }, new int[] { 0x12, 0x12, 0x10, 0x10, 1, 0x10, 0x10, 0x12, 14, 0x12, 14, 1 }, new int[] { 20, 20, 0x12, 0x12, 1, 0x12, 0x12, 0x16, 0x12, 0x16, 0x12, 1 }, new int[] { 0x16, 0x16, 20, 20, 1, 20, 20, 30, 20, 30, 20, 1 }, new int[] { 0x18, 0x18, 0x16, 0x16, 1, 0x16, 0x16, 0x24, 0x18, 0x24, 0x18, 1 }, new int[] { 0x1a, 0x1a, 0x18, 0x18, 1, 0x18, 0x18, 0x2c, 0x1c, 0x2c, 0x1c, 1 }, new int[] { 0x20, 0x20, 14, 14, 4, 0x1c, 0x1c, 0x3e, 0x24, 0x3e, 0x24, 1 }, new int[] { 0x24, 0x24, 0x10, 0x10, 4, 0x20, 0x20, 0x56, 0x2a, 0x56, 0x2a, 1 }, new int[] { 40, 40, 0x12, 0x12, 4, 0x24, 0x24, 0x72, 0x30, 0x72, 0x30, 1 }, new int[] { 0x2c, 0x2c, 20, 20, 4, 40, 40, 0x90, 0x38, 0x90, 0x38, 1 }, new int[] { 0x30, 0x30, 0x16, 0x16, 4, 0x2c, 0x2c, 0xae, 0x44, 0xae, 0x44, 1 }, new int[] { 0x34, 0x34, 0x18, 0x18, 4, 0x30, 0x30, 0xcc, 0x54, 0x66, 0x2a, 2 }, new int[] { 0x40, 0x40, 14, 14, 0x10, 0x38, 0x38, 280, 0x70, 140, 0x38, 2 }, 
            new int[] { 0x48, 0x48, 0x10, 0x10, 0x10, 0x40, 0x40, 0x170, 0x90, 0x5c, 0x24, 4 }, new int[] { 80, 80, 0x12, 0x12, 0x10, 0x48, 0x48, 0x1c8, 0xc0, 0x72, 0x30, 4 }, new int[] { 0x58, 0x58, 20, 20, 0x10, 80, 80, 0x240, 0xe0, 0x90, 0x38, 4 }, new int[] { 0x60, 0x60, 0x16, 0x16, 0x10, 0x58, 0x58, 0x2b8, 0x110, 0xae, 0x44, 4 }, new int[] { 0x68, 0x68, 0x18, 0x18, 0x10, 0x60, 0x60, 0x330, 0x150, 0x88, 0x38, 6 }, new int[] { 120, 120, 0x12, 0x12, 0x24, 0x6c, 0x6c, 0x41a, 0x1f0, 0xaf, 0x44, 6 }, new int[] { 0x84, 0x84, 20, 20, 0x24, 120, 120, 0x518, 0x1f0, 0xa3, 0x3e, 8 }, new int[] { 0x90, 0x90, 0x16, 0x16, 0x24, 0x84, 0x84, 0x616, 620, 0x9c, 0x3e, 10 }, new int[] { 8, 0x12, 6, 0x10, 1, 6, 0x10, 5, 7, 5, 7, 1 }, new int[] { 8, 0x20, 6, 14, 2, 6, 0x1c, 10, 11, 10, 11, 1 }, new int[] { 12, 0x1a, 10, 0x18, 1, 10, 0x18, 0x10, 14, 0x10, 14, 1 }, new int[] { 12, 0x24, 10, 0x10, 2, 10, 0x20, 0x16, 0x12, 0x16, 0x12, 1 }, new int[] { 0x10, 0x24, 14, 0x10, 2, 14, 0x20, 0x20, 0x18, 0x20, 0x18, 1 }, new int[] { 0x10, 0x30, 14, 0x16, 2, 14, 0x2c, 0x31, 0x1c, 0x31, 0x1c, 1 }
         };
        private int m_nCol;
        private int m_nCurEncodation;
        public int m_nPreferredEncodation;
        private int m_nPreferredFormat;
        private int m_nRow;
        private int mapcols;
        private int maprows;
        private int reedblocks;
        private int reeddata;
        private int reederr;
        private int regions;
        private int S_104X104 = 20;
        private int S_10X10 = 0;
        private int S_120X120 = 0x15;
        private int S_12X12 = 1;
        private int S_12X26 = 0x1a;
        private int S_12X36 = 0x1b;
        private int S_132X132 = 0x16;
        private int S_144X144 = 0x17;
        private int S_14X14 = 2;
        private int S_16X16 = 3;
        private int S_16X36 = 0x1c;
        private int S_16X48 = 0x1d;
        private int S_18X18 = 4;
        private int S_20X20 = 5;
        private int S_22X22 = 6;
        private int S_24X24 = 7;
        private int S_26X26 = 8;
        private int S_32X32 = 9;
        private int S_36X36 = 10;
        private int S_40X40 = 11;
        private int S_44X44 = 12;
        private int S_48X48 = 13;
        private int S_52X52 = 14;
        private int S_64X64 = 15;
        private int S_72X72 = 0x10;
        private int S_80X80 = 0x11;
        private int S_88X88 = 0x12;
        private int S_8X18 = 0x18;
        private int S_8X32 = 0x19;
        private int S_96X96 = 0x13;
        private ArrayList szDataToEncode = new ArrayList();
        public int targetSizeID;
        private int totaldata;
        private int totalerr;

        public DataMatrixEncoder()
        {
            this.m_nCurEncodation = this.E_ASCII;
            this.m_nPreferredEncodation = this.E_ASCII;
            this.m_nPreferredFormat = -1;
            this.targetSizeID = 0;
            this.binaryEncode = true;
        }

        private int ASCII_Encode(ref int nCurPos, ref ArrayList pBuf, int nLen, bool bAuto)
        {
            int num5;
            int num = 0;
            int num2 = 0;
            int count = this.szDataToEncode.Count;
            if (!bAuto)
            {
                num5 = nLen;
            }
            else
            {
                num5 = nCurPos;
                num2 = nCurPos;
                while (num2 < count)
                {
                    if (this.LookAhead(ref this.szDataToEncode, num2) != this.E_ASCII)
                    {
                        break;
                    }
                    num2++;
                }
                num5 = num2;
            }
            for (num2 = nCurPos; num2 < num5; num2++)
            {
                if (char.IsNumber((char) this.szDataToEncode[num2]))
                {
                    if (((num2 + 1) < num5) && char.IsNumber((char) this.szDataToEncode[num2 + 1]))
                    {
                        int num3 = (10 * (((char) this.szDataToEncode[num2]) - '0')) + (((char) this.szDataToEncode[num2 + 1]) - '0');
                        pBuf.Add((char) (num3 + 130));
                        num++;
                        num2++;
                    }
                    else
                    {
                        pBuf.Add((char) (((char) this.szDataToEncode[num2]) + '\x0001'));
                        num++;
                    }
                }
                else if ((((char) this.szDataToEncode[num2]) == '\x00e8') && !this.binaryEncode)
                {
                    count = pBuf.Count;
                    if ((((count >= 1) && (((char) pBuf[count - 1]) == '\x00e9')) || ((count >= 2) && (((char) pBuf[count - 2]) == '\x00e9'))) || (((count >= 5) && (((char) pBuf[count - 5]) == '\x00e9')) || ((count >= 6) && (((char) pBuf[count - 6]) == '\x00e9'))))
                    {
                        pBuf.Add('\x00e8');
                    }
                    else
                    {
                        pBuf.Add('\x001d');
                    }
                }
                else if ((((((char) this.szDataToEncode[num2]) == '\x00ea') || (((char) this.szDataToEncode[num2]) == '\x00ec')) || (((char) this.szDataToEncode[num2]) == '\x00ed')) && !this.binaryEncode)
                {
                    pBuf.Add(this.szDataToEncode[num2]);
                    num++;
                }
                else if ((((char) this.szDataToEncode[num2]) == '\x00e9') && !this.binaryEncode)
                {
                    if ((num2 + 4) < num5)
                    {
                        pBuf.Add('\x00e9');
                        pBuf.Add(this.szDataToEncode[num2 + 1]);
                        pBuf.Add(this.szDataToEncode[num2 + 2]);
                        pBuf.Add(this.szDataToEncode[num2 + 3]);
                        num2 += 3;
                        num += 4;
                    }
                }
                else if ((((char) this.szDataToEncode[num2]) == '\x00f1') && !this.binaryEncode)
                {
                    if ((num2 + 7) < num5)
                    {
                        string text = "";
                        for (int i = 0; i < 6; i++)
                        {
                            text = text + ((char) this.szDataToEncode[(num2 + i) + 1]);
                        }
                        double num6 = Convert.ToDouble(text);
                        pBuf.Add('\x00f1');
                        if (num6 <= 126)
                        {
                            pBuf.Add((char) ((ushort) (num6 + 1)));
                            num += 2;
                        }
                        else if ((num6 >= 127) && (num6 <= 16382))
                        {
                            pBuf.Add((char) ((ushort) (((num6 - 127) / 254) + 128)));
                            pBuf.Add((char) ((ushort) (((num6 - 127) % 254) + 1)));
                            num += 3;
                        }
                        else if (num6 >= 16383)
                        {
                            pBuf.Add((char) ((ushort) (((num6 - 16383) / 64516) + 192)));
                            pBuf.Add(((ushort) (((num6 - 16383) / 254) % 254)) + 1);
                            pBuf.Add(((ushort) ((num6 - 16383) % 254)) + 1);
                            num += 4;
                        }
                        num2 += 6;
                    }
                }
                else if ((((char) this.szDataToEncode[num2]) >= '\0') && (((char) this.szDataToEncode[num2]) <= '\x007f'))
                {
                    pBuf.Add((char) (((char) this.szDataToEncode[num2]) + '\x0001'));
                    num++;
                }
                else if ((((char) this.szDataToEncode[num2]) >= '\x0080') && (((char) this.szDataToEncode[num2]) <= '\x00ff'))
                {
                    pBuf.Add('\x00eb');
                    pBuf.Add((char) ((((char) this.szDataToEncode[num2]) - '\x0080') + 1));
                    num += 2;
                }
            }
            nCurPos = num5;
            return num;
        }

        private int AUTO_Encode(ref ArrayList pBuf)
        {
            int num = 0;
            this.m_nCurEncodation = this.E_ASCII;
            int nCurEncodation = this.E_ASCII;
            int nCurPos = 0;
            int count = this.szDataToEncode.Count;
            while (nCurPos < count)
            {
                int num2;
                if (this.m_nCurEncodation == this.E_ASCII)
                {
                    if ((char.IsNumber((char) this.szDataToEncode[nCurPos]) && ((nCurPos + 1) < count)) && char.IsNumber((char) this.szDataToEncode[nCurPos + 1]))
                    {
                        if ((nCurEncodation != this.E_ASCII) && (num > 0))
                        {
                            char ch1 = (char) pBuf[num - 1];
                        }
                        num += this.ASCII_Encode(ref nCurPos, ref pBuf, nCurPos + 2, false);
                        continue;
                    }
                    if ((num2 = this.LookAhead(ref this.szDataToEncode, nCurPos)) != this.E_ASCII)
                    {
                        nCurEncodation = this.m_nCurEncodation;
                        this.m_nCurEncodation = num2;
                    }
                    else
                    {
                        if (((nCurEncodation != this.E_ASCII) && (nCurEncodation != this.E_BASE256)) && (num > 0))
                        {
                            char ch2 = (char) pBuf[num - 1];
                        }
                        num += this.ASCII_Encode(ref nCurPos, ref pBuf, nCurPos + 1, true);
                        continue;
                    }
                }
                if (this.m_nCurEncodation == this.E_C40)
                {
                    if ((num2 = this.LookAhead(ref this.szDataToEncode, nCurPos)) != this.E_C40)
                    {
                        nCurEncodation = this.m_nCurEncodation;
                        this.m_nCurEncodation = num2;
                    }
                    else
                    {
                        if ((nCurEncodation != this.E_C40) || ((num > 0) && (((char) pBuf[num - 1]) == '\x00fe')))
                        {
                            pBuf.Add('\x00e6');
                            nCurEncodation = this.E_C40;
                            num++;
                        }
                        num += this.C40_Encode(ref nCurPos, ref pBuf, nCurPos + 3, true);
                        continue;
                    }
                }
                if (this.m_nCurEncodation == this.E_TEXT)
                {
                    if ((num2 = this.LookAhead(ref this.szDataToEncode, nCurPos)) != this.E_TEXT)
                    {
                        nCurEncodation = this.m_nCurEncodation;
                        this.m_nCurEncodation = num2;
                    }
                    else
                    {
                        if ((nCurEncodation != this.E_TEXT) || ((num > 0) && (((char) pBuf[num - 1]) == '\x00fe')))
                        {
                            pBuf.Add('\x00ef');
                            nCurEncodation = this.E_TEXT;
                            num++;
                        }
                        num += this.Text_Encode(ref nCurPos, ref pBuf, nCurPos + 3, true);
                        continue;
                    }
                }
                if (this.m_nCurEncodation == this.E_X12)
                {
                    if ((num2 = this.LookAhead(ref this.szDataToEncode, nCurPos)) != this.E_X12)
                    {
                        nCurEncodation = this.m_nCurEncodation;
                        this.m_nCurEncodation = num2;
                    }
                    else
                    {
                        if ((nCurEncodation != this.E_X12) || ((num > 0) && (((char) pBuf[num - 1]) == '\x00fe')))
                        {
                            pBuf.Add('\x00ee');
                            nCurEncodation = this.E_X12;
                            num++;
                        }
                        num += this.X12_Encode(ref nCurPos, ref pBuf, nCurPos + 3, true);
                        continue;
                    }
                }
                if (this.m_nCurEncodation == this.E_EDIFACT)
                {
                    if ((num2 = this.LookAhead(ref this.szDataToEncode, nCurPos)) != this.E_EDIFACT)
                    {
                        nCurEncodation = this.m_nCurEncodation;
                        this.m_nCurEncodation = num2;
                    }
                    else
                    {
                        if ((nCurEncodation != this.E_EDIFACT) || ((num > 0) && (((char) pBuf[num - 1]) == '\x00fe')))
                        {
                            pBuf.Add('\x00f0');
                            nCurEncodation = this.E_EDIFACT;
                            num++;
                        }
                        num += this.EDIFACT_Encode(ref nCurPos, ref pBuf, nCurPos + 3, true);
                        continue;
                    }
                }
                if (this.m_nCurEncodation == this.E_BASE256)
                {
                    if ((num2 = this.LookAhead(ref this.szDataToEncode, nCurPos)) != this.E_BASE256)
                    {
                        nCurEncodation = this.m_nCurEncodation;
                        this.m_nCurEncodation = num2;
                    }
                    else
                    {
                        if ((nCurEncodation != this.E_BASE256) || ((num > 0) && (((char) pBuf[num - 1]) == '\x00fe')))
                        {
                            pBuf.Add('\x00e7');
                            nCurEncodation = this.E_BASE256;
                            num++;
                        }
                        num += this.Base256_Encode(ref nCurPos, ref pBuf, nCurPos + 1, true);
                    }
                }
            }
            return num;
        }

        private int B2D(string pszBin)
        {
            int num = 0;
            int length = pszBin.Length;
            for (int i = 0; i < length; i++)
            {
                if (pszBin[i] == '1')
                {
                    num = (num << 1) + 1;
                }
                else
                {
                    num = num << 1;
                }
            }
            return num;
        }

        private int Base256_Encode(ref int nCurPos, ref ArrayList pBuf, int nNextStart, bool bAuto)
        {
            int num2;
            int num3;
            int num = 0;
            ArrayList list = new ArrayList();
            int count = this.szDataToEncode.Count;
            int num5 = pBuf.Count;
            if (!bAuto)
            {
                num3 = nNextStart;
            }
            else
            {
                num3 = nCurPos;
                num2 = nCurPos;
                while (num2 < count)
                {
                    if (this.LookAhead(ref this.szDataToEncode, num2) != this.E_BASE256)
                    {
                        break;
                    }
                    num2++;
                }
                num3 = num2;
            }
            for (num2 = nCurPos; num2 < num3; num2++)
            {
                list.Add(this.szDataToEncode[num2]);
            }
            int nCodeword = list.Count;
            if (nCodeword == 0)
            {
                pBuf.Add('\0');
                num++;
                num5++;
            }
            else if (nCodeword < 250)
            {
                pBuf.Add(this.Base256Random(nCodeword, ++num5));
                num++;
            }
            else if ((nCodeword >= 250) && (nCodeword < 0x614))
            {
                pBuf.Add(this.Base256Random((nCodeword / 250) + 0xf9, ++num5));
                pBuf.Add(this.Base256Random(nCodeword % 250, ++num5));
                num += 2;
            }
            num2 = 0;
            while (num2 < nCodeword)
            {
                pBuf.Add(this.Base256Random((char) list[num2++], ++num5));
                num++;
            }
            nCurPos = num3;
            return num;
        }

        private char Base256Random(int nCodeword, int nPos)
        {
            int num = ((0x95 * nPos) % 0xff) + 1;
            int num2 = nCodeword + num;
            if (num2 <= 0xff)
            {
                return (char) num2;
            }
            return (char) (num2 - 0x100);
        }

        private void BasicEncode(string pszDataToEncode, ref ArrayList ResultList)
        {
            int index;
            ArrayList pBuf = new ArrayList();
            string inpara = pszDataToEncode;
            this.szDataToEncode = this.TildeCodes(inpara);
            int nLen = this.szDataToEncode.Count;
            int nCurPos = 0;
            int num4 = 0;
            if (this.m_nPreferredEncodation != this.E_AUTO)
            {
                this.m_nCurEncodation = this.m_nPreferredEncodation;
            }
            if (this.m_nPreferredEncodation == this.E_AUTO)
            {
                num4 += this.AUTO_Encode(ref pBuf);
            }
            else if (this.m_nPreferredEncodation == this.E_ASCII)
            {
                num4 += this.ASCII_Encode(ref nCurPos, ref pBuf, nLen, false);
            }
            else if (this.m_nPreferredEncodation == this.E_BASE256)
            {
                pBuf.Add('\x00e7');
                num4++;
                num4 += this.Base256_Encode(ref nCurPos, ref pBuf, nLen, false);
            }
            else if (this.m_nPreferredEncodation == this.E_C40)
            {
                pBuf.Add('\x00e6');
                num4++;
                num4 += this.C40_Encode(ref nCurPos, ref pBuf, nLen, false);
            }
            else if (this.m_nPreferredEncodation == this.E_TEXT)
            {
                pBuf.Add('\x00ef');
                num4++;
                num4 += this.Text_Encode(ref nCurPos, ref pBuf, nLen, false);
            }
            else if (this.m_nPreferredEncodation == this.E_X12)
            {
                pBuf.Add('\x00ee');
                num4++;
                num4 += this.X12_Encode(ref nCurPos, ref pBuf, nLen, false);
            }
            else if (this.m_nPreferredEncodation == this.E_EDIFACT)
            {
                pBuf.Add('\x00f0');
                num4++;
                num4 += this.EDIFACT_Encode(ref nCurPos, ref pBuf, nLen, false);
            }
            else if (this.m_nPreferredEncodation == this.E_NONE)
            {
                for (index = 0; index < nLen; index++)
                {
                    pBuf.Add(this.szDataToEncode[index]);
                }
                num4 = nLen;
            }
            int nPreferredFormat = 0;
            if (this.m_nPreferredFormat != -1)
            {
                nPreferredFormat = this.m_nPreferredFormat;
                if (num4 > ECC200Size[nPreferredFormat][7])
                {
                    nPreferredFormat = 0;
                }
            }
            nLen = pBuf.Count;
            while ((num4 >= ECC200Size[nPreferredFormat][7]) && (nPreferredFormat < 30))
            {
                if (((this.m_nCurEncodation == this.E_C40) || (this.m_nCurEncodation == this.E_TEXT)) || (this.m_nCurEncodation == this.E_X12))
                {
                    if ((((char) pBuf[num4 - 2]) == '\x00fe') && (ECC200Size[nPreferredFormat][7] == (num4 - 1)))
                    {
                        pBuf[num4 - 2] = pBuf[num4 - 1];
                        pBuf[num4 - 1] = 0;
                        num4--;
                        break;
                    }
                    if ((((char) pBuf[num4 - 1]) == '\x00fe') && (ECC200Size[nPreferredFormat][7] == (num4 - 1)))
                    {
                        pBuf[num4 - 1] = 0;
                        num4--;
                        break;
                    }
                }
                nPreferredFormat++;
            }
            if (ECC200Size[this.targetSizeID][7] > ECC200Size[nPreferredFormat][7])
            {
                nPreferredFormat = this.targetSizeID;
            }
            this.m_nRow = ECC200Size[nPreferredFormat][0];
            this.m_nCol = ECC200Size[nPreferredFormat][1];
            this.datarows = ECC200Size[nPreferredFormat][2];
            this.datacols = ECC200Size[nPreferredFormat][3];
            this.maprows = ECC200Size[nPreferredFormat][5];
            this.mapcols = ECC200Size[nPreferredFormat][6];
            this.regions = ECC200Size[nPreferredFormat][4];
            this.totaldata = ECC200Size[nPreferredFormat][7];
            this.totalerr = ECC200Size[nPreferredFormat][8];
            this.reeddata = ECC200Size[nPreferredFormat][9];
            this.reederr = ECC200Size[nPreferredFormat][10];
            this.reedblocks = ECC200Size[nPreferredFormat][11];
            if ((((this.m_nCurEncodation == this.E_C40) || (this.m_nCurEncodation == this.E_TEXT)) || (this.m_nCurEncodation == this.E_X12)) && ((num4 < this.totaldata) && (((char) pBuf[num4 - 1]) != '\x00fe')))
            {
                pBuf.Add('\x00fe');
                num4++;
            }
            bool flag = true;
            for (index = num4; index < this.totaldata; index++)
            {
                num4++;
                if (flag)
                {
                    pBuf.Add('\x0081');
                    flag = false;
                }
                else
                {
                    pBuf.Add((char) this.Random253State(0x81, index + 1));
                }
            }
            int[][] numArray = new int[10][];
            for (int i = 0; i < 10; i++)
            {
                numArray[i] = new int[0xff];
            }
            int num7 = 0;
            int num8 = 0;
            for (index = 1; index <= this.totaldata; index++)
            {
                numArray[num7][num8] = (char) pBuf[index - 1];
                if (++num7 == this.reedblocks)
                {
                    num7 = 0;
                    num8++;
                }
            }
            int[] numArray2 = new int[10];
            int num9 = 0;
            DataMatrixReed reed = new DataMatrixReed();
            for (index = 0; index < this.reedblocks; index++)
            {
                numArray2[index] = this.reeddata + this.reederr;
                int reeddata = this.reeddata;
                if ((this.m_nRow == 0x90) && (index > 7))
                {
                    numArray2[index] = (this.reeddata + this.reederr) - 1;
                    reeddata = 0x9b;
                }
                reed.ReedSolomon(numArray[index], reeddata, this.reederr);
                num9 += numArray2[index];
            }
            string text2 = "";
            for (index = 0; index < numArray2[0]; index++)
            {
                for (nCurPos = 0; nCurPos < this.reedblocks; nCurPos++)
                {
                    if (index < numArray2[nCurPos])
                    {
                        text2 = text2 + ((char) numArray[nCurPos][index]);
                    }
                }
            }
            ArrayList strMatrix = new ArrayList();
            ArrayList nArray = new ArrayList();
            for (int j = 0; j < (this.m_nRow * this.m_nCol); j++)
            {
                strMatrix.Add('0');
                nArray.Add('0');
            }
            this.Ecc200Placement(text2, ref nArray);
            this.ModulePlace(nArray, ref strMatrix);
            ResultList = strMatrix;
        }

        private int C40_Encode(ref int nCurPos, ref ArrayList pBuf, int nLen, bool bAuto)
        {
            int num3;
            int num5;
            ArrayList list = new ArrayList();
            int num = 0;
            int num2 = nCurPos;
            int num7 = 0;
            int count = this.szDataToEncode.Count;
            if (!bAuto)
            {
                num5 = nLen;
            }
            else
            {
                num5 = nCurPos;
                num2 = nCurPos;
                while (num2 < count)
                {
                    if (this.LookAhead(ref this.szDataToEncode, num2) != this.E_C40)
                    {
                        break;
                    }
                    num2++;
                }
                num5 = num2;
            }
            num2 = nCurPos;
            while ((num2 < num5) || ((num7 + 3) <= list.Count))
            {
                int num6;
                while ((num7 + 3) <= list.Count)
                {
                    num3 = ((('ـ' * ((char) list[num7])) + ('(' * ((char) list[num7 + 1]))) + ((char) list[num7 + 2])) + 1;
                    pBuf.Add((char) (num3 / 0x100));
                    pBuf.Add((char) (num3 % 0x100));
                    num += 2;
                    num7 += 3;
                }
                if ((num2 + 3) > num5)
                {
                    num6 = num5;
                }
                else
                {
                    num6 = num2 + 3;
                }
                while (num2 < num6)
                {
                    this.getC40Value((char) this.szDataToEncode[num2], ref list);
                    num2++;
                }
            }
        Label_011B:
            if ((num7 + 3) == list.Count)
            {
                num3 = ((('ـ' * ((char) list[num7])) + ('(' * ((char) list[num7 + 1]))) + ((char) list[num7 + 2])) + 1;
                pBuf.Add((char) (num3 / 0x100));
                pBuf.Add((char) (num3 % 0x100));
                num += 2;
            }
            else if ((num7 + 2) == list.Count)
            {
                num3 = (('ـ' * ((char) list[num7])) + ('(' * ((char) list[num7 + 1]))) + 1;
                pBuf.Add((char) (num3 / 0x100));
                pBuf.Add((char) (num3 % 0x100));
                num += 2;
            }
            else if ((num7 + 1) == list.Count)
            {
                if ((num2 < count) && (((char) this.szDataToEncode[num2]) < '\x0080'))
                {
                    this.getC40Value((char) this.szDataToEncode[num2++], ref list);
                    goto Label_011B;
                }
                num3 = ((('ـ' * ((char) list[num7])) + 40) + 30) + 1;
                pBuf.Add((char) (num3 / 0x100));
                pBuf.Add((char) (num3 % 0x100));
                num += 2;
            }
            if (num2 < count)
            {
                this.m_nCurEncodation = this.E_ASCII;
                pBuf.Add('\x00fe');
                num++;
            }
            nCurPos = num2;
            return num;
        }

        private bool ConvertBasicEncodeResultIntoStringArray(ArrayList strInput, ref string resultArray)
        {
            string text = "";
            for (int i = 0; i < this.m_nRow; i++)
            {
                for (int j = i * this.m_nCol; j < ((i * this.m_nCol) + this.m_nCol); j++)
                {
                    text = text + ((char) strInput[j]);
                }
                resultArray = resultArray + text;
                resultArray = resultArray + "Z";
                text = "";
            }
            return true;
        }

        private void corner1(byte chr, ref ArrayList nArray, ref ArrayList bPlaced)
        {
            this.module(this.maprows - 1, 0, chr, 1, ref nArray, ref bPlaced);
            this.module(this.maprows - 1, 1, chr, 2, ref nArray, ref bPlaced);
            this.module(this.maprows - 1, 2, chr, 3, ref nArray, ref bPlaced);
            this.module(0, this.mapcols - 2, chr, 4, ref nArray, ref bPlaced);
            this.module(0, this.mapcols - 1, chr, 5, ref nArray, ref bPlaced);
            this.module(1, this.mapcols - 1, chr, 6, ref nArray, ref bPlaced);
            this.module(2, this.mapcols - 1, chr, 7, ref nArray, ref bPlaced);
            this.module(3, this.mapcols - 1, chr, 8, ref nArray, ref bPlaced);
        }

        private void corner2(byte chr, ref ArrayList nArray, ref ArrayList bPlaced)
        {
            this.module(this.maprows - 3, 0, chr, 1, ref nArray, ref bPlaced);
            this.module(this.maprows - 2, 0, chr, 2, ref nArray, ref bPlaced);
            this.module(this.maprows - 1, 0, chr, 3, ref nArray, ref bPlaced);
            this.module(0, this.mapcols - 4, chr, 4, ref nArray, ref bPlaced);
            this.module(0, this.mapcols - 3, chr, 5, ref nArray, ref bPlaced);
            this.module(0, this.mapcols - 2, chr, 6, ref nArray, ref bPlaced);
            this.module(0, this.mapcols - 1, chr, 7, ref nArray, ref bPlaced);
            this.module(1, this.mapcols - 1, chr, 8, ref nArray, ref bPlaced);
        }

        private void corner3(byte chr, ref ArrayList nArray, ref ArrayList bPlaced)
        {
            this.module(this.maprows - 3, 0, chr, 1, ref nArray, ref bPlaced);
            this.module(this.maprows - 2, 0, chr, 2, ref nArray, ref bPlaced);
            this.module(this.maprows - 1, 0, chr, 3, ref nArray, ref bPlaced);
            this.module(0, this.mapcols - 2, chr, 4, ref nArray, ref bPlaced);
            this.module(0, this.mapcols - 1, chr, 5, ref nArray, ref bPlaced);
            this.module(1, this.mapcols - 1, chr, 6, ref nArray, ref bPlaced);
            this.module(2, this.mapcols - 1, chr, 7, ref nArray, ref bPlaced);
            this.module(3, this.mapcols - 1, chr, 8, ref nArray, ref bPlaced);
        }

        private void corner4(byte chr, ref ArrayList nArray, ref ArrayList bPlaced)
        {
            this.module(this.maprows - 1, 0, chr, 1, ref nArray, ref bPlaced);
            this.module(this.maprows - 1, this.mapcols - 1, chr, 2, ref nArray, ref bPlaced);
            this.module(0, this.mapcols - 3, chr, 3, ref nArray, ref bPlaced);
            this.module(0, this.mapcols - 2, chr, 4, ref nArray, ref bPlaced);
            this.module(0, this.mapcols - 1, chr, 5, ref nArray, ref bPlaced);
            this.module(1, this.mapcols - 3, chr, 6, ref nArray, ref bPlaced);
            this.module(1, this.mapcols - 2, chr, 7, ref nArray, ref bPlaced);
            this.module(1, this.mapcols - 1, chr, 8, ref nArray, ref bPlaced);
        }

        private int D2B(int nDec, ref string pBin)
        {
            string text = "";
            int num = nDec;
            while (num > 1)
            {
                if ((num & 1) == 1)
                {
                    text = '1' + text;
                }
                else
                {
                    text = '0' + text;
                }
                num = num >> 1;
            }
            if (num == 1)
            {
                text = '1' + text;
            }
            else
            {
                text = '0' + text;
            }
            pBin = text.PadLeft(8, '0');
            return 1;
        }

        private void Ecc200Placement(string strECC200, ref ArrayList nArray)
        {
            ArrayList bPlaced = new ArrayList();
            for (int i = 0; i < (this.maprows * this.mapcols); i++)
            {
                bPlaced.Add('0');
            }
            int num3 = 0;
            int row = 4;
            int col = 0;
            do
            {
                if ((row == this.maprows) && (col == 0))
                {
                    this.corner1((byte) strECC200[num3++], ref nArray, ref bPlaced);
                }
                if (((row == (this.maprows - 2)) && (col == 0)) && ((this.mapcols % 4) > 0))
                {
                    this.corner2((byte) strECC200[num3++], ref nArray, ref bPlaced);
                }
                if (((row == (this.maprows - 2)) && (col == 0)) && ((this.mapcols % 8) == 4))
                {
                    this.corner3((byte) strECC200[num3++], ref nArray, ref bPlaced);
                }
                if (((row == (this.maprows + 4)) && (col == 2)) && ((this.mapcols % 8) == 0))
                {
                    this.corner4((byte) strECC200[num3++], ref nArray, ref bPlaced);
                }
                do
                {
                    if (((row < this.maprows) && (col >= 0)) && ((((char) nArray[(row * this.mapcols) + col]) == '0') && (((char) bPlaced[(row * this.mapcols) + col]) == '0')))
                    {
                        this.utah(row, col, (byte) strECC200[num3++], ref nArray, ref bPlaced);
                    }
                    row -= 2;
                    col += 2;
                }
                while ((row >= 0) && (col < this.mapcols));
                row++;
                col += 3;
                do
                {
                    if (((row >= 0) && (col < this.mapcols)) && ((((char) nArray[(row * this.mapcols) + col]) == '0') && (((char) bPlaced[(row * this.mapcols) + col]) == '0')))
                    {
                        this.utah(row, col, (byte) strECC200[num3++], ref nArray, ref bPlaced);
                    }
                    row += 2;
                    col -= 2;
                }
                while ((row < this.maprows) && (col >= 0));
                row += 3;
                col++;
            }
            while ((row < this.maprows) || (col < this.mapcols));
            if (((char) bPlaced[(this.maprows * this.mapcols) - 1]) == '0')
            {
                int index = (this.maprows * this.mapcols) - 1;
                nArray.RemoveAt(index);
                nArray.Insert(index, '1');
                nArray.RemoveAt((index - this.mapcols) - 2);
                nArray.Insert((index - this.mapcols) - 2, '1');
            }
        }

        private int EDIFACT(string pszToEncode, ref ArrayList pBuf)
        {
            string text = "@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^ !\"#$%&'()*+,-./0123456789:;<=>?";
            int num = 0;
            string[] textArray = new string[4];
            for (int i = 0; i < 4; i++)
            {
                char ch = pszToEncode[i];
                if ((text.IndexOf(ch) > -1) || (pszToEncode[i] == '_'))
                {
                    this.D2B(pszToEncode[i], ref textArray[i]);
                }
                else
                {
                    this.D2B(0x20, ref textArray[i]);
                }
            }
            string pszBin = textArray[0].Substring(2, textArray[0].Length - 2) + textArray[1][2] + textArray[1][3];
            pBuf.Add((char) this.B2D(pszBin));
            pszBin = "";
            pszBin = (textArray[1].Substring(4, textArray[1].Length - 4) + textArray[2][2] + textArray[2][3]) + textArray[2][4] + textArray[2][5];
            pBuf.Add((char) this.B2D(pszBin));
            pszBin = "";
            pszBin = textArray[2].Substring(6, textArray[2].Length - 6);
            string text2 = textArray[3].Substring(2, textArray[3].Length - 2);
            pszBin = pszBin + text2;
            pBuf.Add((char) this.B2D(pszBin));
            return (num + 3);
        }

        private int EDIFACT_Encode(ref int nCurPos, ref ArrayList pBuf, int nLen, bool bAuto)
        {
            int num4;
            int num = 0;
            int num2 = nCurPos;
            int count = this.szDataToEncode.Count;
            if (!bAuto)
            {
                num4 = nLen;
            }
            else
            {
                num4 = nCurPos;
                num2 = nCurPos;
                while (num2 < count)
                {
                    if (this.LookAhead(ref this.szDataToEncode, num2) != this.E_EDIFACT)
                    {
                        break;
                    }
                    num2++;
                }
                num4 = num2;
            }
            ArrayList list = new ArrayList();
            for (int i = nCurPos; i < num4; i++)
            {
                list.Add(this.szDataToEncode[i]);
            }
            count = list.Count;
            if (count >= 4)
            {
                if ((count % 4) == 3)
                {
                    list.Add('_');
                }
                else
                {
                    list.Insert((count - (count % 4)) - 1, '_');
                }
            }
            count++;
            num2 = 0;
            while ((num2 + 4) <= count)
            {
                string pszToEncode = "";
                pszToEncode = (pszToEncode + ((char) list[num2]) + ((char) list[num2 + 1])) + ((char) list[num2 + 2]) + ((char) list[num2 + 3]);
                num += this.EDIFACT(pszToEncode, ref pBuf);
                num2 += 4;
            }
            this.m_nCurEncodation = this.E_ASCII;
            nCurPos = (nCurPos + num2) - 1;
            return num;
        }

        public string Encode(string pszDataToEncode)
        {
            ArrayList resultList = new ArrayList();
            this.BasicEncode(pszDataToEncode, ref resultList);
            string resultArray = "";
            this.ConvertBasicEncodeResultIntoStringArray(resultList, ref resultArray);
            return resultArray;
        }

        private bool FinderPattern(ArrayList strData, ref string strReturn, int nLength, int nWidth)
        {
            int num;
            strReturn = "";
            for (num = 0; num < (nLength + 2); num++)
            {
                if ((num % 2) == 0)
                {
                    strReturn = strReturn + '1';
                }
                else
                {
                    strReturn = strReturn + '0';
                }
            }
            for (num = 0; num < nWidth; num++)
            {
                strReturn = strReturn + '1';
                for (int i = 0; i < nLength; i++)
                {
                    strReturn = strReturn + ((char) strData[(num * nLength) + i]);
                }
                if ((num % 2) == 0)
                {
                    strReturn = strReturn + '1';
                }
                else
                {
                    strReturn = strReturn + '0';
                }
            }
            for (num = 0; num < (nLength + 2); num++)
            {
                strReturn = strReturn + '1';
            }
            return true;
        }

        private int FindFontMapping(int a, int b, int c, int d)
        {
            int num = 0;
            if (a == 0x31)
            {
                num += 8;
            }
            if (b == 0x31)
            {
                num += 4;
            }
            if (c == 0x31)
            {
                num += 2;
            }
            if (d == 0x31)
            {
                num++;
            }
            return num;
        }

        private int getBit(byte chr, int nBit)
        {
            if ((nBit >= 1) && (nBit <= 8))
            {
                return ((chr >> (8 - nBit)) & 1);
            }
            return 0;
        }

        private bool getC40Value(char cChar, ref ArrayList pBuf)
        {
            string text = " 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string text2 = "!\"#$%&'()*+,-./:;<=>?@[\\]^_";
            text2 = text2 + ((byte) 0xe8);
            string text3 = "'abcdefghijklmnopqrstuvwxyz{|}~";
            text3 = text3 + '\x007f';
            int index = text.IndexOf(cChar);
            if (index > -1)
            {
                pBuf.Add((char) (index + 3));
            }
            else if ((cChar >= '\0') && (cChar <= '\x001f'))
            {
                pBuf.Add('\0');
                pBuf.Add(cChar);
            }
            else
            {
                index = text2.IndexOf(cChar);
                if (index > -1)
                {
                    pBuf.Add('\x0001');
                    pBuf.Add((char) index);
                }
                else
                {
                    index = text3.IndexOf(cChar);
                    if (index > -1)
                    {
                        pBuf.Add('\x0002');
                        pBuf.Add((char) index);
                    }
                    else
                    {
                        if ((cChar < '\x0080') || (cChar > '\x00ff'))
                        {
                            return false;
                        }
                        index = text.IndexOf((char) (cChar - '\x0080'));
                        if (index > -1)
                        {
                            pBuf.Add('\x0001');
                            pBuf.Add('\x001e');
                            pBuf.Add((char) (index + 3));
                        }
                        else if (((cChar - '\x0080') >= 0) && ((cChar - '\x0080') <= 0x1f))
                        {
                            pBuf.Add('\x0001');
                            pBuf.Add('\x001e');
                            pBuf.Add('\0');
                            pBuf.Add((char) (cChar - '\x0080'));
                        }
                        else
                        {
                            index = text2.IndexOf((char) (cChar - '\x0080'));
                            if (index > -1)
                            {
                                pBuf.Add('\x0001');
                                pBuf.Add('\x001e');
                                pBuf.Add('\x0001');
                                pBuf.Add((char) index);
                            }
                            else
                            {
                                index = text3.IndexOf((char) (cChar - '\x0080'));
                                if (index > -1)
                                {
                                    pBuf.Add('\x0001');
                                    pBuf.Add('\x001e');
                                    pBuf.Add('\x0002');
                                    pBuf.Add((char) index);
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        private bool GetModule(ArrayList strData, ref string strMatrix, int nLeft, int nTop, int nLength, int nHeight)
        {
            int index = 0;
            ArrayList list = new ArrayList();
            for (int i = 0; i < (nLength * nHeight); i++)
            {
                list.Add('0');
            }
            for (int j = nTop; j < (nTop + nHeight); j++)
            {
                for (int k = nLeft; k < (nLeft + nLength); k++)
                {
                    list.RemoveAt(index);
                    list.Insert(index, (char) strData[(j * this.mapcols) + k]);
                    index++;
                }
            }
            return this.FinderPattern(list, ref strMatrix, nLength, nHeight);
        }

        private bool getTextValue(char cChar, ref ArrayList pBuf)
        {
            string text = " 0123456789abcdefghijklmnopqrstuvwxyz";
            string text2 = "!\"#$%&'()*+,-./:;<=>?@[\\]^_";
            text2 = text2 + ((byte) 0xe8);
            string text3 = "`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~";
            text3 = text3 + '\x007f';
            int index = text.IndexOf(cChar);
            if (index > -1)
            {
                pBuf.Add((char) (index + 3));
            }
            else if ((cChar >= '\0') && (cChar <= '\x001f'))
            {
                pBuf.Add('\0');
                pBuf.Add(cChar);
            }
            else
            {
                index = text2.IndexOf(cChar);
                if (index > -1)
                {
                    pBuf.Add('\x0001');
                    pBuf.Add((char) index);
                }
                else
                {
                    index = text3.IndexOf(cChar);
                    if (index > -1)
                    {
                        pBuf.Add('\x0002');
                        pBuf.Add((char) index);
                    }
                    else
                    {
                        if ((cChar < '\x0080') || (cChar > '\x00ff'))
                        {
                            return false;
                        }
                        index = text.IndexOf((char) (cChar - '\x0080'));
                        if (index > -1)
                        {
                            pBuf.Add('\x0001');
                            pBuf.Add('\x001e');
                            pBuf.Add((char) (index + 3));
                        }
                        else if (((cChar - '\x0080') >= 0) && ((cChar - '\x0080') <= 0x1f))
                        {
                            pBuf.Add('\x0001');
                            pBuf.Add('\x001e');
                            pBuf.Add('\0');
                            pBuf.Add((char) (cChar - '\x0080'));
                        }
                        else
                        {
                            index = text2.IndexOf((char) (cChar - '\x0080'));
                            if (index > -1)
                            {
                                pBuf.Add('\x0001');
                                pBuf.Add('\x001e');
                                pBuf.Add('\x0001');
                                pBuf.Add((char) index);
                            }
                            else
                            {
                                index = text3.IndexOf((char) (cChar - '\x0080'));
                                if (index > -1)
                                {
                                    pBuf.Add('\x0001');
                                    pBuf.Add('\x001e');
                                    pBuf.Add('\x0002');
                                    pBuf.Add((char) index);
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        private bool getX12Value(char cChar, ref ArrayList pBuf)
        {
            string text = " 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            text = '>' + text;
            text = '*' + text;
            int index = ('\r' + text).IndexOf(cChar);
            if (index > -1)
            {
                pBuf.Add((char) index);
                return true;
            }
            pBuf.Add('\x0003');
            return false;
        }

        private int LookAhead(ref ArrayList pBuf, int nCurPos)
        {
            double a = 1;
            double num2 = 2;
            double num3 = 2;
            double num4 = 2;
            double num5 = 2;
            double num6 = 2.25;
            string text = " ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string text2 = " 0123456789abcdefghijklmnopqrstuvwxyz";
            string text3 = " 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            text3 = '\r' + text3;
            text3 = '*' + text3;
            text3 = '>' + text3;
            string text4 = "@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_ !\"#$%&'()*+,-./0123456789:;<=>?";
            if (this.m_nCurEncodation == this.E_ASCII)
            {
                a = 0;
                num2 = 1;
                num3 = 1;
                num4 = 1;
                num5 = 1;
                num6 = 1.25;
            }
            else
            {
                a = 1;
                num2 = 2;
                num3 = 2;
                num4 = 2;
                num5 = 2;
                num6 = 2.25;
            }
            if (this.m_nCurEncodation == this.E_C40)
            {
                num2 = 0;
            }
            else if (this.m_nCurEncodation == this.E_TEXT)
            {
                num3 = 0;
            }
            else if (this.m_nCurEncodation == this.E_X12)
            {
                num4 = 0;
            }
            else if (this.m_nCurEncodation == this.E_EDIFACT)
            {
                num5 = 0;
            }
            else if (this.m_nCurEncodation == this.E_BASE256)
            {
                num6 = 0;
            }
            int count = pBuf.Count;
            for (int i = nCurPos; i < count; i++)
            {
                if (char.IsDigit("" + ((char) pBuf[i]), 0))
                {
                    a += 0.5;
                }
                else if (((char) pBuf[i]) > '\x007f')
                {
                    a = Math.Ceiling(a) + 2;
                }
                else
                {
                    a = Math.Ceiling(a) + 1;
                }
                if (text.IndexOf((char) pBuf[i]) >= 0)
                {
                    num2 += 0.66666666666666663;
                }
                else if (((char) pBuf[i]) > '\x007f')
                {
                    num2 += 2.6666666666666665;
                }
                else
                {
                    num2 += 1.3333333333333333;
                }
                if (text2.IndexOf((char) pBuf[i]) >= 0)
                {
                    num3 += 0.66666666666666663;
                }
                else if (((char) pBuf[i]) > '\x007f')
                {
                    num3 += 2.6666666666666665;
                }
                else
                {
                    num3 += 1.3333333333333333;
                }
                if (text3.IndexOf((char) pBuf[i]) >= 0)
                {
                    num4 += 0.66666666666666663;
                }
                else if (((char) pBuf[i]) > '\x007f')
                {
                    num4 += 4.333333333333333;
                }
                else
                {
                    num4 += 3.3333333333333335;
                }
                if (text4.IndexOf((char) pBuf[i]) >= 0)
                {
                    num5 += 0.75;
                }
                else if (((char) pBuf[i]) > '\x007f')
                {
                    num5 += 4.25;
                }
                else
                {
                    num5 += 3.25;
                }
                if (((((char) pBuf[i]) == '\x00e8') || (((char) pBuf[i]) == '\x00e9')) || ((((char) pBuf[i]) == '\x00ea') || (((char) pBuf[i]) == '\x00f1')))
                {
                    num6 += 4;
                }
                else
                {
                    num6 += 1;
                }
                a = (float) a;
                num6 = (float) num6;
                num5 = (float) num5;
                num3 = (float) num3;
                num4 = (float) num4;
                num2 = (float) num2;
                if ((i - nCurPos) >= 4)
                {
                    if (((((a + 1) <= num2) && ((a + 1) <= num3)) && (((a + 1) <= num4) && ((a + 1) <= num5))) && ((a + 1) <= num6))
                    {
                        return this.E_ASCII;
                    }
                    if (((((num6 + 1) <= a) && ((num6 + 1) < num2)) && (((num6 + 1) < num3) && ((num6 + 1) < num4))) && ((num6 + 1) < num5))
                    {
                        return this.E_BASE256;
                    }
                    if (((((num5 + 1) < a) && ((num5 + 1) < num2)) && (((num5 + 1) < num3) && ((num5 + 1) < num4))) && ((num5 + 1) < num6))
                    {
                        return this.E_EDIFACT;
                    }
                    if (((((num3 + 1) < a) && ((num3 + 1) < num2)) && (((num3 + 1) < num4) && ((num3 + 1) < num5))) && ((num3 + 1) < num6))
                    {
                        return this.E_TEXT;
                    }
                    if (((((num4 + 1) < a) && ((num4 + 1) < num2)) && (((num4 + 1) < num3) && ((num4 + 1) < num5))) && ((num4 + 1) < num6))
                    {
                        return this.E_X12;
                    }
                    if ((((num2 + 1) < a) && ((num2 + 1) < num3)) && (((num2 + 1) < num5) && ((num2 + 1) < num6)))
                    {
                        if (num2 < num4)
                        {
                            return this.E_C40;
                        }
                        if (num2 == num4)
                        {
                            bool flag = false;
                            bool flag2 = false;
                            int num7 = i + 1;
                            while ((num7 < count) && (text3.IndexOf((char) pBuf[num7]) >= 0))
                            {
                                if ((text3.IndexOf((char) pBuf[num7]) >= 0) && (text3.IndexOf((char) pBuf[num7]) <= 2))
                                {
                                    flag = true;
                                    break;
                                }
                                num7++;
                            }
                            if (num7 == count)
                            {
                                flag2 = true;
                            }
                            if (!flag2 && !flag)
                            {
                                return this.E_C40;
                            }
                            return this.E_X12;
                        }
                    }
                }
            }
            a = Math.Ceiling(a);
            num2 = Math.Ceiling(num2);
            num3 = Math.Ceiling(num3);
            num4 = Math.Ceiling(num4);
            num5 = Math.Ceiling(num5);
            num6 = Math.Ceiling(num6);
            if ((((a <= num2) && (a <= num3)) && ((a <= num4) && (a <= num5))) && (a <= num6))
            {
                return this.E_ASCII;
            }
            if ((((num6 < a) && (num6 < num2)) && ((num6 < num3) && (num6 < num4))) && (num6 < num5))
            {
                return this.E_BASE256;
            }
            if ((((num5 < a) && (num5 < num2)) && ((num5 < num3) && (num5 < num4))) && (num5 < num6))
            {
                return this.E_EDIFACT;
            }
            if ((((num3 < a) && (num3 < num2)) && ((num3 < num4) && (num3 < num5))) && (num3 < num6))
            {
                return this.E_TEXT;
            }
            if ((((num4 < a) && (num4 < num2)) && ((num4 < num3) && (num4 < num5))) && (num4 < num6))
            {
                return this.E_X12;
            }
            return this.E_C40;
        }

        private void module(int row, int col, byte chr, int bit, ref ArrayList nArray, ref ArrayList bPlaced)
        {
            if (row < 0)
            {
                row += this.maprows;
                col += 4 - ((this.maprows + 4) % 8);
            }
            if (col < 0)
            {
                col += this.mapcols;
                row += 4 - ((this.mapcols + 4) % 8);
            }
            int num = this.getBit(chr, bit);
            int index = (row * this.mapcols) + col;
            nArray.RemoveAt(index);
            nArray.Insert(index, (char) (num + 0x30));
            bPlaced.RemoveAt(index);
            bPlaced.Insert(index, '1');
        }

        private void ModulePlace(ArrayList strData, ref ArrayList strMatrix)
        {
            string text = "";
            int num = 0;
            int num2 = 0;
            if (this.regions == 2)
            {
                this.GetModule(strData, ref text, 0, 0, this.datacols, this.datarows);
                this.PutModule(ref strMatrix, text, 0, 0, this.datacols + 2, this.datarows + 2);
                text = "";
                this.GetModule(strData, ref text, this.datacols, 0, this.datacols, this.datarows);
                this.PutModule(ref strMatrix, text, this.datacols + 2, 0, this.datacols + 2, this.datarows + 2);
                text = "";
            }
            else
            {
                int num3 = (int) Math.Sqrt((double) this.regions);
                for (num = 0; num < num3; num++)
                {
                    for (num2 = 0; num2 < num3; num2++)
                    {
                        this.GetModule(strData, ref text, num * this.datacols, num2 * this.datarows, this.datacols, this.datarows);
                        this.PutModule(ref strMatrix, text, num * (this.datacols + 2), num2 * (this.datarows + 2), this.datacols + 2, this.datarows + 2);
                    }
                }
            }
        }

        private bool PutModule(ref ArrayList strData, string strMatrix, int nLeft, int nTop, int nLength, int nHeight)
        {
            int num3 = 0;
            for (int i = nTop; i < (nTop + nHeight); i++)
            {
                for (int j = nLeft; j < (nLeft + nLength); j++)
                {
                    int index = (i * this.m_nCol) + j;
                    char ch = strMatrix[num3++];
                    strData.RemoveAt(index);
                    strData.Insert(index, ch);
                }
            }
            return true;
        }

        private byte Random253State(int i, int j)
        {
            int num = ((0x95 * j) % 0xfd) + 1;
            int num2 = i + num;
            if (num2 <= 0xfe)
            {
                return (byte) num2;
            }
            return (byte) (num2 - 0xfe);
        }

        private int Text_Encode(ref int nCurPos, ref ArrayList pBuf, int nLen, bool bAuto)
        {
            int num4;
            int num7;
            int num = 0;
            ArrayList list = new ArrayList();
            int num2 = 0;
            int num3 = nCurPos;
            int count = this.szDataToEncode.Count;
            if (!bAuto)
            {
                num7 = nLen;
            }
            else
            {
                num7 = nCurPos;
                num3 = nCurPos;
                while (num3 < count)
                {
                    if (this.LookAhead(ref this.szDataToEncode, num3) != this.E_TEXT)
                    {
                        break;
                    }
                    num3++;
                }
                num7 = num3;
            }
            num3 = nCurPos;
            while ((num3 < num7) || ((num2 + 3) <= list.Count))
            {
                int num5;
                while ((num2 + 3) <= list.Count)
                {
                    num4 = ((('ـ' * ((char) list[num2])) + ('(' * ((char) list[num2 + 1]))) + ((char) list[num2 + 2])) + 1;
                    pBuf.Add((char) (num4 / 0x100));
                    pBuf.Add((char) (num4 % 0x100));
                    num += 2;
                    num2 += 3;
                }
                if ((num3 + 3) > num7)
                {
                    num5 = num7;
                }
                else
                {
                    num5 = num3 + 3;
                }
                while (num3 < num5)
                {
                    this.getTextValue((char) this.szDataToEncode[num3], ref list);
                    num3++;
                }
            }
        Label_0116:
            if ((num2 + 3) == list.Count)
            {
                num4 = ((('ـ' * ((char) list[num2])) + ('(' * ((char) list[num2 + 1]))) + ((char) list[num2 + 2])) + 1;
                pBuf.Add((char) (num4 / 0x100));
                pBuf.Add((char) (num4 % 0x100));
                num += 2;
            }
            else if ((num2 + 2) == list.Count)
            {
                num4 = (('ـ' * ((char) list[num2])) + ('(' * ((char) list[num2 + 1]))) + 1;
                pBuf.Add((char) (num4 / 0x100));
                pBuf.Add((char) (num4 % 0x100));
                num += 2;
            }
            else if ((num2 + 1) == list.Count)
            {
                if ((num3 < count) && (((char) this.szDataToEncode[num3]) < '\x0080'))
                {
                    this.getTextValue((char) this.szDataToEncode[num3++], ref list);
                    goto Label_0116;
                }
                num4 = ((('ـ' * ((char) list[num2])) + 40) + 30) + 1;
                pBuf.Add((char) (num4 / 0x100));
                pBuf.Add((char) (num4 % 0x100));
                num += 2;
            }
            if (num3 < count)
            {
                this.m_nCurEncodation = this.E_ASCII;
                pBuf.Add('\x00fe');
                num++;
            }
            nCurPos = num3;
            return num;
        }

        protected ArrayList TildeCodes(string inpara)
        {
            ArrayList list = new ArrayList();
            for (int i = 0; i < inpara.Length; i++)
            {
                if (inpara[i] == '~')
                {
                    int num = inpara[i + 1] - '@';
                    if ((num > 0) && (num < 0x1b))
                    {
                        list.Add((char) num);
                        i++;
                    }
                    else if ((((inpara.Length - i) > 2) && char.IsNumber(inpara[i + 1])) && (char.IsNumber(inpara[i + 2]) && char.IsNumber(inpara[i + 3])))
                    {
                        num = (((100 * (inpara[i + 1] - '0')) + (10 * (inpara[i + 2] - '0'))) + inpara[i + 3]) - 0x30;
                        if (num < 0x100)
                        {
                            list.Add((char) num);
                            i += 3;
                        }
                        else
                        {
                            list.Add(inpara[i + 1]);
                            i++;
                        }
                    }
                    else if (inpara[i + 1] == '1')
                    {
                        list.Add('\x00c8');
                        i++;
                    }
                    else if (inpara[i + 1] == '~')
                    {
                        list.Add('~');
                        i++;
                    }
                    else
                    {
                        list.Add('~');
                    }
                }
                else
                {
                    list.Add(inpara[i]);
                }
            }
            return list;
        }

        private void utah(int row, int col, byte chr, ref ArrayList nArray, ref ArrayList bPlaced)
        {
            this.module(row - 2, col - 2, chr, 1, ref nArray, ref bPlaced);
            this.module(row - 2, col - 1, chr, 2, ref nArray, ref bPlaced);
            this.module(row - 1, col - 2, chr, 3, ref nArray, ref bPlaced);
            this.module(row - 1, col - 1, chr, 4, ref nArray, ref bPlaced);
            this.module(row - 1, col, chr, 5, ref nArray, ref bPlaced);
            this.module(row, col - 2, chr, 6, ref nArray, ref bPlaced);
            this.module(row, col - 1, chr, 7, ref nArray, ref bPlaced);
            this.module(row, col, chr, 8, ref nArray, ref bPlaced);
        }

        private int X12_Encode(ref int nCurPos, ref ArrayList pBuf, int nLen, bool bAuto)
        {
            int num7;
            int num = 0;
            ArrayList list = new ArrayList();
            int num2 = 0;
            int num3 = nCurPos;
            int count = this.szDataToEncode.Count;
            if (!bAuto)
            {
                num7 = nLen;
            }
            else
            {
                num7 = nCurPos;
                num3 = nCurPos;
                while (num3 < count)
                {
                    if (this.LookAhead(ref this.szDataToEncode, num3) != this.E_X12)
                    {
                        break;
                    }
                    num3++;
                }
                num7 = num3;
            }
            num3 = nCurPos;
            while ((num3 < num7) || ((num2 + 3) <= list.Count))
            {
                while ((num2 + 3) <= list.Count)
                {
                    int num4 = ((('ـ' * ((char) list[num2])) + ('(' * ((char) list[num2 + 1]))) + ((char) list[num2 + 2])) + 1;
                    pBuf.Add((char) (num4 / 0x100));
                    pBuf.Add((char) (num4 % 0x100));
                    num += 2;
                    num2 += 3;
                }
                if ((num3 + 3) > num7)
                {
                    break;
                }
                int num5 = num3 + 3;
                while (num3 < num5)
                {
                    this.getX12Value((char) this.szDataToEncode[num3], ref list);
                    num3++;
                }
            }
            if (num3 < count)
            {
                this.m_nCurEncodation = this.E_ASCII;
                pBuf.Add('\x00fe');
                num++;
            }
            nCurPos = num3;
            return num;
        }
    }
}

