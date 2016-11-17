#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class BoneOffsetsV1 : DAInterface<BoneOffsetsV1>
    {
        internal int xLength { get; set; }
        internal short BoneOffsetsCount { get; set; }
        public BoneOffsetEntryV1[] BoneOffsetEntries { get; set; }

        public int Length => this.InstanceLength();

        public BoneOffsetsV1 Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            BoneOffsetsCount = (short) io.ReadBit2(0x10);
            BoneOffsetEntries = new BoneOffsetEntryV1[BoneOffsetsCount];
            for (int i = 0; i < BoneOffsetsCount; i++)
                BoneOffsetEntries[i] = new BoneOffsetEntryV1().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength) io.WriteBits(Length, 0x18);
                
                if (BoneOffsetEntries == null)
                {
                    BoneOffsetEntries = new BoneOffsetEntryV1[BoneOffsetsCount];

                    for (int xb = 0; xb < BoneOffsetsCount; xb++)
                        BoneOffsetEntries[xb] = new BoneOffsetEntryV1();
                }
                io.WriteBits(BoneOffsetEntries.Length, 0x10);
                foreach (BoneOffsetEntryV1 t in BoneOffsetEntries)
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