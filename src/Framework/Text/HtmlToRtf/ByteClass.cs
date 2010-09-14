using System;

namespace InfoControl.Text.HtmlToRtf
{
    public class ByteClass
    {
        public byte[] b;
        public int len;
        private int size;

        public ByteClass()
        {
            size = 0x200;
            b = new byte[size];
            len = 0;
        }

        public ByteClass(int part)
        {
            size = part;
            b = new byte[size];
            len = 0;
        }

        public void Add(ByteClass newb)
        {
            int len = newb.len;
            while ((this.len + len) > size)
            {
                Newstr();
            }
            for (int i = 0; i < len; i++)
            {
                b[this.len + i] = newb.b[i];
            }
            this.len += len;
        }

        public void Add(byte newb)
        {
            while ((len + 1) > size)
            {
                Newstr();
            }
            b[len] = newb;
            len++;
        }

        public void Add(string s)
        {
            int length = s.Length;
            while ((len + length) > size)
            {
                Newstr();
            }
            for (int i = 0; i < length; i++)
            {
                b[len + i] = (byte) s[i];
            }
            len += length;
        }

        public void Add(ByteClass newb, int pos)
        {
            int len = newb.len;
            int num3 = 0;
            while ((this.len + len) > size)
            {
                Newstr();
            }
            int index = pos;
            num3 = 0;
            while (index < len)
            {
                b[this.len + num3] = newb.b[index];
                index++;
                num3++;
            }
            this.len += num3;
        }

        public void Add(ByteClass newb, int posStart, int kolByte)
        {
            int len;
            if ((kolByte + posStart) > newb.len)
            {
                len = newb.len;
            }
            else
            {
                len = kolByte + posStart;
            }
            while ((this.len + len) > size)
            {
                Newstr();
            }
            for (int i = posStart; i < len; i++)
            {
                b[this.len + i] = newb.b[i];
            }
            this.len += len;
        }

        public void AddToStart(ByteClass newb)
        {
            int len = newb.len;
            if (len != 0)
            {
                while ((this.len + len) > size)
                {
                    Newstr();
                }
                for (int i = this.len - 1; i >= 0; i--)
                {
                    b[len + i] = b[i];
                }
                for (int j = len - 1; j >= 0; j--)
                {
                    b[j] = newb.b[j];
                }
                this.len += len;
            }
        }

        public void AddToStart(string s)
        {
            int length = s.Length;
            if (length != 0)
            {
                int num2;
                while ((len + length) > size)
                {
                    Newstr();
                }
                for (num2 = len - 1; num2 >= 0; num2--)
                {
                    b[length + num2] = b[num2];
                }
                for (num2 = length - 1; num2 >= 0; num2--)
                {
                    b[num2] = (byte) s[num2];
                }
                len += length;
            }
        }

        public int byteCmp(ByteClass bc)
        {
            if (bc.len != len)
            {
                return -1;
            }
            for (int i = 0; i < bc.len; i++)
            {
                if (char.ToLower((char) b[i]) != char.ToLower((char) bc.b[i]))
                {
                    return -1;
                }
            }
            return 0;
        }

        public int byteCmp(string bc)
        {
            if (bc.Length != len)
            {
                return -1;
            }
            return IndexOfBegin(bc);
        }

        public int byteCmpi(string bc)
        {
            if (bc.Length != len)
            {
                return -1;
            }
            return IndexOfBeginI(bc);
        }

        public byte[] byteEndClear(ByteClass bc)
        {
            var buffer = new byte[bc.len];
            for (int i = 0; i < bc.len; i++)
            {
                buffer[i] = bc.b[i];
            }
            return buffer;
        }

        public int ByteToInt()
        {
            string str = "";
            int index = 0;
            while (index < len)
            {
                if (((b[index] >= 0x30) && (b[index] <= 0x39)) ||
                    (((b[index] == 0x2d) && (b[index + 1] >= 0x30)) && (b[index + 1] <= 0x39)))
                {
                    break;
                }
                index++;
            }
            while ((((index < len) && (b[index] >= 0x30)) && (b[index] <= 0x39)) || (b[index] == 0x2d))
            {
                if (b[index] == 0x2d)
                {
                    str = str + "-";
                }
                else
                {
                    str = str + (b[index] - 0x30);
                }
                index++;
            }
            if (str != "")
            {
                return Convert.ToInt32(str, 10);
            }
            return 0;
        }

        public string ByteToString()
        {
            var chArray = new char[len];
            for (int i = 0; i < len; i++)
            {
                chArray[i] = (char) b[i];
            }
            return new string(chArray);
        }

        public void Clear()
        {
            for (int i = 0; i < len; i++)
            {
                b[i] = 0;
            }
            len = 0;
        }

        public int IndexOf(string s)
        {
            int num = -1;
            int length = s.Length;
            for (int i = 0; (i <= (len - length)) && (num == -1); i++)
            {
                bool flag = true;
                for (int j = 0; j < length; j++)
                {
                    if (char.ToLower(s[j]) != char.ToLower((char) b[i + j]))
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    num = i;
                }
            }
            return num;
        }

        public int IndexOfBegin(string s)
        {
            int num = -1;
            int length = s.Length;
            for (int i = 0; (i <= (len - length)) && (num == -1); i++)
            {
                bool flag = true;
                for (int j = 0; j < length; j++)
                {
                    if (((byte) s[j]) != b[i + j])
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    return i;
                }
            }
            return num;
        }

        public int IndexOfBeginI(string s)
        {
            int num = -1;
            int length = s.Length;
            for (int i = 0; (i <= (len - length)) && (num == -1); i++)
            {
                bool flag = true;
                for (int j = 0; j < length; j++)
                {
                    if (((byte) char.ToLower(s[j])) != ((byte) char.ToLower((char) b[i + j])))
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    return i;
                }
            }
            return num;
        }

        public void Insert(ByteClass newb, int pos)
        {
            int len = newb.len;
            if (len != 0)
            {
                while ((this.len + len) > size)
                {
                    Newstr();
                }
                for (int i = this.len - 1; i >= pos; i--)
                {
                    b[len + i] = b[i];
                }
                for (int j = len - 1; j >= pos; j--)
                {
                    b[j] = newb.b[j];
                }
                this.len += len;
            }
        }

        private void Newstr()
        {
            var buffer = new byte[size*2];
            for (int i = 0; i < size; i++)
            {
                buffer[i] = b[i];
            }
            size += size;
            b = buffer;
        }

        public ByteClass ToByteCStartPos(int startPosition)
        {
            var class2 = new ByteClass(0x80);
            for (int i = startPosition; i < len; i++)
            {
                if (b[i] == 0)
                {
                    return class2;
                }
                class2.Add(b[i]);
            }
            return class2;
        }
    }
}