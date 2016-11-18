#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class CraftedStatInstance : DAInterface<CraftedStatInstance>
    {
        internal int xLength { get; set; }
        internal short StatsCount { get; set; }
        public CraftedItemStats[] Stats { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public CraftedStatInstance Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            StatsCount = io.ReadInt16();
            Stats = new CraftedItemStats[StatsCount];
            for (int i = 0; i < StatsCount; i++)
                Stats[i] = new CraftedItemStats().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                if (Stats == null)
                {
                    Stats = new CraftedItemStats[StatsCount];

                    for (int xb = 0; xb < StatsCount; xb++)
                        Stats[xb] = new CraftedItemStats();
                }
                io.WriteInt16((short) Stats.Length);
                for (int i = 0; i < StatsCount; i++)
                    Stats[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}