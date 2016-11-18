#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class StoreInventories : DAInterface<StoreInventories>
    {
        public StoreInventories(SaveDataStructure xstruct)
        {
            SStructure = xstruct;
        }

        internal SaveDataStructure SStructure { get; private set; }
        internal int xLength { get; set; }
        public int Version { get; set; }
        public short InventoryCount { get; set; }
        public StoreInventory[] Inventories { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public StoreInventories Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            Version = io.ReadInt32();
            InventoryCount = io.ReadInt16();
            Inventories = new StoreInventory[InventoryCount];
            for (int i = 0; i < InventoryCount; i++)
                Inventories[i] = new StoreInventory(SStructure).Read(io);
            return this;
        }

        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                io.WriteInt32(Version);
                if (Inventories == null)
                {
                    Inventories = new StoreInventory[InventoryCount];

                    for (int xb = 0; xb < InventoryCount; xb++)
                        Inventories[xb] = new StoreInventory(SStructure);
                }
                io.WriteInt16((short) Inventories.Length);
                for (int i = 0; i < Inventories.Length; i++)
                    Inventories[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}