#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class ItemStatInstance : DAInterface<ItemStatInstance>
    {
        public bool HasDynamicStats { get; set; }
        public ItemDynamicStats DynamicStats { get; set; }
        public CraftedStatIntances CraftedStatIntances { get; set; }
        public ItemAbilities ItemAbilities { get; set; }
        public ItemTimelines ItemTimelines { get; set; }
        public ItemMaterials ItemMaterials { get; set; }
        public string DisplayString { get; set; }
        public CompositionDisplay CompositionDisplay { get; set; }
        public int Level { get; set; }
        public int DamageType { get; set; }
        public int Quality { get; set; }
        public uint LengthBits => 0;
        public int Length => this.InstanceLength();

        private readonly ItemStatsInstance _info;
        private readonly bool _iscrafted;

        public ItemStatInstance(ItemStatsInstance info, bool iscrafted)
        {
            _info = info;
            _iscrafted = iscrafted;
        }

        public ItemStatInstance Read(DAIIO io)
        {
            if (_info.Version > 1)
            {
                HasDynamicStats = io.ReadBoolean();
                if (HasDynamicStats)
                    DynamicStats = new ItemDynamicStats().Read(io);
                else if (_info.Version > 3)
                    DynamicStats = new ItemDynamicStats().Read(io);
            }
            else if (_iscrafted)
                CraftedStatIntances = new CraftedStatIntances().Read(io);
            if (_info.Version > 9)
                ItemAbilities = new ItemAbilities().Read(io);
            if (_info.Version < 3)
                ItemTimelines = new ItemTimelines().Read(io);
            ItemMaterials = new ItemMaterials().Read(io);
            if (_info.Version < 5)
                CompositionDisplay = new CompositionDisplay().Read(io);
            else
            {
                short count = io.ReadInt16();
                DisplayString = io.ReadString(count);
            }
            Level = io.ReadInt32();
            DamageType = io.ReadInt32();
            Quality = io.ReadInt32();
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (_info.Version > 1)
                {
                    io.WriteBoolean(HasDynamicStats);
                    if (HasDynamicStats)
                        DynamicStats.Write(io);
                    else if (_info.Version > 3)
                        DynamicStats.Write(io);
                }
                else if (_iscrafted)
                    CraftedStatIntances.Write(io);
                if (_info.Version > 9)
                    ItemAbilities.Write(io);
                if (_info.Version < 3)
                    ItemTimelines.Write(io);
                ItemMaterials.Write(io);
                if (_info.Version < 5)
                    CompositionDisplay.Write(io);
                else
                {
                    io.WriteInt16((short) DisplayString.Length);
                    io.WriteString(DisplayString);
                }
                io.WriteInt32(Level);
                io.WriteInt32(DamageType);
                io.WriteInt32(Quality);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}