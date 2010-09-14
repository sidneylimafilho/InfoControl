namespace InfoControl.Web.UI.WebControls.BarCode.Encoder
{
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    public class LinearEncoder
    {
        private string CHARSET_NUMERIC = "0123456789";
        private string CHARSET_UEB = "0123456789,";
        private string CodabarIndex = "0123456789$-./:+ABCD";
        private static string[] CodabarMapTable = new string[] { 
            "nnnnnwwn", "nnnnwwnn", "nnnwnnwn", "wwnnnnnn", "nnwnnwnn", "wnnnnwnn", "nwnnnnwn", "nwnnwnnn", "nwwnnnnn", "wnnwnnnn", "nnwwnnnn", "nnnwwnnn", "wnwnwnnn", "wnwnnnwn", "wnnnwnwn", "nnwwwwwn", 
            "nnwwnwnn", "nwnwnnwn", "nnnwnwwn", "nnnwwwnn"
         };
        private string Code11Index = "0123456789-";
        private static string[] Code11MapTable = new string[] { "nnnnwn", "wnnnwn", "nwnnwn", "wwnnnn", "nnwnwn", "wnwnnn", "nwwnnn", "nnnwwn", "wnnwnn", "wnnnnn", "nnwnnn" };
        private string Code11Start = "nnwwnn";
        private const int CODE128_FNC1 = 200;
        private const int CODE128_FNC2 = 0xc9;
        private const int CODE128_FNC3 = 0xca;
        private const int CODE128_FNC4 = 0xcb;
        private static string[] Code128MapTable = new string[] { 
            "11011001100", "11001101100", "11001100110", "10010011000", "10010001100", "10001001100", "10011001000", "10011000100", "10001100100", "11001001000", "11001000100", "11000100100", "10110011100", "10011011100", "10011001110", "10111001100", 
            "10011101100", "10011100110", "11001110010", "11001011100", "11001001110", "11011100100", "11001110100", "11101101110", "11101001100", "11100101100", "11100100110", "11101100100", "11100110100", "11100110010", "11011011000", "11011000110", 
            "11000110110", "10100011000", "10001011000", "10001000110", "10110001000", "10001101000", "10001100010", "11010001000", "11000101000", "11000100010", "10110111000", "10110001110", "10001101110", "10111011000", "10111000110", "10001110110", 
            "11101110110", "11010001110", "11000101110", "11011101000", "11011100010", "11011101110", "11101011000", "11101000110", "11100010110", "11101101000", "11101100010", "11100011010", "11101111010", "11001000010", "11110001010", "10100110000", 
            "10100001100", "10010110000", "10010000110", "10000101100", "10000100110", "10110010000", "10110000100", "10011010000", "10011000010", "10000110100", "10000110010", "11000010010", "11001010000", "11110111010", "11000010100", "10001111010", 
            "10100111100", "10010111100", "10010011110", "10111100100", "10011110100", "10011110010", "11110100100", "11110010100", "11110010010", "11011011110", "11011110110", "11110110110", "10101111000", "10100011110", "10001011110", "10111101000", 
            "10111100010", "11110101000", "11110100010", "10111011110", "10111101110", "11101011110", "11110101110", "11010111100", "11010010000", "11010011100"
         };
        private string Code128Stop = "11000111010";
        private string Code128Termination = "11";
        private static string[] Code25MapTable = new string[] { "nnnnwnwnnn", "wnnnnnnnwn", "nnwnnnnnwn", "wnwnnnnnnn", "nnnnwnnnwn", "wnnnwnnnnn", "nnwnwnnnnn", "nnnnnnwnwn", "wnnnnnwnnn", "nnwnnnwnnn" };
        private string Code25Start = "11011010";
        private string Code25Stop = "11010110";
        private static string[] Code39ExtTable = new string[] { 
            "%U", "$A", "$B", "$C", "$D", "$E", "$F", "$G", "$H", "$I", "$J", "$K", "$L", "$M", "$N", "$O", 
            "$P", "$Q", "$R", "$S", "$T", "$U", "$V", "$W", "$X", "$Y", "$Z", "%A", "%B", "%C", "%D", "%E", 
            " ", "/A", "/B", "/C", "/D", "/E", "/F", "/G", "/H", "/I", "/J", "/K", "/L", "-", ".", "/O", 
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "/Z", "%F", "%G", "%H", "%I", "%J", 
            "%V", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", 
            "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "%K", "%L", "%M", "%N", "%O", 
            "%W", "+A", "+B", "+C", "+D", "+E", "+F", "+G", "+H", "+I", "+J", "+K", "+L", "+M", "+N", "+O", 
            "+P", "+Q", "+R", "+S", "+T", "+U", "+V", "+W", "+X", "+Y", "+Z", "%P", "%Q", "%R", "%S", "%T"
         };
        private string Code39Index = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%";
        private static string[] Code39Map = new string[] { 
            "nnnwwnwnnn", "wnnwnnnnwn", "nnwwnnnnwn", "wnwwnnnnnn", "nnnwwnnnwn", "wnnwwnnnnn", "nnwwwnnnnn", "nnnwnnwnwn", "wnnwnnwnnn", "nnwwnnwnnn", "wnnnnwnnwn", "nnwnnwnnwn", "wnwnnwnnnn", "nnnnwwnnwn", "wnnnwwnnnn", "nnwnwwnnnn", 
            "nnnnnwwnwn", "wnnnnwwnnn", "nnwnnwwnnn", "nnnnwwwnnn", "wnnnnnnwwn", "nnwnnnnwwn", "wnwnnnnwnn", "nnnnwnnwwn", "wnnnwnnwnn", "nnwnwnnwnn", "nnnnnnwwwn", "wnnnnnwwnn", "nnwnnnwwnn", "nnnnwnwwnn", "wwnnnnnnwn", "nwwnnnnnwn", 
            "wwwnnnnnnn", "nwnnwnnnwn", "wwnnwnnnnn", "nwwnwnnnnn", "nwnnnnwnwn", "wwnnnnwnnn", "nwwnnnwnnn", "nwnwnwnnnn", "nwnwnnnwnn", "nwnnnwnwnn", "nnnwnwnwnn"
         };
        private string code39Start = "nwnnwnwnnn";
        private string Code93Index = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%@#^&";
        private static string[] Code93MapTable = new string[] { 
            "100010100", "101001000", "101000100", "101000010", "100101000", "100100100", "100100010", "101010000", "100010010", "100001010", "110101000", "110100100", "110100010", "110010100", "110010010", "110001010", 
            "101101000", "101100100", "101100010", "100110100", "100011010", "101011000", "101001100", "101000110", "100101100", "100010110", "110110100", "110110010", "110101100", "110100110", "110010110", "110011010", 
            "101101100", "101100110", "100110110", "100111010", "100101110", "111010100", "111010010", "111001010", "101101110", "101110110", "110101110", "100100110", "111011010", "111010110", "100110010", "101011110"
         };
        private string I25Start = "1010";
        private string I25Stop = "1101";
        private static string[] Itf25MapTable = new string[] { "nnwwn", "wnnnw", "nwnnw", "wwnnn", "nnwnw", "wnwnn", "nwwnn", "nnnww", "wnnwn", "nwnwn" };
        private static string[] LefthandEven = new string[] { "0100111", "0110011", "0011011", "0100001", "0011101", "0111001", "0000101", "0010001", "0001001", "0010111" };
        private static string[] LefthandOdd = new string[] { "0001101", "0011001", "0010011", "0111101", "0100011", "0110001", "0101111", "0111011", "0110111", "0001011" };
        private static string[] MSIMapTable = new string[] { "100100100100", "100100100110", "100100110100", "100100110110", "100110100100", "100110100110", "100110110100", "100110110110", "110100100100", "110100100110" };
        private string MSIStart = "110";
        private string MSIStop = "1001";
        public int narrowratio;
        private char NULL_CHAR = '\0';
        private static string[] Parity5 = new string[] { "00111", "01011", "01101", "01110", "10011", "11001", "11100", "10101", "10110", "11010" };
        private static string[] PostnetMapTable = new string[] { "11000", "00011", "00101", "00110", "01001", "01010", "01100", "10001", "10010", "10100" };
        private string telepenEnd = "wwnnnnnnnnnn";
        private string telepenStart = "nnnnnnnnnnww";
        private string uebCenterGuards = "01010";
        private string uebSeparator = "01";
        private string uebStartGuards = "101";
        private static string[] upc_code_c = new string[] { "1110010", "1100110", "1101100", "1000010", "1011100", "1001110", "1010000", "1000100", "1001000", "1110100" };

        public string BooklandEncoding(ref string str, bool checkDigit, out string checkStr, out string demoStr, out string drawText)
        {
            string text = this.ConvertISBN2EAN13(str);
            str = text;
            return this.Ean13Encoding(ref text, checkDigit, out checkStr, out demoStr, out drawText);
        }

        public string CodabarEncoding(ref string str, bool checkDigit, out string checkStr, out string demoStr, out string drawText)
        {
            int length = 0;
            checkStr = "";
            string text3 = "";
            string text = this.TildeCodes(str);
            string text2 = this.MaskFilter(text, this.CodabarIndex);
            str = text2;
            switch (text2[0])
            {
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                    break;

                case 'a':
                case 'b':
                case 'c':
                case 'd':
                    text2.Remove(0, 1);
                    text2.Insert(0, "" + ((char) (text2[0] - ' ')));
                    break;

                default:
                    text2 = 'A' + text2;
                    break;
            }
            length = text2.Length;
            switch (text2[length - 1])
            {
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                    break;

                case 'a':
                case 'b':
                case 'c':
                case 'd':
                    text2.Remove(0, 1);
                    text2.Insert(0, "" + ((char) (text2[0] - ' ')));
                    break;

                default:
                    text2 = text2 + 'B';
                    break;
            }
            demoStr = "";
            for (length = 0; length < text2.Length; length++)
            {
                text3 = text3 + this.codeTobinary(CodabarMapTable[this.CodabarIndex.IndexOf(text2[length])]);
                object obj2 = demoStr;
                demoStr = string.Concat(new object[] { obj2, text2[length], ":", this.codeTobinary(CodabarMapTable[this.CodabarIndex.IndexOf(text2[length])]), '\x0080' });
            }
            drawText = ((string) str) + ((string) checkStr);
            return text3;
        }

        public string Code11Encoding(ref string str, bool checkDigit, out string checkStr, out string demoStr, out string drawText)
        {
            char ch;
            object obj2;
            int num2 = 0;
            string text = this.TildeCodes(str);
            string text2 = this.MaskFilter(text, this.Code11Index);
            if (text2.Length > 10)
            {
                text2 = text2.Substring(0, 10);
            }
            str = text2;
            string text3 = this.codeTobinary(this.Code11Start);
            demoStr = "Start Character:" + this.codeTobinary(this.Code11Start) + '\x0080';
            int length = text2.Length;
            for (int i = 0; i < length; i++)
            {
                int index = this.Code11Index.IndexOf(text2[(length - i) - 1]);
                if (index >= 0)
                {
                    num2 += (i + 1) * index;
                }
                text3 = text3 + this.codeTobinary(Code11MapTable[this.Code11Index.IndexOf(text2[i])]);
                obj2 = demoStr;
                demoStr = string.Concat(new object[] { obj2, text2[i], ":", this.codeTobinary(Code11MapTable[this.Code11Index.IndexOf(text2[i])]), '\x0080' });
            }
            num2 = num2 % 11;
            if (num2 == 10)
            {
                ch = '-';
            }
            else
            {
                ch = (char) (0x30 + num2);
            }
            checkStr = "" + ch;
            text3 = text3 + this.codeTobinary(Code11MapTable[this.Code11Index.IndexOf(ch)]);
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Check Digit ", ch, ":", this.codeTobinary(Code11MapTable[this.Code11Index.IndexOf(ch)]), '\x0080' });
            text3 = text3 + this.codeTobinary(this.Code11Start);
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "End Character:", this.codeTobinary(this.Code11Start), '\x0080' });
            drawText = ((string) str) + ((string) checkStr);
            return text3;
        }

        public string Code128Encoding(ref string str, bool checkDigit, out string checkStr, out string demoStr, out string drawText)
        {
            object obj2;
            int num = 0;
            int num4 = 0;
            string text2 = this.Get128CharSetA();
            string text3 = this.Get128CharSetB();
            string text4 = this.CHARSET_NUMERIC;
            string text5 = this.TildeCodes(str);
            string text6 = "";
            demoStr = "";
            str = text5;
            ArrayList list = new ArrayList();
            string text = text4;
            list.Add(0x69);
            int length = text5.Length;
            for (num = 0; num < length; num++)
            {
                if (text5[num] == '\x00c8')
                {
                    list.Add(0x66);
                }
                else if (text5[num] == '\x00c9')
                {
                    if (text == text4)
                    {
                        list.Add(100);
                        text = text3;
                    }
                    list.Add(0x61);
                }
                else if (text5[num] == '\x00ca')
                {
                    if (text == text4)
                    {
                        list.Add(100);
                        text = text3;
                    }
                    list.Add(0x60);
                }
                else if (text5[num] == '\x00cb')
                {
                    if (text == text4)
                    {
                        list.Add(100);
                        text = text3;
                        list.Add(100);
                    }
                    else if (text == text3)
                    {
                        list.Add(100);
                    }
                    else
                    {
                        list.Add(0x65);
                    }
                }
                else
                {
                    int index;
                    if (((text == text4) && (num < (length - 1))) && (char.IsNumber(text5[num]) && char.IsNumber(text5[num + 1])))
                    {
                        index = ((10 * (text5[num] - '0')) + text5[num + 1]) - 0x30;
                        list.Add(index);
                        num++;
                    }
                    else if ((((num <= (length - 4)) && char.IsNumber(text5[num])) && (char.IsNumber(text5[num + 1]) && char.IsNumber(text5[num + 2]))) && char.IsNumber(text5[num + 3]))
                    {
                        if (text != text4)
                        {
                            list.Add(0x63);
                            text = text4;
                        }
                        index = ((10 * (text5[num] - '0')) + text5[num + 1]) - 0x30;
                        list.Add(index);
                        index = ((10 * (text5[num + 2] - '0')) + text5[num + 3]) - 0x30;
                        list.Add(index);
                        num += 3;
                    }
                    else if ((((num <= (length - 5)) && char.IsNumber(text5[num])) && (char.IsNumber(text5[num + 1]) && (text5[num + 2] == '\x00c8'))) && (char.IsNumber(text5[num + 3]) && char.IsNumber(text5[num + 4])))
                    {
                        if (text != text4)
                        {
                            list.Add(0x63);
                            text = text4;
                        }
                        index = ((10 * (text5[num] - '0')) + text5[num + 1]) - 0x30;
                        list.Add(index);
                        list.Add(0x66);
                        index = ((10 * (text5[num + 3] - '0')) + text5[num + 4]) - 0x30;
                        list.Add(index);
                        num += 4;
                    }
                    else if (((num <= (length - 1)) && (text5[num] > '\x001f')) && (text5[num] < '\x0080'))
                    {
                        if (text == text4)
                        {
                            list.Add(100);
                            text = text3;
                        }
                        else if ((text == text2) && (text5[num] > '`'))
                        {
                            list.Add(100);
                            text = text3;
                        }
                        index = text.IndexOf(text5[num]);
                        if (index >= 0)
                        {
                            list.Add(index);
                        }
                    }
                    else if (((num <= (length - 1)) && ((text5[num] < '\x001f') || (text5[num] == this.NULL_CHAR))) || (((text == text2) && (text5[num] > ' ')) && (text5[num] < '`')))
                    {
                        if (text != text2)
                        {
                            list.Add(0x65);
                            text = text2;
                        }
                        index = text.IndexOf(text5[num]);
                        if (index >= 0)
                        {
                            list.Add(index);
                        }
                    }
                }
            }
            int num6 = 0;
            if (((int) list[1]) == 100)
            {
                list.RemoveAt(0);
                list.RemoveAt(0);
                list.Insert(0, 0x68);
            }
            else if (((int) list[1]) == 0x65)
            {
                list.RemoveAt(0);
                list.RemoveAt(0);
                list.Insert(0, 0x67);
            }
            length = list.Count;
            for (num = 0; num < length; num++)
            {
                int num7;
                num6 = (int) list[num];
                if (num > 1)
                {
                    num7 = num;
                }
                else
                {
                    num7 = 1;
                }
                num4 += num6 * num7;
                text6 = text6 + Code128MapTable[num6];
                string text7 = "";
                switch (num6)
                {
                    case 0x66:
                        text7 = "FNC1";
                        break;

                    case 0x67:
                        text7 = "Start A";
                        break;

                    case 0x68:
                        text7 = "Strat B";
                        break;

                    case 0x69:
                        text7 = "Start C";
                        break;

                    default:
                        text7 = num6.ToString();
                        break;
                }
                obj2 = demoStr;
                demoStr = string.Concat(new object[] { obj2, text7, ": ", Code128MapTable[num6], '\x0080' });
            }
            int num5 = num4 % 0x67;
            text6 = text6 + Code128MapTable[num5];
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "CheckDigit ", num5.ToString(), ": ", Code128MapTable[num5], '\x0080' });
            text6 = text6 + this.Code128Stop + this.Code128Termination;
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Stop:", this.Code128Stop, '\x0080' });
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Termination Bar:", this.Code128Termination, '\x0080' });
            checkStr = num5.ToString();
            drawText = ((string) str) + ((string) checkStr);
            return text6;
        }

        public string Code25Encoding(ref string str, bool checkDigit, out string checkStr, out string demoStr, out string drawText)
        {
            object obj2;
            int num = 0;
            char ch = '0';
            string text = this.TildeCodes(str);
            string text2 = this.MaskFilter(text, this.CHARSET_NUMERIC);
            string text3 = this.Code25Start;
            str = text2;
            demoStr = "Start :" + this.Code25Start + '\x0080';
            int length = text2.Length;
            for (int i = 0; i < length; i++)
            {
                if ((i % 2) == 0)
                {
                    num += 3 * (text2[(length - i) - 1] - '0');
                }
                else
                {
                    num += text2[(length - i) - 1] - '0';
                }
                text3 = text3 + this.codeTobinary(Code25MapTable[text2[i] - '0']);
                obj2 = demoStr;
                demoStr = string.Concat(new object[] { obj2, text2[i], ":", this.codeTobinary(Code25MapTable[text2[i] - '0']), '\x0080' });
            }
            num = num % 10;
            if (num != 0)
            {
                ch = (char) ((10 - num) + 0x30);
            }
            else
            {
                ch = '0';
            }
            checkStr = "" + ch;
            if (checkDigit)
            {
                text3 = text3 + this.codeTobinary(Code25MapTable[ch - '0']);
                obj2 = demoStr;
                demoStr = string.Concat(new object[] { obj2, "Check Digit ", ch, ":", this.codeTobinary(Code25MapTable[ch - '0']), '\x0080' });
            }
            text3 = text3 + this.Code25Stop;
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Stop :", this.Code25Stop, '\x0080' });
            drawText = ((string) str) + ((string) checkStr);
            return text3;
        }

        public string Code39Encoding(ref string str, bool checkDigit, out string checkStr, out string demoStr, out string drawText)
        {
            object obj2;
            drawText = str;
            char ch = '0';
            string text = this.TildeCodes(str);
            text = this.MaskFilter(text, this.Code39Index);
            str = text;
            checkStr = "";
            demoStr = "";
            string t = this.code39Start;
            demoStr = "Start *:" + this.codeTobinary(this.code39Start) + '\x0080';
            for (int i = 0; i < text.Length; i++)
            {
                ch = text[i];
                t = t + Code39Map[this.Code39Index.IndexOf(ch)];
                obj2 = demoStr;
                demoStr = string.Concat(new object[] { obj2, ch, ": ", this.codeTobinary(Code39Map[this.Code39Index.IndexOf(ch)]), '\x0080' });
            }
            if (checkDigit)
            {
                int num2 = 0;
                for (int j = 0; j < text.Length; j++)
                {
                    ch = text[j];
                    num2 += this.Code39Index.IndexOf(ch);
                }
                ch = this.Code39Index[num2 % 0x2b];
                t = t + Code39Map[this.Code39Index.IndexOf(ch)];
                checkStr = "" + ch;
                obj2 = demoStr;
                demoStr = string.Concat(new object[] { obj2, ch, ": ", this.codeTobinary(Code39Map[this.Code39Index.IndexOf(ch)]), '\x0080' });
            }
            t = t + this.code39Start;
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Stop *: ", this.codeTobinary(this.code39Start), '\x0080' });
            drawText = ((string) str) + ((string) checkStr);
            return this.codeTobinary(t);
        }

        public string Code39ExtendedEncoding(ref string str, bool checkDigit, out string checkStr, out string demoStr, out string drawText)
        {
            string text = "";
            str = this.TildeCodes(str);
            for (int i = 0; i < str.Length; i++)
            {
                text = text + Code39ExtTable[str[i]];
            }
            return this.Code39Encoding(ref text, checkDigit, out checkStr, out demoStr, out drawText);
        }

        public string Code93Encoding(ref string str, bool checkDigit, out string checkStr, out string demoStr, out string drawText)
        {
            int index;
            int num7;
            object obj2;
            string text = "";
            string text2 = "";
            string text3 = "";
            text2 = Code93MapTable[0x2f];
            demoStr = "Start *:" + Code93MapTable[0x2f] + '\x0080';
            str = this.TildeCodes(str);
            for (num7 = 0; num7 < str.Length; num7++)
            {
                char ch3 = str[num7];
                if (ch3 <= '\x007f')
                {
                    if ((((ch3 != '@') && (ch3 != '#')) && ((ch3 != '^') && (ch3 != '&'))) && (this.Code93Index.IndexOf(ch3) >= 0))
                    {
                        text = "" + ch3;
                    }
                    else
                    {
                        text = Code39ExtTable[ch3];
                    }
                    for (int i = 0; i < text.Length; i++)
                    {
                        ch3 = text[i];
                        switch (ch3)
                        {
                            case '$':
                                ch3 = '@';
                                break;

                            case '/':
                                ch3 = '^';
                                break;

                            case '+':
                                ch3 = '&';
                                break;

                            case '%':
                                ch3 = '#';
                                break;
                        }
                        text3 = text3 + ch3;
                        text2 = text2 + Code93MapTable[this.Code93Index.IndexOf(ch3)];
                        obj2 = demoStr;
                        demoStr = string.Concat(new object[] { obj2, ch3, ":", Code93MapTable[this.Code93Index.IndexOf(ch3)], '\x0080' });
                    }
                }
            }
            int length = text3.Length;
            int num5 = 0;
            for (num7 = 0; num7 < length; num7++)
            {
                int num = (num7 + 1) % 20;
                if (num == 0)
                {
                    num = 20;
                }
                index = this.Code93Index.IndexOf(text3[(length - num7) - 1]);
                num5 += num * index;
            }
            num5 = num5 % 0x2f;
            char ch = this.Code93Index[num5];
            text3 = text3 + ch;
            length = text3.Length;
            int num6 = 0;
            for (num7 = 0; num7 < length; num7++)
            {
                int num2 = (num7 + 1) % 15;
                if (num2 == 0)
                {
                    num2 = 15;
                }
                index = this.Code93Index.IndexOf(text3[(length - num7) - 1]);
                num6 += num2 * index;
            }
            num6 = num6 % 0x2f;
            char ch2 = this.Code93Index[num6];
            text2 = (text2 + Code93MapTable[this.Code93Index.IndexOf(ch)] + Code93MapTable[this.Code93Index.IndexOf(ch2)]) + Code93MapTable[0x2f] + '1';
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "C Check Digit ", ch, ":", Code93MapTable[this.Code93Index.IndexOf(ch)], '\x0080' });
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "K Check Digit ", ch2, ":", Code93MapTable[this.Code93Index.IndexOf(ch2)], '\x0080' });
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Stop *:", Code93MapTable[0x2f], '\x0080' });
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Termination Bar:", '1', '\x0080' });
            checkStr = "" + ch + ch2;
            drawText = ((string) str) + ((string) checkStr);
            return text2;
        }

        public string codeTobinary(string t)
        {
            string text = "";
            t = t.ToLower();
            for (int i = 0; i < t.Length; i++)
            {
                if (((i + 1) % 2) != 0)
                {
                    if (t[i] == 'n')
                    {
                        text = text + "1";
                    }
                    else if (t[i] == 'w')
                    {
                        for (int j = 0; j < this.narrowratio; j++)
                        {
                            text = text + '1';
                        }
                    }
                }
                else if (t[i] == 'n')
                {
                    text = text + "0";
                }
                else if (t[i] == 'w')
                {
                    for (int k = 0; k < this.narrowratio; k++)
                    {
                        text = text + '0';
                    }
                }
            }
            return text;
        }

        private string ConvertISBN2EAN13(string str)
        {
            string text = "";
            str = this.TildeCodes(str);
            string text2 = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsNumber(str[i]) || (str[i] == ','))
                {
                    text2 = text2 + str[i];
                }
            }
            int startIndex = text2.IndexOf(',');
            if (startIndex >= 0)
            {
                text = text2.Substring(startIndex, text2.Length - startIndex);
                text2 = text2.Substring(0, startIndex);
            }
            int length = text2.Length;
            if (length > 9)
            {
                text2 = text2.Substring(0, 9);
            }
            else if (length < 9)
            {
                while (length++ < 9)
                {
                    text2 = text2 + '0';
                }
            }
            return ("978" + text2 + text);
        }

        public string Ean128Encoding(ref string str, bool checkDigit, out string checkStr, out string demoStr, out string drawText)
        {
            string text = "";
            str = this.TildeCodes(str);
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '(')
                {
                    text = text + '\x00c8';
                }
                else if (str[i] == ')')
                {
                    //text = text;
                }
                else
                {
                    text = text + str[i];
                }
            }
            return this.Code128Encoding(ref text, checkDigit, out checkStr, out demoStr, out drawText);
        }

        public string Ean13Encoding(ref string str, bool checkDigit, out string checkStr, out string demoStr, out string drawText)
        {
            int num;
            object[] objArray;
            string text = "";
            string text2 = "";
            string text3 = "";
            string text4 = "";
            int length = 12;
            checkStr = "";
            demoStr = "";
            int num2 = str.Length;
            string text5 = this.TildeCodes(str);
            string text6 = this.MaskFilter(text5, this.CHARSET_UEB);
            str = text6;
            int index = text6.IndexOf(',');
            if (index >= 0)
            {
                text2 = text6.Substring(index + 1, (text6.Length - index) - 1);
                text3 = this.UPC25SUPP(text2);
                text6 = text6.Substring(0, index);
            }
            num2 = text6.Length;
            if (num2 > length)
            {
                text6 = text6.Substring(0, length);
            }
            else if (num2 < length)
            {
                while (num2 < length)
                {
                    text6 = text6 + '0';
                    num2++;
                }
            }
            if (str.IndexOf(',') > 0)
            {
                str = text6 + ',' + text2;
            }
            else
            {
                str = text6;
            }
            text4 = text4 + this.uebStartGuards;
            object obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Ean 13 Left Guards:", this.uebStartGuards, '\x0080' });
            switch (text6[0])
            {
                case '0':
                    text = "AAAAAA";
                    break;

                case '1':
                    text = "AABABB";
                    break;

                case '2':
                    text = "AABBAB";
                    break;

                case '3':
                    text = "AABBBA";
                    break;

                case '4':
                    text = "ABAABB";
                    break;

                case '5':
                    text = "ABBAAB";
                    break;

                case '6':
                    text = "ABBBAA";
                    break;

                case '7':
                    text = "ABABAB";
                    break;

                case '8':
                    text = "ABABBA";
                    break;

                case '9':
                    text = "ABBABA";
                    break;
            }
            for (num = 1; num < 7; num++)
            {
                if (text[num - 1] == 'A')
                {
                    text4 = text4 + LefthandOdd[text6[num] - '0'];
                    obj2 = demoStr;
                    objArray = new object[] { obj2, text6[num].ToString(), ":", LefthandOdd[text6[num] - '0'], '\x0080' };
                    demoStr = string.Concat(objArray);
                }
                else if (text[num - 1] == 'B')
                {
                    text4 = text4 + LefthandEven[text6[num] - '0'];
                    obj2 = demoStr;
                    objArray = new object[] { obj2, text6[num].ToString(), ":", LefthandEven[text6[num] - '0'], '\x0080' };
                    demoStr = string.Concat(objArray);
                }
            }
            text4 = text4 + this.uebCenterGuards;
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Ean 13 Center Guards:", this.uebCenterGuards, '\x0080' });
            for (num = 7; num < 12; num++)
            {
                text4 = text4 + upc_code_c[text6[num] - '0'];
                obj2 = demoStr;
                objArray = new object[] { obj2, text6[num].ToString(), ":", upc_code_c[text6[num] - '0'], '\x0080' };
                demoStr = string.Concat(objArray);
            }
            char ch = this.getUPCCheck(text6);
            text4 = text4 + upc_code_c[ch - '0'] + this.uebStartGuards;
            checkStr = "" + ch;
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, ch, ":", upc_code_c[ch - '0'], '\x0080' });
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Ean 13 Right Guards:", this.uebStartGuards, '\x0080' });
            if (text3.Trim() != "")
            {
                text4 = text4 + ',' + text3;
                obj2 = demoStr;
                demoStr = string.Concat(new object[] { obj2, "SUPPLEMENTS:", text3, '\x0080' });
            }
            drawText = ((string) str) + ((string) checkStr);
            return text4;
        }

        public string Ean8Encoding(ref string str, bool checkDigit, out string checkStr, out string demoStr, out string drawText)
        {
            int num;
            object[] objArray;
            string text = "";
            string text2 = "";
            string text3 = "";
            int length = 7;
            checkStr = "";
            demoStr = "";
            int num2 = str.Length;
            string text4 = this.TildeCodes(str);
            string text5 = this.MaskFilter(text4, this.CHARSET_UEB);
            str = text5;
            int index = text5.IndexOf(',');
            if (index >= 0)
            {
                text = text5.Substring(index + 1, (text5.Length - index) - 1);
                text2 = this.UPC25SUPP(text);
                text5 = text5.Substring(0, index);
            }
            num2 = text5.Length;
            if (num2 > length)
            {
                text5 = text5.Substring(0, length);
            }
            else if (num2 < length)
            {
                while (num2 < length)
                {
                    text5 = text5 + '0';
                    num2++;
                }
            }
            if (str.IndexOf(',') > 0)
            {
                str = text5 + ',' + text;
            }
            else
            {
                str = text5;
            }
            text3 = text3 + this.uebStartGuards;
            object obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Ean 8 Left Guards:", this.uebStartGuards, '\x0080' });
            for (num = 0; num < 4; num++)
            {
                text3 = text3 + LefthandOdd[text5[num] - '0'];
                obj2 = demoStr;
                objArray = new object[] { obj2, text5[num].ToString(), ":", LefthandOdd[text5[num] - '0'], '\x0080' };
                demoStr = string.Concat(objArray);
            }
            text3 = text3 + this.uebCenterGuards;
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Ean 8 Center Guards:", this.uebCenterGuards, '\x0080' });
            for (num = 4; num < 7; num++)
            {
                text3 = text3 + upc_code_c[text5[num] - '0'];
                obj2 = demoStr;
                objArray = new object[] { obj2, text5[num].ToString(), ":", upc_code_c[text5[num] - '0'], '\x0080' };
                demoStr = string.Concat(objArray);
            }
            char ch = this.getUPCCheck(text5);
            text3 = text3 + upc_code_c[ch - '0'] + this.uebStartGuards;
            checkStr = "" + ch;
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, ch, ":", upc_code_c[ch - '0'], '\x0080' });
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Ean 8 Right Guards:", this.uebStartGuards, '\x0080' });
            if (text2.Trim() != "")
            {
                text3 = text3 + ',' + text2;
                obj2 = demoStr;
                demoStr = string.Concat(new object[] { obj2, "SUPPLEMENTS:", text2, '\x0080' });
            }
            drawText = ((string) str) + ((string) checkStr);
            return text3;
        }

        public string Encoding(int symbology, ref string str, bool checkDigit, out string checkStr, out string demoStr, out string drawText)
        {
            checkStr = "";
            demoStr = "";
            drawText = "";
            switch (symbology)
            {
                case 0:
                    return this.Code39Encoding(ref str, checkDigit, out checkStr, out demoStr, out drawText);

                case 1:
                    return this.Code39ExtendedEncoding(ref str, checkDigit, out checkStr, out demoStr, out drawText);

                case 2:
                    return this.Code93Encoding(ref str, checkDigit, out checkStr, out demoStr, out drawText);

                case 3:
                    return this.UpcaEncoding(ref str, checkDigit, out checkStr, out demoStr, out drawText);

                case 4:
                    return this.Ean13Encoding(ref str, checkDigit, out checkStr, out demoStr, out drawText);

                case 5:
                    return this.Ean8Encoding(ref str, checkDigit, out checkStr, out demoStr, out drawText);

                case 6:
                    return this.UpceEncoding(ref str, checkDigit, out checkStr, out demoStr, out drawText);

                case 7:
                    return this.BooklandEncoding(ref str, checkDigit, out checkStr, out demoStr, out drawText);

                case 8:
                    return this.Code128Encoding(ref str, checkDigit, out checkStr, out demoStr, out drawText);

                case 9:
                    return this.Ean128Encoding(ref str, checkDigit, out checkStr, out demoStr, out drawText);

                case 10:
                    return this.Code25Encoding(ref str, checkDigit, out checkStr, out demoStr, out drawText);

                case 11:
                    return this.Itf25Encoding(ref str, checkDigit, out checkStr, out demoStr, out drawText);

                case 12:
                    return this.Code11Encoding(ref str, checkDigit, out checkStr, out demoStr, out drawText);

                case 13:
                    return this.CodabarEncoding(ref str, checkDigit, out checkStr, out demoStr, out drawText);

                case 14:
                    return this.MSIEncoding(ref str, checkDigit, out checkStr, out demoStr, out drawText);

                case 15:
                    return this.PostnetEncoding(ref str, checkDigit, out checkStr, out demoStr, out drawText);
            }
            return null;
        }

        private string Get128CharSetA()
        {
            string text = "";
            for (int i = 0x20; i <= 0x5f; i++)
            {
                text = text + ((char) i);
            }
            for (int j = 0; j <= 0x1f; j++)
            {
                text = text + ((char) j);
            }
            return text;
        }

        private string Get128CharSetB()
        {
            string text = "";
            for (int i = 0x20; i <= 0x7f; i++)
            {
                text = text + ((char) i);
            }
            return text;
        }

        private string GetItf25String(int num)
        {
            string text3 = "";
            string text = Itf25MapTable[num / 10];
            string text2 = Itf25MapTable[num % 10];
            for (int i = 0; i < text.Length; i++)
            {
                object obj2 = text3;
                text3 = string.Concat(new object[] { obj2, "", text[i], text2[i] });
            }
            return text3;
        }

        private char getUPCCheck(string str)
        {
            int num = 0;
            int length = str.Length;
            for (int i = 0; i < length; i++)
            {
                if ((i % 2) == 0)
                {
                    num += 3 * (str[(length - i) - 1] - '0');
                }
                else
                {
                    num += str[(length - i) - 1] - '0';
                }
            }
            num = num % 10;
            if (num != 0)
            {
                num = 10 - num;
            }
            return (char) (0x30 + num);
        }

        public string Itf25Encoding(ref string str, bool checkDigit, out string checkStr, out string demoStr, out string drawText)
        {
            int length;
            object obj2;
            int num = 0;
            int num4 = 0;
            string text = this.TildeCodes(str);
            string text2 = this.MaskFilter(text, this.CHARSET_NUMERIC);
            if ((!checkDigit && ((text2.Length % 2) == 1)) || (checkDigit && ((text2.Length % 2) == 0)))
            {
                text2 = '0' + text2;
            }
            str = text2;
            string text3 = "";
            checkStr = "";
            demoStr = "";
            if (checkDigit)
            {
                char ch;
                length = text2.Length;
                for (num4 = 0; num4 < length; num4++)
                {
                    if ((num4 % 2) == 0)
                    {
                        num += 3 * (text2[(length - num4) - 1] - '0');
                    }
                    else
                    {
                        num += text2[(length - num4) - 1] - '0';
                    }
                }
                num = num % 10;
                if (num == 0)
                {
                    ch = '0';
                }
                else
                {
                    ch = (char) ((10 - num) + 0x30);
                }
                checkStr = "" + ch;
                text2 = text2 + ch;
            }
            length = text2.Length;
            for (num4 = 0; num4 < length; num4 += 2)
            {
                int num3 = ((10 * (text2[num4] - '0')) + text2[num4 + 1]) - 0x30;
                text3 = text3 + this.codeTobinary(this.GetItf25String(num3));
                obj2 = demoStr;
                demoStr = string.Concat(new object[] { obj2, num3, ":", this.codeTobinary(this.GetItf25String(num3)), '\x0080' });
            }
            text3 = this.I25Start + text3 + this.I25Stop;
            demoStr = string.Concat(new object[] { "Start:", this.I25Start, '\x0080', demoStr });
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Stop:", this.I25Stop, '\x0080' });
            drawText = ((string) str) + ((string) checkStr);
            return text3;
        }

        private string MaskFilter(string str, string charset)
        {
            string text = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (charset.IndexOf(str[i]) >= 0)
                {
                    text = text + str[i];
                }
            }
            return text;
        }

        public string MSIEncoding(ref string str, bool checkDigit, out string checkStr, out string demoStr, out string drawText)
        {
            object obj2;
            int num = 0;
            int num2 = 0;
            int num5 = 0;
            string text = this.TildeCodes(str);
            string text2 = this.MaskFilter(text, this.CHARSET_NUMERIC);
            string text3 = "";
            string text4 = "";
            str = text2;
            int length = text2.Length;
            int num4 = length % 2;
            demoStr = "";
            for (num = 0; num < length; num++)
            {
                text4 = text4 + MSIMapTable[text2[num] - '0'];
                obj2 = demoStr;
                demoStr = string.Concat(new object[] { obj2, text2[num], ":", MSIMapTable[text2[num] - '0'], '\x0080' });
                if ((num % 2) == num4)
                {
                    num2 = (num2 + text2[num]) - 0x30;
                }
                else
                {
                    text3 = text3 + text2[num];
                }
            }
            length = text3.Length;
            for (num = 0; num < length; num++)
            {
                num5 = ((10 * num5) + text3[num]) - 0x30;
            }
            num5 = 2 * num5;
            text3 = "";
            while (num5 > 0)
            {
                text3 = text3 + ((char) ((num5 % 10) + 0x30));
                num5 /= 10;
            }
            length = text3.Length;
            for (num = 0; num < length; num++)
            {
                num2 = (num2 + text3[num]) - 0x30;
            }
            num2 = num2 % 10;
            if (num2 != 0)
            {
                num2 = 10 - num2;
            }
            char ch = (char) (num2 + 0x30);
            checkStr = "" + ch;
            text4 = text4 + MSIMapTable[ch - '0'];
            text4 = this.MSIStart + text4 + this.MSIStop;
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Check Digit ", ch, ":", MSIMapTable[ch - '0'], '\x0080' });
            demoStr = string.Concat(new object[] { "Start :", this.MSIStart, '\x0080', demoStr });
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Stop :", this.MSIStop, '\x0080' });
            drawText = ((string) str) + ((string) checkStr);
            return text4;
        }

        private string PostnetEncoding(ref string str, bool checkDigit, out string checkStr, out string demoStr, out string drawText)
        {
            char ch;
            demoStr = "";
            drawText = str;
            int index = 0;
            string text = "";
            checkStr = "";
            string text2 = this.TildeCodes(str);
            string text3 = this.MaskFilter(text2, this.CHARSET_NUMERIC);
            int length = text3.Length;
            if ((length >= 0) && (length < 5))
            {
                while (text3.Length < 5)
                {
                    text3 = text3 + '0';
                }
            }
            else if ((length > 5) && (length < 9))
            {
                while (text3.Length < 9)
                {
                    text3 = text3 + '0';
                }
            }
            else if ((length > 9) && (length < 11))
            {
                while (text3.Length < 11)
                {
                    text3 = text3 + '0';
                }
            }
            else if (length > 11)
            {
                text3 = text3.Substring(0, 11);
            }
            length = text3.Length;
            for (int i = 0; i < length; i++)
            {
                ch = text3[i];
                index += ch - '0';
                text = text + PostnetMapTable[ch - '0'];
            }
            index = index % 10;
            if (index != 0)
            {
                index = 10 - index;
            }
            ch = (char) (index + 0x30);
            text = text + PostnetMapTable[index];
            text = '1' + text + '1';
            checkStr = checkStr + ch;
            return text;
        }

        protected string TildeCodes(string inpara)
        {
            string text = "";
            for (int i = 0; i < inpara.Length; i++)
            {
                if (inpara[i] == '~')
                {
                    int num = inpara[i + 1] - '@';
                    if ((num > 0) && (num < 0x1b))
                    {
                        text = text + ((char) num);
                        i++;
                    }
                    else if ((((inpara.Length - i) > 2) && char.IsNumber(inpara[i + 1])) && (char.IsNumber(inpara[i + 2]) && char.IsNumber(inpara[i + 3])))
                    {
                        num = (((100 * (inpara[i + 1] - '0')) + (10 * (inpara[i + 2] - '0'))) + inpara[i + 3]) - 0x30;
                        if (num < 0x100)
                        {
                            text = text + ((char) num);
                            i += 3;
                        }
                        else
                        {
                            text = text + inpara[i + 1];
                            i++;
                        }
                    }
                    else if (inpara[i + 1] == '1')
                    {
                        text = text + '\x00c8';
                        i++;
                    }
                    else if (inpara[i + 1] == '~')
                    {
                        text = text + '~';
                        i++;
                    }
                    else
                    {
                        text = text + '~';
                    }
                }
                else
                {
                    text = text + inpara[i];
                }
            }
            return text;
        }

        private string UPC25SUPP(string str)
        {
            string text2 = this.TildeCodes(str);
            string text3 = "";
            int length = text2.Length;
            switch (length)
            {
                case 0:
                    return "";

                case 1:
                    text2 = text2 + '0';
                    return this.UPC2SUPP(text2);

                case 2:
                    return this.UPC2SUPP(text2);

                case 3:
                    text2 = text2 + "00";
                    return this.UPC5SUPP(text2);

                case 4:
                    text2 = text2 + '0';
                    return this.UPC5SUPP(text2);

                case 5:
                    return this.UPC5SUPP(text2);
            }
            if (length > 5)
            {
                text2 = text2.Substring(0, 5);
                text3 = this.UPC5SUPP(text2);
            }
            return text3;
        }

        private string UPC2SUPP(string str)
        {
            int num = 0;
            string text = "";
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            for (num = 0; num < str.Length; num++)
            {
                num2 = ((10 * num2) + str[num]) - 0x30;
            }
            switch ((num2 % 4))
            {
                case 0:
                    num3 = 1;
                    num4 = 1;
                    break;

                case 1:
                    num3 = 1;
                    num4 = 0;
                    break;

                case 2:
                    num3 = 0;
                    num4 = 1;
                    break;

                case 3:
                    num3 = 0;
                    num4 = 0;
                    break;
            }
            return ((text + this.uebStartGuards + ((num3 == 1) ? LefthandOdd[str[0] - '0'] : LefthandEven[str[0] - '0'])) + this.uebSeparator + ((num4 == 1) ? LefthandOdd[str[1] - '0'] : LefthandEven[str[1] - '0']));
        }

        private string UPC5SUPP(string str)
        {
            string text = "";
            int num = 0;
            int num2 = 0;
            num2 = ((((3 * (str[0] - '0')) + (9 * (str[1] - '0'))) + (3 * (str[2] - '0'))) + (9 * (str[3] - '0'))) + (3 * (str[4] - '0'));
            string text2 = Parity5[num2 % 10];
            text = text + "1011";
            for (num = 0; num < 5; num++)
            {
                text = text + (((text2[num] - '0') == 1) ? LefthandOdd[str[num] - '0'] : LefthandEven[str[num] - '0']);
                if (num < 4)
                {
                    text = text + "01";
                }
            }
            return text;
        }

        public string UpcaEncoding(ref string str, bool checkDigit, out string checkStr, out string demoStr, out string drawText)
        {
            int num;
            object[] objArray;
            int length = 11;
            string text = "";
            string text2 = "";
            string text3 = "";
            checkStr = "";
            demoStr = "";
            int num2 = str.Length;
            string text4 = this.TildeCodes(str);
            string text5 = this.MaskFilter(text4, this.CHARSET_UEB);
            str = text5;
            int index = text5.IndexOf(',');
            if (index >= 0)
            {
                text = text5.Substring(index + 1, (text5.Length - index) - 1);
                text2 = this.UPC25SUPP(text);
                text5 = text5.Substring(0, index);
            }
            num2 = text5.Length;
            if (num2 > length)
            {
                text5 = text5.Substring(0, length);
            }
            else if (num2 < length)
            {
                while (num2 < length)
                {
                    text5 = text5 + '0';
                    num2++;
                }
            }
            if (str.IndexOf(',') > 0)
            {
                str = text5 + ',' + text;
            }
            else
            {
                str = text5;
            }
            text3 = text3 + this.uebStartGuards;
            object obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Upc A Left Guards:", this.uebStartGuards, '\x0080' });
            for (num = 0; num < 6; num++)
            {
                text3 = text3 + LefthandOdd[text5[num] - '0'];
                obj2 = demoStr;
                objArray = new object[] { obj2, text5[num].ToString(), ":", LefthandOdd[text5[num] - '0'], '\x0080' };
                demoStr = string.Concat(objArray);
            }
            text3 = text3 + this.uebCenterGuards;
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Upc A Center Guards:", this.uebCenterGuards, '\x0080' });
            for (num = 6; num < 11; num++)
            {
                text3 = text3 + upc_code_c[text5[num] - '0'];
                obj2 = demoStr;
                objArray = new object[] { obj2, text5[num].ToString(), ":", upc_code_c[text5[num] - '0'], '\x0080' };
                demoStr = string.Concat(objArray);
            }
            char ch = this.getUPCCheck(text5);
            text3 = text3 + upc_code_c[ch - '0'] + this.uebStartGuards;
            checkStr = "" + ch;
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, ch, ":", upc_code_c[ch - '0'], '\x0080' });
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Upc A Right Guards:", this.uebStartGuards, '\x0080' });
            if (text2.Trim() != "")
            {
                text3 = text3 +  ',' + text2;
                obj2 = demoStr;
                demoStr = string.Concat(new object[] { obj2, "SUPPLEMENTS:", text2, '\x0080' });
            }
            drawText = ((string) str) + ((string) checkStr);
            return text3;
        }

        private string upce2upca(string str)
        {
            string text = "";
            if ((str[0] != '0') || (str.Length != 7))
            {
                return "";
            }
            switch (str[6])
            {
                case '0':
                case '1':
                case '2':
                    return ((((text + str[0] + str[1]) + str[2] + str[6]) + "0000" + str[3]) + str[4] + str[5]);

                case '3':
                    if (((str[3] != '0') && (str[3] != '1')) && (str[3] != '2'))
                    {
                        return ((((text + str[0]) + str[1] + str[2]) +  str[3] + "00000") + str[4] + str[5]);
                    }
                    return null;

                case '4':
                    return ((((text + str[0]) + str[1] + str[2]) + str[3] + str[4]) + "00000" + str[5]);

                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return ((((text + str[0] + str[1]) + str[2] + str[3]) + str[4] + str[5]) + "0000" + str[6]);
            }
            return text;
        }

        public string UpceEncoding(ref string str, bool checkDigit, out string checkStr, out string demoStr, out string drawText)
        {
            string text = "";
            string text2 = "";
            string text3 = "";
            string text4 = "";
            int length = 6;
            checkStr = "";
            demoStr = "";
            int num2 = str.Length;
            string text5 = this.TildeCodes(str);
            string text6 = this.MaskFilter(text5, this.CHARSET_UEB);
            str = text6;
            int index = text6.IndexOf(',');
            if (index >= 0)
            {
                text2 = text6.Substring(index + 1, (text6.Length - index) - 1);
                text3 = this.UPC25SUPP(text2);
                text6 = text6.Substring(0, index);
            }
            num2 = text6.Length;
            if (num2 > length)
            {
                text6 = text6.Substring(0, length);
            }
            else if (num2 < length)
            {
                while (num2 < length)
                {
                    text6 = text6 + '0';
                    num2++;
                }
            }
            if (str.IndexOf(',') > 0)
            {
                str = text6 + ',' + text2;
            }
            else
            {
                str = text6;
            }
            text6 = '0' + text6;
            string text7 = this.upce2upca(text6);
            char ch = this.getUPCCheck(text7);
            checkStr = "" + ch;
            switch (ch)
            {
                case '0':
                    text = "BBBAAA";
                    break;

                case '1':
                    text = "BBABAA";
                    break;

                case '2':
                    text = "BBAABA";
                    break;

                case '3':
                    text = "BBAAAB";
                    break;

                case '4':
                    text = "BABBAA";
                    break;

                case '5':
                    text = "BAABBA";
                    break;

                case '6':
                    text = "BAAABB";
                    break;

                case '7':
                    text = "BABABA";
                    break;

                case '8':
                    text = "BABAAB";
                    break;

                case '9':
                    text = "BAABAB";
                    break;
            }
            text4 = text4 + this.uebStartGuards;
            object obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Upc E Left Guards:", this.uebStartGuards, '\x0080' });
            for (int i = 1; i < 7; i++)
            {
                object[] objArray;
                if (text[i - 1] == 'A')
                {
                    text4 = text4 + LefthandOdd[text6[i] - '0'];
                    obj2 = demoStr;
                    objArray = new object[] { obj2, text6[i].ToString(), ":", LefthandOdd[text6[i] - '0'], '\x0080' };
                    demoStr = string.Concat(objArray);
                }
                else if (text[i - 1] == 'B')
                {
                    text4 = text4 + LefthandEven[text6[i] - '0'];
                    obj2 = demoStr;
                    objArray = new object[] { obj2, text6[i].ToString(), ":", LefthandEven[text6[i] - '0'], '\x0080' };
                    demoStr = string.Concat(objArray);
                }
            }
            text4 = text4 + this.uebCenterGuards + '1';
            obj2 = demoStr;
            demoStr = string.Concat(new object[] { obj2, "Upc E Center Guards +a Trailing bar:", this.uebCenterGuards, '1', '\x0080' });
            if (text3.Trim() != "")
            {
                text4 = text4 +  ',' + text3;
                obj2 = demoStr;
                demoStr = string.Concat(new object[] { obj2, "SUPPLEMENTS:", text3, '\x0080' });
            }
            drawText = ((string) str) + ((string) checkStr);
            return text4;
        }
    }
}

