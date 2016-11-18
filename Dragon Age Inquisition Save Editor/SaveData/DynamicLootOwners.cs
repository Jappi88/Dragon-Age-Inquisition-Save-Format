#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class DynamicLootOwners : DAInterface<DynamicLootOwners>
    {
        internal int xLength { get; set; }
        public int NextUid { get; set; }
        internal short LootOwnersCount { get; set; }
        public LootOwner[] LootOwners { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public DynamicLootOwners Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            NextUid = io.ReadBit2(0x20);
            LootOwnersCount = io.ReadInt16();
            LootOwners = new LootOwner[LootOwnersCount];
            for (int i = 0; i < LootOwnersCount; i++)
            {
                var x = new LootOwner();
                x.Read(io);
                LootOwners[i] = x;
            }

            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                io.WriteBits(NextUid, 0x20);
                if (LootOwners == null)
                {
                    LootOwners = new LootOwner[LootOwnersCount];
                    for (int xb = 0; xb < LootOwnersCount; xb++)
                        LootOwners[xb] = new LootOwner();
                }
                io.WriteInt16((short) LootOwners.Length);
                for (int i = 0; i < LootOwnersCount; i++)
                    LootOwners[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}