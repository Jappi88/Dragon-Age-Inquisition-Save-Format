#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class ItemEntry : DAInterface<ItemEntry>
    {
        public ItemStatsInstance ItemStats { get; set; }
        public byte MasterworkSuccesses { get; set; }
        public bool MasterworkSuccess { get; set; }

        public int Length => this.InstanceLength();

        public ItemEntry Read(DAIIO io)
        {
            ItemStats = new ItemStatsInstance().Read(io);
            if ((io.Position - ItemStats.Offset) < ItemStats.xLength)
            {
                if (ItemStats.Version >= 6)
                    MasterworkSuccesses = (byte) io.ReadBit2(5);
                else MasterworkSuccess = io.ReadBoolean();
            }
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                ItemStats.Write(io);
                if ((io.Position - ItemStats.Offset) < ItemStats.xLength)
                {
                    if (ItemStats.Version >= 6)
                        io.WriteBits(MasterworkSuccesses, 5);
                    else io.WriteBoolean(MasterworkSuccess);
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