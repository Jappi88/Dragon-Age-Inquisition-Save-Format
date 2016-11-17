#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class PersistentLoot : DAInterface<PersistentLoot>
    {
        internal int xLength { get; set; }
        internal short LootMapCount { get; set; }
        public LootMap[] LootMaps { get; set; }

        public int Length => this.InstanceLength();

        public PersistentLoot Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            LootMapCount = io.ReadInt16();
            LootMaps = new LootMap[LootMapCount];
            for (int i = 0; i < LootMapCount; i++)
                LootMaps[i] = new LootMap().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength) io.WriteBits(Length, 0x18);
                if (LootMaps == null)
                {
                    LootMaps = new LootMap[LootMapCount];

                    for (int xb = 0; xb < LootMapCount; xb++)
                        LootMaps[xb] = new LootMap();
                }
                io.WriteInt16((short)LootMaps.Length);
                for (int i = 0; i < LootMapCount; i++)
                    LootMaps[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}