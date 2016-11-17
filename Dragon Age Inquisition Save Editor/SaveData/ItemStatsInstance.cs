#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class ItemStatsInstance : DAInterface<ItemStatsInstance>
    {
        internal long Offset { get; private set; }
        internal int xLength { get; set; }
        public short Version { get; set; }
        public ItemAsset StatsData { get; set; }
        public int Uid { get; set; }
        public bool IsForSale { get; set; }
        public bool IsNew { get; set; }
        public bool IsCrafted { get; set; }
        public int StringId { get; set; }
        public byte StackSize { get; set; }
        public byte MaxStackSize { get; set; }
        public ItemStatInstance StatsInstance { get; set; }
        public ItemUpgrades Upgrades { get; set; }
        public bool SuppressClassRestriction { get; set; }
        public bool IsPlaceHolder { get; set; }
        public bool HasSoundActionsReference { get; set; }
        public ItemAsset SoundActionsReference { get; set; }

        public int Length => this.InstanceLength();

        public ItemStatsInstance()
        {
        }

        public ItemStatsInstance Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            Offset = io.Position;
            Version = io.ReadInt16();
            StatsData = new ItemAsset().Read(io);
            if (Version < 8)
                Uid = io.ReadInt32();
            IsForSale = io.ReadBoolean();
            IsNew = io.ReadBoolean();
            IsCrafted = io.ReadBoolean();
            StringId = io.ReadInt32();
            if (xLength > 0x8a)
            {
                StatsInstance = new ItemStatInstance(this,IsCrafted).Read(io);
                if ((io.Position - Offset) < xLength)
                {
                    Upgrades = new ItemUpgrades().Read(io);
                    SuppressClassRestriction = io.ReadBoolean();
                    if (Version > 6)
                        IsPlaceHolder = io.ReadBoolean();
                    if (Version > 0xA)
                    {
                        HasSoundActionsReference = io.ReadBoolean();
                        if (HasSoundActionsReference)
                            SoundActionsReference = new ItemAsset().Read(io);
                    }
                }
            }

            if ((io.Position - Offset) + 8 <= xLength)
                StackSize = (byte) io.ReadBit2(0x8);
            else StackSize = 0xff;
            if (Version >= 9 && ((io.Position - Offset) + 8 <= xLength))
                MaxStackSize = (byte) io.ReadBit2(0x8);
            else MaxStackSize = 0xff;
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, 0x18);
                Offset = io.Position;
                io.WriteInt16(Version);
                if (StatsData == null)
                    StatsData = new ItemAsset();
                StatsData.Write(io);
                if (Version < 8)
                    io.WriteInt32(Uid);
                io.WriteBoolean(IsForSale);
                io.WriteBoolean(IsNew);
                io.WriteBoolean(IsCrafted);
                io.WriteInt32(StringId);
                if (StatsInstance != null)
                {
                    StatsInstance.Write(io);
                    if (Upgrades != null)
                    {
                        Upgrades.Write(io);
                        io.WriteBoolean(SuppressClassRestriction);
                        if (Version > 6)
                            io.WriteBoolean(IsPlaceHolder);
                        if (Version > 0xA)
                        {
                            io.WriteBoolean(HasSoundActionsReference);
                            if (HasSoundActionsReference)
                                SoundActionsReference.Write(io);
                        }
                    }
                }
                if (StackSize != 0xff)
                    io.WriteBits(StackSize, 0x8);
                if (MaxStackSize != 0xff)
                    io.WriteBits(MaxStackSize, 0x8);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}