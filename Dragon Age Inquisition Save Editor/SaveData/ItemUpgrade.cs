#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class ItemUpgrade : DAInterface<ItemUpgrade>
    {
        public ItemStatsInstance Stats { get; set; }

        public int Length => this.InstanceLength();

        public ItemUpgrade Read(DAIIO io)
        {
            Stats = new ItemStatsInstance().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                Stats.Write(io);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}