#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class StoreInventory : DAInterface<StoreInventory>
    {
        public StoreInventory(SaveDataStructure xstruct)
        {
            SStructure = xstruct;
        }

        internal SaveDataStructure SStructure { get; private set; }
        internal int xLength { get; set; }
        public int storeEntitySaveUid { get; set; }
        public short ItemCount { get; set; }
        public StoreItemEntry[] Items { get; set; }

        public int Length => this.InstanceLength();

        public StoreInventory Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            storeEntitySaveUid = io.ReadInt32();
            ItemCount = io.ReadInt16();
            Items = new StoreItemEntry[ItemCount];
            for (int i = 0; i < ItemCount; i++)
                Items[i] = new StoreItemEntry().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, 0x18);
                io.WriteInt32(storeEntitySaveUid);
               
                if (Items == null)
                {
                    Items = new StoreItemEntry[ItemCount];

                    for (int xb = 0; xb < ItemCount; xb++)
                        Items[xb] = new StoreItemEntry();
                }
                io.WriteInt16((short) Items.Length);
                for (int i = 0; i < Items.Length; i++)
                    Items[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}