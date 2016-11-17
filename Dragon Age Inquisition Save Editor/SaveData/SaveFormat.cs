#region

using System;
using System.IO;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class SaveFormat
    {
        #region Constants
        public const long HeaderMagic = 0x4642484541444552;

        #endregion

        #region Public Properties

        internal short Version = 1;
        public int TotalLength { get; internal set; }
        public int HeaderLength { get; internal set; }
        public int DataLength { get; internal set; }
        public byte[] Header { get; set; }
        public SaveDataStructure DataStructure { get; private set; }
        public PackageHeader PackageHeader { get; set; }

        #endregion

        #region Constructors

        public SaveFormat(string file)
        {
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                Init(fs);
            }
        }

        public SaveFormat(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                Init(ms);
            }
        }

        public SaveFormat(Stream input)
        {
            Init(input);
        }

        #endregion

        #region Private Methods

        private void Init(Stream input)
        {
            if (input.Length < 0x100)
                throw new Exception(
                    "Invalid Savedata!\nData is too short for it to be valid, please select a valid save file");
            long val = input.ReadInt32(true);
            if(val != (long) PackageHeader.Mc02)
            {
                input.Position -= 4;
                val = input.ReadInt64(true);
                if (val != (long) PackageHeader.Fb)
                    throw new Exception("Invalid Package Header!");
            }
            PackageHeader = (PackageHeader) val;
            bool isbigendian = PackageHeader == PackageHeader.Mc02;
            if (PackageHeader == PackageHeader.Mc02)
                InitMcPackage(input);
            else
                InitFbPackage(input);
            uint hash = input.ReadUInt32(isbigendian);
            Header = input.ReadBytes(HeaderLength - 4);
            var crc = new DA3CRC();
            if (crc.Compute(Header) != hash)
                throw new Exception(
                    "Header data did not pass intergrity check!\nPlease choose a valid unmodified save to proceed");
            hash = input.ReadUInt32(isbigendian);
            var data = input.ReadBytes(DataLength - 4);
            uint xhash = crc.Compute(data);
            if (hash != xhash)
                throw new Exception(
                    "Save data did not pass the intergrity check!\nPlease choose a valid unmodified save to proceed");

            var xio = new DAIIO(data, isbigendian);
            DataStructure = new SaveDataStructure().Read(xio);
            xio.Close();
        }

        void InitMcPackage(Stream input)
        {
            TotalLength = input.ReadInt32(true);
            if (TotalLength != input.Length)
                throw new Exception(
                    "Invalid file descriptor!\nLength specified does not match the actual file length");
            HeaderLength = input.ReadInt32(true);
            if (HeaderLength > (input.Length - (input.Position + 0x14)))
                throw new Exception(
                    "Invalid file descriptor!\nHeaderLength specified does not match the actual header length");
            DataLength = input.ReadInt32(true);
            if (DataLength > (input.Length - ((input.Position + 0xc) + HeaderLength)))
                throw new Exception(
                    "Invalid file descriptor!\nDataLength specified does not match the actual data length");
            input.Position += 0xc;
        }

        void InitFbPackage(Stream input)
        {
            Version = input.ReadShort(false);
            HeaderLength = input.ReadInt32(false);
            if (HeaderLength > (input.Length - (input.Position + 0x4)))
                throw new Exception(
                    "Invalid file descriptor!\nHeaderLength specified does not match the actual header length");
            DataLength = input.ReadInt32(false);
            if (DataLength > (input.Length - (input.Position + HeaderLength)))
                throw new Exception(
                    "Invalid file descriptor!\nDataLength specified does not match the actual data length");
        }

        byte[] BuildMcPackage(byte[] data)
        {
            var x = data;
            using (var ms = new MemoryStream())
            {
                ms.WriteUInt32((uint)PackageHeader.Mc02, true);
                var xx = ((Header.Length + x.Length) + 0x24);
                ms.WriteInt32(xx, true);
                ms.WriteInt32(Header.Length + 4, true);
                ms.WriteInt32(x.Length + 4, true);
                ms.Write(new byte[0xc], 0, 0xc);
                ms.WriteUInt32(new DA3CRC().Compute(Header), true);
                ms.Write(Header, 0, Header.Length);
                ms.WriteUInt32(new DA3CRC().Compute(x), true);
                ms.Write(x, 0, x.Length);
                x = ms.ToArray();
            }
            return x;
        }

        byte[] BuildFbPackage(byte[] data)
        {
            var x = data;
            using (var ms = new MemoryStream())
            {
                ms.WriteInt64((long)PackageHeader.Fb, true);
                ms.WriteInt32(Header.Length + 4, false);
                ms.WriteInt32(x.Length + 4, false);
                ms.WriteUInt32(new DA3CRC().Compute(Header), false);
                ms.Write(Header, 0, Header.Length);
                ms.WriteUInt32(new DA3CRC().Compute(x), false);
                ms.Write(x, 0, x.Length);
                x = ms.ToArray();
            }
            return x;
        }

        public byte[] Rebuild()
        {
            var io = new DAIIO(PackageHeader == PackageHeader.Mc02);
            DataStructure.Write(io);
            var x = io.ToArray();
            io.Close();
            io.Dispose();
            if (PackageHeader == PackageHeader.Mc02)
                return BuildMcPackage(x);
            return BuildFbPackage(x);
        }

        #endregion
    }
}