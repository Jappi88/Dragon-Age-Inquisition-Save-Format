#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class ItemDynamicStats : DAInterface<ItemDynamicStats>
    {
        public short StatsCount { get; set; }
        public ItemStatsData[] ItemStats { get; set; }

        public int Length => this.InstanceLength();

        public ItemDynamicStats Read(DAIIO io)
        {
            StatsCount = io.ReadInt16();
            ItemStats = new ItemStatsData[StatsCount];
            for (int i = 0; i < StatsCount; i++)
                ItemStats[i] = new ItemStatsData().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (ItemStats == null)
                {
                    ItemStats = new ItemStatsData[StatsCount];

                    for (int xb = 0; xb < StatsCount; xb++)
                        ItemStats[xb] = new ItemStatsData();
                }
                io.WriteInt16((short) ItemStats.Length);
                for (int i = 0; i < ItemStats.Length; i++)
                    ItemStats[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}