#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class StoreInventoryManager : DAInterface<StoreInventoryManager>
    {
        public StoreInventoryManager(SaveDataStructure xstruct)
        {
            SStructure = xstruct;
        }

        internal SaveDataStructure SStructure { get; private set; }
        internal int xLength { get; set; }
        public StoreInventories StoreInventories { get; set; }

        public int UnknownBitLength { get; set; }
        public byte[] UnknownData { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public StoreInventoryManager Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            StoreInventories = new StoreInventories(SStructure).Read(io);
            var x = xLength - Length;
            if (x > 0)
            {
                UnknownBitLength = io.ReadBit2(0x18);
                if (UnknownBitLength > 0)
                    UnknownData = io.ReadData(UnknownBitLength);
            }
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                StoreInventories.Write(io);
                if (UnknownBitLength > 0)
                {
                    io.WriteBits(UnknownBitLength, 0x18);
                    if (UnknownData != null)
                        io.WriteData(UnknownData, UnknownBitLength);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}