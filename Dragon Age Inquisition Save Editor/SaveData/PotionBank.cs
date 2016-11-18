#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class PotionBank : DAInterface<PotionBank>
    {
        internal int xLength { get; set; }
        public byte Value { get; set; }
        public ItemAsset Key { get; set; }

        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public PotionBank Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            Key = new ItemAsset().Read(io);
            Value = (byte) io.ReadBit2(0x8);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                Key.Write(io);
                io.WriteBits(Value, 0x8);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}