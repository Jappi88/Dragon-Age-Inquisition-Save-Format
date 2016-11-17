#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class Inventory : DAInterface<Inventory>
    {
        internal int xLength { get; set; }
        internal short ItemCount { get; set; }
        public ItemEntry[] Items { get; set; }

        public int Length => this.InstanceLength();

        public Inventory Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            ItemCount = io.ReadInt16();
            Items = new ItemEntry[ItemCount];
            for (int i = 0; i < ItemCount; i++)
                Items[i] = new ItemEntry().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength) io.WriteBits(Length, 0x18);
                if (Items == null)
                {
                    Items = new ItemEntry[ItemCount];

                    for (int xb = 0; xb < ItemCount; xb++)
                        Items[xb] = new ItemEntry();
                }
                ItemCount = (short) Items.Length;
                io.WriteInt16(ItemCount);
                for (int i = 0; i < ItemCount; i++)
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