#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class BoneOffsetEntryV2 : DAInterface<BoneOffsetEntryV2>
    {
       internal int xLength { get; set; }
        public byte[] Value { get; set; }
        public int BoneNameHash { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public BoneOffsetEntryV2 Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            BoneNameHash = io.ReadBit2(0x20);
            Value = new byte[0xC];
            for (int j = 0; j < 0xC; j++)
                Value[j] = (byte) io.ReadBit(0x8);

            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength)io.WriteBits(Length, LengthBits);
                io.WriteBits(BoneNameHash, 0x20);
                if (Value == null)
                    Value = new byte[0xC];
                for (int j = 0; j < 0xC; j++)
                    io.WriteBits(Value[j], 0x8);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}