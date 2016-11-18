#region

using System;
using System.Collections.Generic;
using System.IO;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public static class Utils
    {
        public static int ReadInt32(this Stream input, bool isbigendian)
        {
            var x = new byte[4];
            input.Read(x, 0, 4);
            if (isbigendian)
                Array.Reverse(x);
            return BitConverter.ToInt32(x, 0);
        }

        public static short ReadShort(this Stream input, bool isbigendian)
        {
            var x = new byte[2];
            input.Read(x, 0, 2);
            if (isbigendian)
                Array.Reverse(x);
            return BitConverter.ToInt16(x, 0);
        }

        public static long ReadInt64(this Stream input, bool isbigendian)
        {
            var x = new byte[8];
            input.Read(x, 0, 8);
            if (isbigendian)
                Array.Reverse(x);
            return BitConverter.ToInt64(x, 0);
        }

        public static byte[] ReadBytes(this Stream input, int length)
        {
            var x = new byte[length];
            input.Read(x, 0, length);
            return x;
        }

        public static uint ReadUInt32(this Stream input, bool isbigendian)
        {
            var x = new byte[4];
            if (input.Position + 4 > input.Length)
                input.Read(x, 0, (int) (input.Length - input.Position));
            else
                input.Read(x, 0, 4);
            if (isbigendian)
                Array.Reverse(x);
            return BitConverter.ToUInt32(x, 0);
        }

        public static void WriteInt32(this Stream input, int value, bool isbigendian)
        {
            byte[] x = BitConverter.GetBytes(value);
            if (isbigendian)
                Array.Reverse(x);
            input.Write(x, 0, x.Length);
        }

        public static void WriteInt16(this Stream input, short value, bool isbigendian)
        {
            byte[] x = BitConverter.GetBytes(value);
            if (isbigendian)
                Array.Reverse(x);
            input.Write(x, 0, x.Length);
        }

        public static void WriteInt24(this Stream input, int value, bool isbigendian)
        {
            byte[] x = BitConverter.GetBytes(value);
            if (isbigendian)
                Array.Reverse(x);
            input.Write(x, 0, 3);
        }

        public static void WriteInt64(this Stream input, long value, bool isbigendian)
        {
            byte[] x = BitConverter.GetBytes(value);
            if (isbigendian)
                Array.Reverse(x);
            input.Write(x, 0, x.Length);
        }

        public static void WriteUInt32(this Stream input, uint value, bool isbigendian)
        {
            byte[] x = BitConverter.GetBytes(value);
            if (isbigendian)
                Array.Reverse(x);
            input.Write(x, 0, x.Length);
        }

        public static bool MemCompare(this byte[] data, byte[] crit, int offset, ref Dictionary<int,byte> valid, ref Dictionary<int, byte> invalid)
        {
            if (crit == null || crit.Length == 0 || (offset + crit.Length) > data.Length)
                return false;
            for (int i = 0; i < crit.Length; i++)
            {
                if (data[offset + i] != crit[i])
                {
                    invalid.Add(i, crit[i]);
                    valid.Add(offset + i, data[offset + i]);
                }
            }
            return invalid.Count == 0;
        }

        public static bool MemCompare(this byte[] data, byte[] crit, int offset)
        {
            Dictionary<int, byte> x = new Dictionary<int, byte>();
            Dictionary<int, byte> x2 = new Dictionary<int, byte>();
            return data.MemCompare(crit, offset, ref x,ref x2);
        }

        public static short ToShort(this byte[] buffer, int index, bool isbigendian)
        {
            var temp = new byte[2];
            Array.Copy(buffer, index, temp, 0, 2);
            if (isbigendian)
                Array.Reverse(temp);
            return BitConverter.ToInt16(temp, 0);
        }

        public static ushort ToUShort(this byte[] buffer, int index, bool isbigendian)
        {
            var temp = new byte[2];
            Array.Copy(buffer, index, temp, 0, 2);
            if (isbigendian)
                Array.Reverse(temp);
            return BitConverter.ToUInt16(temp, 0);
        }

        public static int ToInt32(this byte[] buffer, int index, bool isbigendian)
        {
            var temp = new byte[4];
            Array.Copy(buffer, index, temp, 0, 4);
            if (isbigendian)
                Array.Reverse(temp);
            return BitConverter.ToInt32(temp, 0);
        }

        public static uint ToUInt32(this byte[] buffer, int index, bool isbigendian)
        {
            var temp = new byte[4];
            Array.Copy(buffer, index, temp, 0, 4);
            if (isbigendian)
                Array.Reverse(temp);
            return BitConverter.ToUInt32(temp, 0);
        }

        public static long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long) (timeSpan.TotalSeconds + 0x0000003118a41200);
        }

        public static long ToUnixSecondsWithAdd(this DateTime time)
        {
            var timeSpan = (time - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long) (timeSpan.TotalSeconds + 0x0000003118a41200);
        }

        public static int ToUnixSecondsNoAdd(this DateTime time)
        {
            var timeSpan = (time - new DateTime(1970, 1, 1, 0, 0, 0));
            return (int) (timeSpan.TotalSeconds);
        }

        public static DateTime ToUnixTime(this long value)
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(value - 0x0000003118a41200));
        }

        public static DateTime ToUnixTime(this int value)
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(value));
        }

        public static int NumberOfSetBits(this int x)
        {
            return ((x/0x20) + ((x%0x20) == 0 ? 0 : 1));
        }

        internal static int InstanceLength<T>(this DAInterface<T> instance, bool skiplength) where T : class
        {
            if (instance == null)
                return 0;
            int xreturn = 0;
            var io = new DAIIO(false);
            instance.Write(io, skiplength);
            xreturn = (int) io.CurrentBit;
            io.Close();
            return xreturn;
        }

        internal static int InstanceLength<T>(this DAInterface<T> instance) where T : class
        {
            return InstanceLength(instance,true);
        }
    }
}