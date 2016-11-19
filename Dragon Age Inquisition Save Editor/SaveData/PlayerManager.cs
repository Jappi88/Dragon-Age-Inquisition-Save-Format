#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class PlayerManager : DAInterface<PlayerManager>
    {
        public PlayerManager(SaveDataStructure xstruc)
        {
            SStructure = xstruc;
        }

        internal SaveDataStructure SStructure { get; private set; }
        internal int xLength { get; set; }
        public bool InventoryExist { get; set; }
        public Inventory Inventory { get; set; }
        public bool ItemManagerExists { get; set; }
        public short ResearchedUpgradesCount { get; set; }
        public int[] ResearchedUpgradeIndexes { get; set; }
        public short PotionBankCount { get; set; }
        public PotionBank[] PotionBanks { get; set; }
        public bool HasUnknownBank { get; set; }
        public short UnknownBankCount { get; set; }
        public PotionBank[] UnknownBanks { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public PlayerManager Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            long xpos = io.Position;
            InventoryExist = io.ReadBoolean();
            if (InventoryExist)
                Inventory = new Inventory().Read(io);
            if (SStructure.EntityVersion > 8)
            {
                ItemManagerExists = io.ReadBoolean();
                if (ItemManagerExists)
                {
                    ResearchedUpgradesCount = io.ReadInt16();
                    ResearchedUpgradeIndexes = new int[ResearchedUpgradesCount];
                    for (int i = 0; i < ResearchedUpgradesCount; i++)
                        ResearchedUpgradeIndexes[i] = io.ReadInt32();

                    if (SStructure.ProjectVersion > 0x20)
                    {
                        PotionBankCount = io.ReadInt16();
                        PotionBanks = new PotionBank[PotionBankCount];
                        for (int i = 0; i < PotionBankCount; i++)
                            PotionBanks[i] = new PotionBank().Read(io);

                        //if (PotionBankCount > 0)
                        //{
                        //    UnknownBankCount = io.ReadInt16();
                        //    UnknownBanks = new PotionBank[UnknownBankCount];
                        //    for (int i = 0; i < UnknownBankCount; i++)
                        //        UnknownBanks[i] = new PotionBank().Read(io);
                        //}

                    }
                }
            }
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                io.WriteBoolean(InventoryExist);
                if (InventoryExist)
                    Inventory.Write(io);
                if (SStructure.EntityVersion > 8)
                {
                    io.WriteBoolean(ItemManagerExists);
                    if (ItemManagerExists)
                    {
                        if (ResearchedUpgradeIndexes == null)
                            ResearchedUpgradeIndexes = new int[ResearchedUpgradesCount];
                        io.WriteInt16((short) ResearchedUpgradeIndexes.Length);
                        foreach (int t in ResearchedUpgradeIndexes)
                            io.WriteInt32(t);
                        if (SStructure.ProjectVersion > 0x20)
                        {
                            if (PotionBanks == null)
                            {
                                PotionBanks = new PotionBank[PotionBankCount];

                                for (int xb = 0; xb < PotionBankCount; xb++)
                                    PotionBanks[xb] = new PotionBank();
                            }
                            io.WriteInt16((short) PotionBanks.Length);
                            foreach (PotionBank t in PotionBanks)
                                t.Write(io);
                            //if (PotionBanks.Length > 0)
                            //{
                            //    if (UnknownBanks == null)
                            //    {
                            //        UnknownBanks = new PotionBank[UnknownBankCount];

                            //        for (int xb = 0; xb < UnknownBankCount; xb++)
                            //            UnknownBanks[xb] = new PotionBank();
                            //    }
                            //    io.WriteInt16((short) UnknownBanks.Length);
                            //    foreach (PotionBank t in UnknownBanks)
                            //        t.Write(io);
                            //}
                        }
                    }
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