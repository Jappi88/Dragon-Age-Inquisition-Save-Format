#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class LootMap : DAInterface<LootMap>
    {
        internal int xLength { get; set; }
        public int LevelId { get; set; }
        public int BufferSize { get; set; }
        public byte[] Buffer { get; set; }

        public int Length => this.InstanceLength();

        public LootMap Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            LevelId = io.ReadBit2(0x20);
            BufferSize = io.ReadBit2(0x20);
            Buffer = new byte[BufferSize];
            for (int j = 0; j < BufferSize; j++)
                Buffer[j] = (byte) io.ReadBit(0x8);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, 0x18);
                io.WriteBits(LevelId, 0x20);
                if (Buffer == null)
                    Buffer = new byte[BufferSize];
                io.WriteBits(Buffer.Length, 0x20);
                for (int j = 0; j < Buffer.Length; j++)
                    io.WriteBits(Buffer[j], 0x8);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}