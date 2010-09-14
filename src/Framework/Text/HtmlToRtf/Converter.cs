using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using InfoControl.Text.HtmlToRtf;

namespace InfoControl
{
    public class HtmlToRtfConverter
    {
        #region Members

        private string _baseURL = "";
        private eBorderVisibility _borderVisibility = eBorderVisibility.Hidden;
        private int _createTraceFile = 0;
        private int _deleteImages = 0;
        private int _deleteTables = 0;
        private eEncoding _encoding = eEncoding.AutoSelect;
        private eFontFace _fontFace = eFontFace.f_Arial;
        private int _fontSize = 10;
        private string _htmlPath = "";
        private eImageCompatible _imageCompatible = eImageCompatible.image_Word;
        private eOutputTextFormat _outputTextFormat = eOutputTextFormat.Rtf;
        private ePageAlignment _pageAlignment = ePageAlignment.AlignLeft;
        private string _pageFooter = "";
        private string _pageHeader = "";
        private int _pageMarginBottom = 20;
        private int _pageMarginLeft = 0x19;
        private int _pageMarginRight = 20;
        private int _pageMarginTop = 20;
        private ePageNumbers _pageNumbers = ePageNumbers.PageNumDisable;
        private ePageAlignment _pageNumbersAlignH = ePageAlignment.AlignRight;
        private ePageAlignment _pageNumbersAlignV = ePageAlignment.AlignBottom;
        private ePageOrientation _pageOrientation = ePageOrientation.Portrait;
        private ePageSize _pageSize = ePageSize.Letter;
        private int _preserveAlignment = 1;
        private int _preserveBackgroundColor = 1;
        private int _preserveFontColor = 1;
        private int _preserveFontFace = 1;
        private int _preserveFontSize = 1;
        private int _preserveHR = 1;
        private int _preserveHttpImages = 1;
        private int _preserveHyperlinks = 1;
        private int _preserveImages = 1;
        private int _preserveNestedTables = 1;
        private int _preservePageBreaks = 1;
        private int _preserveTables = 1;
        private int _preserveTableWidth = 1;
        private eRtfLanguage _rtfLanguage = eRtfLanguage.l_English;
        private eRtfParts _rtfParts = eRtfParts.RtfCompletely;
        private string _serial = "";
        private int _tableCellPadding = 2;
        private string _traceFilePath = @"c:\htmltortf-trace.txt";
        private int ALIGN_CENTER = 1;
        private int ALIGN_RIGHT = 2;
        private string BR_STR_BULLETS = "\n\\line ";
        private int CLVMGF = 0x6f;
        private int CLVMRG = 0x309;
        private int CLVMRG2 = 0x30a;
        private int DEF_MARGIN = 720;
        private string DIV_STR = "\n\\par\\pard ";
        private string DIV_TBL_STR = "\n\\par\\pard\\intbl ";
        private int FONT_SIZE_MAX = 200;
        private float FONT_SIZE_MIN = 0.5f;
        private int HYPERLINK_SIZE = 0x7d0;
        private ByteClass imageFileName = new ByteClass();
        private ByteClass imageFolder = new ByteClass();
        private string imagePath;
        private ByteClass imageTempFolder = new ByteClass();
        private eImageType imageType = eImageType.Jpeg;
        private int imgH = -1;
        private int imgW = -1;
        private string LF = "\r\n";
        private int margb = 0;
        private int margl = 0;
        private int margr = 0;
        private int margt = 0;
        private int MAX_COLORS = 0x100;
        private int MAX_COLUMNS = 50;
        private int MAX_FONTS = 0x19;
        private int MAX_NEST = 0x10;
        private int MAX_STYLE_NAME_LENGTH = 0x29;        
        private int MAX_SYMBOLS_IN_COLUMN = 300;
        private int page_height = 0;
        private int PAGE_SIZE_A3_H = 0x5d06;
        private int PAGE_SIZE_A3_W = 0x41c7;
        private int PAGE_SIZE_A4_H = 0x41c7;
        private int PAGE_SIZE_A4_W = 0x2e83;
        private int PAGE_SIZE_A5_H = 0x2e83;
        private int PAGE_SIZE_A5_W = 0x20c8;
        private int PAGE_SIZE_B5_H = 0x375f;
        private int PAGE_SIZE_B5_W = 0x26fb;
        private int PAGE_SIZE_EXECUTIVE_H = 0x3b12;
        private int PAGE_SIZE_EXECUTIVE_W = 0x28c6;
        private int PAGE_SIZE_LEGAL_H = 0x4ec0;
        private int PAGE_SIZE_LEGAL_W = 0x2fd0;
        private int PAGE_SIZE_LETTER_H = 0x3de0;
        private int PAGE_SIZE_LETTER_W = 0x2fd0;
        private int PAGE_SIZE_MONARH_H = 0x2a31;
        private int PAGE_SIZE_MONARH_W = 0x15cb;
        private int page_width = 0;
        private int PX_TO_TWIPS = 0x10;
        private int SCREEN_W_DEF = 820;
        private int STK_MAX = 0x19;
        private int STYLES_KNOW = 0x13;
        private int TBLEN = 0x251c;
        private ByteClass tempS = new ByteClass();
        private int TRGAPH = 0x23;
        private int TRPADDB = 0;
        private int TRPADDL = 0;
        private int TRPADDR = 0;
        private int TRPADDT = 0;
        private int TWIPS_PER_ONE_SYMBOL = 0x7d;
        private int VALIGN_BOTTOM = 2;
        private int VALIGN_CENTER = 1;
        private int VALIGN_TOP = 0;
        #endregion

        public event BeforeImageDownloadEventHanfler BeforeImageDownload;

        public HtmlToRtfConverter()
        {
            
        }

        private void ByteToHexByteClass(byte b, ByteClass tempS)
        {
            tempS.Clear();
            tempS.Add((byte)((b / 0x10) + 0x30));
            if (tempS.b[0] > 0x39)
            {
                tempS.b[0] = (byte)((0x61 + tempS.b[0]) - 0x3a);
            }
            tempS.Add((byte)((b % 0x10) + 0x30));
            if (tempS.b[1] > 0x39)
            {
                tempS.b[1] = (byte)((0x61 + tempS.b[1]) - 0x3a);
            }
        }

        private bool check_position_object(ByteClass buf, int ct, int max_pos, CSS_tag_type css_tag_type)
        {
            bool flag = false;
            while ((buf.b[ct - 1] != 0x3e) && (ct < max_pos))
            {
                ct++;
            }
            while (ct < max_pos)
            {
                if ((((((css_tag_type == CSS_tag_type.SPAN_CSS) && (buf.b[ct] == 60)) && (buf.b[ct + 1] == 0x2f)) && ((buf.b[ct + 2] == 0x73) || (buf.b[ct + 2] == 0x53))) && ((buf.b[ct + 3] == 0x70) || (buf.b[ct + 3] == 80))) && ((((buf.b[ct + 4] == 0x61) || (buf.b[ct + 4] == 0x41)) && ((buf.b[ct + 5] == 110) || (buf.b[ct + 5] == 0x4e))) && (buf.b[ct + 6] == 0x3e)))
                {
                    return flag;
                }
                if ((((((css_tag_type == CSS_tag_type.DIV_CSS) && (buf.b[ct] == 60)) && (buf.b[ct + 1] == 0x2f)) && ((buf.b[ct + 2] == 100) || (buf.b[ct + 2] == 0x44))) && ((buf.b[ct + 3] == 0x69) || (buf.b[ct + 3] == 0x49))) && (((buf.b[ct + 4] == 0x76) || (buf.b[ct + 4] == 0x56)) && (buf.b[ct + 5] == 0x3e)))
                {
                    return flag;
                }
                if ((((((css_tag_type == CSS_tag_type.SPAN_CSS) && (buf.b[ct] == 60)) && ((buf.b[ct + 1] == 0x73) || (buf.b[ct + 1] == 0x53))) && ((buf.b[ct + 2] == 0x70) || (buf.b[ct + 2] == 80))) && (((buf.b[ct + 3] == 0x61) || (buf.b[ct + 3] == 0x41)) && ((buf.b[ct + 4] == 110) || (buf.b[ct + 4] == 0x4e)))) && ((buf.b[ct + 5] == 0x3e) || (buf.b[ct + 5] == 0x20)))
                {
                    while ((buf.b[ct] != 0x3e) && (ct < max_pos))
                    {
                        ct++;
                    }
                }
                else if ((((((css_tag_type == CSS_tag_type.DIV_CSS) && (buf.b[ct] == 60)) && ((buf.b[ct + 1] == 100) || (buf.b[ct + 1] == 0x44))) && ((buf.b[ct + 2] == 0x69) || (buf.b[ct + 2] == 0x49))) && ((buf.b[ct + 3] == 0x76) || (buf.b[ct + 3] == 0x56))) && ((buf.b[ct + 4] == 0x3e) || (buf.b[ct + 4] == 0x20)))
                {
                    while ((buf.b[ct] != 0x3e) && (ct < max_pos))
                    {
                        ct++;
                    }
                }
                else if ((((((css_tag_type == CSS_tag_type.SPAN_CSS) && (buf.b[ct] == 60)) && ((buf.b[ct + 1] == 0x73) || (buf.b[ct + 1] == 0x53))) && ((buf.b[ct + 2] == 0x70) || (buf.b[ct + 2] == 80))) && (((buf.b[ct + 3] == 0x61) || (buf.b[ct + 3] == 0x41)) && ((buf.b[ct + 4] == 110) || (buf.b[ct + 4] == 0x4e)))) && ((buf.b[ct + 5] == 0x3e) || (buf.b[ct + 5] == 0x20)))
                {
                    while ((buf.b[ct] != 0x3e) && (ct < max_pos))
                    {
                        ct++;
                    }
                }
                else
                {
                    if ((((buf.b[ct] == 60) && ((buf.b[ct + 1] == 0x69) || (buf.b[ct + 1] == 0x49))) && ((buf.b[ct + 2] == 0x6d) || (buf.b[ct + 2] == 0x4d))) && (((buf.b[ct + 3] == 0x67) || (buf.b[ct + 3] == 0x47)) && ((buf.b[ct + 4] == 0x20) || (buf.b[ct + 4] == 10))))
                    {
                        return true;
                    }
                    if (buf.b[ct] == 60)
                    {
                        while ((buf.b[ct] != 0x3e) && (ct < max_pos))
                        {
                            ct++;
                        }
                    }
                    else if (buf.b[ct] > 0x20)
                    {
                        return true;
                    }
                }
                ct++;
            }
            return flag;
        }

        private bool check_tr_close(ByteClass buf, int ct, int blocks)
        {
            int num = 0;
            bool flag = true;
            while ((ct + num) < buf.len)
            {
                if (((((buf.b[ct + num] == 60) && (buf.b[(ct + num) + 1] == 0x2f)) && ((buf.b[(ct + num) + 2] == 0x74) || (buf.b[(ct + num) + 2] == 0x54))) && ((buf.b[(ct + num) + 3] == 0x72) || (buf.b[(ct + num) + 3] == 0x52))) && (buf.b[(ct + num) + 4] == 0x3e))
                {
                    return false;
                }
                if ((((((buf.b[ct + num] == 60) && (buf.b[(ct + num) + 1] == 0x2f)) && ((buf.b[(ct + num) + 2] == 0x74) || (buf.b[(ct + num) + 2] == 0x54))) && ((buf.b[(ct + num) + 3] == 0x61) || (buf.b[(ct + num) + 3] == 0x41))) && (((buf.b[(ct + num) + 4] == 0x62) || (buf.b[(ct + num) + 4] == 0x42)) && ((buf.b[(ct + num) + 5] == 0x6c) || (buf.b[(ct + num) + 5] == 0x4c)))) && (((buf.b[(ct + num) + 6] == 0x65) || (buf.b[(ct + num) + 6] == 0x45)) && (buf.b[(ct + num) + 7] == 0x3e)))
                {
                    return flag;
                }
                num++;
            }
            return flag;
        }

        private bool check_tr_tag(ByteClass buf, int ct, int blocks)
        {
            int num = 0;
            bool flag = false;
            while ((ct + num) < buf.len)
            {
                if ((((buf.b[ct + num] == 60) && ((buf.b[(ct + num) + 1] == 0x74) || (buf.b[(ct + num) + 1] == 0x54))) && ((buf.b[(ct + num) + 2] == 0x72) || (buf.b[(ct + num) + 2] == 0x52))) && ((buf.b[(ct + num) + 3] == 0x3e) || this.IS_DELIMITER(buf.b[(ct + num) + 3])))
                {
                    return true;
                }
                if ((((((buf.b[ct + num] == 60) && (buf.b[(ct + num) + 1] == 0x2f)) && ((buf.b[(ct + num) + 2] == 0x74) || (buf.b[(ct + num) + 2] == 0x54))) && ((buf.b[(ct + num) + 3] == 0x61) || (buf.b[(ct + num) + 3] == 0x41))) && (((buf.b[(ct + num) + 4] == 0x62) || (buf.b[(ct + num) + 4] == 0x42)) && ((buf.b[(ct + num) + 5] == 0x6c) || (buf.b[(ct + num) + 5] == 0x4c)))) && (((buf.b[(ct + num) + 6] == 0x65) || (buf.b[(ct + num) + 6] == 0x45)) && (buf.b[(ct + num) + 7] == 0x3e)))
                {
                    return flag;
                }
                if ((((buf.b[ct + num] == 60) && ((buf.b[(ct + num) + 1] == 0x74) || (buf.b[(ct + num) + 1] == 0x54))) && (((buf.b[(ct + num) + 2] == 100) || (buf.b[(ct + num) + 2] == 0x44)) || ((buf.b[(ct + num) + 2] == 0x68) || (buf.b[(ct + num) + 2] == 0x48)))) && ((buf.b[(ct + num) + 3] == 0x3e) || this.IS_DELIMITER(buf.b[(ct + num) + 3])))
                {
                    return flag;
                }
                num++;
            }
            return flag;
        }

        private void CheckRemotePath(ByteClass path)
        {
            ByteClass class2 = new ByteClass();
            int index = 0;
            class2.Clear();
            for (index = 0; index < path.len; index++)
            {
                if (path.b[index] == 0x20)
                {
                    class2.Add("%20");
                }
                else
                {
                    class2.Add(path.b[index]);
                }
            }
            if ((class2.len > 0) && (class2.len != path.len))
            {
                path.Clear();
                for (index = 0; index < class2.len; index++)
                {
                    path.Add(class2.b[index]);
                }
            }
        }

        private void combine_img_path(ByteClass folder, ByteClass name)
        {
            int index = 0;
            int num2 = 0;
            int len = 0;
            int num4 = 0;
            int num5 = 0;
            ByteClass class2 = new ByteClass();
            if ((char.IsLetter((char)name.b[0]) && (name.b[1] == 0x3a)) && (name.b[2] == 0x5c))
            {
                folder.Clear();
                for (index = 0; index < name.len; index++)
                {
                    folder.Add(name.b[index]);
                }
            }
            else
            {
                for (index = 0; index < 9; index++)
                {
                    class2.Add(folder.b[index]);
                    if (folder.b[index] == 0x5c)
                    {
                        index++;
                        class2.b[index] = 0;
                        break;
                    }
                }
                len = class2.len;
                num4 = folder.len;
                num2 = name.len;
                index = 0;
                while (index < name.len)
                {
                    if ((name.b[index] != 0x2e) && (name.b[index] != 0x5c))
                    {
                        break;
                    }
                    if (((name.b[index] == 0x2e) && (name.b[index + 1] == 0x2e)) && (name.b[index + 2] == 0x5c))
                    {
                        num5++;
                    }
                    index++;
                }
                num2 = index;
                for (index = num4 - 2; (index >= (len - 1)) && (num5 > 0); index--)
                {
                    if (folder.b[index] == 0x5c)
                    {
                        num5--;
                    }
                    if (num5 == 0)
                    {
                        index++;
                        folder.b[index] = 0;
                        folder.len = index + 1;
                        break;
                    }
                }
                if (folder.len > len)
                {
                    while (num2 < name.len)
                    {
                        folder.Add(name.b[num2]);
                        num2++;
                    }
                }
                else
                {
                    folder.Clear();
                    for (index = 0; index < class2.len; index++)
                    {
                        folder.Add(class2.b[index]);
                    }
                    while (num2 < name.len)
                    {
                        folder.Add(name.b[num2]);
                        num2++;
                    }
                }
            }
        }

        private void CombinePaths(string folder, ByteClass file)
        {
            ByteClass class2 = new ByteClass();
            string str = file.ByteToString();
            if ((((str.Length <= 6) || ((str[0] != 'h') && (str[0] != 'H'))) || ((str[1] != 't') && (str[1] != 'T'))) || ((((str[2] != 't') && (str[2] != 'T')) || ((str[3] != 'p') && (str[3] != 'P'))) || (((str[4] != ':') || (str[5] != '/')) || (str[6] != '/'))))
            {
                class2.Add(folder);
                this.combine_img_path(class2, file);
                file.Clear();
                file.Add(class2);
            }
        }

        private string CombineURLs(string baseURL, string pathURL)
        {
            string str = "";
            int num = 0;
            int num2 = 0;
            int startIndex = 0;
            int num4 = 0;
            int num5 = 0;
            if (baseURL.Length > 8)
            {
                for (startIndex = 7; startIndex < baseURL.Length; startIndex++)
                {
                    if (baseURL[startIndex] == '/')
                    {
                        num5 = startIndex;
                        break;
                    }
                }
                startIndex = baseURL.Length - 1;
                num4 = num5;
                while (startIndex > num5)
                {
                    if (baseURL[startIndex] == '.')
                    {
                        num4 = startIndex;
                        break;
                    }
                    startIndex--;
                }
                for (startIndex = baseURL.Length - 1; startIndex > num4; startIndex--)
                {
                    if (baseURL[startIndex] == '/')
                    {
                        num++;
                    }
                }
                if (pathURL.Length > 0)
                {
                    if (pathURL[0] == '/')
                    {
                        num2 = num;
                    }
                    else if (pathURL[0] == '.')
                    {
                        startIndex = 0;
                        while ((startIndex + 3) < pathURL.Length)
                        {
                            if (((pathURL[startIndex] != '.') || (pathURL[startIndex + 1] != '.')) || (pathURL[startIndex + 2] != '/'))
                            {
                                break;
                            }
                            pathURL = pathURL.Remove(0, 3);
                            startIndex = 0;
                            num2++;
                        }
                    }
                    else
                    {
                        num2 = 0;
                    }
                }
                str = baseURL;
                num = 0;
                for (startIndex = baseURL.Length - 2; startIndex >= num4; startIndex--)
                {
                    if (baseURL[startIndex] == '/')
                    {
                        num++;
                        str = baseURL.Remove(startIndex, baseURL.Length - startIndex);
                    }
                    if (num >= num2)
                    {
                        break;
                    }
                }
                str = str + "/" + pathURL;
                for (startIndex = num4; startIndex < (str.Length - 1); startIndex++)
                {
                    if ((str[startIndex] == '/') && (str[startIndex + 1] == '/'))
                    {
                        str = str.Remove(startIndex, 1);
                    }
                }
            }
            return str;
        }

        private static string ConvertEncoding(string value, System.Text.Encoding src, System.Text.Encoding trg)
        {
            System.Text.Decoder decoder = src.GetDecoder();
            byte[] bytes = trg.GetBytes(value);
            char[] chars = new char[decoder.GetCharCount(bytes, 0, bytes.Length)];
            decoder.GetChars(bytes, 0, bytes.Length, chars, 0);
            return new string(chars);
        }

        public int ConvertFile(string htmlFile, string outFolder)
        {
            ByteClass path = new ByteClass();
            ByteClass class3 = new ByteClass();
            ByteClass file = new ByteClass();
            string htmlString = "";
            string str2 = "";
            bool flag = false;
            if ((((htmlFile[0] == 'h') || (htmlFile[0] == 'H')) && ((htmlFile[1] == 't') || (htmlFile[1] == 'T'))) && ((((htmlFile[2] == 't') || (htmlFile[2] == 'T')) && ((htmlFile[3] == 'p') || (htmlFile[3] == 'P'))) && (((htmlFile[4] == ':') && (htmlFile[5] == '/')) && (htmlFile[6] == '/'))))
            {
                htmlString = this.DownloadFile(htmlFile);
            }
            if (htmlString.Length > 0)
            {
                flag = true;
            }
            if (!flag && !File.Exists(htmlFile))
            {
                return 1;
            }
            if (!flag)
            {
                FileStream stream = new FileStream(htmlFile, FileMode.Open);
                ByteClass class5 = new ByteClass((int)stream.Length);
                try
                {
                    class5.Clear();
                    class5.len = stream.Read(class5.b, 0, (int)stream.Length);
                    stream.Close();
                    htmlString = class5.ByteToString();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            bool flag2 = false;
            bool flag3 = false;
            for (int i = outFolder.Length - 1; i > 0; i--)
            {
                if (outFolder[i] == '.')
                {
                    flag3 = true;
                }
                if ((outFolder[i] == '\\') || (outFolder[i] == '/'))
                {
                    if (flag3)
                    {
                        flag2 = true;
                    }
                    break;
                }
            }
            if (File.Exists(outFolder))
            {
                if ((File.GetAttributes(outFolder) & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    path.Clear();
                    path.Add(outFolder);
                    if ((path.b[path.len - 1] != 0x5c) || (path.b[path.len - 1] != 0x2f))
                    {
                        path.Add(@"\");
                    }
                }
                else
                {
                    path.Clear();
                    path.Add(outFolder);
                    if (!flag2)
                    {
                        this.seldirectory(path);
                    }
                }
            }
            else
            {
                try
                {
                    if ((File.GetAttributes(outFolder) & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        path.Clear();
                        path.Add(outFolder);
                        if ((path.b[path.len - 1] != 0x5c) && (path.b[path.len - 1] != 0x2f))
                        {
                            path.Add(@"\");
                        }
                    }
                    else
                    {
                        path.Clear();
                        path.Add(outFolder);
                        this.seldirectory(path);
                    }
                }
                catch (Exception)
                {
                    path.Clear();
                    path.Add(outFolder);
                }
            }
            class3.Add(htmlFile);
            this.selfile(class3, file);
            class3.Clear();
            class3.Add(htmlFile);
            this.seldirectory(class3);
            this._htmlPath = System.Text.Encoding.UTF8.GetString(class3.b, 0, class3.len).ToString();
            ByteClass class6 = new ByteClass();
            class6.Add(file);
            while (class6.len > 0)
            {
                class6.len--;
                if (class6.b[class6.len] == 0x2e)
                {
                    break;
                }
            }
            if (this._outputTextFormat == eOutputTextFormat.Doc)
            {
                class6.Add(".doc");
            }
            else if (this._outputTextFormat == eOutputTextFormat.Text)
            {
                class6.Add(".txt");
            }
            else if (this._outputTextFormat == eOutputTextFormat.Html)
            {
                class6.Add(".htm");
            }
            else
            {
                class6.Add(".rtf");
            }
            string str4 = System.Text.Encoding.UTF8.GetString(class6.b, 0, class6.len);
            string str5 = System.Text.Encoding.UTF8.GetString(path.b, 0, path.len);
            if (!flag2)
            {
                str5 = str5 + str4;
            }
            try
            {
                str2 = this.ConvertString(htmlString);
                StreamWriter writer = new StreamWriter(str5, false);
                writer.Write(str2);
                writer.Close();
            }
            catch (Exception)
            {
                return 3;
            }
            return 0;
        }

        public string ConvertFileToString(string htmlFile)
        {
            string str = "";
            string htmlString = "";
            if (!File.Exists(htmlFile))
            {
                return "";
            }
            FileStream stream = new FileStream(htmlFile, FileMode.Open);
            ByteClass class2 = new ByteClass((int)stream.Length);
            try
            {
                class2.Clear();
                class2.len = stream.Read(class2.b, 0, (int)stream.Length);
                stream.Close();
                htmlString = class2.ByteToString();
                if (htmlString.Length > 0)
                {
                    str = this.ConvertString(htmlString);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(exception.Message);
            }
            return str;
        }

        public string ConvertString(string htmlString)
        {
            return this.ConvertStringPrivate(htmlString, null);
        }

        public string ConvertString(string htmlString, ArrayList ImageList)
        {
            return this.ConvertStringPrivate(htmlString, ImageList);
        }

        private string ConvertStringPrivate(string htmlString, ArrayList ImageList)
        {
            
            int num4;
            int num5;
            int num38;
            if ((this._tableCellPadding > -1) && (this._tableCellPadding < 11))
            {
                this.TRPADDR = this._tableCellPadding * 20;
                this.TRPADDL = this._tableCellPadding * 20;
                this.TRPADDB = this._tableCellPadding * 20;
                this.TRPADDT = this._tableCellPadding * 20;
            }
            string s = this.LF + @"\trowd\trgaph" + this.TRGAPH.ToString() + @"\trautofit1\trpaddfr3\trpaddfl3\trpaddft3\trpaddfb3\trpaddr" + this.TRPADDR.ToString() + @"\trpaddl" + this.TRPADDL.ToString() + @"\trpaddb" + this.TRPADDB.ToString() + @"\trpaddt" + this.TRPADDT.ToString() + " ";
            string str2 = this.LF + @"{\*\nesttableprops\trowd\trgaph" + this.TRGAPH.ToString() + @"\trautofit1\trpaddfr3\trpaddfl3\trpaddft3\trpaddfb3\trpaddr" + this.TRPADDR.ToString() + @"\trpaddl" + this.TRPADDL.ToString() + @"\trpaddb" + this.TRPADDB.ToString() + @"\trpaddt" + this.TRPADDT.ToString() + " ";
            string str3 = @"{\pict\pngblip\picw420\pich375\picwgoal420\pichgoal375\picscalex100\picscaley100" + this.LF + "89504e470d0a1a0a0000000d494844520000001c000000190803000000151ebe15000000017352474200aece1ce90000000467414d410000b18f0bfc6105000000206348524d00007a26000080840000fa00000080e8000075300000ea6000003a98000017709cba513c00000060504c5445000000fbf9eedfddd1cccccc7a786d202020f2f0e5171718333333faf8ede6e6e6f4f2e7848276d7d5ce141414ffffffdfdfdf7b7a70272727f7f7f73b3b3cf0f0efd6d7d7e4e2d6858378f7efef807e74000000000000000000000000000000d2cf2ddc00000083494441542853c5925d0fc2200c45afa263b6dbcac78a8bffff872a6862228d6fcbce232797de14207fc02132d3979f0290f9123f4cf74eba48da20e55e3e723ba3556f86d46ae90a32e414d62094e0ade41690ca085fc498b99187c7524406ab50599ab364cce292ab212ba9afaeedc25ece67663ef150e966be576072cc63eff2fb9e550c27c74d708d7c0000000049454e44ae426082}" + this.LF;
            string str4 = @"{\pict\pngblip\picw420\pich375\picwgoal420\pichgoal375\picscalex100\picscaley100" + this.LF + "89504e470d0a1a0a0000000d494844520000001c000000190803000000151ebe15000000017352474200aece1ce90000000467414d410000b18f0bfc6105000000206348524d00007a26000080840000fa00000080e8000075300000ea6000003a98000017709cba513c00000030504c54457a786dffffffe5e5e4ccccccf2f0e5f5f4f2d7d5cef4f2e77b7a70dfddd1807e74f0f0eff7f7f8e4e2d684827600000079b7580a000000494944415428536360c40318064492950901d01cc0c0c8c6c001057cbc1892cc1c4cdc50c08e29c9c5031363c122c93ac82439c875101b273b0b0c60f8131e0498313b30914d93d4070094c8044f2aaddf8c0000000049454e44ae426082}" + this.LF;
            string str5 = @"{\pict\pngblip\picw420\pich375\picwgoal420\pichgoal375\picscalex100\picscaley100" + this.LF + "89504e470d0a1a0a0000000d494844520000001c000000190803000000151ebe15000000017352474200aece1ce90000000467414d410000b18f0bfc6105000000206348524d00007a26000080840000fa00000080e8000075300000ea6000003a98000017709cba513c00000018504c5445000000fffffff1efe2aca899716f64000000000000000000d5d8b27300000053494441542853c59131120030040489fcffcd3934194491c6150a6b624d889bd01c14246f3721918d64ac104847bcde71e89d554063a45391a2a790903ff87cb615625966e2359e62202368ce7d59bd7942e8006d6903093c53bdc70000000049454e44ae42608200000000000000}" + this.LF;
            string str6 = @"{\pict\pngblip\picw420\pich375\picwgoal420\pichgoal375\picscalex100\picscaley100" + this.LF + "89504e470d0a1a0a0000000d494844520000001c000000190803000000151ebe15000000017352474200aece1ce90000000467414d410000b18f0bfc6105000000206348524d00007a26000080840000fa00000080e8000075300000ea6000003a98000017709cba513c0000000c504c5445716f64fffffff1efe2aca8995b47d6600000004d494441542853c5d14b0a00200804503ff7bf7393431029ee42172e7a852389362573e8a83c3d02b90b2af346d0bec27e17912756e0b1ac78f409db40ea1639d9df55023261f3b92fab274f045a17b502ddf2d9efa10000000049454e44ae42608200000000000000000000000000000000000000000000000000}" + this.LF;
         
            ByteClass message = new ByteClass();
            ByteClass buf = new ByteClass();
            ByteClass class4 = new ByteClass();
            string str8 = "";
            this.EncodingToASCII(htmlString, buf);
            if (this._outputTextFormat == eOutputTextFormat.Html)
            {
                return this.HtmlToHtml(buf).ByteToString();
            }
            int index = 0;
            int num3 = 0;
            int num7 = 0;
            int num8 = 0;
            int num9 = 0;
            int num10 = 0;
            int num11 = 0;
            int num12 = 0;
            ByteClass str = new ByteClass(0x80);
            str.Add(@"\'");
            ByteClass class6 = new ByteClass();
            class6.Clear();
            class6.Add(this.GetFontByEnum(this._fontFace));
            ByteClass class7 = new ByteClass();
            ByteClass class8 = new ByteClass();
            ByteClass newb = new ByteClass();
            ByteClass class10 = new ByteClass();
            ByteClass class11 = new ByteClass();
            ArrayList list = new ArrayList();
            ArrayList list2 = new ArrayList();
            int num13 = 0;
            int num14 = 0;
            bool flag = false;
            bool flag2 = false;
            bool flag3 = true;
            bool flag4 = true;
            bool flag5 = true;
            int[] numArray = new int[3];
            int[] numArray2 = new int[] { 6, 8, 10, 12, 14, 0x12, 0x18, 0x24 };
            int num15 = 0;
            int num16 = 3;
            int fontsize = this._fontSize;
            int num18 = this._fontSize;
            bool flag6 = false;
            bool flag7 = false;
            ByteClass class12 = new ByteClass(0x20);
            int num19 = 0;
            int num20 = 1;
            int num21 = 1;
            int num22 = 1;
            ByteClass class13 = new ByteClass();
            ByteClass class14 = new ByteClass();
            int[] numArray3 = new int[this.STK_MAX];
            ByteClass class15 = new ByteClass();
            bool flag8 = false;
            ByteClass class16 = new ByteClass();
            ByteClass class17 = new ByteClass();
            ByteClass class18 = new ByteClass();
            class18.Add(@"\qj ");
            ByteClass class19 = new ByteClass();
            class19.Add(@"\qc ");
            ByteClass class20 = new ByteClass();
            class20.Add(@"\ql ");
            ByteClass class21 = new ByteClass();
            class21.Add(@"\qr ");
            ByteClass class22 = new ByteClass();
            class22.Add(@"\ql ");
            byte num23 = 0x6a;
            if (this._pageAlignment == ePageAlignment.AlignLeft)
            {
                num23 = 0x6c;
            }
            else if (this._pageAlignment == ePageAlignment.AlignRight)
            {
                num23 = 0x72;
            }
            else if (this._pageAlignment == ePageAlignment.AlignCenter)
            {
                num23 = 0x63;
            }
            else if (this._pageAlignment == ePageAlignment.AlignJustify)
            {
                num23 = 0x6a;
            }
            class22.b[2] = num23;
            ByteClass align = new ByteClass();
            align.Add(class22);
            int len = align.len;
            int num25 = 100;
            int num26 = 100;
            byte num27 = 0;
            string str9 = @"\li450 ";
            bool flag9 = false;
            int num28 = 0;
            int num29 = 0;
            int num30 = 0;
            int num31 = 0;
            int num32 = 0;
            int num33 = 0;
            bool flag10 = false;
            bool flag11 = false;
            int num34 = 0;
            bool flag12 = false;
            bool flag13 = false;
            string str10 = "";
            bool flag14 = false;
            bool flag15 = false;
            ByteClass class24 = new ByteClass();
            bool flag16 = false;
            bool flag17 = false;
            if (this._borderVisibility == eBorderVisibility.Visible)
            {
                flag17 = true;
            }
            CSS_params _params = new CSS_params();
            _params.CSS_reset(_params);
            bool flag18 = false;
            nest_tables[] _tablesArray = new nest_tables[this.MAX_NEST];
            for (int i = 0; i < this.MAX_NEST; i++)
            {
                _tablesArray[i] = new nest_tables();
            }
            int num36 = 0;
            string str12 = "";
            int tBLEN = this.TBLEN;
            bool flag19 = false;
            ByteClass class25 = new ByteClass();
            int num39 = 0;
            int num40 = 0;
            int num41 = 0;
            int num42 = 0;
            ByteClass class26 = new ByteClass();
            ByteClass folder = new ByteClass();
            ByteClass class28 = new ByteClass();
            ByteClass valueStr = new ByteClass();
            string path = "";
            class28.Clear();
            class28.Add(this._htmlPath);
            ByteClass class30 = new ByteClass(8);
            int charset = 0;
            int num44 = 0x4e4;
            ByteClass class31 = new ByteClass(8);
            class31.Add("fswiss");
            bool hieroglyph = false;
            int num45 = 0x4e4;
            int lang = 0x409;
            this.detect_lang(this._rtfLanguage, ref lang, ref hieroglyph, ref num45, ref charset);
            this.margl = (int)(this._pageMarginLeft * 56.7);
            this.margr = (int)(this._pageMarginRight * 56.7);
            this.margt = (int)(this._pageMarginTop * 56.7);
            this.margb = (int)(this._pageMarginBottom * 56.7);
            this.page_width = this.PAGE_SIZE_LETTER_W;
            this.page_height = this.PAGE_SIZE_LETTER_H;
            if (this._pageSize == ePageSize.A3)
            {
                this.page_width = this.PAGE_SIZE_A3_W;
                this.page_height = this.PAGE_SIZE_A3_H;
            }
            else if (this._pageSize == ePageSize.A5)
            {
                this.page_width = this.PAGE_SIZE_A5_W;
                this.page_height = this.PAGE_SIZE_A5_H;
            }
            else if (this._pageSize == ePageSize.A4)
            {
                this.page_width = this.PAGE_SIZE_A4_W;
                this.page_height = this.PAGE_SIZE_A4_H;
            }
            else if (this._pageSize == ePageSize.B5)
            {
                this.page_width = this.PAGE_SIZE_B5_W;
                this.page_height = this.PAGE_SIZE_B5_W;
            }
            else if (this._pageSize == ePageSize.Letter)
            {
                this.page_width = this.PAGE_SIZE_LETTER_W;
                this.page_height = this.PAGE_SIZE_LETTER_H;
            }
            else if (this._pageSize == ePageSize.Legal)
            {
                this.page_width = this.PAGE_SIZE_LEGAL_W;
                this.page_height = this.PAGE_SIZE_LEGAL_H;
            }
            else if (this._pageSize == ePageSize.Executive)
            {
                this.page_width = this.PAGE_SIZE_EXECUTIVE_W;
                this.page_height = this.PAGE_SIZE_EXECUTIVE_H;
            }
            else if (this._pageSize == ePageSize.Monarh)
            {
                this.page_width = this.PAGE_SIZE_MONARH_W;
                this.page_height = this.PAGE_SIZE_MONARH_H;
            }
            if ((this.margb < 0) || (this.margb > (this.page_height / 2)))
            {
                this.margb = this.DEF_MARGIN * 2;
            }
            if ((this.margt < 0) || (this.margt > (this.page_height / 2)))
            {
                this.margt = this.DEF_MARGIN;
            }
            if ((this.margl < 0) || (this.margl > (this.page_width / 2)))
            {
                this.margl = this.DEF_MARGIN;
            }
            if ((this.margr < 0) || (this.margr > (this.page_width / 2)))
            {
                this.margr = this.DEF_MARGIN;
            }
            int max = buf.len;
            if (max <= 0)
            {
                return "";
            }
            int num = 0;
            num10 = 0;
            while (num < max)
            {
                if (buf.b[num] == 0)
                {
                    num10++;
                }
                num++;
            }
            if ((num10 * 3) > max)
            {
                class4.Clear();
                for (num = 0; num < max; num++)
                {
                    if ((buf.b[num] != 0) && (buf.b[num + 1] != 0))
                    {
                        this.ToSpecSymbol(class24, buf.b[num + 1], buf.b[num]);
                        class4.Add(class24);
                        num++;
                    }
                    else if (buf.b[num] != 0)
                    {
                        class4.Add(buf.b[num]);
                    }
                }
                max = max = num10;
                for (num = 0; num < max; num++)
                {
                    buf.b[num] = class4.b[num];
                }
            }
            if (this._encoding != eEncoding.AutoSelect)
            {
                if (this._encoding == eEncoding.Windows_1250)
                {
                    num44 = 0x4e3;
                    charset = 0xee;
                    class31.Add("fswiss");
                }
                else if (this._encoding == eEncoding.Windows_1251)
                {
                    num44 = 0x4e3;
                    charset = 0xcc;
                    class31.Add("fnil");
                }
                else if (this._encoding == eEncoding.Windows_1252)
                {
                    num44 = 0x4e3;
                    charset = 0;
                    class31.Add("fswiss");
                }
                else if (this._encoding == eEncoding.Windows_1253)
                {
                    num44 = 0x4e3;
                    charset = 0xa1;
                    class31.Add("fswiss");
                }
                else if (this._encoding == eEncoding.Windows_1254)
                {
                    num44 = 0x4e6;
                    charset = 0xa2;
                    class31.Add("fswiss");
                }
                else if (this._encoding == eEncoding.Windows_1255)
                {
                    num44 = 0x4e3;
                    charset = 0xb1;
                    class31.Add("fswiss");
                }
                else if (this._encoding == eEncoding.Windows_1256)
                {
                    num44 = 0x4e8;
                    charset = 0xb2;
                    class31.Add("fswiss");
                }
                else if (this._encoding == eEncoding.Windows_1257)
                {
                    num44 = 0x4e3;
                    charset = 0xba;
                    class31.Add("fswiss");
                }
                else if (this._encoding == eEncoding.Windows_1258)
                {
                    num44 = 0x4e3;
                    charset = 0;
                    class31.Add("fswiss");
                }
                else if (this._encoding == eEncoding.KOI8_R)
                {
                    num44 = 0x4e3;
                    charset = 0xcc;
                    class31.Add("fnil");
                }
                else if (this._encoding == eEncoding.ISO_8859_5)
                {
                    num44 = 0x4e3;
                    charset = 0xcc;
                    class31.Add("fnil");
                }
                else if (this._encoding == eEncoding.UTF_8)
                {
                    num44 = 0x4e4;
                    class31.Add("fswiss");
                }
                else
                {
                    num44 = 0x4e4;
                    charset = 0;
                    class31.Add("fswiss");
                }
            }
            else
            {
                num = 0;
                while (num < max)
                {
                    if ((((buf.b[num] == 60) && ((buf.b[num + 1] == 0x68) || (buf.b[num + 1] == 0x48))) && (((buf.b[num + 2] == 0x65) || (buf.b[num + 2] == 0x45)) && ((buf.b[num + 3] == 0x61) || (buf.b[num + 3] == 0x41)))) && (((buf.b[num + 4] == 100) || (buf.b[num + 4] == 0x44)) && ((buf.b[num + 5] == 0x3e) || this.IS_DELIMITER(buf.b[num + 5]))))
                    {
                        num += 5;
                        while ((buf.b[num] != 0x3e) && (num < max))
                        {
                            num++;
                        }
                        break;
                    }
                    num++;
                }
                while (num < max)
                {
                    if (((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x68) || (buf.b[num + 2] == 0x48))) && (((buf.b[num + 3] == 0x65) || (buf.b[num + 3] == 0x45)) && ((buf.b[num + 4] == 0x61) || (buf.b[num + 4] == 0x41)))) && (((buf.b[num + 5] == 100) || (buf.b[num + 5] == 0x44)) && (buf.b[num + 6] == 0x3e)))
                    {
                        break;
                    }
                    if (((((buf.b[num] == 0x63) || (buf.b[num] == 0x43)) && ((buf.b[num + 1] == 0x68) || (buf.b[num + 1] == 0x48))) && (((buf.b[num + 2] == 0x61) || (buf.b[num + 2] == 0x41)) && ((buf.b[num + 3] == 0x72) || (buf.b[num + 3] == 0x52)))) && ((((buf.b[num + 4] == 0x73) || (buf.b[num + 4] == 0x53)) && ((buf.b[num + 5] == 0x65) || (buf.b[num + 5] == 0x45))) && (((buf.b[num + 6] == 0x74) || (buf.b[num + 6] == 0x54)) && (buf.b[num + 7] == 0x3d))))
                    {
                        while (num < max)
                        {
                            if (((buf.b[num] == 0x31) && (buf.b[num + 1] == 50)) && ((buf.b[num + 2] == 0x35) && (buf.b[num + 3] == 0x30)))
                            {
                                charset = 0xee;
                                num44 = 0x4e3;
                                class31.Clear();
                                class31.Add("fswiss");
                            }
                            if (((buf.b[num] == 0x31) && (buf.b[num + 1] == 50)) && ((buf.b[num + 2] == 0x35) && (buf.b[num + 3] == 0x31)))
                            {
                                charset = 0xcc;
                                num44 = 0x4e3;
                                class31.Clear();
                                class31.Add("fnil");
                            }
                            if (((buf.b[num] == 0x31) && (buf.b[num + 1] == 50)) && ((buf.b[num + 2] == 0x35) && (buf.b[num + 3] == 50)))
                            {
                                charset = 0;
                                num44 = 0x4e3;
                                class31.Clear();
                                class31.Add("fswiss");
                            }
                            if (((buf.b[num] == 0x31) && (buf.b[num + 1] == 50)) && ((buf.b[num + 2] == 0x35) && (buf.b[num + 3] == 0x33)))
                            {
                                charset = 0xa1;
                                num44 = 0x4e3;
                                class31.Clear();
                                class31.Add("fswiss");
                            }
                            if (((buf.b[num] == 0x31) && (buf.b[num + 1] == 50)) && ((buf.b[num + 2] == 0x35) && (buf.b[num + 3] == 0x34)))
                            {
                                num44 = 0x4e6;
                                charset = 0xa2;
                                class31.Clear();
                                class31.Add("fswiss");
                            }
                            if (((buf.b[num] == 0x31) && (buf.b[num + 1] == 50)) && ((buf.b[num + 2] == 0x35) && (buf.b[num + 3] == 0x35)))
                            {
                                charset = 0xb1;
                                num44 = 0x4e3;
                                class31.Clear();
                                class31.Add("fswiss");
                            }
                            if (((buf.b[num] == 0x31) && (buf.b[num + 1] == 50)) && ((buf.b[num + 2] == 0x35) && (buf.b[num + 3] == 0x36)))
                            {
                                num44 = 0x4e8;
                                charset = 0xb2;
                                class31.Clear();
                                class31.Add("fswiss");
                            }
                            if (((buf.b[num] == 0x31) && (buf.b[num + 1] == 50)) && ((buf.b[num + 2] == 0x35) && (buf.b[num + 3] == 0x37)))
                            {
                                charset = 0xba;
                                num44 = 0x4e3;
                                class31.Clear();
                                class31.Add("fswiss");
                            }
                            if (((buf.b[num] == 0x31) && (buf.b[num + 1] == 50)) && ((buf.b[num + 2] == 0x35) && (buf.b[num + 3] == 0x38)))
                            {
                                charset = 0;
                                num44 = 0x4e3;
                                class31.Clear();
                                class31.Add("fswiss");
                            }
                            if ((((buf.b[num] == 0x6b) || (buf.b[num] == 0x4b)) && ((buf.b[num + 1] == 0x6f) || (buf.b[num + 1] == 0x4f))) && (((buf.b[num + 2] == 0x69) || (buf.b[num + 2] == 0x49)) && (buf.b[num + 3] == 0x38)))
                            {
                                charset = 0xcc;
                                num44 = 0x4e3;
                                class31.Clear();
                                class31.Add("fnil");
                            }
                            if ((((buf.b[num] == 0x38) && (buf.b[num + 1] == 0x38)) && ((buf.b[num + 2] == 0x35) && (buf.b[num + 3] == 0x39))) && ((buf.b[num + 4] == 0x2d) && (buf.b[num + 5] == 0x35)))
                            {
                                charset = 0xcc;
                                num44 = 0x4e3;
                                class31.Clear();
                                class31.Add("fnil");
                            }
                            if (((((buf.b[num] == 0x6c) || (buf.b[num] == 0x4c)) && ((buf.b[num + 1] == 0x61) || (buf.b[num + 1] == 0x41))) && (((buf.b[num + 2] == 0x74) || (buf.b[num + 2] == 0x54)) && ((buf.b[num + 3] == 0x69) || (buf.b[num + 3] == 0x49)))) && (((buf.b[num + 4] == 110) || (buf.b[num + 4] == 0x4e)) && (buf.b[num + 5] == 0x35)))
                            {
                                charset = 0xcc;
                                num44 = 0x4e3;
                                class31.Clear();
                                class31.Add("fnil");
                            }
                            if ((((buf.b[num] == 0x38) && (buf.b[num + 1] == 0x38)) && ((buf.b[num + 2] == 0x35) && (buf.b[num + 3] == 0x39))) && ((buf.b[num + 4] == 0x2d) && (buf.b[num + 5] == 0x31)))
                            {
                                num44 = 0x4e4;
                                charset = 0;
                                class31.Clear();
                                class31.Add("fswiss");
                            }
                            if (((((buf.b[num] == 0x6c) || (buf.b[num] == 0x4c)) && ((buf.b[num + 1] == 0x61) || (buf.b[num + 1] == 0x41))) && (((buf.b[num + 2] == 0x74) || (buf.b[num + 2] == 0x54)) && ((buf.b[num + 3] == 0x69) || (buf.b[num + 3] == 0x49)))) && (((buf.b[num + 4] == 110) || (buf.b[num + 4] == 0x4e)) && (buf.b[num + 5] == 0x31)))
                            {
                                num44 = 0x4e4;
                                charset = 0;
                                class31.Clear();
                                class31.Add("fswiss");
                            }
                            if ((((((buf.b[num] == 0x73) || (buf.b[num] == 0x53)) && ((buf.b[num + 1] == 0x68) || (buf.b[num + 1] == 0x48))) && (((buf.b[num + 2] == 0x69) || (buf.b[num + 2] == 0x49)) && ((buf.b[num + 3] == 0x66) || (buf.b[num + 3] == 70)))) && ((((buf.b[num + 4] == 0x74) || (buf.b[num + 4] == 0x54)) && (buf.b[num + 5] == 0x5f)) && (((buf.b[num + 6] == 0x6a) || (buf.b[num + 6] == 0x4a)) && ((buf.b[num + 7] == 0x69) || (buf.b[num + 7] == 0x49))))) && ((buf.b[num + 8] == 0x73) || (buf.b[num + 8] == 0x53)))
                            {
                                num44 = 0x4e4;
                                class31.Clear();
                                class31.Add("fswiss");
                                lang = 0x409;
                                hieroglyph = true;
                                num45 = 0x3a4;
                                charset = 0x80;
                            }
                            if (((((buf.b[num] == 0x75) || (buf.b[num] == 0x55)) && ((buf.b[num + 1] == 0x74) || (buf.b[num + 1] == 0x54))) && (((buf.b[num + 2] == 0x66) || (buf.b[num + 2] == 70)) && (buf.b[num + 3] == 0x2d))) && ((buf.b[num + 4] == 0x38) || ((buf.b[num + 4] == 0x31) && (buf.b[num + 5] == 0x36))))
                            {
                                num44 = 0x4e4;
                                class31.Clear();
                                class31.Add("fswiss");
                            }
                            if (buf.b[num] == 0x3e)
                            {
                                break;
                            }
                            num++;
                        }
                    }
                    num++;
                }
            }
            if (class30.byteCmpi("Japanese") == 0)
            {
                charset = 0x80;
            }
            else if (class30.byteCmpi("SimplifiedChinese") == 0)
            {
                charset = 0x86;
            }
            else if (class30.byteCmpi("TraditionalChinese") == 0)
            {
                charset = 0x88;
            }
            else if (class30.byteCmpi("Korean") == 0)
            {
                charset = 0x81;
            }
            else if (class30.byteCmpi("Russian") == 0)
            {
                charset = 0xcc;
                num45 = 0x4e3;
            }
            num44 = this.get_ansicpg(class30.ByteToString());
            int num47 = 0;
            if (num47 == 0)
            {
                num47 = 80;
            }
            if (this._outputTextFormat == eOutputTextFormat.Text)
            {
                class4.Clear();
                num = 0;
                num4 = 0;
                num10 = 0;
                while (num < max)
                {
                    if ((((!flag9 && (buf.b[num] == 60)) && ((buf.b[num + 1] == 0x68) || (buf.b[num + 1] == 0x48))) && (((buf.b[num + 2] == 0x65) || (buf.b[num + 2] == 0x45)) && ((buf.b[num + 3] == 0x61) || (buf.b[num + 3] == 0x41)))) && (((buf.b[num + 4] == 100) || (buf.b[num + 4] == 0x44)) && (buf.b[num + 5] == 0x3e)))
                    {
                        while (num < max)
                        {
                            if ((((((buf.b[num] == 60) && (buf.b[num + 1] == 0x73)) && ((buf.b[num + 2] == 0x74) && (buf.b[num + 3] == 0x79))) && ((buf.b[num + 4] == 0x6c) && (buf.b[num + 5] == 0x65))) || ((((buf.b[num] == 60) && (buf.b[num + 1] == 0x53)) && ((buf.b[num + 2] == 0x74) && (buf.b[num + 3] == 0x79))) && ((buf.b[num + 4] == 0x6c) && (buf.b[num + 5] == 0x65)))) || ((((buf.b[num] == 60) && (buf.b[num + 1] == 0x53)) && ((buf.b[num + 2] == 0x54) && (buf.b[num + 3] == 0x59))) && ((buf.b[num + 4] == 0x4c) && (buf.b[num + 5] == 0x45))))
                            {
                                num4 = 0;
                                while ((num4 == 0) && (num < max))
                                {
                                    if ((((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x73) && (buf.b[num + 3] == 0x74))) && (((buf.b[num + 4] == 0x79) && (buf.b[num + 5] == 0x6c)) && ((buf.b[num + 6] == 0x65) && (buf.b[num + 7] == 0x3e)))) || ((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x53) && (buf.b[num + 3] == 0x74))) && (((buf.b[num + 4] == 0x79) && (buf.b[num + 5] == 0x6c)) && ((buf.b[num + 6] == 0x65) && (buf.b[num + 7] == 0x3e))))) || ((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x53) && (buf.b[num + 3] == 0x54))) && (((buf.b[num + 4] == 0x59) && (buf.b[num + 5] == 0x4c)) && ((buf.b[num + 6] == 0x45) && (buf.b[num + 7] == 0x3e)))))
                                    {
                                        num4 = 1;
                                    }
                                    num++;
                                }
                                num += 7;
                            }
                            if ((((buf.b[num] == 60) && ((buf.b[num + 1] == 0x62) || (buf.b[num + 1] == 0x42))) && (((buf.b[num + 2] == 0x6f) || (buf.b[num + 2] == 0x4f)) && ((buf.b[num + 3] == 100) || (buf.b[num + 3] == 0x44)))) && (((buf.b[num + 4] == 0x79) || (buf.b[num + 4] == 0x59)) && ((buf.b[num + 5] == 0x3e) || this.IS_DELIMITER(buf.b[num + 5]))))
                            {
                                if ((buf.b[num + 5] == 0x20) || this.IS_DELIMITER(buf.b[num + 5]))
                                {
                                    while ((buf.b[num + 5] != 0x3e) && (num < max))
                                    {
                                        num++;
                                    }
                                }
                                num += 5;
                                flag9 = true;
                                goto Label_27BA;
                            }
                            if (((buf.b[num] == 60) && ((buf.b[num + 1] == 0x70) || (buf.b[num + 1] == 80))) && (((buf.b[num + 2] == 0x3e) || (buf.b[num + 2] == 0x20)) || this.IS_DELIMITER(buf.b[num + 2])))
                            {
                                num--;
                                flag9 = true;
                                goto Label_27BA;
                            }
                            if ((((buf.b[num] == 60) && ((buf.b[num + 1] == 0x74) || (buf.b[num + 1] == 0x54))) && (((buf.b[num + 2] == 100) || (buf.b[num + 2] == 0x44)) || ((buf.b[num + 2] == 0x68) || (buf.b[num + 2] == 0x48)))) && ((buf.b[num + 3] == 0x3e) || this.IS_DELIMITER(buf.b[num + 3])))
                            {
                                if ((buf.b[num + 3] == 0x20) || this.IS_DELIMITER(buf.b[num + 3]))
                                {
                                    while ((buf.b[num + 3] != 0x3e) && (num < max))
                                    {
                                        num++;
                                    }
                                }
                                num += 3;
                                flag9 = true;
                                goto Label_27BA;
                            }
                            if ((((buf.b[num] == 60) && ((buf.b[num + 1] == 0x73) || (buf.b[num + 1] == 0x53))) && (((buf.b[num + 2] == 0x63) || (buf.b[num + 2] == 0x43)) && ((buf.b[num + 3] == 0x72) || (buf.b[num + 3] == 0x52)))) && ((((buf.b[num + 4] == 0x69) || (buf.b[num + 4] == 0x49)) && ((buf.b[num + 5] == 0x70) || (buf.b[num + 5] == 80))) && ((buf.b[num + 6] == 0x74) || (buf.b[num + 6] == 0x54))))
                            {
                                num4 = 0;
                                while ((num4 == 0) && (num < max))
                                {
                                    if (((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x73) || (buf.b[num + 2] == 0x53))) && (((buf.b[num + 3] == 0x63) || (buf.b[num + 3] == 0x43)) && ((buf.b[num + 4] == 0x72) || (buf.b[num + 4] == 0x52)))) && ((((buf.b[num + 5] == 0x69) || (buf.b[num + 5] == 0x49)) && ((buf.b[num + 6] == 0x70) || (buf.b[num + 6] == 80))) && (((buf.b[num + 7] == 0x74) || (buf.b[num + 7] == 0x54)) && (buf.b[num + 8] == 0x3e))))
                                    {
                                        num4 = 1;
                                    }
                                    num++;
                                }
                                num += 7;
                            }
                            if (((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x68) || (buf.b[num + 2] == 0x48))) && (((buf.b[num + 3] == 0x65) || (buf.b[num + 3] == 0x45)) && ((buf.b[num + 4] == 0x61) || (buf.b[num + 4] == 0x41)))) && (((buf.b[num + 5] == 100) || (buf.b[num + 5] == 0x44)) && (buf.b[num + 6] == 0x3e)))
                            {
                                num += 6;
                                flag9 = true;
                                goto Label_27BA;
                            }
                            num++;
                        }
                        goto Label_27BA;
                    }
                    if ((((buf.b[num] == 60) && ((buf.b[num + 1] == 0x73) || (buf.b[num + 1] == 0x53))) && (((buf.b[num + 2] == 0x63) || (buf.b[num + 2] == 0x43)) && ((buf.b[num + 3] == 0x72) || (buf.b[num + 3] == 0x52)))) && ((((buf.b[num + 4] == 0x69) || (buf.b[num + 4] == 0x49)) && ((buf.b[num + 5] == 0x70) || (buf.b[num + 5] == 80))) && ((buf.b[num + 6] == 0x74) || (buf.b[num + 6] == 0x54))))
                    {
                        num4 = 0;
                        while ((num4 == 0) && (num < max))
                        {
                            if (((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x73) || (buf.b[num + 2] == 0x53))) && (((buf.b[num + 3] == 0x63) || (buf.b[num + 3] == 0x43)) && ((buf.b[num + 4] == 0x72) || (buf.b[num + 4] == 0x52)))) && ((((buf.b[num + 5] == 0x69) || (buf.b[num + 5] == 0x49)) && ((buf.b[num + 6] == 0x70) || (buf.b[num + 6] == 80))) && (((buf.b[num + 7] == 0x74) || (buf.b[num + 7] == 0x54)) && (buf.b[num + 8] == 0x3e))))
                            {
                                num4 = 1;
                            }
                            num++;
                        }
                        num += 7;
                        goto Label_27BA;
                    }
                    if ((((buf.b[num] != 60) || ((buf.b[num + 1] != 0x6f) && (buf.b[num + 1] != 0x4f))) || (((buf.b[num + 2] != 0x70) && (buf.b[num + 2] != 80)) || ((buf.b[num + 3] != 0x74) && (buf.b[num + 3] != 0x54)))) || ((((buf.b[num + 4] != 0x69) && (buf.b[num + 4] != 0x49)) || ((buf.b[num + 5] != 0x6f) && (buf.b[num + 5] != 0x4f))) || (((buf.b[num + 6] != 110) && (buf.b[num + 6] != 0x4e)) || ((buf.b[num + 7] != 0x3e) && (buf.b[num + 7] != 0x20)))))
                    {
                        goto Label_238A;
                    }
                    num4 = 0;
                    while ((num4 == 0) && (num < max))
                    {
                        if (((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x6f) || (buf.b[num + 2] == 0x4f))) && (((buf.b[num + 3] == 0x70) || (buf.b[num + 3] == 80)) && ((buf.b[num + 4] == 0x74) || (buf.b[num + 4] == 0x54)))) && ((((buf.b[num + 5] == 0x69) || (buf.b[num + 5] == 0x49)) && ((buf.b[num + 6] == 0x6f) || (buf.b[num + 6] == 0x4f))) && (((buf.b[num + 7] == 110) || (buf.b[num + 7] == 0x4e)) && (buf.b[num + 8] == 0x3e))))
                        {
                            num4 = 1;
                            goto Label_237F;
                        }
                        num++;
                    }
                Label_237F:
                    num += 8;
                goto Label_27BA;
            Label_238A:
                if ((((((buf.b[num] == 60) && (buf.b[num + 1] == 0x73)) && ((buf.b[num + 2] == 0x74) && (buf.b[num + 3] == 0x79))) && ((buf.b[num + 4] == 0x6c) && (buf.b[num + 5] == 0x65))) || ((((buf.b[num] == 60) && (buf.b[num + 1] == 0x53)) && ((buf.b[num + 2] == 0x74) && (buf.b[num + 3] == 0x79))) && ((buf.b[num + 4] == 0x6c) && (buf.b[num + 5] == 0x65)))) || ((((buf.b[num] == 60) && (buf.b[num + 1] == 0x53)) && ((buf.b[num + 2] == 0x54) && (buf.b[num + 3] == 0x59))) && ((buf.b[num + 4] == 0x4c) && (buf.b[num + 5] == 0x45))))
                {
                    num4 = 0;
                    while ((num4 == 0) && (num < max))
                    {
                        if ((((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x73) && (buf.b[num + 3] == 0x74))) && (((buf.b[num + 4] == 0x79) && (buf.b[num + 5] == 0x6c)) && ((buf.b[num + 6] == 0x65) && (buf.b[num + 7] == 0x3e)))) || ((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x53) && (buf.b[num + 3] == 0x74))) && (((buf.b[num + 4] == 0x79) && (buf.b[num + 5] == 0x6c)) && ((buf.b[num + 6] == 0x65) && (buf.b[num + 7] == 0x3e))))) || ((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x53) && (buf.b[num + 3] == 0x54))) && (((buf.b[num + 4] == 0x59) && (buf.b[num + 5] == 0x4c)) && ((buf.b[num + 6] == 0x45) && (buf.b[num + 7] == 0x3e)))))
                        {
                            num4 = 1;
                        }
                        num++;
                    }
                    num += 7;
                }
                else if ((buf.b[num] == 60) && (buf.b[num + 1] == 0x25))
                {
                    while (num < max)
                    {
                        if ((buf.b[num - 1] == 0x25) && (buf.b[num] == 0x3e))
                        {
                            goto Label_27BA;
                        }
                        num++;
                    }
                }
                else if (((buf.b[num] == 60) && (buf.b[num + 1] == 0x21)) && ((buf.b[num + 2] == 0x2d) && (buf.b[num + 3] == 0x2d)))
                {
                    num += 3;
                    num7 = 1;
                    while ((num7 > 0) && (num < max))
                    {
                        if (((buf.b[num] == 0x2d) && (buf.b[num + 1] == 0x2d)) && (buf.b[num + 2] == 0x3e))
                        {
                            num7--;
                            num++;
                        }
                        if (((buf.b[num] == 60) && (buf.b[num + 1] == 0x21)) && ((buf.b[num + 2] == 0x2d) && (buf.b[num + 3] == 0x2d)))
                        {
                            num7++;
                            num += 3;
                        }
                        num++;
                    }
                }
                else
                {
                    class4.Add(buf.b[num]);
                    num10++;
                }
        Label_27BA:
            num++;
                }
                max = num10 = class4.len;
                buf.Clear();
                num = 0;
                num4 = 1;
                num11 = 0;
                num5 = 0;
                while (num < class4.len)
                {
                    if (buf.len > 0x1193f)
                    {
                        break;
                    }
                    if (((num == 0) && (class4.b[num] == 0xef)) && ((class4.b[num + 1] == 0xbb) && (class4.b[num + 2] == 0xbf)))
                    {
                        num += 2;
                    }
                    else if ((((!flag14 && (class4.b[num] == 60)) && ((class4.b[num + 1] == 0x68) || (class4.b[num + 1] == 0x48))) && (((class4.b[num + 2] == 0x74) || (class4.b[num + 2] == 0x54)) && ((class4.b[num + 3] == 0x6d) || (class4.b[num + 3] == 0x4d)))) && (((class4.b[num + 4] == 0x6c) || (class4.b[num + 4] == 0x4c)) && ((class4.b[num + 5] == 0x3e) || this.IS_DELIMITER(class4.b[num + 5]))))
                    {
                        num += 5;
                        while ((class4.b[num] != 0x3e) && (num < max))
                        {
                            num++;
                        }
                        num11 = 0;
                        flag14 = true;
                    }
                    else if (((class4.b[num] == 60) && (class4.b[num + 1] == 0x21)) && ((class4.b[num + 2] == 0x2d) && (class4.b[num + 3] == 0x2d)))
                    {
                        num += 3;
                        while (num < max)
                        {
                            if (((class4.b[num] == 0x2d) && (class4.b[num + 1] == 0x2d)) && (class4.b[num + 2] == 0x3e))
                            {
                                break;
                            }
                            if (((class4.b[num] == 60) && (class4.b[num + 1] == 0x21)) && ((class4.b[num + 2] == 0x2d) && (class4.b[num + 3] == 0x2d)))
                            {
                                num += 3;
                                while (num < max)
                                {
                                    if (((class4.b[num] == 0x2d) && (class4.b[num + 1] == 0x2d)) && (class4.b[num + 2] == 0x3e))
                                    {
                                        num += 2;
                                        break;
                                    }
                                    num++;
                                }
                            }
                            num++;
                        }
                        num += 2;
                    }
                    else
                    {
                        if ((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x70) || (class4.b[num + 2] == 80))) && (class4.b[num + 3] == 0x3e))
                        {
                            buf.Add(this.LF);
                            num += 4;
                        }
                        if (((class4.b[num] == 60) && ((class4.b[num + 1] == 0x70) || (class4.b[num + 1] == 80))) && (class4.b[num + 2] == 0x3e))
                        {
                            buf.Add(this.LF);
                            num += 3;
                        }
                        if ((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x68) || (class4.b[num + 2] == 0x48))) && (class4.b[num + 4] == 0x3e))
                        {
                            buf.Add(this.LF);
                            num += 5;
                        }
                        if ((((this._preserveHyperlinks != 1) || ((class4.b[num] != 0x68) && (class4.b[num] != 0x48))) || (((class4.b[num + 1] != 0x72) && (class4.b[num + 1] != 0x52)) || ((class4.b[num + 2] != 0x65) && (class4.b[num + 2] != 0x45)))) || (((class4.b[num + 3] != 0x66) && (class4.b[num + 3] != 70)) || ((class4.b[num + 4] != 0x3d) || (class4.b[num + 5] != 0x22))))
                        {
                            goto Label_2EC9;
                        }
                        flag10 = false;
                        index = num;
                        while (index > 0)
                        {
                            if ((class4.b[index] == 60) || (class4.b[index] == 0x3e))
                            {
                                break;
                            }
                            if (((class4.b[index + 1] == 0x20) && ((class4.b[index] == 0x61) || (class4.b[index] == 0x41))) && (class4.b[index - 1] == 60))
                            {
                                flag10 = true;
                            }
                            index--;
                        }
                        if (flag10)
                        {
                            num += 5;
                            num7 = 0;
                            class16.b[num7] = class4.b[num++];
                            num7++;
                            flag19 = true;
                            for (num38 = 0; (num38 < 500) && ((num + num38) < num10); num38++)
                            {
                                if ((((class4.b[num + num38] == 60) && (class4.b[(num + 1) + num38] == 0x2f)) && ((class4.b[(num + 2) + num38] == 0x61) || (class4.b[(num + 2) + num38] == 0x41))) && (class4.b[(num + 3) + num38] == 0x3e))
                                {
                                    break;
                                }
                                if ((((class4.b[num + num38] == 60) && ((class4.b[(num + 1) + num38] == 0x69) || (class4.b[(num + 1) + num38] == 0x49))) && (((class4.b[(num + 2) + num38] == 0x6d) || (class4.b[(num + 2) + num38] == 0x4d)) && ((class4.b[(num + 3) + num38] == 0x67) || (class4.b[(num + 3) + num38] == 0x47)))) && (class4.b[(num + 4) + num38] == 0x20))
                                {
                                    flag19 = false;
                                }
                            }
                            if (flag19 || (this._preserveImages != 0))
                            {
                                goto Label_2E10;
                            }
                        }
                    }
                    goto Label_3D27;
                Label_2DF9:
                    class16.Add(class4.b[num]);
                num++;
            Label_2E10:
                if ((class4.b[num] != 0x22) && (num < max))
                {
                    goto Label_2DF9;
                }
            class16.b[num7++] = class4.b[num];
            class16.b[num7] = 0;
            for (num7 = 0; class16.b[num7] != 0; num7++)
            {
                class17.b[num7] = 0;
            }
            if (class17.IndexOf("javascript") != -1)
            {
                flag10 = false;
                flag8 = false;
            }
            else
            {
                class15.Add("[");
                class15.Add(class16);
                class15.Add("]");
                flag8 = true;
            }
            while ((class4.b[num] != 0x3e) && (num < max))
            {
                num++;
            }
            goto Label_3D27;
        Label_2EC9:
            if ((((this._preserveHyperlinks == 1) && (class4.b[num] == 60)) && (class4.b[num + 1] == 0x2f)) && (((class4.b[num + 2] == 0x61) || (class4.b[num + 2] == 0x41)) && (class4.b[num + 3] == 0x3e)))
            {
                if (flag8)
                {
                    buf.Add(class15);
                }
                flag8 = false;
                num += 3;
            }
            else if ((((class4.b[num] == 60) && ((class4.b[num + 1] == 0x74) || (class4.b[num + 1] == 0x54))) && (((class4.b[num + 2] == 0x61) || (class4.b[num + 2] == 0x41)) && ((class4.b[num + 3] == 0x62) || (class4.b[num + 3] == 0x42)))) && ((((class4.b[num + 4] == 0x6c) || (class4.b[num + 4] == 0x4c)) && ((class4.b[num + 5] == 0x65) || (class4.b[num + 5] == 0x45))) && ((class4.b[num + 6] == 0x3e) || this.IS_DELIMITER(class4.b[num + 6]))))
            {
                if (this.IS_DELIMITER(class4.b[num + 6]))
                {
                    _tablesArray[num36].table_p.percent_width = 0;
                    int num48 = -1111;
                    _tablesArray[num36].table_p.table_width = this.get_width(class4, num, max, ref _tablesArray[num36].table_p.percent_width, ref index, ref index, null, num48, ref num48, null, ref num48, ref num48);
                    while ((class4.b[num + 6] != 0x3e) && (num < max))
                    {
                        num++;
                    }
                }
                if (_tablesArray[num36].table)
                {
                    while (_tablesArray[num36].cell > 0)
                    {
                        buf.Add("\t\t");
                        num11 += 2;
                        nest_tables _tables1 = _tablesArray[num36];
                        _tables1.cell--;
                    }
                    buf.Add(this.LF);
                    num11++;
                }
                _tablesArray[num36].table = false;
                num4 = 0;
                num += 6;
            }
            else if (((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x74) || (class4.b[num + 2] == 0x54))) && (((class4.b[num + 3] == 0x61) || (class4.b[num + 3] == 0x41)) && ((class4.b[num + 4] == 0x62) || (class4.b[num + 4] == 0x42)))) && ((((class4.b[num + 5] == 0x6c) || (class4.b[num + 5] == 0x4c)) && ((class4.b[num + 6] == 0x65) || (class4.b[num + 6] == 0x45))) && (class4.b[num + 7] == 0x3e)))
            {
                _tablesArray[num36].table = false;
                if (buf.len > 1)
                {
                    while ((buf.len > 1) && this.IS_DELIMITER(buf.b[buf.len - 1]))
                    {
                        buf.len--;
                    }
                }
                buf.Add(this.LF);
                num11++;
                num += 7;
            }
            else if (((class4.b[num] == 60) && ((class4.b[num + 1] == 0x74) || (class4.b[num + 1] == 0x54))) && (((class4.b[num + 2] == 0x72) || (class4.b[num + 2] == 0x52)) && ((class4.b[num + 3] == 0x3e) || this.IS_DELIMITER(class4.b[num + 3]))))
            {
                if (this.IS_DELIMITER(class4.b[num + 3]))
                {
                    while ((class4.b[num + 3] != 0x3e) && (num < max))
                    {
                        num++;
                    }
                }
                num += 3;
                if (_tablesArray[num36].table)
                {
                    while (_tablesArray[num36].cell > 0)
                    {
                        buf.Add("\t\t");
                        num11 += 2;
                        nest_tables _tables2 = _tablesArray[num36];
                        _tables2.cell--;
                    }
                    buf.Add(this.LF);
                    num11++;
                }
                if (((buf.len > 1) && (buf.b[buf.len - 1] == 0x20)) && this.IS_DELIMITER(buf.b[buf.len - 2]))
                {
                    buf.len--;
                }
                _tablesArray[num36].table = true;
                for (num7 = 0; num7 < this.MAX_COLUMNS; num7++)
                {
                    _tablesArray[num36].td_width[num7] = 0;
                    _tablesArray[num36].td_percent_width[num7] = 0;
                }
                _tablesArray[num36].td = 0;
                for (num7 = 0; (num + num7) < max; num7++)
                {
                    if (((((class4.b[num + num7] == 60) && (class4.b[(num + num7) + 1] == 0x2f)) && ((class4.b[(num + num7) + 2] == 0x74) || (class4.b[(num + num7) + 2] == 0x54))) && (((class4.b[(num + num7) + 3] == 0x72) || (class4.b[(num + num7) + 3] == 0x52)) && (class4.b[(num + num7) + 4] == 0x3e))) || (((class4.b[num + num7] == 60) && ((class4.b[(num + num7) + 1] == 0x74) || (class4.b[(num + num7) + 1] == 0x54))) && (((class4.b[(num + num7) + 2] == 0x72) || (class4.b[(num + num7) + 2] == 0x52)) && ((class4.b[(num + num7) + 3] == 0x3e) || this.IS_DELIMITER(class4.b[(num + num7) + 3])))))
                    {
                        break;
                    }
                    if ((((class4.b[num + num7] == 60) && ((class4.b[(num + num7) + 1] == 0x74) || (class4.b[(num + num7) + 1] == 0x54))) && (((class4.b[(num + num7) + 2] == 100) || (class4.b[(num + num7) + 2] == 0x44)) || ((class4.b[(num + num7) + 2] == 0x68) || (class4.b[(num + num7) + 2] == 0x48)))) && ((class4.b[(num + num7) + 3] == 0x3e) || this.IS_DELIMITER(class4.b[(num + num7) + 3])))
                    {
                        nest_tables _tables3 = _tablesArray[num36];
                        _tables3.td++;
                        if (this.IS_DELIMITER(class4.b[(num + num7) + 3]))
                        {
                            num7 += 3;
                            int num49 = -1111;
                            _tablesArray[num36].td_width[_tablesArray[num36].td - 1] = this.get_width(class4, num + num7, max, ref _tablesArray[num36].td_percent_width[_tablesArray[num36].td - 1], ref index, ref index, null, num49, ref num49, null, ref num49, ref num49);
                        }
                    }
                    if ((((class4.b[num + num7] == 60) && ((class4.b[(num + num7) + 1] == 0x74) || (class4.b[(num + num7) + 1] == 0x54))) && (((class4.b[(num + num7) + 2] == 0x61) || (class4.b[(num + num7) + 2] == 0x41)) && ((class4.b[(num + num7) + 3] == 0x62) || (class4.b[(num + num7) + 3] == 0x42)))) && ((((class4.b[(num + num7) + 4] == 0x6c) || (class4.b[(num + num7) + 4] == 0x4c)) && ((class4.b[(num + num7) + 5] == 0x65) || (class4.b[(num + num7) + 5] == 0x45))) && (this.IS_DELIMITER(class4.b[(num + num7) + 6]) || (class4.b[(num + num7) + 6] == 0x3e))))
                    {
                        break;
                    }
                }
                _tablesArray[num36].cell = _tablesArray[num36].td;
                this.fill_columns_width(_tablesArray[num36].td_width, _tablesArray[num36].td_percent_width, _tablesArray[num36].td, _tablesArray[num36].table_p.table_width, _tablesArray[num36].table_p.percent_width, this.SCREEN_W_DEF);
            }
            else if ((((class4.b[num] == 60) && ((class4.b[num + 1] == 0x74) || (class4.b[num + 1] == 0x54))) && (((class4.b[num + 2] == 100) || (class4.b[num + 2] == 0x44)) || ((class4.b[num + 2] == 0x68) || (class4.b[num + 2] == 0x48)))) && ((class4.b[num + 3] == 0x3e) || this.IS_DELIMITER(class4.b[num + 3])))
            {
                if (this.IS_DELIMITER(class4.b[num + 3]))
                {
                    while (!this.IS_XTHAN(class4.b[num + 3]) && (num < max))
                    {
                        num++;
                    }
                }
                num += 3;
                num4 = 0;
            }
            else if ((((_tablesArray[num36].table && (class4.b[num] == 60)) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x74) || (class4.b[num + 2] == 0x54))) && ((((class4.b[num + 3] == 100) || (class4.b[num + 3] == 0x44)) || ((class4.b[num + 3] == 0x68) || (class4.b[num + 3] == 0x48))) && (class4.b[num + 4] == 0x3e)))
            {
                buf.Add("\t\t");
                num11 += 2;
                num += 4;
                nest_tables _tables4 = _tablesArray[num36];
                _tables4.cell--;
            }
            else if ((((_tablesArray[num36].table && (class4.b[num] == 60)) && (class4.b[num + 1] == 0x2f)) && (((class4.b[num + 2] == 0x74) || (class4.b[num + 2] == 0x54)) && ((class4.b[num + 3] == 0x72) || (class4.b[num + 3] == 0x52)))) && (class4.b[num + 4] == 0x3e))
            {
                while (_tablesArray[num36].cell > 0)
                {
                    buf.Add("\t\t");
                    num11 += 2;
                    nest_tables _tables5 = _tablesArray[num36];
                    _tables5.cell--;
                }
                buf.Add(this.LF);
                num11++;
                num += 4;
                _tablesArray[num36].table = false;
            }
            else if (((class4.b[num] == 60) && ((class4.b[num + 1] == 0x62) || (class4.b[num + 1] == 0x42))) && (((class4.b[num + 2] == 0x72) || (class4.b[num + 2] == 0x52)) && (((class4.b[num + 3] == 0x3e) || (class4.b[num + 3] == 0x20)) || (class4.b[num + 3] == 0x2f))))
            {
                if ((class4.b[num + 3] == 0x20) || (class4.b[num + 3] == 0x2f))
                {
                    while ((class4.b[num + 3] != 0x3e) && (num < max))
                    {
                        num++;
                    }
                }
                buf.Add(this.LF);
                num11++;
                num += 3;
            }
            else
            {
                if (((class4.b[num] == 60) && ((class4.b[num + 1] == 0x6c) || (class4.b[num + 1] == 0x4c))) && (((class4.b[num + 2] == 0x69) || (class4.b[num + 2] == 0x49)) && ((class4.b[num + 3] == 0x3e) || (class4.b[num + 3] == 0x20))))
                {
                    if (class4.b[num + 3] == 0x20)
                    {
                        while ((class4.b[num + 3] != 0x3e) && (num < max))
                        {
                            num++;
                        }
                    }
                    buf.Add(this.LF);
                    num11++;
                    num += 4;
                }
                if (((num > 1) && (class4.b[num - 1] == 0x3e)) && (class4.b[num] != 60))
                {
                    num4 = 1;
                }
                if ((class4.b[num] == 0x26) && (num4 != 0))
                {
                    num += this.special_symbols_txt(class4, num, max, class24);
                    if ((class24.b[0] != 0x20) || flag18)
                    {
                        buf.Add(class24);
                        num11 += class24.len;
                        num5 = 0;
                    }
                }
                else
                {
                    if (num11 > 0x1139b)
                    {
                        break;
                    }
                    if (class4.b[num] == 60)
                    {
                        num4 = 0;
                    }
                    else
                    {
                        if (num4 != 0)
                        {
                            if (this.IS_DELIMITER(class4.b[num]))
                            {
                                if (((buf.len > 0) && (num5 == 0)) && !this.IS_DELIMITER(buf.b[buf.len - 1]))
                                {
                                    buf.Add((byte)0x20);
                                    num11++;
                                    num5 = 1;
                                }
                            }
                            else
                            {
                                num5 = 0;
                                buf.Add(class4.b[num]);
                                num11++;
                                flag18 = true;
                            }
                        }
                        
                    }
                }
            }
    Label_3D27:
        num++;
                }
            }
            else
            {
                object[] objArray;
                fontsize = this._fontSize;
                string[] strArray = new string[] { this.LF, @"\par\pard\sb", num25.ToString(), @"\sa", num26.ToString(), @"\sbauto1\saauto1\fs", (fontsize * 2).ToString(), @"\lang", lang.ToString(), " " };
                string str14 = string.Concat(strArray);
                strArray = new string[10];
                strArray[0] = this.LF;
                strArray[1] = @"\par\pard\intbl\sb";
                strArray[2] = num25.ToString();
                strArray[3] = @"\sa";
                strArray[4] = num26.ToString();
                strArray[5] = @"\sbauto1\saauto1\fs";
                strArray[6] = (fontsize * 2).ToString();
                strArray[7] = @"\lang";
                strArray[8] = lang.ToString();
                strArray[9] = " ";
                string str15 = string.Concat(strArray);
                strArray = new string[8];
                strArray[0] = this.LF;
                strArray[1] = @"\pard\intbl\sb";
                strArray[2] = num25.ToString();
                strArray[3] = @"\fs";
                strArray[4] = (fontsize * 2).ToString();
                strArray[5] = @"\lang";
                strArray[6] = lang.ToString();
                strArray[7] = " ";
                string str16 = string.Concat(strArray);
                string str17 = this.LF + @"\sb0\sa0\par ";
                string str18 = this.LF + @"\sb0\sa0\par\intbl\lang" + lang.ToString() + " ";
                class7.Clear();
                class7.Add((fontsize * 2).ToString());
                if ((fontsize <= 0) || (fontsize > this.FONT_SIZE_MAX))
                {
                    fontsize = 10;
                }
                string str19 = (fontsize * 2).ToString();
                string str20 = this.LF + @"{\par\b\fs" + str19 + " ";
                string str21 = this.LF + @"{\b\fs" + str19 + " ";
                int length = str20.Length;
                int num51 = str21.Length;
                int num52 = 0;
                str19 = (fontsize / 2).ToString();
                string str22 = @"{\sub\dn" + str19 + " ";
                int num53 = str22.Length;
                str19 = (fontsize / 2).ToString();
                string str23 = @"{\super\up" + str19 + " ";
                int num54 = str23.Length;
                int num55 = 0;
                int num56 = 0;
                int num57 = 1;
                int num58 = 0;
                int num59 = 0;
                int[] numArray4 = new int[this.STK_MAX];
                int num60 = 0;
                int num61 = 100;
                listStyleType[] typeArray = new listStyleType[10];
                ByteClass strOLsymbol = new ByteClass();
                strOLsymbol.Add("1.");
                bool flag21 = false;
                class4.Clear();
                num = 0;
                num4 = 0;
                num10 = 0;
                while (num < max)
                {
                    if ((((buf.b[num] != 60) || ((buf.b[num + 1] != 0x6c) && (buf.b[num + 1] != 0x4c))) || (((buf.b[num + 2] != 0x69) && (buf.b[num + 2] != 0x49)) || ((buf.b[num + 3] != 110) && (buf.b[num + 3] != 0x4e)))) || (((buf.b[num + 4] != 0x6b) && (buf.b[num + 4] != 0x4b)) || !this.IS_DELIMITER(buf.b[num + 5])))
                    {
                        goto Label_43C7;
                    }
                    num += 5;
                    num7 = 0;
                    num5 = 0;
                    while (((num + num7) < max) && (buf.b[num + num7] != 0x3e))
                    {
                        if ((((((buf.b[num + num7] == 0x73) || (buf.b[num + num7] == 0x53)) && ((buf.b[(num + num7) + 1] == 0x74) || (buf.b[(num + num7) + 1] == 0x54))) && (((buf.b[(num + num7) + 2] == 0x79) || (buf.b[(num + num7) + 2] == 0x59)) && ((buf.b[(num + num7) + 3] == 0x6c) || (buf.b[(num + num7) + 3] == 0x4c)))) && ((((buf.b[(num + num7) + 4] == 0x65) || (buf.b[(num + num7) + 4] == 0x45)) && ((buf.b[(num + num7) + 5] == 0x73) || (buf.b[(num + num7) + 5] == 0x53))) && (((buf.b[(num + num7) + 6] == 0x68) || (buf.b[(num + num7) + 6] == 0x48)) && ((buf.b[(num + num7) + 7] == 0x65) || (buf.b[(num + num7) + 7] == 0x45))))) && (((buf.b[(num + num7) + 8] == 0x65) || (buf.b[(num + num7) + 8] == 0x45)) && ((buf.b[(num + num7) + 9] == 0x74) || (buf.b[(num + num7) + 9] == 0x54))))
                        {
                            num5 = 1;
                            goto Label_4287;
                        }
                        num7++;
                    }
                Label_4287:
                    if (num5 == 1)
                    {
                        for (num7 = 0; ((num + num7) < max) && (buf.b[num + num7] != 0x3e); num7++)
                        {
                            if (((((buf.b[num + num7] == 0x68) || (buf.b[num + num7] == 0x48)) && ((buf.b[(num + num7) + 1] == 0x72) || (buf.b[(num + num7) + 1] == 0x52))) && (((buf.b[(num + num7) + 2] == 0x65) || (buf.b[(num + num7) + 2] == 0x45)) && ((buf.b[(num + num7) + 3] == 0x66) || (buf.b[(num + num7) + 3] == 70)))) && (buf.b[(num + num7) + 4] == 0x3d))
                            {
                                num += num7;
                                num += 4;
                                this.ReadValue(buf, ref num, _params.file_name);
                                this.CombinePaths(this._htmlPath, _params.file_name);
                                this.CSS_read_file(_params);
                                goto Label_43AB;
                            }
                        }
                    }
            Label_43AB:
                while ((buf.b[num] != 0x3e) && (num < max))
                {
                    num++;
                }
            goto Label_5BD8;
        Label_43C7:
            if ((((!flag9 && (buf.b[num] == 60)) && ((buf.b[num + 1] == 0x68) || (buf.b[num + 1] == 0x48))) && (((buf.b[num + 2] == 0x65) || (buf.b[num + 2] == 0x45)) && ((buf.b[num + 3] == 0x61) || (buf.b[num + 3] == 0x41)))) && (((buf.b[num + 4] == 100) || (buf.b[num + 4] == 0x44)) && (buf.b[num + 5] == 0x3e)))
            {
                while (num < max)
                {
                    if ((((buf.b[num] != 60) || ((buf.b[num + 1] != 0x6c) && (buf.b[num + 1] != 0x4c))) || (((buf.b[num + 2] != 0x69) && (buf.b[num + 2] != 0x49)) || ((buf.b[num + 3] != 110) && (buf.b[num + 3] != 0x4e)))) || (((buf.b[num + 4] != 0x6b) && (buf.b[num + 4] != 0x4b)) || !this.IS_DELIMITER(buf.b[num + 5])))
                    {
                        goto Label_4834;
                    }
                    num += 5;
                    num7 = 0;
                    num5 = 0;
                    while (((num + num7) < max) && (buf.b[num + num7] != 0x3e))
                    {
                        if ((((((buf.b[num + num7] == 0x73) || (buf.b[num + num7] == 0x53)) && ((buf.b[(num + num7) + 1] == 0x74) || (buf.b[(num + num7) + 1] == 0x54))) && (((buf.b[(num + num7) + 2] == 0x79) || (buf.b[(num + num7) + 2] == 0x59)) && ((buf.b[(num + num7) + 3] == 0x6c) || (buf.b[(num + num7) + 3] == 0x4c)))) && ((((buf.b[(num + num7) + 4] == 0x65) || (buf.b[(num + num7) + 4] == 0x45)) && ((buf.b[(num + num7) + 5] == 0x73) || (buf.b[(num + num7) + 5] == 0x53))) && (((buf.b[(num + num7) + 6] == 0x68) || (buf.b[(num + num7) + 6] == 0x48)) && ((buf.b[(num + num7) + 7] == 0x65) || (buf.b[(num + num7) + 7] == 0x45))))) && (((buf.b[(num + num7) + 8] == 0x65) || (buf.b[(num + num7) + 8] == 0x45)) && ((buf.b[(num + num7) + 9] == 0x74) || (buf.b[(num + num7) + 9] == 0x54))))
                        {
                            num5 = 1;
                            goto Label_46FC;
                        }
                        num7++;
                    }
                Label_46FC:
                    if (num5 == 1)
                    {
                        for (num7 = 0; ((num + num7) < max) && (buf.b[num + num7] != 0x3e); num7++)
                        {
                            if (((((buf.b[num + num7] == 0x68) || (buf.b[num + num7] == 0x48)) && ((buf.b[(num + num7) + 1] == 0x72) || (buf.b[(num + num7) + 1] == 0x52))) && (((buf.b[(num + num7) + 2] == 0x65) || (buf.b[(num + num7) + 2] == 0x45)) && ((buf.b[(num + num7) + 3] == 0x66) || (buf.b[(num + num7) + 3] == 70)))) && (buf.b[(num + num7) + 4] == 0x3d))
                            {
                                num += num7;
                                num += 4;
                                this.ReadValue(buf, ref num, _params.file_name);
                                this.CombinePaths(this._htmlPath, _params.file_name);
                                this.CSS_read_file(_params);
                                goto Label_4820;
                            }
                        }
                    }
            Label_4820:
                while ((buf.b[num] != 0x3e) && (num < max))
                {
                    num++;
                }
        Label_4834:
            if (((((((buf.b[num] == 60) && (buf.b[num + 1] == 0x73)) && ((buf.b[num + 2] == 0x74) && (buf.b[num + 3] == 0x79))) && ((buf.b[num + 4] == 0x6c) && (buf.b[num + 5] == 0x65))) || ((((buf.b[num] == 60) && (buf.b[num + 1] == 0x53)) && ((buf.b[num + 2] == 0x74) && (buf.b[num + 3] == 0x79))) && ((buf.b[num + 4] == 0x6c) && (buf.b[num + 5] == 0x65)))) || ((((buf.b[num] == 60) && (buf.b[num + 1] == 0x53)) && ((buf.b[num + 2] == 0x54) && (buf.b[num + 3] == 0x59))) && ((buf.b[num + 4] == 0x4c) && (buf.b[num + 5] == 0x45)))) && !this.CSS_read_from_tags(_params, max, buf, ref num))
            {
                num4 = 0;
                while ((num4 == 0) && (num < max))
                {
                    if ((((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x73) && (buf.b[num + 3] == 0x74))) && (((buf.b[num + 4] == 0x79) && (buf.b[num + 5] == 0x6c)) && ((buf.b[num + 6] == 0x65) && (buf.b[num + 7] == 0x3e)))) || ((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x53) && (buf.b[num + 3] == 0x74))) && (((buf.b[num + 4] == 0x79) && (buf.b[num + 5] == 0x6c)) && ((buf.b[num + 6] == 0x65) && (buf.b[num + 7] == 0x3e))))) || ((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x53) && (buf.b[num + 3] == 0x54))) && (((buf.b[num + 4] == 0x59) && (buf.b[num + 5] == 0x4c)) && ((buf.b[num + 6] == 0x45) && (buf.b[num + 7] == 0x3e)))))
                    {
                        num4 = 1;
                    }
                    num++;
                }
                num += 7;
            }
        if ((((buf.b[num] == 60) && ((buf.b[num + 1] == 0x62) || (buf.b[num + 1] == 0x42))) && (((buf.b[num + 2] == 0x6f) || (buf.b[num + 2] == 0x4f)) && ((buf.b[num + 3] == 100) || (buf.b[num + 3] == 0x44)))) && (((buf.b[num + 4] == 0x79) || (buf.b[num + 4] == 0x59)) && ((buf.b[num + 5] == 0x3e) || this.IS_DELIMITER(buf.b[num + 5]))))
        {
            if (this.IS_DELIMITER(buf.b[num + 5]))
            {
                while ((buf.b[num + 5] != 0x3e) && (num < max))
                {
                    num++;
                }
            }
            num += 5;
            flag9 = true;
            goto Label_5BD8;
        }
        if (((buf.b[num] == 60) && ((buf.b[num + 1] == 0x70) || (buf.b[num + 1] == 80))) && ((buf.b[num + 2] == 0x3e) || this.IS_DELIMITER(buf.b[num + 2])))
        {
            num--;
            flag9 = true;
            goto Label_5BD8;
        }
        if ((((buf.b[num] == 60) && ((buf.b[num + 1] == 0x74) || (buf.b[num + 1] == 0x54))) && (((buf.b[num + 2] == 100) || (buf.b[num + 2] == 0x44)) || ((buf.b[num + 2] == 0x68) || (buf.b[num + 2] == 0x48)))) && ((buf.b[num + 3] == 0x3e) || this.IS_DELIMITER(buf.b[num + 3])))
        {
            if (this.IS_DELIMITER(buf.b[num + 3]))
            {
                while (!this.IS_XTHAN(buf.b[num + 3]) && (num < max))
                {
                    num++;
                }
            }
            num += 3;
            flag9 = true;
            goto Label_5BD8;
        }
        if ((((buf.b[num] == 60) && ((buf.b[num + 1] == 0x73) || (buf.b[num + 1] == 0x53))) && (((buf.b[num + 2] == 0x63) || (buf.b[num + 2] == 0x43)) && ((buf.b[num + 3] == 0x72) || (buf.b[num + 3] == 0x52)))) && ((((buf.b[num + 4] == 0x69) || (buf.b[num + 4] == 0x49)) && ((buf.b[num + 5] == 0x70) || (buf.b[num + 5] == 80))) && ((buf.b[num + 6] == 0x74) || (buf.b[num + 6] == 0x54))))
        {
            num4 = 0;
            while ((num4 == 0) && (num < max))
            {
                if (((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x73) || (buf.b[num + 2] == 0x53))) && (((buf.b[num + 3] == 0x63) || (buf.b[num + 3] == 0x43)) && ((buf.b[num + 4] == 0x72) || (buf.b[num + 4] == 0x52)))) && ((((buf.b[num + 5] == 0x69) || (buf.b[num + 5] == 0x49)) && ((buf.b[num + 6] == 0x70) || (buf.b[num + 6] == 80))) && (((buf.b[num + 7] == 0x74) || (buf.b[num + 7] == 0x54)) && (buf.b[num + 8] == 0x3e))))
                {
                    num4 = 1;
                }
                num++;
            }
            num += 7;
        }
        if (((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x68) || (buf.b[num + 2] == 0x48))) && (((buf.b[num + 3] == 0x65) || (buf.b[num + 3] == 0x45)) && ((buf.b[num + 4] == 0x61) || (buf.b[num + 4] == 0x41)))) && (((buf.b[num + 5] == 100) || (buf.b[num + 5] == 0x44)) && (buf.b[num + 6] == 0x3e)))
        {
            num += 6;
            flag9 = true;
            goto Label_5BD8;
        }
        num++;
                }
            }
            else if ((((buf.b[num] == 60) && ((buf.b[num + 1] == 0x73) || (buf.b[num + 1] == 0x53))) && (((buf.b[num + 2] == 0x63) || (buf.b[num + 2] == 0x43)) && ((buf.b[num + 3] == 0x72) || (buf.b[num + 3] == 0x52)))) && ((((buf.b[num + 4] == 0x69) || (buf.b[num + 4] == 0x49)) && ((buf.b[num + 5] == 0x70) || (buf.b[num + 5] == 80))) && ((buf.b[num + 6] == 0x74) || (buf.b[num + 6] == 0x54))))
            {
                num4 = 0;
                while ((num4 == 0) && (num < max))
                {
                    if (((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x73) || (buf.b[num + 2] == 0x53))) && (((buf.b[num + 3] == 0x63) || (buf.b[num + 3] == 0x43)) && ((buf.b[num + 4] == 0x72) || (buf.b[num + 4] == 0x52)))) && ((((buf.b[num + 5] == 0x69) || (buf.b[num + 5] == 0x49)) && ((buf.b[num + 6] == 0x70) || (buf.b[num + 6] == 80))) && (((buf.b[num + 7] == 0x74) || (buf.b[num + 7] == 0x54)) && (buf.b[num + 8] == 0x3e))))
                    {
                        num4 = 1;
                    }
                    num++;
                }
                num += 7;
            }
            else if ((((((buf.b[num] == 60) && (buf.b[num + 1] == 0x73)) && ((buf.b[num + 2] == 0x74) && (buf.b[num + 3] == 0x79))) && ((buf.b[num + 4] == 0x6c) && (buf.b[num + 5] == 0x65))) || ((((buf.b[num] == 60) && (buf.b[num + 1] == 0x53)) && ((buf.b[num + 2] == 0x74) && (buf.b[num + 3] == 0x79))) && ((buf.b[num + 4] == 0x6c) && (buf.b[num + 5] == 0x65)))) || ((((buf.b[num] == 60) && (buf.b[num + 1] == 0x53)) && ((buf.b[num + 2] == 0x54) && (buf.b[num + 3] == 0x59))) && ((buf.b[num + 4] == 0x4c) && (buf.b[num + 5] == 0x45))))
            {
                if (!this.CSS_read_from_tags(_params, max, buf, ref num))
                {
                    num4 = 0;
                    while ((num4 == 0) && (num < max))
                    {
                        if ((((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x73) && (buf.b[num + 3] == 0x74))) && (((buf.b[num + 4] == 0x79) && (buf.b[num + 5] == 0x6c)) && ((buf.b[num + 6] == 0x65) && (buf.b[num + 7] == 0x3e)))) || ((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x53) && (buf.b[num + 3] == 0x74))) && (((buf.b[num + 4] == 0x79) && (buf.b[num + 5] == 0x6c)) && ((buf.b[num + 6] == 0x65) && (buf.b[num + 7] == 0x3e))))) || ((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x53) && (buf.b[num + 3] == 0x54))) && (((buf.b[num + 4] == 0x59) && (buf.b[num + 5] == 0x4c)) && ((buf.b[num + 6] == 0x45) && (buf.b[num + 7] == 0x3e)))))
                        {
                            num4 = 1;
                        }
                        num++;
                    }
                    num += 7;
                }
            }
            else if ((((buf.b[num] == 60) && ((buf.b[num + 1] == 0x66) || (buf.b[num + 1] == 70))) && (((buf.b[num + 2] == 0x6f) || (buf.b[num + 2] == 0x4f)) && ((buf.b[num + 3] == 0x72) || (buf.b[num + 3] == 0x52)))) && (((buf.b[num + 4] == 0x6d) || (buf.b[num + 4] == 0x4d)) && ((buf.b[num + 5] == 0x3e) || this.IS_DELIMITER(buf.b[num + 5]))))
            {
                num += 5;
                while ((buf.b[num] != 0x3e) && (num < max))
                {
                    num++;
                }
            }
            else if (((((buf.b[num] == 60) && ((buf.b[num + 1] == 0x6f) || (buf.b[num + 1] == 0x4f))) && (((buf.b[num + 2] == 0x70) || (buf.b[num + 2] == 80)) && ((buf.b[num + 3] == 0x74) || (buf.b[num + 3] == 0x54)))) && ((((buf.b[num + 4] == 0x69) || (buf.b[num + 4] == 0x49)) && ((buf.b[num + 5] == 0x6f) || (buf.b[num + 5] == 0x4f))) && ((buf.b[num + 6] == 110) || (buf.b[num + 6] == 0x4e)))) && ((buf.b[num + 7] == 0x3e) || (buf.b[num + 7] == 0x20)))
            {
                while (num < max)
                {
                    if (((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x6f) || (buf.b[num + 2] == 0x4f))) && (((buf.b[num + 3] == 0x70) || (buf.b[num + 3] == 80)) && ((buf.b[num + 4] == 0x74) || (buf.b[num + 4] == 0x54)))) && ((((buf.b[num + 5] == 0x69) || (buf.b[num + 5] == 0x49)) && ((buf.b[num + 6] == 0x6f) || (buf.b[num + 6] == 0x4f))) && (((buf.b[num + 7] == 110) || (buf.b[num + 7] == 0x4e)) && (buf.b[num + 8] == 0x3e))))
                    {
                        num += 8;
                        goto Label_5BD8;
                    }
                    num++;
                }
            }
            else if ((((buf.b[num] == 60) && ((buf.b[num + 1] == 0x73) || (buf.b[num + 1] == 0x53))) && (((buf.b[num + 2] == 0x65) || (buf.b[num + 2] == 0x45)) && ((buf.b[num + 3] == 0x6c) || (buf.b[num + 3] == 0x4c)))) && ((((buf.b[num + 4] == 0x65) || (buf.b[num + 4] == 0x45)) && ((buf.b[num + 5] == 0x63) || (buf.b[num + 5] == 0x43))) && (((buf.b[num + 6] == 0x74) || (buf.b[num + 6] == 0x54)) && ((buf.b[num + 7] == 0x3e) || this.IS_DELIMITER(buf.b[num + 7])))))
            {
                num += 7;
                while (num < max)
                {
                    if (((((buf.b[num] == 60) && (buf.b[num + 1] == 0x2f)) && ((buf.b[num + 2] == 0x73) || (buf.b[num + 2] == 0x53))) && (((buf.b[num + 3] == 0x65) || (buf.b[num + 3] == 0x45)) && ((buf.b[num + 4] == 0x6c) || (buf.b[num + 4] == 0x4c)))) && ((((buf.b[num + 5] == 0x65) || (buf.b[num + 5] == 0x45)) && ((buf.b[num + 6] == 0x63) || (buf.b[num + 6] == 0x43))) && (((buf.b[num + 7] == 0x74) || (buf.b[num + 7] == 0x54)) && (buf.b[num + 8] == 0x3e))))
                    {
                        num += 8;
                        goto Label_5BD8;
                    }
                    num++;
                }
            }
            else if ((buf.b[num] == 60) && (buf.b[num + 1] == 0x25))
            {
                while (num < max)
                {
                    if ((buf.b[num - 1] == 0x25) && (buf.b[num] == 0x3e))
                    {
                        goto Label_5BD8;
                    }
                    num++;
                }
            }
            else if (((buf.b[num] == 60) && (buf.b[num + 1] == 0x21)) && ((buf.b[num + 2] == 0x2d) && (buf.b[num + 3] == 0x2d)))
            {
                num += 3;
                num7 = 1;
                while ((num7 > 0) && (num < max))
                {
                    if (((buf.b[num] == 0x2d) && (buf.b[num + 1] == 0x2d)) && (buf.b[num + 2] == 0x3e))
                    {
                        num7--;
                        num++;
                    }
                    if (((buf.b[num] == 60) && (buf.b[num + 1] == 0x21)) && ((buf.b[num + 2] == 0x2d) && (buf.b[num + 3] == 0x2d)))
                    {
                        num7++;
                        num += 3;
                    }
                    num++;
                }
            }
            else if (((class4.len <= 1) || (buf.b[num] != 13)) || (class4.b[class4.len - 1] != 13))
            {
                class4.Add(buf.b[num]);
                num10++;
            }
    Label_5BD8:
        num++;
                }
                max = num10;
                for (num = 0; num < num10; num++)
                {
                    if (((((this._preserveFontFace == 1) && ((class4.b[num] == 0x66) || (class4.b[num] == 70))) && (((class4.b[num + 1] == 0x6f) || (class4.b[num + 1] == 0x4f)) && ((class4.b[num + 2] == 110) || (class4.b[num + 2] == 0x4e)))) && ((((class4.b[num + 3] == 0x74) || (class4.b[num + 3] == 0x54)) && (class4.b[num + 4] == 0x2d)) && (((class4.b[num + 5] == 0x66) || (class4.b[num + 5] == 70)) && ((class4.b[num + 6] == 0x61) || (class4.b[num + 6] == 0x41))))) && (((((class4.b[num + 7] == 0x6d) || (class4.b[num + 7] == 0x4d)) && ((class4.b[num + 8] == 0x69) || (class4.b[num + 8] == 0x49))) && (((class4.b[num + 9] == 0x6c) || (class4.b[num + 9] == 0x4c)) && ((class4.b[num + 10] == 0x79) || (class4.b[num + 10] == 0x59)))) && (class4.b[num + 11] == 0x3a)))
                    {
                        num += 12;
                        class8.Clear();
                        this.read_value_CSS(class4, ref num, num10, class8, -1111, true);
                        if (class8.len != 0)
                        {
                            num7 = 0;
                            flag = false;
                            while ((num7 < num13) && (num7 < this.MAX_FONTS))
                            {
                                if (class8.byteCmpi(list[num7].ToString()) == 0)
                                {
                                    flag = true;
                                }
                                num7++;
                            }
                            if (!flag)
                            {
                                list.Add(class8.ByteToString());
                                num13++;
                            }
                        }
                    }
                    else
                    {
                        if (((((this._preserveFontColor == 1) || (this._preserveBackgroundColor == 1)) && ((class4.b[num] == 0x63) || (class4.b[num] == 0x43))) && (((class4.b[num + 1] == 0x6f) || (class4.b[num + 1] == 0x4f)) && ((class4.b[num + 2] == 0x6c) || (class4.b[num + 2] == 0x4c)))) && ((((class4.b[num + 3] == 0x6f) || (class4.b[num + 3] == 0x4f)) && ((class4.b[num + 4] == 0x72) || (class4.b[num + 4] == 0x52))) && (class4.b[num + 5] == 0x3a)))
                        {
                            num += 6;
                            this.read_color(class4, ref num, max, class10);
                            num7 = 0;
                            flag2 = false;
                            while ((num7 < num14) && (num7 < this.MAX_COLORS))
                            {
                                if (class10.byteCmpi(list2[num7].ToString()) == 0)
                                {
                                    flag2 = true;
                                }
                                num7++;
                            }
                            if (!flag2)
                            {
                                list2.Add(class10.ByteToString());
                                num14++;
                                class10.Clear();
                            }
                        }
                        if ((((class4.b[num] == 60) && ((class4.b[num + 1] == 0x66) || (class4.b[num + 1] == 70))) && (((class4.b[num + 2] == 0x6f) || (class4.b[num + 2] == 0x4f)) && ((class4.b[num + 3] == 110) || (class4.b[num + 3] == 0x4e)))) && (((class4.b[num + 4] == 0x74) || (class4.b[num + 4] == 0x54)) && this.IS_DELIMITER(class4.b[num + 5])))
                        {
                            while ((class4.b[num + 5] != 0x3e) && (num < num10))
                            {
                                if ((((this._preserveFontColor != 1) || ((class4.b[num] != 0x66) && (class4.b[num] != 70))) || (((class4.b[num + 1] != 0x61) && (class4.b[num + 1] != 0x41)) || ((class4.b[num + 2] != 0x63) && (class4.b[num + 2] != 0x43)))) || (((class4.b[num + 3] != 0x65) && (class4.b[num + 3] != 0x45)) || (class4.b[num + 4] != 0x3d)))
                                {
                                    goto Label_61CA;
                                }
                                num7 = 0;
                                class8.Clear();
                                if ((class4.b[num + 5] != 0x22) && (class4.b[num + 5] != 0x27))
                                {
                                    goto Label_6145;
                                }
                                while ((((class4.b[num + 6] != 0x3e) && (class4.b[num + 6] != 0x22)) && (class4.b[num + 6] != 0x27)) && (class4.b[num + 6] != 0x2c))
                                {
                                    class8.Add(class4.b[num + 6]);
                                    num++;
                                }
                                goto Label_6179;
                            Label_612C:
                                class8.Add(class4.b[num + 5]);
                            num++;
                        Label_6145:
                            if (((class4.b[num + 5] != 0x3e) && !this.IS_DELIMITER(class4.b[num + 5])) && (class4.b[num + 6] != 0x2c))
                            {
                                goto Label_612C;
                            }
                    Label_6179:
                        num7 = 0;
                    flag = false;
                    while ((num7 < num13) && (num7 < this.MAX_FONTS))
                    {
                        if (class8.byteCmpi(list[num7].ToString()) == 0)
                        {
                            flag = true;
                        }
                        num7++;
                    }
                    if (!flag)
                    {
                        list.Add(class8.ByteToString());
                        num13++;
                    }
                Label_61CA:
                    if (((((this._preserveFontColor == 1) || (this._preserveBackgroundColor == 1)) && ((class4.b[num] == 0x63) || (class4.b[num] == 0x43))) && (((class4.b[num + 1] == 0x6f) || (class4.b[num + 1] == 0x4f)) && ((class4.b[num + 2] == 0x6c) || (class4.b[num + 2] == 0x4c)))) && ((((class4.b[num + 3] == 0x6f) || (class4.b[num + 3] == 0x4f)) && ((class4.b[num + 4] == 0x72) || (class4.b[num + 4] == 0x52))) && ((class4.b[num + 5] == 0x3d) || (class4.b[num + 5] == 0x3a))))
                    {
                        num += 6;
                        this.read_color(class4, ref num, max, class10);
                        num7 = 0;
                        flag2 = false;
                        while ((num7 < num14) && (num7 < this.MAX_COLORS))
                        {
                            if (class10.byteCmpi(list2[num7].ToString()) == 0)
                            {
                                flag2 = true;
                            }
                            num7++;
                        }
                        if (!flag2)
                        {
                            list2.Add(class10.ByteToString());
                            num14++;
                            class10.Clear();
                        }
                    }
                num++;
                            }
                        }
                        else if (((((this._preserveBackgroundColor == 1) && ((class4.b[num] == 0x62) || (class4.b[num] == 0x42))) && (((class4.b[num + 1] == 0x67) || (class4.b[num + 1] == 0x47)) && ((class4.b[num + 2] == 0x63) || (class4.b[num + 2] == 0x43)))) && ((((class4.b[num + 3] == 0x6f) || (class4.b[num + 3] == 0x4f)) && ((class4.b[num + 4] == 0x6c) || (class4.b[num + 4] == 0x4c))) && (((class4.b[num + 5] == 0x6f) || (class4.b[num + 5] == 0x4f)) && ((class4.b[num + 6] == 0x72) || (class4.b[num + 6] == 0x52))))) && ((class4.b[num + 7] == 0x3d) || (class4.b[num + 7] == 0x3a)))
                        {
                            num += 8;
                            this.read_color(class4, ref num, max, class10);
                            num7 = 0;
                            flag2 = false;
                            while ((num7 < num14) && (num7 < this.MAX_COLORS))
                            {
                                if (class10.byteCmpi(list2[num7].ToString()) == 0)
                                {
                                    flag2 = true;
                                }
                                num7++;
                            }
                            if (!flag2)
                            {
                                list2.Add(class10.ByteToString());
                                num14++;
                                class10.Clear();
                            }
                        }
                    }
                }
                if (_params.use)
                {
                    _params.font_list_num = num13;
                    _params.color_list_num = num14;
                    this.CSS_analyse(_params, _params.buf, list, list2);
                    num13 = _params.font_list_num;
                    num14 = _params.color_list_num;
                }
                else
                {
                    _params.font_list_num = num13;
                    _params.color_list_num = num14;
                }
                if ((_params.body_tag && (((CSS_styles)_params.CSS_style[_params.body_tag_index]).ablaze.b[0] == 1)) && (this._preserveFontFace == 1))
                {
                    class6.Clear();
                    class6.Add((string)list[((CSS_styles)_params.CSS_style[_params.body_tag_index]).font_family]);
                }
                buf.Clear();
                num11 = 0;
                if (this._rtfParts == eRtfParts.RtfCompletely)
                {
                    if (!hieroglyph)
                    {
                        class16.Clear();
                        objArray = new object[] { @"{\rtf1\ansi\ansicpg", num44, @"\deff0\deflang", lang, @"\fs", (fontsize * 2).ToString(), @"{\fonttbl{\f0\", class31.ByteToString(), @"\fprq2\fcharset", charset, " ", class6.ByteToString(), ";}", this.LF };
                        class16.Add(string.Concat(objArray));
                    }
                    else
                    {
                        class16.Clear();
                        objArray = new object[] { @"{\rtf1\ansi\ansicpg", num44, @"\deff0\deflang", lang, @"\fs", (fontsize * 2).ToString(), @"{\fonttbl{\f0\", class31.ByteToString(), @"\fprq2\fcharset", charset, " ", class6.ByteToString(), " Unicode MS;}", this.LF };
                        class16.Add(string.Concat(objArray));
                    }
                    buf.Add(class16);
                    num11 += class16.len;
                }
                else
                {
                    buf.Add("{");
                    num11++;
                }
                if (this._rtfParts == eRtfParts.RtfCompletely)
                {
                    for (num7 = 0; num7 < num13; num7++)
                    {
                        class16.Clear();
                        class16.Add(string.Concat(new object[] { @"{\f", num7 + 2, @"\fnil\fcharset", charset, " ", list[num7].ToString(), ";}", this.LF }));
                        buf.Add(class16);
                        num11 += class16.len;
                    }
                    class16.Clear();
                    class16.Add(@"{\f99\froman\fcharset0\fprq2{\*\panose 02020603050405020304}" + class6.ByteToString() + ";}");
                    class16.Add(@"{\f100\fnil\fcharset2 Symbol;}{\f101\fnil\fcharset2 Wingdings;}{\f102\fcharset204{\*\fname Courier New;}Courier New CYR;}{\f103\fcharset0 " + class6.ByteToString() + ";}}");
                    class16.Add(this.LF + @"{\colortbl ;\red0\green0\blue0;\red51\green102\blue255;");
                    buf.Add(class16);
                    num11 += class16.len;
                    for (num7 = 0; num7 < list2.Count; num7++)
                    {
                        class10.Clear();
                        str8 = list2[num7].ToString();
                        class10.Add((byte)str8[0]);
                        class10.Add((byte)str8[1]);
                        numArray[0] = this.hex_to_dec(class10);
                        class10.Clear();
                        class10.Add((byte)str8[2]);
                        class10.Add((byte)str8[3]);
                        numArray[1] = this.hex_to_dec(class10);
                        class10.Clear();
                        class10.Add((byte)str8[4]);
                        class10.Add((byte)str8[5]);
                        numArray[2] = this.hex_to_dec(class10);
                        class10.Clear();
                        class16.Clear();
                        class16.Add(@"\red" + numArray[0].ToString() + @"\green" + numArray[1].ToString() + @"\blue" + numArray[2].ToString() + ";");
                        buf.Add(class16);
                        num11 += class16.len;
                    }
                    buf.Add("}" + this.LF);
                    num11 += 2;
                }
                if (this._pageOrientation == ePageOrientation.Portrait)
                {
                    class16.Clear();
                    class16.Add(string.Concat(new object[] { @"\paperw", this.page_width, @"\paperh", this.page_height, @"\margl", this.margl, @"\margr", this.margr, @"\margt", this.margt, @"\margb", this.margb, "" }));
                    this.TBLEN = this.TBLEN = (this.page_width - this.margl) - this.margr;
                    if (this.TBLEN < 0x3e8)
                    {
                        this.TBLEN = 0x251c;
                    }
                }
                else
                {
                    class16.Clear();
                    class16.Add(string.Concat(new object[] { @"\paperh", this.page_width, @"\paperw", this.page_height, @"\landscape\margl", this.margl, @"\margr", this.margr, @"\margt", this.margt, @"\margb", this.margb }));
                    this.TBLEN = (this.page_height - this.margl) - this.margr;
                    if (this.TBLEN < 0x3e8)
                    {
                        this.TBLEN = 0x251c;
                    }
                }
                if (this._rtfParts == eRtfParts.RtfCompletely)
                {
                    buf.Add(class16);
                    num11 += class16.len;
                }
                if (_params.body_tag)
                {
                    if ((((CSS_styles)_params.CSS_style[_params.body_tag_index]).ablaze.b[0] == 1) && (this._preserveFontFace == 1))
                    {
                        class16.Clear();
                        class16.Add(this.LF + @"\f" + ((((CSS_styles)_params.CSS_style[_params.body_tag_index]).font_family + 2)).ToString() + " ");
                        buf.Add(class16);
                        num11 += class16.len;
                    }
                    if ((((CSS_styles)_params.CSS_style[_params.body_tag_index]).ablaze.b[1] == 1) && (this._preserveFontColor == 1))
                    {
                        class16.Clear();
                        class16.Add(this.LF + @"\cf" + ((((CSS_styles)_params.CSS_style[_params.body_tag_index]).color + 3)).ToString() + " ");
                        buf.Add(class16);
                        num11 += class16.len;
                    }
                    if ((((CSS_styles)_params.CSS_style[_params.body_tag_index]).ablaze.b[3] == 1) && (this._preserveFontSize == 1))
                    {
                        fontsize = ((CSS_styles)_params.CSS_style[_params.body_tag_index]).font_size / 2;
                        if ((fontsize <= 0) || (fontsize > this.FONT_SIZE_MAX))
                        {
                            fontsize = 12;
                        }
                        else
                        {
                            str19 = (fontsize * 2).ToString();
                            strArray = new string[] { this.LF, @"\par\pard\sb", num25.ToString(), @"\sa", num26.ToString(), @"\sbauto1\saauto1\fs", (fontsize * 2).ToString(), @"\lang", lang.ToString(), " " };
                            str14 = string.Concat(strArray);
                            strArray = new string[10];
                            strArray[0] = this.LF;
                            strArray[1] = @"\par\pard\intbl\sb";
                            strArray[2] = num25.ToString();
                            strArray[3] = @"\sa";
                            strArray[4] = num26.ToString();
                            strArray[5] = @"\sbauto1\saauto1\fs";
                            strArray[6] = (fontsize * 2).ToString();
                            strArray[7] = @"\lang";
                            strArray[8] = lang.ToString();
                            strArray[9] = " ";
                            str15 = string.Concat(strArray);
                            strArray = new string[8];
                            strArray[0] = this.LF;
                            strArray[1] = @"\pard\intbl\sb";
                            strArray[2] = num25.ToString();
                            strArray[3] = @"\fs";
                            strArray[4] = (fontsize * 2).ToString();
                            strArray[5] = @"\lang";
                            strArray[6] = lang.ToString();
                            strArray[7] = " ";
                            str16 = string.Concat(strArray);
                            str17 = this.LF + @"\sb0\sa0\par ";
                            str18 = this.LF + @"\sb0\sa0\par\intbl\lang" + lang.ToString() + " ";
                            str20 = this.LF + @"{\par\b\fs" + str19 + " ";
                            str21 = this.LF + @"{\b\fs" + str19 + " ";
                            length = str20.Length;
                            num51 = str21.Length;
                            str19 = (fontsize / 2).ToString();
                            str22 = @"{\sub\dn" + str19 + " ";
                            num53 = str22.Length;
                            str19 = (fontsize / 2).ToString();
                            str23 = @"{\super\up" + str19 + " ";
                            num54 = str23.Length;
                        }
                    }
                }
                if (this._rtfParts == eRtfParts.RtfCompletely)
                {
                    if (this._pageNumbers == ePageNumbers.PageNumSecond)
                    {
                        class16.Clear();
                        class16.Add(this.LF + @"\titlepg ");
                        buf.Add(class16);
                        num11 += class16.len;
                    }
                    if (this._pageHeader.Length > 0)
                    {
                        class16.Clear();
                        class16.Add(this.LF + @"{\header\pard\plain " + this._pageHeader + @" \par}");
                        buf.Add(class16);
                        num11 += class16.len;
                    }
                    if (((this._pageFooter.Length > 0) || (this._pageNumbers == ePageNumbers.PageNumFirst)) || (this._pageNumbers == ePageNumbers.PageNumSecond))
                    {
                        class16.Clear();
                        if ((this._pageNumbers == ePageNumbers.PageNumFirst) || (this._pageNumbers == ePageNumbers.PageNumSecond))
                        {
                            if (this._pageOrientation == ePageOrientation.Landscape)
                            {
                                num39 = this.page_height;
                                num40 = this.page_width;
                            }
                            else
                            {
                                num39 = this.page_width;
                                num40 = this.page_height;
                            }
                            if (this._pageNumbersAlignH == ePageAlignment.AlignCenter)
                            {
                                num8 = num39 / 2;
                            }
                            else if (this._pageNumbersAlignH == ePageAlignment.AlignRight)
                            {
                                num8 = num39 - this.margr;
                            }
                            else
                            {
                                num8 = 0;
                            }
                            if (num8 > num39)
                            {
                                num8 = 0;
                            }
                            if (this._pageNumbersAlignV == ePageAlignment.AlignTop)
                            {
                                num9 = -(this.margt / 2);
                            }
                            else
                            {
                                num9 = num40 - 950;
                            }
                            if (num9 > num40)
                            {
                                num9 = 0;
                            }
                            class16.Clear();
                            str19 = (fontsize * 2).ToString();
                            class17.Add(string.Concat(new object[] { @" \pvpg\phpg\posx", num8, @"\posy", num9, @"{\field{\*\fldinst {\fs", str19, @" PAGE}}}\par", this.LF }));
                        }
                        class16.Clear();
                        class16.Add(this.LF + @"{\footer" + class17.ByteToString() + @"\pard\plain " + this._pageFooter + @" \par}");
                        buf.Add(class16);
                        num11 += class16.len;
                    }
                    class16.Clear();
                    str19 = (fontsize * 2).ToString();
                    objArray = new object[] { this.LF, @"\pard\sb", num25, @"\sa", num26, @"\sbauto1\saauto1\fs", (fontsize * 2).ToString(), @"\lang", lang, this.LF };
                    class16.Add(string.Concat(objArray));
                    buf.Add(class16);
                    num11 += class16.len;
                }
                numArray3[0] = fontsize;
                num = 0;
                num4 = 1;
                num12 = buf.len;
                num5 = 1;
                while (num < num10)
                {
                    string str11;
                    if ((((class4.len > 0) && (num > 0)) && ((class4.b[num - 1] == 0x3e) && (class4.b[num] != 60))) && (class4.b[num] != 10))
                    {
                        num4 = 1;
                    }
                    if ((((!flag14 && (class4.b[num] == 60)) && ((class4.b[num + 1] == 0x68) || (class4.b[num + 1] == 0x48))) && (((class4.b[num + 2] == 0x74) || (class4.b[num + 2] == 0x54)) && ((class4.b[num + 3] == 0x6d) || (class4.b[num + 3] == 0x4d)))) && (((class4.b[num + 4] == 0x6c) || (class4.b[num + 4] == 0x4c)) && ((class4.b[num + 5] == 0x3e) || this.IS_DELIMITER(class4.b[num + 5]))))
                    {
                        num += 5;
                        while ((class4.b[num] != 0x3e) && (num < max))
                        {
                            num++;
                        }
                        buf.len = num12;
                        flag14 = true;
                        goto Label_134B7;
                    }
                    if ((((class4.b[num] == 60) && ((class4.b[num + 1] == 0x62) || (class4.b[num + 1] == 0x42))) && (((class4.b[num + 2] == 0x6f) || (class4.b[num + 2] == 0x4f)) && ((class4.b[num + 3] == 100) || (class4.b[num + 3] == 0x44)))) && (((class4.b[num + 4] == 0x79) || (class4.b[num + 4] == 0x59)) && ((class4.b[num + 5] == 0x3e) || this.IS_DELIMITER(class4.b[num + 5]))))
                    {
                        if (this.IS_DELIMITER(class4.b[num + 5]))
                        {
                            while ((class4.b[num + 5] != 0x3e) && (num < max))
                            {
                                num++;
                            }
                        }
                        num += 5;
                        buf.len = num12;
                        goto Label_134B7;
                    }
                    if (((num == 0) && (class4.b[num] == 0xef)) && ((class4.b[num + 1] == 0xbb) && (class4.b[num + 2] == 0xbf)))
                    {
                        num += 2;
                        goto Label_134B7;
                    }
                    if ((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x70) || (class4.b[num + 2] == 80))) && (class4.b[num + 3] == 0x3e))
                    {
                        if (flag12)
                        {
                            if (num34 < 2)
                            {
                                num34++;
                            }
                            else
                            {
                                num34 = 1;
                                num5 = 0;
                            }
                        }
                        if (_tablesArray[num36].table)
                        {
                            buf.Add(str15);
                            num5 = 1;
                            if ((num36 > 0) && (this._preserveNestedTables == 1))
                            {
                                str11 = @"\itap" + _tablesArray[num36].table_level + " ";
                                buf.Add(str11);
                                num11 += str11.Length;
                            }
                        }
                        else
                        {
                            buf.Add(str14);
                            num5 = 1;
                        }
                        num += 3;
                        if (!flag3)
                        {
                            buf.Add(class8);
                            num11 += class8.len;
                        }
                        if (!flag5)
                        {
                            buf.Add(newb);
                            num11 += newb.len;
                        }
                        this.CSS_close(class22, class18, align, buf, ref num11, _params, ref flag4, class10, class14, ref num21, ref flag16, ref flag5, newb, fontsize, CSS_tag_type.P_CSS);
                        goto Label_134B7;
                    }
                    if (((class4.b[num] == 60) && ((class4.b[num + 1] == 0x70) || (class4.b[num + 1] == 80))) && ((class4.b[num + 2] == 0x3e) || (class4.b[num + 2] == 0x20)))
                    {
                        if (flag12)
                        {
                            if (num34 < 2)
                            {
                                num34++;
                                num5 = 1;
                            }
                            else
                            {
                                num34 = 1;
                                num5 = 0;
                            }
                        }
                        if (buf.len < (num12 + 2))
                        {
                            num5 = 1;
                        }
                        if (num5 == 0)
                        {
                            if (_tablesArray[num36].table)
                            {
                                if (flag21)
                                {
                                    buf.Add(str15);
                                }
                                else
                                {
                                    buf.Add(str16);
                                }
                                if ((num36 > 0) && (this._preserveNestedTables == 1))
                                {
                                    str11 = @"\itap" + _tablesArray[num36].table_level + " ";
                                    buf.Add(str11);
                                    num11 += str11.Length;
                                }
                            }
                            else
                            {
                                buf.Add(str14);
                            }
                            num5 = 1;
                        }
                        if (!flag3)
                        {
                            buf.Add(class8);
                            num11 += class8.len;
                        }
                        if (!flag5)
                        {
                            buf.Add(newb);
                            num11 += newb.len;
                        }
                        this.CSS_insert(class22, class18, class19, class20, class21, align, class4, ref num, buf, ref num11, max, _params, ref flag4, class10, class14, ref num21, ref flag16, ref flag5, newb, list, list2, CSS_tag_type.P_CSS);
                        goto Label_134B7;
                    }
                    if (((class4.b[num] == 60) && (class4.b[num + 1] == 0x21)) && ((class4.b[num + 2] == 0x2d) && (class4.b[num + 3] == 0x2d)))
                    {
                        num += 3;
                        while (num < max)
                        {
                            if (((class4.b[num] == 0x2d) && (class4.b[num + 1] == 0x2d)) && (class4.b[num + 2] == 0x3e))
                            {
                                break;
                            }
                            if (((class4.b[num] == 60) && (class4.b[num + 1] == 0x21)) && ((class4.b[num + 2] == 0x2d) && (class4.b[num + 3] == 0x2d)))
                            {
                                num += 3;
                                while (num < max)
                                {
                                    if (((class4.b[num] == 0x2d) && (class4.b[num + 1] == 0x2d)) && (class4.b[num + 2] == 0x3e))
                                    {
                                        num += 2;
                                        break;
                                    }
                                    num++;
                                }
                            }
                            num++;
                        }
                        num += 2;
                        goto Label_134B7;
                    }
                    if ((((((num5 == 0) && (class4.b[num] == 60)) && ((class4.b[num + 1] == 0x61) || (class4.b[num + 1] == 0x41))) && (((class4.b[num + 2] == 100) || (class4.b[num + 2] == 0x44)) && ((class4.b[num + 3] == 100) || (class4.b[num + 3] == 0x44)))) && ((((class4.b[num + 4] == 0x72) || (class4.b[num + 4] == 0x52)) && ((class4.b[num + 5] == 0x65) || (class4.b[num + 5] == 0x45))) && (((class4.b[num + 6] == 0x73) || (class4.b[num + 6] == 0x53)) && ((class4.b[num + 7] == 0x73) || (class4.b[num + 7] == 0x53))))) && (class4.b[num + 8] == 0x3e))
                    {
                        if (_tablesArray[num36].table)
                        {
                            buf.Add(str15);
                            if ((num36 > 0) && (this._preserveNestedTables == 1))
                            {
                                str11 = @"\itap" + _tablesArray[num36].table_level + " ";
                                buf.Add(str11);
                                num11 += str11.Length;
                            }
                        }
                        else
                        {
                            buf.Add(str14);
                        }
                        num5 = 1;
                        num += 8;
                        if (!flag3)
                        {
                            buf.Add(class8);
                            num11 += class8.len;
                        }
                        if (!flag5)
                        {
                            buf.Add(newb);
                            num11 += newb.len;
                        }
                        goto Label_134B7;
                    }
                    if ((((((num5 == 0) && (class4.b[num] == 60)) && (class4.b[num + 1] == 0x2f)) && (((class4.b[num + 2] == 0x61) || (class4.b[num + 2] == 0x41)) && ((class4.b[num + 3] == 100) || (class4.b[num + 3] == 0x44)))) && ((((class4.b[num + 4] == 100) || (class4.b[num + 4] == 0x44)) && ((class4.b[num + 5] == 0x72) || (class4.b[num + 5] == 0x52))) && (((class4.b[num + 6] == 0x65) || (class4.b[num + 6] == 0x45)) && ((class4.b[num + 7] == 0x73) || (class4.b[num + 7] == 0x53))))) && (((class4.b[num + 8] == 0x73) || (class4.b[num + 8] == 0x53)) && (class4.b[num + 9] == 0x3e)))
                    {
                        if (_tablesArray[num36].table)
                        {
                            buf.Add(str15);
                            if ((num36 > 0) && (this._preserveNestedTables == 1))
                            {
                                str11 = @"\itap" + _tablesArray[num36].table_level + " ";
                                buf.Add(str11);
                                num11 += str11.Length;
                            }
                        }
                        else
                        {
                            buf.Add(str14);
                        }
                        num5 = 1;
                        num += 9;
                        if (!flag3)
                        {
                            buf.Add(class8);
                            num11 += class8.len;
                        }
                        if (!flag5)
                        {
                            buf.Add(newb);
                            num11 += newb.len;
                        }
                        goto Label_134B7;
                    }
                    if ((((class4.b[num] == 60) && ((class4.b[num + 1] == 0x66) || (class4.b[num + 1] == 70))) && (((class4.b[num + 2] == 0x6f) || (class4.b[num + 2] == 0x4f)) && ((class4.b[num + 3] == 110) || (class4.b[num + 3] == 0x4e)))) && (((class4.b[num + 4] == 0x74) || (class4.b[num + 4] == 0x54)) && ((class4.b[num + 5] == 0x3e) || this.IS_DELIMITER(class4.b[num + 5]))))
                    {
                        index = num + 5;
                        flag6 = false;
                        flag7 = false;
                        if (this.IS_DELIMITER(class4.b[num + 5]))
                        {
                            while ((class4.b[num] != 0x3e) && (num < max))
                            {
                                if ((((this._preserveFontFace == 1) && ((class4.b[num] == 0x66) || (class4.b[num] == 70))) && (((class4.b[num + 1] == 0x61) || (class4.b[num + 1] == 0x41)) && ((class4.b[num + 2] == 0x63) || (class4.b[num + 2] == 0x43)))) && (((class4.b[num + 3] == 0x65) || (class4.b[num + 3] == 0x45)) && (((class4.b[num + 4] == 0x3d) || (class4.b[num + 4] == 0x3a)) || (class4.b[num + 4] == 0x20))))
                                {
                                    num7 = 0;
                                    class8.Clear();
                                    if (!flag3 && (num20 < this.STK_MAX))
                                    {
                                        class13.Add((byte)this.ToInt(class8));
                                        num20++;
                                    }
                                    num += 5;
                                    this.ReadValue(class4, ref num, class8);
                                    for (num7 = 0; num7 < num13; num7++)
                                    {
                                        if (class8.byteCmpi(list[num7].ToString()) == 0)
                                        {
                                            flag3 = false;
                                            class8.Clear();
                                            class8.Add(@"\f" + ((num7 + 2)).ToString() + " ");
                                            flag16 = true;
                                        }
                                    }
                                }
                                if ((((this._preserveFontSize == 1) && ((class4.b[num] == 0x73) || (class4.b[num] == 0x53))) && (((class4.b[num + 1] == 0x69) || (class4.b[num + 1] == 0x49)) && ((class4.b[num + 2] == 0x7a) || (class4.b[num + 2] == 90)))) && (((class4.b[num + 3] == 0x65) || (class4.b[num + 3] == 0x45)) && (((class4.b[num + 4] == 0x3d) || (class4.b[num + 4] == 0x3a)) || (class4.b[num + 4] == 0x20))))
                                {
                                    num += 5;
                                    this.read_value_CSS_tolower(class4, ref num, max, newb, this.MAX_STYLE_NAME_LENGTH);
                                    num18 = newb.ByteToInt();
                                    num15 = num18;
                                    if (newb.IndexOf("px") != -1)
                                    {
                                        num18 = (int)(num18 * 0.75);
                                    }
                                    else if (newb.IndexOf("em") != -1)
                                    {
                                        num18 *= 12;
                                    }
                                    else if (newb.IndexOf("pt") != -1)
                                    {
                                        num18 = num18;
                                    }
                                    else if (newb.IndexOf("xx-small") != -1)
                                    {
                                        num18 = 8;
                                    }
                                    else if (newb.IndexOf("x-small") != -1)
                                    {
                                        num18 = 10;
                                    }
                                    else if (newb.IndexOf("small") != -1)
                                    {
                                        num18 = 12;
                                    }
                                    else if (newb.IndexOf("medium") != -1)
                                    {
                                        num18 = 14;
                                    }
                                    else if (newb.IndexOf("large") != -1)
                                    {
                                        num18 = 0x12;
                                    }
                                    else if (newb.IndexOf("x-large") != -1)
                                    {
                                        num18 = 0x18;
                                    }
                                    else if (newb.IndexOf("xx-large") != -1)
                                    {
                                        num18 = 0x24;
                                    }
                                    else if (newb.IndexOf("%") != -1)
                                    {
                                        num18 = (num15 * 12) / 100;
                                    }
                                    else
                                    {
                                        if (newb.b[0] == 0x2b)
                                        {
                                            num15 += num16;
                                            if (num15 < 0)
                                            {
                                                num15 = 0;
                                            }
                                            if (num15 > 7)
                                            {
                                                num15 = 7;
                                            }
                                        }
                                        if (newb.b[0] == 0x2d)
                                        {
                                            num15 = num16 - Math.Abs(num15);
                                            if (num15 < 0)
                                            {
                                                num15 = 0;
                                            }
                                            if (num15 > 7)
                                            {
                                                num15 = 7;
                                            }
                                        }
                                        if ((num15 >= 0) && (num15 <= 7))
                                        {
                                            num18 = numArray2[num15];
                                        }
                                    }
                                    flag5 = false;
                                    newb.Clear();
                                    newb.Add(@"\fs" + ((2 * num18)).ToString() + " ");
                                    if (num22 < this.STK_MAX)
                                    {
                                        numArray3[num22] = num18;
                                    }
                                    flag16 = true;
                                    flag7 = true;
                                    num--;
                                }
                                if ((((((this._preserveBackgroundColor == 1) && ((class4.b[num] == 0x62) || (class4.b[num] == 0x42))) && (((class4.b[num + 1] == 0x61) || (class4.b[num + 1] == 0x41)) && ((class4.b[num + 2] == 0x63) || (class4.b[num + 2] == 0x43)))) && ((((class4.b[num + 3] == 0x6b) || (class4.b[num + 3] == 0x4b)) && ((class4.b[num + 4] == 0x67) || (class4.b[num + 4] == 0x47))) && (((class4.b[num + 5] == 0x72) || (class4.b[num + 5] == 0x52)) && ((class4.b[num + 6] == 0x6f) || (class4.b[num + 6] == 0x4f))))) && (((((class4.b[num + 7] == 0x75) || (class4.b[num + 7] == 0x55)) && ((class4.b[num + 8] == 110) || (class4.b[num + 8] == 0x4e))) && (((class4.b[num + 9] == 100) || (class4.b[num + 9] == 0x44)) && (class4.b[num + 10] == 0x2d))) && ((((class4.b[num + 11] == 0x63) || (class4.b[num + 11] == 0x43)) && ((class4.b[num + 12] == 0x6f) || (class4.b[num + 12] == 0x4f))) && (((class4.b[num + 13] == 0x6c) || (class4.b[num + 13] == 0x4c)) && ((class4.b[num + 14] == 0x6f) || (class4.b[num + 14] == 0x4f)))))) && (((class4.b[num + 15] == 0x72) || (class4.b[num + 15] == 0x52)) && (class4.b[num + 0x10] == 0x3a)))
                                {
                                    num += 0x11;
                                    this.read_color(class4, ref num, max, class12);
                                    for (num7 = 0; num7 < num14; num7++)
                                    {
                                        if (class12.byteCmpi(list2[num7].ToString()) == 0)
                                        {
                                            class12.Clear();
                                            class12.Add(@"\chcbpat" + ((num7 + 3)).ToString() + " ");
                                            buf.Add(class12);
                                            num11 += class12.len;
                                            num19 = 1;
                                        }
                                    }
                                }
                                if ((((this._preserveFontColor == 1) && ((class4.b[num] == 0x63) || (class4.b[num] == 0x43))) && (((class4.b[num + 1] == 0x6f) || (class4.b[num + 1] == 0x4f)) && ((class4.b[num + 2] == 0x6c) || (class4.b[num + 2] == 0x4c)))) && ((((class4.b[num + 3] == 0x6f) || (class4.b[num + 3] == 0x4f)) && ((class4.b[num + 4] == 0x72) || (class4.b[num + 4] == 0x52))) && (class4.b[num + 5] == 0x3d)))
                                {
                                    num += 6;
                                    this.ToInt(class10);
                                    this.read_color(class4, ref num, max, class10);
                                    for (num7 = 0; num7 < num14; num7++)
                                    {
                                        if (class10.byteCmpi(list2[num7].ToString()) == 0)
                                        {
                                            class10.Clear();
                                            class10.Add(@"\cf" + ((num7 + 3)).ToString() + " ");
                                            flag16 = true;
                                            if (num21 < this.STK_MAX)
                                            {
                                                class14.b[num21] = (byte)(num7 + 3);
                                            }
                                            flag4 = false;
                                            flag6 = true;
                                            break;
                                        }
                                    }
                                }
                                num++;
                            }
                        }
                        else
                        {
                            num += 5;
                        }
                        if (!flag3)
                        {
                            buf.Add(class8);
                            num11 += class8.len;
                        }
                        if (!flag4)
                        {
                            buf.Add(class10);
                            num11 += class10.len;
                            if (!flag6 && (num21 < this.STK_MAX))
                            {
                                class14.b[num21] = class14.b[num21 - 1];
                            }
                            num21++;
                        }
                        else
                        {
                            class14.Add((byte)1);
                            num21++;
                            flag4 = false;
                        }
                        if (!flag5)
                        {
                            buf.Add(newb);
                            num11 += newb.len;
                            if (!flag7 && (num22 < this.STK_MAX))
                            {
                                numArray3[num22] = numArray3[num22 - 1];
                            }
                            num22++;
                        }
                        num = index;
                        this.CSS_insert(class22, class18, class19, class20, class21, align, class4, ref num, buf, ref num11, max, _params, ref flag4, class10, class14, ref num21, ref flag16, ref flag5, newb, list, list2, CSS_tag_type.FONT_CSS);
                        goto Label_134B7;
                    }
                    if (((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x66) || (class4.b[num + 2] == 70))) && (((class4.b[num + 3] == 0x6f) || (class4.b[num + 3] == 0x4f)) && ((class4.b[num + 4] == 110) || (class4.b[num + 4] == 0x4e)))) && (((class4.b[num + 5] == 0x74) || (class4.b[num + 5] == 0x54)) && (class4.b[num + 6] == 0x3e)))
                    {
                        if ((this._preserveBackgroundColor == 1) && (num19 == 1))
                        {
                            buf.Add(@"\chcbpat0 ");
                            num11 += 10;
                            num19 = 0;
                        }
                        if (num20 > 0)
                        {
                            class8.Clear();
                            class8.Add(@"\f" + class13.b[num20 - 1] + " ");
                            flag16 = true;
                            num20--;
                        }
                        else
                        {
                            class8.Clear();
                            class8.Add(@"\f0 ");
                            flag16 = true;
                            flag3 = true;
                        }
                        if (num21 > 0)
                        {
                            num21--;
                            if (num21 == 0)
                            {
                                class10.Clear();
                                class10.Add(@"\cf1 ");
                                flag16 = true;
                                flag4 = true;
                            }
                            else if (num21 > 0)
                            {
                                class10.Clear();
                                class10.Add(@"\cf" + class14.b[num21 - 1] + " ");
                            }
                            flag16 = true;
                        }
                        else
                        {
                            class10.Clear();
                            class10.Add(@"\cf1 ");
                            flag16 = true;
                            flag4 = true;
                        }
                        if (num22 > 0)
                        {
                            num22--;
                            newb.Clear();
                            if (num22 > 0)
                            {
                                newb.Add(@"\fs" + ((numArray3[num22 - 1] * 2)).ToString() + " ");
                                flag16 = true;
                            }
                        }
                        else
                        {
                            newb.Clear();
                            newb.Add(@"\fs" + (fontsize * 2) + " ");
                            flag16 = true;
                            flag5 = true;
                        }
                        flag5 = true;
                        num += 6;
                        buf.Add(class10);
                        num11 += class10.len;
                        buf.Add(class8);
                        num11 += class8.len;
                        buf.Add(newb);
                        num11 += newb.len;
                        this.CSS_close(class22, class18, align, buf, ref num11, _params, ref flag4, class10, class14, ref num21, ref flag16, ref flag5, newb, fontsize, CSS_tag_type.FONT_CSS);
                        goto Label_134B7;
                    }
                    if ((((class4.b[num] == 60) && ((class4.b[num + 1] == 0x73) || (class4.b[num + 1] == 0x53))) && (((class4.b[num + 2] == 0x70) || (class4.b[num + 2] == 80)) && ((class4.b[num + 3] == 0x61) || (class4.b[num + 3] == 0x41)))) && (((class4.b[num + 4] == 110) || (class4.b[num + 4] == 0x4e)) && this.IS_DELIMITER(class4.b[num + 5])))
                    {
                        num += 5;
                        this.CSS_insert(class22, class18, class19, class20, class21, align, class4, ref num, buf, ref num11, max, _params, ref flag4, class10, class14, ref num21, ref flag16, ref flag5, newb, list, list2, CSS_tag_type.SPAN_CSS);
                        goto Label_134B7;
                    }
                    if (((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x73) || (class4.b[num + 2] == 0x53))) && (((class4.b[num + 3] == 0x70) || (class4.b[num + 3] == 80)) && ((class4.b[num + 4] == 0x61) || (class4.b[num + 4] == 0x41)))) && (((class4.b[num + 5] == 110) || (class4.b[num + 5] == 0x4e)) && (class4.b[num + 6] == 0x3e)))
                    {
                        num += 6;
                        this.CSS_close(class22, class18, align, buf, ref num11, _params, ref flag4, class10, class14, ref num21, ref flag16, ref flag5, newb, fontsize, CSS_tag_type.SPAN_CSS);
                        goto Label_134B7;
                    }
                    if ((((class4.b[num] == 60) && ((class4.b[num + 1] == 0x69) || (class4.b[num + 1] == 0x49))) && (((class4.b[num + 2] == 110) || (class4.b[num + 2] == 0x4e)) && ((class4.b[num + 3] == 0x70) || (class4.b[num + 3] == 80)))) && ((((class4.b[num + 4] == 0x75) || (class4.b[num + 4] == 0x55)) && ((class4.b[num + 5] == 0x74) || (class4.b[num + 5] == 0x54))) && this.IS_DELIMITER(class4.b[num + 6])))
                    {
                        num += 6;
                        bool flag22 = false;
                        int num62 = 0;
                        while ((class4.b[num] != 0x3e) && (num < class4.len))
                        {
                            if (((((class4.b[num] == 0x74) || (class4.b[num] == 0x54)) && ((class4.b[num + 1] == 0x79) || (class4.b[num + 1] == 0x59))) && (((class4.b[num + 2] == 0x70) || (class4.b[num + 2] == 80)) && ((class4.b[num + 3] == 0x65) || (class4.b[num + 3] == 0x45)))) && (this.IS_DELIMITER(class4.b[num + 4]) || (class4.b[num + 4] == 0x3d)))
                            {
                                valueStr.Clear();
                                num += 4;
                                this.ReadValue(class4, ref num, valueStr);
                                if (valueStr.byteCmpi("checkbox") == 0)
                                {
                                    num62 = 1;
                                }
                                else if (valueStr.byteCmpi("radio") == 0)
                                {
                                    num62 = 2;
                                }
                            }
                            if (((this.IS_DELIMITER(class4.b[num]) && ((class4.b[num + 1] == 0x63) || (class4.b[num + 1] == 0x43))) && (((class4.b[num + 2] == 0x68) || (class4.b[num + 2] == 0x48)) && ((class4.b[num + 3] == 0x65) || (class4.b[num + 3] == 0x45)))) && ((((class4.b[num + 4] == 0x63) || (class4.b[num + 4] == 0x43)) && ((class4.b[num + 5] == 0x6b) || (class4.b[num + 5] == 0x4b))) && (((class4.b[num + 6] == 0x65) || (class4.b[num + 6] == 0x45)) && ((class4.b[num + 7] == 100) || (class4.b[num + 7] == 0x44)))))
                            {
                                flag22 = true;
                            }
                            num++;
                        }
                        if (num62 != 0)
                        {
                            if (num62 == 1)
                            {
                                if (flag22)
                                {
                                    buf.Add(str3);
                                }
                                else
                                {
                                    buf.Add(str4);
                                }
                            }
                            else if (num62 == 2)
                            {
                                if (flag22)
                                {
                                    buf.Add(str5);
                                }
                                else
                                {
                                    buf.Add(str6);
                                }
                            }
                        }
                        goto Label_134B7;
                    }
                    if (((class4.b[num] == 60) && ((class4.b[num + 1] == 0x62) || (class4.b[num + 1] == 0x42))) && (((class4.b[num + 2] == 0x72) || (class4.b[num + 2] == 0x52)) && (((class4.b[num + 3] == 0x3e) || (class4.b[num + 3] == 0x20)) || (class4.b[num + 3] == 0x2f))))
                    {
                        if ((class4.b[num + 3] == 0x20) || (class4.b[num + 3] == 0x2f))
                        {
                            while ((class4.b[num + 3] != 0x3e) && (num < max))
                            {
                                num++;
                            }
                        }
                        num5 = 1;
                        if ((num55 > 0) || (num56 > 0))
                        {
                            buf.Add(this.BR_STR_BULLETS);
                            num11 += this.BR_STR_BULLETS.Length;
                            num5 = 0;
                        }
                        else if (_tablesArray[num36].table)
                        {
                            if (_tablesArray[num36].cell > 0)
                            {
                                buf.Add(str18);
                                if ((num36 > 0) && (this._preserveNestedTables == 1))
                                {
                                    if (!_tablesArray[num36].tr_open && (_tablesArray[num36].table_level > 1))
                                    {
                                        str11 = @"\itap" + ((_tablesArray[num36].table_level - 1)).ToString() + " ";
                                    }
                                    else
                                    {
                                        str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
                                    }
                                    buf.Add(str11);
                                    num11 += str11.Length;
                                }
                            }
                        }
                        else
                        {
                            buf.Add(str17);
                        }
                        num += 3;
                        if (!flag3)
                        {
                            buf.Add(class8);
                            num11 += class8.len;
                        }
                        if (!flag5)
                        {
                            buf.Add(newb);
                            num11 += newb.len;
                        }
                        buf.Add(align);
                        num11 += len;
                        goto Label_134B7;
                    }
                    if (((class4.b[num] == 60) && ((class4.b[num + 1] == 0x75) || (class4.b[num + 1] == 0x55))) && (((class4.b[num + 2] == 0x6c) || (class4.b[num + 2] == 0x4c)) && ((class4.b[num + 3] == 0x3e) || (class4.b[num + 3] == 0x20))))
                    {
                        num += 3;
                        if (class4.b[num] == 0x20)
                        {
                            while ((class4.b[num] != 0x3e) && (num < max))
                            {
                                if (((((class4.b[num] == 0x63) || (class4.b[num] == 0x43)) && ((class4.b[num + 1] == 0x6c) || (class4.b[num + 1] == 0x4c))) && (((class4.b[num + 2] == 0x61) || (class4.b[num + 2] == 0x41)) && ((class4.b[num + 3] == 0x73) || (class4.b[num + 3] == 0x53)))) && (((class4.b[num + 4] == 0x73) || (class4.b[num + 4] == 0x53)) && (class4.b[num + 5] == 0x3d)))
                                {
                                    num += 5;
                                    _params.found = false;
                                    this.read_value_exact(class4, ref num, max, _params.style_name);
                                    if (_params.style_name.len > 0)
                                    {
                                        _params.found = true;
                                    }
                                    if (_params.found)
                                    {
                                        _params.found = false;
                                        for (num7 = 0; num7 < _params.styles; num7++)
                                        {
                                            if ((((CSS_styles)_params.CSS_style[num7]).name.byteCmp(_params.style_name.ByteToString()) == 0) && ((((CSS_styles)_params.CSS_style[num7]).css_tag_type == CSS_tag_type.UL_CSS) || (((CSS_styles)_params.CSS_style[num7]).css_tag_type == CSS_tag_type.UNKNOWN_CSS)))
                                            {
                                                _params.found = true;
                                                if (((CSS_styles)_params.CSS_style[num7]).list_style_type == 1)
                                                {
                                                    typeArray[num55] = listStyleType.LIST_NONE;
                                                }
                                                else if (((CSS_styles)_params.CSS_style[num7]).list_style_type == 2)
                                                {
                                                    typeArray[num55] = listStyleType.LIST_UL_DISC;
                                                }
                                                else if (((CSS_styles)_params.CSS_style[num7]).list_style_type == 3)
                                                {
                                                    typeArray[num55] = listStyleType.LIST_UL_CIRCLE;
                                                }
                                                else if (((CSS_styles)_params.CSS_style[num7]).list_style_type == 4)
                                                {
                                                    typeArray[num55] = listStyleType.LIST_UL_SQUARE;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                                if ((((((class4.b[num] == 0x6c) || (class4.b[num] == 0x4c)) && ((class4.b[num + 1] == 0x69) || (class4.b[num + 1] == 0x49))) && (((class4.b[num + 2] == 0x73) || (class4.b[num + 2] == 0x53)) && ((class4.b[num + 3] == 0x74) || (class4.b[num + 3] == 0x54)))) && (((class4.b[num + 4] == 0x2d) && ((class4.b[num + 5] == 0x73) || (class4.b[num + 5] == 0x53))) && (((class4.b[num + 6] == 0x74) || (class4.b[num + 6] == 0x54)) && ((class4.b[num + 7] == 0x79) || (class4.b[num + 7] == 0x59))))) && ((((class4.b[num + 8] == 0x6c) || (class4.b[num + 8] == 0x4c)) && ((class4.b[num + 9] == 0x65) || (class4.b[num + 9] == 0x45))) && (class4.b[num + 10] == 0x3a)))
                                {
                                    num += 11;
                                    this.read_value_CSS(class4, ref num, max, class16, -1111, false);
                                    if (class16.byteCmpi("none") == 0)
                                    {
                                        typeArray[num55] = listStyleType.LIST_NONE;
                                    }
                                    else
                                    {
                                        if (class16.byteCmpi("disc") == 0)
                                        {
                                            typeArray[num55] = listStyleType.LIST_UL_DISC;
                                            continue;
                                        }
                                        if (class16.byteCmpi("circle") == 0)
                                        {
                                            typeArray[num55] = listStyleType.LIST_UL_CIRCLE;
                                        }
                                        else if (class16.byteCmpi("square") == 0)
                                        {
                                            typeArray[num55] = listStyleType.LIST_UL_SQUARE;
                                        }
                                    }
                                }
                                else
                                {
                                    if ((((((class4.b[num] == 0x6c) || (class4.b[num] == 0x4c)) && ((class4.b[num + 1] == 0x69) || (class4.b[num + 1] == 0x49))) && (((class4.b[num + 2] == 0x73) || (class4.b[num + 2] == 0x53)) && ((class4.b[num + 3] == 0x74) || (class4.b[num + 3] == 0x54)))) && (((class4.b[num + 4] == 0x2d) && ((class4.b[num + 5] == 0x73) || (class4.b[num + 5] == 0x53))) && (((class4.b[num + 6] == 0x74) || (class4.b[num + 6] == 0x54)) && ((class4.b[num + 7] == 0x79) || (class4.b[num + 7] == 0x59))))) && (((((class4.b[num + 8] == 0x6c) || (class4.b[num + 8] == 0x4c)) && ((class4.b[num + 9] == 0x65) || (class4.b[num + 9] == 0x45))) && ((class4.b[num + 10] == 0x2d) && ((class4.b[num + 11] == 0x74) || (class4.b[num + 11] == 0x54)))) && ((((class4.b[num + 12] == 0x79) || (class4.b[num + 12] == 0x59)) && ((class4.b[num + 13] == 0x70) || (class4.b[num + 13] == 80))) && (((class4.b[num + 14] == 0x65) || (class4.b[num + 14] == 0x45)) && (class4.b[num + 15] == 0x3a)))))
                                    {
                                        num += 0x10;
                                        this.read_value_CSS(class4, ref num, max, class16, -1111, false);
                                        if (class16.byteCmpi("none") == 0)
                                        {
                                            typeArray[num55] = listStyleType.LIST_NONE;
                                        }
                                        else if (class16.byteCmpi("disc") == 0)
                                        {
                                            typeArray[num55] = listStyleType.LIST_UL_DISC;
                                        }
                                        else if (class16.byteCmpi("circle") == 0)
                                        {
                                            typeArray[num55] = listStyleType.LIST_UL_CIRCLE;
                                        }
                                        else if (class16.byteCmpi("square") == 0)
                                        {
                                            typeArray[num55] = listStyleType.LIST_UL_SQUARE;
                                        }
                                        continue;
                                    }
                                    if ((((class4.b[num] == 0x74) || (class4.b[num] == 0x54)) && ((class4.b[num + 1] == 0x79) || (class4.b[num + 1] == 0x59))) && (((class4.b[num + 2] == 0x70) || (class4.b[num + 2] == 80)) && ((class4.b[num + 3] == 0x65) || (class4.b[num + 3] == 0x45))))
                                    {
                                        num += 4;
                                        this.read_value_CSS(class4, ref num, max, class16, -1111, false);
                                        if (class16.byteCmpi("none") == 0)
                                        {
                                            typeArray[num55] = listStyleType.LIST_NONE;
                                        }
                                        else if (class16.byteCmpi("disc") == 0)
                                        {
                                            typeArray[num55] = listStyleType.LIST_UL_DISC;
                                        }
                                        else if (class16.byteCmpi("circle") == 0)
                                        {
                                            typeArray[num55] = listStyleType.LIST_UL_CIRCLE;
                                        }
                                        else if (class16.byteCmpi("square") == 0)
                                        {
                                            typeArray[num55] = listStyleType.LIST_UL_SQUARE;
                                        }
                                        continue;
                                    }
                                    num++;
                                }
                            }
                        }
                        num55++;
                        numArray4[num60] = 0;
                        if (num60 < (this.STK_MAX - 1))
                        {
                            num60++;
                        }
                        if (num5 == 0)
                        {
                            if (_tablesArray[num36].table)
                            {
                                buf.Add(str15);
                                if ((num36 > 0) && (this._preserveNestedTables == 1))
                                {
                                    str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
                                    buf.Add(str11);
                                    num11 += str11.Length;
                                }
                            }
                            else
                            {
                                buf.Add(str14);
                                num5 = 1;
                            }
                        }
                        num59 = 0;
                        goto Label_134B7;
                    }
                    if ((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x75) || (class4.b[num + 2] == 0x55))) && (((class4.b[num + 3] == 0x6c) || (class4.b[num + 3] == 0x4c)) && (class4.b[num + 4] == 0x3e)))
                    {
                        num55--;
                        if (num55 < 0)
                        {
                            num55 = 0;
                        }
                        if (num60 >= 1)
                        {
                            num60--;
                        }
                        if (num60 == 0)
                        {
                            num59 = 0;
                        }
                        else
                        {
                            num59 = numArray4[num60 - 1];
                        }
                        typeArray[num55] = listStyleType.LIST_STANDARD;
                        num += 4;
                        if (_tablesArray[num36].table)
                        {
                            buf.Add(str15);
                            if ((num36 > 0) && (this._preserveNestedTables == 1))
                            {
                                str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
                                buf.Add(str11);
                                num11 += str11.Length;
                            }
                        }
                        else if (num5 == 0)
                        {
                            buf.Add(str14);
                            num5 = 1;
                        }
                        goto Label_134B7;
                    }
                    if (((class4.b[num] == 60) && ((class4.b[num + 1] == 0x6f) || (class4.b[num + 1] == 0x4f))) && (((class4.b[num + 2] == 0x6c) || (class4.b[num + 2] == 0x4c)) && ((class4.b[num + 3] == 0x3e) || (class4.b[num + 3] == 0x20))))
                    {
                        num58 = 0;
                        num += 3;
                        if (class4.b[num] == 0x20)
                        {
                            while ((class4.b[num] != 0x3e) && (num < max))
                            {
                                if (((((class4.b[num] == 0x74) || (class4.b[num] == 0x54)) && ((class4.b[num + 1] == 0x79) || (class4.b[num + 1] == 0x59))) && (((class4.b[num + 2] == 0x70) || (class4.b[num + 2] == 80)) && ((class4.b[num + 3] == 0x65) || (class4.b[num + 3] == 0x45)))) && (class4.b[num + 4] == 0x3d))
                                {
                                    num += 4;
                                    this.read_value_exact(class4, ref num, max, class17);
                                    if (class17.byteCmp("i") == 0)
                                    {
                                        typeArray[num56] = listStyleType.LIST_OL_LOWER_ROMAN;
                                    }
                                    else if (class17.byteCmp("I") == 0)
                                    {
                                        typeArray[num56] = listStyleType.LIST_OL_UPPER_ROMAN;
                                    }
                                    else if (class17.byteCmp("a") == 0)
                                    {
                                        typeArray[num56] = listStyleType.LIST_OL_LOWER_ALPHA;
                                    }
                                    else if (class17.byteCmp("A") == 0)
                                    {
                                        typeArray[num56] = listStyleType.LIST_OL_UPPER_ALPHA;
                                    }
                                    else if (class17.byteCmp("1") == 0)
                                    {
                                        typeArray[num56] = listStyleType.LIST_STANDARD;
                                    }
                                }
                                if (((((class4.b[num] == 0x63) || (class4.b[num] == 0x43)) && ((class4.b[num + 1] == 0x6c) || (class4.b[num + 1] == 0x4c))) && (((class4.b[num + 2] == 0x61) || (class4.b[num + 2] == 0x41)) && ((class4.b[num + 3] == 0x73) || (class4.b[num + 3] == 0x53)))) && (((class4.b[num + 4] == 0x73) || (class4.b[num + 4] == 0x53)) && (class4.b[num + 5] == 0x3d)))
                                {
                                    num += 5;
                                    _params.found = false;
                                    this.read_value_exact(class4, ref num, max, _params.style_name);
                                    if (_params.style_name.len > 0)
                                    {
                                        _params.found = true;
                                    }
                                    if (_params.found)
                                    {
                                        _params.found = false;
                                        for (num7 = 0; num7 < _params.styles; num7++)
                                        {
                                            if ((((CSS_styles)_params.CSS_style[num7]).name.byteCmp(_params.style_name) == 0) && ((((CSS_styles)_params.CSS_style[num7]).css_tag_type == CSS_tag_type.OL_CSS) || (((CSS_styles)_params.CSS_style[num7]).css_tag_type == CSS_tag_type.UNKNOWN_CSS)))
                                            {
                                                _params.found = true;
                                                if (((CSS_styles)_params.CSS_style[num7]).list_style_type == 1)
                                                {
                                                    typeArray[num56] = listStyleType.LIST_NONE;
                                                }
                                                else if (((CSS_styles)_params.CSS_style[num7]).list_style_type == 5)
                                                {
                                                    typeArray[num56] = listStyleType.LIST_OL_LOWER_ROMAN;
                                                }
                                                else if (((CSS_styles)_params.CSS_style[num7]).list_style_type == 6)
                                                {
                                                    typeArray[num56] = listStyleType.LIST_OL_UPPER_ROMAN;
                                                }
                                                else if (((CSS_styles)_params.CSS_style[num7]).list_style_type == 7)
                                                {
                                                    typeArray[num56] = listStyleType.LIST_OL_LOWER_ALPHA;
                                                }
                                                else if (((CSS_styles)_params.CSS_style[num7]).list_style_type == 8)
                                                {
                                                    typeArray[num56] = listStyleType.LIST_OL_UPPER_ALPHA;
                                                }
                                                else if (((CSS_styles)_params.CSS_style[num7]).list_style_type == 9)
                                                {
                                                    typeArray[num56] = listStyleType.LIST_STANDARD;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                                if ((((((class4.b[num] == 0x6c) || (class4.b[num] == 0x4c)) && ((class4.b[num + 1] == 0x69) || (class4.b[num + 1] == 0x49))) && (((class4.b[num + 2] == 0x73) || (class4.b[num + 2] == 0x53)) && ((class4.b[num + 3] == 0x74) || (class4.b[num + 3] == 0x54)))) && (((class4.b[num + 4] == 0x2d) && ((class4.b[num + 5] == 0x73) || (class4.b[num + 5] == 0x53))) && (((class4.b[num + 6] == 0x74) || (class4.b[num + 6] == 0x54)) && ((class4.b[num + 7] == 0x79) || (class4.b[num + 7] == 0x59))))) && ((((class4.b[num + 8] == 0x6c) || (class4.b[num + 8] == 0x4c)) && ((class4.b[num + 9] == 0x65) || (class4.b[num + 9] == 0x45))) && (class4.b[num + 10] == 0x3a)))
                                {
                                    num += 11;
                                    this.read_value_CSS(class4, ref num, max, class16, -1111, false);
                                    if (class16.byteCmpi("none") == 0)
                                    {
                                        typeArray[num56] = listStyleType.LIST_NONE;
                                    }
                                }
                                else
                                {
                                    num++;
                                }
                            }
                        }
                        num56++;
                        num57 = 1;
                        numArray4[num60] = 1;
                        if (num60 < (this.STK_MAX - 1))
                        {
                            num60++;
                        }
                        if (num5 == 0)
                        {
                            if (_tablesArray[num36].table)
                            {
                                buf.Add(str15);
                                if ((num36 > 0) && (this._preserveNestedTables == 1))
                                {
                                    str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
                                    buf.Add(str11);
                                    num11 += str11.Length;
                                }
                            }
                            else
                            {
                                buf.Add(str14);
                                num5 = 1;
                            }
                        }
                        num59 = 1;
                        goto Label_134B7;
                    }
                    if ((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x6f) || (class4.b[num + 2] == 0x4f))) && (((class4.b[num + 3] == 0x6c) || (class4.b[num + 3] == 0x4c)) && (class4.b[num + 4] == 0x3e)))
                    {
                        num58 = 0;
                        if (num60 >= 1)
                        {
                            num60--;
                        }
                        if (num60 == 0)
                        {
                            num59 = 0;
                        }
                        else
                        {
                            num59 = numArray4[num60 - 1];
                        }
                        num56--;
                        if (num56 < 0)
                        {
                            num56 = 0;
                        }
                        typeArray[num56] = listStyleType.LIST_STANDARD;
                        num += 4;
                        if (_tablesArray[num36].table)
                        {
                            buf.Add(str15);
                            if ((num36 > 0) && (this._preserveNestedTables == 1))
                            {
                                str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
                                buf.Add(str11);
                                num11 += str11.Length;
                            }
                        }
                        else if (num5 == 0)
                        {
                            buf.Add(str14);
                            num5 = 1;
                        }
                        goto Label_134B7;
                    }
                    if (((class4.b[num] == 60) && ((class4.b[num + 1] == 0x6c) || (class4.b[num + 1] == 0x4c))) && (((class4.b[num + 2] == 0x69) || (class4.b[num + 2] == 0x49)) && ((class4.b[num + 3] == 0x3e) || (class4.b[num + 3] == 0x20))))
                    {
                        num58++;
                        if (class4.b[num + 3] == 0x20)
                        {
                            while ((class4.b[num + 3] != 0x3e) && (num < max))
                            {
                                num++;
                            }
                        }
                        if (num5 == 0)
                        {
                            if (_tablesArray[num36].table)
                            {
                                buf.Add(str18);
                                if ((num36 > 0) && (this._preserveNestedTables == 1))
                                {
                                    str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
                                    buf.Add(str11);
                                    num11 += str11.Length;
                                }
                            }
                            else
                            {
                                buf.Add(str17);
                            }
                        }
                        if ((num55 > 0) && (num59 == 0))
                        {
                            if (typeArray[num55 - 1] == listStyleType.LIST_UL_DISC)
                            {
                                num61 = 100;
                            }
                            else if (typeArray[num55 - 1] == listStyleType.LIST_UL_SQUARE)
                            {
                                num61 = 0x65;
                            }
                            else if (typeArray[num55 - 1] == listStyleType.LIST_UL_CIRCLE)
                            {
                                num61 = 0x66;
                            }
                            string str24 = _tablesArray[num36].table ? @"\pard\intbl" : string.Concat(strArray = new string[9]);
                            buf.Add(str24);
                            num11 += str24.Length;
                            if ((_tablesArray[num36].table && (num36 > 0)) && (this._preserveNestedTables == 1))
                            {
                                str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
                                buf.Add(str11);
                                num11 += str11.Length;
                            }
                        }
                        else if ((num56 > 0) && (num59 == 1))
                        {
                            this.MakeOLsymbol(num58, strOLsymbol, (int)typeArray[num56 - 1]);
                            strArray = new string[] { _tablesArray[num36].table ? @"\pard\intbl" : "", @"{\*\pn\pnlvlbody\pnf0\pnindent0\pnstart1", (typeArray[num56 - 1] == listStyleType.LIST_STANDARD) ? @"\pndec" : "", @"{\pntxta ", strOLsymbol.ByteToString(), @".}}\fi-240\li", ((400 * num56) * ((num55 > 0) ? (num55 + 1) : 1)).ToString(), @"{\pntext}" };
                            string str26 = string.Concat(strArray);
                            buf.Add(str26);
                            num11 += str26.Length;
                            if ((_tablesArray[num36].table && (num36 > 0)) && (this._preserveNestedTables == 1))
                            {
                                str11 = @"\itap" + _tablesArray[num36].table_level + " ";
                                buf.Add(str11);
                                num11 += str11.Length;
                            }
                            num57++;
                        }
                        num += 3;
                        num5 = 1;
                        if (!flag3)
                        {
                            buf.Add(class8);
                            num11 += class8.len;
                        }
                        if (!flag5)
                        {
                            buf.Add(newb);
                            num11 += newb.len;
                        }
                        goto Label_134B7;
                    }
                    if (((class4.b[num] == 60) && ((class4.b[num + 1] == 0x68) || (class4.b[num + 1] == 0x48))) && (((class4.b[num + 2] >= 0x30) && (class4.b[num + 2] <= 0x39)) && ((class4.b[num + 3] == 0x3e) || (class4.b[num + 3] == 0x20))))
                    {
                        if (flag12)
                        {
                            if (num34 < 2)
                            {
                                num34++;
                                num5 = 1;
                            }
                            else
                            {
                                num34 = 1;
                                num5 = 0;
                                buf.Add(str21);
                                num11 += num51;
                            }
                        }
                        if (num5 == 0)
                        {
                            buf.Add(str20);
                            num11 += length;
                            num5 = 1;
                        }
                        else
                        {
                            buf.Add(str21);
                            num11 += num51;
                        }
                        if ((class4.b[num + 2] > 0x30) && (class4.b[num + 2] < 0x37))
                        {
                            switch (((char)class4.b[num + 2]))
                            {
                                case '1':
                                    num7 = 0x24;
                                    break;

                                case '2':
                                    num7 = 0x20;
                                    break;

                                case '3':
                                    num7 = 0x1c;
                                    break;

                                case '4':
                                    num7 = 0x18;
                                    break;

                                case '5':
                                    num7 = 20;
                                    break;

                                case '6':
                                    num7 = 0x10;
                                    break;

                                default:
                                    num7 = 0x18;
                                    break;
                            }
                            if (this._preserveFontSize == 1)
                            {
                                newb.Clear();
                                newb.Add(@"\fs" + num7.ToString() + " ");
                                flag16 = true;
                                flag5 = false;
                            }
                        }
                        switch (class4.b[num + 2])
                        {
                            case 0x31:
                                _params.hNumber = CSS_tag_type.H1_CSS;
                                break;

                            case 50:
                                _params.hNumber = CSS_tag_type.H2_CSS;
                                break;

                            case 0x33:
                                _params.hNumber = CSS_tag_type.H3_CSS;
                                break;

                            case 0x34:
                                _params.hNumber = CSS_tag_type.H4_CSS;
                                break;

                            case 0x35:
                                _params.hNumber = CSS_tag_type.H5_CSS;
                                break;

                            case 0x36:
                                _params.hNumber = CSS_tag_type.H6_CSS;
                                break;

                            default:
                                _params.hNumber = CSS_tag_type.H_CSS;
                                break;
                        }
                        num7 = num;
                        if (class4.b[num + 3] == 0x20)
                        {
                            while ((class4.b[num + 3] != 0x3e) && (num < max))
                            {
                                num++;
                            }
                        }
                        num5 = 1;
                        if (!flag3)
                        {
                            buf.Add(class8);
                            num11 += class8.len;
                        }
                        if (!flag5)
                        {
                            buf.Add(newb);
                            num11 += newb.len;
                        }
                        num = num7;
                        num += 3;
                        this.CSS_insert(class22, class18, class19, class20, class21, align, class4, ref num, buf, ref num11, max, _params, ref flag4, class10, class14, ref num21, ref flag16, ref flag5, newb, list, list2, _params.hNumber);
                        goto Label_134B7;
                    }
                    if ((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x68) || (class4.b[num + 2] == 0x48))) && (class4.b[num + 4] == 0x3e))
                    {
                        flag5 = true;
                        if (flag12)
                        {
                            if (num34 < 2)
                            {
                                num34++;
                                buf.Add((byte)0x7d);
                            }
                            else
                            {
                                num34 = 1;
                                num11++;
                                buf.Add((byte)0x5c);
                                num11++;
                                buf.Add((byte)0x70);
                                num11++;
                                buf.Add((byte)0x61);
                                num11++;
                                buf.Add((byte)0x72);
                                num11++;
                                buf.Add((byte)0x7d);
                            }
                        }
                        else
                        {
                            num11++;
                            buf.Add((byte)0x5c);
                            num11++;
                            buf.Add((byte)0x70);
                            num11++;
                            buf.Add((byte)0x61);
                            num11++;
                            buf.Add((byte)0x72);
                            num11++;
                            buf.Add((byte)0x7d);
                        }
                        num += 4;
                        switch (_params.hNumber)
                        {
                            case CSS_tag_type.H1_CSS:
                                _params.hNumber = CSS_tag_type.H1_CSS;
                                break;

                            case CSS_tag_type.H2_CSS:
                                _params.hNumber = CSS_tag_type.H2_CSS;
                                break;

                            case CSS_tag_type.H3_CSS:
                                _params.hNumber = CSS_tag_type.H3_CSS;
                                break;

                            case CSS_tag_type.H4_CSS:
                                _params.hNumber = CSS_tag_type.H4_CSS;
                                break;

                            case CSS_tag_type.H5_CSS:
                                _params.hNumber = CSS_tag_type.H5_CSS;
                                break;

                            case CSS_tag_type.H6_CSS:
                                _params.hNumber = CSS_tag_type.H6_CSS;
                                break;

                            default:
                                _params.hNumber = CSS_tag_type.H_CSS;
                                break;
                        }
                        this.CSS_close(class22, class18, align, buf, ref num11, _params, ref flag4, class10, class14, ref num21, ref flag16, ref flag5, newb, fontsize, _params.hNumber);
                        goto Label_134B7;
                    }
                    if ((((class4.b[num] == 60) && ((class4.b[num + 1] == 0x73) || (class4.b[num + 1] == 0x53))) && (((class4.b[num + 2] == 0x74) || (class4.b[num + 2] == 0x54)) && ((class4.b[num + 3] == 0x72) || (class4.b[num + 3] == 0x52)))) && ((((class4.b[num + 4] == 0x6f) || (class4.b[num + 4] == 0x4f)) && ((class4.b[num + 5] == 110) || (class4.b[num + 5] == 0x4e))) && (((class4.b[num + 6] == 0x67) || (class4.b[num + 6] == 0x47)) && ((class4.b[num + 7] == 0x3e) || (class4.b[num + 7] == 0x20)))))
                    {
                        num11++;
                        buf.Add((byte)0x7b);
                        num11++;
                        buf.Add((byte)0x5c);
                        num11++;
                        buf.Add((byte)0x62);
                        num11++;
                        buf.Add((byte)0x20);
                        flag16 = true;
                        if (class4.b[num + 7] == 0x20)
                        {
                            while ((class4.b[num + 7] != 0x3e) && (num < max))
                            {
                                num++;
                            }
                        }
                        num += 7;
                        num29++;
                        if (num29 <= 0)
                        {
                            num29 = 1;
                        }
                        goto Label_134B7;
                    }
                    if (((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x73) || (class4.b[num + 2] == 0x53))) && (((class4.b[num + 3] == 0x74) || (class4.b[num + 3] == 0x54)) && ((class4.b[num + 4] == 0x72) || (class4.b[num + 4] == 0x52)))) && ((((class4.b[num + 5] == 0x6f) || (class4.b[num + 5] == 0x4f)) && ((class4.b[num + 6] == 110) || (class4.b[num + 6] == 0x4e))) && (((class4.b[num + 7] == 0x67) || (class4.b[num + 7] == 0x47)) && (class4.b[num + 8] == 0x3e))))
                    {
                        if (num29 > 0)
                        {
                            buf.Add((byte)0x7d);
                            num11++;
                            flag16 = false;
                        }
                        num29--;
                        num += 8;
                        goto Label_134B7;
                    }
                    if (((class4.b[num] == 60) && ((class4.b[num + 1] == 0x62) || (class4.b[num + 1] == 0x42))) && ((class4.b[num + 2] == 0x3e) || (class4.b[num + 2] == 0x20)))
                    {
                        num11++;
                        buf.Add((byte)0x7b);
                        num11++;
                        buf.Add((byte)0x5c);
                        num11++;
                        buf.Add((byte)0x62);
                        num11++;
                        buf.Add((byte)0x20);
                        flag16 = true;
                        if (class4.b[num + 2] == 0x20)
                        {
                            while ((class4.b[num + 2] != 0x3e) && (num < max))
                            {
                                num++;
                            }
                        }
                        num += 2;
                        num28++;
                        if (num28 <= 0)
                        {
                            num28 = 1;
                        }
                        goto Label_134B7;
                    }
                    if ((((class4.b[num] != 60) || (class4.b[num + 1] != 0x2f)) || ((class4.b[num + 2] != 0x62) && (class4.b[num + 2] != 0x42))) || (class4.b[num + 3] != 0x3e))
                    {
                        goto Label_AFB9;
                    }
                    if (num28 > 0)
                    {
                        buf.Add((byte)0x7d);
                        num11++;
                        flag16 = false;
                    }
                    num28--;
                    num += 3;
                    if (num11 <= 0x1116f)
                    {
                        goto Label_134B7;
                    }
                    if (!_tablesArray[num36].table)
                    {
                        break;
                    }
                    if (this._preserveNestedTables != 1)
                    {
                        goto Label_AF9B;
                    }
                    goto Label_AF73;
                Label_AE98:
                    buf.Add("\n\\pard \\intbl");
                str11 = @"\itap" + _tablesArray[num36].table_level + " ";
                buf.Add(str11);
                num11 += str11.Length;
                buf.Add("\n\\nestcell");
                nest_tables _tables6 = _tablesArray[num36];
                _tables6.cell--;
            Label_AEF7:
                if (_tablesArray[num36].cell > 0)
                {
                    goto Label_AE98;
                }
            buf.Add("\n\\pard \\intbl");
            str11 = @"\itap" + _tablesArray[num36].table_level + " ";
            buf.Add(str11);
            num11 += str11.Length;
            buf.Add(_tablesArray[num36].nestTblDescription);
            buf.Add(@"\nestrow}{\nonesttables\par}" + this.LF);
            num36--;
        Label_AF73:
            if (num36 > 0)
            {
                goto Label_AEF7;
            }
    Label_AF9B:
        while (_tablesArray[num36].cell > 0)
        {
            buf.Add("\n\\pard \\intbl\\cell");
            nest_tables _tables7 = _tablesArray[num36];
            _tables7.cell--;
        }
    buf.Add("\n\\pard \\intbl \\row \\pard");
    break;
Label_AFB9:
    if (((class4.b[num] == 60) && ((class4.b[num + 1] == 0x69) || (class4.b[num + 1] == 0x49))) && ((class4.b[num + 2] == 0x3e) || (class4.b[num + 2] == 0x20)))
    {
        num11++;
        buf.Add((byte)0x7b);
        num11++;
        buf.Add((byte)0x5c);
        num11++;
        buf.Add((byte)0x69);
        num11++;
        buf.Add((byte)0x20);
        flag16 = true;
        if (class4.b[num + 2] == 0x20)
        {
            while ((class4.b[num + 2] != 0x3e) && (num < max))
            {
                num++;
            }
        }
        num += 2;
        num31++;
        if (num31 <= 0)
        {
            num31 = 1;
        }
        goto Label_134B7;
    }
if ((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x69) || (class4.b[num + 2] == 0x49))) && (class4.b[num + 3] == 0x3e))
{
    if (num31 > 0)
    {
        buf.Add((byte)0x7d);
        num11++;
        flag16 = false;
    }
    num31--;
    num += 3;
    goto Label_134B7;
}
if (((class4.b[num] == 60) && ((class4.b[num + 1] == 0x65) || (class4.b[num + 1] == 0x45))) && (((class4.b[num + 2] == 0x6d) || (class4.b[num + 2] == 0x4d)) && ((class4.b[num + 3] == 0x3e) || (class4.b[num + 3] == 0x20))))
{
    num11++;
    buf.Add((byte)0x7b);
    this.CSS_insert(class22, class18, class19, class20, class21, align, class4, ref num, buf, ref num11, max, _params, ref flag4, class10, class14, ref num21, ref flag16, ref flag5, newb, list, list2, CSS_tag_type.EM_CSS);
    num11++;
    buf.Add((byte)0x5c);
    num11++;
    buf.Add((byte)0x69);
    num11++;
    buf.Add((byte)0x20);
    flag16 = true;
    num33++;
    if (num33 <= 0)
    {
        num33 = 1;
    }
    goto Label_134B7;
}
if ((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x65) || (class4.b[num + 2] == 0x45))) && (((class4.b[num + 3] == 0x6d) || (class4.b[num + 3] == 0x4d)) && (class4.b[num + 4] == 0x3e)))
{
    this.CSS_close(class22, class18, align, buf, ref num11, _params, ref flag4, class10, class14, ref num21, ref flag16, ref flag5, newb, fontsize, CSS_tag_type.EM_CSS);
    if (num33 > 0)
    {
        buf.Add((byte)0x7d);
        num11++;
        flag16 = false;
    }
    num33--;
    num += 4;
    goto Label_134B7;
}
if (((class4.b[num] == 60) && ((class4.b[num + 1] == 0x75) || (class4.b[num + 1] == 0x55))) && ((class4.b[num + 2] == 0x3e) || (class4.b[num + 2] == 0x20)))
{
    num11++;
    buf.Add((byte)0x7b);
    num11++;
    buf.Add((byte)0x5c);
    num11++;
    buf.Add((byte)0x75);
    num11++;
    buf.Add((byte)0x6c);
    num11++;
    buf.Add((byte)0x20);
    flag16 = true;
    if (class4.b[num + 2] == 0x20)
    {
        while ((class4.b[num + 2] != 0x3e) && (num < max))
        {
            num++;
        }
    }
    num += 2;
    num32++;
    if (num32 <= 0)
    {
        num32 = 1;
    }
    goto Label_134B7;
}
if ((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x75) || (class4.b[num + 2] == 0x55))) && (class4.b[num + 3] == 0x3e))
{
    if (num32 > 0)
    {
        buf.Add((byte)0x7d);
        num11++;
        flag16 = false;
    }
    num32--;
    num += 3;
    goto Label_134B7;
}
if ((((class4.b[num] == 60) && ((class4.b[num + 1] == 0x73) || (class4.b[num + 1] == 0x53))) && (((class4.b[num + 2] == 0x74) || (class4.b[num + 2] == 0x54)) && ((class4.b[num + 3] == 0x72) || (class4.b[num + 3] == 0x52)))) && ((((class4.b[num + 4] == 0x69) || (class4.b[num + 4] == 0x49)) && ((class4.b[num + 5] == 0x6b) || (class4.b[num + 5] == 0x4b))) && (((class4.b[num + 6] == 0x65) || (class4.b[num + 6] == 0x45)) && ((class4.b[num + 7] == 0x3e) || (class4.b[num + 7] == 0x20)))))
{
    num11++;
    buf.Add((byte)0x7b);
    num11++;
    buf.Add((byte)0x5c);
    num11++;
    buf.Add((byte)0x73);
    num11++;
    buf.Add((byte)0x74);
    num11++;
    buf.Add((byte)0x72);
    num11++;
    buf.Add((byte)0x69);
    num11++;
    buf.Add((byte)0x6b);
    num11++;
    buf.Add((byte)0x65);
    num11++;
    buf.Add((byte)0x20);
    flag16 = true;
    if (class4.b[num + 7] == 0x20)
    {
        while ((class4.b[num + 7] != 0x3e) && (num < max))
        {
            num++;
        }
    }
    num += 7;
    num30++;
    if (num30 <= 0)
    {
        num30 = 1;
    }
    goto Label_134B7;
}
if (((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x73) || (class4.b[num + 2] == 0x53))) && (((class4.b[num + 3] == 0x74) || (class4.b[num + 3] == 0x54)) && ((class4.b[num + 4] == 0x72) || (class4.b[num + 4] == 0x52)))) && ((((class4.b[num + 5] == 0x69) || (class4.b[num + 5] == 0x49)) && ((class4.b[num + 6] == 0x6b) || (class4.b[num + 6] == 0x4b))) && (((class4.b[num + 7] == 0x65) || (class4.b[num + 7] == 0x45)) && (class4.b[num + 8] == 0x3e))))
{
    if (num30 > 0)
    {
        buf.Add((byte)0x7d);
        num11++;
        flag16 = false;
    }
    num30--;
    num += 8;
    goto Label_134B7;
}
if (((class4.b[num] == 60) && ((class4.b[num + 1] == 0x73) || (class4.b[num + 1] == 0x53))) && ((class4.b[num + 2] == 0x3e) || (class4.b[num + 2] == 0x20)))
{
    num11++;
    buf.Add((byte)0x7b);
    num11++;
    buf.Add((byte)0x5c);
    num11++;
    buf.Add((byte)0x73);
    num11++;
    buf.Add((byte)0x74);
    num11++;
    buf.Add((byte)0x72);
    num11++;
    buf.Add((byte)0x69);
    num11++;
    buf.Add((byte)0x6b);
    num11++;
    buf.Add((byte)0x65);
    num11++;
    buf.Add((byte)0x20);
    flag16 = true;
    if (class4.b[num + 2] == 0x20)
    {
        while ((class4.b[num + 2] != 0x3e) && (num < max))
        {
            num++;
        }
    }
    num += 2;
    num30++;
    if (num30 <= 0)
    {
        num30 = 1;
    }
    goto Label_134B7;
}
if ((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x73) || (class4.b[num + 2] == 0x53))) && (class4.b[num + 3] == 0x3e))
{
    if (num30 > 0)
    {
        buf.Add((byte)0x7d);
        num11++;
        flag16 = false;
    }
    num30--;
    num += 3;
    goto Label_134B7;
}
if ((((class4.b[num] == 60) && ((class4.b[num + 1] == 0x73) || (class4.b[num + 1] == 0x53))) && (((class4.b[num + 2] == 0x75) || (class4.b[num + 2] == 0x55)) && ((class4.b[num + 3] == 0x62) || (class4.b[num + 3] == 0x42)))) && ((class4.b[num + 4] == 0x3e) || (class4.b[num + 4] == 0x20)))
{
    if (class4.b[num + 4] == 0x20)
    {
        while ((class4.b[num + 4] != 0x3e) && (num < max))
        {
            num++;
        }
    }
    buf.Add(str22);
    num11 += num53;
    num += 4;
    if (!flag3)
    {
        buf.Add(class8);
        num11 += class8.len;
    }
    if (!flag5)
    {
        buf.Add(newb);
        num11 += newb.len;
    }
    goto Label_134B7;
}
if (((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x73) || (class4.b[num + 2] == 0x53))) && (((class4.b[num + 3] == 0x75) || (class4.b[num + 3] == 0x55)) && ((class4.b[num + 4] == 0x62) || (class4.b[num + 4] == 0x42)))) && (class4.b[num + 5] == 0x3e))
{
    buf.Add("}");
    num11++;
    flag16 = false;
    num += 5;
    goto Label_134B7;
}
if ((((class4.b[num] == 60) && ((class4.b[num + 1] == 0x73) || (class4.b[num + 1] == 0x53))) && (((class4.b[num + 2] == 0x75) || (class4.b[num + 2] == 0x55)) && ((class4.b[num + 3] == 0x70) || (class4.b[num + 3] == 80)))) && ((class4.b[num + 4] == 0x3e) || (class4.b[num + 4] == 0x20)))
{
    if (class4.b[num + 4] == 0x20)
    {
        while ((class4.b[num + 4] != 0x3e) && (num < max))
        {
            num++;
        }
    }
    buf.Add(str23);
    num11 += num54;
    num += 4;
    if (!flag3)
    {
        buf.Add(class8);
        num11 += class8.len;
    }
    if (!flag5)
    {
        buf.Add(newb);
        num11 += newb.len;
    }
    goto Label_134B7;
}
if (((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x73) || (class4.b[num + 2] == 0x53))) && (((class4.b[num + 3] == 0x75) || (class4.b[num + 3] == 0x55)) && ((class4.b[num + 4] == 0x70) || (class4.b[num + 4] == 80)))) && (class4.b[num + 5] == 0x3e))
{
    buf.Add("}");
    num11++;
    flag16 = false;
    num += 5;
    goto Label_134B7;
}
if ((((class4.b[num] == 60) && ((class4.b[num + 1] == 100) || (class4.b[num + 1] == 0x44))) && (((class4.b[num + 2] == 0x69) || (class4.b[num + 2] == 0x49)) && ((class4.b[num + 3] == 0x76) || (class4.b[num + 3] == 0x56)))) && ((class4.b[num + 4] == 0x3e) || (class4.b[num + 4] == 0x20)))
{
    num += 4;
    if (flag12)
    {
        if (num34 < 2)
        {
            num34++;
            num5 = 1;
        }
        else
        {
            num34 = 1;
            num5 = 0;
        }
    }
    if (buf.len < (num12 + 2))
    {
        num5 = 1;
    }
    if (num5 == 0)
    {
        if (_tablesArray[num36].table)
        {
            buf.Add(this.DIV_TBL_STR);
            num11 += this.DIV_TBL_STR.Length;
            if ((num36 > 0) && (this._preserveNestedTables == 1))
            {
                str11 = @"\itap" + _tablesArray[num36].table_level + " ";
                buf.Add(str11);
                num11 += str11.Length;
            }
        }
        else
        {
            buf.Add(this.DIV_STR);
            num11 += this.DIV_STR.Length;
        }
        num5 = 1;
    }
    if (!flag3)
    {
        buf.Add(class8);
        num11 += class8.len;
    }
    if (!flag5)
    {
        buf.Add(newb);
        num11 += newb.len;
    }
    this.CSS_insert(class22, class18, class19, class20, class21, align, class4, ref num, buf, ref num11, max, _params, ref flag4, class10, class14, ref num21, ref flag16, ref flag5, newb, list, list2, CSS_tag_type.DIV_CSS);
    goto Label_134B7;
}
if (((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 100) || (class4.b[num + 2] == 0x44))) && (((class4.b[num + 3] == 0x69) || (class4.b[num + 3] == 0x49)) && ((class4.b[num + 4] == 0x76) || (class4.b[num + 4] == 0x56)))) && (class4.b[num + 5] == 0x3e))
{
    if (flag12)
    {
        if (num34 < 2)
        {
            num34++;
            num5 = 1;
        }
        else
        {
            num34 = 1;
            num5 = 0;
        }
    }
    if (num5 == 0)
    {
        if (_tablesArray[num36].table)
        {
            buf.Add(this.DIV_TBL_STR);
            num11 += this.DIV_TBL_STR.Length;
            if ((num36 > 0) && (this._preserveNestedTables == 1))
            {
                str11 = @"\itap" + _tablesArray[num36].table_level + " ";
                buf.Add(str11);
                num11 += str11.Length;
            }
        }
        else
        {
            buf.Add(this.DIV_STR);
            num11 += this.DIV_STR.Length;
        }
        if (!flag3)
        {
            buf.Add(class8);
            num11 += class8.len;
        }
        if (!flag5)
        {
            buf.Add(newb);
            num11 += newb.len;
        }
        num5 = 1;
    }
    num += 5;
    this.CSS_close(class22, class18, align, buf, ref num11, _params, ref flag4, class10, class14, ref num21, ref flag16, ref flag5, newb, fontsize, CSS_tag_type.DIV_CSS);
    goto Label_134B7;
}
if ((((num5 == 0) && (class4.b[num] == 60)) && ((class4.b[num + 1] == 100) || (class4.b[num + 1] == 0x44))) && (((class4.b[num + 2] == 0x6c) || (class4.b[num + 2] == 0x4c)) && (class4.b[num + 3] == 0x3e)))
{
    if (_tablesArray[num36].table)
    {
        buf.Add(str15);
        if ((num36 > 0) && (this._preserveNestedTables == 1))
        {
            str11 = @"\itap" + _tablesArray[num36].table_level + " ";
            buf.Add(str11);
            num11 += str11.Length;
        }
    }
    else
    {
        buf.Add(str14);
    }
    num += 3;
    num5 = 1;
    if (!flag3)
    {
        buf.Add(class8);
        num11 += class8.len;
    }
    if (!flag5)
    {
        buf.Add(newb);
        num11 += newb.len;
    }
    goto Label_134B7;
}
if (((((num5 == 0) && (class4.b[num] == 60)) && (class4.b[num + 1] == 0x2f)) && (((class4.b[num + 2] == 100) || (class4.b[num + 2] == 0x44)) && ((class4.b[num + 3] == 100) || (class4.b[num + 3] == 0x44)))) && (class4.b[num + 4] == 0x3e))
{
    if (_tablesArray[num36].table)
    {
        buf.Add(str15);
        if ((num36 > 0) && (this._preserveNestedTables == 1))
        {
            str11 = @"\itap" + _tablesArray[num36].table_level + " ";
            buf.Add(str11);
            num11 += str11.Length;
        }
    }
    else
    {
        buf.Add(str14);
    }
    num += 4;
    if (!flag3)
    {
        buf.Add(class8);
        num11 += class8.len;
    }
    if (!flag5)
    {
        buf.Add(newb);
        num11 += newb.len;
    }
    goto Label_134B7;
}
if (((((this._preserveAlignment == 1) && (class4.b[num] == 60)) && ((class4.b[num + 1] == 0x63) || (class4.b[num + 1] == 0x43))) && (((class4.b[num + 2] == 0x65) || (class4.b[num + 2] == 0x45)) && ((class4.b[num + 3] == 110) || (class4.b[num + 3] == 0x4e)))) && ((((class4.b[num + 4] == 0x74) || (class4.b[num + 4] == 0x54)) && ((class4.b[num + 5] == 0x65) || (class4.b[num + 5] == 0x45))) && (((class4.b[num + 6] == 0x72) || (class4.b[num + 6] == 0x52)) && ((class4.b[num + 7] == 0x3e) || (class4.b[num + 7] == 0x20)))))
{
    if (num5 == 0)
    {
        if (_tablesArray[num36].table)
        {
            buf.Add(str15);
            if ((num36 > 0) && (this._preserveNestedTables == 1))
            {
                str11 = @"\itap" + _tablesArray[num36].table_level + " ";
                buf.Add(str11);
                num11 += str11.Length;
            }
        }
        else
        {
            buf.Add(str14);
        }
        num5 = 1;
    }
    num11++;
    buf.Add((byte)0x5c);
    num11++;
    buf.Add((byte)0x71);
    num11++;
    buf.Add((byte)0x63);
    num11++;
    buf.Add((byte)0x20);
    if (class4.b[num + 7] == 0x20)
    {
        while ((class4.b[num + 7] != 0x3e) && (num < max))
        {
            num++;
        }
    }
    num += 7;
    num52 = 1;
    goto Label_134B7;
}
if ((((((this._preserveAlignment == 1) && (class4.b[num] == 60)) && (class4.b[num + 1] == 0x2f)) && (((class4.b[num + 2] == 0x63) || (class4.b[num + 2] == 0x43)) && ((class4.b[num + 3] == 0x65) || (class4.b[num + 3] == 0x45)))) && ((((class4.b[num + 4] == 110) || (class4.b[num + 4] == 0x4e)) && ((class4.b[num + 5] == 0x74) || (class4.b[num + 5] == 0x54))) && (((class4.b[num + 6] == 0x65) || (class4.b[num + 6] == 0x45)) && ((class4.b[num + 7] == 0x72) || (class4.b[num + 7] == 0x52))))) && (class4.b[num + 8] == 0x3e))
{
    if (num5 == 0)
    {
        if (_tablesArray[num36].table)
        {
            buf.Add(str15);
            if ((num36 > 0) && (this._preserveNestedTables == 1))
            {
                str11 = @"\itap" + _tablesArray[num36].table_level + " ";
                buf.Add(str11);
                num11 += str11.Length;
            }
        }
        else
        {
            buf.Add(str14);
        }
        num5 = 1;
    }
    num11++;
    buf.Add((byte)0x5c);
    num11++;
    buf.Add((byte)0x71);
    num11++;
    buf.Add(num23);
    num11++;
    buf.Add((byte)0x20);
    num += 8;
    num52 = 0;
    goto Label_134B7;
}
if ((((this._preserveHyperlinks == 1) && (class4.b[num] == 60)) && ((class4.b[num + 1] == 0x61) || (class4.b[num + 1] == 0x41))) && this.IS_DELIMITER(class4.b[num + 2]))
{
    if (flag8)
    {
        buf.Add("}}}");
    }
    flag8 = false;
    num += 2;
    flag10 = false;
    while ((class4.b[num] != 0x3e) && (num < max))
    {
        if (((!this.IS_DELIMITER(class4.b[num]) || ((class4.b[num + 1] != 0x68) && (class4.b[num + 1] != 0x48))) || (((class4.b[num + 2] != 0x72) && (class4.b[num + 2] != 0x52)) || ((class4.b[num + 3] != 0x65) && (class4.b[num + 3] != 0x45)))) || ((((class4.b[num + 4] != 0x66) && (class4.b[num + 4] != 70)) || (class4.b[num + 5] != 0x3d)) || ((class4.b[num + 6] != 0x22) && (class4.b[num + 6] != 0x27))))
        {
            goto Label_CAA6;
        }
        if (class4.b[num + 6] == 0x27)
        {
            flag10 = true;
        }
        num += 6;
        num7 = 0;
        class16.Clear();
        class16.Add(class4.b[num++]);
        num7++;
        flag19 = true;
        for (num38 = 0; (num38 < 500) && ((num + num38) < num10); num38++)
        {
            if ((((class4.b[num + num38] == 60) && (class4.b[(num + 1) + num38] == 0x2f)) && ((class4.b[(num + 2) + num38] == 0x61) || (class4.b[(num + 2) + num38] == 0x41))) && (class4.b[(num + 3) + num38] == 0x3e))
            {
                break;
            }
            if ((((class4.b[num + num38] == 60) && ((class4.b[(num + 1) + num38] == 0x69) || (class4.b[(num + 1) + num38] == 0x49))) && (((class4.b[(num + 2) + num38] == 0x6d) || (class4.b[(num + 2) + num38] == 0x4d)) && ((class4.b[(num + 3) + num38] == 0x67) || (class4.b[(num + 3) + num38] == 0x47)))) && (class4.b[(num + 4) + num38] == 0x20))
            {
                flag19 = false;
            }
        }
        if (flag19 || (this._preserveImages != 0))
        {
            goto Label_C8DF;
        }
        continue;
    Label_C8A4:
        if ((class4.b[num] == 0x27) && flag10)
        {
            goto Label_C901;
        }
    class16.Add(class4.b[num]);
    num++;
    if (num7 < (this.HYPERLINK_SIZE - 5))
    {
        num7++;
    }
Label_C8DF:
    if (((class4.b[num] != 0x22) && (num < max)) && (class4.b[num] != 0x3e))
    {
        goto Label_C8A4;
    }
Label_C901:
class16.Add(class4.b[num]);
class17.Clear();
for (num7 = 0; num7 < class16.len; num7++)
{
    if (flag10 && (class16.b[num7] == 0x27))
    {
        class17.Add("\"");
    }
    else
    {
        class17.Add(class16.b[num7]);
    }
}
class15.Clear();
if (class17.len > 1)
{
    if (class17.b[1] == 0x23)
    {
        num7 = 1;
        while (class17.b[num7] == 0)
        {
            class17.b[num7] = class17.b[num7++ + 1];
        }
        if (class17.b[1] != 0x22)
        {
            class15.Clear();
            class15.Add(@"{\field\fldedit{\*\fldinst { HYPERLINK \\l " + class17.ByteToString() + @" }}{\fldrslt {" + (!flag19 ? "" : @"\ul\cf2") + " ");
        }
    }
    else
    {
        if (class17.b[1] != 0x22)
        {
            class15.Clear();
        }
        class15.Add(@"{\field\fldedit{\*\fldinst { HYPERLINK " + class17.ByteToString() + @" }}{\fldrslt {" + (!flag19 ? "" : @"\ul\cf2") + " ");
    }
}
if (class15.len != 0)
{
    class15.ByteToString();
    buf.Add(class15);
    num11 += class15.len;
    flag8 = true;
}
Label_CAA6:
if (((((class4.b[num] != 110) && (class4.b[num] != 0x4e)) || ((class4.b[num + 1] != 0x61) && (class4.b[num + 1] != 0x41))) || (((class4.b[num + 2] != 0x6d) && (class4.b[num + 2] != 0x4d)) || ((class4.b[num + 3] != 0x65) && (class4.b[num + 3] != 0x45)))) || ((class4.b[num + 4] != 0x3d) || ((class4.b[num + 5] != 0x22) && (class4.b[num + 5] != 0x27))))
{
    goto Label_CDEA;
}
if (class4.b[num + 5] == 0x27)
{
    flag10 = true;
}
num += 5;
num7 = 0;
class16.Clear();
class16.Add(class4.b[num++]);
num7++;
flag19 = true;
for (num38 = 0; (num38 < 500) && ((num + num38) < num10); num38++)
{
    if ((((class4.b[num + num38] == 60) && (class4.b[(num + 1) + num38] == 0x2f)) && ((class4.b[(num + 2) + num38] == 0x61) || (class4.b[(num + 2) + num38] == 0x41))) && (class4.b[(num + 3) + num38] == 0x3e))
    {
        break;
    }
    if ((((class4.b[num + num38] == 60) && ((class4.b[(num + 1) + num38] == 0x69) || (class4.b[(num + 1) + num38] == 0x49))) && (((class4.b[(num + 2) + num38] == 0x6d) || (class4.b[(num + 2) + num38] == 0x4d)) && ((class4.b[(num + 3) + num38] == 0x67) || (class4.b[(num + 3) + num38] == 0x47)))) && (class4.b[(num + 4) + num38] == 0x20))
    {
        flag19 = false;
    }
}
if (flag19 || (this._preserveImages != 0))
{
    goto Label_CD11;
}
continue;
Label_CCD6:
if ((class4.b[num] == 0x27) && flag10)
{
    goto Label_CD33;
}
class16.Add(class4.b[num]);
num++;
if (num7 < (this.HYPERLINK_SIZE - 5))
{
    num7++;
}
Label_CD11:
if (((class4.b[num] != 0x22) && (num < max)) && (class4.b[num] != 0x3e))
{
    goto Label_CCD6;
}
Label_CD33:
class16.Add(class4.b[num]);
num7++;
class17.Clear();
for (num7 = 0; class16.b[num7] != 0; num7++)
{
    if ((!flag10 && (class16.b[num7] != 0x22)) || (flag10 && (class16.b[num7] != 0x27)))
    {
        class17.Add(class16.b[num7]);
    }
}
class15.Clear();
class15.Add(string.Concat(new object[] { @"{\*\bkmkstart ", class17, @"}{\*\bkmkend ", class17, "}" }));
buf.Add(class15);
Label_CDEA:
if (((((class4.b[num] == 0x74) || (class4.b[num] == 0x54)) && ((class4.b[num + 1] == 0x69) || (class4.b[num + 1] == 0x49))) && (((class4.b[num + 2] == 0x74) || (class4.b[num + 2] == 0x54)) && ((class4.b[num + 3] == 0x6c) || (class4.b[num + 3] == 0x4c)))) && (((class4.b[num + 4] == 0x65) || (class4.b[num + 4] == 0x45)) && ((class4.b[num + 5] == 0x3d) || this.IS_DELIMITER(class4.b[num + 5]))))
{
    num += 5;
    this.ReadValue(class4, ref num, class17);
}
num++;
    }
    goto Label_134B7;
}
if ((((this._preserveHyperlinks == 1) && (class4.b[num] == 60)) && (class4.b[num + 1] == 0x2f)) && (((class4.b[num + 2] == 0x61) || (class4.b[num + 2] == 0x41)) && (class4.b[num + 3] == 0x3e)))
{
    if (flag8)
    {
        buf.Add("}}}");
        num11 += 3;
    }
    flag8 = false;
    num += 3;
    goto Label_134B7;
}
if (((((class4.b[num] == 60) && ((class4.b[num + 1] == 0x62) || (class4.b[num + 1] == 0x42))) && (((class4.b[num + 2] == 0x6c) || (class4.b[num + 2] == 0x4c)) && ((class4.b[num + 3] == 0x6f) || (class4.b[num + 3] == 0x4f)))) && ((((class4.b[num + 4] == 0x63) || (class4.b[num + 4] == 0x43)) && ((class4.b[num + 5] == 0x6b) || (class4.b[num + 5] == 0x4b))) && (((class4.b[num + 6] == 0x71) || (class4.b[num + 6] == 0x51)) && ((class4.b[num + 7] == 0x75) || (class4.b[num + 7] == 0x55))))) && ((((class4.b[num + 8] == 0x6f) || (class4.b[num + 8] == 0x4f)) && ((class4.b[num + 9] == 0x74) || (class4.b[num + 9] == 0x54))) && (((class4.b[num + 10] == 0x65) || (class4.b[num + 10] == 0x45)) && ((class4.b[num + 11] == 0x3e) || (class4.b[num + 11] == 0x20)))))
{
    if (class4.b[num + 11] == 0x20)
    {
        while ((class4.b[num + 11] != 0x3e) && (num < max))
        {
            num++;
        }
    }
    num += 11;
    num27 = (byte)(num27 + 1);
    if (num5 != 0)
    {
        buf.Add(str9);
        num11 += str9.Length;
    }
    str14 = str14 + str9;
    str15 = str15 + str9;
    if (_tablesArray[num36].table)
    {
        buf.Add(str15);
        if ((num36 > 0) && (this._preserveNestedTables == 1))
        {
            str11 = @"\itap" + _tablesArray[num36].table_level + " ";
            buf.Add(str11);
            num11 += str11.Length;
        }
    }
    else
    {
        buf.Add(str14);
    }
    num5 = 1;
    goto Label_134B7;
}
if ((((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x62) || (class4.b[num + 2] == 0x42))) && (((class4.b[num + 3] == 0x6c) || (class4.b[num + 3] == 0x4c)) && ((class4.b[num + 4] == 0x6f) || (class4.b[num + 4] == 0x4f)))) && ((((class4.b[num + 5] == 0x63) || (class4.b[num + 5] == 0x43)) && ((class4.b[num + 6] == 0x6b) || (class4.b[num + 6] == 0x4b))) && (((class4.b[num + 7] == 0x71) || (class4.b[num + 7] == 0x51)) && ((class4.b[num + 8] == 0x75) || (class4.b[num + 8] == 0x55))))) && ((((class4.b[num + 9] == 0x6f) || (class4.b[num + 9] == 0x4f)) && ((class4.b[num + 10] == 0x74) || (class4.b[num + 10] == 0x54))) && (((class4.b[num + 11] == 0x65) || (class4.b[num + 11] == 0x45)) && (class4.b[num + 12] == 0x3e))))
{
    num += 12;
    if ((num5 != 0) && (buf.ToByteCStartPos(buf.len - str9.Length).byteCmp(str9) == 0))
    {
        buf.len -= str9.Length;
    }
    if (num27 > 0)
    {
        str15 = str15.Substring(0, str15.Length - str9.Length);
    }
    num27 = (byte)(num27 - 1);
    if (num27 < 0)
    {
        num27 = 0;
    }
    if (_tablesArray[num36].table)
    {
        buf.Add(str15);
        if ((num36 > 0) && (this._preserveNestedTables == 1))
        {
            str11 = @"\itap" + _tablesArray[num36].table_level + " ";
            buf.Add(str11);
            num11 += str11.Length;
        }
    }
    else
    {
        buf.Add(str14);
    }
    num5 = 1;
    goto Label_134B7;
}
if ((((class4.b[num] == 60) && ((class4.b[num + 1] == 0x70) || (class4.b[num + 1] == 80))) && (((class4.b[num + 2] == 0x72) || (class4.b[num + 2] == 0x52)) && ((class4.b[num + 3] == 0x65) || (class4.b[num + 3] == 0x45)))) && (class4.b[num + 4] == 0x3e))
{
    num += 4;
    flag11 = true;
    num4 = 1;
    goto Label_134B7;
}
if (((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x70) || (class4.b[num + 2] == 80))) && (((class4.b[num + 3] == 0x72) || (class4.b[num + 3] == 0x52)) && ((class4.b[num + 4] == 0x65) || (class4.b[num + 4] == 0x45)))) && (class4.b[num + 5] == 0x3e))
{
    num += 5;
    flag11 = false;
    goto Label_134B7;
}
if ((class4.b[num] == 0x26) && (num4 != 0))
{
    num += this.special_symbols_rtf(class4, num, max, class24, charset != 0);
    buf.Add(class24);
    num11 += class24.len;
    num5 = 0;
    goto Label_134B7;
}
if (((class4.b[num] == 0x7b) || (class4.b[num] == 0x7d)) && !hieroglyph)
{
    if (num4 != 0)
    {
        num11++;
        buf.Add((byte)0x5c);
        num11++;
        buf.Add(class4.b[num]);
    }
    goto Label_134B7;
}
if (((class4.b[num] == 0x5c) && (num4 != 0)) && !hieroglyph)
{
    buf.Add(@"\\");
    num11 += 2;
    goto Label_134B7;
}
if (this._preserveTables != 1)
{
    goto Label_1143D;
}
if ((this._preserveNestedTables == 0) && (this._preserveTableWidth == 0))
{
    if (((((class4.b[num] == 60) && ((class4.b[num + 1] == 0x74) || (class4.b[num + 1] == 0x54))) && ((class4.b[num + 2] == 0x61) || (class4.b[num + 2] == 0x41))) && (((class4.b[num + 3] == 0x62) || (class4.b[num + 3] == 0x42)) && ((class4.b[num + 4] == 0x6c) || (class4.b[num + 4] == 0x4c)))) && (((class4.b[num + 5] == 0x65) || (class4.b[num + 5] == 0x45)) && ((class4.b[num + 6] == 0x20) || (class4.b[num + 6] == 0x3e))))
    {
        _tablesArray[num36].table_border_visible = flag17;
        if (class4.b[num + 6] == 0x20)
        {
            if (this._borderVisibility == eBorderVisibility.SameAsOriginalHtml)
            {
                _tablesArray[num36].table_border_visible = false;
                _tablesArray[num36].table_border_visible = this.get_border(class4, num, max);
            }
            _tablesArray[num36].table_p.percent_width = 0;
            int num63 = -1111;
            _tablesArray[num36].table_p.table_width = this.get_width(class4, num, max, ref _tablesArray[num36].table_p.percent_width, ref index, ref index, null, num63, ref num63, null, ref num63, ref num63);
            while ((class4.b[num + 6] != 0x3e) && (num < max))
            {
                num++;
            }
        }
        if (_tablesArray[num36].table)
        {
            while (_tablesArray[num36].cell > 0)
            {
                buf.Add(this.LF + @"\pard \intbl\cell");
                num11 += 0x12;
                nest_tables _tables8 = _tablesArray[num36];
                _tables8.cell--;
            }
            buf.Add(this.LF + @"\pard \intbl \row");
            num11 += 0x12;
        }
        _tablesArray[num36].table = false;
        num4 = 0;
        if (flag21 && (num5 == 0))
        {
            buf.Add(this.LF + @"\pard\par" + this.LF);
            num11 += 11;
        }
        num += 6;
        goto Label_134B7;
    }
    if ((((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x74) || (class4.b[num + 2] == 0x54))) && ((class4.b[num + 3] == 0x61) || (class4.b[num + 3] == 0x41))) && (((class4.b[num + 4] == 0x62) || (class4.b[num + 4] == 0x42)) && ((class4.b[num + 5] == 0x6c) || (class4.b[num + 5] == 0x4c)))) && (((class4.b[num + 6] == 0x65) || (class4.b[num + 6] == 0x45)) && (class4.b[num + 7] == 0x3e)))
    {
        _tablesArray[num36].table = false;
        if (class4.len > 1)
        {
            while ((class4.len > 1) && this.IS_DELIMITER(class4.b[buf.len - 1]))
            {
                class4.len--;
            }
        }
        buf.Add(@"\pard" + this.LF);
        num11 += 6;
        num += 7;
        flag21 = false;
        goto Label_134B7;
    }
    if ((((class4.b[num] == 60) && ((class4.b[num + 1] == 0x74) || (class4.b[num + 1] == 0x54))) && ((class4.b[num + 2] == 0x72) || (class4.b[num + 2] == 0x52))) && ((class4.b[num + 3] == 0x3e) || (class4.b[num + 3] == 0x20)))
    {
        if (class4.b[num + 3] == 0x20)
        {
            while ((class4.b[num + 3] != 0x3e) && (num < max))
            {
                num++;
            }
        }
        num += 3;
        if (_tablesArray[num36].table)
        {
            while (_tablesArray[num36].cell > 0)
            {
                buf.Add(this.LF + @"\pard \intbl\cell");
                num11 += 0x12;
                nest_tables _tables9 = _tablesArray[num36];
                _tables9.cell--;
            }
            buf.Add(@"\pard \intbl \row" + this.LF);
            num11 += 0x12;
        }
        if (((buf.len > 1) && (buf.b[buf.len - 1] == 0x20)) && this.IS_DELIMITER(buf.b[buf.len - 2]))
        {
            buf.len--;
        }
        buf.Add(s);
        num11 += s.Length;
        if (_tablesArray[num36].table_p.tableAlign != 0)
        {
            if (_tablesArray[num36].table_p.tableAlign == this.ALIGN_CENTER)
            {
                buf.Add(@"\trqc ");
                num11 += 6;
            }
            else if (_tablesArray[num36].table_p.tableAlign == this.ALIGN_RIGHT)
            {
                buf.Add(@"\trqr ");
                num11 += 6;
            }
        }
        _tablesArray[num36].table = true;
        for (num7 = 0; num7 < this.MAX_COLUMNS; num7++)
        {
            _tablesArray[num36].td_width[num7] = 0;
            _tablesArray[num36].td_percent_width[num7] = 0;
        }
        _tablesArray[num36].td = 0;
        for (num7 = 0; (num + num7) < max; num7++)
        {
            if ((((((class4.b[num + num7] == 60) && (class4.b[(num + num7) + 1] == 0x2f)) && ((class4.b[(num + num7) + 2] == 0x74) || (class4.b[(num + num7) + 2] == 0x54))) && ((class4.b[(num + num7) + 3] == 0x72) || (class4.b[(num + num7) + 3] == 0x52))) && (class4.b[(num + num7) + 4] == 0x3e)) || ((((class4.b[num + num7] == 60) && ((class4.b[(num + num7) + 1] == 0x74) || (class4.b[(num + num7) + 1] == 0x54))) && ((class4.b[(num + num7) + 2] == 0x72) || (class4.b[(num + num7) + 2] == 0x52))) && ((class4.b[(num + num7) + 3] == 0x3e) || (class4.b[(num + num7) + 3] == 0x20))))
            {
                break;
            }
            if ((((class4.b[num + num7] == 60) && ((class4.b[(num + num7) + 1] == 0x74) || (class4.b[(num + num7) + 1] == 0x54))) && (((class4.b[(num + num7) + 2] == 100) || (class4.b[(num + num7) + 2] == 0x44)) || ((class4.b[(num + num7) + 2] == 0x68) || (class4.b[(num + num7) + 2] == 0x48)))) && ((class4.b[(num + num7) + 3] == 0x3e) || (class4.b[(num + num7) + 3] == 0x20)))
            {
                nest_tables _tables10 = _tablesArray[num36];
                _tables10.td++;
                if (class4.b[(num + num7) + 3] == 0x20)
                {
                    num7 += 3;
                    int bgcolor = -1111;
                    _tablesArray[num36].td_width[_tablesArray[num36].td - 1] = this.get_width(class4, num + num7, max, ref _tablesArray[num36].td_percent_width[_tablesArray[num36].td - 1], ref index, ref index, null, -1111, ref bgcolor, null, ref bgcolor, ref bgcolor);
                }
            }
            if (((((class4.b[num + num7] == 60) && ((class4.b[(num + num7) + 1] == 0x74) || (class4.b[(num + num7) + 1] == 0x54))) && ((class4.b[(num + num7) + 2] == 0x61) || (class4.b[(num + num7) + 2] == 0x41))) && (((class4.b[(num + num7) + 3] == 0x62) || (class4.b[(num + num7) + 3] == 0x42)) && ((class4.b[(num + num7) + 4] == 0x6c) || (class4.b[(num + num7) + 4] == 0x4c)))) && (((class4.b[(num + num7) + 5] == 0x65) || (class4.b[(num + num7) + 5] == 0x45)) && ((class4.b[(num + num7) + 6] == 0x20) || (class4.b[(num + num7) + 6] == 0x3e))))
            {
                break;
            }
        }
        _tablesArray[num36].cell = _tablesArray[num36].td;
        this.fill_columns_width(_tablesArray[num36].td_width, _tablesArray[num36].td_percent_width, _tablesArray[num36].td, _tablesArray[num36].table_p.table_width, _tablesArray[num36].table_p.percent_width, this.SCREEN_W_DEF);
        if (_tablesArray[num36].td != 0)
        {
            for (num7 = 0; num7 < _tablesArray[num36].td; num7++)
            {
                if (_tablesArray[num36].table_border_visible)
                {
                    buf.Add(this.LF + @"\clbrdrl\brdrs\clbrdrt\brdrs\clbrdrr\brdrs\clbrdrb\brdrs\cellx");
                }
                else
                {
                    buf.Add(this.LF + @"\cellx");
                    num11 += 7;
                }
                str12 = _tablesArray[num36].td_width[num7].ToString();
                buf.Add(str12);
                num11 += str12.Length;
            }
        }
        goto Label_134B7;
    }
    if ((((class4.b[num] == 60) && ((class4.b[num + 1] == 0x74) || (class4.b[num + 1] == 0x54))) && (((class4.b[num + 2] == 100) || (class4.b[num + 2] == 0x44)) || ((class4.b[num + 2] == 0x68) || (class4.b[num + 2] == 0x48)))) && ((class4.b[num + 3] == 0x3e) || this.IS_DELIMITER(class4.b[num + 3])))
    {
        buf.Add(this.LF + @"\pard \intbl ");
        num11 += 14;
        if (this.IS_DELIMITER(class4.b[num + 3]))
        {
            while (!this.IS_XTHAN(class4.b[num + 3]) && (num < max))
            {
                if (this._preserveAlignment == 1)
                {
                    if (((((class4.b[num] == 0x63) || (class4.b[num] == 0x43)) && ((class4.b[num + 1] == 0x65) || (class4.b[num + 1] == 0x45))) && (((class4.b[num + 2] == 110) || (class4.b[num + 2] == 0x4e)) && ((class4.b[num + 3] == 0x74) || (class4.b[num + 3] == 0x54)))) && (((class4.b[num + 4] == 0x65) || (class4.b[num + 4] == 0x45)) && ((class4.b[num + 5] == 0x72) || (class4.b[num + 5] == 0x52))))
                    {
                        align = class19;
                    }
                    if (((((class4.b[num] == 0x6d) || (class4.b[num] == 0x4d)) && ((class4.b[num + 1] == 0x69) || (class4.b[num + 1] == 0x49))) && (((class4.b[num + 2] == 100) || (class4.b[num + 2] == 0x44)) && ((class4.b[num + 3] == 100) || (class4.b[num + 3] == 0x44)))) && (((class4.b[num + 4] == 0x6c) || (class4.b[num + 4] == 0x4c)) && ((class4.b[num + 5] == 0x65) || (class4.b[num + 5] == 0x45))))
                    {
                        align = class19;
                    }
                    if ((((class4.b[num] == 0x6c) || (class4.b[num] == 0x4c)) && ((class4.b[num + 1] == 0x65) || (class4.b[num + 1] == 0x45))) && (((class4.b[num + 2] == 0x66) || (class4.b[num + 2] == 70)) && ((class4.b[num + 3] == 0x74) || (class4.b[num + 3] == 0x54))))
                    {
                        align = class20;
                    }
                    if (((((class4.b[num] == 0x72) || (class4.b[num] == 0x52)) && ((class4.b[num + 1] == 0x69) || (class4.b[num + 1] == 0x49))) && (((class4.b[num + 2] == 0x67) || (class4.b[num + 2] == 0x47)) && ((class4.b[num + 3] == 0x68) || (class4.b[num + 3] == 0x48)))) && ((class4.b[num + 4] == 0x74) || (class4.b[num + 4] == 0x54)))
                    {
                        align = class21;
                    }
                }
                if (((((this._preserveBackgroundColor == 1) && ((class4.b[num] == 0x62) || (class4.b[num] == 0x42))) && ((class4.b[num + 1] == 0x67) || (class4.b[num + 1] == 0x47))) && (((class4.b[num + 2] == 0x63) || (class4.b[num + 2] == 0x43)) && ((class4.b[num + 3] == 0x6f) || (class4.b[num + 3] == 0x4f)))) && ((((class4.b[num + 4] == 0x6c) || (class4.b[num + 4] == 0x4c)) && ((class4.b[num + 5] == 0x6f) || (class4.b[num + 5] == 0x4f))) && (((class4.b[num + 6] == 0x72) || (class4.b[num + 6] == 0x52)) && (class4.b[num + 7] == 0x3d))))
                {
                    num += 8;
                    this.read_color(class4, ref num, max, class10);
                    for (num7 = 0; num7 < num14; num7++)
                    {
                        if (class10.byteCmpi((string)list2[num7]) == 0)
                        {
                            _tablesArray[num36].td_bg = num7 + 3;
                        }
                    }
                }
                num++;
            }
        }
        if (this._preserveAlignment == 1)
        {
            buf.Add(align);
            num11 += len;
        }
        num += 3;
        num4 = 0;
        goto Label_134B7;
    }
    if (((((_tablesArray[num36].table && (class4.b[num] == 60)) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x74) || (class4.b[num + 2] == 0x54))) && (((class4.b[num + 3] == 100) || (class4.b[num + 3] == 0x44)) || ((class4.b[num + 3] == 0x68) || (class4.b[num + 3] == 0x48)))) && (class4.b[num + 4] == 0x3e))
    {
        buf.Add(@"\cell");
        num11 += 5;
        num += 4;
        nest_tables _tables11 = _tablesArray[num36];
        _tables11.cell--;
        if (this._preserveBackgroundColor == 1)
        {
            _tablesArray[num36].td_bg = 0;
        }
        if (this._preserveAlignment == 1)
        {
            align.Clear();
            align.Add(class22);
            buf.Add(align);
            num11 += align.len;
        }
        goto Label_134B7;
    }
    if (((((_tablesArray[num36].table && (class4.b[num] == 60)) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x74) || (class4.b[num + 2] == 0x54))) && ((class4.b[num + 3] == 0x72) || (class4.b[num + 3] == 0x52))) && (class4.b[num + 4] == 0x3e))
    {
        while (_tablesArray[num36].cell > 0)
        {
            buf.Add(this.LF + @"\pard \intbl\cell");
            num11 += 0x12;
            nest_tables _tables12 = _tablesArray[num36];
            _tables12.cell--;
        }
        buf.Add(@"\pard \intbl \row" + this.LF);
        num11 += 0x12;
        num += 4;
        _tablesArray[num36].table = false;
        goto Label_134B7;
    }
}
if (this._preserveNestedTables != 1)
{
    goto Label_1143D;
}
if (((((class4.b[num] == 60) && ((class4.b[num + 1] == 0x74) || (class4.b[num + 1] == 0x54))) && ((class4.b[num + 2] == 0x61) || (class4.b[num + 2] == 0x41))) && (((class4.b[num + 3] == 0x62) || (class4.b[num + 3] == 0x42)) && ((class4.b[num + 4] == 0x6c) || (class4.b[num + 4] == 0x4c)))) && (((class4.b[num + 5] == 0x65) || (class4.b[num + 5] == 0x45)) && ((class4.b[num + 6] == 0x3e) || this.IS_DELIMITER(class4.b[num + 6]))))
{
    if ((_tablesArray[num36].table && (_tablesArray[num36].cell == 0)) && _tablesArray[num36].tr_open)
    {
        if (_tablesArray[num36].table_level > 1)
        {
            buf.Add(this.LF + @"\pard \intbl");
            str11 = @"\itap" + _tablesArray[num36].table_level + " ";
            buf.Add(str11);
            buf.Add(_tablesArray[num36].nestTblDescription);
            buf.Add(@"\nestrow}{\nonesttables\par}" + this.LF);
            if (_tablesArray[num36].tr_cur == _tablesArray[num36].table_p.rows)
            {
                nest_tables _tables13 = _tablesArray[num36];
                _tables13.table_level--;
            }
        }
        else
        {
            buf.Add(@"\pard\intbl\row" + this.LF);
            _tablesArray[num36].table = false;
        }
        this.table_clear_carts(_tablesArray[num36]);
        this.nested_table_clear(_tablesArray[num36]);
        if (num36 > 0)
        {
            num36--;
        }
    }
    if (this._preserveNestedTables == 1)
    {
        if (_tablesArray[num36].table_level < 1)
        {
            nest_tables _tables14 = _tablesArray[num36];
            _tables14.table_level++;
        }
        else
        {
            num36++;
            _tablesArray[num36].table_level = num36 + 1;
        }
        if (align.b[2] == 0x72)
        {
            _tablesArray[num36].table_p.tableAlign = 2;
        }
        else if ((align.b[2] == 0x63) || (num52 == 1))
        {
            _tablesArray[num36].table_p.tableAlign = 1;
        }
        else
        {
            _tablesArray[num36].table_p.tableAlign = 0;
        }
        if ((this.table_getsize(class4, num, max, _tablesArray[num36].table_p) == table_types.TABLE_UNCLOSED) || (_tablesArray[num36].table_p.cols > (this.MAX_COLUMNS - 1)))
        {
            this.table_skip_table2(class4, ref num, max);
            if (_tablesArray[num36].table_level == 1)
            {
                nest_tables _tables15 = _tablesArray[num36];
                _tables15.table_level--;
            }
            else
            {
                _tablesArray[num36].table_level = num36 - 1;
                num36--;
            }
            goto Label_134B7;
        }
        if ((_tablesArray[num36].table_p.cols == 0) || (_tablesArray[num36].table_p.rows == 0))
        {
            this.table_skip_table2(class4, ref num, max);
            if (_tablesArray[num36].table_level == 1)
            {
                nest_tables _tables16 = _tablesArray[num36];
                _tables16.table_level--;
            }
            else
            {
                _tablesArray[num36].table_level = num36 - 1;
                num36--;
            }
            goto Label_134B7;
        }
        this.table_clear_carts(_tablesArray[num36]);
        this.table_alloc_carts(_tablesArray[num36]);
        if (_tablesArray[num36].table_level > 1)
        {
            tBLEN = this.get_max_tblen_width(_tablesArray[num36 - 1]);
        }
        else
        {
            tBLEN = this.TBLEN;
        }
        this.table_analyse(class4, num, max, _tablesArray[num36].table_array, _tablesArray[num36].table_symbols, _tablesArray[num36].table_map, _tablesArray[num36].table_colspan, _tablesArray[num36].table_rowspan, _tablesArray[num36].table_images, _tablesArray[num36].table_width, _tablesArray[num36].table_colbg, _tablesArray[num36].table_valign, _params, _tablesArray[num36].table_p, list2, num14, class25, tBLEN, hieroglyph, _tablesArray[num36].tdAlignColgroup);
        this.table_free_carts(_tablesArray[num36]);
    }
    _tablesArray[num36].table_border_visible = flag17;
    if (this.IS_DELIMITER(class4.b[num + 6]))
    {
        if (this._borderVisibility == eBorderVisibility.SameAsOriginalHtml)
        {
            _tablesArray[num36].table_border_visible = false;
            _tablesArray[num36].table_border_visible = this.get_border(class4, num, max);
        }
        while ((class4.b[num + 6] != 0x3e) && (num < max))
        {
            num++;
        }
    }
    if (_tablesArray[num36].table_level > 1)
    {
        if (flag21)
        {
            buf.Add(str15);
            if (num36 > 0)
            {
                buf.Add(@"\intbl");
                str11 = @"\itap" + ((_tablesArray[num36].table_level - 1)).ToString() + " ";
                buf.Add(str11);
                num11 += str11.Length;
            }
        }
    }
    else
    {
        if (_tablesArray[num36].table)
        {
            while (_tablesArray[num36].cell > 0)
            {
                buf.Add(@"\pard \intbl" + this.LF + @"\cell");
                nest_tables _tables17 = _tablesArray[num36];
                _tables17.cell--;
            }
            _tablesArray[num36].td_open = false;
            buf.Add(this.LF + @"\pard \intbl \row\pard" + this.LF);
        }
        if (flag21 && (num5 == 0))
        {
            buf.Add(this.LF + @"\pard\par" + this.LF);
            num11 += 11;
        }
    }
    num += 6;
    if (!this.check_tr_tag(class4, num, max))
    {
        this.check_tr_close(class4, num, max);
        if ((_tablesArray[num36].table_level <= 1) && _tablesArray[num36].table)
        {
            while (_tablesArray[num36].cell > 0)
            {
                buf.Add(this.LF + @"\pard\intbl\cell");
                nest_tables _tables18 = _tablesArray[num36];
                _tables18.cell--;
            }
            buf.Add(@"\pard\intbl\row" + this.LF);
        }
        if (((buf.len > 1) && (buf.b[buf.len - 1] == 0x20)) && this.IS_DELIMITER(buf.b[buf.len - 2]))
        {
            buf.len--;
        }
        if (_tablesArray[num36].table_level > 1)
        {
            _tablesArray[num36].nestTblDescription = "";
            _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + str2;
            _tablesArray[num36].td = _tablesArray[num36].table_map[_tablesArray[num36].tr_cur, 0];
            _tablesArray[num36].tblen = this.get_table_width(_tablesArray[num36], _tablesArray[num36 - 1]);
        }
        else
        {
            buf.Add(s);
            _tablesArray[num36].td = _tablesArray[num36].table_map[_tablesArray[num36].tr_cur, 0];
            _tablesArray[num36].tblen = this.TBLEN;
            _tablesArray[num36].td_up_columns = _tablesArray[num36].td;
            _tablesArray[num36].cell = _tablesArray[num36].td;
            _tablesArray[num36].td_up_curcol = 0;
        }
        _tablesArray[num36].table = true;
        if (_tablesArray[num36].td != 0)
        {
            for (num7 = 0; num7 < _tablesArray[num36].td; num7++)
            {
                if (_tablesArray[num36].table_border_visible)
                {
                    if (_tablesArray[num36].table_level > 1)
                    {
                        _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + this.LF + @"\clbrdrl\brdrs\clbrdrt\brdrs\clbrdrr\brdrs\clbrdrb\brdrs";
                    }
                    else
                    {
                        buf.Add(this.LF + @"\clbrdrl\brdrs\clbrdrt\brdrs\clbrdrr\brdrs\clbrdrb\brdrs");
                    }
                }
                if (_tablesArray[num36].table_rowspan[_tablesArray[num36].tr_cur, num7 + 1] > 0)
                {
                    if ((_tablesArray[num36].table_rowspan[_tablesArray[num36].tr_cur, num7 + 1] == this.CLVMGF) || (_tablesArray[num36].table_rowspan[_tablesArray[num36].tr_cur, num7 + 1] == this.CLVMRG2))
                    {
                        if (_tablesArray[num36].table_level > 1)
                        {
                            str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
                            _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + str11;
                        }
                        if (_tablesArray[num36].table_level > 1)
                        {
                            _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + @"\clvmgf";
                        }
                        else
                        {
                            buf.Add(@"\clvmgf");
                        }
                    }
                    else if (_tablesArray[num36].table_rowspan[_tablesArray[num36].tr_cur, num7 + 1] == this.CLVMRG)
                    {
                        if (_tablesArray[num36].table_level > 1)
                        {
                            str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
                            _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + str11;
                        }
                        if (_tablesArray[num36].table_level > 1)
                        {
                            _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + @"\clvmrg";
                        }
                        else
                        {
                            buf.Add(@"\clvmrg");
                        }
                    }
                }
                if (_tablesArray[num36].table_level > 1)
                {
                    _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + this.LF + @"\cellx";
                }
                else
                {
                    buf.Add(this.LF + @"\cellx");
                }
                str12 = this.table_make_cellx(_tablesArray[num36].table_map, _tablesArray[num36].tr_cur, num7);
                if (_tablesArray[num36].table_level > 1)
                {
                    _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + str12;
                }
                else
                {
                    buf.Add(str12);
                }
            }
        }
    }
    goto Label_134B7;
}
if ((((((class4.b[num] != 60) || (class4.b[num + 1] != 0x2f)) || ((class4.b[num + 2] != 0x74) && (class4.b[num + 2] != 0x54))) || ((class4.b[num + 3] != 0x61) && (class4.b[num + 3] != 0x41))) || (((class4.b[num + 4] != 0x62) && (class4.b[num + 4] != 0x42)) || ((class4.b[num + 5] != 0x6c) && (class4.b[num + 5] != 0x4c)))) || (((class4.b[num + 6] != 0x65) && (class4.b[num + 6] != 0x45)) || (class4.b[num + 7] != 0x3e)))
{
    goto Label_F56E;
}
if (_tablesArray[num36].tr_open && _tablesArray[num36].table)
{
    if (_tablesArray[num36].table_level <= 1)
    {
        goto Label_F435;
    }
    while (_tablesArray[num36].cell > 0)
    {
        buf.Add(this.LF + @"\pard\intbl");
        str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
        buf.Add(str11);
        buf.Add(this.LF + @"\nestcell");
        nest_tables _tables19 = _tablesArray[num36];
        _tables19.cell--;
    }
    buf.Add(this.LF + @"\pard\intbl");
    str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
    buf.Add(str11);
    buf.Add(_tablesArray[num36].nestTblDescription);
    buf.Add(@"\nestrow}{\nonesttables\par}" + this.LF);
}
goto Label_F464;
Label_F40C:
buf.Add(this.LF + @"\pard\intbl\cell");
nest_tables _tables20 = _tablesArray[num36];
_tables20.cell--;
Label_F435:
if (_tablesArray[num36].cell > 0)
{
    goto Label_F40C;
}
buf.Add(@"\pard\intbl\row" + this.LF);
_tablesArray[num36].table = false;
Label_F464:
if ((_tablesArray[num36].table_level > 1) || (num36 > 0))
{
    _tablesArray[num36].table = false;
    if (buf.len > 0)
    {
        while ((buf.b[buf.len - 1] != 10) && (buf.len > 1))
        {
            buf.len--;
        }
    }
}
else
{
    if (buf.len > 1)
    {
        while ((buf.len > 1) && this.IS_DELIMITER(buf.b[buf.len - 1]))
        {
            buf.len--;
        }
    }
    buf.Add(@"\pard" + this.LF);
}
num += 7;
if (this._preserveNestedTables == 1)
{
    this.table_clear_carts(_tablesArray[num36]);
    this.nested_table_clear(_tablesArray[num36]);
    if (num36 > 0)
    {
        num36--;
        if (num36 == 0)
        {
            buf.Add(@"\itap1" + this.LF);
        }
    }
}
flag21 = false;
num34 = 0;
flag12 = false;
goto Label_134B7;
Label_F56E:
if ((((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x63) || (class4.b[num + 2] == 0x43))) && ((class4.b[num + 3] == 0x61) || (class4.b[num + 3] == 0x41))) && (((class4.b[num + 4] == 0x70) || (class4.b[num + 4] == 80)) && ((class4.b[num + 5] == 0x74) || (class4.b[num + 5] == 0x54)))) && ((((class4.b[num + 6] == 0x69) || (class4.b[num + 6] == 0x49)) && ((class4.b[num + 7] == 0x6f) || (class4.b[num + 7] == 0x4f))) && (((class4.b[num + 8] == 110) || (class4.b[num + 8] == 0x4e)) && (class4.b[num + 9] == 0x3e))))
{
    if (_tablesArray[num36].table)
    {
        buf.Add(str15);
        num5 = 1;
        if ((num36 > 0) && (this._preserveNestedTables == 1))
        {
            str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
            buf.Add(str11);
        }
    }
    else
    {
        buf.Add(str14);
        num5 = 1;
    }
    num += 9;
    goto Label_134B7;
}
if ((((class4.b[num] == 60) && ((class4.b[num + 1] == 0x74) || (class4.b[num + 1] == 0x54))) && ((class4.b[num + 2] == 0x72) || (class4.b[num + 2] == 0x52))) && ((class4.b[num + 3] == 0x3e) || this.IS_DELIMITER(class4.b[num + 3])))
{
    num += 3;
    if (this.IS_DELIMITER(class4.b[num]))
    {
        while (!this.IS_XTHAN(class4.b[num]) && (num < class4.len))
        {
            if ((((((this._preserveAlignment == 1) && (class4.b[num] == 0x20)) && ((class4.b[num + 1] == 0x61) || (class4.b[num + 1] == 0x41))) && ((class4.b[num + 2] == 0x6c) || (class4.b[num + 2] == 0x4c))) && (((class4.b[num + 3] == 0x69) || (class4.b[num + 3] == 0x49)) && ((class4.b[num + 4] == 0x67) || (class4.b[num + 4] == 0x47)))) && ((class4.b[num + 5] == 110) || (class4.b[num + 5] == 0x4e)))
            {
                num += 6;
                this.read_value(class4, ref num, class16);
                if (class16.byteCmpi("center") == 0)
                {
                    _tablesArray[num36].tr_align = class19.ByteToString();
                }
                else if (class16.byteCmpi("middle") == 0)
                {
                    _tablesArray[num36].tr_align = class19.ByteToString();
                }
                else if (class16.byteCmpi("left") == 0)
                {
                    _tablesArray[num36].tr_align = class20.ByteToString();
                }
                else if (class16.byteCmpi("right") == 0)
                {
                    _tablesArray[num36].tr_align = class21.ByteToString();
                }
            }
            if (((((this._preserveBackgroundColor == 1) && ((class4.b[num] == 0x62) || (class4.b[num] == 0x42))) && ((class4.b[num + 1] == 0x67) || (class4.b[num + 1] == 0x47))) && (((class4.b[num + 2] == 0x63) || (class4.b[num + 2] == 0x43)) && ((class4.b[num + 3] == 0x6f) || (class4.b[num + 3] == 0x4f)))) && ((((class4.b[num + 4] == 0x6c) || (class4.b[num + 4] == 0x4c)) && ((class4.b[num + 5] == 0x6f) || (class4.b[num + 5] == 0x4f))) && (((class4.b[num + 6] == 0x72) || (class4.b[num + 6] == 0x52)) && (class4.b[num + 7] == 0x3d))))
            {
                num += 8;
                if (this.read_color(class4, ref num, max, class11))
                {
                    for (num7 = 0; num7 < num14; num7++)
                    {
                        if (class11.byteCmpi((string)list2[num7]) == 0)
                        {
                            _tablesArray[num36].tr_bg = num7 + 3;
                        }
                    }
                }
            }
            num++;
        }
    }
    if ((_tablesArray[num36].table_p.cols == 0) || (_tablesArray[num36].table_p.rows == 0))
    {
        this.table_skip_table(class4, ref num, max);
    }
    else
    {
        if (_tablesArray[num36].table_level > 1)
        {
            if (_tablesArray[num36].table && _tablesArray[num36].tr_open)
            {
                while (_tablesArray[num36].cell > 0)
                {
                    buf.Add(this.LF + @"\pard\intbl");
                    str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
                    buf.Add(str11);
                    buf.Add(this.LF + @"\nestcell");
                    nest_tables _tables21 = _tablesArray[num36];
                    _tables21.cell--;
                }
                buf.Add(this.LF + @"\pard\intbl");
                str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
                buf.Add(str11);
                buf.Add(_tablesArray[num36].nestTblDescription);
                buf.Add(@"\nestrow}{\nonesttables\par}" + this.LF);
            }
        }
        else if (_tablesArray[num36].table)
        {
            while (_tablesArray[num36].cell > 0)
            {
                buf.Add(this.LF + @"\pard\intbl\cell");
                nest_tables _tables22 = _tablesArray[num36];
                _tables22.cell--;
            }
            buf.Add(@"\pard\intbl\row" + this.LF);
        }
        if (((buf.len > 2) && (buf.b[buf.len - 1] == 0x20)) && this.IS_DELIMITER(buf.b[buf.len - 2]))
        {
            buf.len--;
        }
        if (_tablesArray[num36].table_level > 1)
        {
            _tablesArray[num36].nestTblDescription = "";
            _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + str2;
            if (_tablesArray[num36].table_p.tableAlign != 0)
            {
                if (_tablesArray[num36].table_p.tableAlign == this.ALIGN_CENTER)
                {
                    _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + @"\trqc ";
                }
                else if (_tablesArray[num36].table_p.tableAlign == this.ALIGN_RIGHT)
                {
                    _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + @"\trqr ";
                }
            }
            _tablesArray[num36].td = _tablesArray[num36].table_map[_tablesArray[num36].tr_cur, 0];
            _tablesArray[num36].td_up_columns = _tablesArray[num36].td;
            _tablesArray[num36].cell = _tablesArray[num36].td;
            _tablesArray[num36].td_up_curcol = 0;
            if (_tablesArray[num36].change_width != 1)
            {
                _tablesArray[num36].tblen = this.get_table_width(_tablesArray[num36], _tablesArray[num36 - 1]);
            }
        }
        else
        {
            buf.Add(s);
            if (_tablesArray[num36].table_p.tableAlign != 0)
            {
                if (_tablesArray[num36].table_p.tableAlign == this.ALIGN_CENTER)
                {
                    buf.Add(@"\trqc ");
                }
                else if (_tablesArray[num36].table_p.tableAlign == this.ALIGN_RIGHT)
                {
                    buf.Add(@"\trqr ");
                }
            }
            _tablesArray[num36].td = _tablesArray[num36].table_map[_tablesArray[num36].tr_cur, 0];
            _tablesArray[num36].tblen = this.TBLEN;
            _tablesArray[num36].td_up_columns = _tablesArray[num36].td;
            _tablesArray[num36].cell = _tablesArray[num36].td;
            _tablesArray[num36].td_up_curcol = 0;
        }
        _tablesArray[num36].table = true;
        if (_tablesArray[num36].td != 0)
        {
            for (num7 = 0; num7 < _tablesArray[num36].td; num7++)
            {
                if (_tablesArray[num36].table_border_visible)
                {
                    if (_tablesArray[num36].table_level > 1)
                    {
                        _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + this.LF + @"\clbrdrl\brdrs\clbrdrt\brdrs\clbrdrr\brdrs\clbrdrb\brdrs";
                    }
                    else
                    {
                        buf.Add(this.LF + @"\clbrdrl\brdrs\clbrdrt\brdrs\clbrdrr\brdrs\clbrdrb\brdrs");
                    }
                }
                if ((this._preserveBackgroundColor == 1) && (_tablesArray[num36].tr_bg != 0))
                {
                    if (_tablesArray[num36].table_level > 1)
                    {
                        _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + @"\clcbpat" + _tablesArray[num36].tr_bg.ToString() + " ";
                    }
                    else
                    {
                        buf.Add(@"\clcbpat" + _tablesArray[num36].tr_bg.ToString() + " ");
                    }
                }
                if (_tablesArray[num36].table_rowspan[_tablesArray[num36].tr_cur, num7 + 1] > 0)
                {
                    if ((_tablesArray[num36].table_rowspan[_tablesArray[num36].tr_cur, num7 + 1] == this.CLVMGF) || (_tablesArray[num36].table_rowspan[_tablesArray[num36].tr_cur, num7 + 1] == this.CLVMRG2))
                    {
                        if (_tablesArray[num36].table_level > 1)
                        {
                            str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
                            _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + str11;
                        }
                        if (_tablesArray[num36].table_level > 1)
                        {
                            _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + @"\clvmgf";
                        }
                        else
                        {
                            buf.Add(@"\clvmgf");
                        }
                    }
                    else if (_tablesArray[num36].table_rowspan[_tablesArray[num36].tr_cur, num7 + 1] == this.CLVMRG)
                    {
                        if (_tablesArray[num36].table_level > 1)
                        {
                            str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
                            _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + str11;
                        }
                        if (_tablesArray[num36].table_level > 1)
                        {
                            _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + @"\clvmrg";
                        }
                        else
                        {
                            buf.Add(@"\clvmrg");
                        }
                    }
                }
                if (((this._preserveBackgroundColor == 1) && (_tablesArray[num36].table_colbg[_tablesArray[num36].tr_cur, num7 + 1] > 0)) && (_tablesArray[num36].table_colbg[_tablesArray[num36].tr_cur, num7 + 1] < (num14 + 5)))
                {
                    if (_tablesArray[num36].table_level > 1)
                    {
                        _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + @"\clcbpat" + _tablesArray[num36].table_colbg[_tablesArray[num36].tr_cur, num7 + 1].ToString();
                    }
                    else
                    {
                        buf.Add(@"\clcbpat" + _tablesArray[num36].table_colbg[_tablesArray[num36].tr_cur, num7 + 1].ToString());
                    }
                }
                if (this._preserveAlignment == 1)
                {
                    if (_tablesArray[num36].table_valign[_tablesArray[num36].tr_cur, num7 + 1] == this.VALIGN_CENTER)
                    {
                        if (_tablesArray[num36].table_level > 1)
                        {
                            _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + @"\clvertalc";
                        }
                        else
                        {
                            buf.Add(@"\clvertalc");
                        }
                    }
                    else if (_tablesArray[num36].table_valign[_tablesArray[num36].tr_cur, num7 + 1] == this.VALIGN_BOTTOM)
                    {
                        if (_tablesArray[num36].table_level > 1)
                        {
                            _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + @"\clvertalb";
                        }
                        else
                        {
                            buf.Add(@"\clvertalb");
                        }
                    }
                }
                if (_tablesArray[num36].table_level > 1)
                {
                    _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + this.LF + @"\cellx";
                }
                else
                {
                    buf.Add(this.LF + @"\cellx");
                }
                str12 = this.table_make_cellx(_tablesArray[num36].table_map, _tablesArray[num36].tr_cur, num7);
                if (_tablesArray[num36].table_level > 1)
                {
                    _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + str12;
                }
                else
                {
                    buf.Add(str12);
                }
            }
            if ((_tablesArray[num36].td_up_curcol == 0) && (_tablesArray[num36].table_rowspan[_tablesArray[num36].tr_cur, 1] == this.CLVMRG))
            {
                if (_tablesArray[num36].table_level > 1)
                {
                    _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + this.LF + @"\pard\intbl";
                    str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
                    _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + str11;
                    _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + @"\nestcell";
                    nest_tables _tables23 = _tablesArray[num36];
                    _tables23.cell--;
                }
                else
                {
                    buf.Add(@"\pard\intbl\cell");
                    nest_tables _tables24 = _tablesArray[num36];
                    _tables24.cell--;
                }
                nest_tables _tables25 = _tablesArray[num36];
                _tables25.td_up_curcol++;
                for (num7 = 2; num7 < _tablesArray[num36].table_p.cols; num7++)
                {
                    if (_tablesArray[num36].table_rowspan[_tablesArray[num36].tr_cur, num7] != this.CLVMRG)
                    {
                        break;
                    }
                    if (_tablesArray[num36].table_level > 1)
                    {
                        _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + this.LF + @"\pard\intbl";
                        str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
                        _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + str11;
                        _tablesArray[num36].nestTblDescription = _tablesArray[num36].nestTblDescription + @"\nestcell";
                        nest_tables _tables26 = _tablesArray[num36];
                        _tables26.cell--;
                    }
                    else
                    {
                        buf.Add(@"\pard \intbl\cell");
                        nest_tables _tables27 = _tablesArray[num36];
                        _tables27.cell--;
                    }
                    nest_tables _tables28 = _tablesArray[num36];
                    _tables28.td_up_curcol++;
                }
                _tablesArray[num36].td_open = false;
            }
        }
        if (_tablesArray[num36].table_p.rows > 1)
        {
            nest_tables _tables29 = _tablesArray[num36];
            _tables29.tr_cur++;
        }
        _tablesArray[num36].tr_open = true;
        _tablesArray[num36].td_open = false;
    }
    goto Label_134B7;
}
if ((((class4.b[num] == 60) && ((class4.b[num + 1] == 0x74) || (class4.b[num + 1] == 0x54))) && (((class4.b[num + 2] == 100) || (class4.b[num + 2] == 0x44)) || ((class4.b[num + 2] == 0x68) || (class4.b[num + 2] == 0x48)))) && ((class4.b[num + 3] == 0x3e) || this.IS_DELIMITER(class4.b[num + 3])))
{
    if ((_tablesArray[num36].table && (_tablesArray[num36].cell == 0)) && _tablesArray[num36].tr_open)
    {
        if (_tablesArray[num36].table_level > 1)
        {
            buf.Add(this.LF + @"\pard\intbl");
            str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
            buf.Add(str11);
            buf.Add(_tablesArray[num36].nestTblDescription);
            buf.Add(@"\nestrow}{\nonesttables\par}" + this.LF);
            if (_tablesArray[num36].tr_cur == _tablesArray[num36].table_p.rows)
            {
                nest_tables _tables30 = _tablesArray[num36];
                _tables30.table_level--;
            }
        }
        else
        {
            buf.Add(@"\pard\intbl\row" + this.LF);
            _tablesArray[num36].table = false;
        }
        this.table_clear_carts(_tablesArray[num36]);
        this.nested_table_clear(_tablesArray[num36]);
        if (num36 > 0)
        {
            num36--;
        }
    }
    if (_tablesArray[num36].td_open)
    {
        if (_tablesArray[num36].table_level > 1)
        {
            buf.Add(this.LF + @"\pard\intbl");
            str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
            buf.Add(str11);
            buf.Add(@"\nestcell");
            nest_tables _tables31 = _tablesArray[num36];
            _tables31.cell--;
        }
        else
        {
            buf.Add(@"\pard\intbl\cell");
            nest_tables _tables32 = _tablesArray[num36];
            _tables32.cell--;
        }
        nest_tables _tables33 = _tablesArray[num36];
        _tables33.td_up_curcol++;
        _tablesArray[num36].td_open = false;
    }
    if ((_tablesArray[num36].tr_cur > 0) && (_tablesArray[num36].table_rowspan[_tablesArray[num36].tr_cur - 1, _tablesArray[num36].td_up_curcol + 1] == this.CLVMRG))
    {
        for (num7 = _tablesArray[num36].td_up_curcol + 1; (_tablesArray[num36].table_rowspan[_tablesArray[num36].tr_cur - 1, num7] == this.CLVMRG) && (num7 < _tablesArray[num36].table_p.cols); num7++)
        {
            if (_tablesArray[num36].table_level > 1)
            {
                buf.Add(this.LF + @"\pard \intbl");
                str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
                buf.Add(str11);
                buf.Add(@"\nestcell");
                nest_tables _tables34 = _tablesArray[num36];
                _tables34.cell--;
            }
            else
            {
                buf.Add(@"\pard\intbl\cell");
                nest_tables _tables35 = _tablesArray[num36];
                _tables35.cell--;
            }
            nest_tables _tables36 = _tablesArray[num36];
            _tables36.td_up_curcol++;
            _tablesArray[num36].td_open = false;
        }
    }
    if (_tablesArray[num36].table_level > 1)
    {
        buf.Add(this.LF + @"\pard\intbl");
        str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
        buf.Add(str11);
    }
    else
    {
        buf.Add(this.LF + @"\pard\intbl ");
    }
    if (((_tablesArray[num36].td_up_curcol >= 0) && (_tablesArray[num36].td_up_curcol < (this.MAX_COLUMNS - 1))) && (_tablesArray[num36].tdAlignColgroup[_tablesArray[num36].td_up_curcol] != 0))
    {
        if (_tablesArray[num36].tdAlignColgroup[_tablesArray[num36].td_up_curcol] == 0x63)
        {
            _tablesArray[num36].td_align = class19.ByteToString();
        }
        else if (_tablesArray[num36].tdAlignColgroup[_tablesArray[num36].td_up_curcol] == 0x6c)
        {
            _tablesArray[num36].td_align = class20.ByteToString();
        }
        else if (_tablesArray[num36].tdAlignColgroup[_tablesArray[num36].td_up_curcol] == 0x72)
        {
            _tablesArray[num36].td_align = class21.ByteToString();
        }
        else if (_tablesArray[num36].tdAlignColgroup[_tablesArray[num36].td_up_curcol] == 0x6a)
        {
            _tablesArray[num36].td_align = class18.ByteToString();
        }
    }
    num3 = num;
    if (this.IS_DELIMITER(class4.b[num + 3]))
    {
        while ((class4.b[num + 3] != 0x3e) && (num < class4.len))
        {
            if (((((this._preserveAlignment == 1) && ((class4.b[num] == 0x61) || (class4.b[num] == 0x41))) && ((class4.b[num + 1] == 0x6c) || (class4.b[num + 1] == 0x4c))) && (((class4.b[num + 2] == 0x69) || (class4.b[num + 2] == 0x49)) && ((class4.b[num + 3] == 0x67) || (class4.b[num + 3] == 0x47)))) && ((class4.b[num + 4] == 110) || (class4.b[num + 4] == 0x4e)))
            {
                num += 5;
                if (class4.b[num] == 0x3a)
                {
                    num++;
                }
                this.read_value_CSS_tolower(class4, ref num, max, class16, 30);
                if (class16.byteCmpi("center") == 0)
                {
                    _tablesArray[num36].td_align = class19.ByteToString();
                }
                else if (class16.byteCmpi("middle") == 0)
                {
                    _tablesArray[num36].td_align = class19.ByteToString();
                }
                else if (class16.byteCmpi("left") == 0)
                {
                    _tablesArray[num36].td_align = class20.ByteToString();
                }
                else if (class16.byteCmpi("right") == 0)
                {
                    _tablesArray[num36].td_align = class21.ByteToString();
                }
            }
            if (((((this._preserveBackgroundColor == 1) && ((class4.b[num] == 0x62) || (class4.b[num] == 0x42))) && ((class4.b[num + 1] == 0x67) || (class4.b[num + 1] == 0x47))) && (((class4.b[num + 2] == 0x63) || (class4.b[num + 2] == 0x43)) && ((class4.b[num + 3] == 0x6f) || (class4.b[num + 3] == 0x4f)))) && ((((class4.b[num + 4] == 0x6c) || (class4.b[num + 4] == 0x4c)) && ((class4.b[num + 5] == 0x6f) || (class4.b[num + 5] == 0x4f))) && (((class4.b[num + 6] == 0x72) || (class4.b[num + 6] == 0x52)) && (class4.b[num + 7] == 0x3d))))
            {
                num += 8;
                this.read_color(class4, ref num, max, class11);
                for (num7 = 0; num7 < num14; num7++)
                {
                    if (class11.byteCmpi((string)list2[num7]) == 0)
                    {
                        _tablesArray[num36].td_bg = num7 + 3;
                    }
                }
            }
            num++;
        }
    }
    if (this._preserveAlignment == 1)
    {
        if (_tablesArray[num36].td_align == null)
        {
            if (_tablesArray[num36].tr_align == null)
            {
                _tablesArray[num36].tr_align = class18.ByteToString();
                buf.Add(_tablesArray[num36].tr_align);
            }
            else
            {
                buf.Add(_tablesArray[num36].tr_align);
            }
        }
        else
        {
            buf.Add(_tablesArray[num36].td_align);
            align.Clear();
            align.Add(_tablesArray[num36].td_align);
        }
    }
    num = num3;
    num += 3;
    num4 = 0;
    flag21 = false;
    _tablesArray[num36].td_open = true;
    flag12 = true;
    this.CSS_insert(class22, class18, class19, class20, class21, align, class4, ref num, buf, ref num11, max, _params, ref flag4, class10, class14, ref num21, ref flag16, ref flag5, newb, list, list2, CSS_tag_type.TD_CSS);
    goto Label_134B7;
}
if (((((_tablesArray[num36].table && (class4.b[num] == 60)) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x74) || (class4.b[num + 2] == 0x54))) && (((class4.b[num + 3] == 100) || (class4.b[num + 3] == 0x44)) || ((class4.b[num + 3] == 0x68) || (class4.b[num + 3] == 0x48)))) && (class4.b[num + 4] == 0x3e))
{
    while (num28 > 0)
    {
        buf.Add((byte)0x7d);
        num11++;
        num28--;
    }
    while (num29 > 0)
    {
        buf.Add((byte)0x7d);
        num11++;
        num29--;
    }
    num += 4;
    this.CSS_close(class22, class18, align, buf, ref num11, _params, ref flag4, class10, class14, ref num21, ref flag16, ref flag5, newb, fontsize, CSS_tag_type.TD_CSS);
    if (_tablesArray[num36].table_level > 1)
    {
        str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
        buf.Add(str11);
        buf.Add(@"\nestcell");
        nest_tables _tables37 = _tablesArray[num36];
        _tables37.cell--;
    }
    else
    {
        buf.Add(@"\cell");
        nest_tables _tables38 = _tablesArray[num36];
        _tables38.cell--;
    }
    nest_tables _tables39 = _tablesArray[num36];
    _tables39.td_up_curcol++;
    _tablesArray[num36].td_open = false;
    if (this._preserveBackgroundColor == 1)
    {
        _tablesArray[num36].td_bg = 0;
    }
    if (this._preserveAlignment == 1)
    {
        align.Clear();
        align.Add(class22);
        _tablesArray[num36].td_align = class22.ByteToString();
    }
    num34 = 0;
    flag12 = false;
    goto Label_134B7;
}
if (((((!_tablesArray[num36].table || (class4.b[num] != 60)) || (class4.b[num + 1] != 0x2f)) || ((class4.b[num + 2] != 0x74) && (class4.b[num + 2] != 0x54))) || ((class4.b[num + 3] != 0x72) && (class4.b[num + 3] != 0x52))) || (class4.b[num + 4] != 0x3e))
{
    goto Label_1143D;
}
if (_tablesArray[num36].table_level <= 1)
{
    goto Label_112B1;
}
while (_tablesArray[num36].cell > 0)
{
    buf.Add(this.LF + @"\pard\intbl");
    str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
    buf.Add(str11);
    buf.Add(this.LF + @"\nestcell");
    nest_tables _tables40 = _tablesArray[num36];
    _tables40.cell--;
}
buf.Add(this.LF + @"\pard\intbl");
str11 = @"\itap" + _tablesArray[num36].table_level.ToString() + " ";
buf.Add(str11);
buf.Add(_tablesArray[num36].nestTblDescription);
buf.Add(@"\nestrow}{\nonesttables\par}" + this.LF);
if (_tablesArray[num36].tr_cur == _tablesArray[num36].table_p.rows)
{
    nest_tables _tables41 = _tablesArray[num36];
    _tables41.table_level--;
}
if ((_tablesArray[num36].tr_cur == 0) && (_tablesArray[num36].table_p.rows == 1))
{
    str11 = @"\intbl\itap" + ((_tablesArray[num36].table_level - 1)).ToString() + " ";
}
else
{
    str11 = @"\intbl\itap" + _tablesArray[num36].table_level.ToString() + " ";
}
buf.Add(str11);
flag16 = false;
goto Label_112E0;
Label_11288:
buf.Add(this.LF + @"\pard\intbl\cell");
nest_tables _tables42 = _tablesArray[num36];
_tables42.cell--;
Label_112B1:
if (_tablesArray[num36].cell > 0)
{
    goto Label_11288;
}
buf.Add(@"\pard\intbl\row" + this.LF);
_tablesArray[num36].table = false;
Label_112E0:
if (this._preserveAlignment == 1)
{
    _tablesArray[num36].tr_align = class18.ByteToString();
}
if (this._preserveBackgroundColor == 1)
{
    _tablesArray[num36].tr_bg = 0;
}
_tablesArray[num36].tr_open = false;
num += 4;
if (_tablesArray[num36].tr_cur == _tablesArray[num36].table_p.rows)
{
    while (num < class4.len)
    {
        if ((((((class4.b[num] == 60) && (class4.b[num + 1] == 0x2f)) && ((class4.b[num + 2] == 0x74) || (class4.b[num + 2] == 0x54))) && ((class4.b[num + 3] == 0x61) || (class4.b[num + 3] == 0x41))) && (((class4.b[num + 4] == 0x62) || (class4.b[num + 4] == 0x42)) && ((class4.b[num + 5] == 0x6c) || (class4.b[num + 5] == 0x4c)))) && (((class4.b[num + 6] == 0x65) || (class4.b[num + 6] == 0x45)) && (class4.b[num + 7] == 0x3e)))
        {
            num--;
            break;
        }
        num++;
    }
}
goto Label_134B7;
Label_1143D:
if ((((class4.b[num] == 60) && ((class4.b[num + 1] == 0x68) || (class4.b[num + 1] == 0x48))) && ((class4.b[num + 2] == 0x72) || (class4.b[num + 2] == 0x52))) && ((class4.b[num + 3] == 0x3e) || this.IS_DELIMITER(class4.b[num + 3])))
{
    num += 3;
    str10 = @"{\*\picprop {\sp{\sn pictureBiLevel}{\sv 1}}}";
    index = num;
    while ((class4.b[num] != 0x3e) && (num < max))
    {
        if (((((class4.b[num] == 0x62) || (class4.b[num] == 0x42)) && ((class4.b[num + 1] == 0x6c) || (class4.b[num + 1] == 0x4c))) && (((class4.b[num + 2] == 0x61) || (class4.b[num + 2] == 0x41)) && ((class4.b[num + 3] == 0x63) || (class4.b[num + 3] == 0x43)))) && ((class4.b[num + 4] == 0x6b) || (class4.b[num + 4] == 0x4b)))
        {
            str10 = @"{\*\picprop {\sp{\sn pictureBiLevel}{\sv 1}}}";
        }
        num++;
    }
    if (this._preserveHR == 1)
    {
        if (this._pageOrientation == ePageOrientation.Landscape)
        {
            num39 = (this.page_height - this.margl) - this.margr;
        }
        else
        {
            num39 = (this.page_width - this.margl) - this.margr;
        }
        if (_tablesArray[num36].table && (_tablesArray[num36].table_p.rows > 1))
        {
            if (_tablesArray[num36].td_up_curcol == 0)
            {
                num39 = _tablesArray[num36].table_map[_tablesArray[num36].tr_cur - 1, _tablesArray[num36].td_up_curcol + 1];
            }
            else
            {
                num39 = _tablesArray[num36].table_map[_tablesArray[num36].tr_cur - 1, _tablesArray[num36].td_up_curcol + 1] - _tablesArray[num36].table_map[_tablesArray[num36].tr_cur - 1, _tablesArray[num36].td_up_curcol];
            }
        }
        class16.Clear();
        strArray = new string[] { this.LF, @"{\par\pict", str10, @"\picscalex", (num39 / 10).ToString(), @"\picscaley6\picw52\pich52\picwgoal1000\pichgoal500\wmetafile8", this.LF };
        class16.Add(string.Concat(strArray));
        buf.Add(class16);
        num11 += class16.len;
        buf.Add(@"0100090000033a0000000000240000000000050000000b0200000000050000000c022300230024000000430f2000cc00000001000100000000002300230000000000280000000100000001000000010018000000000004000000120b0000120b0000000000000000000000000000030000000000}\par ");
        num11 += @"0100090000033a0000000000240000000000050000000b0200000000050000000c022300230024000000430f2000cc00000001000100000000002300230000000000280000000100000001000000010018000000000004000000120b0000120b0000000000000000000000000000030000000000}\par ".Length;
        flag16 = false;
    }
    num = index;
    this.CSS_insert(class22, class18, class19, class20, class21, align, class4, ref num, buf, ref num11, max, _params, ref flag4, class10, class14, ref num21, ref flag16, ref flag5, newb, list, list2, CSS_tag_type.HR_CSS);
    goto Label_134B7;
}
if ((((class4.b[num] != 60) || ((class4.b[num + 1] != 0x69) && (class4.b[num + 1] != 0x49))) || ((class4.b[num + 2] != 0x6d) && (class4.b[num + 2] != 0x4d))) || (((class4.b[num + 3] != 0x67) && (class4.b[num + 3] != 0x47)) || !this.IS_DELIMITER(class4.b[num + 4])))
{
    goto Label_12A9F;
}
if (this._deleteImages == 1)
{
    while ((class4.b[num] != 0x3e) && (num < class4.len))
    {
        num++;
    }
    goto Label_134B7;
}
if (this._preserveImages == 0)
{
    while ((class4.b[num] != 0x3e) && (num < class4.len))
    {
        buf.Add(class4.b[num]);
        num++;
    }
    buf.Add(">");
    goto Label_134B7;
}
num3 = num;
folder.Clear();
class26.Clear();
num41 = -1;
num42 = -1;
while ((class4.b[num] != 0x3e) && (num < class4.len))
{
    if ((((class4.b[num] == 0x73) || (class4.b[num] == 0x53)) && ((class4.b[num + 1] == 0x72) || (class4.b[num + 1] == 0x52))) && ((class4.b[num + 2] == 0x63) || (class4.b[num + 2] == 0x43)))
    {
        while ((class4.b[num] != 0x3e) && (num < class4.len))
        {
            if (class4.b[num] == 0x3d)
            {
                while ((class4.b[num] != 0x3e) && (num < class4.len))
                {
                    if ((class4.b[num] == 0x22) || (class4.b[num] == 0x27))
                    {
                        break;
                    }
                    if ((class4.b[num] == 0x20) && ((class4.b[num + 1] != 0x22) || (class4.b[num + 1] != 0x27)))
                    {
                        num++;
                        break;
                    }
                    if (((class4.b[num + 1] != 0x20) && (class4.b[num + 1] != 0x22)) && (class4.b[num + 1] != 0x27))
                    {
                        num++;
                        break;
                    }
                    num++;
                }
                break;
            }
            num++;
        }
        byte num65 = 0;
        if ((class4.b[num] != 0x22) && (class4.b[num] != 0x27))
        {
            goto Label_11A65;
        }
        num65 = class4.b[num];
        num++;
        while ((class4.b[num] != 0x3e) && (num < class4.len))
        {
            if (class4.b[num] == num65)
            {
                break;
            }
            class26.Add(class4.b[num]);
            num++;
        }
    }
    goto Label_11AA8;
Label_11A4E:
    class26.Add(class4.b[num]);
num++;
Label_11A65:
if (((class4.b[num] != 0x3e) && (num < class4.len)) && (((class4.b[num] != 0x22) && (class4.b[num] != 0x27)) && (class4.b[num] != 0x20)))
{
    goto Label_11A4E;
}
Label_11AA8:
if ((((class4.b[num] == 0x61) || (class4.b[num] == 0x41)) && ((class4.b[num + 1] == 0x6c) || (class4.b[num + 1] == 0x4c))) && (((class4.b[num + 2] == 0x74) || (class4.b[num + 2] == 0x54)) && (class4.b[num + 3] == 0x3d)))
{
    num += 3;
    this.read_value(class4, ref num, valueStr);
}
if (((((class4.b[num] == 0x77) || (class4.b[num] == 0x57)) && ((class4.b[num + 1] == 0x69) || (class4.b[num + 1] == 0x49))) && (((class4.b[num + 2] == 100) || (class4.b[num + 2] == 0x44)) && ((class4.b[num + 3] == 0x74) || (class4.b[num + 3] == 0x54)))) && ((class4.b[num + 4] == 0x68) || (class4.b[num + 4] == 0x48)))
{
    num += 5;
    this.read_value(class4, ref num, valueStr);
    num41 = this.ToInt(valueStr);
}
if (((((class4.b[num] == 0x68) || (class4.b[num] == 0x48)) && ((class4.b[num + 1] == 0x65) || (class4.b[num + 1] == 0x45))) && (((class4.b[num + 2] == 0x69) || (class4.b[num + 2] == 0x49)) && ((class4.b[num + 3] == 0x67) || (class4.b[num + 3] == 0x47)))) && (((class4.b[num + 4] == 0x68) || (class4.b[num + 4] == 0x48)) && ((class4.b[num + 5] == 0x74) || (class4.b[num + 5] == 0x54))))
{
    num += 6;
    this.read_value(class4, ref num, valueStr);
    num42 = this.ToInt(valueStr);
}
num++;
}
if (class26.b[0] == 0x20)
{
    num7 = 0;
    while (class26.b[num7] == 0x20)
    {
        num7++;
    }
    while (num7 < class26.len)
    {
        folder.Add(class26.b[num7]);
        num7++;
    }
    class26.Clear();
    for (num7 = 0; num7 < folder.len; num7++)
    {
        if (((folder.b[num7] == 0x25) && (folder.b[num7 + 1] == 50)) && (folder.b[num7 + 2] == 0x30))
        {
            class26.Add((byte)0x20);
            num7 += 3;
        }
        else if ((((folder.b[num7] == 0x26) && (folder.b[num7 + 1] == 0x61)) && ((folder.b[num7 + 2] == 0x6d) && (folder.b[num7 + 3] == 0x70))) && (folder.b[num7 + 4] == 0x3b))
        {
            class26.Add((byte)0x26);
            num7 += 5;
        }
        else
        {
            class26.Add(folder.b[num7]);
        }
    }
}
string requestUriString = System.Text.Encoding.UTF8.GetString(class26.b, 0, class26.len);
Stream responseStream = null;
bool flag23 = false;
bool flag24 = false;
WebResponse response = null;
if (((((this._baseURL.Length > 6) && ((this._baseURL[0] == 'h') || (this._baseURL[0] == 'H'))) && ((this._baseURL[1] == 't') || (this._baseURL[1] == 'T'))) && (((this._baseURL[2] == 't') || (this._baseURL[2] == 'T')) && ((this._baseURL[3] == 'p') || (this._baseURL[3] == 'P')))) && (((this._baseURL[4] == ':') && (this._baseURL[5] == '/')) && (this._baseURL[6] == '/')))
{
    for (num7 = this._baseURL.Length - 1; num7 > 0; num7--)
    {
        if (this._baseURL[num7] == '/')
        {
            break;
        }
        this._baseURL = this._baseURL.Remove(num7, 1);
    }
    flag24 = true;
}
if ((((class26.b[0] == 0x77) || (class26.b[0] == 0x57)) && ((class26.b[1] == 0x77) || (class26.b[1] == 0x57))) && (((class26.b[2] == 0x77) || (class26.b[2] == 0x57)) && (class26.b[3] == 0x2e)))
{
    valueStr.Clear();
    valueStr.Add("http://");
    valueStr.Add(class26);
    class26.Clear();
    class26.Add(valueStr);
    requestUriString = System.Text.Encoding.UTF8.GetString(class26.b, 0, class26.len);
}
if (((((class26.b[0] == 0x68) || (class26.b[0] == 0x48)) && ((class26.b[1] == 0x74) || (class26.b[1] == 0x54))) && (((class26.b[2] == 0x74) || (class26.b[2] == 0x54)) && ((class26.b[3] == 0x70) || (class26.b[3] == 80)))) && ((class26.b[4] == 0x3a) || (((class26.b[4] == 0x73) || (class26.b[4] == 0x53)) && (class26.b[5] == 0x3a))))
{
    if (this._preserveHttpImages == 0)
    {
        goto Label_134B7;
    }
    this.CheckRemotePath(class26);
    requestUriString = System.Text.Encoding.UTF8.GetString(class26.b, 0, class26.len);
    HttpWebRequest aWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
    this.OnBeforeImageDownload(aWebRequest);
    try
    {
        message.Clear();
        message.Add("Trying to connect with: ");
        message.Add(requestUriString);
        message.Add(this.LF);
        if (this._createTraceFile == 1)
        {
            this.MyTrace(message, this._traceFilePath);
        }
        responseStream = aWebRequest.GetResponse().GetResponseStream();
        if (responseStream.CanRead)
        {
            flag23 = true;
            message.Clear();
            message.Add("Connected OK!" + this.LF);
            if (this._createTraceFile == 1)
            {
                this.MyTrace(message, this._traceFilePath);
            }
        }
    }
    catch (Exception exception)
    {
        flag23 = false;
        message.Clear();
        message.Add(exception.Message);
        message.Add("Error!" + this.LF);
        if (this._createTraceFile == 1)
        {
            this.MyTrace(message, this._traceFilePath);
        }
    }
}
else if (flag24)
{
    requestUriString = this.CombineURLs(this._baseURL, requestUriString);
    HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(requestUriString);
    this.OnBeforeImageDownload(request2);
    try
    {
        message.Clear();
        message.Add("Trying to connect with: ");
        message.Add(requestUriString);
        message.Add(this.LF);
        if (this._createTraceFile == 1)
        {
            this.MyTrace(message, this._traceFilePath);
        }
        response = request2.GetResponse();
        responseStream = response.GetResponseStream();
        if (responseStream.CanRead)
        {
            flag23 = true;
            message.Clear();
            message.Add("Connected OK!" + this.LF);
            if (this._createTraceFile == 1)
            {
                this.MyTrace(message, this._traceFilePath);
            }
        }
    }
    catch (Exception exception2)
    {
        flag23 = false;
        message.Clear();
        message.Add(exception2.Message);
        message.Add("Error!" + this.LF);
        if (this._createTraceFile == 1)
        {
            this.MyTrace(message, this._traceFilePath);
        }
    }
}
ByteClass class33 = new ByteClass();
bool flag25 = false;
Image img = null;
if (!flag23)
{
    for (num7 = 0; num7 < class26.len; num7++)
    {
        if (class26.b[num7] == 0x2f)
        {
            class26.b[num7] = 0x5c;
        }
    }
    if (((((class26.b[0] == 0x66) || (class26.b[0] == 70)) && ((class26.b[1] == 0x69) || (class26.b[1] == 0x49))) && (((class26.b[2] == 0x6c) || (class26.b[2] == 0x4c)) && ((class26.b[3] == 0x65) || (class26.b[3] == 0x45)))) && (class26.b[4] == 0x3a))
    {
        num7 = 5;
        while ((class26.b[num7] == 0x5c) && (num7 < class26.len))
        {
            num7++;
        }
        while (num7 < class26.len)
        {
            folder.Add(class26.b[num7]);
            num7++;
        }
        class26.Clear();
        for (num7 = 0; num7 < folder.len; num7++)
        {
            class26.Add(folder.b[num7]);
        }
    }
    folder.Clear();
    for (num7 = 0; num7 < class28.len; num7++)
    {
        folder.Add(class28.b[num7]);
    }
    if ((folder.len > 0) && (folder.b[folder.len - 1] != 0x5c))
    {
        folder.Add((byte)0x5c);
    }
    if (((class26.b[1] != 0x3a) && (class26.b[2] != 0x5c)) && ((class26.b[0] != 0x2e) && (class26.b[1] != 0x2e)))
    {
        for (num7 = 0; num7 < class26.len; num7++)
        {
            folder.Add(class26.b[num7]);
        }
    }
    else
    {
        this.combine_img_path(folder, class26);
    }
    path = "";
    path = System.Text.Encoding.UTF8.GetString(folder.b, 0, folder.len);
    if (!File.Exists(path))
    {
        flag25 = false;
        if ((((class26.b[0] == 0x63) || (class26.b[0] == 0x43)) && ((class26.b[1] == 0x69) || (class26.b[1] == 0x49))) && (((class26.b[2] == 100) || (class26.b[2] == 0x44)) && (class26.b[3] == 0x3a)))
        {
            class33.Clear();
            for (num7 = 4; num7 < class26.len; num7++)
            {
                class33.Add(class26.b[num7]);
            }
            if (ImageList != null)
            {
                for (num7 = 0; num7 < ImageList.Count; num7++)
                {
                    if (class33.byteCmpi(((SautinImage)ImageList[num7]).Cid) == 0)
                    {
                        img = ((SautinImage)ImageList[num7]).Img;
                        flag25 = true;
                        break;
                    }
                }
            }
        }
        if (!flag25)
        {
            message.Clear();
            message.Add("Can't open file from: " + path + this.LF);
            if (this._createTraceFile == 1)
            {
                this.MyTrace(message, this._traceFilePath);
            }
            goto Label_134B7;
        }
    }
}
Image image2 = null;
MemoryStream stream2 = new MemoryStream();
if (flag25)
{
    image2 = img;
}
else
{
    if (flag23)
    {
        message.Clear();
        message.Add("Reading remote image..." + this.LF);
        if (this._createTraceFile == 1)
        {
            this.MyTrace(message, this._traceFilePath);
        }
        try
        {
            image2 = Image.FromStream(responseStream);
            responseStream.Close();
            response.Close();
            goto Label_1274A;
        }
        catch (Exception exception3)
        {
            message.Clear();
            message.Add(exception3.Message);
            message.Add("Reading remote image error!" + this.LF);
            if (this._createTraceFile == 1)
            {
                this.MyTrace(message, this._traceFilePath);
            }
            goto Label_134B7;
        }
    }
    message.Clear();
    message.Add("Reading local image..." + this.LF);
    if (this._createTraceFile == 1)
    {
        this.MyTrace(message, this._traceFilePath);
    }
    try
    {
        image2 = Image.FromFile(path);
    }
    catch (Exception exception4)
    {
        message.Clear();
        message.Add(exception4.Message);
        message.Add("Reading local image error!" + this.LF);
        if (this._createTraceFile == 1)
        {
            this.MyTrace(message, this._traceFilePath);
        }
        goto Label_134B7;
    }
}
Label_1274A:
if (path.ToLower().IndexOf(".png", 0, path.Length) > 0)
{
    this.imageType = eImageType.Png;
}
else if (path.ToLower().IndexOf(".jpg", 0, path.Length) > 0)
{
    this.imageType = eImageType.Jpeg;
}
else if (path.ToLower().IndexOf(".jpeg", 0, path.Length) > 0)
{
    this.imageType = eImageType.Jpeg;
}
else if (path.ToLower().IndexOf(".gif", 0, path.Length) > 0)
{
    this.imageType = eImageType.Gif;
}
else if (path.ToLower().IndexOf(".bmp", 0, path.Length) > 0)
{
    this.imageType = eImageType.Bmp;
}
if (this._imageCompatible == eImageCompatible.image_Word)
{
    if (this.imageType == eImageType.Jpeg)
    {
        image2.Save(stream2, ImageFormat.Jpeg);
    }
    else
    {
        image2.Save(stream2, ImageFormat.Png);
    }
}
else
{
    image2.Save(stream2, ImageFormat.Bmp);
}
if (stream2.Length != 0L)
{
    int num68;
    int num69;
    message.Clear();
    message.Add("Image was converted OK!" + this.LF);
    if (this._createTraceFile == 1)
    {
        this.MyTrace(message, this._traceFilePath);
    }
    byte[] buffer = stream2.GetBuffer();
    stream2.Close();
    str8 = "";
    int width = image2.Width;
    int height = image2.Height;
    if (num41 >= 0)
    {
        num68 = num41 * 15;
    }
    else
    {
        num68 = width * 15;
    }
    if (num42 >= 0)
    {
        num69 = num42 * 15;
    }
    else
    {
        num69 = height * 15;
    }
    int num70 = 100;
    int num71 = 100;
    if (this._imageCompatible == eImageCompatible.image_Word)
    {
        if (this.imageType == eImageType.Jpeg)
        {
            str8 = string.Format(@"{{\pict\jpegblip\picw{0:D}\pich{1:D}\picwgoal{2:D}\pichgoal{3:D}\picscalex{4:D}\picscaley{5:D}" + this.LF, new object[] { width * 15, height * 15, num68, num69, num70, num71 });
        }
        else
        {
            str8 = string.Format(@"{{\pict\pngblip\picw{0:D}\pich{1:D}\picwgoal{2:D}\pichgoal{3:D}\picscalex{4:D}\picscaley{5:D}" + this.LF, new object[] { width * 15, height * 15, num68, num69, num70, num71 });
        }
    }
    else
    {
        str8 = string.Format(@"{{\pict\dibitmap0\picw{0:D}\pich{1:D}\picwgoal{2:D}\pichgoal{3:D}\picscalex{4:D}\picscaley{5:D}" + this.LF, new object[] { (int)(width * 26.45), (int)(height * 26.45), num68, num69, num70, num71 });
    }
    buf.Add(str8);
    str8 = "";
    if (this._imageCompatible == eImageCompatible.image_Word)
    {
        num7 = 0;
    }
    else
    {
        num7 = 14;
    }
    while (num7 < buffer.Length)
    {
        this.ByteToHexByteClass(buffer[num7], valueStr);
        buf.Add(valueStr);
        num7++;
    }
    buf.Add("}");
}
goto Label_134B7;
Label_12A9F:
if (flag11)
{
    if (class4.b[num] == 10)
    {
        buf.Add(@"\par" + this.LF);
        goto Label_134B7;
    }
    if (class4.b[num] == 0x20)
    {
        buf.Add(" ");
        goto Label_134B7;
    }
    if (class4.b[num] == 9)
    {
        buf.Add(@"\tab" + this.LF);
        goto Label_134B7;
    }
    if (class4.b[num] == 0x3e)
    {
        goto Label_134B7;
    }
    if (class4.b[num] == 60)
    {
        while ((class4.b[num] != 0x3e) && (num < max))
        {
            if ((((this._preserveHyperlinks == 1) && ((class4.b[num] == 0x68) || (class4.b[num] == 0x48))) && ((class4.b[num + 1] == 0x72) || (class4.b[num + 1] == 0x52))) && ((((class4.b[num + 2] == 0x65) || (class4.b[num + 2] == 0x45)) && ((class4.b[num + 3] == 0x66) || (class4.b[num + 3] == 70))) && ((class4.b[num + 4] == 0x3d) && (class4.b[num + 5] == 0x22))))
            {
                num--;
                break;
            }
            num++;
        }
        goto Label_134B7;
    }
}
if (num11 <= 0x11463)
{
    goto Label_12D68;
}
if (!_tablesArray[num36].table)
{
    break;
}
if (this._preserveNestedTables != 1)
{
    goto Label_12D4A;
}
goto Label_12D22;
Label_12C47:
buf.Add("\n\\pard \\intbl");
str11 = @"\itap" + _tablesArray[num36].table_level + " ";
buf.Add(str11);
num11 += str11.Length;
buf.Add("\n\\nestcell");
nest_tables _tables43 = _tablesArray[num36];
_tables43.cell--;
Label_12CA6:
if (_tablesArray[num36].cell > 0)
{
    goto Label_12C47;
}
buf.Add("\n\\pard \\intbl");
str11 = @"\itap" + _tablesArray[num36].table_level + " ";
buf.Add(str11);
num11 += str11.Length;
buf.Add(_tablesArray[num36].nestTblDescription);
buf.Add(@"\nestrow}{\nonesttables\par}" + this.LF);
num36--;
Label_12D22:
if (num36 > 0)
{
    goto Label_12CA6;
}
Label_12D4A:
while (_tablesArray[num36].cell > 0)
{
    buf.Add("\n\\pard \\intbl\\cell");
    nest_tables _tables44 = _tablesArray[num36];
    _tables44.cell--;
}
buf.Add("\n\\pard \\intbl \\row\\pard");
break;
Label_12D68:
if (num11 != 0x11167)
{
    goto Label_12EB5;
}
if (!_tablesArray[num36].table)
{
    break;
}
if (this._preserveNestedTables != 1)
{
    goto Label_12E97;
}
goto Label_12E6F;
Label_12D94:
buf.Add("\n\\pard \\intbl");
str11 = @"\itap" + _tablesArray[num36].table_level + " ";
buf.Add(str11);
num11 += str11.Length;
buf.Add("\n\\nestcell");
nest_tables _tables45 = _tablesArray[num36];
_tables45.cell--;
Label_12DF3:
if (_tablesArray[num36].cell > 0)
{
    goto Label_12D94;
}
buf.Add("\n\\pard \\intbl");
str11 = @"\itap" + _tablesArray[num36].table_level + " ";
buf.Add(str11);
num11 += str11.Length;
buf.Add(_tablesArray[num36].nestTblDescription);
buf.Add(@"\nestrow}{\nonesttables\par}" + this.LF);
num36--;
Label_12E6F:
if (num36 > 0)
{
    goto Label_12DF3;
}
Label_12E97:
while (_tablesArray[num36].cell > 0)
{
    buf.Add("\n\\pard \\intbl\\cell");
    nest_tables _tables46 = _tablesArray[num36];
    _tables46.cell--;
}
buf.Add("\n\\pard \\intbl \\row \\pard");
break;
Label_12EB5:
if (((class4.len > 0) && (num > 0)) && ((class4.b[num - 1] == 0x3e) && (class4.b[num] != 60)))
{
    num4 = 1;
}
if (class4.b[num] == 60)
{
    num4 = 0;
    goto Label_134B7;
}
if ((num4 != 0) || flag11)
{
    if (flag15 && (num5 == 0))
    {
        buf.Add(str14);
        num5 = 1;
    }
    flag15 = false;
    if (this.IS_DELIMITER(class4.b[num]))
    {
        if ((num5 == 0) && (buf.len > 0))
        {
            if (((class4.b[num] == 10) || (class4.b[num] == 13)) && (buf.b[buf.len - 1] == 0x7d))
            {
                if ((buf.len > 2) && (buf.b[buf.len - 2] != 0x20))
                {
                    buf.Add((byte)0x20);
                    num11 = buf.len;
                }
            }
            else if (buf.b[buf.len - 1] != 0x20)
            {
                buf.Add((byte)0x20);
                num11 = buf.len;
            }
            else if (flag16)
            {
                buf.Add((byte)0x20);
                num11++;
                flag21 = true;
            }
            flag16 = false;
        }
    }
    else
    {
        if (class4.b[num] > 0x1f)
        {
            if (hieroglyph)
            {
                this.tohex(class4.b[num], str);
                buf.Add(str);
                num11 += 4;
            }
            else if ((class4.b[num] < 160) || (charset > 0))
            {
                buf.Add(class4.b[num]);
                num11++;
                if (flag13 && (charset > 0))
                {
                    buf.Add(@"\f0 ");
                    num11 += 4;
                    flag13 = false;
                }
            }
            else
            {
                if (!flag13 && (charset > 0))
                {
                    buf.Add(@"\f99 ");
                    num11 += @"\f99 ".Length;
                    flag13 = true;
                }
                this.tohex(class4.b[num], str);
                buf.Add(str);
                num11 += 4;
            }
            if (num52 == 1)
            {
                num52 = 2;
            }
            num5 = 0;
            flag21 = true;
        }
        flag16 = false;
    }
}
if (num11 <= Int32.MaxValue)
{
    goto Label_13220;
}
if (!_tablesArray[num36].table)
{
    break;
}
if (this._preserveNestedTables != 1)
{
    goto Label_13202;
}
goto Label_131DA;
Label_130FF:
buf.Add("\n\\pard \\intbl");
str11 = @"\itap" + _tablesArray[num36].table_level + " ";
buf.Add(str11);
num11 += str11.Length;
buf.Add("\n\\nestcell");
nest_tables _tables47 = _tablesArray[num36];
_tables47.cell--;
Label_1315E:
if (_tablesArray[num36].cell > 0)
{
    goto Label_130FF;
}
buf.Add("\n\\pard \\intbl");
str11 = @"\itap" + _tablesArray[num36].table_level + " ";
buf.Add(str11);
num11 += str11.Length;
buf.Add(_tablesArray[num36].nestTblDescription);
buf.Add(@"\nestrow}{\nonesttables\par}" + this.LF);
num36--;
Label_131DA:
if (num36 > 0)
{
    goto Label_1315E;
}
Label_13202:
while (_tablesArray[num36].cell > 0)
{
    buf.Add("\n\\pard \\intbl\\cell");
    nest_tables _tables48 = _tablesArray[num36];
    _tables48.cell--;
}
buf.Add("\n\\pard \\intbl \\row \\pard");
break;
Label_13220:
if (num11 <= 0x124f7)
{
    goto Label_1336D;
}
if (!_tablesArray[num36].table)
{
    break;
}
if (this._preserveNestedTables != 1)
{
    goto Label_1334F;
}
goto Label_13327;
Label_1324C:
buf.Add("\n\\pard \\intbl");
str11 = @"\itap" + _tablesArray[num36].table_level + " ";
buf.Add(str11);
num11 += str11.Length;
buf.Add("\n\\nestcell");
nest_tables _tables49 = _tablesArray[num36];
_tables49.cell--;
Label_132AB:
if (_tablesArray[num36].cell > 0)
{
    goto Label_1324C;
}
buf.Add("\n\\pard \\intbl");
str11 = @"\itap" + _tablesArray[num36].table_level + " ";
buf.Add(str11);
num11 += str11.Length;
buf.Add(_tablesArray[num36].nestTblDescription);
buf.Add(@"\nestrow}{\nonesttables\par}" + this.LF);
num36--;
Label_13327:
if (num36 > 0)
{
    goto Label_132AB;
}
Label_1334F:
while (_tablesArray[num36].cell > 0)
{
    buf.Add("\n\\pard \\intbl\\cell");
    nest_tables _tables50 = _tablesArray[num36];
    _tables50.cell--;
}
buf.Add("\n\\pard \\intbl \\row \\pard");
break;
Label_1336D:
if (num11 <= 0x1117a)
{
    goto Label_134B7;
}
if (!_tablesArray[num36].table)
{
    break;
}
if (this._preserveNestedTables != 1)
{
    goto Label_1349C;
}
goto Label_13474;
Label_13399:
buf.Add("\n\\pard \\intbl");
str11 = @"\itap" + _tablesArray[num36].table_level + " ";
buf.Add(str11);
num11 += str11.Length;
buf.Add("\n\\nestcell");
nest_tables _tables51 = _tablesArray[num36];
_tables51.cell--;
Label_133F8:
if (_tablesArray[num36].cell > 0)
{
    goto Label_13399;
}
buf.Add("\n\\pard \\intbl");
str11 = @"\itap" + _tablesArray[num36].table_level + " ";
buf.Add(str11);
num11 += str11.Length;
buf.Add(_tablesArray[num36].nestTblDescription);
buf.Add(@"\nestrow}{\nonesttables\par}" + this.LF);
num36--;
Label_13474:
if (num36 > 0)
{
    goto Label_133F8;
}
Label_1349C:
while (_tablesArray[num36].cell > 0)
{
    buf.Add("\n\\pard \\intbl\\cell");
    nest_tables _tables52 = _tablesArray[num36];
    _tables52.cell--;
}
buf.Add("\n\\pard \\intbl \\row \\pard");
break;
Label_134B7:
num++;
                }
            }
            
            if (this._outputTextFormat == eOutputTextFormat.Rtf)
            {
                if (this._rtfParts == eRtfParts.RtfBody)
                {
                    buf.Add("}");
                }
                else
                {
                    buf.Add(@"\pard}");
                }
            }
            int num72 = 0;
            int num73 = 0;
            for (num7 = 0; num7 < buf.len; num7++)
            {
                if (buf.b[num7] == 0x5c)
                {
                    num7++;
                }
                else
                {
                    if (buf.b[num7] == 0x7b)
                    {
                        num72++;
                    }
                    if (buf.b[num7] == 0x7d)
                    {
                        num73++;
                    }
                }
            }
            for (num7 = 0; num7 < (num72 - num73); num7++)
            {
                buf.Add("}");
            }
            return buf.ByteToString();
        }

        public int ConvertStringToFile(string htmlString, string rtfFile)
        {
            string str = "";
            try
            {
                str = this.ConvertString(htmlString);
                StreamWriter writer = new StreamWriter(rtfFile, false);
                writer.Write(str);
                writer.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(exception.Message);
                return 1;
            }
            return 0;
        }

        private void CSS_analyse(CSS_params CSS_param, ByteClass buf, ArrayList font_list, ArrayList color_list)
        {
            int num3;
            int num2 = 0;
            int num4 = 0;
            int num5 = 0;
            int ct = 0;
            ByteClass class2 = new ByteClass();
            buf.ByteToString();
            int index = 0;
            CSS_param.styles = 0;
            while (index < buf.len)
            {
                if ((buf.b[index] == 0x7b) && (num2 == 0))
                {
                    num2++;
                    num3 = index;
                    num5 = this.CSS_read_style_name(buf, ref num3, class2);
                }
                if ((buf.b[index] == 0x7d) && (num2 == 1))
                {
                    if (num5 > 1)
                    {
                        CSS_param.styles += num5;
                    }
                    else
                    {
                        CSS_param.styles++;
                    }
                    num2 = 0;
                    num5 = 0;
                }
                index++;
            }
            for (index = 0; index < CSS_param.styles; index++)
            {
                CSS_styles cSS = new CSS_styles();
                this.CSS_reset_style(cSS);
                CSS_param.CSS_style.Add(cSS);
            }
            int num7 = 0;
            for (num3 = 0; num3 < (buf.len - 1); num3++)
            {
                if ((buf.b[num3] == 0x2f) && (buf.b[num3 + 1] == 0x2a))
                {
                    num7 = 0;
                    while (num3 < (buf.len - 1))
                    {
                        if ((buf.b[num3] == 0x2f) && (buf.b[num3 + 1] == 0x2a))
                        {
                            num7++;
                        }
                        if ((buf.b[num3] == 0x2a) && (buf.b[num3 + 1] == 0x2f))
                        {
                            num7--;
                            buf.b[num3] = 0x20;
                            buf.b[num3 + 1] = 0x20;
                            if (num7 <= 0)
                            {
                                break;
                            }
                        }
                        buf.b[num3] = 0x20;
                        num3++;
                    }
                }
            }
            bool flag = false;
            num3 = 0;
            num4 = 0;
            while ((num3 < buf.len) && (num4 < CSS_param.styles))
            {
                if (buf.b[num3] == 0x7b)
                {
                    num5 = this.CSS_read_style_name(buf, ref num3, ((CSS_styles)CSS_param.CSS_style[num4]).name);
                    if (num5 > 0)
                    {
                        if (num5 > 1)
                        {
                            this.CSS_read_second_name(buf, ref num3, CSS_param.buf_size, ((CSS_styles)CSS_param.CSS_style[num4]).name);
                            this.CSS_parse_name(((CSS_styles)CSS_param.CSS_style[num4]).name, (CSS_styles)CSS_param.CSS_style[num4]);
                            num5--;
                            ct = num3;
                            while ((buf.b[num3] != 0x7b) && (num3 < CSS_param.buf_size))
                            {
                                num3++;
                            }
                        }
                        this.CSS_parse_name(((CSS_styles)CSS_param.CSS_style[num4]).name, (CSS_styles)CSS_param.CSS_style[num4]);
                        if (((((CSS_styles)CSS_param.CSS_style[num4]).name.b[0] == 0x70) || (((CSS_styles)CSS_param.CSS_style[num4]).name.b[0] == 80)) && (((CSS_styles)CSS_param.CSS_style[num4]).name.b[1] == 0x2e))
                        {
                            CSS_param.p_tag = true;
                        }
                        if ((((((CSS_styles)CSS_param.CSS_style[num4]).name.b[0] == 0x65) || (((CSS_styles)CSS_param.CSS_style[num4]).name.b[0] == 0x45)) && ((((CSS_styles)CSS_param.CSS_style[num4]).name.b[1] == 0x6d) || (((CSS_styles)CSS_param.CSS_style[num4]).name.b[1] == 0x4d))) && (((CSS_styles)CSS_param.CSS_style[num4]).name.b[2] == 0x2e))
                        {
                            CSS_param.em_tag = true;
                        }
                        if (((((((CSS_styles)CSS_param.CSS_style[num4]).name.b[0] == 0x62) || (((CSS_styles)CSS_param.CSS_style[num4]).name.b[0] == 0x42)) && ((((CSS_styles)CSS_param.CSS_style[num4]).name.b[1] == 0x6f) || (((CSS_styles)CSS_param.CSS_style[num4]).name.b[1] == 0x4f))) && (((((CSS_styles)CSS_param.CSS_style[num4]).name.b[2] == 100) || (((CSS_styles)CSS_param.CSS_style[num4]).name.b[2] == 0x44)) && ((((CSS_styles)CSS_param.CSS_style[num4]).name.b[3] == 0x79) || (((CSS_styles)CSS_param.CSS_style[num4]).name.b[3] == 0x59)))) && (((CSS_styles)CSS_param.CSS_style[num4]).name.b[4] == 0))
                        {
                            if (CSS_param.body_tag)
                            {
                                flag = true;
                            }
                            else
                            {
                                CSS_param.body_tag = true;
                                CSS_param.body_tag_index = num4;
                            }
                        }
                        if (((((CSS_styles)CSS_param.CSS_style[num4]).name.b[0] == 0x70) || (((CSS_styles)CSS_param.CSS_style[num4]).name.b[0] == 80)) && (((CSS_styles)CSS_param.CSS_style[num4]).name.b[1] == 0))
                        {
                            CSS_param.p_tag_only = true;
                            CSS_param.p_tag_only_index = num4;
                        }
                        this.CSS_read_style_params(buf, ref num3, CSS_param.buf_size, (CSS_styles)CSS_param.CSS_style[num4], CSS_param, font_list, color_list);
                        if (((flag && ((((CSS_styles)CSS_param.CSS_style[num4]).name.b[0] == 0x62) || (((CSS_styles)CSS_param.CSS_style[num4]).name.b[0] == 0x42))) && ((((CSS_styles)CSS_param.CSS_style[num4]).name.b[1] == 0x6f) || (((CSS_styles)CSS_param.CSS_style[num4]).name.b[1] == 0x4f))) && ((((((CSS_styles)CSS_param.CSS_style[num4]).name.b[2] == 100) || (((CSS_styles)CSS_param.CSS_style[num4]).name.b[2] == 0x44)) && ((((CSS_styles)CSS_param.CSS_style[num4]).name.b[3] == 0x79) || (((CSS_styles)CSS_param.CSS_style[num4]).name.b[3] == 0x59))) && (((CSS_styles)CSS_param.CSS_style[num4]).name.b[4] == 0)))
                        {
                            this.CSSJoinStyles((CSS_styles)CSS_param.CSS_style[CSS_param.body_tag_index], (CSS_styles)CSS_param.CSS_style[num4]);
                        }
                        num4++;
                    }
                    if (num5 > 1)
                    {
                        while (num5 != 0)
                        {
                            if (num4 < (CSS_param.styles - 1))
                            {
                                ((CSS_styles)CSS_param.CSS_style[num4 - 1]).CopyCSS_styles((CSS_styles)CSS_param.CSS_style[num4]);
                                this.CSS_read_second_name(buf, ref ct, CSS_param.buf_size, ((CSS_styles)CSS_param.CSS_style[num4]).name);
                                this.CSS_parse_name(((CSS_styles)CSS_param.CSS_style[num4]).name, (CSS_styles)CSS_param.CSS_style[num4]);
                                num4++;
                            }
                            num5--;
                        }
                    }
                }
                num3++;
            }
        }

        private void CSS_check_file_name(ByteClass fname, ByteClass cur_dir)
        {
            ByteClass class2 = new ByteClass();
            int len = fname.len;
            for (int i = 0; i < len; i++)
            {
                if (fname.b[i] == 0x2f)
                {
                    fname.b[i] = 0x5c;
                }
            }
            class2.Clear();
            class2.Add(cur_dir);
            if ((fname.b[1] != 0x3a) && (fname.b[2] != 0x5c))
            {
                class2.Add(fname);
            }
            int num3 = 0;
            for (int j = 0; j < class2.len; j++)
            {
                if (class2.b[j] == 0x5c)
                {
                    if ((num3 > 0) && (fname.b[num3 - 1] != 0x5c))
                    {
                        num3++;
                        fname.Add(class2.b[j]);
                    }
                }
                else
                {
                    num3++;
                    fname.Add(class2.b[j]);
                }
            }
            class2.Clear();
            class2.Add(fname);
        }

        private int CSS_close(ByteClass align_def, ByteClass align_j, ByteClass align, ByteClass buf, ref int p_count2, CSS_params CSS_param, ref bool font_color_default, ByteClass font_color_temp, ByteClass color_stack, ref int color_stack_num, ref bool may_use_space, ref bool font_size_default, ByteClass font_size_temp, int fontsize, CSS_tag_type css_tag_type)
        {
            int num = p_count2;
            int num2 = 0;
            int num3 = 0;
            bool flag = false;
            fontsize *= 2;
            if ((css_tag_type != CSS_tag_type.UNKNOWN_CSS) && this.CSS_del_from_stack_tag(ref num3, CSS_param, css_tag_type))
            {
                num2 = ((CSS_stack_element)CSS_param.stack_tag[num3]).style_index;
                if ((num2 >= 0) && (num2 < CSS_param.styles))
                {
                    this.CSS_close_styles(((CSS_styles)CSS_param.CSS_style[num2]).ablaze, (CSS_styles)CSS_param.CSS_style[num2], CSS_param, align_def, align_j, align, buf, ref num, ref font_color_default, font_color_temp, color_stack, ref color_stack_num, ref may_use_space, ref font_size_default, font_size_temp, fontsize, css_tag_type, null, 0);
                }
                p_count2 = num;
            }
            if (CSS_param.style_str_stack_num > 0)
            {
                flag = false;
                num2 = CSS_param.style_str_stack_num - 1;
                while (num2 >= 0)
                {
                    if (css_tag_type == ((CSS_styles)CSS_param.CSS_style_str_stack[num2]).css_tag_type)
                    {
                        flag = true;
                        break;
                    }
                    num2--;
                }
            }
            if (flag)
            {
                for (int i = CSS_param.style_str_stack_num - 1; i > num2; i--)
                {
                    this.CSS_close(align_def, align_j, align, buf, ref p_count2, CSS_param, ref font_color_default, font_color_temp, color_stack, ref color_stack_num, ref may_use_space, ref font_size_default, font_size_temp, fontsize, ((CSS_styles)CSS_param.CSS_style_str_stack[i]).css_tag_type);
                }
                this.CSS_close_styles(((CSS_styles)CSS_param.CSS_style_str_stack[num2]).ablaze, (CSS_styles)CSS_param.CSS_style_str_stack[num2], CSS_param, align_def, align_j, align, buf, ref num, ref font_color_default, font_color_temp, color_stack, ref color_stack_num, ref may_use_space, ref font_size_default, font_size_temp, fontsize, css_tag_type, null, 0);
                p_count2 = num;
                this.CSS_reset_style((CSS_styles)CSS_param.CSS_style_str_stack[num2]);
                this.CSS_del_from_style_str_stack(num2, CSS_param, css_tag_type);
            }
            if (css_tag_type == CSS_tag_type.SPAN_CSS)
            {
                if (CSS_param.span <= 0)
                {
                    CSS_param.span = 0;
                    return 0;
                }
                CSS_param.span--;
            }
            else if (css_tag_type == CSS_tag_type.DIV_CSS)
            {
                if (align != align_def)
                {
                    align.Clear();
                    align.Add(align_def);
                    buf.Add(align);
                    num += align.len;
                    p_count2 = num;
                }
                if (CSS_param.div <= 0)
                {
                    CSS_param.div = 0;
                    return 0;
                }
                CSS_param.div--;
            }
            else if (css_tag_type == CSS_tag_type.P_CSS)
            {
                if (align != align_def)
                {
                    align.Clear();
                    align.Add(align_def);
                    buf.Add(align);
                    num += align.len;
                    p_count2 = num;
                }
                if (CSS_param.p_tag_open <= 0)
                {
                    CSS_param.p_tag_open = 0;
                    return 0;
                }
                CSS_param.p_tag_open--;
            }
            else if (css_tag_type == CSS_tag_type.EM_CSS)
            {
                if (CSS_param.em_tag_open <= 0)
                {
                    CSS_param.em_tag_open = 0;
                    return 0;
                }
                CSS_param.em_tag_open--;
            }
            else if (css_tag_type == CSS_tag_type.FONT_CSS)
            {
                if (CSS_param.font_tag_open <= 0)
                {
                    CSS_param.font_tag_open = 0;
                    return 0;
                }
                CSS_param.font_tag_open--;
            }
            else if (css_tag_type != CSS_tag_type.TD_CSS)
            {
                return 0;
            }
            if (this.CSS_del_from_stack(ref num3, CSS_param, css_tag_type))
            {
                num2 = ((CSS_stack_element)CSS_param.stack[num3]).style_index;
                if ((num2 < 0) || (num2 > (CSS_param.styles - 1)))
                {
                    return 0;
                }
                this.CSS_close_styles(((CSS_styles)CSS_param.CSS_style[num2]).ablaze, (CSS_styles)CSS_param.CSS_style[num2], CSS_param, align_def, align_j, align, buf, ref num, ref font_color_default, font_color_temp, color_stack, ref color_stack_num, ref may_use_space, ref font_size_default, font_size_temp, fontsize, css_tag_type, CSS_param.stack, CSS_param.stack_num);
                p_count2 = num;
            }
            return 0;
        }

        private bool CSS_close_styles(ByteClass ablaze, CSS_styles style, CSS_params CSS_param, ByteClass align_def, ByteClass align_j, ByteClass align, ByteClass buf, ref int p_count2, ref bool font_color_default, ByteClass font_color_temp, ByteClass color_stack, ref int color_stack_num, ref bool may_use_space, ref bool font_size_default, ByteClass font_size_temp, int fontsize, CSS_tag_type css_tag_type, ArrayList styleStack, int styleStackNum)
        {
            int num = p_count2;
            bool flag = false;
            ByteClass newb = new ByteClass(0x10);
            int num2 = 0;
            if ((CSS_param.pos.open && (ablaze.b[9] == 1)) && ((ablaze.b[10] == 1) && (ablaze.b[11] == 1)))
            {
                buf.Add(CSS_param.pos.pos_close_str);
                num += CSS_param.pos.pos_close_str.Length;
                CSS_param.pos.open = false;
            }
            if ((ablaze.b[2] == 1) || (css_tag_type == CSS_tag_type.TD_CSS))
            {
                if (CSS_param.bold > 0)
                {
                    buf.Add("}");
                    num++;
                }
                CSS_param.bold--;
            }
            if ((ablaze.b[0] == 1) && (this._preserveFontFace == 1))
            {
                newb.Clear();
                newb.Add(@"\f0 ");
                if ((styleStack != null) && (styleStackNum > 0))
                {
                    flag = false;
                    num2 = styleStackNum - 1;
                    while (num2 >= 0)
                    {
                        if (((CSS_styles)CSS_param.CSS_style[((CSS_stack_element)styleStack[num2]).style_index]).ablaze.b[0] == 1)
                        {
                            flag = true;
                            break;
                        }
                        num2--;
                    }
                    if (flag && (((CSS_styles)CSS_param.CSS_style[((CSS_stack_element)styleStack[num2]).style_index]).font_family >= 0))
                    {
                        newb.Clear();
                        newb.Add(@"\f" + ((((CSS_styles)CSS_param.CSS_style[((CSS_stack_element)styleStack[num2]).style_index]).font_family + 2)).ToString() + " ");
                    }
                }
                may_use_space = true;
                buf.Add(newb);
                num += newb.len;
            }
            if ((ablaze.b[3] == 1) && (this._preserveFontSize == 1))
            {
                if ((styleStack == null) || (styleStackNum <= 0))
                {
                    if (style.font_size != fontsize)
                    {
                        font_size_default = false;
                        font_size_temp.Clear();
                        font_size_temp.Add(@"\fs" + fontsize + " ");
                        may_use_space = true;
                        buf.Add(font_size_temp);
                        num += font_size_temp.len;
                    }
                }
                else
                {
                    flag = false;
                    num2 = styleStackNum - 1;
                    while (num2 >= 0)
                    {
                        if (((CSS_styles)CSS_param.CSS_style[((CSS_stack_element)styleStack[num2]).style_index]).ablaze.b[3] == 1)
                        {
                            flag = true;
                            break;
                        }
                        num2--;
                    }
                    if (flag && (((CSS_styles)CSS_param.CSS_style[((CSS_stack_element)styleStack[num2]).style_index]).font_size > 0))
                    {
                        font_size_default = false;
                        font_size_temp.Clear();
                        font_size_temp.Add(@"\fs" + ((CSS_styles)CSS_param.CSS_style[((CSS_stack_element)styleStack[num2]).style_index]).font_size + " ");
                        may_use_space = true;
                        buf.Add(font_size_temp);
                        num += font_size_temp.len;
                    }
                }
            }
            if (((ablaze.b[4] == 1) && (this._preserveAlignment == 1)) && (css_tag_type != CSS_tag_type.TD_CSS))
            {
                align.Clear();
                align.Add(align_def);
                if (css_tag_type != CSS_tag_type.DIV_CSS)
                {
                    buf.Add(align);
                    num += align.len;
                }
            }
            if (((ablaze.b[5] == 1) || (css_tag_type == CSS_tag_type.TD_CSS)) && (CSS_param.italic > 0))
            {
                buf.Add("}");
                num++;
                CSS_param.italic--;
            }
            if ((ablaze.b[1] == 1) && (this._preserveFontColor == 1))
            {
                if (color_stack_num > 0)
                {
                    font_color_temp.Clear();
                    font_color_temp.Add(@"\cf" + color_stack.b[color_stack_num - 1] + " ");
                    if ((ablaze.b[5] == 1) || (ablaze.b[2] == 1))
                    {
                        may_use_space = true;
                    }
                    else
                    {
                        may_use_space = false;
                    }
                    color_stack_num--;
                }
                else
                {
                    if (CSS_param.body_tag && (ablaze.b[1] == 1))
                    {
                        font_color_temp.Clear();
                        if (((CSS_styles)CSS_param.CSS_style[CSS_param.body_tag_index]).ablaze.b[1] == 1)
                        {
                            font_color_temp.Add(@"\cf" + (((CSS_styles)CSS_param.CSS_style[CSS_param.body_tag_index]).color + 3) + " ");
                        }
                        else
                        {
                            font_color_temp.Add(@"\cf1 ");
                        }
                    }
                    else
                    {
                        font_color_temp.Clear();
                        font_color_temp.Add(@"\cf1 ");
                    }
                    if ((ablaze.b[5] == 1) || (ablaze.b[2] == 1))
                    {
                        may_use_space = true;
                    }
                    else
                    {
                        may_use_space = false;
                    }
                }
                buf.Add(font_color_temp);
                num += font_color_temp.len;
            }
            if ((ablaze.b[7] == 1) && (this._preserveBackgroundColor == 1))
            {
                font_color_temp.Clear();
                font_color_temp.Add(@"\chcbpat0 ");
                may_use_space = true;
                buf.Add(font_color_temp);
                num += font_color_temp.len;
            }
            if ((ablaze.b[12] == 1) && (CSS_param.underline > 0))
            {
                buf.Add("}");
                num++;
                CSS_param.underline--;
            }
            if ((ablaze.b[13] == 1) && (CSS_param.vertical_align > 0))
            {
                buf.Add("}");
                num++;
                CSS_param.vertical_align--;
            }
            if (((ablaze.b[15] == 1) && (this._preservePageBreaks == 1)) && (style.page_break_after == 1))
            {
                buf.Add("\n\\pard\\page\\par\n");
                num += 0x10;
            }
            p_count2 = num;
            return true;
        }

        private bool CSS_del_from_stack(ref int style_num, CSS_params CSS_param, CSS_tag_type css_tag_type)
        {
            int num = 0;
            style_num = num;
            CSS_stack_element _element = new CSS_stack_element();
            if (CSS_param.stack_num <= 0)
            {
                CSS_param.stack_num = 0;
                return false;
            }
            int num2 = CSS_param.stack_num - 1;
            CSS_param.found = false;
            while (num2 >= 0)
            {
                if (((CSS_stack_element)CSS_param.stack[num2]).css_tag_type == css_tag_type)
                {
                    CSS_param.found = true;
                    num = num2;
                    break;
                }
                num2--;
            }
            if (!CSS_param.found)
            {
                return false;
            }
            if (num > 0)
            {
                CSS_param.CSS_style_default = (CSS_styles)CSS_param.CSS_style[num - 1];
                style_num = num;
            }
            if ((num + 1) != CSS_param.stack_num)
            {
                for (num2 = num; (num2 < CSS_param.stack_num) && (num2 < (CSS_param.stack.Count - 1)); num2++)
                {
                    _element = (CSS_stack_element)CSS_param.stack[num2 + 1];
                    CSS_param.stack[num2] = _element;
                }
            }
            CSS_param.stack_num--;
            return true;
        }

        private bool CSS_del_from_stack_tag(ref int style_num, CSS_params CSS_param, CSS_tag_type css_tag_type)
        {
            int num = 0;
            int num2 = 0;
            CSS_stack_element _element = new CSS_stack_element();
            if (CSS_param.stack_tag_num <= 0)
            {
                CSS_param.stack_tag_num = 0;
                return false;
            }
            num2 = CSS_param.stack_tag_num - 1;
            CSS_param.found = false;
            while (num2 >= 0)
            {
                if (((CSS_stack_element)CSS_param.stack_tag[num2]).css_tag_type == css_tag_type)
                {
                    CSS_param.found = true;
                    num = num2;
                    break;
                }
                num2--;
            }
            if (!CSS_param.found)
            {
                return false;
            }
            if (num > 0)
            {
                CSS_param.CSS_style_default = (CSS_styles)CSS_param.CSS_style[num - 1];
                style_num = num;
            }
            if ((num + 1) != CSS_param.stack_tag_num)
            {
                for (num2 = num; (num2 < CSS_param.stack_tag_num) && (num2 < (CSS_param.stack_tag.Count - 1)); num2++)
                {
                    _element = (CSS_stack_element)CSS_param.stack_tag[num2 + 1];
                    CSS_param.stack_tag[num2] = _element;
                }
            }
            CSS_param.stack_tag_num--;
            return true;
        }

        private bool CSS_del_from_style_str_stack(int style_num, CSS_params CSS_param, CSS_tag_type css_tag_type)
        {
            int num = 0;
            CSS_styles _styles = new CSS_styles();
            if ((style_num + 1) != CSS_param.style_str_stack_num)
            {
                for (num = style_num; (num < CSS_param.style_str_stack_num) && (num < (CSS_param.CSS_style_str_stack.Count - 1)); num++)
                {
                    _styles = (CSS_styles)CSS_param.CSS_style_str_stack[num + 1];
                    CSS_param.CSS_style_str_stack[num] = _styles;
                }
            }
            CSS_param.style_str_stack_num--;
            return true;
        }

        private bool CSS_insert(ByteClass align_def, ByteClass align_j, ByteClass align_c, ByteClass align_l, ByteClass align_r, ByteClass align, ByteClass bufout, ref int p_ct, ByteClass buf, ref int p_count2, int max, CSS_params CSS_param, ref bool font_color_default, ByteClass font_color_temp, ByteClass color_stack, ref int color_stack_num, ref bool may_use_space, ref bool font_size_default, ByteClass font_size_temp, ArrayList font_list, ArrayList color_list, CSS_tag_type css_tag_type)
        {
            new ByteClass();
            int index = p_ct;
            int num2 = p_count2;
            int num3 = 0;
            int num4 = 0;
            ByteClass class2 = new ByteClass(20);
            int num5 = 0;
            int num6 = 0;
            bool flag = false;
            ByteClass valueStr = new ByteClass(0x40);
            int ct = 0;
            if (bufout.b[index] == 60)
            {
                index++;
            }
            num3 = index;
            num5 = 1;
            num6 = 0;
            while ((index < bufout.len) && (num5 != num6))
            {
                if (bufout.b[index] == 0x3e)
                {
                    num6++;
                }
                if (num5 == num6)
                {
                    break;
                }
                if (((((bufout.b[index] == 0x73) || (bufout.b[index] == 0x53)) && ((bufout.b[index + 1] == 0x74) || (bufout.b[index + 1] == 0x54))) && (((bufout.b[index + 2] == 0x79) || (bufout.b[index + 2] == 0x59)) && ((bufout.b[index + 3] == 0x6c) || (bufout.b[index + 3] == 0x4c)))) && (((bufout.b[index + 4] == 0x65) || (bufout.b[index + 4] == 0x45)) && (this.IS_DELIMITER(bufout.b[index + 5]) || (bufout.b[index + 5] == 0x3d))))
                {
                    index += 6;
                    this.ReadValue(bufout, ref index, valueStr);
                    break;
                }
                index++;
            }
            int len = valueStr.len;
            index = num3;
            num3 = 0;
            ct = 0;
            CSS_param.found = false;
            num5 = 1;
            num6 = 0;
            while ((index < max) && (num5 != num6))
            {
                if (bufout.b[index] == 0x3e)
                {
                    num6++;
                }
                if (num5 == num6)
                {
                    break;
                }
                if (((((bufout.b[index] == 0x63) || (bufout.b[index] == 0x43)) && ((bufout.b[index + 1] == 0x6c) || (bufout.b[index + 1] == 0x4c))) && (((bufout.b[index + 2] == 0x61) || (bufout.b[index + 2] == 0x41)) && ((bufout.b[index + 3] == 0x73) || (bufout.b[index + 3] == 0x53)))) && (((bufout.b[index + 4] == 0x73) || (bufout.b[index + 4] == 0x53)) && ((bufout.b[index + 5] == 0x3d) || (bufout.b[index + 5] == 0x20))))
                {
                    if (bufout.b[index + 5] == 0x20)
                    {
                        index += 6;
                    }
                    else
                    {
                        index += 5;
                    }
                    this.read_value_exact(bufout, ref index, max, CSS_param.style_name);
                    if (CSS_param.style_name.len > 0)
                    {
                        CSS_param.found = true;
                    }
                }
                if ((((bufout.b[index] == 0x69) || (bufout.b[index] == 0x49)) && ((bufout.b[index + 1] == 100) || (bufout.b[index + 1] == 0x44))) && (bufout.b[index + 2] == 0x3d))
                {
                    index += 2;
                    this.read_value_exact(bufout, ref index, max, CSS_param.style_name);
                    if (CSS_param.style_name.len > 0)
                    {
                        CSS_param.found = true;
                    }
                }
                if ((((((this._preserveAlignment == 1) && (bufout.b[index] == 0x20)) && ((bufout.b[index + 1] == 0x61) || (bufout.b[index + 1] == 0x41))) && ((bufout.b[index + 2] == 0x6c) || (bufout.b[index + 2] == 0x4c))) && (((bufout.b[index + 3] == 0x69) || (bufout.b[index + 3] == 0x49)) && ((bufout.b[index + 4] == 0x67) || (bufout.b[index + 4] == 0x47)))) && ((bufout.b[index + 5] == 110) || (bufout.b[index + 5] == 0x4e)))
                {
                    index += 6;
                    this.read_value(bufout, ref index, class2);
                    flag = true;
                    align.Clear();
                    if (class2.byteCmpi("center") == 0)
                    {
                        align.Add(@"\qc ");
                    }
                    else if (class2.byteCmpi("middle") == 0)
                    {
                        align.Add(@"\qc ");
                    }
                    else if (class2.byteCmpi("left") == 0)
                    {
                        align.Add(@"\ql ");
                    }
                    else if (class2.byteCmpi("right") == 0)
                    {
                        align.Add(@"\qr ");
                    }
                    else if (class2.byteCmpi("justify") == 0)
                    {
                        align.Add(@"\qj ");
                    }
                }
                index++;
            }
            if ((this._preserveAlignment == 1) && flag)
            {
                buf.Add(align);
                num2 += align.len;
            }
            for (num3 = 0; num3 < CSS_param.styles; num3++)
            {
                if (((((CSS_styles)CSS_param.CSS_style[num3]).name.len == 0) && (((CSS_styles)CSS_param.CSS_style[num3]).css_tag_type != CSS_tag_type.UNKNOWN_CSS)) && (((CSS_styles)CSS_param.CSS_style[num3]).css_tag_type == css_tag_type))
                {
                    CSS_stack_element _element = new CSS_stack_element();
                    CSS_param.stack_tag.Add(_element);
                    ((CSS_stack_element)CSS_param.stack_tag[CSS_param.stack_tag_num]).style_index = num3;
                    ((CSS_stack_element)CSS_param.stack_tag[CSS_param.stack_tag_num]).css_tag_type = css_tag_type;
                    if (CSS_param.stack_tag_num < (this.STK_MAX - 1))
                    {
                        CSS_param.stack_tag_num++;
                    }
                    this.CSS_insert_styles(((CSS_styles)CSS_param.CSS_style[num3]).ablaze, (CSS_styles)CSS_param.CSS_style[num3], CSS_param, align_def, align_j, align_c, align_l, align_r, align, bufout, ref index, buf, ref num2, max, ref font_color_default, font_color_temp, color_stack, ref color_stack_num, ref may_use_space, ref font_size_default, font_size_temp, css_tag_type);
                    break;
                }
            }
            if (css_tag_type != CSS_tag_type.FONT_CSS)
            {
                CSS_styles _styles = new CSS_styles();
                CSS_param.CSS_style_str_stack.Add(_styles);
                this.CSS_reset_style((CSS_styles)CSS_param.CSS_style_str_stack[CSS_param.style_str_stack_num]);
                if (this.CSS_read_style_params(valueStr, ref ct, len, (CSS_styles)CSS_param.CSS_style_str_stack[CSS_param.style_str_stack_num], CSS_param, font_list, color_list))
                {
                    if (CSS_param.style_str_stack_num > (this.STK_MAX - 1))
                    {
                        CSS_param.style_str_stack_num = this.STK_MAX - 1;
                    }
                    this.CSS_insert_styles(((CSS_styles)CSS_param.CSS_style_str_stack[CSS_param.style_str_stack_num]).ablaze, (CSS_styles)CSS_param.CSS_style_str_stack[CSS_param.style_str_stack_num], CSS_param, align_def, align_j, align_c, align_l, align_r, align, bufout, ref index, buf, ref num2, max, ref font_color_default, font_color_temp, color_stack, ref color_stack_num, ref may_use_space, ref font_size_default, font_size_temp, css_tag_type);
                    ((CSS_styles)CSS_param.CSS_style_str_stack[CSS_param.style_str_stack_num]).css_tag_type = css_tag_type;
                    if (CSS_param.style_str_stack_num < (this.STK_MAX - 1))
                    {
                        CSS_param.style_str_stack_num++;
                    }
                }
                else
                {
                    CSS_param.CSS_style_str_stack.RemoveAt(CSS_param.style_str_stack_num);
                }
            }
            if (CSS_param.found)
            {
                CSS_param.found = false;
                num3 = 0;
                while (num3 < CSS_param.styles)
                {
                    if ((((CSS_styles)CSS_param.CSS_style[num3]).name.byteCmp(CSS_param.style_name) == 0) && ((((CSS_styles)CSS_param.CSS_style[num3]).css_tag_type == css_tag_type) || (((CSS_styles)CSS_param.CSS_style[num3]).css_tag_type == CSS_tag_type.UNKNOWN_CSS)))
                    {
                        CSS_param.found = true;
                        break;
                    }
                    num3++;
                }
                if (CSS_param.found)
                {
                    CSS_stack_element _element2 = new CSS_stack_element();
                    CSS_param.stack.Add(_element2);
                    ((CSS_stack_element)CSS_param.stack[CSS_param.stack_num]).style_index = num3;
                    ((CSS_stack_element)CSS_param.stack[CSS_param.stack_num]).css_tag_type = css_tag_type;
                    if (CSS_param.stack_num < (this.STK_MAX - 1))
                    {
                        CSS_param.stack_num++;
                    }
                    for (num4 = 0; num4 < this.STYLES_KNOW; num4++)
                    {
                        CSS_param.ablaze.b[num4] = ((CSS_styles)CSS_param.CSS_style[num3]).ablaze.b[num4];
                    }
                    this.CSS_insert_styles(CSS_param.ablaze, (CSS_styles)CSS_param.CSS_style[num3], CSS_param, align_def, align_j, align_c, align_l, align_r, align, bufout, ref index, buf, ref num2, max, ref font_color_default, font_color_temp, color_stack, ref color_stack_num, ref may_use_space, ref font_size_default, font_size_temp, css_tag_type);
                    if (css_tag_type == CSS_tag_type.SPAN_CSS)
                    {
                        CSS_param.span++;
                    }
                    else if (css_tag_type == CSS_tag_type.DIV_CSS)
                    {
                        CSS_param.div++;
                    }
                    else if (css_tag_type == CSS_tag_type.P_CSS)
                    {
                        CSS_param.p_tag_open++;
                    }
                    else if (css_tag_type == CSS_tag_type.EM_CSS)
                    {
                        CSS_param.em_tag_open++;
                    }
                    else if (css_tag_type == CSS_tag_type.FONT_CSS)
                    {
                        CSS_param.font_tag_open++;
                    }
                }
            }
            p_ct = index;
            p_count2 = num2;
            return CSS_param.found;
        }

        private bool CSS_insert_styles(ByteClass ablaze, CSS_styles style, CSS_params CSS_param, ByteClass align_def, ByteClass align_j, ByteClass align_c, ByteClass align_l, ByteClass align_r, ByteClass align, ByteClass bufout, ref int p_ct, ByteClass buf, ref int p_count2, int max, ref bool font_color_default, ByteClass font_color_temp, ByteClass color_stack, ref int color_stack_num, ref bool may_use_space, ref bool font_size_default, ByteClass font_size_temp, CSS_tag_type css_tag_type)
        {
            int ct = p_ct;
            int num2 = p_count2;
            ByteClass newb = new ByteClass(0x20);
            int num3 = 0;
            if ((!CSS_param.pos.open && (ablaze.b[9] == 1)) && ((ablaze.b[10] == 1) && (ablaze.b[11] == 1)))
            {
                CSS_param.pos.x = style.left * this.PX_TO_TWIPS;
                CSS_param.pos.y = style.top * this.PX_TO_TWIPS;
                if (this._pageOrientation == ePageOrientation.Portrait)
                {
                    CSS_param.pos.w = this.page_width;
                }
                else
                {
                    CSS_param.pos.w = this.page_height;
                }
                CSS_param.pos.w -= CSS_param.pos.x;
                CSS_param.pos.h = 10 * this.PX_TO_TWIPS;
                CSS_param.pos.pos_open_str = string.Concat(new object[] { @"{\pard\plain\posx", CSS_param.pos.x, @"\posy%d\absw", CSS_param.pos.w, @"\absh", CSS_param.pos.h, " " });
                CSS_param.pos.pos_close_str = @"\par}";
                CSS_param.pos.open = this.check_position_object(bufout, ct, max, style.css_tag_type);
                if (CSS_param.pos.open)
                {
                    buf.Add(CSS_param.pos.pos_open_str);
                    num2 += CSS_param.pos.pos_open_str.Length;
                }
            }
            if (((ablaze.b[14] == 1) && (this._preservePageBreaks == 1)) && (style.page_break_before == 1))
            {
                buf.Add("\n\\pard\\page\\par\n");
                num2 += 0x10;
            }
            else if (((ablaze.b[15] == 1) && (this._preservePageBreaks == 1)) && ((style.page_break_after == 1) && (css_tag_type == CSS_tag_type.HR_CSS)))
            {
                buf.Add("\n\\pard\\page\\par\n");
                num2 += 0x10;
            }
            if ((ablaze.b[0] == 1) && (this._preserveFontFace == 1))
            {
                num3 = style.font_family;
                if ((num3 >= 0) && (num3 < this.MAX_FONTS))
                {
                    newb.Clear();
                    newb.Add(@"\f" + ((num3 + 2)).ToString() + " ");
                    may_use_space = true;
                    buf.Add(newb);
                    num2 += newb.len;
                }
            }
            if ((ablaze.b[1] == 1) && (this._preserveFontColor == 1))
            {
                if (!font_color_default && (color_stack_num < color_stack.len))
                {
                    color_stack.b[color_stack_num++] = (byte)this.ToInt(font_color_temp.ToByteCStartPos(3));
                }
                font_color_default = false;
                font_color_temp.Clear();
                font_color_temp.Add(@"\cf" + ((style.color + 3)).ToString() + " ");
                may_use_space = true;
                if (!font_color_default)
                {
                    buf.Add(font_color_temp);
                    num2 += font_color_temp.len;
                }
            }
            if (ablaze.b[2] == 1)
            {
                if (style.font_weight == 1)
                {
                    buf.Add(@"{\b ");
                    num2 += 4;
                    may_use_space = true;
                }
                if (style.font_weight == 0)
                {
                    buf.Add(@"\b0 ");
                    num2 += 4;
                }
                if (style.font_weight == 1)
                {
                    CSS_param.bold++;
                    if (CSS_param.bold <= 0)
                    {
                        CSS_param.bold = 1;
                    }
                }
            }
            if ((ablaze.b[3] == 1) && (this._preserveFontSize == 1))
            {
                font_size_default = false;
                font_size_temp.Clear();
                font_size_temp.Add(@"\fs" + style.font_size + " ");
                may_use_space = true;
                buf.Add(font_size_temp);
                num2 += font_size_temp.len;
            }
            if ((ablaze.b[4] == 1) && (this._preserveAlignment == 1))
            {
                if (style.text_align == 0x6c)
                {
                    align = align_l;
                }
                else if (style.text_align == 0x72)
                {
                    align = align_r;
                }
                else if (style.text_align == 0x63)
                {
                    align = align_c;
                }
                else if (style.text_align == 0x6a)
                {
                    align = align_j;
                }
                buf.Add(align);
                num2 += align.len;
            }
            if ((ablaze.b[5] == 1) && (style.font_style == 0x69))
            {
                buf.Add(@"{\i ");
                num2 += 4;
                CSS_param.italic++;
                if (CSS_param.italic <= 0)
                {
                    CSS_param.italic = 1;
                }
                may_use_space = true;
            }
            if (ablaze.b[6] == 1)
            {
                newb.Clear();
                newb.Add(@"\sb" + style.margin_top + " ");
                buf.Add(newb);
                num2 += newb.len;
            }
            if ((ablaze.b[7] == 1) && (this._preserveBackgroundColor == 1))
            {
                font_color_temp.Clear();
                font_color_temp.Add(@"\chcbpat" + ((style.background_color + 3)).ToString() + " ");
                may_use_space = true;
                buf.Add(font_color_temp);
                num2 += font_color_temp.len;
            }
            if (ablaze.b[12] == 1)
            {
                if (style.text_decoration == 0x75)
                {
                    buf.Add(@"{\ul ");
                    num2 += 5;
                }
                if (style.text_decoration == 110)
                {
                    buf.Add(@"{\ul0 ");
                    num2 += 6;
                }
                if (style.text_decoration == 0x73)
                {
                    buf.Add(@"{\strike ");
                    num2 += 9;
                }
                CSS_param.underline++;
                if (CSS_param.underline <= 0)
                {
                    CSS_param.underline = 1;
                }
            }
            if (ablaze.b[13] == 1)
            {
                if (style.vertical_align == 0x62)
                {
                    buf.Add(@"{\sub ");
                    num2 += 6;
                }
                if (style.vertical_align == 0x72)
                {
                    buf.Add(@"{\super ");
                    num2 += 8;
                }
                CSS_param.vertical_align++;
                if (CSS_param.vertical_align <= 0)
                {
                    CSS_param.vertical_align = 1;
                }
            }
            p_count2 = num2;
            return true;
        }

        private int CSS_parse_name(ByteClass name, CSS_styles style)
        {
            ByteClass newb = new ByteClass();
            bool flag = false;
            if (name.byteCmpi("DIV") == 0)
            {
                name.Clear();
                style.css_tag_type = CSS_tag_type.DIV_CSS;
            }
            else if (name.byteCmpi("SPAN") == 0)
            {
                name.Clear();
                style.css_tag_type = CSS_tag_type.SPAN_CSS;
            }
            else if (name.byteCmpi("P") == 0)
            {
                name.Clear();
                style.css_tag_type = CSS_tag_type.P_CSS;
            }
            else if (name.byteCmpi("EM") == 0)
            {
                name.Clear();
                style.css_tag_type = CSS_tag_type.EM_CSS;
            }
            else if (name.byteCmpi("FONT") == 0)
            {
                name.Clear();
                style.css_tag_type = CSS_tag_type.FONT_CSS;
            }
            else if (name.byteCmpi("TD") == 0)
            {
                name.Clear();
                style.css_tag_type = CSS_tag_type.TD_CSS;
            }
            else if (name.byteCmpi("TABLE") == 0)
            {
                name.Clear();
                style.css_tag_type = CSS_tag_type.TABLE_CSS;
            }
            else if (name.byteCmpi("UL") == 0)
            {
                name.Clear();
                style.css_tag_type = CSS_tag_type.UL_CSS;
            }
            else if (name.byteCmpi("OL") == 0)
            {
                name.Clear();
                style.css_tag_type = CSS_tag_type.OL_CSS;
            }
            else if (name.byteCmpi("H1") == 0)
            {
                name.Clear();
                style.css_tag_type = CSS_tag_type.H1_CSS;
            }
            else if (name.byteCmpi("H2") == 0)
            {
                name.Clear();
                style.css_tag_type = CSS_tag_type.H2_CSS;
            }
            else if (name.byteCmpi("H3") == 0)
            {
                name.Clear();
                style.css_tag_type = CSS_tag_type.H3_CSS;
            }
            else if (name.byteCmpi("H4") == 0)
            {
                name.Clear();
                style.css_tag_type = CSS_tag_type.H4_CSS;
            }
            else if (name.byteCmpi("H5") == 0)
            {
                name.Clear();
                style.css_tag_type = CSS_tag_type.H5_CSS;
            }
            else if (name.byteCmpi("H6") == 0)
            {
                name.Clear();
                style.css_tag_type = CSS_tag_type.H6_CSS;
            }
            else if (name.byteCmpi("HR") == 0)
            {
                name.Clear();
                style.css_tag_type = CSS_tag_type.HR_CSS;
            }
            int len = name.len;
            newb.Clear();
            for (int i = 0; i < len; i++)
            {
                if ((name.b[i] == 0x2e) || (name.b[i] == 0x23))
                {
                    flag = false;
                    if (newb.byteCmpi("DIV") == 0)
                    {
                        flag = true;
                        style.css_tag_type = CSS_tag_type.DIV_CSS;
                    }
                    else if (newb.byteCmpi("SPAN") == 0)
                    {
                        flag = true;
                        style.css_tag_type = CSS_tag_type.SPAN_CSS;
                    }
                    else if (newb.byteCmpi("P") == 0)
                    {
                        flag = true;
                        style.css_tag_type = CSS_tag_type.P_CSS;
                    }
                    else if (newb.byteCmpi("EM") == 0)
                    {
                        flag = true;
                        style.css_tag_type = CSS_tag_type.EM_CSS;
                    }
                    else if (newb.byteCmpi("FONT") == 0)
                    {
                        flag = true;
                        style.css_tag_type = CSS_tag_type.FONT_CSS;
                    }
                    else if (newb.byteCmpi("TD") == 0)
                    {
                        flag = true;
                        style.css_tag_type = CSS_tag_type.TD_CSS;
                    }
                    else if (newb.byteCmpi("TABLE") == 0)
                    {
                        flag = true;
                        style.css_tag_type = CSS_tag_type.TABLE_CSS;
                    }
                    else if (newb.byteCmpi("UL") == 0)
                    {
                        flag = true;
                        style.css_tag_type = CSS_tag_type.UL_CSS;
                    }
                    else if (newb.byteCmpi("OL") == 0)
                    {
                        flag = true;
                        style.css_tag_type = CSS_tag_type.OL_CSS;
                    }
                    else if (newb.byteCmpi("H1") == 0)
                    {
                        flag = true;
                        style.css_tag_type = CSS_tag_type.H1_CSS;
                    }
                    else if (newb.byteCmpi("H2") == 0)
                    {
                        flag = true;
                        style.css_tag_type = CSS_tag_type.H2_CSS;
                    }
                    else if (newb.byteCmpi("H3") == 0)
                    {
                        flag = true;
                        style.css_tag_type = CSS_tag_type.H3_CSS;
                    }
                    else if (newb.byteCmpi("H4") == 0)
                    {
                        flag = true;
                        style.css_tag_type = CSS_tag_type.H4_CSS;
                    }
                    else if (newb.byteCmpi("H5") == 0)
                    {
                        flag = true;
                        style.css_tag_type = CSS_tag_type.H5_CSS;
                    }
                    else if (newb.byteCmpi("H6") == 0)
                    {
                        flag = true;
                        style.css_tag_type = CSS_tag_type.H6_CSS;
                    }
                    else if (newb.byteCmpi("HR") == 0)
                    {
                        flag = true;
                        style.css_tag_type = CSS_tag_type.HR_CSS;
                    }
                    if (flag)
                    {
                        newb.Clear();
                        newb.Add(name);
                        style.name.Clear();
                        style.name.Add(newb, i + 1);
                    }
                    break;
                }
                newb.Add(name.b[i]);
            }
            return 0;
        }

        private bool CSS_read_file(CSS_params CSS_param)
        {
            string s = "";
            string url = CSS_param.file_name.ByteToString();
            bool flag = false;
            if ((((url.Length > 6) && ((url[0] == 'h') || (url[0] == 'H'))) && ((url[1] == 't') || (url[1] == 'T'))) && ((((url[2] == 't') || (url[2] == 'T')) && ((url[3] == 'p') || (url[3] == 'P'))) && (((url[4] == ':') && (url[5] == '/')) && (url[6] == '/'))))
            {
                flag = true;
            }
            try
            {
                if (flag)
                {
                    s = this.DownloadFile(url);
                }
                else
                {
                    StreamReader reader = File.OpenText(url);
                    s = reader.ReadToEnd();
                    reader.Close();
                }
                CSS_param.buf.Add(s);
            }
            catch
            {
                return false;
            }
            CSS_param.buf_size = CSS_param.buf.len;
            CSS_param.buf_pos += s.Length - 1;
            CSS_param.use = true;
            return true;
        }

        private void CSS_read_file_name(ByteClass buf, ref int ct, int max_pos, ByteClass value_str)
        {
            int index = ct;
            int num2 = 0;
            bool flag = true;
            int num3 = 0;
            while (((buf.b[index] == 0x27) || (buf.b[index] == 0x22)) || ((buf.b[index] == 0x20) || (buf.b[index] == 0x3d)))
            {
                index++;
            }
            if ((buf.b[index - 1] == 0x22) || (buf.b[index - 1] == 0x27))
            {
                flag = false;
                num3 = buf.b[index - 1];
            }
            while (((buf.b[index] >= 0x20) && (buf.b[index] != 0x3e)) && ((index < max_pos) && (num2 < 0x200)))
            {
                if (flag && (buf.b[index] == 0x20))
                {
                    return;
                }
                if (buf.b[index] == num3)
                {
                    return;
                }
                value_str.Add((byte)char.ToLower((char)buf.b[index]));
                num2++;
                index++;
            }
        }

        private bool CSS_read_from_tags(CSS_params CSS_param, int max_buf_size, ByteClass buf, ref int ct)
        {
            int index = ct;
            int num3 = 0;
            if (max_buf_size <= 0)
            {
                return false;
            }
            if ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x73)) && ((buf.b[index + 2] == 0x74) && (buf.b[index + 3] == 0x79))) && ((buf.b[index + 4] == 0x6c) && (buf.b[index + 5] == 0x65))) || ((((buf.b[index] == 60) && (buf.b[index + 1] == 0x53)) && ((buf.b[index + 2] == 0x74) && (buf.b[index + 3] == 0x79))) && ((buf.b[index + 4] == 0x6c) && (buf.b[index + 5] == 0x65)))) || ((((buf.b[index] == 60) && (buf.b[index + 1] == 0x53)) && ((buf.b[index + 2] == 0x54) && (buf.b[index + 3] == 0x59))) && ((buf.b[index + 4] == 0x4c) && (buf.b[index + 5] == 0x45))))
            {
                while ((buf.b[index + 5] != 0x3e) && (index < (CSS_param.buf_size + max_buf_size)))
                {
                    index++;
                }
                index += 6;
                int num2 = 0;
                num3 = CSS_param.buf_pos;
                while ((num2 == 0) && (index < (CSS_param.buf_size + max_buf_size)))
                {
                    if ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x73) && (buf.b[index + 3] == 0x74))) && (((buf.b[index + 4] == 0x79) && (buf.b[index + 5] == 0x6c)) && ((buf.b[index + 6] == 0x65) && (buf.b[index + 7] == 0x3e)))) || ((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x53) && (buf.b[index + 3] == 0x74))) && (((buf.b[index + 4] == 0x79) && (buf.b[index + 5] == 0x6c)) && ((buf.b[index + 6] == 0x65) && (buf.b[index + 7] == 0x3e))))) || ((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x53) && (buf.b[index + 3] == 0x54))) && (((buf.b[index + 4] == 0x59) && (buf.b[index + 5] == 0x4c)) && ((buf.b[index + 6] == 0x45) && (buf.b[index + 7] == 0x3e)))))
                    {
                        num2 = 1;
                    }
                    else
                    {
                        CSS_param.buf.Add(buf.b[index]);
                        num3++;
                        index++;
                    }
                }
                index += 7;
            }
            CSS_param.buf_size += max_buf_size;
            CSS_param.buf_pos = num3;
            CSS_param.use = true;
            ct = index;
            return true;
        }

        private void CSS_read_second_name(ByteClass buf, ref int ct, int max, ByteClass name)
        {
            int index = ct;
            name.Clear();
            while (this.IS_DELIMITER(buf.b[index]))
            {
                index++;
            }
            while (index < max)
            {
                if (((buf.b[index] == 0x2c) || (buf.b[index] == 0x7b)) || (buf.b[index] == 0x20))
                {
                    break;
                }
                name.Add(buf.b[index++]);
            }
            if ((buf.b[index] == 0x2c) || (buf.b[index] == 0x20))
            {
                index++;
            }
            ct = index;
        }

        private int CSS_read_style_name(ByteClass buf, ref int pct, ByteClass style_name)
        {
            ByteClass newb = new ByteClass();
            int index = 0;
            int num2 = 0;
            int num3 = 1;
            int num4 = pct;
            while ((num4 > 0) && (this.isspace(buf.b[num4]) || this.ispunct(buf.b[num4])))
            {
                num4--;
            }
            num2 = num4;
            while (num4 > 0)
            {
                if (buf.b[num4] == 0x2c)
                {
                    num3++;
                }
                if ((!this.isalpha(buf.b[num4]) && !this.ispunct(buf.b[num4])) && (!this.isdigit(buf.b[num4]) && (buf.b[num4] != 0x20)))
                {
                    num4++;
                    break;
                }
                num4--;
            }
            while ((buf.b[num4] == 0x20) && (num4 < buf.len))
            {
                num4++;
            }
            index = num4;
            if (num3 > 1)
            {
                pct = num4;
                return num3;
            }
            if (buf.b[index] == 10)
            {
                index++;
            }
            if (buf.b[index] == 0x2e)
            {
                index++;
            }
            newb.Clear();
            while (index <= num2)
            {
                newb.Add(buf.b[index++]);
            }
            style_name.Clear();
            style_name.Add(newb);
            if (style_name.len == 0)
            {
                return 0;
            }
            return 1;
        }

        private bool CSS_read_style_params(ByteClass buf, ref int ct, int max, CSS_styles CSS_style, CSS_params CSS_param, ArrayList font_list, ArrayList color_list)
        {
            int num2;
            ByteClass class2 = new ByteClass();
            int index = ct;
            bool quote = false;
            while (ct < max)
            {
                if (buf.b[ct] == 0x7d)
                {
                    max = ct;
                    break;
                }
                ct++;
            }
            while (buf.b[index] == 0x7b)
            {
                index++;
            }
        Label_0F0F:
            while ((index < max) && (buf.b[index] != 0x7d))
            {
                bool flag2;
                double num3;
                this.read_value_CSS(buf, ref index, max, class2, -1111, quote);
                if (index < (buf.len - 1))
                {
                    while (((buf.b[index] == 0x3a) || (buf.b[index] == 0x3b)) || (((buf.b[index] >= 0) && (buf.b[index] <= 0x20)) && (index < (buf.len - 1))))
                    {
                        index++;
                    }
                }
                new ByteClass();
                if (class2.byteCmpi((string)CSS_param.names[0]) == 0)
                {
                    this.read_value_CSS(buf, ref index, max, class2, -1111, true);
                    num2 = 0;
                    bool flag = false;
                    while ((num2 < CSS_param.font_list_num) && (num2 < this.MAX_FONTS))
                    {
                        if (class2.byteCmpi((string)font_list[num2]) == 0)
                        {
                            flag = true;
                            break;
                        }
                        num2++;
                    }
                    if (!flag)
                    {
                        font_list.Add(class2.ByteToString());
                        CSS_style.ablaze.b[0] = 1;
                        CSS_style.font_family = (byte)CSS_param.font_list_num;
                        CSS_param.font_list_num++;
                    }
                    else
                    {
                        CSS_style.ablaze.b[0] = 1;
                        CSS_style.font_family = (byte)num2;
                    }
                    continue;
                }
                if (class2.byteCmpi((string)CSS_param.names[1]) == 0)
                {
                    this.read_color(buf, ref index, max, class2);
                    index += class2.len;
                    num2 = 0;
                    flag2 = false;
                    while ((num2 < CSS_param.color_list_num) && (num2 < this.MAX_COLORS))
                    {
                        if (class2.byteCmpi(color_list[num2].ToString()) == 0)
                        {
                            flag2 = true;
                            break;
                        }
                        num2++;
                    }
                    if (!flag2)
                    {
                        color_list.Add(class2.ByteToString());
                        class2.Clear();
                        CSS_style.ablaze.b[1] = 1;
                        CSS_style.color = (byte)CSS_param.color_list_num;
                        CSS_param.color_list_num++;
                    }
                    else
                    {
                        CSS_style.ablaze.b[1] = 1;
                        CSS_style.color = (byte)num2;
                    }
                    continue;
                }
                if (class2.byteCmpi((string)CSS_param.names[2]) == 0)
                {
                    this.read_value_CSS(buf, ref index, max, class2, -1111, quote);
                    if (class2.byteCmpi("bold") == 0)
                    {
                        CSS_style.ablaze.b[2] = 1;
                        CSS_style.font_weight = 1;
                    }
                    if (class2.byteCmpi("normal") == 0)
                    {
                        CSS_style.ablaze.b[2] = 1;
                        CSS_style.font_weight = 0;
                    }
                    continue;
                }
                if (class2.byteCmpi((string)CSS_param.names[3]) == 0)
                {
                    this.read_value_CSS_tolower(buf, ref index, max, class2, -1111);
                    num3 = this.ToFloat(class2);
                    if (class2.IndexOfBegin("px") != -1)
                    {
                        num3 *= 0.75;
                    }
                    else if (class2.IndexOfBegin("em") != -1)
                    {
                        num3 *= 12;
                    }
                    else if (class2.byteCmp("xx-small") == 0)
                    {
                        num3 = 8;
                    }
                    else if (class2.byteCmp("x-small") == 0)
                    {
                        num3 = 10;
                    }
                    else if (class2.byteCmp("small") == 0)
                    {
                        num3 = 12;
                    }
                    else if (class2.byteCmp("medium") == 0)
                    {
                        num3 = 14;
                    }
                    else if (class2.byteCmp("large") == 0)
                    {
                        num3 = 18;
                    }
                    else if (class2.byteCmp("x-large") == 0)
                    {
                        num3 = 24;
                    }
                    else if (class2.byteCmp("xx-large") == 0)
                    {
                        num3 = 36;
                    }
                    else if (class2.IndexOfBegin("%") != -1)
                    {
                        num3 = (num3 * 12) / 100;
                    }
                    if ((num3 > this.FONT_SIZE_MIN) && (num3 < this.FONT_SIZE_MAX))
                    {
                        CSS_style.ablaze.b[3] = 1;
                        CSS_style.font_size = (byte)(num3 * 2);
                    }
                    continue;
                }
                if ((class2.byteCmpi((string)CSS_param.names[4]) == 0) || (class2.byteCmpi("align") == 0))
                {
                    this.read_value_CSS_tolower(buf, ref index, max, class2, -1111);
                    if (((class2.b[0] == 0x6c) || (class2.b[0] == 0x72)) || ((class2.b[0] == 0x63) || (class2.b[0] == 0x6a)))
                    {
                        CSS_style.ablaze.b[4] = 1;
                        CSS_style.text_align = class2.b[0];
                    }
                    continue;
                }
                if (class2.byteCmpi((string)CSS_param.names[5]) == 0)
                {
                    this.read_value_CSS(buf, ref index, max, class2, -1111, quote);
                    if (class2.byteCmpi("italic") == 0)
                    {
                        CSS_style.ablaze.b[5] = 1;
                        CSS_style.font_style = 0x69;
                    }
                    continue;
                }
                if (class2.byteCmpi((string)CSS_param.names[6]) == 0)
                {
                    this.read_value_CSS_tolower(buf, ref index, max, class2, -1111);
                    num3 = (int)(this.ToFloat(class2) * 20f);
                    if (class2.IndexOfBegin("px") != 0)
                    {
                        num3 = (int)(num3 / 1.333299994468689);
                    }
                    if ((num3 >= 0) && (num3 <= 250))
                    {
                        CSS_style.ablaze.b[6] = 1;
                        CSS_style.margin_top = (byte)num3;
                    }
                    continue;
                }
                if ((class2.byteCmpi((string)CSS_param.names[7]) == 0) || (class2.byteCmpi("background") == 0))
                {
                    if (!this.read_color(buf, ref index, max, class2))
                    {
                        continue;
                    }
                    index += class2.len;
                    num2 = 0;
                    flag2 = false;
                    while ((num2 < CSS_param.color_list_num) && (num2 < this.MAX_COLORS))
                    {
                        if (class2.byteCmpi(color_list[num2].ToString()) == 0)
                        {
                            flag2 = true;
                            break;
                        }
                        num2++;
                    }
                    if (!flag2)
                    {
                        color_list.Add(class2.ByteToString());
                        class2.Clear();
                        CSS_style.ablaze.b[7] = 1;
                        CSS_style.background_color = (byte)CSS_param.color_list_num;
                        CSS_param.color_list_num++;
                    }
                    else
                    {
                        CSS_style.ablaze.b[7] = 1;
                        CSS_style.background_color = (byte)num2;
                    }
                    continue;
                }
                if (class2.byteCmpi((string)CSS_param.names[8]) == 0)
                {
                    int num4 = 1;
                    int num5 = 1;
                    for (int i = 0; ((num4 != 0) || (num5 != 0)) && (i < 10); i++)
                    {
                        this.read_value_CSS(buf, ref index, max, class2, -1111, quote);
                        num4 = num5;
                        num5 = class2.b[0];
                        if (class2.byteCmpi("italic") == 0)
                        {
                            CSS_style.ablaze.b[5] = 1;
                            CSS_style.font_style = 0x69;
                        }
                        if (class2.byteCmpi("bold") == 0)
                        {
                            CSS_style.ablaze.b[2] = 1;
                            CSS_style.font_weight = 1;
                        }
                        if (class2.IndexOfBegin("px") != 0)
                        {
                            num3 = this.ToFloat(class2);
                            num3 = (int)(num3 * 0.75);
                            if ((num3 > this.FONT_SIZE_MIN) && (num3 < this.FONT_SIZE_MAX))
                            {
                                CSS_style.ablaze.b[3] = 1;
                                CSS_style.font_size = (byte)(num3 * 2);
                            }
                        }
                        if (class2.IndexOfBegin("pt") != 0)
                        {
                            num3 = this.ToFloat(class2);
                            if ((num3 > this.FONT_SIZE_MIN) && (num3 < this.FONT_SIZE_MAX))
                            {
                                CSS_style.ablaze.b[3] = 1;
                                CSS_style.font_size = (byte)(num3 * 2);
                            }
                        }
                        if (class2.byteCmpi((string)CSS_param.names[1]) == 0)
                        {
                            while ((buf.b[index] == 0x3a) || (buf.b[index] == 0x20))
                            {
                                index++;
                            }
                            this.read_color(buf, ref index, max, class2);
                            index += class2.len;
                            num2 = 0;
                            flag2 = false;
                            while ((num2 < CSS_param.color_list_num) && (num2 < this.MAX_COLORS))
                            {
                                if (class2.byteCmpi(color_list[num2].ToString()) == 0)
                                {
                                    flag2 = true;
                                    break;
                                }
                                num2++;
                            }
                            if (!flag2)
                            {
                                color_list.Add(class2.ByteToString());
                                class2.Clear();
                                CSS_style.ablaze.b[1] = 1;
                                CSS_style.color = (byte)CSS_param.color_list_num;
                                CSS_param.color_list_num++;
                            }
                            else
                            {
                                CSS_style.ablaze.b[1] = 1;
                                CSS_style.color = (byte)num2;
                            }
                        }
                    }
                }
                else if (class2.byteCmpi((string)CSS_param.names[9]) == 0)
                {
                    this.read_value_CSS_tolower(buf, ref index, max, class2, -1111);
                    num3 = (int)this.ToFloat(class2);
                    if (num3 > 0)
                    {
                        CSS_style.ablaze.b[9] = 1;
                        CSS_style.top = (byte)num3;
                    }
                }
                else
                {
                    if (class2.byteCmpi((string)CSS_param.names[10]) == 0)
                    {
                        this.read_value_CSS_tolower(buf, ref index, max, class2, -1111);
                        num3 = (int)this.ToFloat(class2);
                        if (num3 > 0)
                        {
                            CSS_style.ablaze.b[10] = 1;
                            CSS_style.left = (byte)num3;
                        }
                        goto Label_0F0F;
                    }
                    if (class2.byteCmpi((string)CSS_param.names[11]) == 0)
                    {
                        this.read_value_CSS_tolower(buf, ref index, max, class2, -1111);
                        num3 = (int)this.ToFloat(class2);
                        if (num3 > 0)
                        {
                            CSS_style.ablaze.b[11] = 1;
                            CSS_style.width = (byte)num3;
                            if (class2.IndexOf("%") != -1)
                            {
                                CSS_style.width_in_percent = true;
                            }
                            else
                            {
                                CSS_style.width_in_percent = false;
                            }
                        }
                        goto Label_0F0F;
                    }
                    if (class2.byteCmpi((string)CSS_param.names[12]) == 0)
                    {
                        this.read_value_CSS(buf, ref index, max, class2, -1111, quote);
                        if (class2.byteCmpi("underline") == 0)
                        {
                            CSS_style.ablaze.b[12] = 1;
                            CSS_style.text_decoration = 0x75;
                        }
                        if (class2.byteCmpi("none") == 0)
                        {
                            CSS_style.ablaze.b[12] = 1;
                            CSS_style.text_decoration = 110;
                        }
                        if (class2.byteCmpi("line-through") == 0)
                        {
                            CSS_style.ablaze.b[12] = 1;
                            CSS_style.text_decoration = 0x73;
                        }
                        goto Label_0F0F;
                    }
                    if (class2.byteCmpi((string)CSS_param.names[13]) == 0)
                    {
                        this.read_value_CSS(buf, ref index, max, class2, -1111, quote);
                        if (class2.byteCmpi("sub") == 0)
                        {
                            CSS_style.ablaze.b[13] = 1;
                            CSS_style.vertical_align = 0x62;
                        }
                        if (class2.byteCmpi("super") == 0)
                        {
                            CSS_style.ablaze.b[13] = 1;
                            CSS_style.vertical_align = 0x72;
                        }
                        goto Label_0F0F;
                    }
                    if (class2.byteCmpi((string)CSS_param.names[14]) == 0)
                    {
                        this.read_value_CSS(buf, ref index, max, class2, -1111, quote);
                        if (class2.byteCmpi("avoid") == 0)
                        {
                            CSS_style.ablaze.b[14] = 1;
                            CSS_style.page_break_before = 0;
                        }
                        else if (class2.len > 0)
                        {
                            CSS_style.ablaze.b[14] = 1;
                            CSS_style.page_break_before = 1;
                        }
                        goto Label_0F0F;
                    }
                    if (class2.byteCmpi((string)CSS_param.names[15]) == 0)
                    {
                        this.read_value_CSS(buf, ref index, max, class2, -1111, quote);
                        if (class2.byteCmpi("avoid") == 0)
                        {
                            CSS_style.ablaze.b[15] = 1;
                            CSS_style.page_break_after = 0;
                        }
                        else if (class2.len > 0)
                        {
                            CSS_style.ablaze.b[15] = 1;
                            CSS_style.page_break_after = 1;
                        }
                        goto Label_0F0F;
                    }
                    if (class2.byteCmpi((string)CSS_param.names[0x10]) == 0)
                    {
                        this.read_value_CSS(buf, ref index, max, class2, -1111, quote);
                        if (class2.byteCmpi("none") == 0)
                        {
                            CSS_style.ablaze.b[0x10] = 1;
                            CSS_style.list_style_type = 1;
                        }
                        else if (class2.byteCmpi("disc") == 0)
                        {
                            CSS_style.ablaze.b[0x10] = 1;
                            CSS_style.list_style_type = 2;
                        }
                        else if (class2.byteCmpi("circle") == 0)
                        {
                            CSS_style.ablaze.b[0x10] = 1;
                            CSS_style.list_style_type = 3;
                        }
                        else if (class2.byteCmpi("square") == 0)
                        {
                            CSS_style.ablaze.b[0x10] = 1;
                            CSS_style.list_style_type = 4;
                        }
                        else if (class2.byteCmpi("lower-roman") == 0)
                        {
                            CSS_style.ablaze.b[0x10] = 1;
                            CSS_style.list_style_type = 5;
                        }
                        else if (class2.byteCmpi("upper-roman") == 0)
                        {
                            CSS_style.ablaze.b[0x10] = 1;
                            CSS_style.list_style_type = 6;
                        }
                        else if (class2.byteCmpi("lower-alpha") == 0)
                        {
                            CSS_style.ablaze.b[0x10] = 1;
                            CSS_style.list_style_type = 7;
                        }
                        else if (class2.byteCmpi("upper-alpha") == 0)
                        {
                            CSS_style.ablaze.b[0x10] = 1;
                            CSS_style.list_style_type = 8;
                        }
                        else if (class2.byteCmpi("arabic") == 0)
                        {
                            CSS_style.ablaze.b[0x10] = 1;
                            CSS_style.list_style_type = 9;
                        }
                        goto Label_0F0F;
                    }
                    if (class2.byteCmpi((string)CSS_param.names[0x11]) == 0)
                    {
                        this.read_value_CSS_tolower(buf, ref index, max, class2, -1111);
                        num3 = (int)(this.ToFloat(class2) * 20f);
                        if (class2.IndexOfBegin("px") != 0)
                        {
                            num3 = (int)(num3 / 1.3333);
                        }
                        if ((num3 >= 0) && (num3 <= 250))
                        {
                            CSS_style.ablaze.b[0x11] = 1;
                            CSS_style.margin_bottom = (byte)num3;
                        }
                        goto Label_0F0F;
                    }
                    if (class2.byteCmpi("style") == 0)
                    {
                        quote = true;
                        while ((((buf.b[index] == 0x3a) || (buf.b[index] == 0x20)) || (buf.b[index] == 0x3d)) && (index < max))
                        {
                            index++;
                        }
                    }
                }
            }
        ct = index;
        for (num2 = 0; num2 < this.STYLES_KNOW; num2++)
        {
            if (CSS_style.ablaze.b[num2] == 1)
            {
                return true;
            }
        }
        return false;
        }

        private void CSS_reset_style(CSS_styles CSS)
        {
            for (int i = 0; i < this.STYLES_KNOW; i++)
            {
                CSS.ablaze.b[i] = 0;
            }
            CSS.name.Clear();
            CSS.font_family = 0;
            CSS.color = 0;
            CSS.font_weight = 0;
            CSS.font_size = 0;
            CSS.text_align = 0;
            CSS.font_style = 0;
            CSS.margin_top = 0;
            CSS.background_color = 0;
            CSS.font = 0;
            CSS.top = 0;
            CSS.left = 0;
            CSS.width = 0;
            CSS.text_decoration = 0;
            CSS.vertical_align = 0;
            CSS.page_break_before = 0;
            CSS.page_break_after = 0;
            CSS.list_style_type = 0;
            CSS.margin_bottom = 0;
            CSS.width_in_percent = false;
            CSS.css_tag_type = CSS_tag_type.UNKNOWN_CSS;
        }

        private void CSSJoinStyles(CSS_styles dest, CSS_styles src)
        {
            if (src.ablaze.b[0] == 1)
            {
                dest.ablaze.b[0] = 1;
                dest.font_family = src.font_family;
            }
            if (src.ablaze.b[1] == 1)
            {
                dest.ablaze.b[1] = 1;
                dest.color = src.color;
            }
            if (src.ablaze.b[2] == 1)
            {
                dest.ablaze.b[2] = 1;
                dest.font_weight = src.font_weight;
            }
            if (src.ablaze.b[3] == 1)
            {
                dest.ablaze.b[3] = 1;
                dest.font_size = src.font_size;
            }
            if (src.ablaze.b[4] == 1)
            {
                dest.ablaze.b[4] = 1;
                dest.text_align = src.text_align;
            }
            if (src.ablaze.b[5] == 1)
            {
                dest.ablaze.b[5] = 1;
                dest.font_style = src.font_style;
            }
            if (dest.ablaze.b[6] == 1)
            {
                dest.ablaze.b[6] = 1;
                dest.margin_top = src.margin_top;
            }
            if (dest.ablaze.b[7] == 1)
            {
                dest.ablaze.b[7] = 1;
                dest.background_color = src.background_color;
            }
            if (dest.ablaze.b[12] == 1)
            {
                dest.ablaze.b[12] = 1;
                dest.text_decoration = src.text_decoration;
            }
            if (dest.ablaze.b[0x11] == 1)
            {
                dest.ablaze.b[0x11] = 1;
                dest.margin_bottom = src.margin_bottom;
            }
        }

        private void detect_lang(eRtfLanguage _rtfLanguage, ref int lang, ref bool hieroglyph, ref int codepage_id, ref int charset)
        {
            if (_rtfLanguage == eRtfLanguage.l_Albanian)
            {
                lang = 0x41c;
            }
            else if (_rtfLanguage == eRtfLanguage.l_English)
            {
                lang = 0x409;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Belgian)
            {
                lang = 0x813;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Bulgarian)
            {
                lang = 0x402;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Hungarian)
            {
                lang = 0x40e;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Danish)
            {
                lang = 0x406;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Italian)
            {
                lang = 0x410;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Spanish)
            {
                lang = 0xc0a;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Latvian)
            {
                lang = 0x426;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Lithuanian)
            {
                lang = 0x427;
            }
            else if (_rtfLanguage == eRtfLanguage.l_German)
            {
                lang = 0x407;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Netherlands)
            {
                lang = 0x413;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Norwegian)
            {
                lang = 0x814;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Portuguese)
            {
                lang = 0x816;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Romanian)
            {
                lang = 0x418;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Russian)
            {
                lang = 0x419;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Ukrainian)
            {
                lang = 0x422;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Finnish)
            {
                lang = 0x40b;
            }
            else if (_rtfLanguage == eRtfLanguage.l_French)
            {
                lang = 0x40c;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Czech)
            {
                lang = 0x405;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Swedish)
            {
                lang = 0x41d;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Swedish)
            {
                lang = 0x41d;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Turkish)
            {
                lang = 0x41f;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Japanese)
            {
                lang = 0x409;
                hieroglyph = true;
                codepage_id = 0x3a4;
                charset = 0x80;
            }
            else if (_rtfLanguage == eRtfLanguage.l_SimplifiedChinese)
            {
                lang = 0x409;
                hieroglyph = true;
                codepage_id = 0x3a8;
                charset = 0x86;
            }
            else if (_rtfLanguage == eRtfLanguage.l_TraditionalChinese)
            {
                lang = 0x409;
                hieroglyph = true;
                codepage_id = 950;
                charset = 0x88;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Korean)
            {
                lang = 0x409;
                hieroglyph = true;
                codepage_id = 0x3b5;
                charset = 0x81;
            }
            else if (_rtfLanguage == eRtfLanguage.l_Thai)
            {
                lang = 0x409;
                hieroglyph = true;
                codepage_id = 0x36a;
                charset = 0x80;
            }
            else
            {
                lang = 0x409;
            }
        }

        private string DownloadFile(string url)
        {
            string str = "";
            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                str = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
            }
            catch
            {
            }
            return str;
        }

        private int EncodingToASCII(string htmlString, ByteClass buf)
        {
            buf.Clear();
            for (int i = 0; i < htmlString.Length; i++)
            {
                if (htmlString[i] > '\x00ff')
                {
                    buf.Add("&#" + ((int)htmlString[i]).ToString() + ";");
                }
                else
                {
                    buf.Add((byte)htmlString[i]);
                }
            }
            return 0;
        }

        private void fill_columns_width(int[] td_width, int[] td_percent_width, int td, int table_width, int percent_width_table, int screen_w)
        {
            int num = 0;
            int num2 = 0;
            if ((this._preserveNestedTables == 1) && (td != 0))
            {
                int num3;
                if (percent_width_table == 1)
                {
                    if (screen_w > 0)
                    {
                        table_width = screen_w;
                    }
                    else
                    {
                        table_width = this.SCREEN_W_DEF;
                    }
                }
                for (num3 = 0; num3 < td; num3++)
                {
                    if (td_percent_width[num3] == 1)
                    {
                        td_width[num3] = (td_width[num3] * table_width) / 100;
                    }
                    num2 += td_width[num3];
                    if (td_width[num3] == 0)
                    {
                        num++;
                    }
                }
                if ((table_width >= num2) && (num2 > 0))
                {
                    for (num3 = 0; num3 < td; num3++)
                    {
                        if (td_width[num3] == 0)
                        {
                            td_width[num3] = (table_width - num2) / num;
                            if (td_width[num3] == 0)
                            {
                                td_width[num3]++;
                            }
                        }
                    }
                    num2 = 0;
                    for (num3 = 0; num3 < (td - 1); num3++)
                    {
                        td_width[num3] = (this.TBLEN * td_width[num3]) / table_width;
                        td_width[num3] = num2 + td_width[num3];
                    }
                    td_width[td - 1] = this.TBLEN;
                }
                else
                {
                    this.set_default_td_width(td_width, td, this.TBLEN);
                }
            }
            else
            {
                this.set_default_td_width(td_width, td, this.TBLEN);
            }
        }

        private int get_ansicpg(string language)
        {
            int num = 0x4e4;
            if (string.Compare(language, "Turkish", true) == 0)
            {
                return 0x4e6;
            }
            if (string.Compare(language, "Arabic", true) == 0)
            {
                return 0x4e8;
            }
            if (string.Compare(language, "Russian", true) == 0)
            {
                num = 0x4e3;
            }
            return num;
        }

        private bool get_border(ByteClass buf, int posStartBuf, int max)
        {
            int index = posStartBuf;
            int num2 = 0;
            ByteClass s = new ByteClass(0x20);
            int num3 = 0;
            while ((buf.b[index] != 0x3e) && (index < max))
            {
                if (((((buf.b[index] == 0x62) || (buf.b[index] == 0x42)) && ((buf.b[index + 1] == 0x6f) || (buf.b[index + 1] == 0x4f))) && (((buf.b[index + 2] == 0x72) || (buf.b[index + 2] == 0x52)) && ((buf.b[index + 3] == 100) || (buf.b[index + 3] == 0x44)))) && (((buf.b[index + 4] == 0x65) || (buf.b[index + 4] == 0x45)) && ((buf.b[index + 5] == 0x72) || (buf.b[index + 5] == 0x52))))
                {
                    index += 5;
                    while (buf.b[index] != 0x3d)
                    {
                        if (buf.b[index] == 0x3e)
                        {
                            return false;
                        }
                        index++;
                    }
                    while ((buf.b[index] < 0x30) || (buf.b[index] > 0x39))
                    {
                        if (buf.b[index] == 0x3e)
                        {
                            return false;
                        }
                        index++;
                    }
                    num2 = 0;
                    while (((buf.b[index] >= 0x30) && (buf.b[index] <= 0x39)) && (num2 < 20))
                    {
                        if (buf.b[index] == 0x3e)
                        {
                            break;
                        }
                        num2++;
                        s.Add(buf.b[index]);
                        index++;
                    }
                    num3 = this.ToInt(s);
                    break;
                }
                index++;
            }
            return (num3 > 0);
        }

        private int get_max_tblen_width(nest_tables prev_tab)
        {
            int tBLEN = this.TBLEN;
            int num2 = prev_tab.td_up_curcol;
            int num3 = prev_tab.tr_cur - 1;
            if (num3 < 0)
            {
                num3 = 0;
            }
            if (num2 > 0)
            {
                tBLEN = prev_tab.table_map[num3, num2 + 1] - prev_tab.table_map[num3, num2];
            }
            else
            {
                tBLEN = prev_tab.table_map[num3, 1];
            }
            if ((tBLEN > 0) && (tBLEN <= this.TBLEN))
            {
                return tBLEN;
            }
            return this.TBLEN;
        }

        private int get_table_width(nest_tables cur_tab, nest_tables prev_tab)
        {
            int tBLEN = this.TBLEN;
            int num2 = prev_tab.td_up_curcol;
            int num3 = prev_tab.tr_cur - 1;
            float num6 = 1f;
            if (num3 < 0)
            {
                num3 = 0;
            }
            if (num2 > 0)
            {
                tBLEN = prev_tab.table_map[num3, num2 + 1] - prev_tab.table_map[num3, num2];
            }
            else
            {
                tBLEN = prev_tab.table_map[num3, 1];
            }
            if (cur_tab.change_width != 1)
            {
                for (int i = 0; i < cur_tab.table_p.rows; i++)
                {
                    for (int j = 1; j < (cur_tab.table_p.cols + 1); j++)
                    {
                        cur_tab.table_map[i, j] = (int)(cur_tab.table_map[i, j] * num6);
                    }
                }
            }
            cur_tab.change_width = 1;
            if (tBLEN <= 0)
            {
                tBLEN = this.TBLEN;
            }
            return tBLEN;
        }

        private int get_width(ByteClass buf, int posStart, int max, ref int percent, ref int colspan, ref int rowspan, ArrayList color_list, int color_list_num, ref int bgcolor, CSS_params CSS_param, ref int valign, ref int align)
        {
            int index = 0;
            colspan = 1;
            rowspan = 1;
            int num2 = 0;
            ByteClass class2 = new ByteClass(20);
            ByteClass class3 = new ByteClass(20);
            int width = 0;
            ByteClass class4 = new ByteClass(0x29);
            bool flag = false;
            if (bgcolor != -1111)
            {
                bgcolor = 0;
            }
            percent = 0;
            ByteClass class5 = new ByteClass(0x1f);
            if (valign != -1111)
            {
                valign = this.VALIGN_TOP;
            }
            index = posStart;
            while ((buf.b[index] != 0x3e) && (index < max))
            {
                if ((((this.IS_DELIMITER(buf.b[index]) && ((buf.b[index + 1] == 0x77) || (buf.b[index + 1] == 0x57))) && ((buf.b[index + 2] == 0x69) || (buf.b[index + 2] == 0x49))) && (((buf.b[index + 3] == 100) || (buf.b[index + 3] == 0x44)) && ((buf.b[index + 4] == 0x74) || (buf.b[index + 4] == 0x54)))) && ((buf.b[index + 5] == 0x68) || (buf.b[index + 5] == 0x48)))
                {
                    index += 4;
                    while ((buf.b[index] != 0x3d) && (buf.b[index] != 0x3a))
                    {
                        if (buf.b[index] == 0x3e)
                        {
                            return width;
                        }
                        index++;
                    }
                    while ((buf.b[index] < 0x30) || (buf.b[index] > 0x39))
                    {
                        if (buf.b[index] == 0x3e)
                        {
                            return width;
                        }
                        index++;
                    }
                    num2 = 0;
                    class2.Clear();
                    while ((buf.b[index] >= 0x30) && (buf.b[index] <= 0x39))
                    {
                        if (buf.b[index] == 0x3e)
                        {
                            return width;
                        }
                        class2.Add(buf.b[index]);
                        index++;
                    }
                    if (buf.b[index] == 0x25)
                    {
                        percent = 1;
                    }
                    width = class2.ByteToInt();
                    class2.Clear();
                }
                else if (((((buf.b[index] == 0x63) || (buf.b[index] == 0x43)) && ((buf.b[index + 1] == 0x6f) || (buf.b[index + 1] == 0x4f))) && (((buf.b[index + 2] == 0x6c) || (buf.b[index + 2] == 0x4c)) && ((buf.b[index + 3] == 0x73) || (buf.b[index + 3] == 0x53)))) && ((((buf.b[index + 4] == 0x70) || (buf.b[index + 4] == 80)) && ((buf.b[index + 5] == 0x61) || (buf.b[index + 5] == 0x41))) && ((buf.b[index + 6] == 110) || (buf.b[index + 6] == 0x4e))))
                {
                    index += 6;
                    while (buf.b[index] != 0x3d)
                    {
                        if (buf.b[index] == 0x3e)
                        {
                            return width;
                        }
                        index++;
                    }
                    while ((buf.b[index] < 0x30) || (buf.b[index] > 0x39))
                    {
                        if (buf.b[index] == 0x3e)
                        {
                            return width;
                        }
                        index++;
                    }
                    num2 = 0;
                    class2.Clear();
                    while ((buf.b[index] >= 0x30) && (buf.b[index] <= 0x39))
                    {
                        if (buf.b[index] == 0x3e)
                        {
                            return width;
                        }
                        class2.Add(buf.b[index]);
                        index++;
                    }
                    colspan = class2.ByteToInt();
                }
                else
                {
                    if (((((buf.b[index] == 0x72) || (buf.b[index] == 0x52)) && ((buf.b[index + 1] == 0x6f) || (buf.b[index + 1] == 0x4f))) && (((buf.b[index + 2] == 0x77) || (buf.b[index + 2] == 0x57)) && ((buf.b[index + 3] == 0x73) || (buf.b[index + 3] == 0x53)))) && ((((buf.b[index + 4] == 0x70) || (buf.b[index + 4] == 80)) && ((buf.b[index + 5] == 0x61) || (buf.b[index + 5] == 0x41))) && ((buf.b[index + 6] == 110) || (buf.b[index + 6] == 0x4e))))
                    {
                        index += 6;
                        while (buf.b[index] != 0x3d)
                        {
                            if (buf.b[index] == 0x3e)
                            {
                                return width;
                            }
                            index++;
                        }
                        while ((buf.b[index] < 0x30) || (buf.b[index] > 0x39))
                        {
                            if (buf.b[index] == 0x3e)
                            {
                                return width;
                            }
                            index++;
                        }
                        num2 = 0;
                        class2.Clear();
                        while ((buf.b[index] >= 0x30) && (buf.b[index] <= 0x39))
                        {
                            if (buf.b[index] == 0x3e)
                            {
                                return width;
                            }
                            class2.Add(buf.b[index]);
                            index++;
                        }
                        rowspan = class2.ByteToInt();
                        class2.Clear();
                        continue;
                    }
                    if (((((buf.b[index] == 0x62) || (buf.b[index] == 0x42)) && ((buf.b[index + 1] == 0x67) || (buf.b[index + 1] == 0x47))) && (((buf.b[index + 2] == 0x63) || (buf.b[index + 2] == 0x43)) && ((buf.b[index + 3] == 0x6f) || (buf.b[index + 3] == 0x4f)))) && ((((buf.b[index + 4] == 0x6c) || (buf.b[index + 4] == 0x4c)) && ((buf.b[index + 5] == 0x6f) || (buf.b[index + 5] == 0x4f))) && (((buf.b[index + 6] == 0x72) || (buf.b[index + 6] == 0x52)) && (buf.b[index + 7] == 0x3d))))
                    {
                        index += 8;
                        if ((color_list_num != -1111) && (color_list != null))
                        {
                            class3.Clear();
                            this.read_color(buf, ref index, max, class3);
                            for (num2 = 0; num2 < color_list_num; num2++)
                            {
                                if ((class3.byteCmpi(color_list[num2].ToString()) == 0) && (bgcolor != -1111))
                                {
                                    bgcolor = num2 + 3;
                                }
                            }
                        }
                        continue;
                    }
                    if ((((((buf.b[index] == 0x62) || (buf.b[index] == 0x42)) && ((buf.b[index + 1] == 0x61) || (buf.b[index + 1] == 0x41))) && (((buf.b[index + 2] == 0x63) || (buf.b[index + 2] == 0x43)) && ((buf.b[index + 3] == 0x6b) || (buf.b[index + 3] == 0x4b)))) && ((((buf.b[index + 4] == 0x67) || (buf.b[index + 4] == 0x47)) && ((buf.b[index + 5] == 0x72) || (buf.b[index + 5] == 0x52))) && (((buf.b[index + 6] == 0x6f) || (buf.b[index + 6] == 0x4f)) && ((buf.b[index + 7] == 0x75) || (buf.b[index + 7] == 0x55))))) && (((((buf.b[index + 8] == 110) || (buf.b[index + 8] == 0x4e)) && ((buf.b[index + 9] == 100) || (buf.b[index + 9] == 0x44))) && (((buf.b[index + 10] == 0x2d) && ((buf.b[index + 11] == 0x63) || (buf.b[index + 11] == 0x43))) && ((buf.b[index + 12] == 0x6f) || (buf.b[index + 12] == 0x4f)))) && ((((buf.b[index + 13] == 0x6c) || (buf.b[index + 13] == 0x4c)) && ((buf.b[index + 14] == 0x6f) || (buf.b[index + 14] == 0x4f))) && (((buf.b[index + 15] == 0x72) || (buf.b[index + 15] == 0x52)) && (buf.b[index + 0x10] == 0x3a)))))
                    {
                        index += 0x11;
                        if ((color_list_num != -1111) && (color_list != null))
                        {
                            class3.Clear();
                            this.read_color(buf, ref index, max, class3);
                            for (num2 = 0; num2 < color_list_num; num2++)
                            {
                                if ((class3.byteCmpi(color_list[num2].ToString()) == 0) && (bgcolor != -1111))
                                {
                                    bgcolor = num2 + 3;
                                }
                            }
                        }
                        continue;
                    }
                    if (((((buf.b[index] == 0x63) || (buf.b[index] == 0x43)) && ((buf.b[index + 1] == 0x6c) || (buf.b[index + 1] == 0x4c))) && (((buf.b[index + 2] == 0x61) || (buf.b[index + 2] == 0x41)) && ((buf.b[index + 3] == 0x73) || (buf.b[index + 3] == 0x53)))) && (((buf.b[index + 4] == 0x73) || (buf.b[index + 4] == 0x53)) && (buf.b[index + 5] == 0x3d)))
                    {
                        index += 5;
                        class4.Clear();
                        this.read_value_exact(buf, ref index, max, class4);
                        if (class4.len > 0)
                        {
                            flag = true;
                        }
                    }
                    if ((((buf.b[index] == 0x69) || (buf.b[index] == 0x49)) && ((buf.b[index + 1] == 100) || (buf.b[index + 1] == 0x44))) && (buf.b[index + 2] == 0x3d))
                    {
                        index += 2;
                        class4.Clear();
                        this.read_value_exact(buf, ref index, max, class4);
                        if (class4.len > 0)
                        {
                            flag = true;
                        }
                    }
                    if (((((buf.b[index] == 0x76) || (buf.b[index] == 0x56)) && ((buf.b[index + 1] == 0x61) || (buf.b[index + 1] == 0x41))) && (((buf.b[index + 2] == 0x6c) || (buf.b[index + 2] == 0x4c)) && ((buf.b[index + 3] == 0x69) || (buf.b[index + 3] == 0x49)))) && ((((buf.b[index + 4] == 0x67) || (buf.b[index + 4] == 0x47)) && ((buf.b[index + 5] == 110) || (buf.b[index + 5] == 0x4e))) && (buf.b[index + 6] == 0x3d)))
                    {
                        index += 7;
                        class5.Clear();
                        this.read_value_exact(buf, ref index, max, class5);
                        if ((class5.len > 0) && (valign != -1111))
                        {
                            if (class5.byteCmp("center") == 0)
                            {
                                valign = this.VALIGN_CENTER;
                            }
                            else if (class5.byteCmp("middle") == 0)
                            {
                                valign = this.VALIGN_CENTER;
                            }
                            else if (class5.byteCmp("bottom") == 0)
                            {
                                valign = this.VALIGN_BOTTOM;
                            }
                            else
                            {
                                valign = this.VALIGN_TOP;
                            }
                        }
                        continue;
                    }
                    if (((((buf.b[index] == 0x61) || (buf.b[index] == 0x41)) && ((buf.b[index + 1] == 0x6c) || (buf.b[index + 1] == 0x4c))) && (((buf.b[index + 2] == 0x69) || (buf.b[index + 2] == 0x49)) && ((buf.b[index + 3] == 0x67) || (buf.b[index + 3] == 0x47)))) && (((buf.b[index + 4] == 110) || (buf.b[index + 4] == 0x4e)) && (buf.b[index + 5] == 0x3d)))
                    {
                        index += 6;
                        class5.Clear();
                        this.read_value_exact(buf, ref index, max, class5);
                        if ((class5.len > 0) && (align != -1111))
                        {
                            if (class5.byteCmp("center") == 0)
                            {
                                align = this.ALIGN_CENTER;
                            }
                            else if (class5.byteCmp("right") == 0)
                            {
                                align = this.ALIGN_RIGHT;
                            }
                        }
                        continue;
                    }
                    index++;
                }
            }
            if ((CSS_param != null) && flag)
            {
                flag = false;
                num2 = 0;
                while (num2 < CSS_param.styles)
                {
                    if ((class4.byteCmp(((CSS_styles)CSS_param.CSS_style[num2]).name) == 0) && ((((CSS_styles)CSS_param.CSS_style[num2]).css_tag_type == CSS_tag_type.TD_CSS) || (((CSS_styles)CSS_param.CSS_style[num2]).css_tag_type == CSS_tag_type.UNKNOWN_CSS)))
                    {
                        flag = true;
                        break;
                    }
                    num2++;
                }
                if (flag)
                {
                    if ((((CSS_styles)CSS_param.CSS_style[num2]).ablaze.b[7] == 1) && (bgcolor != -1111))
                    {
                        bgcolor = ((CSS_styles)CSS_param.CSS_style[num2]).background_color + 3;
                    }
                    if (((CSS_styles)CSS_param.CSS_style[num2]).ablaze.b[11] == 1)
                    {
                        width = ((CSS_styles)CSS_param.CSS_style[num2]).width;
                        if (((CSS_styles)CSS_param.CSS_style[num2]).width_in_percent)
                        {
                            percent = 1;
                        }
                    }
                }
            }
            return width;
        }

        private int get_width_img(ByteClass buf, int posStartBuf, int max, ByteClass img_folder)
        {
            int num5;
            int width = 0;
            bool flag = false;
            int num2 = 0;
            int index = posStartBuf + 1;
            int num4 = 1;
            ByteClass s = new ByteClass(0x20);
            while (index < max)
            {
                if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x61) || (buf.b[index + 2] == 0x41))) && (((buf.b[index + 3] == 0x62) || (buf.b[index + 3] == 0x42)) && ((buf.b[index + 4] == 0x6c) || (buf.b[index + 4] == 0x4c)))) && (((buf.b[index + 5] == 0x65) || (buf.b[index + 5] == 0x45)) && ((buf.b[index + 6] == 0x3e) || (buf.b[index + 6] == 0x20))))
                {
                    index += 6;
                    num4++;
                    while ((num4 > 1) && (index < max))
                    {
                        if ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && ((buf.b[index + 3] == 0x61) || (buf.b[index + 3] == 0x41))) && (((buf.b[index + 4] == 0x62) || (buf.b[index + 4] == 0x42)) && ((buf.b[index + 5] == 0x6c) || (buf.b[index + 5] == 0x4c)))) && (((buf.b[index + 6] == 0x65) || (buf.b[index + 6] == 0x45)) && (buf.b[index + 7] == 0x3e)))
                        {
                            num4--;
                            index += 6;
                        }
                        else if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x61) || (buf.b[index + 2] == 0x41))) && (((buf.b[index + 3] == 0x62) || (buf.b[index + 3] == 0x42)) && ((buf.b[index + 4] == 0x6c) || (buf.b[index + 4] == 0x4c)))) && (((buf.b[index + 5] == 0x65) || (buf.b[index + 5] == 0x45)) && ((buf.b[index + 6] == 0x3e) || (buf.b[index + 6] == 0x20))))
                        {
                            num4++;
                        }
                        index++;
                    }
                }
                else if ((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x69) || (buf.b[index + 1] == 0x49))) && ((buf.b[index + 2] == 0x6d) || (buf.b[index + 2] == 0x4d))) && (((buf.b[index + 3] == 0x67) || (buf.b[index + 3] == 0x47)) && (buf.b[index + 4] == 0x20)))
                {
                    flag = true;
                    num2 = index;
                    while ((buf.b[index] != 0x3e) && (index < max))
                    {
                        if (((((buf.b[index] == 0x77) || (buf.b[index] == 0x57)) && ((buf.b[index + 1] == 0x69) || (buf.b[index + 1] == 0x49))) && (((buf.b[index + 2] == 100) || (buf.b[index + 2] == 0x44)) && ((buf.b[index + 3] == 0x74) || (buf.b[index + 3] == 0x54)))) && ((buf.b[index + 4] == 0x68) || (buf.b[index + 4] == 0x48)))
                        {
                            index += 4;
                            while ((buf.b[index] != 0x3d) && (buf.b[index] != 0x3a))
                            {
                                if (buf.b[index] == 0x3e)
                                {
                                    return width;
                                }
                                index++;
                            }
                            while ((buf.b[index] < 0x30) || (buf.b[index] > 0x39))
                            {
                                if (buf.b[index] == 0x3e)
                                {
                                    return width;
                                }
                                index++;
                            }
                            num5 = 0;
                            while ((buf.b[index] >= 0x30) && (buf.b[index] <= 0x39))
                            {
                                if (buf.b[index] == 0x3e)
                                {
                                    return width;
                                }
                                num5++;
                                s.Add(buf.b[index]);
                                index++;
                            }
                            width = this.ToInt(s);
                        }
                        else
                        {
                            index++;
                        }
                    }
                }
                else if (((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && (((buf.b[index + 3] == 100) || (buf.b[index + 3] == 0x44)) || ((buf.b[index + 3] == 0x68) || (buf.b[index + 3] == 0x48)))) && (buf.b[index + 4] == 0x3e))
                {
                    break;
                }
                index++;
            }
            if (flag && (width == 0))
            {
                index = num2;
                bool cyr = false;
                ByteClass class3 = new ByteClass(0x20);
                ByteClass newb = new ByteClass();
                int num6 = 0;
                int len = 0;
                ByteClass folder = new ByteClass();
                if ((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x69) || (buf.b[index + 1] == 0x49))) && ((buf.b[index + 2] == 0x6d) || (buf.b[index + 2] == 0x4d))) && (((buf.b[index + 3] == 0x67) || (buf.b[index + 3] == 0x47)) && (buf.b[index + 4] == 0x20)))
                {
                    while ((buf.b[index] != 0x3e) && (index < max))
                    {
                        if ((((buf.b[index] == 0x73) || (buf.b[index] == 0x53)) && ((buf.b[index + 1] == 0x72) || (buf.b[index + 1] == 0x52))) && ((buf.b[index + 2] == 0x63) || (buf.b[index + 2] == 0x43)))
                        {
                            while ((buf.b[index] != 0x3e) && (index < max))
                            {
                                if (buf.b[index] == 0x3d)
                                {
                                    while ((buf.b[index] != 0x3e) && (index < max))
                                    {
                                        if ((buf.b[index] == 0x22) || (buf.b[index] == 0x27))
                                        {
                                            break;
                                        }
                                        if ((buf.b[index] == 0x20) && ((buf.b[index + 1] != 0x22) || (buf.b[index + 1] != 0x27)))
                                        {
                                            index++;
                                            break;
                                        }
                                        if (((buf.b[index + 1] != 0x20) && (buf.b[index + 1] != 0x22)) && (buf.b[index + 1] != 0x27))
                                        {
                                            index++;
                                            break;
                                        }
                                        index++;
                                    }
                                    break;
                                }
                                index++;
                            }
                            num5 = 0;
                            int num8 = 0;
                            if ((buf.b[index] != 0x22) && (buf.b[index] != 0x27))
                            {
                                goto Label_07CB;
                            }
                            num8 = buf.b[index];
                            index++;
                            while ((buf.b[index] != 0x3e) && (index < max))
                            {
                                if (buf.b[index] == num8)
                                {
                                    break;
                                }
                                if (buf.b[index] == 0x26)
                                {
                                    index += this.special_symbols_rtf(buf, index, max, class3, cyr);
                                    newb.Add(class3);
                                    num5 += class3.len;
                                    index++;
                                }
                                else
                                {
                                    newb.Add(buf.b[index]);
                                    num5++;
                                    index++;
                                }
                            }
                        }
                        goto Label_0802;
                    Label_077B:
                        if (buf.b[index] == 0x26)
                        {
                            index += this.special_symbols_rtf(buf, index, max, class3, cyr);
                            newb.Add(class3);
                            num5 += class3.len;
                            index++;
                        }
                        else
                        {
                            newb.Add(buf.b[index]);
                            num5++;
                            index++;
                        }
                Label_07CB:
                    if (((buf.b[index] != 0x3e) && (index < max)) && (((buf.b[index] != 0x22) && (buf.b[index] != 0x27)) && (buf.b[index] != 0x20)))
                    {
                        goto Label_077B;
                    }
            Label_0802:
                index++;
                    }
                    len = newb.len;
                    for (num5 = 0; num5 < len; num5++)
                    {
                        if (newb.b[num5] == 0x2f)
                        {
                            newb.b[num5] = 0x5c;
                        }
                    }
                    folder.Clear();
                    folder.Add(img_folder);
                    if (((newb.b[1] != 0x3a) && (newb.b[2] != 0x5c)) && ((newb.b[0] != 0x2e) && (newb.b[1] != 0x2e)))
                    {
                        folder.Add(newb);
                    }
                    else
                    {
                        this.combine_img_path(folder, newb);
                    }
                    num5 = 0;
                    num6 = 0;
                    while (folder.b[num5] != 0)
                    {
                        if (folder.b[num5] == 0x5c)
                        {
                            if ((num6 > 0) && (newb.b[num6 - 1] != 0x5c))
                            {
                                newb.b[num6++] = folder.b[num5];
                            }
                        }
                        else
                        {
                            newb.b[num6++] = folder.b[num5];
                        }
                        num5++;
                    }
                    newb.b[num6] = 0;
                    folder.Clear();
                    folder.Add(newb);
                    if (newb.ToByteCStartPos(newb.len - 3).byteCmpi("GIF") == 0)
                    {
                        this.GIF_info(newb, ref width);
                    }
                    return (width * this.PX_TO_TWIPS);
                }
            }
            return (width * this.PX_TO_TWIPS);
        }

        private string GetFontByEnum(eFontFace _fontFace)
        {
            string str = "Arial";
            if (_fontFace == eFontFace.f_Arial)
            {
                return "Arial";
            }
            if (_fontFace == eFontFace.f_Times_New_Roman)
            {
                return "Times New Roman";
            }
            if (_fontFace == eFontFace.f_Verdana)
            {
                return "Verdana";
            }
            if (_fontFace == eFontFace.f_Helvetica)
            {
                return "Helvetica";
            }
            if (_fontFace == eFontFace.f_Courier)
            {
                return "Courier";
            }
            if (_fontFace == eFontFace.f_Courier_New)
            {
                return "Courier New";
            }
            if (_fontFace == eFontFace.f_Times)
            {
                return "Times";
            }
            if (_fontFace == eFontFace.f_Georgia)
            {
                return "Georgia";
            }
            if (_fontFace == eFontFace.f_MS_Sans_Serif)
            {
                return "MS Sans Serif";
            }
            if (_fontFace == eFontFace.f_Futura)
            {
                return "Futura";
            }
            if (_fontFace == eFontFace.f_Arial_Narrow)
            {
                return "Arial Narrow";
            }
            if (_fontFace == eFontFace.f_Garamond)
            {
                return "Garamond";
            }
            if (_fontFace == eFontFace.f_Impact)
            {
                return "Impact";
            }
            if (_fontFace == eFontFace.f_Lucida_Console)
            {
                return "Lucida Console";
            }
            if (_fontFace == eFontFace.f_Tahoma)
            {
                return "Tahoma";
            }
            if (_fontFace == eFontFace.f_Inform)
            {
                return "Inform";
            }
            if (_fontFace == eFontFace.f_Symbol)
            {
                return "Symbol";
            }
            if (_fontFace == eFontFace.f_WingDings)
            {
                return "WingDings";
            }
            if (_fontFace == eFontFace.f_Traditional_Arabic)
            {
                str = "Traditional Arabic";
            }
            return str;
        }

        private void GIF_info(ByteClass path, ref int width)
        {
            width = 0;
        }

        private int hex_to_dec(ByteClass str)
        {
            int num = 0;
            int num4 = 0;
            int index = str.len - 1;
            for (int i = 0; index >= 0; i++)
            {
                if ((str.b[index] >= 0x41) && (str.b[index] <= 70))
                {
                    num4 = (str.b[index] - 0x41) + 10;
                }
                else if ((str.b[index] >= 0x61) && (str.b[index] <= 0x66))
                {
                    num4 = (str.b[index] - 0x61) + 10;
                }
                else
                {
                    num4 = str.b[index] - 0x30;
                }
                num += num4 * ((int)Math.Pow(16, (double)i));
                index--;
            }
            return num;
        }

        private int hex_to_dec(ByteClass str, int posStart)
        {
            int num = 0;
            int num4 = 0;
            int len = str.len;
            for (int i = 0; len >= 0; i++)
            {
                if ((str.b[len] >= 0x41) && (str.b[len] <= 70))
                {
                    num4 = (str.b[len] - 0x41) + 10;
                }
                else if ((str.b[len] >= 0x61) && (str.b[len] <= 0x66))
                {
                    num4 = (str.b[len] - 0x61) + 10;
                }
                else
                {
                    num4 = str.b[len] - 0x30;
                }
                num += num4 * this.pow(0x10, i);
                len--;
            }
            return num;
        }

        private ByteClass HtmlToHtml(ByteClass HtmlIn)
        {
            ByteClass buf = new ByteClass(HtmlIn.len);
            ByteClass bOut = new ByteClass();
            ByteClass message = new ByteClass();
            buf = HtmlIn;
            int index = 0;
            int num2 = 0;
            new ByteClass();
            new ByteClass();
            ByteClass class5 = new ByteClass();
            ByteClass class6 = new ByteClass();
            class5.Clear();
            class5.Add(this.HtmlPath);
            message.Clear();
            message.Add(".Net module started\n");
            if (this._createTraceFile == 1)
            {
                this.MyTrace(message, this.TraceFilePath);
            }
            for (index = 0; index < buf.len; index++)
            {
                if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x62) || (buf.b[index + 1] == 0x42))) && ((buf.b[index + 2] == 0x61) || (buf.b[index + 2] == 0x41))) && (((buf.b[index + 3] == 0x73) || (buf.b[index + 3] == 0x53)) && ((buf.b[index + 4] == 0x65) || (buf.b[index + 4] == 0x45)))) && ((buf.b[index + 5] == 0x20) || (buf.b[index + 5] == 10)))
                {
                    num2 = index;
                    index += 5;
                    while ((buf.b[index] != 0x3e) && (index < buf.len))
                    {
                        if ((((buf.b[index] == 0x68) || (buf.b[index] == 0x48)) && ((buf.b[index + 1] == 0x72) || (buf.b[index + 1] == 0x52))) && (((buf.b[index + 2] == 0x65) || (buf.b[index + 2] == 0x45)) && ((buf.b[index + 3] == 0x66) || (buf.b[index + 3] == 70))))
                        {
                            index += 4;
                            class6.Clear();
                            this.read_value(buf, ref index, class6);
                            this.BaseURL = System.Text.Encoding.UTF8.GetString(class6.b, 0, class6.len);
                        }
                        index++;
                    }
                    this.LeaveTag(buf, num2, bOut);
                }
                else if ((((((this._deleteTables == 1) && (buf.b[index] == 60)) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x61) || (buf.b[index + 2] == 0x41))) && (((buf.b[index + 3] == 0x62) || (buf.b[index + 3] == 0x42)) && ((buf.b[index + 4] == 0x6c) || (buf.b[index + 4] == 0x4c)))) && (((buf.b[index + 5] == 0x65) || (buf.b[index + 5] == 0x45)) && (((buf.b[index + 6] == 0x3e) || (buf.b[index + 6] == 0x20)) || (buf.b[index + 6] == 10))))
                {
                    index += 6;
                    int num3 = 1;
                    while ((num3 > 0) && (index < buf.len))
                    {
                        if ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && ((buf.b[index + 3] == 0x61) || (buf.b[index + 3] == 0x41))) && (((buf.b[index + 4] == 0x62) || (buf.b[index + 4] == 0x42)) && ((buf.b[index + 5] == 0x6c) || (buf.b[index + 5] == 0x4c)))) && (((buf.b[index + 6] == 0x65) || (buf.b[index + 6] == 0x45)) && (buf.b[index + 7] == 0x3e)))
                        {
                            num3--;
                            index += 6;
                        }
                        else if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x61) || (buf.b[index + 2] == 0x41))) && (((buf.b[index + 3] == 0x62) || (buf.b[index + 3] == 0x42)) && ((buf.b[index + 4] == 0x6c) || (buf.b[index + 4] == 0x4c)))) && (((buf.b[index + 5] == 0x65) || (buf.b[index + 5] == 0x45)) && ((buf.b[index + 6] == 0x3e) || (buf.b[index + 6] == 0x20))))
                        {
                            num3++;
                        }
                        index++;
                    }
                }
                else
                {
                    if (this._preserveTables == 0)
                    {
                        if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x61) || (buf.b[index + 2] == 0x41))) && (((buf.b[index + 3] == 0x62) || (buf.b[index + 3] == 0x42)) && ((buf.b[index + 4] == 0x6c) || (buf.b[index + 4] == 0x4c)))) && (((buf.b[index + 5] == 0x65) || (buf.b[index + 5] == 0x45)) && (((buf.b[index + 6] == 0x3e) || (buf.b[index + 6] == 0x20)) || (buf.b[index + 6] == 10))))
                        {
                            while ((buf.b[index + 6] != 0x3e) && (index < buf.len))
                            {
                                index++;
                            }
                            index += 6;
                            goto Label_14C6;
                        }
                        if ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && ((buf.b[index + 3] == 0x61) || (buf.b[index + 3] == 0x41))) && (((buf.b[index + 4] == 0x62) || (buf.b[index + 4] == 0x42)) && ((buf.b[index + 5] == 0x6c) || (buf.b[index + 5] == 0x4c)))) && (((buf.b[index + 6] == 0x65) || (buf.b[index + 6] == 0x45)) && (buf.b[index + 7] == 0x3e)))
                        {
                            index += 7;
                            goto Label_14C6;
                        }
                        if ((((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x63) || (buf.b[index + 1] == 0x43))) && ((buf.b[index + 2] == 0x61) || (buf.b[index + 2] == 0x41))) && (((buf.b[index + 3] == 0x70) || (buf.b[index + 3] == 80)) && ((buf.b[index + 4] == 0x74) || (buf.b[index + 4] == 0x54)))) && ((((buf.b[index + 5] == 0x69) || (buf.b[index + 5] == 0x49)) && ((buf.b[index + 6] == 0x6f) || (buf.b[index + 6] == 0x4f))) && ((buf.b[index + 7] == 110) || (buf.b[index + 7] == 0x4e)))) && (((buf.b[index + 8] == 0x3e) || (buf.b[index + 8] == 0x20)) || (buf.b[index + 8] == 10)))
                        {
                            while ((buf.b[index + 8] != 0x3e) && (index < buf.len))
                            {
                                index++;
                            }
                            index += 8;
                            goto Label_14C6;
                        }
                        if ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x63) || (buf.b[index + 2] == 0x43))) && ((buf.b[index + 3] == 0x61) || (buf.b[index + 3] == 0x41))) && (((buf.b[index + 4] == 0x70) || (buf.b[index + 4] == 80)) && ((buf.b[index + 5] == 0x74) || (buf.b[index + 5] == 0x54)))) && ((((buf.b[index + 6] == 0x69) || (buf.b[index + 6] == 0x49)) && ((buf.b[index + 7] == 0x6f) || (buf.b[index + 7] == 0x4f))) && (((buf.b[index + 8] == 110) || (buf.b[index + 8] == 0x4e)) && (buf.b[index + 9] == 0x3e))))
                        {
                            index += 9;
                            goto Label_14C6;
                        }
                        if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x68) || (buf.b[index + 2] == 0x48))) && (((buf.b[index + 3] == 0x65) || (buf.b[index + 3] == 0x45)) && ((buf.b[index + 4] == 0x61) || (buf.b[index + 4] == 0x41)))) && (((buf.b[index + 5] == 100) || (buf.b[index + 5] == 0x44)) && (((buf.b[index + 6] == 0x3e) || (buf.b[index + 6] == 0x20)) || (buf.b[index + 6] == 10))))
                        {
                            while ((buf.b[index + 6] != 0x3e) && (index < buf.len))
                            {
                                index++;
                            }
                            index += 6;
                            goto Label_14C6;
                        }
                        if ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && ((buf.b[index + 3] == 0x68) || (buf.b[index + 3] == 0x48))) && (((buf.b[index + 4] == 0x65) || (buf.b[index + 4] == 0x45)) && ((buf.b[index + 5] == 0x61) || (buf.b[index + 5] == 0x41)))) && (((buf.b[index + 6] == 100) || (buf.b[index + 6] == 0x44)) && (buf.b[index + 7] == 0x3e)))
                        {
                            index += 7;
                            goto Label_14C6;
                        }
                        if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x66) || (buf.b[index + 2] == 70))) && (((buf.b[index + 3] == 0x6f) || (buf.b[index + 3] == 0x4f)) && ((buf.b[index + 4] == 0x6f) || (buf.b[index + 4] == 0x4f)))) && (((buf.b[index + 5] == 0x74) || (buf.b[index + 5] == 0x54)) && (((buf.b[index + 6] == 0x3e) || (buf.b[index + 6] == 0x20)) || (buf.b[index + 6] == 10))))
                        {
                            while ((buf.b[index + 6] != 0x3e) && (index < buf.len))
                            {
                                index++;
                            }
                            index += 6;
                            goto Label_14C6;
                        }
                        if ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && ((buf.b[index + 3] == 0x66) || (buf.b[index + 3] == 70))) && (((buf.b[index + 4] == 0x6f) || (buf.b[index + 4] == 0x4f)) && ((buf.b[index + 5] == 0x6f) || (buf.b[index + 5] == 0x4f)))) && (((buf.b[index + 6] == 0x74) || (buf.b[index + 6] == 0x54)) && (buf.b[index + 7] == 0x3e)))
                        {
                            index += 7;
                            goto Label_14C6;
                        }
                        if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x62) || (buf.b[index + 2] == 0x42))) && (((buf.b[index + 3] == 0x6f) || (buf.b[index + 3] == 0x4f)) && ((buf.b[index + 4] == 100) || (buf.b[index + 4] == 0x44)))) && (((buf.b[index + 5] == 0x79) || (buf.b[index + 5] == 0x59)) && (((buf.b[index + 6] == 0x3e) || (buf.b[index + 6] == 0x20)) || (buf.b[index + 6] == 10))))
                        {
                            while ((buf.b[index + 6] != 0x3e) && (index < buf.len))
                            {
                                index++;
                            }
                            index += 6;
                            goto Label_14C6;
                        }
                        if ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && ((buf.b[index + 3] == 0x62) || (buf.b[index + 3] == 0x42))) && (((buf.b[index + 4] == 0x6f) || (buf.b[index + 4] == 0x4f)) && ((buf.b[index + 5] == 100) || (buf.b[index + 5] == 0x44)))) && (((buf.b[index + 6] == 0x79) || (buf.b[index + 6] == 0x59)) && (buf.b[index + 7] == 0x3e)))
                        {
                            index += 7;
                            goto Label_14C6;
                        }
                        if ((((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x63) || (buf.b[index + 1] == 0x43))) && ((buf.b[index + 2] == 0x6f) || (buf.b[index + 2] == 0x4f))) && (((buf.b[index + 3] == 0x6c) || (buf.b[index + 3] == 0x4c)) && ((buf.b[index + 4] == 0x67) || (buf.b[index + 4] == 0x47)))) && ((((buf.b[index + 5] == 0x72) || (buf.b[index + 5] == 0x52)) && ((buf.b[index + 6] == 0x6f) || (buf.b[index + 6] == 0x4f))) && (((buf.b[index + 7] == 0x75) || (buf.b[index + 7] == 0x55)) && ((buf.b[index + 8] == 0x70) || (buf.b[index + 8] == 80))))) && (((buf.b[index + 9] == 0x3e) || (buf.b[index + 9] == 0x20)) || (buf.b[index + 9] == 10)))
                        {
                            while ((buf.b[index + 9] != 0x3e) && (index < buf.len))
                            {
                                index++;
                            }
                            index += 9;
                            goto Label_14C6;
                        }
                        if ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x63) || (buf.b[index + 2] == 0x43))) && ((buf.b[index + 3] == 0x6f) || (buf.b[index + 3] == 0x4f))) && (((buf.b[index + 4] == 0x6c) || (buf.b[index + 4] == 0x4c)) && ((buf.b[index + 5] == 0x67) || (buf.b[index + 5] == 0x47)))) && ((((buf.b[index + 6] == 0x72) || (buf.b[index + 6] == 0x52)) && ((buf.b[index + 7] == 0x6f) || (buf.b[index + 7] == 0x4f))) && ((((buf.b[index + 8] == 0x75) || (buf.b[index + 8] == 0x55)) && ((buf.b[index + 9] == 0x70) || (buf.b[index + 9] == 80))) && (buf.b[index + 10] == 0x3e))))
                        {
                            index += 10;
                            goto Label_14C6;
                        }
                        if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x63) || (buf.b[index + 1] == 0x43))) && ((buf.b[index + 2] == 0x6f) || (buf.b[index + 2] == 0x4f))) && ((buf.b[index + 3] == 0x6c) || (buf.b[index + 3] == 0x4c))) && (((buf.b[index + 4] == 0x3e) || (buf.b[index + 4] == 0x20)) || (buf.b[index + 4] == 10)))
                        {
                            while ((buf.b[index + 4] != 0x3e) && (index < buf.len))
                            {
                                index++;
                            }
                            index += 4;
                            goto Label_14C6;
                        }
                        if (((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x63) || (buf.b[index + 2] == 0x43))) && ((buf.b[index + 3] == 0x6f) || (buf.b[index + 3] == 0x4f))) && (((buf.b[index + 4] == 0x6c) || (buf.b[index + 4] == 0x4c)) && (buf.b[index + 5] == 0x3e)))
                        {
                            index += 5;
                            goto Label_14C6;
                        }
                        if ((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x72) || (buf.b[index + 2] == 0x52))) && ((buf.b[index + 3] == 0x3e) || (buf.b[index + 3] == 0x20)))
                        {
                            while ((buf.b[index + 3] != 0x3e) && (index < buf.len))
                            {
                                index++;
                            }
                            index += 3;
                            bOut.Add("\n<p>");
                            goto Label_14C6;
                        }
                        if ((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && (((buf.b[index + 2] == 100) || (buf.b[index + 2] == 0x44)) || ((buf.b[index + 2] == 0x68) || (buf.b[index + 2] == 0x48)))) && (((buf.b[index + 3] == 0x3e) || (buf.b[index + 3] == 0x20)) || (buf.b[index + 3] == 10)))
                        {
                            while ((buf.b[index + 3] != 0x3e) && (index < buf.len))
                            {
                                index++;
                            }
                            index += 3;
                            goto Label_14C6;
                        }
                        if (((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && ((buf.b[index + 3] == 0x72) || (buf.b[index + 3] == 0x52))) && (buf.b[index + 4] == 0x3e))
                        {
                            index += 4;
                            bOut.Add("</p>");
                            goto Label_14C6;
                        }
                        if (((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && (((buf.b[index + 3] == 100) || (buf.b[index + 3] == 0x44)) || ((buf.b[index + 3] == 0x68) || (buf.b[index + 3] == 0x48)))) && (buf.b[index + 4] == 0x3e))
                        {
                            index += 4;
                            bOut.Add("&nbsp;");
                            goto Label_14C6;
                        }
                    }
                    if ((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x69) || (buf.b[index + 1] == 0x49))) && ((buf.b[index + 2] == 0x6d) || (buf.b[index + 2] == 0x4d))) && (((buf.b[index + 3] == 0x67) || (buf.b[index + 3] == 0x47)) && ((buf.b[index + 4] == 0x20) || (buf.b[index + 4] == 10))))
                    {
                        if (this._deleteImages == 1)
                        {
                            while ((buf.b[index] != 0x3e) && (index < buf.len))
                            {
                                index++;
                            }
                            goto Label_14C6;
                        }
                        if (this._preserveImages == 0)
                        {
                            while ((buf.b[index] != 0x3e) && (index < buf.len))
                            {
                                bOut.Add(buf.b[index]);
                                index++;
                            }
                            bOut.Add(">");
                            goto Label_14C6;
                        }
                    }
                    bOut.Add(buf.b[index]);
                Label_14C6: ;
                }
            }
            return bOut;
        }

        private bool IS_DELIMITER(byte a)
        {
            if (((a != 0x20) && (a != 10)) && ((a != 13) && (a != 10)))
            {
                return false;
            }
            return true;
        }

        private bool IS_XTHAN(byte a)
        {
            if ((a != 0x3e) && (a != 60))
            {
                return false;
            }
            return true;
        }

        private bool isalpha(byte b)
        {
            return char.IsLetter((char)b);
        }

        private bool isdigit(byte b)
        {
            return ((b >= 0x30) && (b <= 0x39));
        }

        private bool ispunct(byte b)
        {
            return char.IsPunctuation((char)b);
        }

        private bool isspace(byte b)
        {
            return char.IsWhiteSpace((char)b);
        }

        private void LeaveTag(ByteClass bIn, int ct_in, ByteClass bOut)
        {
            while ((bIn.b[ct_in] != 0x3e) && (ct_in < bIn.len))
            {
                bOut.Add(bIn.b[ct_in]);
                ct_in++;
            }
            bOut.Add(">");
        }

        private void MakeOLsymbol(int num, ByteClass strOLsymbol, int listType)
        {
            if (listType == 7)
            {
                if ((num < 1) || (num > 0x1a))
                {
                    num = 1;
                }
                strOLsymbol.Clear();
                strOLsymbol.Add((byte)((0x61 + num) - 1));
            }
            else if (listType == 8)
            {
                if ((num < 1) || (num > 0x1a))
                {
                    num = 1;
                }
                strOLsymbol.Clear();
                strOLsymbol.Add((byte)((num - 1) + 0x41));
            }
            else if (listType == 5)
            {
                if ((num < 1) || (num > 20))
                {
                    num = 1;
                }
                strOLsymbol.Clear();
                switch (num)
                {
                    case 1:
                        strOLsymbol.Add("i");
                        return;

                    case 2:
                        strOLsymbol.Add("ii");
                        return;

                    case 3:
                        strOLsymbol.Add("iii");
                        return;

                    case 4:
                        strOLsymbol.Add("iv");
                        return;

                    case 5:
                        strOLsymbol.Add("v");
                        return;

                    case 6:
                        strOLsymbol.Add("vi");
                        return;

                    case 7:
                        strOLsymbol.Add("vii");
                        return;

                    case 8:
                        strOLsymbol.Add("viii");
                        return;

                    case 9:
                        strOLsymbol.Add("ix");
                        return;

                    case 10:
                        strOLsymbol.Add("x");
                        return;

                    case 11:
                        strOLsymbol.Add("xi");
                        return;

                    case 12:
                        strOLsymbol.Add("xii");
                        return;

                    case 13:
                        strOLsymbol.Add("xiii");
                        return;

                    case 14:
                        strOLsymbol.Add("xiv");
                        return;

                    case 15:
                        strOLsymbol.Add("xv");
                        return;

                    case 0x10:
                        strOLsymbol.Add("xvi");
                        return;

                    case 0x11:
                        strOLsymbol.Add("xvii");
                        return;

                    case 0x12:
                        strOLsymbol.Add("xviii");
                        return;

                    case 0x13:
                        strOLsymbol.Add("xix");
                        return;

                    case 20:
                        strOLsymbol.Add("xx");
                        return;
                }
            }
            else if (listType == 6)
            {
                if ((num < 1) || (num > 20))
                {
                    num = 1;
                }
                strOLsymbol.Clear();
                switch (num)
                {
                    case 1:
                        strOLsymbol.Add("I");
                        return;

                    case 2:
                        strOLsymbol.Add("II");
                        return;

                    case 3:
                        strOLsymbol.Add("III");
                        return;

                    case 4:
                        strOLsymbol.Add("IV");
                        return;

                    case 5:
                        strOLsymbol.Add("V");
                        return;

                    case 6:
                        strOLsymbol.Add("VI");
                        return;

                    case 7:
                        strOLsymbol.Add("VII");
                        return;

                    case 8:
                        strOLsymbol.Add("VIII");
                        return;

                    case 9:
                        strOLsymbol.Add("IX");
                        return;

                    case 10:
                        strOLsymbol.Add("X");
                        return;

                    case 11:
                        strOLsymbol.Add("XI");
                        return;

                    case 12:
                        strOLsymbol.Add("XII");
                        return;

                    case 13:
                        strOLsymbol.Add("XIII");
                        return;

                    case 14:
                        strOLsymbol.Add("XIV");
                        return;

                    case 15:
                        strOLsymbol.Add("XV");
                        return;

                    case 0x10:
                        strOLsymbol.Add("XVI");
                        return;

                    case 0x11:
                        strOLsymbol.Add("XVII");
                        return;

                    case 0x12:
                        strOLsymbol.Add("XVIII");
                        return;

                    case 0x13:
                        strOLsymbol.Add("XIX");
                        return;

                    case 20:
                        strOLsymbol.Add("XX");
                        return;
                }
            }
            else
            {
                strOLsymbol.Clear();
            }
        }

        private void MyTrace(ByteClass message, string path)
        {
            try
            {
                FileStream stream = new FileStream(path, FileMode.Append);
                stream.Write(message.b, 0, message.len);
                stream.Close();
            }
            catch (Exception)
            {
            }
        }

        private void nested_table_clear(nest_tables nested_table)
        {
            nested_table.table = false;
            nested_table.table_border_visible = false;
            nested_table.table_p.cols = 0;
            nested_table.table_p.percent_width = 0;
            nested_table.table_p.rows = 0;
            nested_table.table_p.table_tag = 0;
            nested_table.table_p.table_width = 0;
            nested_table.table_p.tableAlign = 0;
            nested_table.tblen = 0;
            nested_table.td = 0;
            nested_table.td_align = "";
            nested_table.td_bg = 0;
            nested_table.td_open = false;
            nested_table.td_percent_width.Initialize();
            nested_table.td_up_columns = 0;
            nested_table.td_up_curcol = 0;
            nested_table.td_up_width = 0;
            nested_table.td_width.Initialize();
            nested_table.tdAlignColgroup.Initialize();
            nested_table.tr_align = "";
            nested_table.tr_bg = 0;
            nested_table.tr_cur = 0;
            nested_table.tr_open = false;
            nested_table.table_level = 0;
        }

        internal void OnBeforeImageDownload(HttpWebRequest aWebRequest)
        {
            if (aWebRequest == null)
            {
                throw new ArgumentNullException("aWebRequest");
            }
            if (this.BeforeImageDownload != null)
            {
                this.BeforeImageDownload(aWebRequest);
            }
        }

        private int pow(int num, int step)
        {
            int num2 = num;
            for (int i = 0; i < step; i++)
            {
                num *= num2;
            }
            return num;
        }

        private bool read_color(ByteClass buf, ref int ct, int max_pos, ByteClass color_str)
        {
            color_str.Clear();
            ByteClass class2 = new ByteClass(0x19);
            int index = ct;
            int num2 = 0;
            int num3 = 1;
            bool flag = false;
            bool flag2 = true;
            while ((((buf.b[index] == 0x27) || (buf.b[index] == 0x22)) || ((buf.b[index] == 0x20) || (buf.b[index] == 0x3d))) || (buf.b[index] == 0x5c))
            {
                index++;
            }
            while ((((buf.b[index] != 0x27) && (buf.b[index] != 0x5c)) && ((buf.b[index] != 0x22) && (buf.b[index] != 0x3b))) && (((buf.b[index] != 0x3e) && (index < max_pos)) && (num2 < 0x15)))
            {
                if ((buf.b[index] <= 0x20) && !flag)
                {
                    break;
                }
                if (buf.b[index] == 40)
                {
                    flag = true;
                }
                class2.Add((byte)char.ToLower((char)buf.b[index]));
                num2++;
                index++;
            }
            if (class2.b[0] == 0x23)
            {
                for (num2 = 0; num2 < 6; num2++)
                {
                    if (((class2.b[num2 + 1] >= 0x30) && (class2.b[num2 + 1] <= 0x39)) || (((class2.b[num2 + 1] >= 0x61) || (class2.b[num2 + 1] >= 0x41)) && ((class2.b[num2 + 1] <= 0x66) || (class2.b[num2 + 1] <= 70))))
                    {
                        color_str.Add(class2.b[num2 + 1]);
                    }
                    else
                    {
                        num3 = 0;
                        break;
                    }
                }
            }
            else if (class2.byteCmp("aliceblue") == 0)
            {
                color_str.Clear();
                color_str.Add("F0F8FF");
            }
            else if (class2.byteCmp("antiquewhite") == 0)
            {
                color_str.Clear();
                color_str.Add("FAEBD7");
            }
            else if (class2.byteCmp("aqua") == 0)
            {
                color_str.Clear();
                color_str.Add("00FFFF");
            }
            else if (class2.byteCmp("aquamarine") == 0)
            {
                color_str.Clear();
                color_str.Add("7FFFD4");
            }
            else if (class2.byteCmp("azure") == 0)
            {
                color_str.Clear();
                color_str.Add("F0FFFF");
            }
            else if (class2.byteCmp("transparent") == 0)
            {
                color_str.Clear();
                color_str.Add("FFFFFF");
            }
            else if (class2.byteCmp("beige") == 0)
            {
                color_str.Clear();
                color_str.Add("F5F5DC");
            }
            else if (class2.byteCmp("bisque") == 0)
            {
                color_str.Clear();
                color_str.Add("FFE4C4");
            }
            else if (class2.byteCmp("black") == 0)
            {
                color_str.Clear();
                color_str.Add("000000");
            }
            else if (class2.byteCmp("blanchedalmond") == 0)
            {
                color_str.Clear();
                color_str.Add("FFEBCD");
            }
            else if (class2.byteCmp("blue") == 0)
            {
                color_str.Clear();
                color_str.Add("0000FF");
            }
            else if (class2.byteCmp("blueviolet") == 0)
            {
                color_str.Clear();
                color_str.Add("8A2BE2");
            }
            else if (class2.byteCmp("brown") == 0)
            {
                color_str.Clear();
                color_str.Add("A52A2A");
            }
            else if (class2.byteCmp("burlywood") == 0)
            {
                color_str.Clear();
                color_str.Add("DE8887");
            }
            else if (class2.byteCmp("cadetblue") == 0)
            {
                color_str.Clear();
                color_str.Add("5F9EA0");
            }
            else if (class2.byteCmp("chocolate") == 0)
            {
                color_str.Clear();
                color_str.Add("D2691E");
            }
            else if (class2.byteCmp("coral") == 0)
            {
                color_str.Clear();
                color_str.Add("FF7F50");
            }
            else if (class2.byteCmp("cornflowerblue") == 0)
            {
                color_str.Clear();
                color_str.Add("6495ED");
            }
            else if (class2.byteCmp("cornsilk") == 0)
            {
                color_str.Clear();
                color_str.Add("FFF8DC");
            }
            else if (class2.byteCmp("crimson") == 0)
            {
                color_str.Clear();
                color_str.Add("DC143C");
            }
            else if (class2.byteCmp("cyan") == 0)
            {
                color_str.Clear();
                color_str.Add("00FFFF");
            }
            else if (class2.byteCmp("darkblue") == 0)
            {
                color_str.Clear();
                color_str.Add("00008B");
            }
            else if (class2.byteCmp("darkcyan") == 0)
            {
                color_str.Clear();
                color_str.Add("008B8B");
            }
            else if (class2.byteCmp("darkgoldenrod") == 0)
            {
                color_str.Clear();
                color_str.Add("B8860B");
            }
            else if (class2.byteCmp("darkgray") == 0)
            {
                color_str.Clear();
                color_str.Add("A9A9A9");
            }
            else if (class2.byteCmp("darkgreen") == 0)
            {
                color_str.Clear();
                color_str.Add("006400");
            }
            else if (class2.byteCmp("darkkhaki") == 0)
            {
                color_str.Clear();
                color_str.Add("BDB76D");
            }
            else if (class2.byteCmp("darkmagenta") == 0)
            {
                color_str.Clear();
                color_str.Add("8B008B");
            }
            else if (class2.byteCmp("darkolivegreen") == 0)
            {
                color_str.Clear();
                color_str.Add("556B2F");
            }
            else if (class2.byteCmp("darkorange") == 0)
            {
                color_str.Clear();
                color_str.Add("FF8C00");
            }
            else if (class2.byteCmp("darkorchid") == 0)
            {
                color_str.Clear();
                color_str.Add("9932CC");
            }
            else if (class2.byteCmp("darkred") == 0)
            {
                color_str.Clear();
                color_str.Add("8B0000");
            }
            else if (class2.byteCmp("darksalmon") == 0)
            {
                color_str.Clear();
                color_str.Add("E9967A");
            }
            else if (class2.byteCmp("darkseagreen") == 0)
            {
                color_str.Clear();
                color_str.Add("8FBC8F");
            }
            else if (class2.byteCmp("darkslateblue") == 0)
            {
                color_str.Clear();
                color_str.Add("483D8B");
            }
            else if (class2.byteCmp("darkslategray") == 0)
            {
                color_str.Clear();
                color_str.Add("2F4F4F");
            }
            else if (class2.byteCmp("darkturquoise") == 0)
            {
                color_str.Clear();
                color_str.Add("00CED1");
            }
            else if (class2.byteCmp("darkviolet") == 0)
            {
                color_str.Clear();
                color_str.Add("9400D3");
            }
            else if (class2.byteCmp("deeppink") == 0)
            {
                color_str.Clear();
                color_str.Add("FF1493");
            }
            else if (class2.byteCmp("deepskyblue") == 0)
            {
                color_str.Clear();
                color_str.Add("00BFFF");
            }
            else if (class2.byteCmp("dimgray") == 0)
            {
                color_str.Clear();
                color_str.Add("696969");
            }
            else if (class2.byteCmp("dodgerblue") == 0)
            {
                color_str.Clear();
                color_str.Add("1E90FF");
            }
            else if (class2.byteCmp("firebrick") == 0)
            {
                color_str.Clear();
                color_str.Add("B22222");
            }
            else if (class2.byteCmp("floralwhite") == 0)
            {
                color_str.Clear();
                color_str.Add("FFFAF0");
            }
            else if (class2.byteCmp("forestgreen") == 0)
            {
                color_str.Clear();
                color_str.Add("228B22");
            }
            else if (class2.byteCmp("fuchsia") == 0)
            {
                color_str.Clear();
                color_str.Add("FF00FF");
            }
            else if (class2.byteCmp("gainsboro") == 0)
            {
                color_str.Clear();
                color_str.Add("DCDCDC");
            }
            else if (class2.byteCmp("ghostwhite") == 0)
            {
                color_str.Clear();
                color_str.Add("F8F8FF");
            }
            else if (class2.byteCmp("gold") == 0)
            {
                color_str.Clear();
                color_str.Add("FFD700");
            }
            else if (class2.byteCmp("goldenrod") == 0)
            {
                color_str.Clear();
                color_str.Add("DAA520");
            }
            else if (class2.byteCmp("gray") == 0)
            {
                color_str.Clear();
                color_str.Add("808080");
            }
            else if (class2.byteCmp("green") == 0)
            {
                color_str.Clear();
                color_str.Add("008000");
            }
            else if (class2.byteCmp("greenyellow") == 0)
            {
                color_str.Clear();
                color_str.Add("ADFF2F");
            }
            else if (class2.byteCmp("honeydew") == 0)
            {
                color_str.Clear();
                color_str.Add("F0FFF0");
            }
            else if (class2.byteCmp("hotpink") == 0)
            {
                color_str.Clear();
                color_str.Add("FF69B4");
            }
            else if (class2.byteCmp("indianred") == 0)
            {
                color_str.Clear();
                color_str.Add("CD5C5C");
            }
            else if (class2.byteCmp("indigo") == 0)
            {
                color_str.Clear();
                color_str.Add("4B0082");
            }
            else if (class2.byteCmp("ivory") == 0)
            {
                color_str.Clear();
                color_str.Add("FFFFF0");
            }
            else if (class2.byteCmp("khaki") == 0)
            {
                color_str.Clear();
                color_str.Add("F0E68C");
            }
            else if (class2.byteCmp("lavender") == 0)
            {
                color_str.Clear();
                color_str.Add("E6E6FA");
            }
            else if (class2.byteCmp("lavenderblush") == 0)
            {
                color_str.Clear();
                color_str.Add("FFF0F5");
            }
            else if (class2.byteCmp("lemonchiffon") == 0)
            {
                color_str.Clear();
                color_str.Add("FFFACD");
            }
            else if (class2.byteCmp("lightblue") == 0)
            {
                color_str.Clear();
                color_str.Add("ADD8E6");
            }
            else if (class2.byteCmp("lightcoral") == 0)
            {
                color_str.Clear();
                color_str.Add("F08080");
            }
            else if (class2.byteCmp("lightcyan") == 0)
            {
                color_str.Clear();
                color_str.Add("E0FFFF");
            }
            else if (class2.byteCmp("lightgoldenrodyellow") == 0)
            {
                color_str.Clear();
                color_str.Add("FAFAD2");
            }
            else if (class2.byteCmp("lightgreen") == 0)
            {
                color_str.Clear();
                color_str.Add("90EE90");
            }
            else if (class2.byteCmp("lightpink") == 0)
            {
                color_str.Clear();
                color_str.Add("FFB6C1");
            }
            else if (class2.byteCmp("lightsalmon") == 0)
            {
                color_str.Clear();
                color_str.Add("FFA07A");
            }
            else if (class2.byteCmp("lightseagreen") == 0)
            {
                color_str.Clear();
                color_str.Add("20B2AA");
            }
            else if (class2.byteCmp("lightskyblue") == 0)
            {
                color_str.Clear();
                color_str.Add("87CEFA");
            }
            else if (class2.byteCmp("lightslategray") == 0)
            {
                color_str.Clear();
                color_str.Add("778899");
            }
            else if (class2.byteCmp("lightsteelblue") == 0)
            {
                color_str.Clear();
                color_str.Add("B0C4DE");
            }
            else if (class2.byteCmp("lightyellow") == 0)
            {
                color_str.Clear();
                color_str.Add("FFFFE0");
            }
            else if (class2.byteCmp("lime") == 0)
            {
                color_str.Clear();
                color_str.Add("00FF00");
            }
            else if (class2.byteCmp("limegreen") == 0)
            {
                color_str.Clear();
                color_str.Add("32CD32");
            }
            else if (class2.byteCmp("linen") == 0)
            {
                color_str.Clear();
                color_str.Add("FAF0F6");
            }
            else if (class2.byteCmp("magenta") == 0)
            {
                color_str.Clear();
                color_str.Add("FF00FF");
            }
            else if (class2.byteCmp("maroon") == 0)
            {
                color_str.Clear();
                color_str.Add("800000");
            }
            else if (class2.byteCmp("mediumaquamarine") == 0)
            {
                color_str.Clear();
                color_str.Add("66CDAA");
            }
            else if (class2.byteCmp("mediumblue") == 0)
            {
                color_str.Clear();
                color_str.Add("0000CD");
            }
            else if (class2.byteCmp("mediumorchid") == 0)
            {
                color_str.Clear();
                color_str.Add("BA55D3");
            }
            else if (class2.byteCmp("mediumpurple") == 0)
            {
                color_str.Clear();
                color_str.Add("9370DB");
            }
            else if (class2.byteCmp("mediumseagreen") == 0)
            {
                color_str.Clear();
                color_str.Add("3CB371");
            }
            else if (class2.byteCmp("mediumslateblue") == 0)
            {
                color_str.Clear();
                color_str.Add("7B68EE");
            }
            else if (class2.byteCmp("mediumspringgreen") == 0)
            {
                color_str.Clear();
                color_str.Add("00FA9A");
            }
            else if (class2.byteCmp("mediumturquoise") == 0)
            {
                color_str.Clear();
                color_str.Add("48D1CC");
            }
            else if (class2.byteCmp("mediumvioletred") == 0)
            {
                color_str.Clear();
                color_str.Add("C71585");
            }
            else if (class2.byteCmp("midnightblue") == 0)
            {
                color_str.Clear();
                color_str.Add("191970");
            }
            else if (class2.byteCmp("mintcream") == 0)
            {
                color_str.Clear();
                color_str.Add("F5FFFA");
            }
            else if (class2.byteCmp("mistyrose") == 0)
            {
                color_str.Clear();
                color_str.Add("FFE4E1");
            }
            else if (class2.byteCmp("moccasin") == 0)
            {
                color_str.Clear();
                color_str.Add("FFE4B5");
            }
            else if (class2.byteCmp("navajowhite") == 0)
            {
                color_str.Clear();
                color_str.Add("FFDEAD");
            }
            else if (class2.byteCmp("navy") == 0)
            {
                color_str.Clear();
                color_str.Add("000080");
            }
            else if (class2.byteCmp("oldlace") == 0)
            {
                color_str.Clear();
                color_str.Add("FDF5E6");
            }
            else if (class2.byteCmp("olive") == 0)
            {
                color_str.Clear();
                color_str.Add("808000");
            }
            else if (class2.byteCmp("olivedrab") == 0)
            {
                color_str.Clear();
                color_str.Add("6B8E23");
            }
            else if (class2.byteCmp("orange") == 0)
            {
                color_str.Clear();
                color_str.Add("FFA500");
            }
            else if (class2.byteCmp("orangered") == 0)
            {
                color_str.Clear();
                color_str.Add("FF4500");
            }
            else if (class2.byteCmp("orchid") == 0)
            {
                color_str.Clear();
                color_str.Add("DA70D6");
            }
            else if (class2.byteCmp("palegoldenrod") == 0)
            {
                color_str.Clear();
                color_str.Add("EEE8AA");
            }
            else if (class2.byteCmp("palegreen") == 0)
            {
                color_str.Clear();
                color_str.Add("98FB98");
            }
            else if (class2.byteCmp("paleturquoise") == 0)
            {
                color_str.Clear();
                color_str.Add("AFEEEE");
            }
            else if (class2.byteCmp("palevioletred") == 0)
            {
                color_str.Clear();
                color_str.Add("DB7093");
            }
            else if (class2.byteCmp("papayawhip") == 0)
            {
                color_str.Clear();
                color_str.Add("FFEFD5");
            }
            else if (class2.byteCmp("peachpuff") == 0)
            {
                color_str.Clear();
                color_str.Add("FFDAB9");
            }
            else if (class2.byteCmp("peru") == 0)
            {
                color_str.Clear();
                color_str.Add("CD853F");
            }
            else if (class2.byteCmp("pink") == 0)
            {
                color_str.Clear();
                color_str.Add("FFC0CB");
            }
            else if (class2.byteCmp("plum") == 0)
            {
                color_str.Clear();
                color_str.Add("DDA0DD");
            }
            else if (class2.byteCmp("powderblue") == 0)
            {
                color_str.Clear();
                color_str.Add("B0E0E6");
            }
            else if (class2.byteCmp("purple") == 0)
            {
                color_str.Clear();
                color_str.Add("800080");
            }
            else if (class2.byteCmp("red") == 0)
            {
                color_str.Clear();
                color_str.Add("FF0000");
            }
            else if (class2.byteCmp("rosybrown") == 0)
            {
                color_str.Clear();
                color_str.Add("BC8F8F");
            }
            else if (class2.byteCmp("royalblue") == 0)
            {
                color_str.Clear();
                color_str.Add("4169E1");
            }
            else if (class2.byteCmp("salmon") == 0)
            {
                color_str.Clear();
                color_str.Add("FA8072");
            }
            else if (class2.byteCmp("sandybrown") == 0)
            {
                color_str.Clear();
                color_str.Add("F4A460");
            }
            else if (class2.byteCmp("seagreen") == 0)
            {
                color_str.Clear();
                color_str.Add("2E2B57");
            }
            else if (class2.byteCmp("seashell") == 0)
            {
                color_str.Clear();
                color_str.Add("FFE5EE");
            }
            else if (class2.byteCmp("sienna") == 0)
            {
                color_str.Clear();
                color_str.Add("A0522D");
            }
            else if (class2.byteCmp("silver") == 0)
            {
                color_str.Clear();
                color_str.Add("C0C0C0");
            }
            else if (class2.byteCmp("skyblue") == 0)
            {
                color_str.Clear();
                color_str.Add("87CEEB");
            }
            else if (class2.byteCmp("slateblue") == 0)
            {
                color_str.Clear();
                color_str.Add("6A5ACD");
            }
            else if (class2.byteCmp("slategray") == 0)
            {
                color_str.Clear();
                color_str.Add("708090");
            }
            else if (class2.byteCmp("snow") == 0)
            {
                color_str.Clear();
                color_str.Add("FFFAFA");
            }
            else if (class2.byteCmp("springgreen") == 0)
            {
                color_str.Clear();
                color_str.Add("00FF7F");
            }
            else if (class2.byteCmp("steelblue") == 0)
            {
                color_str.Clear();
                color_str.Add("4682B4");
            }
            else if (class2.byteCmp("tan") == 0)
            {
                color_str.Clear();
                color_str.Add("D2B48C");
            }
            else if (class2.byteCmp("teal") == 0)
            {
                color_str.Clear();
                color_str.Add("008080");
            }
            else if (class2.byteCmp("thistle") == 0)
            {
                color_str.Clear();
                color_str.Add("D8BFD8");
            }
            else if (class2.byteCmp("tomato") == 0)
            {
                color_str.Clear();
                color_str.Add("FF6347");
            }
            else if (class2.byteCmp("turquoise") == 0)
            {
                color_str.Clear();
                color_str.Add("40E0D0");
            }
            else if (class2.byteCmp("violet") == 0)
            {
                color_str.Clear();
                color_str.Add("EE82EE");
            }
            else if (class2.byteCmp("wheat") == 0)
            {
                color_str.Clear();
                color_str.Add("F5DEB3");
            }
            else if (class2.byteCmp("white") == 0)
            {
                color_str.Clear();
                color_str.Add("FFFFFF");
            }
            else if (class2.byteCmp("whitesmoke") == 0)
            {
                color_str.Clear();
                color_str.Add("F5F5F5");
            }
            else if (class2.byteCmp("yellow") == 0)
            {
                color_str.Clear();
                color_str.Add("FFFF00");
            }
            else if (class2.byteCmp("yellowgreen") == 0)
            {
                color_str.Clear();
                color_str.Add("9ACD32");
            }
            else
            {
                num3 = 0;
            }
            if (num3 == 0)
            {
                if (class2.len != 6)
                {
                    if ((class2.IndexOf("rgb") != -1) && this.read_rgb_color(class2, color_str))
                    {
                        num3 = 1;
                    }
                }
                else
                {
                    num3 = 1;
                    for (index = 0; index < 6; index++)
                    {
                        if ((((class2.b[index] >= 0x61) && (class2.b[index] <= 0x66)) || ((class2.b[index] >= 0x41) && (class2.b[index] <= 70))) || ((class2.b[index] >= 0x30) && (class2.b[index] <= 0x39)))
                        {
                            num3 = 1;
                        }
                        else
                        {
                            num3 = 0;
                            break;
                        }
                    }
                    if (num3 == 1)
                    {
                        color_str.Clear();
                        color_str.Add(class2);
                    }
                }
            }
            if (num3 == 0)
            {
                color_str.Clear();
                color_str.Add("000000");
                flag2 = false;
            }
            return flag2;
        }

        private bool read_rgb_color(ByteClass rgb_str, ByteClass color_str)
        {
            int len = rgb_str.len;
            ByteClass class2 = new ByteClass(7);
            ByteClass str = new ByteClass(5);
            int num = 0;
            int num3 = 0;
            int num4 = 0;
            int num6 = 0;
            for (int i = 0; i < len; i++)
            {
                if (this.isdigit(rgb_str.b[i]) && (num6 == 0))
                {
                    this.read_value_CSS_tolower(rgb_str, ref i, len, class2, 5);
                    num = this.ToInt(class2);
                    num6++;
                }
                if (this.isdigit(rgb_str.b[i]) && (num6 == 1))
                {
                    this.read_value_CSS_tolower(rgb_str, ref i, len, class2, 5);
                    num3 = this.ToInt(class2);
                    num6++;
                }
                if (this.isdigit(rgb_str.b[i]) && (num6 == 2))
                {
                    this.read_value_CSS_tolower(rgb_str, ref i, len, class2, 5);
                    num4 = this.ToInt(class2);
                    num6++;
                }
            }
            if (num6 == 3)
            {
                color_str.Clear();
                this.tohex(num, str);
                color_str.Add(str, 2);
                this.tohex(num3, str);
                color_str.Add(str, 2);
                this.tohex(num4, str);
                color_str.Add(str, 2);
                return true;
            }
            return false;
        }

        private void read_value(ByteClass buf, ref int ct, ByteClass value_str)
        {
            int index = ct;
            int num2 = 0;
            value_str.Clear();
            while (((buf.b[index] == 0x27) || (buf.b[index] == 0x22)) || ((buf.b[index] == 0x20) || (buf.b[index] == 0x3d)))
            {
                index++;
            }
            while (((buf.b[index] != 0x27) && (buf.b[index] != 0x22)) && (((buf.b[index] > 0x20) && (buf.b[index] != 0x3e)) && (num2 < 30)))
            {
                value_str.Add((byte)char.ToLower((char)buf.b[index]));
                num2++;
                index++;
            }
        }

        private void read_value_big(ByteClass buf, ref int ct, int max_pos, ByteClass value_str)
        {
            int index = ct;
            int num2 = 0;
            value_str.Clear();
            while (((buf.b[index] == 0x27) || (buf.b[index] == 0x22)) || ((buf.b[index] == 0x20) || (buf.b[index] == 0x3d)))
            {
                index++;
            }
            while ((((buf.b[index] != 0x27) && (buf.b[index] != 0x22)) && ((buf.b[index] > 0x20) && (buf.b[index] != 0x3e))) && ((index < max_pos) && (num2 < 0x200)))
            {
                value_str.Add((byte)char.ToLower((char)buf.b[index]));
                num2++;
                index++;
            }
        }

        private void read_value_CSS(ByteClass buf, ref int ct, int max_pos, ByteClass value_str, int value_length, bool quote)
        {
            int index = ct;
            int num2 = 0;
            value_str.Clear();
            while ((((buf.b[index] == 0x27) || (buf.b[index] == 0x22)) || ((buf.b[index] == 0x20) || (buf.b[index] == 0x3d))) && (index < (buf.len - 1)))
            {
                index++;
            }
            while ((((buf.b[index] != 0x27) && (buf.b[index] != 0x22)) && ((buf.b[index] != 0x3a) && (buf.b[index] != 0x3d))) && ((((buf.b[index] != 0x3b) && (buf.b[index] != 0x2c)) && ((buf.b[index] != 0x2e) && (buf.b[index] != 0x7d))) && (((buf.b[index] > 0x1f) && (buf.b[index] != 0x3e)) && (index < (buf.len - 1)))))
            {
                if (((buf.b[index] == 0x20) && !quote) || (buf.b[index] == 0))
                {
                    break;
                }
                value_str.Add(buf.b[index]);
                num2++;
                index++;
            }
            if (ct == index)
            {
                index++;
            }
            ct = index;
        }

        private void read_value_CSS_tolower(ByteClass buf, ref int ct, int max_pos, ByteClass value_str, int value_length)
        {
            int index = ct;
            int num2 = 0;
            value_str.Clear();
            while (((buf.b[index] == 0x27) || (buf.b[index] == 0x22)) || ((buf.b[index] == 0x20) || (buf.b[index] == 0x3d)))
            {
                index++;
            }
            while ((((buf.b[index] != 0x27) && (buf.b[index] != 0x22)) && ((buf.b[index] != 0x3a) && (buf.b[index] != 0x3b))) && (((buf.b[index] != 0x2c) && (buf.b[index] != 0x7d)) && (((buf.b[index] > 0x20) && (buf.b[index] != 0x3e)) && (index < max_pos))))
            {
                value_str.Add((byte)char.ToLower((char)buf.b[index]));
                num2++;
                index++;
            }
            if (ct == index)
            {
                index++;
            }
            ct = index;
        }

        private void read_value_exact(ByteClass buf, ref int ct, int max_pos, ByteClass value_str)
        {
            int index = ct;
            int num2 = 0;
            value_str.Clear();
            while (((buf.b[index] == 0x27) || (buf.b[index] == 0x22)) || ((buf.b[index] == 0x20) || (buf.b[index] == 0x3d)))
            {
                index++;
            }
            while ((((buf.b[index] != 0x27) && (buf.b[index] != 0x22)) && ((buf.b[index] > 0x20) && (buf.b[index] != 0x3e))) && ((index < max_pos) && (num2 < 30)))
            {
                value_str.Add(buf.b[index]);
                num2++;
                index++;
            }
        }

        private void ReadValue(ByteClass buf, ref int ct, ByteClass valueStr)
        {
            int index = ct;
            bool flag = false;
            bool flag2 = false;
            valueStr.Clear();
            while (index < buf.len)
            {
                if (this.isalpha(buf.b[index]) || this.isdigit(buf.b[index]))
                {
                    break;
                }
                if (buf.b[index] == 0x27)
                {
                    flag = true;
                    index++;
                    break;
                }
                if (buf.b[index] == 0x22)
                {
                    flag2 = true;
                    index++;
                    break;
                }
                index++;
            }
            while (index < buf.len)
            {
                if (((((buf.b[index] == 0x20) || (buf.b[index] == 0x3e)) || ((buf.b[index] == 0x27) || (buf.b[index] == 0x22))) && (!flag && !flag2)) || (((buf.b[index] == 0x27) && flag) || ((buf.b[index] == 0x22) && flag2)))
                {
                    break;
                }
                valueStr.Add(buf.b[index]);
                index++;
            }
            ct = index;
        }

        private void seldirectory(ByteClass path)
        {
            int index = 0;
            index = path.len;
            while (((index > 0) && (path.b[index] != 0x5c)) && (path.b[index] != 0x2f))
            {
                index--;
            }
            path.len = index + 1;
        }

        private void selfile(ByteClass path, ByteClass file)
        {
            int num = 0;
            int index = 0;
            num = path.len - 1;
            index = num;
            while (((index > 0) && (path.b[index] != 0x5c)) && (path.b[index] != 0x2f))
            {
                index--;
            }
            while (index < (path.len - 1))
            {
                file.Add(path.b[index + 1]);
                index++;
            }
        }

        private void set_default_td_width(int[] td_width, int td, int MAX)
        {
            for (int i = 0; i < (td - 1); i++)
            {
                td_width[i] = (MAX * (i + 1)) / td;
            }
            td_width[td - 1] = MAX;
        }

        private void set_default_td_width_nested(nest_tables nested_table, int MAX)
        {
            for (int i = 0; i < (nested_table.td - 1); i++)
            {
                nested_table.td_width[i] = (MAX * (i + 1)) / nested_table.td;
            }
            nested_table.td_width[nested_table.td - 1] = MAX;
        }

        private int special_symbols_rtf(ByteClass bufout, int ct, int max, ByteClass special_string, bool cyr)
        {
            special_string.Clear();
            string[] strArray = new string[] { 
                "quot", "amp", "lt", "gt", "nbsp", "iexcl", "cent", "pound", "curren", "yen", "brvbar", "sect", "uml", "copy", "ordf", "laquo", 
                "not", "shy", "reg", "hibar", "deg", "plusm", "sup2", "sup3", "acute", "micro", "para", "middot", "cedil", "sup1", "ordm", "raquo", 
                "fraq14", "fraq12", "fraq34", "iquest", "Agrave", "Aacute", "Acirc", "Atilde", "Auml", "Aring", "AElig", "Ccedil", "Egrave", "Eacute", "Ecirc", "Euml", 
                "Igrave", "Iacute", "Icirc", "Iuml", "ETH", "Ntilde", "Ograve", "Oacute", "Ocirc", "Otilde", "Ouml", "times", "Oslash", "Ugrave", "Uacute", "Ucirc", 
                "Uuml", "Yacute", "THORN", "szlig", "agrave", "aacute", "acirc", "atilde", "auml", "aring", "aelig", "ccedil", "egrave", "eacute", "ecirc", "euml", 
                "igrave", "iacute", "icirc", "iuml", "eth", "ntilde", "ograve", "oacute", "ocirc", "otilde", "ouml", "divide", "oslash", "ugrave", "uacute", "ucirc", 
                "uuml", "yacute", "thorn", "yuml", "euro", "Alpha", "Beta", "Gamma", "Delta", "Epsilon", "Zeta", "Eta", "Theta", "Iota", "Kappa", "Lambda", 
                "Mu", "Nu", "Xi", "Omicron", "Pi", "Rho", "Sigma", "Tau", "Upsilon", "Phi", "Chi", "Psi", "Omega", "alpha", "beta", "gamma", 
                "delta", "epsilon", "zeta", "eta", "theta", "iota", "kappa", "lambda", "mu", "nu", "xi", "omicron", "pi", "rho", "sigmaf", "sigma", 
                "tau", "upsilon", "phi", "chi", "psi", "omega", "thetasym", "upsih", "piv", "bull", "hellip", "prime", "Prime", "oline", "frasl", "weierp", 
                "image", "real", "trade", "alefsym", "larr", "uarr", "rarr", "darr", "harr", "crarr", "lArr", "uArr", "rArr", "dArr", "hArr", "forall", 
                "part", "exist", "empty", "nabla", "isin", "notin", "ni", "prod", "sum", "minus", "lowast", "radic", "prop", "infin", "ang", "and", 
                "or", "cap", "cup", "int", "there4", "sim", "cong", "asymp", "ne", "equiv", "le", "ge", "sub", "sup", "nsub", "sube", 
                "supe", "oplus", "otimes", "perp", "sdot", "lceil", "rceil", "lfloor", "rfloor", "lang", "rang", "loz", "spades", "clubs", "hearts", "diams", 
                "OElig", "oelig", "Scaron", "scaron", "Yuml", "circ", "tilde", "ensp", "emsp", "thinsp", "zwnj", "zwj", "lrm", "rlm", "ndash", "mdash", 
                "lsquo", "rsquo", "sbquo", "ldquo", "rdquo", "bdquo", "dagger", "Dagger", "permil", "lsaquo", "rsaquo", "macr", "plusmn", "frac14", "frac12", "frac34"
             };
            int[] numArray = new int[] { 
                0x22, 0x26, 60, 0x3e, 160, 0xa1, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7, 0xa8, 0xa9, 170, 0xab, 
                0xac, 0xad, 0xae, 0xaf, 0xb0, 0xb1, 0xb2, 0xb3, 180, 0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xbb, 
                0xbc, 0xbd, 190, 0xbf, 0xc0, 0xc1, 0xc2, 0xc3, 0xc4, 0xc5, 0xc6, 0xc7, 200, 0xc9, 0xca, 0xcb, 
                0xcc, 0xcd, 0xce, 0xcf, 0xd0, 0xd1, 210, 0xd3, 0xd4, 0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda, 0xdb, 
                220, 0xdd, 0xde, 0xdf, 0xe0, 0xe1, 0xe2, 0xe3, 0xe4, 0xe5, 230, 0xe7, 0xe8, 0xe9, 0xea, 0xeb, 
                0xec, 0xed, 0xee, 0xef, 240, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 250, 0xfb, 
                0xfc, 0xfd, 0xfe, 0xff, 0x80, 0x391, 0x392, 0x393, 0x394, 0x395, 0x396, 0x397, 920, 0x399, 0x39a, 0x39b, 
                0x39c, 0x39d, 0x39e, 0x39f, 0x3a0, 0x3a1, 0x3a3, 0x3a4, 0x3a5, 0x3a6, 0x3a7, 0x3a8, 0x3a9, 0x3b1, 0x3b2, 0x3b3, 
                0x3b4, 0x3b5, 950, 0x3b7, 0x3b8, 0x3b9, 0x3ba, 0x3bb, 0x3bc, 0x3bd, 0x3be, 0x3bf, 960, 0x3c1, 0x3c2, 0x3c3, 
                0x3c4, 0x3c5, 0x3c6, 0x3c7, 0x3c8, 0x3c9, 0x3d1, 0x3d2, 0x3d6, 0x95, 0x85, 0x2032, 0x2033, 0x203e, 0x2044, 0x2118, 
                0x2111, 0x211c, 0x99, 0x2135, 0x2190, 0x2190, 0x2192, 0x2193, 0x2194, 0x21b5, 0x21d0, 0x21d1, 0x21d2, 0x21d3, 0x21d4, 0x2200, 
                0x2202, 0x2203, 0x2205, 0x2207, 0x2208, 0x2209, 0x220b, 0x220f, 0x2211, 0x2212, 0x2217, 0x221a, 0x221d, 0x221e, 0x2220, 0x2227, 
                0x2228, 0x2229, 0x222a, 0x222b, 0x2234, 0x223c, 0x2245, 0x2248, 0x2260, 0x2261, 0x2264, 0x2265, 0x2282, 0x2283, 0x2284, 0x2286, 
                0x2287, 0x2295, 0x2297, 0x22a5, 0x22c5, 0x2308, 0x2309, 0x230a, 0x230b, 0x2329, 0x232a, 0x25ca, 0x2660, 0x2663, 0x2665, 0x2666, 
                140, 0x9c, 0x8a, 0x9a, 0x9f, 0x88, 0x98, 0x2002, 0x2003, 0x2009, 0x200c, 0x200d, 0x200e, 0x200f, 150, 0x97, 
                0x91, 0x92, 130, 0x93, 0x94, 0x85, 0x86, 0x87, 0x89, 0x8b, 0x9b, 0xaf, 0xb1, 0xbc, 0xbd, 190
             };
            int index = 0;
            int length = 0;
            int num3 = 0;
            int num = 0;
            ByteClass str = new ByteClass(8);
            special_string.Clear();
            if ((((bufout.b[ct] == 0x26) && (bufout.b[ct + 1] == 110)) && ((bufout.b[ct + 2] == 0x62) && (bufout.b[ct + 3] == 0x73))) && ((bufout.b[ct + 4] == 0x70) && (bufout.b[ct + 5] == 0x3b)))
            {
                special_string.Add(" ");
                return 5;
            }
            if ((((bufout.b[ct] == 0x26) && (bufout.b[ct + 1] == 0x61)) && ((bufout.b[ct + 2] == 0x6d) && (bufout.b[ct + 3] == 0x70))) && (bufout.b[ct + 4] == 0x3b))
            {
                special_string.Add("&");
                return 4;
            }
            if (((bufout.b[ct] == 0x26) && (bufout.b[ct + 1] == 0x6c)) && ((bufout.b[ct + 2] == 0x74) && (bufout.b[ct + 3] == 0x3b)))
            {
                special_string.Add("<");
                return 3;
            }
            if (((bufout.b[ct] == 0x26) && (bufout.b[ct + 1] == 0x67)) && ((bufout.b[ct + 2] == 0x74) && (bufout.b[ct + 3] == 0x3b)))
            {
                special_string.Add(">");
                return 3;
            }
            if ((((bufout.b[ct] == 0x26) && (bufout.b[ct + 1] == 0x71)) && ((bufout.b[ct + 2] == 0x75) && (bufout.b[ct + 3] == 0x6f))) && ((bufout.b[ct + 4] == 0x74) && (((bufout.b[ct + 5] == 0x3b) || (bufout.b[ct + 5] == 0x20)) || (bufout.b[ct + 5] == 60))))
            {
                special_string.Add("\"");
                num3 = 5;
                if ((bufout.b[ct + 5] != 0x20) && (bufout.b[ct + 5] != 60))
                {
                    return num3;
                }
                return 4;
            }
            if ((bufout.b[ct] == 0x26) && (bufout.b[ct + 1] == 0x20))
            {
                special_string.Add("&");
                return 0;
            }
            if (bufout.b[ct + 1] == 0x23)
            {
                if (bufout.b[ct + 2] == 120)
                {
                    num3 = 3;
                    index = 0;
                    while (((bufout.b[ct + num3] != 0x3b) && (bufout.b[ct + num3] != 0x20)) && ((index < 10) && ((ct + num3) < max)))
                    {
                        special_string.Add(bufout.b[ct + num3]);
                        num3++;
                        index++;
                    }
                    if (index == 10)
                    {
                        special_string.Add("&");
                        return 1;
                    }
                    num = this.hex_to_dec(special_string);
                }
                else
                {
                    num3 = 2;
                    index = 0;
                    while (((bufout.b[ct + num3] != 0x3b) && (bufout.b[ct + num3] != 0x20)) && ((index < 10) && ((ct + num3) < max)))
                    {
                        special_string.Add(bufout.b[ct + num3]);
                        num3++;
                        index++;
                    }
                    if (index == 10)
                    {
                        special_string.Add("&");
                        return 0;
                    }
                    num = this.ToInt(special_string);
                    if (num == 160)
                    {
                        num = 0x20;
                    }
                }
                if ((num >= 0x21) && (num <= 0xff))
                {
                    special_string.Clear();
                    if (cyr)
                    {
                        special_string.Add(@"\f99\'" + num.ToString("x"));
                        return num3;
                    }
                    special_string.Add(@"\'" + num.ToString("x"));
                    return num3;
                }
                if (num == 0x20)
                {
                    special_string.Clear();
                    special_string.Add(" ");
                    return num3;
                }
                if (num > 0xff)
                {
                    special_string.Clear();
                    special_string.Add(@"\u" + num.ToString() + "F");
                    return num3;
                }
                special_string.Clear();
                return num3;
            }
            num3 = 1;
            index = 0;
            while (((bufout.b[ct + num3] != 0x3b) && (bufout.b[ct + num3] != 0x20)) && ((index < 10) && ((ct + num3) < max)))
            {
                special_string.Add(bufout.b[ct + num3]);
                num3++;
                index++;
            }
            if (index == 10)
            {
                special_string.Clear();
                special_string.Add("&");
                return 0;
            }
            num = 0;
            length = numArray.Length;
            for (index = 0; index < length; index++)
            {
                if (special_string.byteCmp(strArray[index]) == 0)
                {
                    num = numArray[index];
                    break;
                }
            }
            if ((num > 0) && (num <= 0xff))
            {
                this.tohex(num, str);
                special_string.Clear();
                special_string.Add(str);
                return num3;
            }
            if (num > 0xff)
            {
                special_string.Clear();
                special_string.Add(@"\u" + num.ToString() + "F");
                return num3;
            }
            if (num == 0)
            {
                special_string.Clear();
                special_string.Add("&");
                return 0;
            }
            return num3;
        }

        private int special_symbols_txt(ByteClass bufout, int ct, int max, ByteClass special_string)
        {
            string[] strArray = new string[] { 
                "quot", "amp", "lt", "gt", "nbsp", "iexcl", "cent", "pound", "curren", "yen", "brvbar", "sect", "uml", "copy", "ordf", "laquo", 
                "not", "shy", "reg", "hibar", "deg", "plusm", "sup2", "sup3", "acute", "micro", "para", "middot", "cedil", "sup1", "ordm", "raquo", 
                "fraq14", "fraq12", "fraq34", "iquest", "Agrave", "Aacute", "Acirc", "Atilde", "Auml", "Aring", "Aelig", "Ccedil", "Egrave", "Eacute", "Ecirc", "Euml", 
                "Igrave", "Iacute", "Icirc", "Iuml", "ETH", "Ntilde", "Ograve", "Oacute", "Ocirc", "Otilde", "Ouml", "times", "Oslash", "Ugrave", "Uacute", "Ucirc", 
                "Uuml", "Yacute", "THORN", "szlig", "agrave", "aacute", "acirc", "atilde", "auml", "aring", "elig", "ccedil", "egrave", "eacute", "ecirc", "euml", 
                "igrave", "iacute", "icirc", "iuml", "eth", "ntilde", "ograve", "oacute", "ocirc", "otilde", "ouml", "divide", "oslash", "ugrave", "uacute", "ucirc", 
                "uuml", "yacute", "thorn", "yuml"
             };
            int[] numArray = new int[] { 
                0x22, 0x26, 60, 0x3e, 160, 0xa1, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7, 0xa8, 0xa9, 170, 0xab, 
                0xac, 0xad, 0xae, 0xaf, 0xb0, 0xb1, 0xb2, 0xb3, 180, 0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xbb, 
                0xbc, 0xbd, 190, 0xbf, 0xc0, 0xc1, 0xc2, 0xc3, 0xc4, 0xc5, 0xc6, 0xc7, 200, 0xc9, 0xca, 0xcb, 
                0xcc, 0xcd, 0xce, 0xcf, 0xd0, 0xd1, 210, 0xd3, 0xd4, 0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda, 0xdb, 
                220, 0xdd, 0xde, 0xdf, 0xe0, 0xe1, 0xe2, 0xe3, 0xe4, 0xe5, 230, 0xe7, 0xe8, 0xe9, 0xea, 0xeb, 
                0xec, 0xed, 0xee, 0xef, 240, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 250, 0xfb, 
                0xfc, 0xfd, 0xfe, 0xff
             };
            int index = 0;
            int num2 = 0;
            int num3 = 0;
            special_string.Clear();
            if ((((bufout.b[ct] == 0x26) && (bufout.b[ct + 1] == 110)) && ((bufout.b[ct + 2] == 0x62) && (bufout.b[ct + 3] == 0x73))) && ((bufout.b[ct + 4] == 0x70) && (bufout.b[ct + 5] == 0x3b)))
            {
                special_string.Clear();
                special_string.Add(" ");
                return 5;
            }
            if ((((bufout.b[ct] == 0x26) && (bufout.b[ct + 1] == 110)) && ((bufout.b[ct + 2] == 0x62) && (bufout.b[ct + 3] == 0x73))) && (bufout.b[ct + 4] == 0x70))
            {
                special_string.Clear();
                special_string.Add(" ");
                return 4;
            }
            if ((((bufout.b[ct] == 0x26) && (bufout.b[ct + 1] == 0x61)) && ((bufout.b[ct + 2] == 0x6d) && (bufout.b[ct + 3] == 0x70))) && (bufout.b[ct + 4] == 0x3b))
            {
                special_string.Clear();
                special_string.Add("&");
                return 4;
            }
            if (((bufout.b[ct] == 0x26) && (bufout.b[ct + 1] == 0x6c)) && ((bufout.b[ct + 2] == 0x74) && (bufout.b[ct + 3] == 0x3b)))
            {
                special_string.Clear();
                special_string.Add("<");
                return 3;
            }
            if (((bufout.b[ct] == 0x26) && (bufout.b[ct + 1] == 0x67)) && ((bufout.b[ct + 2] == 0x74) && (bufout.b[ct + 3] == 0x3b)))
            {
                special_string.Clear();
                special_string.Add(">");
                return 3;
            }
            if ((((bufout.b[ct] == 0x26) && (bufout.b[ct + 1] == 0x71)) && ((bufout.b[ct + 2] == 0x75) && (bufout.b[ct + 3] == 0x6f))) && ((bufout.b[ct + 4] == 0x74) && (((bufout.b[ct + 5] == 0x3b) || (bufout.b[ct + 5] == 0x20)) || (bufout.b[ct + 5] == 60))))
            {
                special_string.Clear();
                special_string.Add("\"");
                num2 = 5;
                if ((bufout.b[ct + 5] != 0x20) && (bufout.b[ct + 5] != 60))
                {
                    return num2;
                }
                return 4;
            }
            if ((((bufout.b[ct] == 0x26) && (bufout.b[ct + 1] == 0x72)) && ((bufout.b[ct + 2] == 0x73) && (bufout.b[ct + 3] == 0x71))) && (((bufout.b[ct + 4] == 0x75) && (bufout.b[ct + 5] == 0x6f)) && (((bufout.b[ct + 6] == 0x3b) || (bufout.b[ct + 6] == 0x20)) || (bufout.b[ct + 6] == 60))))
            {
                special_string.Clear();
                special_string.Add("'");
                num2 = 6;
                if ((bufout.b[ct + 6] != 0x20) && (bufout.b[ct + 6] != 60))
                {
                    return num2;
                }
                return 5;
            }
            if ((((bufout.b[ct] == 0x26) && (bufout.b[ct + 1] == 0x6c)) && ((bufout.b[ct + 2] == 0x73) && (bufout.b[ct + 3] == 0x71))) && (((bufout.b[ct + 4] == 0x75) && (bufout.b[ct + 5] == 0x6f)) && (((bufout.b[ct + 6] == 0x3b) || (bufout.b[ct + 6] == 0x20)) || (bufout.b[ct + 6] == 60))))
            {
                special_string.Clear();
                special_string.Add("'");
                num2 = 6;
                if ((bufout.b[ct + 6] != 0x20) && (bufout.b[ct + 6] != 60))
                {
                    return num2;
                }
                return 5;
            }
            if ((((bufout.b[ct] == 0x26) && (bufout.b[ct + 1] == 0x72)) && ((bufout.b[ct + 2] == 100) && (bufout.b[ct + 3] == 0x71))) && (((bufout.b[ct + 4] == 0x75) && (bufout.b[ct + 5] == 0x6f)) && (((bufout.b[ct + 6] == 0x3b) || (bufout.b[ct + 6] == 0x20)) || (bufout.b[ct + 6] == 60))))
            {
                special_string.Clear();
                special_string.Add("\"");
                num2 = 6;
                if ((bufout.b[ct + 6] != 0x20) && (bufout.b[ct + 6] != 60))
                {
                    return num2;
                }
                return 5;
            }
            if ((((bufout.b[ct] == 0x26) && (bufout.b[ct + 1] == 0x6c)) && ((bufout.b[ct + 2] == 100) && (bufout.b[ct + 3] == 0x71))) && (((bufout.b[ct + 4] == 0x75) && (bufout.b[ct + 5] == 0x6f)) && (((bufout.b[ct + 6] == 0x3b) || (bufout.b[ct + 6] == 0x20)) || (bufout.b[ct + 6] == 60))))
            {
                special_string.Clear();
                special_string.Add("\"");
                num2 = 6;
                if ((bufout.b[ct + 6] != 0x20) && (bufout.b[ct + 6] != 60))
                {
                    return num2;
                }
                return 5;
            }
            if (bufout.b[ct + 1] == 0x23)
            {
                if (bufout.b[ct + 2] == 120)
                {
                    num2 = 3;
                    index = 0;
                    while (((bufout.b[ct + num2] != 0x3b) && (bufout.b[ct + num2] != 0x20)) && ((index < 10) && ((ct + num2) < max)))
                    {
                        special_string.Add(bufout.b[ct + num2]);
                        num2++;
                        index++;
                    }
                    if (index == 10)
                    {
                        special_string.Clear();
                        special_string.Add("&");
                        return 1;
                    }
                    num3 = this.hex_to_dec(special_string);
                }
                else
                {
                    num2 = 2;
                    index = 0;
                    while (((bufout.b[ct + num2] != 0x3b) && (bufout.b[ct + num2] != 0x20)) && ((index < 10) && ((ct + num2) < max)))
                    {
                        special_string.Add(bufout.b[ct + num2]);
                        num2++;
                        index++;
                    }
                    if (index == 10)
                    {
                        special_string.Clear();
                        special_string.Add("&");
                        return 1;
                    }
                    num3 = this.ToInt(special_string);
                }
                if ((num3 >= 0x21) && (num3 <= 0xff))
                {
                    special_string.Clear();
                    special_string.Add((byte)num3);
                    return num2;
                }
                if (num3 == 0x20)
                {
                    special_string.Clear();
                    special_string.Add(" ");
                }
                return num2;
            }
            num2 = 1;
            index = 0;
            while (((bufout.b[ct + num2] != 0x3b) && (bufout.b[ct + num2] != 0x20)) && ((index < 10) && ((ct + num2) < max)))
            {
                special_string.Add(bufout.b[ct + num2]);
                num2++;
                index++;
            }
            if (index == 10)
            {
                special_string.Clear();
                special_string.Add("&");
                return 1;
            }
            num3 = 0;
            for (index = 0; index < numArray.Length; index++)
            {
                if (special_string.byteCmp(strArray[index]) == 0)
                {
                    num3 = numArray[index];
                    break;
                }
            }
            if (num3 != 0)
            {
                special_string.Clear();
                special_string.Add((byte)num3);
            }
            return num2;
        }

        private bool table_alloc_carts(nest_tables nested_table)
        {
            nested_table.table_array = this.table_allocate(nested_table.table_p.rows, nested_table.table_p.cols + 2);
            nested_table.table_colspan = this.table_allocate(nested_table.table_p.rows, nested_table.table_p.cols + 2);
            nested_table.table_rowspan = this.table_allocate(nested_table.table_p.rows, nested_table.table_p.cols + 2);
            nested_table.table_images = this.table_allocate(nested_table.table_p.rows, nested_table.table_p.cols + 2);
            nested_table.table_symbols = this.table_allocate(nested_table.table_p.rows, nested_table.table_p.cols + 2);
            nested_table.table_width = this.table_allocate(nested_table.table_p.rows, nested_table.table_p.cols + 2);
            nested_table.table_colbg = this.table_allocate(nested_table.table_p.rows, nested_table.table_p.cols + 2);
            nested_table.table_valign = this.table_allocate(nested_table.table_p.rows, nested_table.table_p.cols + 2);
            nested_table.table_map = this.table_allocate(nested_table.table_p.rows, nested_table.table_p.cols + 2);
            return true;
        }

        private int[,] table_allocate(int x, int y)
        {
            return new int[x, y];
        }

        private void table_analyse(ByteClass buf, int posStartBuf, int max, int[,] table_array, int[,] table_symbols, int[,] table_map, int[,] table_colspan, int[,] table_rowspan, int[,] table_images, int[,] table_width, int[,] table_colbg, int[,] table_valign, CSS_params CSS_param, table_params table_p, ArrayList color_list, int color_list_num, ByteClass img_folder, int TBLEN_MAX, bool hieroglyph, byte[] tdAlignColgroup)
        {
            int num;
            int num2;
            int num11;
            int num16;
            int num17;
            int num18;
            int[,] numArray8;
            IntPtr ptr;
            bool flag = true;
            int index = posStartBuf;
            int percent = 0;
            int num5 = 0;
            int num6 = 0;
            int colspan = 0;
            int rowspan = 0;
            int num9 = 0;
            int num10 = 0;
            int num12 = TBLEN_MAX;
            int num13 = 0;
            int num14 = 0;
            int num15 = 0;
            int[] numArray = new int[2];
            ByteClass class2 = new ByteClass(0x20);
            float num19 = 1f;
            int rows = 1;
            int[] numArray2 = new int[this.MAX_COLUMNS];
            int[] numArray3 = new int[this.MAX_COLUMNS];
            int[] numArray4 = new int[this.MAX_COLUMNS];
            int[] numArray5 = new int[this.MAX_COLUMNS];
            bool flag2 = false;
            bool flag3 = false;
            int bgcolor = 0;
            int valign = 0;
            int[] numArray6 = null;
            bool flag4 = false;
            if (numArray6 != null)
            {
                flag4 = true;
            }
            int[,] numArray7 = new int[table_p.rows, table_p.cols + 2];
            bool flag5 = false;
            bool flag6 = false;
            bool flag7 = false;
            int num23 = 0;
            if (numArray7 != null)
            {
                flag5 = true;
            }
            if (flag5)
            {
                for (num = 0; num < table_p.rows; num++)
                {
                    for (num2 = 0; num2 < (table_p.cols + 2); num2++)
                    {
                        numArray7[num, num2] = 0;
                    }
                }
            }
            table_p.table_width = this.get_width(buf, posStartBuf, max, ref percent, ref colspan, ref rowspan, color_list, color_list_num, ref bgcolor, CSS_param, ref valign, ref table_p.tableAlign);
            table_p.percent_width = percent;
            num = 0;
            num2 = 0;
            for (num11 = 0; num11 < this.MAX_COLUMNS; num11++)
            {
                tdAlignColgroup[num11] = 0;
            }
            this.table_clear(table_array, table_p, 0);
            this.table_clear(table_symbols, table_p, 0);
            this.table_clear(table_colspan, table_p, 1);
            this.table_clear(table_rowspan, table_p, 1);
            this.table_clear(table_images, table_p, 0);
            this.table_clear(table_width, table_p, 0);
            this.table_clear(table_colbg, table_p, 0);
            this.table_clear(table_valign, table_p, this.VALIGN_TOP);
            this.table_clear(table_map, table_p, 0);
            index++;
            while (index < max)
            {
                if ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && ((buf.b[index + 3] == 0x61) || (buf.b[index + 3] == 0x41))) && (((buf.b[index + 4] == 0x62) || (buf.b[index + 4] == 0x42)) && ((buf.b[index + 5] == 0x6c) || (buf.b[index + 5] == 0x4c)))) && (((buf.b[index + 6] == 0x65) || (buf.b[index + 6] == 0x45)) && (buf.b[index + 7] == 0x3e)))
                {
                    if (num2 > 0)
                    {
                        table_array[num, 0] = num2;
                        table_colspan[num, 0] = num2;
                        table_rowspan[num, 0] = num2;
                        table_symbols[num, 0] = num2;
                        table_width[num, 0] = num2;
                        table_images[num, 0] = num2;
                        table_map[num, 0] = num2;
                    }
                    break;
                }
                if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x61) || (buf.b[index + 2] == 0x41))) && (((buf.b[index + 3] == 0x62) || (buf.b[index + 3] == 0x42)) && ((buf.b[index + 4] == 0x6c) || (buf.b[index + 4] == 0x4c)))) && (((buf.b[index + 5] == 0x65) || (buf.b[index + 5] == 0x45)) && ((buf.b[index + 6] == 0x3e) || this.IS_DELIMITER(buf.b[index + 6]))))
                {
                    IntPtr ptr2;
                    index += 6;
                    rows++;
                    if (flag5)
                    {
                        (numArray8 = numArray7)[(int)(ptr = (IntPtr)num), (int)(ptr2 = (IntPtr)num2)] = numArray8[(int)ptr, (int)ptr2] + 1;
                    }
                    if (this.IS_DELIMITER(buf.b[index]))
                    {
                        int align = -1111;
                        num13 = this.get_width(buf, index, max, ref percent, ref colspan, ref rowspan, color_list, color_list_num, ref bgcolor, CSS_param, ref valign, ref align);
                        if (num2 <= 0)
                        {
                            num2 = 1;
                        }
                        if (percent == 0)
                        {
                            num13 = this.table_translate_width(num13);
                            if (table_array[num, num2] < num13)
                            {
                                table_array[num, num2] = num13;
                            }
                        }
                        else
                        {
                            num13 = this.table_translate(num13, table_p, percent);
                            if (table_array[num, num2] < num13)
                            {
                                table_array[num, num2] = num13;
                            }
                        }
                    }
                    while ((buf.b[index] != 0x3e) && (index < max))
                    {
                        index++;
                    }
                    if (buf.b[index] == 0x3e)
                    {
                        index++;
                    }
                    num14 = 0;
                    num15 = 0;
                    while ((rows > 1) && (index < max))
                    {
                        if ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && ((buf.b[index + 3] == 0x61) || (buf.b[index + 3] == 0x41))) && (((buf.b[index + 4] == 0x62) || (buf.b[index + 4] == 0x42)) && ((buf.b[index + 5] == 0x6c) || (buf.b[index + 5] == 0x4c)))) && (((buf.b[index + 6] == 0x65) || (buf.b[index + 6] == 0x45)) && (buf.b[index + 7] == 0x3e)))
                        {
                            rows--;
                            index += 6;
                        }
                        else if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x61) || (buf.b[index + 2] == 0x41))) && (((buf.b[index + 3] == 0x62) || (buf.b[index + 3] == 0x42)) && ((buf.b[index + 4] == 0x6c) || (buf.b[index + 4] == 0x4c)))) && (((buf.b[index + 5] == 0x65) || (buf.b[index + 5] == 0x45)) && ((buf.b[index + 6] == 0x3e) || this.IS_DELIMITER(buf.b[index + 6]))))
                        {
                            rows++;
                            if (flag5)
                            {
                                (numArray8 = numArray7)[(int)(ptr = (IntPtr)num), (int)(ptr2 = (IntPtr)num2)] = numArray8[(int)ptr, (int)ptr2] + 1;
                            }
                            if (this.IS_DELIMITER(buf.b[index + 6]))
                            {
                                int num25 = -1111;
                                num13 = this.get_width(buf, index, max, ref percent, ref colspan, ref rowspan, color_list, color_list_num, ref bgcolor, CSS_param, ref valign, ref num25);
                                if (percent == 0)
                                {
                                    num13 = this.table_translate_width(num13);
                                    if (table_array[num, num2] < num13)
                                    {
                                        table_array[num, num2] = num13;
                                    }
                                }
                                else
                                {
                                    num13 = this.table_translate(num13, table_p, percent);
                                    if (table_array[num, num2] < num13)
                                    {
                                        table_array[num, num2] = num13;
                                    }
                                }
                            }
                        }
                        else if ((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && (((buf.b[index + 2] == 100) || (buf.b[index + 2] == 0x44)) || ((buf.b[index + 2] == 0x68) || (buf.b[index + 2] == 0x48)))) && ((buf.b[index + 3] == 0x3e) || this.IS_DELIMITER(buf.b[index + 3])))
                        {
                            num14 = 0;
                            while ((buf.b[index] != 0x3e) && (index < max))
                            {
                                index++;
                            }
                        }
                        else if (((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && (((buf.b[index + 3] == 100) || (buf.b[index + 3] == 0x44)) || ((buf.b[index + 3] == 0x68) || (buf.b[index + 3] == 0x48)))) && (buf.b[index + 4] == 0x3e))
                        {
                            if (num15 < num14)
                            {
                                num15 = num14;
                            }
                            num14 = 0;
                            while ((buf.b[index] != 0x3e) && (index < max))
                            {
                                index++;
                            }
                        }
                        else if ((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x62) || (buf.b[index + 1] == 0x42))) && ((buf.b[index + 2] == 0x72) || (buf.b[index + 2] == 0x52))) && ((buf.b[index + 3] == 0x3e) || this.IS_DELIMITER(buf.b[index + 3])))
                        {
                            if (num14 > num15)
                            {
                                num15 = num14;
                            }
                            num14 = 0;
                            while ((buf.b[index] != 0x3e) && (index < max))
                            {
                                index++;
                            }
                        }
                        else if (buf.b[index] == 60)
                        {
                            while ((buf.b[index] != 0x3e) && (index < max))
                            {
                                index++;
                            }
                        }
                        else if (buf.b[index] == 0x26)
                        {
                            index += this.special_symbols_rtf(buf, index, max, class2, false);
                        }
                        else if (this.IS_DELIMITER(buf.b[index]))
                        {
                            if (num14 > num15)
                            {
                                num15 = num14;
                            }
                            num14 = 0;
                        }
                        else if (buf.b[index] >= 0x20)
                        {
                            num14++;
                        }
                        index++;
                    }
                    if (num6 < num15)
                    {
                        num6 = num15;
                    }
                    if (num6 < num14)
                    {
                        num6 = num14;
                    }
                }
                else if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x68) || (buf.b[index + 2] == 0x48))) && (((buf.b[index + 3] == 0x65) || (buf.b[index + 3] == 0x45)) && ((buf.b[index + 4] == 0x61) || (buf.b[index + 4] == 0x41)))) && (((buf.b[index + 5] == 100) || (buf.b[index + 5] == 0x44)) && (buf.b[index + 6] == 0x3e)))
                {
                    if (flag3)
                    {
                        if (num2 == 0)
                        {
                            num2 = 1;
                        }
                        table_array[num, 0] = num2;
                        table_colspan[num, 0] = num2;
                        table_rowspan[num, 0] = num2;
                        table_symbols[num, 0] = num2;
                        table_width[num, 0] = num2;
                        table_images[num, 0] = num2;
                        table_map[num, 0] = num2;
                        if (flag5)
                        {
                            numArray7[num, 0] = num2;
                        }
                        num++;
                        num2 = 0;
                    }
                    flag3 = true;
                    flag6 = true;
                }
                else if ((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x72) || (buf.b[index + 2] == 0x52))) && ((buf.b[index + 3] == 0x3e) || this.IS_DELIMITER(buf.b[index + 3])))
                {
                    if (flag6 && (num2 == 0))
                    {
                        flag6 = false;
                        flag3 = true;
                    }
                    else
                    {
                        if (flag3 || (num2 == table_p.cols))
                        {
                            if (num2 == 0)
                            {
                                num2 = 1;
                            }
                            table_array[num, 0] = num2;
                            table_colspan[num, 0] = num2;
                            table_rowspan[num, 0] = num2;
                            table_symbols[num, 0] = num2;
                            table_width[num, 0] = num2;
                            table_images[num, 0] = num2;
                            table_map[num, 0] = num2;
                            if (flag5)
                            {
                                numArray7[num, 0] = num2;
                            }
                            num++;
                            num2 = 0;
                        }
                        flag3 = true;
                        flag6 = false;
                    }
                }
                else if (((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && ((buf.b[index + 3] == 0x72) || (buf.b[index + 3] == 0x52))) && (buf.b[index + 4] == 0x3e))
                {
                    if (flag3 || (num2 == table_p.cols))
                    {
                        table_array[num, 0] = num2;
                        table_colspan[num, 0] = num2;
                        table_rowspan[num, 0] = num2;
                        table_symbols[num, 0] = num2;
                        table_width[num, 0] = num2;
                        table_colbg[num, 0] = num2;
                        table_valign[num, 0] = num2;
                        table_images[num, 0] = num2;
                        table_map[num, 0] = num2;
                        if (flag5)
                        {
                            numArray7[num, 0] = num2;
                        }
                        num++;
                        num2 = 0;
                        while ((buf.b[index] != 0x3e) && (index < max))
                        {
                            index++;
                        }
                    }
                    else if (!flag3 && (num2 > 0))
                    {
                        table_array[num, 0] = num2;
                        table_colspan[num, 0] = num2;
                        table_rowspan[num, 0] = num2;
                        table_symbols[num, 0] = num2;
                        table_width[num, 0] = num2;
                        table_colbg[num, 0] = num2;
                        table_valign[num, 0] = num2;
                        table_images[num, 0] = num2;
                        table_map[num, 0] = num2;
                        if (flag5)
                        {
                            numArray7[num, 0] = num2;
                        }
                        num++;
                        num2 = 0;
                        while ((buf.b[index] != 0x3e) && (index < max))
                        {
                            index++;
                        }
                    }
                    flag3 = false;
                    if (num == table_p.rows)
                    {
                        break;
                    }
                }
                else if ((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && (((buf.b[index + 2] == 100) || (buf.b[index + 2] == 0x44)) || ((buf.b[index + 2] == 0x68) || (buf.b[index + 2] == 0x48)))) && ((buf.b[index + 3] == 0x3e) || this.IS_DELIMITER(buf.b[index + 3])))
                {
                    num2++;
                    if (this.IS_DELIMITER(buf.b[index + 3]))
                    {
                        index += 3;
                        int num26 = -1111;
                        table_array[num, num2] = this.get_width(buf, index, max, ref percent, ref colspan, ref rowspan, color_list, color_list_num, ref bgcolor, CSS_param, ref valign, ref num26);
                        table_colspan[num, num2] = colspan;
                        table_rowspan[num, num2] = rowspan;
                        table_colbg[num, num2] = bgcolor;
                        table_valign[num, num2] = valign;
                        table_array[num, num2] = this.table_translate(table_array[num, num2], table_p, percent);
                    }
                    num5 = 0;
                    num6 = 0;
                    while ((buf.b[index] != 0x3e) && (index < max))
                    {
                        index++;
                    }
                    if (flag)
                    {
                        table_images[num, num2] = this.get_width_img(buf, index, max, img_folder);
                    }
                }
                else if (((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && (((buf.b[index + 3] == 100) || (buf.b[index + 3] == 0x44)) || ((buf.b[index + 3] == 0x68) || (buf.b[index + 3] == 0x48)))) && (buf.b[index + 4] == 0x3e))
                {
                    if (num6 > num5)
                    {
                        num5 = num6;
                    }
                    table_symbols[num, num2] = num5;
                    num5 = 0;
                    num6 = 0;
                    while ((buf.b[index] != 0x3e) && (index < max))
                    {
                        index++;
                    }
                }
                else if ((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x62) || (buf.b[index + 1] == 0x42))) && ((buf.b[index + 2] == 0x72) || (buf.b[index + 2] == 0x52))) && ((buf.b[index + 3] == 0x3e) || this.IS_DELIMITER(buf.b[index + 3])))
                {
                    if (num5 > num6)
                    {
                        num6 = num5;
                    }
                    num5 = 0;
                    while ((buf.b[index] != 0x3e) && (index < max))
                    {
                        index++;
                    }
                }
                else if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x63) || (buf.b[index + 1] == 0x43))) && ((buf.b[index + 2] == 0x6f) || (buf.b[index + 2] == 0x4f))) && (((buf.b[index + 3] == 0x6c) || (buf.b[index + 3] == 0x4c)) && ((buf.b[index + 4] == 0x67) || (buf.b[index + 4] == 0x47)))) && ((((buf.b[index + 5] == 0x72) || (buf.b[index + 5] == 0x52)) && ((buf.b[index + 6] == 0x6f) || (buf.b[index + 6] == 0x4f))) && ((((buf.b[index + 7] == 0x75) || (buf.b[index + 7] == 0x55)) && ((buf.b[index + 8] == 0x70) || (buf.b[index + 8] == 80))) && (buf.b[index + 9] == 0x3e))))
                {
                    flag7 = true;
                }
                else if (((((num23 < (this.MAX_COLUMNS - 1)) && (buf.b[index] == 60)) && ((buf.b[index + 1] == 0x63) || (buf.b[index + 1] == 0x43))) && ((buf.b[index + 2] == 0x6f) || (buf.b[index + 2] == 0x4f))) && (((buf.b[index + 3] == 0x6c) || (buf.b[index + 3] == 0x4c)) && ((buf.b[index + 4] == 0x20) && flag7)))
                {
                    index += 3;
                    while ((buf.b[index] != 0x3e) && (index < max))
                    {
                        if (buf.b[index] == 60)
                        {
                            index--;
                            break;
                        }
                        if (((((buf.b[index] == 0x63) || (buf.b[index] == 0x43)) && ((buf.b[index + 1] == 0x65) || (buf.b[index + 1] == 0x45))) && (((buf.b[index + 2] == 110) || (buf.b[index + 2] == 0x4e)) && ((buf.b[index + 3] == 0x74) || (buf.b[index + 3] == 0x54)))) && (((buf.b[index + 4] == 0x65) || (buf.b[index + 4] == 0x45)) && ((buf.b[index + 5] == 0x72) || (buf.b[index + 5] == 0x52))))
                        {
                            tdAlignColgroup[num23++] = 0x63;
                        }
                        if (((((buf.b[index] == 0x6d) || (buf.b[index] == 0x4d)) && ((buf.b[index + 1] == 0x69) || (buf.b[index + 1] == 0x49))) && (((buf.b[index + 2] == 100) || (buf.b[index + 2] == 0x44)) && ((buf.b[index + 3] == 100) || (buf.b[index + 3] == 0x44)))) && (((buf.b[index + 4] == 0x6c) || (buf.b[index + 4] == 0x4c)) && ((buf.b[index + 5] == 0x65) || (buf.b[index + 5] == 0x45))))
                        {
                            tdAlignColgroup[num23++] = 0x63;
                        }
                        if ((((buf.b[index] == 0x6c) || (buf.b[index] == 0x4c)) && ((buf.b[index + 1] == 0x65) || (buf.b[index + 1] == 0x45))) && (((buf.b[index + 2] == 0x66) || (buf.b[index + 2] == 70)) && ((buf.b[index + 3] == 0x74) || (buf.b[index + 3] == 0x54))))
                        {
                            tdAlignColgroup[num23++] = 0x6c;
                        }
                        if (((((buf.b[index] == 0x72) || (buf.b[index] == 0x52)) && ((buf.b[index + 1] == 0x69) || (buf.b[index + 1] == 0x49))) && (((buf.b[index + 2] == 0x67) || (buf.b[index + 2] == 0x47)) && ((buf.b[index + 3] == 0x68) || (buf.b[index + 3] == 0x48)))) && ((buf.b[index + 4] == 0x74) || (buf.b[index + 4] == 0x54)))
                        {
                            tdAlignColgroup[num23++] = 0x72;
                        }
                        if (((((buf.b[index] == 0x6a) || (buf.b[index] == 0x4a)) && ((buf.b[index + 1] == 0x75) || (buf.b[index + 1] == 0x55))) && (((buf.b[index + 2] == 0x73) || (buf.b[index + 2] == 0x53)) && ((buf.b[index + 3] == 0x74) || (buf.b[index + 3] == 0x54)))) && ((((buf.b[index + 4] == 0x69) || (buf.b[index + 4] == 0x49)) && ((buf.b[index + 5] == 0x66) || (buf.b[index + 5] == 70))) && ((buf.b[index + 6] == 0x79) || (buf.b[index + 6] == 0x59))))
                        {
                            tdAlignColgroup[num23++] = 0x6a;
                        }
                        index++;
                    }
                }
                else if ((buf.b[index] == 60) || (buf.b[index - 1] == 60))
                {
                    while ((buf.b[index] != 0x3e) && (index < max))
                    {
                        index++;
                    }
                }
                else if (buf.b[index] == 0x26)
                {
                    index += this.special_symbols_rtf(buf, index, max, class2, false);
                    num5++;
                }
                else if (this.IS_DELIMITER(buf.b[index]))
                {
                    if (num5 > num6)
                    {
                        num6 = num5;
                    }
                    num5 = 0;
                }
                else if ((buf.b[index] >= 0x20) && ((buf.b[index] <= 0x80) || !hieroglyph))
                {
                    num5++;
                }
                index++;
            }
            num10 = 0;
            int num27 = 0;
            int num28 = 0;
            int num29 = 0;
            int num30 = 0;
            int num31 = 0;
            int num32 = 0;
            int num33 = 0;
            int num34 = 0;
            int num35 = 0;
            int num36 = 0;
            int num37 = 0;
            int num38 = 0;
            for (num = 0; num < table_p.rows; num++)
            {
                for (num2 = 1; num2 < (table_colspan[num, 0] + 1); num2++)
                {
                    if (table_rowspan[num, num2] > 1)
                    {
                        rowspan = table_rowspan[num, num2];
                        for (num17 = 1; (num17 < rowspan) && ((num + num17) < table_p.rows); num17++)
                        {
                            (numArray8 = table_colspan)[(int)(ptr = (IntPtr)(num + num17)), 0] = numArray8[(int)ptr, 0] + 1;
                            num18 = num2;
                            colspan = table_colspan[num, num2];
                            while (num18 < (table_colspan[num + num17, 0] + 1))
                            {
                                num10 = table_colspan[num + num17, num18];
                                table_colspan[num + num17, num18] = colspan;
                                colspan = num10;
                                num18++;
                            }
                        }
                    }
                }
            }
            for (num = 0; num < this.MAX_COLUMNS; num++)
            {
                numArray2[num] = 0;
            }
            for (num = 0; num < table_p.rows; num++)
            {
                if (numArray2[table_colspan[num, 0]] > 0)
                {
                    num2 = numArray2[table_colspan[num, 0]] - 1;
                    for (index = 1; index < (table_p.cols + 1); index++)
                    {
                        table_colspan[num, index] = table_colspan[num2, index];
                    }
                }
                else
                {
                    num16 = 0;
                    for (num2 = 1; num2 < (table_p.cols + 1); num2++)
                    {
                        num16 += table_colspan[num, num2];
                        table_colspan[num, num2] = num16;
                    }
                }
            }
            for (num = 0; num < table_p.rows; num++)
            {
                for (num2 = 1; num2 < (table_colspan[num, 0] + 1); num2++)
                {
                    if (table_rowspan[num, num2] > 1)
                    {
                        rowspan = table_rowspan[num, num2];
                        table_map[num, num2] = this.CLVMGF;
                        for (num17 = 1; (num17 < rowspan) && ((num + num17) < table_p.rows); num17++)
                        {
                            table_rowspan[num + num17, 0] = table_colspan[num + num17, 0];
                            table_map[num + num17, 0] = table_colspan[num + num17, 0];
                            table_images[num + num17, 0] = table_colspan[num + num17, 0];
                            table_array[num + num17, 0] = table_colspan[num + num17, 0];
                            table_symbols[num + num17, 0] = table_colspan[num + num17, 0];
                            table_width[num + num17, 0] = table_colspan[num + num17, 0];
                            table_colbg[num + num17, 0] = table_colspan[num + num17, 0];
                            table_valign[num + num17, 0] = table_colspan[num + num17, 0];
                            if (flag5)
                            {
                                numArray7[num + num17, 0] = table_colspan[num + num17, 0];
                                num33 = numArray7[num, num2];
                            }
                            num16 = table_colspan[num, num2];
                            num18 = num2;
                            while (num18 < (table_colspan[num + num17, 0] + 1))
                            {
                                if (num16 == table_colspan[num + num17, num18])
                                {
                                    break;
                                }
                                num18++;
                            }
                            colspan = table_colspan[num, num2];
                            num27 = table_array[num, num2];
                            num29 = table_symbols[num, num2];
                            num31 = table_images[num, num2];
                            num35 = table_colbg[num, num2];
                            num37 = table_valign[num, num2];
                            while (num18 < (table_colspan[num + num17, 0] + 1))
                            {
                                num28 = table_array[num + num17, num18];
                                table_array[num + num17, num18] = num27;
                                num27 = num28;
                                num30 = table_symbols[num + num17, num18];
                                table_symbols[num + num17, num18] = num29;
                                num29 = num30;
                                num32 = table_images[num + num17, num18];
                                table_images[num + num17, num18] = num31;
                                num31 = num32;
                                num36 = table_colbg[num + num17, num18];
                                table_colbg[num + num17, num18] = num35;
                                num35 = num36;
                                num38 = table_valign[num + num17, num18];
                                table_valign[num + num17, num18] = num37;
                                num37 = num38;
                                if (flag5)
                                {
                                    num34 = numArray7[num + num17, num18];
                                    numArray7[num + num17, num18] = num33;
                                    num33 = num34;
                                }
                                num18++;
                            }
                        }
                    }
                }
            }
            for (num = 0; num < table_p.rows; num++)
            {
                if (table_array[num, 0] == 0)
                {
                    table_array[num, 0] = 1;
                    table_colspan[num, 0] = 1;
                    table_rowspan[num, 0] = 1;
                    table_symbols[num, 0] = 1;
                    table_width[num, 0] = 1;
                    table_images[num, 0] = 1;
                    table_colbg[num, 0] = 1;
                    table_valign[num, 0] = 1;
                    table_map[num, 0] = 1;
                }
            }
            for (num = 0; num < table_p.rows; num++)
            {
                for (num2 = 1; num2 < (table_rowspan[num, 0] + 1); num2++)
                {
                    if (table_rowspan[num, num2] > 1)
                    {
                        rows = table_rowspan[num, num2];
                        if ((table_p.rows - rows) <= 0)
                        {
                            rows = table_p.rows;
                        }
                        if (table_p.rows != 1)
                        {
                            num16 = table_colspan[num, num2];
                            if (table_map[num, num2] != this.CLVMRG)
                            {
                                table_map[num, num2] = this.CLVMGF;
                            }
                            for (num17 = 1; (num17 < rows) && ((num + num17) < table_p.rows); num17++)
                            {
                                for (num18 = 1; num18 < (table_p.cols + 1); num18++)
                                {
                                    if (num16 == table_colspan[num + num17, num18])
                                    {
                                        table_map[num + num17, num18] = this.CLVMRG;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (num = 0; num < this.MAX_COLUMNS; num++)
            {
                numArray2[num] = 0;
                numArray3[num] = 0;
                numArray4[num] = 0;
                numArray5[num] = 0;
            }
            for (num = 0; num < table_p.rows; num++)
            {
                colspan = 0;
                for (num2 = 1; num2 < (table_array[num, 0] + 1); num2++)
                {
                    if (table_array[num, num2] != 0)
                    {
                        break;
                    }
                    colspan++;
                }
                if (table_array[num, 0] == colspan)
                {
                    numArray3[table_array[num, 0]] = 1;
                }
            }
            for (num = 0; num < table_p.rows; num++)
            {
                for (num2 = 1; num2 < (table_p.cols + 1); num2++)
                {
                    if (table_array[num, num2] == 0)
                    {
                        table_width[num, num2] = 1;
                    }
                    table_rowspan[num, num2] = table_map[num, num2];
                    table_map[num, num2] = 0;
                }
            }
            this.table_sort(table_array, table_p);
            this.table_sort(table_symbols, table_p);
            this.TableSpecifyCols(table_array, table_rowspan, table_colspan, table_p);
            for (num = 0; num < this.MAX_COLUMNS; num++)
            {
                numArray2[num] = 0;
            }
            for (num = 0; num < table_p.rows; num++)
            {
                if (numArray2[table_symbols[num, 0]] > 0)
                {
                    num2 = numArray2[table_symbols[num, 0]] - 1;
                    for (index = 1; index < (table_p.cols + 1); index++)
                    {
                        table_symbols[num, index] = table_symbols[num2, index];
                    }
                }
                else
                {
                    for (num2 = 1; num2 < (table_colspan[num, 0] + 1); num2++)
                    {
                        num16 = table_colspan[num, num2];
                        num12 = table_symbols[num, num2];
                        num17 = 0;
                        num10 = 0;
                        while (num17 < table_p.rows)
                        {
                            for (num18 = 1; num18 < (table_colspan[num17, 0] + 1); num18++)
                            {
                                if (num16 == table_colspan[num17, num18])
                                {
                                    num10++;
                                    if (num12 <= table_symbols[num17, num18])
                                    {
                                        num12 = table_symbols[num17, num18];
                                    }
                                }
                            }
                            num17++;
                        }
                        if (num10 > 0)
                        {
                            for (num17 = num; num17 < table_p.rows; num17++)
                            {
                                for (num18 = 1; num18 < (table_colspan[num17, 0] + 1); num18++)
                                {
                                    if ((num16 == table_colspan[num17, num18]) && (table_symbols[num17, num18] == 0))
                                    {
                                        table_symbols[num17, num18] = num12;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            this.table_check_width(table_array, table_symbols, table_images, numArray7, table_colspan, table_p, numArray3, numArray6, flag4);
            for (num = 0; num < this.MAX_COLUMNS; num++)
            {
                numArray2[num] = 0;
            }
            for (num = 0; num < table_p.rows; num++)
            {
                if (numArray2[table_map[num, 0]] > 0)
                {
                    num2 = numArray2[table_map[num, 0]] - 1;
                    for (index = 1; index < (table_p.cols + 1); index++)
                    {
                        table_map[num, index] = table_map[num2, index];
                    }
                }
                else
                {
                    index = 1;
                    num5 = 0;
                    while (index < (table_symbols[num, 0] + 1))
                    {
                        num5 += table_symbols[num, index];
                        index++;
                    }
                    if (num5 > this.MAX_SYMBOLS_IN_COLUMN)
                    {
                        num5 = this.MAX_SYMBOLS_IN_COLUMN;
                    }
                    num5 = this.TBLEN / this.TWIPS_PER_ONE_SYMBOL;
                    for (index = 1; index < (table_symbols[num, 0] + 1); index++)
                    {
                        if (num5 != 0)
                        {
                            if (table_symbols[num, index] == 1)
                            {
                                table_map[num, index] = (table_symbols[num, index] * TBLEN_MAX) / num5;
                            }
                            else if (table_symbols[num, index] == 0)
                            {
                                table_map[num, index] = 10;
                            }
                            else if ((table_symbols[num, index] >= 2) && (table_symbols[num, index] <= 3))
                            {
                                table_map[num, index] = (table_symbols[num, index] * TBLEN_MAX) / num5;
                            }
                            else if ((table_symbols[num, index] >= 4) && (table_symbols[num, index] <= 10))
                            {
                                table_map[num, index] = (table_symbols[num, index] * TBLEN_MAX) / num5;
                            }
                            else if ((table_symbols[num, index] >= 11) && (table_symbols[num, index] <= 0x19))
                            {
                                table_map[num, index] = (table_symbols[num, index] * TBLEN_MAX) / num5;
                            }
                            else if ((table_symbols[num, index] > 0x19) && (table_symbols[num, index] <= 50))
                            {
                                table_map[num, index] = (0x19 * TBLEN_MAX) / num5;
                            }
                            else if ((table_symbols[num, index] > 50) && (table_symbols[num, index] <= 100))
                            {
                                table_map[num, index] = (0x33 * TBLEN_MAX) / num5;
                            }
                            else if ((table_symbols[num, index] > 100) && (table_symbols[num, index] <= 0xc7))
                            {
                                table_map[num, index] = (70 * TBLEN_MAX) / num5;
                            }
                            else if (table_symbols[num, index] > 200)
                            {
                                table_map[num, index] = (this.MAX_SYMBOLS_IN_COLUMN * TBLEN_MAX) / num5;
                            }
                            if (table_map[num, index] > this.TBLEN)
                            {
                                table_map[num, index] = this.TBLEN;
                            }
                        }
                    }
                    for (index = 1; index < (table_p.cols + 1); index++)
                    {
                        if (table_width[num, index] == 0)
                        {
                            if (table_map[num, index] < table_array[num, index])
                            {
                                table_map[num, index] = table_array[num, index];
                            }
                        }
                        else if (table_array[num, index] != 0)
                        {
                            table_map[num, index] = table_array[num, index];
                        }
                    }
                }
            }
            for (num = 0; num < table_p.rows; num++)
            {
                for (num2 = 0; num2 < (table_p.cols + 1); num2++)
                {
                    table_array[num, num2] = table_colspan[num, num2];
                }
            }
            for (num = 0; num < this.MAX_COLUMNS; num++)
            {
                numArray2[num] = 0;
            }
            if (flag)
            {
                for (num = 0; num < table_p.rows; num++)
                {
                    if (numArray2[table_map[num, 0]] > 0)
                    {
                        num2 = numArray2[table_map[num, 0]] - 1;
                        for (index = 1; index < (table_p.cols + 1); index++)
                        {
                            table_map[num, index] = table_map[num2, index];
                        }
                    }
                    else
                    {
                        num10 = 0;
                        for (num11 = 1; num11 < (table_images[num, 0] + 1); num11++)
                        {
                            if (table_images[num, num11] > 0)
                            {
                                num10 = 1;
                                break;
                            }
                        }
                        if (num10 > 0)
                        {
                            for (num2 = 1; num2 < (table_images[num, 0] + 1); num2++)
                            {
                                if (table_map[num, num2] < table_images[num, num2])
                                {
                                    table_map[num, num2] = table_images[num, num2];
                                }
                                if ((table_images[num, num2] > 0) && (table_images[num, num2] > table_map[num, num2]))
                                {
                                    numArray5[num2] = 1;
                                }
                            }
                        }
                        if (num10 == 0)
                        {
                            num2 = 1;
                            num5 = 0;
                            num12 = 0;
                            while (num2 < (table_map[num, 0] + 1))
                            {
                                num5 += table_map[num, num2];
                                if (table_width[num, num2] == 1)
                                {
                                    num12 += table_map[num, num2];
                                }
                                num2++;
                            }
                            num10 = 0;
                            if ((num12 < num5) && (num12 > 0))
                            {
                                if (TBLEN_MAX > (num5 - num12))
                                {
                                    num19 = ((float)num12) / ((float)(TBLEN_MAX - (num5 - num12)));
                                    num10 = 1;
                                }
                                else
                                {
                                    num19 = ((float)num5) / ((float)TBLEN_MAX);
                                }
                            }
                            else
                            {
                                num19 = ((float)num5) / ((float)TBLEN_MAX);
                            }
                            for (num2 = 1; num2 < (table_map[num, 0] + 1); num2++)
                            {
                                if (num10 == 1)
                                {
                                    if (table_width[num, num2] == 1)
                                    {
                                        table_map[num, num2] = (int)(((float)table_map[num, num2]) / num19);
                                    }
                                }
                                else
                                {
                                    table_map[num, num2] = (int)(((float)table_map[num, num2]) / num19);
                                }
                            }
                        }
                        else
                        {
                            num5 = 0;
                            num9 = 0;
                            for (num2 = 1; num2 < (table_map[num, 0] + 1); num2++)
                            {
                                num5 += table_map[num, num2];
                                if (numArray5[num2] == 1)
                                {
                                    num9 += table_map[num, num2];
                                }
                            }
                            if (num9 > TBLEN_MAX)
                            {
                                num9 = TBLEN_MAX - 100;
                            }
                            if (num5 != num9)
                            {
                                num19 = ((float)(num5 - num9)) / ((float)(TBLEN_MAX - num9));
                            }
                            else
                            {
                                num19 = ((float)num9) / ((float)TBLEN_MAX);
                            }
                            for (num2 = 1; num2 < (table_map[num, 0] + 1); num2++)
                            {
                                if ((numArray5[num2] != 1) || (num19 < 1f))
                                {
                                    table_map[num, num2] = (int)(((float)table_map[num, num2]) / num19);
                                }
                            }
                            num2 = 1;
                            num5 = 0;
                            while (num2 < (table_map[num, 0] + 1))
                            {
                                num5 += table_map[num, num2];
                                num2++;
                            }
                            if ((num5 < (TBLEN_MAX - 100)) || (num5 > (TBLEN_MAX + 1)))
                            {
                                num19 = ((float)num5) / ((float)TBLEN_MAX);
                                for (num2 = 1; num2 < (table_map[num, 0] + 1); num2++)
                                {
                                    table_map[num, num2] = (int)(((float)table_map[num, num2]) / num19);
                                }
                            }
                            numArray2[table_map[num, 0]] = num + 1;
                            for (num2 = 0; num2 < table_p.cols; num2++)
                            {
                                numArray5[num2] = 0;
                            }
                        }
                    }
                }
            }
            else
            {
                for (num = 0; num < table_p.rows; num++)
                {
                    if (numArray2[table_map[num, 0]] > 0)
                    {
                        num2 = numArray2[table_map[num, 0]] - 1;
                        for (index = 1; index < (table_p.cols + 1); index++)
                        {
                            table_map[num, index] = table_map[num2, index];
                        }
                    }
                    else
                    {
                        num2 = 1;
                        num5 = 0;
                        while (num2 < (table_map[num, 0] + 1))
                        {
                            num5 += table_map[num, num2];
                            num2++;
                        }
                        num19 = ((float)num5) / ((float)TBLEN_MAX);
                        for (num2 = 1; num2 < (table_map[num, 0] + 1); num2++)
                        {
                            table_map[num, num2] = (int)(((float)table_map[num, num2]) / num19);
                        }
                        numArray2[table_map[num, 0]] = num + 1;
                    }
                }
            }
            for (num = 0; num < this.MAX_COLUMNS; num++)
            {
                numArray2[num] = 0;
            }
            for (num = 0; num < table_p.rows; num++)
            {
                if (numArray2[table_map[num, 0]] > 0)
                {
                    num2 = numArray2[table_map[num, 0]] - 1;
                    for (index = 1; index < (table_p.cols + 1); index++)
                    {
                        table_map[num, index] = table_map[num2, index];
                    }
                }
                else
                {
                    num2 = table_map[num, 0];
                    num5 = 0;
                    while (num2 >= 1)
                    {
                        colspan = table_map[num, num2];
                        table_map[num, num2] = TBLEN_MAX - num5;
                        num5 += colspan;
                        num2--;
                    }
                }
            }
            for (num = 0; num < table_p.rows; num++)
            {
                for (num2 = 1; num2 < (table_p.cols + 1); num2++)
                {
                    table_colspan[num, num2] = 0;
                    table_symbols[num, num2] = table_map[num, num2];
                }
            }
            for (num = 0; num < table_p.rows; num++)
            {
                table_map[num, table_map[num, 0]] = TBLEN_MAX;
            }
            for (num = 0; num < this.MAX_COLUMNS; num++)
            {
                numArray2[num] = 0;
            }
            for (num = 0; num < table_p.rows; num++)
            {
                if (numArray2[table_map[num, 0]] > 0)
                {
                    num2 = numArray2[table_map[num, 0]] - 1;
                    for (index = 1; index < (table_p.cols + 1); index++)
                    {
                        table_map[num, index] = table_map[num2, index];
                    }
                }
                else
                {
                    for (num2 = 1; num2 < (table_array[num, 0] + 1); num2++)
                    {
                        num16 = table_array[num, num2];
                        if (((num16 != 1) || (table_map[num, 0] != 1)) && (((num2 != 1) || (num16 <= 1)) || ((table_width[num, num2] != 1) || (table_map[num, 0] != 1))))
                        {
                            num12 = table_map[num, num2];
                            flag2 = false;
                            if (table_width[num, num2] == 1)
                            {
                                flag2 = true;
                            }
                            num17 = 0;
                            num10 = 0;
                            while (num17 < table_p.rows)
                            {
                                for (num18 = 1; num18 < (table_array[num17, 0] + 1); num18++)
                                {
                                    if ((num16 == table_array[num17, num18]) && ((num16 != 1) || (table_map[num17, 0] != 1)))
                                    {
                                        num10++;
                                        if ((num12 <= table_map[num17, num18]) && (table_map[num17, num18] != TBLEN_MAX))
                                        {
                                            if ((numArray3[table_map[num17, 0]] == 0) || (table_width[num17, num18] == 0))
                                            {
                                                num12 = table_map[num17, num18];
                                            }
                                        }
                                        else if (num12 > table_map[num17, num18])
                                        {
                                            if (flag2 && (table_width[num17, num18] == 0))
                                            {
                                                num12 = table_map[num17, num18];
                                                flag2 = false;
                                            }
                                            else if (numArray4[num17 + 1] == 1)
                                            {
                                                num12 = table_map[num17, num18];
                                            }
                                        }
                                    }
                                }
                                num17++;
                            }
                            if (num10 > 0)
                            {
                                for (num17 = num; num17 < table_p.rows; num17++)
                                {
                                    for (num18 = 1; num18 < (table_array[num17, 0] + 1); num18++)
                                    {
                                        if (num16 == table_array[num17, num18])
                                        {
                                            if (num10 > 1)
                                            {
                                                table_colspan[num17, num18] = 1;
                                            }
                                            table_map[num17, num18] = num12;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (num = 0; num < this.MAX_COLUMNS; num++)
            {
                numArray2[num] = 0;
            }
            for (num = 0; num < table_p.rows; num++)
            {
                if (numArray2[table_map[num, 0]] > 0)
                {
                    num2 = numArray2[table_map[num, 0]] - 1;
                    for (index = 1; index < (table_p.cols + 1); index++)
                    {
                        table_map[num, index] = table_map[num2, index];
                    }
                }
                else
                {
                    for (num2 = 1; num2 < table_map[num, 0]; num2++)
                    {
                        numArray[0] = table_map[num, num2];
                        numArray[1] = table_map[num, num2 + 1];
                        if ((numArray[1] < numArray[0]) && ((num2 + 1) < table_map[num, 0]))
                        {
                            table_map[num, num2 + 1] = (table_map[num, num2] + table_map[num, num2 + 2]) / 2;
                        }
                    }
                }
            }
            for (num = 0; num < table_p.rows; num++)
            {
                table_map[num, table_map[num, 0]] = TBLEN_MAX;
            }
            if ((this._preserveTableWidth == 1) && (table_p.table_width > 0))
            {
                num12 = TBLEN_MAX;
                if (table_p.percent_width == 0)
                {
                    num12 = (table_p.table_width * TBLEN_MAX) / this.SCREEN_W_DEF;
                }
                else if (table_p.table_width < 100)
                {
                    num12 = (table_p.table_width * TBLEN_MAX) / 100;
                }
                if ((num12 < (TBLEN_MAX - 500)) && (num12 > 0x3e8))
                {
                    num19 = ((float)num12) / ((float)TBLEN_MAX);
                    for (num = 0; num < table_p.rows; num++)
                    {
                        for (num2 = 1; num2 < (table_map[num, 0] + 1); num2++)
                        {
                            table_map[num, num2] = (int)(table_map[num, num2] * num19);
                        }
                    }
                }
            }
        }

        private void table_check_width(int[,] table_array, int[,] table_symbols, int[,] table_images, int[,] nested_level, int[,] table_colspan, table_params table_p, int[] row_nulls, int[] rows_empty, bool use_rows_empty)
        {
            int num;
            int num5;
            int num9 = 1;
            int num10 = 1;
            for (num = 0; num < table_p.rows; num++)
            {
                int num3;
                int num7 = 0;
                num5 = 0;
                for (num3 = 1; num3 < (table_array[num, 0] + 1); num3++)
                {
                    if ((table_array[num, num3] >= this.TBLEN) && (table_array[num, 0] != 1))
                    {
                        num7++;
                    }
                    else
                    {
                        num5 += table_array[num, num3];
                    }
                }
                if (num7 > 0)
                {
                    int num8 = (this.TBLEN - num5) / num7;
                    if (num8 <= 0)
                    {
                        num8 = 1;
                    }
                    for (num3 = 1; num3 < (table_array[num, 0] + 1); num3++)
                    {
                        if ((table_array[num, num3] >= this.TBLEN) && (table_array[num, 0] != 1))
                        {
                            table_array[num, num3] = num8;
                        }
                    }
                }
            }
            for (int i = 0; i < table_p.rows; i++)
            {
                int num4 = 0;
                num5 = 0;
                for (num = 1; num < (table_array[i, 0] + 1); num++)
                {
                    if (table_array[i, num] == 0)
                    {
                        num4++;
                    }
                    num5 += table_array[i, num];
                }
                if (use_rows_empty)
                {
                    rows_empty[i] = 0;
                    rows_empty[i] = num4;
                }
                if (num4 == table_array[i, 0])
                {
                    row_nulls[table_array[i, 0]] = 1;
                }
                if (num4 > 0)
                {
                    int num6;
                    if (num5 < this.TBLEN)
                    {
                        num6 = (this.TBLEN - num5) / num4;
                    }
                    else
                    {
                        num6 = (num5 - this.TBLEN) / num4;
                    }
                    for (num = 1; num < (table_array[i, 0] + 1); num++)
                    {
                        if (table_array[i, num] == 0)
                        {
                            num9 = table_colspan[i, num];
                            if (num > 1)
                            {
                                num9 = table_colspan[i, num] - table_colspan[i, num - 1];
                            }
                            num10 = this.TBLEN / table_p.cols;
                            num10 *= num9;
                            if (((table_images[i, num] == 0) && (table_symbols[i, num] == 0)) && (table_array[i, num] == 0))
                            {
                                table_array[i, num] = 1;
                            }
                            else if ((table_images[i, num] > 0) && (table_symbols[i, num] == 0))
                            {
                                table_array[i, num] = table_images[i, num];
                            }
                            else if (table_images[i, num] > 0)
                            {
                                if (num10 < table_images[i, num])
                                {
                                    table_array[i, num] = table_images[i, num];
                                }
                                else
                                {
                                    table_array[i, num] = num10;
                                }
                            }
                        }
                    }
                    num4 = 0;
                    num9 = 1;
                    num5 = 0;
                    for (num = 1; num < (table_array[i, 0] + 1); num++)
                    {
                        if (table_array[i, num] == 0)
                        {
                            num4++;
                            if (num > 1)
                            {
                                num9 += table_colspan[i, num] - table_colspan[i, num - 1];
                            }
                        }
                        num5 += table_array[i, num];
                    }
                    if (num4 > 0)
                    {
                        if (num5 < this.TBLEN)
                        {
                            num6 = (this.TBLEN - num5) / num4;
                            num10 = (this.TBLEN - num5) / num9;
                        }
                        else
                        {
                            num6 = (num5 - this.TBLEN) / num4;
                            num10 = (num5 - this.TBLEN) / num9;
                        }
                        for (num = 1; num < (table_array[i, 0] + 1); num++)
                        {
                            if (table_array[i, num] == 0)
                            {
                                num9 = table_colspan[i, num];
                                if (num > 1)
                                {
                                    num9 = table_colspan[i, num] - table_colspan[i, num - 1];
                                }
                                num6 = num10 * num9;
                                table_array[i, num] = num6;
                            }
                        }
                    }
                }
            }
        }

        private void table_clear(int[,] table_array, table_params table_p, int num)
        {
            for (int i = 0; i < table_p.rows; i++)
            {
                for (int j = 0; j < (table_p.cols + 1); j++)
                {
                    table_array[i, j] = num;
                }
            }
        }

        private void table_clear_carts(nest_tables nested_table)
        {
            if (nested_table.table_array != null)
            {
                nested_table.table_array = null;
            }
            if (nested_table.table_colspan != null)
            {
                nested_table.table_colspan = null;
            }
            if (nested_table.table_rowspan != null)
            {
                nested_table.table_rowspan = null;
            }
            if (nested_table.table_images != null)
            {
                nested_table.table_images = null;
            }
            if (nested_table.table_symbols != null)
            {
                nested_table.table_symbols = null;
            }
            if (nested_table.table_width != null)
            {
                nested_table.table_width = null;
            }
            if (nested_table.table_map != null)
            {
                nested_table.table_map = null;
            }
            if (nested_table.table_colbg != null)
            {
                nested_table.table_colbg = null;
            }
            if (nested_table.table_valign != null)
            {
                nested_table.table_valign = null;
            }
        }

        private void table_free_carts(nest_tables nested_table)
        {
            if (nested_table.table_array != null)
            {
                nested_table.table_array = null;
            }
            if (nested_table.table_colspan != null)
            {
                nested_table.table_colspan = null;
            }
            if (nested_table.table_images != null)
            {
                nested_table.table_images = null;
            }
            if (nested_table.table_symbols != null)
            {
                nested_table.table_symbols = null;
            }
            if (nested_table.table_width != null)
            {
                nested_table.table_width = null;
            }
        }

        private table_types table_getsize(ByteClass buf, int posStartBuf, int max, table_params table_p)
        {
            table_types _types = table_types.TABLE_UNCLOSED;
            int index = posStartBuf;
            int num2 = 0;
            table_p.rows = 0;
            table_p.cols = 0;
            int num3 = 1;
            int percent = 0;
            int colspan = 0;
            int rowspan = 0;
            int num7 = 0;
            bool flag = false;
            bool flag2 = false;
            index++;
            while (index < max)
            {
                if ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && ((buf.b[index + 3] == 0x61) || (buf.b[index + 3] == 0x41))) && (((buf.b[index + 4] == 0x62) || (buf.b[index + 4] == 0x42)) && ((buf.b[index + 5] == 0x6c) || (buf.b[index + 5] == 0x4c)))) && (((buf.b[index + 6] == 0x65) || (buf.b[index + 6] == 0x45)) && (buf.b[index + 7] == 0x3e)))
                {
                    _types = table_types.TABLE_NORMAL;
                    break;
                }
                if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x61) || (buf.b[index + 2] == 0x41))) && (((buf.b[index + 3] == 0x62) || (buf.b[index + 3] == 0x42)) && ((buf.b[index + 4] == 0x6c) || (buf.b[index + 4] == 0x4c)))) && (((buf.b[index + 5] == 0x65) || (buf.b[index + 5] == 0x45)) && ((buf.b[index + 6] == 0x3e) || this.IS_DELIMITER(buf.b[index + 6]))))
                {
                    index += 6;
                    num3++;
                    while ((num3 > 1) && (index < max))
                    {
                        if ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && ((buf.b[index + 3] == 0x61) || (buf.b[index + 3] == 0x41))) && (((buf.b[index + 4] == 0x62) || (buf.b[index + 4] == 0x42)) && ((buf.b[index + 5] == 0x6c) || (buf.b[index + 5] == 0x4c)))) && (((buf.b[index + 6] == 0x65) || (buf.b[index + 6] == 0x45)) && (buf.b[index + 7] == 0x3e)))
                        {
                            num3--;
                            index += 6;
                        }
                        else if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x61) || (buf.b[index + 2] == 0x41))) && (((buf.b[index + 3] == 0x62) || (buf.b[index + 3] == 0x42)) && ((buf.b[index + 4] == 0x6c) || (buf.b[index + 4] == 0x4c)))) && (((buf.b[index + 5] == 0x65) || (buf.b[index + 5] == 0x45)) && ((buf.b[index + 6] == 0x3e) || this.IS_DELIMITER(buf.b[index + 6]))))
                        {
                            num3++;
                        }
                        index++;
                    }
                }
                if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x68) || (buf.b[index + 2] == 0x48))) && (((buf.b[index + 3] == 0x65) || (buf.b[index + 3] == 0x45)) && ((buf.b[index + 4] == 0x61) || (buf.b[index + 4] == 0x41)))) && (((buf.b[index + 5] == 100) || (buf.b[index + 5] == 0x44)) && (buf.b[index + 6] == 0x3e)))
                {
                    table_p.rows++;
                    if (num2 < (table_p.cols + num7))
                    {
                        num2 = table_p.cols + num7;
                    }
                    table_p.cols = 0;
                    num7 = 0;
                    flag = true;
                    index += 6;
                }
                else if ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && ((buf.b[index + 3] == 0x68) || (buf.b[index + 3] == 0x48))) && (((buf.b[index + 4] == 0x65) || (buf.b[index + 4] == 0x45)) && ((buf.b[index + 5] == 0x61) || (buf.b[index + 5] == 0x41)))) && (((buf.b[index + 6] == 100) || (buf.b[index + 6] == 0x44)) && (buf.b[index + 7] == 0x3e)))
                {
                    index += 7;
                    flag = false;
                }
                else
                {
                    if ((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x72) || (buf.b[index + 2] == 0x52))) && ((buf.b[index + 3] == 0x3e) || this.IS_DELIMITER(buf.b[index + 3])))
                    {
                        if (flag && (table_p.cols == 0))
                        {
                            flag2 = true;
                            flag = false;
                            goto Label_077A;
                        }
                        table_p.rows++;
                        if (num2 < (table_p.cols + num7))
                        {
                            num2 = table_p.cols + num7;
                        }
                        table_p.cols = 0;
                        num7 = 0;
                        flag = false;
                        flag2 = true;
                    }
                    if (((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && ((buf.b[index + 3] == 0x72) || (buf.b[index + 3] == 0x52))) && (buf.b[index + 4] == 0x3e))
                    {
                        index += 4;
                        if (!flag2 && (table_p.cols > 0))
                        {
                            table_p.rows++;
                            if (num2 < (table_p.cols + num7))
                            {
                                num2 = table_p.cols + num7;
                            }
                            table_p.cols = 0;
                            num7 = 0;
                        }
                        flag2 = false;
                    }
                    else
                    {
                        if ((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && (((buf.b[index + 2] == 100) || (buf.b[index + 2] == 0x44)) || ((buf.b[index + 2] == 0x68) || (buf.b[index + 2] == 0x48)))) && ((buf.b[index + 3] == 0x3e) || this.IS_DELIMITER(buf.b[index + 3])))
                        {
                            colspan = 0;
                            int num8 = -1111;
                            this.get_width(buf, index, max, ref percent, ref colspan, ref rowspan, null, num8, ref num8, null, ref num8, ref num8);
                            if (colspan > 1)
                            {
                                num7 += colspan - 1;
                            }
                            table_p.cols++;
                        }
                        if (buf.b[index] == 60)
                        {
                            while ((index < max) && (buf.b[index] != 0x3e))
                            {
                                index++;
                            }
                        }
                    }
                }
            Label_077A:
                index++;
            }
            if (num2 > table_p.cols)
            {
                table_p.cols = num2;
            }
            return _types;
        }

        private string table_make_cellx(int[,] table_array, int tr, int td)
        {
            if (table_array[tr, td + 1] > this.TBLEN)
            {
                table_array[tr, td + 1] = this.TBLEN;
            }
            return table_array[tr, td + 1].ToString();
        }

        private void table_skip_table(ByteClass buf, ref int count, int max)
        {
            int index = count + 1;
            int num2 = 1;
            while ((buf.b[index] != 0x3e) && (index < buf.len))
            {
                index++;
            }
            count = index;
            while (index < max)
            {
                if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x61) || (buf.b[index + 2] == 0x41))) && (((buf.b[index + 3] == 0x62) || (buf.b[index + 3] == 0x42)) && ((buf.b[index + 4] == 0x6c) || (buf.b[index + 4] == 0x4c)))) && (((buf.b[index + 5] == 0x65) || (buf.b[index + 5] == 0x45)) && ((buf.b[index + 6] == 0x3e) || this.IS_DELIMITER(buf.b[index + 6]))))
                {
                    index += 6;
                    num2++;
                    while ((num2 > 1) && (index < max))
                    {
                        if ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && ((buf.b[index + 3] == 0x61) || (buf.b[index + 3] == 0x41))) && (((buf.b[index + 4] == 0x62) || (buf.b[index + 4] == 0x42)) && ((buf.b[index + 5] == 0x6c) || (buf.b[index + 5] == 0x4c)))) && (((buf.b[index + 6] == 0x65) || (buf.b[index + 6] == 0x45)) && (buf.b[index + 7] == 0x3e)))
                        {
                            num2--;
                            index += 6;
                        }
                        else if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x61) || (buf.b[index + 2] == 0x41))) && (((buf.b[index + 3] == 0x62) || (buf.b[index + 3] == 0x42)) && ((buf.b[index + 4] == 0x6c) || (buf.b[index + 4] == 0x4c)))) && (((buf.b[index + 5] == 0x65) || (buf.b[index + 5] == 0x45)) && ((buf.b[index + 6] == 0x3e) || this.IS_DELIMITER(buf.b[index + 6]))))
                        {
                            num2++;
                        }
                        index++;
                    }
                }
                if ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && ((buf.b[index + 3] == 0x61) || (buf.b[index + 3] == 0x41))) && (((buf.b[index + 4] == 0x62) || (buf.b[index + 4] == 0x42)) && ((buf.b[index + 5] == 0x6c) || (buf.b[index + 5] == 0x4c)))) && (((buf.b[index + 6] == 0x65) || (buf.b[index + 6] == 0x45)) && (buf.b[index + 7] == 0x3e)))
                {
                    buf.b[index] = 0x20;
                    buf.b[index + 1] = 0x20;
                    buf.b[index + 2] = 0x20;
                    buf.b[index + 3] = 0x20;
                    buf.b[index + 4] = 0x20;
                    buf.b[index + 5] = 0x20;
                    buf.b[index + 6] = 0x20;
                    buf.b[index + 7] = 0x20;
                    return;
                }
                index++;
            }
        }

        private void table_skip_table2(ByteClass buf, ref int count, int max)
        {
            int index = count;
            int num2 = 1;
            while ((buf.b[index] != 0x3e) && (index < max))
            {
                index++;
            }
            count = index;
            while (index < max)
            {
                if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x72) || (buf.b[index + 2] == 0x52))) && (((buf.b[index + 3] == 0x3e) || (buf.b[index + 3] == 0x20)) || (buf.b[index + 3] == 10))) || (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x68) || (buf.b[index + 2] == 0x48))) && (((buf.b[index + 3] == 0x65) || (buf.b[index + 3] == 0x45)) && ((buf.b[index + 4] == 0x61) || (buf.b[index + 4] == 0x41)))) && (((buf.b[index + 5] == 100) || (buf.b[index + 5] == 0x44)) && ((buf.b[index + 6] == 0x3e) || this.IS_DELIMITER(buf.b[index + 6])))))
                {
                    index++;
                    while ((buf.b[index + 1] != 0x3e) && (index < max))
                    {
                        buf.b[index++] = 0x20;
                    }
                }
                else if ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && ((buf.b[index + 3] == 0x72) || (buf.b[index + 3] == 0x52))) && (buf.b[index + 4] == 0x3e)) || ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && ((buf.b[index + 3] == 0x68) || (buf.b[index + 3] == 0x48))) && (((buf.b[index + 4] == 0x65) || (buf.b[index + 4] == 0x45)) && ((buf.b[index + 5] == 0x61) || (buf.b[index + 5] == 0x41)))) && (((buf.b[index + 6] == 100) || (buf.b[index + 6] == 0x44)) && (buf.b[index + 7] == 0x3e))))
                {
                    index++;
                    while ((buf.b[index + 1] != 0x3e) && (index < max))
                    {
                        buf.b[index++] = 0x20;
                    }
                }
                else if ((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && (((buf.b[index + 2] == 100) || (buf.b[index + 2] == 0x44)) || ((buf.b[index + 2] == 0x68) || (buf.b[index + 2] == 0x48)))) && ((buf.b[index + 3] == 0x3e) || this.IS_DELIMITER(buf.b[index + 3])))
                {
                    index++;
                    while ((buf.b[index + 1] != 0x3e) && (index < max))
                    {
                        buf.b[index++] = 0x20;
                    }
                }
                else if (((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && (((buf.b[index + 3] == 100) || (buf.b[index + 3] == 0x44)) || ((buf.b[index + 3] == 0x68) || (buf.b[index + 3] == 0x48)))) && (buf.b[index + 4] == 0x3e))
                {
                    index++;
                    while ((buf.b[index + 1] != 0x3e) && (index < max))
                    {
                        buf.b[index++] = 0x20;
                    }
                }
                else
                {
                    if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x61) || (buf.b[index + 2] == 0x41))) && (((buf.b[index + 3] == 0x62) || (buf.b[index + 3] == 0x42)) && ((buf.b[index + 4] == 0x6c) || (buf.b[index + 4] == 0x4c)))) && (((buf.b[index + 5] == 0x65) || (buf.b[index + 5] == 0x45)) && ((buf.b[index + 6] == 0x3e) || this.IS_DELIMITER(buf.b[index + 6]))))
                    {
                        index += 6;
                        num2++;
                        while ((num2 > 1) && (index < max))
                        {
                            if ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && ((buf.b[index + 3] == 0x61) || (buf.b[index + 3] == 0x41))) && (((buf.b[index + 4] == 0x62) || (buf.b[index + 4] == 0x42)) && ((buf.b[index + 5] == 0x6c) || (buf.b[index + 5] == 0x4c)))) && (((buf.b[index + 6] == 0x65) || (buf.b[index + 6] == 0x45)) && (buf.b[index + 7] == 0x3e)))
                            {
                                num2--;
                                index += 6;
                            }
                            else if (((((buf.b[index] == 60) && ((buf.b[index + 1] == 0x74) || (buf.b[index + 1] == 0x54))) && ((buf.b[index + 2] == 0x61) || (buf.b[index + 2] == 0x41))) && (((buf.b[index + 3] == 0x62) || (buf.b[index + 3] == 0x42)) && ((buf.b[index + 4] == 0x6c) || (buf.b[index + 4] == 0x4c)))) && (((buf.b[index + 5] == 0x65) || (buf.b[index + 5] == 0x45)) && ((buf.b[index + 6] == 0x3e) || this.IS_DELIMITER(buf.b[index + 6]))))
                            {
                                num2++;
                            }
                            index++;
                        }
                    }
                    if ((((((buf.b[index] == 60) && (buf.b[index + 1] == 0x2f)) && ((buf.b[index + 2] == 0x74) || (buf.b[index + 2] == 0x54))) && ((buf.b[index + 3] == 0x61) || (buf.b[index + 3] == 0x41))) && (((buf.b[index + 4] == 0x62) || (buf.b[index + 4] == 0x42)) && ((buf.b[index + 5] == 0x6c) || (buf.b[index + 5] == 0x4c)))) && (((buf.b[index + 6] == 0x65) || (buf.b[index + 6] == 0x45)) && (buf.b[index + 7] == 0x3e)))
                    {
                        buf.b[index + 1] = 0x20;
                        buf.b[index + 2] = 0x20;
                        buf.b[index + 3] = 0x20;
                        buf.b[index + 4] = 0x20;
                        buf.b[index + 5] = 0x20;
                        buf.b[index + 6] = 0x20;
                        return;
                    }
                }
                index++;
            }
        }

        private void table_sort(int[,] table_array, table_params table_p)
        {
            for (int i = 0; i < table_p.rows; i++)
            {
                int num5 = table_array[i, 0];
                for (int j = 1; j < (table_p.cols + 1); j++)
                {
                    int num;
                    int num4 = table_array[i, j];
                    for (num = 0; num < table_p.rows; num++)
                    {
                        if ((num4 < table_array[num, j]) && (table_array[num, 0] == num5))
                        {
                            num4 = table_array[num, j];
                        }
                    }
                    for (num = 0; num < table_p.rows; num++)
                    {
                        if (table_array[num, 0] == num5)
                        {
                            table_array[num, j] = num4;
                        }
                    }
                }
            }
        }

        private int table_translate(int num, table_params table_p, int percent_w)
        {
            int num2 = 0;
            if (table_p.table_width < 1)
            {
                if (table_p.percent_width == 1)
                {
                    table_p.table_width = 100;
                }
                else
                {
                    table_p.table_width = this.SCREEN_W_DEF;
                }
            }
            if (table_p.percent_width == 0)
            {
                if (percent_w != 0)
                {
                    return ((num * this.TBLEN) / 100);
                }
                num2 = (num * this.TBLEN) / table_p.table_width;
                if (num2 > this.TBLEN)
                {
                    num2 = this.TBLEN - 100;
                }
                return num2;
            }
            if (percent_w != 0)
            {
                return ((num * this.TBLEN) / 100);
            }
            return ((num * this.TBLEN) / this.SCREEN_W_DEF);
        }

        private int table_translate_width(int num)
        {
            int tBLEN = 0;
            tBLEN = (num * this.TBLEN) / this.SCREEN_W_DEF;
            if (tBLEN > this.TBLEN)
            {
                tBLEN = this.TBLEN;
            }
            return tBLEN;
        }

        private void TableSpecifyCols(int[,] table_array, int[,] table_rowspan, int[,] table_colspan, table_params table_p)
        {
            int num2;
            int num3;
            bool flag = false;
            int num6 = 1;
            for (num2 = 0; num2 < table_p.rows; num2++)
            {
                for (num3 = 1; num3 < (table_array[num2, 0] + 1); num3++)
                {
                    if (num6 < table_colspan[num2, num3])
                    {
                        num6 = table_colspan[num2, num3];
                    }
                }
            }
            for (int i = 1; i <= num6; i++)
            {
                int num7 = 0;
                num2 = 0;
                flag = false;
                while (num2 < table_p.rows)
                {
                    for (num3 = 1; num3 < (table_array[num2, 0] + 1); num3++)
                    {
                        if (i == table_colspan[num2, num3])
                        {
                            if (num7 < table_array[num2, num3])
                            {
                                if (i == num3)
                                {
                                    num7 = table_array[num2, num3];
                                }
                                else if ((num3 > 1) && (i > 1))
                                {
                                    for (int j = 1; (j < (table_rowspan[num2, num3] + 1)) && ((num2 + j) < table_p.rows); j++)
                                    {
                                        int num;
                                        int num8;
                                        if ((table_colspan[num2, num3] - table_colspan[num2, num3 - 1]) == 1)
                                        {
                                            num8 = table_colspan[num2, num3];
                                        }
                                        else
                                        {
                                            num8 = table_colspan[num2, num3] - table_colspan[num2, num3 - 1];
                                        }
                                        int num9 = i;
                                        bool flag2 = false;
                                        bool flag3 = false;
                                        for (num = 1; num < (table_colspan[num2 + j, 0] + 1); num++)
                                        {
                                            if ((table_colspan[num2 + j, num] == num8) && !flag2)
                                            {
                                                num8 = num;
                                                flag2 = true;
                                            }
                                            if ((table_colspan[num2 + j, num] == num9) && !flag3)
                                            {
                                                num9 = num;
                                                flag3 = true;
                                            }
                                        }
                                        if (flag2 && flag3)
                                        {
                                            int num10 = 0;
                                            int num11 = 0;
                                            for (num = num8; num <= num9; num++)
                                            {
                                                if (table_array[num2 + j, num] == 0)
                                                {
                                                    num10++;
                                                }
                                                num11 += table_array[num2 + j, num];
                                            }
                                            if ((num10 > 0) && ((num9 - num8) >= 0))
                                            {
                                                if (table_array[num2, num3] > num11)
                                                {
                                                    num11 = (table_array[num2, num3] - num11) / num10;
                                                    for (num = num8; num <= num9; num++)
                                                    {
                                                        if (table_array[num2 + j, num] == 0)
                                                        {
                                                            table_array[num2 + j, num] = num11;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if ((num9 - num8) == 0)
                                                    {
                                                        num11 = table_array[num2, num3];
                                                    }
                                                    else
                                                    {
                                                        num11 = table_array[num2, num3] / (num9 - num8);
                                                    }
                                                    for (num = num8; num <= num9; num++)
                                                    {
                                                        table_array[num2 + j, num] = num11;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (table_array[num2, num3] == 0)
                            {
                                flag = true;
                            }
                        }
                    }
                    num2++;
                }
                if ((num7 > 0) && flag)
                {
                    for (num2 = 0; num2 < table_p.rows; num2++)
                    {
                        for (num3 = 1; num3 < (table_array[num2, 0] + 1); num3++)
                        {
                            if ((i == table_colspan[num2, num3]) && (table_array[num2, num3] == 0))
                            {
                                table_array[num2, num3] = num7;
                            }
                        }
                    }
                }
            }
        }

        private void TableTrace(int[,] table, table_params table_p, string message)
        {
            StreamWriter writer = new StreamWriter(@"d:\trace.txt", true);
            writer.WriteLine(message);
            for (int i = 0; i < table_p.rows; i++)
            {
                for (int j = 0; j < (table_p.cols + 1); j++)
                {
                    writer.Write(table[i, j].ToString() + " ");
                }
                writer.WriteLine();
            }
            writer.Close();
        }

        private float ToFloat(ByteClass s)
        {
            if (s.len != 0)
            {
                string str = "";
                int index = 0;
                while (index < s.len)
                {
                    if (((s.b[index] >= 0x30) && (s.b[index] <= 0x39)) || (((s.b[index] == 0x2d) && (s.b[index + 1] >= 0x30)) && (s.b[index + 1] <= 0x39)))
                    {
                        break;
                    }
                    index++;
                }
                while ((((index < s.len) && (s.b[index] >= 0x30)) && (s.b[index] <= 0x39)) || (((s.b[index] == 0x2d) || (s.b[index] == 0x2c)) || (s.b[index] == 0x2e)))
                {
                    if (((s.b[index] == 0x2d) || (s.b[index] == 0x2c)) || (s.b[index] == 0x2e))
                    {
                        str = str + ((char)s.b[index]);
                    }
                    else
                    {
                        str = str + (s.b[index] - 0x30);
                    }
                    index++;
                }
                if (str != "")
                {
                    try
                    {
                        return (float)double.Parse(str.Replace(".", Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));
                    }
                    catch
                    {
                        return 0f;
                    }
                }
            }
            return 0f;
        }

        private void tohex(int num, ByteClass str)
        {
            str.Clear();
            str.Add(@"\'");
            if (num < 0x100)
            {
                str.Add((byte)((num / 0x10) + 0x30));
                if (str.b[2] > 0x39)
                {
                    str.b[2] = (byte)((0x61 + str.b[2]) - 0x3a);
                }
                str.Add((byte)((num % 0x10) + 0x30));
                if (str.b[3] > 0x39)
                {
                    str.b[3] = (byte)((0x61 + str.b[3]) - 0x3a);
                }
            }
        }

        private int ToInt(ByteClass s)
        {
            if (s.len != 0)
            {
                string str = "";
                int index = 0;
                while (index < s.len)
                {
                    if (((s.b[index] >= 0x30) && (s.b[index] <= 0x39)) || (((s.b[index] == 0x2d) && (s.b[index + 1] >= 0x30)) && (s.b[index + 1] <= 0x39)))
                    {
                        break;
                    }
                    index++;
                }
                while ((((index < s.len) && (s.b[index] >= 0x30)) && (s.b[index] <= 0x39)) || (s.b[index] == 0x2d))
                {
                    str = str + (s.b[index] - 0x30);
                    index++;
                }
                if (str != "")
                {
                    return Convert.ToInt32(str, 10);
                }
            }
            return 0;
        }

        private int ToInt(string s)
        {
            ByteClass class2 = new ByteClass();
            class2.Add(s);
            return this.ToInt(class2);
        }

        private void tomyhex(int num, ByteClass str, int pos)
        {
            if (num < 0x100)
            {
                str.b[pos] = (byte)((num / 0x10) + 0x30);
                if (str.b[pos] > 0x39)
                {
                    str.b[pos] = (byte)((0x61 + str.b[pos]) - 0x3a);
                }
                str.b[pos + 1] = (byte)((num % 0x10) + 0x30);
                if (str.b[pos + 1] > 0x39)
                {
                    str.b[pos + 1] = (byte)((0x61 + str.b[pos + 1]) - 0x3a);
                }
                str.b[pos + 2] = 0;
            }
        }

        private void ToSpecSymbol(ByteClass spec_ch, byte b1, byte b2)
        {
            spec_ch.Clear();
            this.tomyhex(b1, spec_ch, 0);
            this.tomyhex(b2, spec_ch, 2);
            spec_ch.Add("&#" + this.hex_to_dec(spec_ch).ToString());
        }

        public string BaseURL
        {
            get
            {
                return this._baseURL;
            }
            set
            {
                this._baseURL = value;
            }
        }

        public eBorderVisibility BorderVisibility
        {
            get
            {
                return this._borderVisibility;
            }
            set
            {
                this._borderVisibility = value;
            }
        }

        public int CreateTraceFile
        {
            get
            {
                return this._createTraceFile;
            }
            set
            {
                this._createTraceFile = value;
            }
        }

        public int DeleteImages
        {
            get
            {
                return this._deleteImages;
            }
            set
            {
                this._deleteImages = value;
            }
        }

        public int DeleteTables
        {
            get
            {
                return this._deleteTables;
            }
            set
            {
                this._deleteTables = value;
            }
        }

        public eEncoding Encoding
        {
            get
            {
                return this._encoding;
            }
            set
            {
                this._encoding = value;
            }
        }

        public eFontFace FontFace
        {
            get
            {
                return this._fontFace;
            }
            set
            {
                this._fontFace = value;
            }
        }

        public int FontSize
        {
            get
            {
                return this._fontSize;
            }
            set
            {
                this._fontSize = value;
            }
        }

        public string HtmlPath
        {
            get
            {
                return this._htmlPath;
            }
            set
            {
                this._htmlPath = value;
            }
        }

        public eImageCompatible ImageCompatible
        {
            get
            {
                return this._imageCompatible;
            }
            set
            {
                this._imageCompatible = value;
            }
        }

        public eOutputTextFormat OutputTextFormat
        {
            get
            {
                return this._outputTextFormat;
            }
            set
            {
                this._outputTextFormat = value;
            }
        }

        public ePageAlignment PageAlignment
        {
            get
            {
                return this._pageAlignment;
            }
            set
            {
                this._pageAlignment = value;
            }
        }

        public string PageFooter
        {
            get
            {
                return this._pageFooter;
            }
            set
            {
                this._pageFooter = value;
            }
        }

        public string PageHeader
        {
            get
            {
                return this._pageHeader;
            }
            set
            {
                this._pageHeader = value;
            }
        }

        public int PageMarginBottom
        {
            get
            {
                return this._pageMarginBottom;
            }
            set
            {
                this._pageMarginBottom = value;
            }
        }

        public int PageMarginLeft
        {
            get
            {
                return this._pageMarginLeft;
            }
            set
            {
                this._pageMarginLeft = value;
            }
        }

        public int PageMarginRight
        {
            get
            {
                return this._pageMarginRight;
            }
            set
            {
                this._pageMarginRight = value;
            }
        }

        public int PageMarginTop
        {
            get
            {
                return this._pageMarginTop;
            }
            set
            {
                this._pageMarginTop = value;
            }
        }

        public ePageNumbers PageNumbers
        {
            get
            {
                return this._pageNumbers;
            }
            set
            {
                this._pageNumbers = value;
            }
        }

        public ePageAlignment PageNumbersAlignH
        {
            get
            {
                return this._pageNumbersAlignH;
            }
            set
            {
                this._pageNumbersAlignH = value;
            }
        }

        public ePageAlignment PageNumbersAlignV
        {
            get
            {
                return this._pageNumbersAlignV;
            }
            set
            {
                this._pageNumbersAlignV = value;
            }
        }

        public ePageOrientation PageOrientation
        {
            get
            {
                return this._pageOrientation;
            }
            set
            {
                this._pageOrientation = value;
            }
        }

        public ePageSize PageSize
        {
            get
            {
                return this._pageSize;
            }
            set
            {
                this._pageSize = value;
            }
        }

        public int PreserveAlignment
        {
            get
            {
                return this._preserveAlignment;
            }
            set
            {
                this._preserveAlignment = value;
            }
        }

        public int PreserveBackgroundColor
        {
            get
            {
                return this._preserveBackgroundColor;
            }
            set
            {
                this._preserveBackgroundColor = value;
            }
        }

        public int PreserveFontColor
        {
            get
            {
                return this._preserveFontColor;
            }
            set
            {
                this._preserveFontColor = value;
            }
        }

        public int PreserveFontFace
        {
            get
            {
                return this._preserveFontFace;
            }
            set
            {
                this._preserveFontFace = value;
            }
        }

        public int PreserveFontSize
        {
            get
            {
                return this._preserveFontSize;
            }
            set
            {
                this._preserveFontSize = value;
            }
        }

        public int PreserveHR
        {
            get
            {
                return this._preserveHR;
            }
            set
            {
                this._preserveHR = value;
            }
        }

        public int PreserveHttpImages
        {
            get
            {
                return this._preserveHttpImages;
            }
            set
            {
                this._preserveHttpImages = value;
            }
        }

        public int PreserveHyperlinks
        {
            get
            {
                return this._preserveHyperlinks;
            }
            set
            {
                this._preserveHyperlinks = value;
            }
        }

        public int PreserveImages
        {
            get
            {
                return this._preserveImages;
            }
            set
            {
                this._preserveImages = value;
            }
        }

        public int PreserveNestedTables
        {
            get
            {
                return this._preserveNestedTables;
            }
            set
            {
                this._preserveNestedTables = value;
            }
        }

        public int PreservePageBreaks
        {
            get
            {
                return this._preservePageBreaks;
            }
            set
            {
                this._preservePageBreaks = value;
            }
        }

        public int PreserveTables
        {
            get
            {
                return this._preserveTables;
            }
            set
            {
                this._preserveTables = value;
            }
        }

        public int PreserveTableWidth
        {
            get
            {
                return this._preserveTableWidth;
            }
            set
            {
                this._preserveTableWidth = value;
            }
        }

        public eRtfLanguage RtfLanguage
        {
            get
            {
                return this._rtfLanguage;
            }
            set
            {
                this._rtfLanguage = value;
            }
        }

        public eRtfParts RtfParts
        {
            get
            {
                return this._rtfParts;
            }
            set
            {
                this._rtfParts = value;
            }
        }

        public string Serial
        {
            get
            {
                return this._serial;
            }
            set
            {
                this._serial = value;
            }
        }

        public int TableCellPadding
        {
            get
            {
                return this._tableCellPadding;
            }
            set
            {
                this._tableCellPadding = value;
            }
        }

        public string TraceFilePath
        {
            get
            {
                return this._traceFilePath;
            }
            set
            {
                this._traceFilePath = value;
            }
        }

        public delegate void BeforeImageDownloadEventHanfler(HttpWebRequest aRequest);
    }
}

