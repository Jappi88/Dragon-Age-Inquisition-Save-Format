#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class StoreItemEntry : DAInterface<StoreItemEntry>
    {
        internal int xLength { get; set; }
        public int NameHash { get; set; }
        public int Quantity { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public StoreItemEntry Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            NameHash = io.ReadInt32();
            Quantity = io.ReadInt32();
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                io.WriteInt32(NameHash);
                io.WriteInt32(Quantity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}