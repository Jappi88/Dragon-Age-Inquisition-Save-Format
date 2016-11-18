#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class BoneOffsetsV2 : DAInterface<BoneOffsetsV2>
    {
        internal int xLength { get; set; }
        internal short BoneOffsetsCount { get; set; }
        public BoneOffsetEntryV2[] BoneOffsetEntries { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public BoneOffsetsV2 Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            BoneOffsetsCount = (short) io.ReadBit2(0x10);
            BoneOffsetEntries = new BoneOffsetEntryV2[BoneOffsetsCount];
            for (int i = 0; i < BoneOffsetsCount; i++)
                BoneOffsetEntries[i] = new BoneOffsetEntryV2().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                if (BoneOffsetEntries == null)
                {
                    BoneOffsetEntries = new BoneOffsetEntryV2[BoneOffsetsCount];

                    for (int xb = 0; xb < BoneOffsetsCount; xb++)
                        BoneOffsetEntries[xb] = new BoneOffsetEntryV2();
                }
                io.WriteBits(BoneOffsetEntries.Length, 0x10);
                foreach (BoneOffsetEntryV2 t in BoneOffsetEntries)
                    t.Write(io); 

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}