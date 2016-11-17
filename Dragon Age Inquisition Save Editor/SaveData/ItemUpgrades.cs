#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class ItemUpgrades : DAInterface<ItemUpgrades>
    {
        public short Count { get; set; }
        public ItemUpgrade[] Upgrades { get; set; }

        public int Length => this.InstanceLength();

        public ItemUpgrades Read(DAIIO io)
        {
            Count = io.ReadInt16();
            Upgrades = new ItemUpgrade[Count];
            for (int i = 0; i < Count; i++)
                Upgrades[i] = new ItemUpgrade().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (Upgrades == null)
                {
                    Upgrades = new ItemUpgrade[Count];

                    for (int xb = 0; xb < Count; xb++)
                        Upgrades[xb] = new ItemUpgrade();
                }
                Count = (short) Upgrades.Length;
                io.WriteInt16(Count);
                for (int i = 0; i < Count; i++)
                    Upgrades[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}