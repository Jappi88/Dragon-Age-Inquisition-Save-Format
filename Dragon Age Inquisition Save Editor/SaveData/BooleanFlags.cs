#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class BooleanFlags : DAInterface<BooleanFlags>
    {
        internal short Count { get; set; }
        public byte[][] Guids { get; set; }
        public uint LengthBits => 0;
        public int Length => this.InstanceLength();


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                
                if (Guids == null)
                    Guids = new byte[Count][];
                Count = (short) Guids.Length;
                io.WriteInt16(Count);
                for (int i = 0; i < Count; i++)
                {
                    if (Guids[i] == null)
                    {
                        Guids[i] = new byte[0x10];
                    }
                    for (int j = 0; j < 0x10; j++)
                        io.WriteBits(Guids[i][j], 0x8);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public BooleanFlags Read(DAIIO io)
        {
            Count = io.ReadInt16();
            Guids = new byte[Count][];
            for (int i = 0; i < Count; i++)
            {
                Guids[i] = new byte[0x10];
                for (int j = 0; j < 0x10; j++)
                    Guids[i][j] = (byte) io.ReadBit(0x8);
            }
            return this;
        }
    }
}