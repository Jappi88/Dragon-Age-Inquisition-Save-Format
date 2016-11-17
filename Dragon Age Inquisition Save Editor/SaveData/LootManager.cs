#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class LootManager : DAInterface<LootManager>
    {
        public LootManager(SaveDataStructure strx)
        {
            SStStructure = strx;
        }

        internal SaveDataStructure SStStructure { get; private set; }
        internal int xLength { get; set; }
        public short Version { get; set; }
        public PersistentLoot PersistentLoot { get; set; }
        public DynamicLootOwners DynamicLootOwners { get; set; }

        public int Length => this.InstanceLength();

        public LootManager Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            Version = io.ReadInt16();
            if (Version > 2)
            {
                PersistentLoot = new PersistentLoot().Read(io);
                DynamicLootOwners = new DynamicLootOwners().Read(io);
            }
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength) io.WriteBits(Length, 0x18);
                io.WriteInt16(Version);
                if (Version > 2)
                {
                    PersistentLoot.Write(io);
                    DynamicLootOwners.Write(io);
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