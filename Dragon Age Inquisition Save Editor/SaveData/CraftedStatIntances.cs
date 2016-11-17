#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class CraftedStatIntances : DAInterface<CraftedStatIntances>
    {
        internal short StatsCount { get; set; }
        public CraftedStatInstance[] Stats { get; set; }

        public int Length => this.InstanceLength();

        public CraftedStatIntances Read(DAIIO io)
        {
            StatsCount = io.ReadInt16();
            Stats = new CraftedStatInstance[StatsCount];
            for (int i = 0; i < StatsCount; i++)
                Stats[i] = new CraftedStatInstance().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (Stats == null)
                {
                    Stats = new CraftedStatInstance[StatsCount];

                    for (int xb = 0; xb < StatsCount; xb++)
                        Stats[xb] = new CraftedStatInstance();
                }
                io.WriteInt16((short) Stats.Length);
                for (int i = 0; i < Stats.Length; i++)
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