#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class Map : DAInterface<Map>
    {
       internal int xLength { get; set; }
        public int MapID { get; set; }
        public bool IsPersistent { get; set; }
        public int UncompressedSize { get; set; }
        public bool IsCompressed { get; set; }
        public int Size { get; set; }
        public byte[] Blob { get; set; }

        public int Length => this.InstanceLength();

        public Map Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            MapID = io.ReadInt32();
            IsPersistent = io.ReadBoolean();
            UncompressedSize = io.ReadInt32();
            IsCompressed = io.ReadBoolean();
            Size = io.ReadInt32();
            Blob = new byte[Size];
            for (int i = 0; i < Size; i++)
                Blob[i] = (byte) io.ReadBit(0x8);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength)io.WriteBits(Length, 0x18);
                io.WriteInt32(MapID);
                io.WriteBoolean(IsPersistent);
                io.WriteInt32(UncompressedSize);
                io.WriteBoolean(IsCompressed);
                if (Blob == null)
                    Blob = new byte[Size];
                io.WriteInt32(Blob.Length);
                for (int i = 0; i < Blob.Length; i++)
                    io.WriteBits(Blob[i], 0x8);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}