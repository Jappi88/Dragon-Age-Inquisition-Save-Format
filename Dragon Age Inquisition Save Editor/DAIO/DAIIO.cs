#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Dragon_Age_Inquisition_Save_Editor.SaveData;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.DAIO
{
    public class DAIIO : Stream
    {
        internal readonly Stream xbaseStream;

        private readonly long xbasepos;
        private readonly long xbitlength;
        private readonly bool xpaststream;

        public uint CurrentBit;


        internal uint xshiftseed;

        public DAIIO(byte[] data, bool isbigendian, long start = 0, long length = 0)
        {
            IsBigEndian = isbigendian;
            xbaseStream = new MemoryStream(data);
            var r9 = (int) ((start) & 0xFFFFFFFF & 0xFFFFFFE0);
            var r28 = (int) (start - r9);
            var r5 = (int) ((((r28 + length) >> 3) & 0x1FFFFFFF) & 0x1FFFFFFC);
            OrigBitPos = start;
            if (length > 0)
                xbitlength = r5;
            else xbitlength = (((data.Length) << 3) & 0x7FFFFFF);
            Position = r28;
        }

        public DAIIO(Stream io, bool isbigendian, long start, long length = 0)
        {
            IsBigEndian = isbigendian;
            xbaseStream = io;
            var r9 = (int) ((start) & 0xFFFFFFFF & 0xFFFFFFE0);
            xbasepos = r9;
            var r28 = (int) (start - r9);
            var r5 = (int) (r28 + length);
            OrigBitPos = r28;
            if (length > 0)
                xbitlength = r5;
            else xbitlength = (((io.Length) << 3) & 0x7FFFFFF);
            Position = r28;
            xpaststream = true;
        }

        public DAIIO(bool isbigendian)
        {
            xbaseStream = new MemoryStream();
            IsBigEndian = isbigendian;
        }

        public bool IsBigEndian { get; set; }
        public long OrigBitPos { get; set; }

        public long OrigPosition => (((OrigBitPos >> 3) & 0x1FFFFFFF) & 0x1FFFFFFC);

        public override long Position
        {
            get { return CurrentBit; }
            set
            {
                xbaseStream.Position = ((((value + xbasepos) >> 3) & 0x1FFFFFFF) & 0x1FFFFFFC);
                var j = (int) ((value + xbasepos) & 0xFFFFFFFF & 0x1F);
                CurrentBit = (uint) value;
                xshiftseed = xbaseStream.ReadUInt32(true) << j;
            }
        }

        public override bool CanRead => xbaseStream.CanRead;

        public override bool CanSeek => xbaseStream.CanSeek;

        public override bool CanWrite => xbaseStream.CanWrite;

        public override long Length => (xbitlength == 0 ? OrigBitLength : xbitlength);

        public long OrigBitLength => (((xbaseStream.Length) << 3) & 0x7FFFFFF);

        public long OrigLength => xbaseStream.Length;

        public override int ReadByte()
        {
            return ReadBit(8);
        }

        public short ReadInt16()
        {
            return (short) ReadBit2(0x10);
        }

        public ushort ReadUInt16()
        {
            return (ushort) ReadBit2(0x10);
        }

        public int ReadInt24()
        {
            return ReadBit2(0x18);
        }

        public int ReadInt32()
        {
            return ReadBit2(0x20);
        }

        public uint ReadUInt32()
        {
            return (uint) ReadBit2(0x20);
        }

        public long ReadInt64()
        {
            var x = ReadBytes(8);
            if (IsBigEndian)
                Array.Reverse(x);
            return BitConverter.ToInt64(x, 0);
        }

        public ulong ReadUInt64()
        {
            var x = ReadBytes(8);
            if (IsBigEndian)
                Array.Reverse(x);
            return BitConverter.ToUInt64(x, 0);
        }

        public bool ReadBoolean()
        {
            return Convert.ToBoolean(ReadBit2(0x1));
        }

        public float ReadSingle()
        {
            var buf = new byte[4];
            Read(buf, 0, 4);
            if (IsBigEndian)
                Array.Reverse(buf);
            return BitConverter.ToSingle(buf, 0);
        }

        public string ReadString(int count)
        {
            string x = "";
            for (int i = 0; i < count; i++)
                x += (char) ReadBit(0x8);
            return x;
        }

        public int ReadBit(uint bits)
        {
            uint xval;
            uint r3;
            if ((CurrentBit + bits) > Length)
                return -1;
            uint xcleanbit = (CurrentBit & 0xFFFFFFFF & 0x1F);
            uint xcurflag = (0x20 - xcleanbit);
            if (xcurflag <= bits)
            {
                Position = Position;
                xval = xshiftseed;
                uint xlastbyteflag = bits - xcurflag;
                xcurflag = ((xval >> (int) xcleanbit) << (int) xlastbyteflag);
                xval = xbaseStream.ReadUInt32(true);
                var r8 = (uint) (1 << (int) xlastbyteflag);
                uint r6 = 0x20 - xlastbyteflag;
                r6 = xval >> (int) r6;
                r8--;
                xval <<= (int) xlastbyteflag;
                xlastbyteflag = r6 & r8;
                r3 = xlastbyteflag | xcurflag;
                xshiftseed = xval;
            }
            else
            {
                xval = ((xshiftseed << (int) bits) & 0xFFFFFF00);
                var tmp = BitConverter.GetBytes(xshiftseed);
                    Array.Reverse(tmp);
                r3 = tmp[0];
                xshiftseed = xval;
            }
            CurrentBit += bits;
            return (byte) (r3 & 0xFFFFFFFF & 0xFF);
        }

        public byte[] ReadBytes(int bitlength, bool shiftdata)
        {
            var r9 = (int) ((Position) & 0xFFFFFFFF & 0xFFFFFFE0);
            var xstartpos = (int) ((((Position + xbasepos) >> 3) & 0x1FFFFFFF) & 0x1FFFFFFC);
            var r28 = (int) (Position - r9);
            int r5 = (((r28 + bitlength) >> 3) & 0x1FFFFFFF) & 0x1FFFFFFC;
            xbaseStream.Position = xstartpos;
            //Position = r9;
            var buff = new byte[r5];
            var ps = Position;
            if (shiftdata)
                Read(buff, 0, r5);
            else
                xbaseStream.Read(buff, 0, r5);
            Position = ps + bitlength;
            return buff;
        }

        public byte[] ReadBytes(int length)
        {
            var buff = new byte[length];
            Read(buff, 0, length);
            return buff;
        }

        public byte[] ReadData(int totalbits)
        {
            var x = new List<byte>();
            int left = totalbits;
            while (left >= 0x20)
            {
                int t = ReadBit2(0x20);
                left -= 0x20;
                var tmp = BitConverter.GetBytes(t);
                if (IsBigEndian)
                    Array.Reverse(tmp);
                x.AddRange(tmp);
            }
            while (left >= 8)
            {
                x.Add((byte) ReadBit2(8));
                left -= 8;
            }
            if (left > 0)
                x.Add((byte) ReadBit2((uint) left));
            return x.ToArray();
        }

        public override void Flush()
        {
            xbaseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if ((offset + count) > buffer.Length)
                return -1;
            if ((count + (Position >> 3)) > Length)
                count = (int) ((((Length - Position) >> 3) & 0x1FFFFFFF) & 0x1FFFFFFC);
            int read = 0;
            for (int i = offset; i < (offset + count); i++)
            {
                if (CurrentBit >= Length)
                    break;
                buffer[i] = (byte) ReadByte();
                read++;
            }
            return read;
        }

        public int ReadBit2(uint bits)
        {
            uint xval;
            int r3;
            if ((CurrentBit + bits) > Length)
                return -1;
            uint xcleanbit = (CurrentBit & 0xFFFFFFFF & 0x1F);
            uint xcurflag = (0x20 - xcleanbit);
            if (bits >= xcurflag)
            {
                Position = Position;
                xval = xshiftseed;
                uint xlastbyteflag = bits - xcurflag;
                xcurflag = ((xval >> (int) xcleanbit) << (int) xlastbyteflag);
                xval = xbaseStream.ReadUInt32(true);
                var r8 = (uint) (1 << (int) xlastbyteflag);
                uint r6 = 0x20 - xlastbyteflag;
                r6 = xval >> (int) r6;
                r8--;
                xval <<= (int) xlastbyteflag;
                xlastbyteflag = r6 & r8;
                r3 = (int) (xlastbyteflag | xcurflag);
                xshiftseed = xval;
            }
            else
            {
                xval = ((xshiftseed << (int) bits));
                r3 = (int) ((xshiftseed >> (int) (0x20 - bits)));
                xshiftseed = xval;
            }
            var r11 = (int) (Math.Abs(bits)*(-1));
            r11 = r11 >> 0x1f;
            CurrentBit += bits;
            return (r11 & r3);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return xbaseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            xbaseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if ((offset + count) > buffer.Length)
                throw new Exception("Unable to write buffer! offset + count is larger then buffer length");
            var value = new byte[count];
            Array.Copy(buffer, offset, value, 0, count);
            var r11 = (int) ((count << 3) & 0xFFFFFFF8);
            if (r11 <= 0x20)
            {
                var val = LoadWord(value, offset);
                WriteBits(val, (uint) r11);
            }
            else
            {
                r11 = (int) (CurrentBit + r11);
                var r5 = (int) (CurrentBit & 0x1F);
                int r27 = count & 3;
                int r7 = 0x20 - r5;
                int r8 = r7 - 1;
                var r10 = (int) (count & 0xFFFFFFFC);
                int r9 = 1 << r8;
                r9 <<= 1;
                r8 = r9 - 1;
                int r3 = ~r8;
                r9 = (int) xshiftseed;
                if (CurrentBit < r11)
                {
                    for (int i = 0; i < r10; i += 4)
                    {
                        int r31 = LoadWord(value, i);
                        r9 &= r3;
                        int r30 = r31 >> r5;
                        r30 &= r8;
                        r31 <<= r7;
                        r30 = (r30 | r9);
                        xshiftseed = (uint) (r9 = (r31 & r3));
                        xbaseStream.WriteInt32(r30, true);
                        CurrentBit += 0x20;
                    }
                    if (r27 > 0)
                    {
                        int r6 = r27 << 3;
                        r11 = LoadWord(value, r10);
                        if (r6 > 0x10)
                        {
                            r9 = 0x20 - r6;
                            r11 >>= r9;
                        }
                        WriteBits(r11, (uint) r6);
                    }
                }
            }
        }

        public void WriteInt16(short value)
        {
            var t = BitConverter.GetBytes(value);
            if (IsBigEndian)
                Array.Reverse(t);
            Write(t, 0, 2);
        }

        public void WriteUInt16(ushort value)
        {
            var t = BitConverter.GetBytes(value);
            if (IsBigEndian)
                Array.Reverse(t);
            Write(t, 0, 2);
        }

        public void WriteInt24(int value)
        {
            WriteBits(value, 0x18);
        }

        public void WriteInt32(int value)
        {
            var t = BitConverter.GetBytes(value);
            if (IsBigEndian)
                Array.Reverse(t);
            Write(t, 0, 4);
        }

        public void WriteUInt32(uint value)
        {
            var t = BitConverter.GetBytes(value);
            if (IsBigEndian)
                Array.Reverse(t);
            Write(t, 0, 4);
        }

        public void WriteInt64(long value)
        {
            var t = BitConverter.GetBytes(value);
            if (IsBigEndian)
                Array.Reverse(t);
            Write(t, 0, 8);
        }

        public void WriteUInt64(ulong value)
        {
            var t = BitConverter.GetBytes(value);
            if (IsBigEndian)
                Array.Reverse(t);
            Write(t, 0, 8);
        }

        public void WriteBoolean(bool value)
        {
            WriteBits((uint) (value ? 1 : 0), 1);
        }

        public void WriteSingle(float value)
        {
            var t = BitConverter.GetBytes(value);
            if (IsBigEndian)
                Array.Reverse(t);
            Write(t, 0, 4);
        }

        public void WriteBits(int value, uint bitlength)
        {
            WriteBits((uint) value, bitlength);
        }

        public void WriteBits(uint value, uint bitlength, bool finish = false)
        {
            var r11 = (int) (Position & 0xFFFFFFFF & 0x1F);
            int r8 = 1;
            int r10 = (0x20 - r11);
            r11 = (int) (bitlength - r10);
            var r9 = (int) xshiftseed;
            if (r11 >= 0)
            {
                r10--;
                uint r6 = value >> r11;
                r10 = r8 << r10;
                r10 = (int) ((r10 << 1) & 0xFFFFFFFE);
                r10--;
                r6 &= (uint) r10;
                r10 = (int) (~r10 & xshiftseed);
                r9 = (int) (r6 | r10);
                xbaseStream.WriteInt32(r9, true);
                r10 = 0x20 - r11;
                r11 = r8 << r11;
                r11--;
                r8 = (int) (value << r10);
                r11 <<= r10;
            }
            else
            {
                r10 = (int) (bitlength - 1);
                r11 = -r11;
                r10 = r8 << r10;
                r10 = (int) ((r10 << 1) & 0xFFFFFFFE);
                r8 = (int) (value << r11);
                r10--;
                r11 = r10 << r11;
            }
            r10 = r8 & r11;
            r11 = (~r11 & r9);
            xshiftseed = (uint) (r10 | r11);
            CurrentBit += bitlength;
            if (finish)
            {
                r11 = (int)(Position & 0xFFFFFFFF & 0x1F);
                r10 = (0x20 - r11);
                r8 = (int) (xshiftseed >> r10);
                if (r10 <= 8)
                    xbaseStream.WriteByte((byte)r8);
                else if (r10 <= 0x10)
                    xbaseStream.WriteInt16((short)r8, true);
                else if (r10 <= 0x18)
                    xbaseStream.WriteInt24(r8, true);
                else if (r10 < 0x20)
                    xbaseStream.WriteInt24(r8, true);
            }
        }

        public int WriteData(byte[] data, int totalbits)
        {
            int index = 0;
            var xcur = (int) CurrentBit;
            var count = (((totalbits >> 3) & 0x1FFFFFFF) & 0x1FFFFFFC);
            if(data == null)
                data = new byte[count];
            if (count > data.Length)
                Array.Resize(ref data, count);
            while (totalbits >= 0x20)
            {
                var d = new byte[0x4];
                Array.Copy(data, index, d, 0, 4);
                if (IsBigEndian)
                    Array.Reverse(d);
                WriteBits(BitConverter.ToUInt32(d, 0), 0x20);
                index += 4;
                totalbits -= 0x20;
            }
            while (totalbits >= 8 && index < data.Length)
            {
                WriteBits(data[index], 8);
                totalbits -= 8;
                index++;
            }
            if (totalbits > 0 && index < data.Length)
                WriteBits(data[index], (uint) totalbits);
            return (int) (CurrentBit - xcur);
        }


        private int LoadWord(byte[] value, int index)
        {
            var temp = new byte[4];
            if (value.Length < (index + 4))
            {
                int left = value.Length - index;
                int start = value.Length - left;

                if (left == 1)
                    return value[start];

                if (left == 2)
                    Array.Copy(value, start, temp, 2, 2);
                else
                    Array.Copy(value, start, temp, 0, left);
            }
            else
                Array.Copy(value, index, temp, 0, 4);
            if (IsBigEndian)
                Array.Reverse(temp);
            return BitConverter.ToInt32(temp, 0);
        }

        public void WriteString(string value)
        {
            var x = Encoding.ASCII.GetBytes(value);
            Write(x, 0, x.Length);
        }

        public void FinishWriter()
        {
            if (Position > Length)
            {
                var xleft = Position - Length;
                if (xleft > 0)
                {
                    //Position -= xleft;
                    //var val = ReadBit2((uint) xleft);
                    WriteBits(0, (uint)xleft, true);
                }
            }
        }

        public byte[] ToArray()
        {
            var ps = xbaseStream.Position;
            xbaseStream.Position = 0;
            var data = new byte[xbaseStream.Length];
            xbaseStream.Read(data, 0, (int) xbaseStream.Length);
            xbaseStream.Position = ps;
            return data;
        }

        public byte[] GetLastChunck()
        {
            xbaseStream.Position = (((Length >> 3) & 0x1FFFFFFF) & 0x1FFFFFFC);
            if (xbaseStream.Position < xbaseStream.Length)
            {
                var x = (int) (xbaseStream.Length - xbaseStream.Position);
                var t = new byte[x];
                xbaseStream.Read(t, 0, x);
                return t;
            }
            return null;
        }

        public override void Close()
        {
            if (!xpaststream)
            {
                xbaseStream.Close();
                xbaseStream.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}