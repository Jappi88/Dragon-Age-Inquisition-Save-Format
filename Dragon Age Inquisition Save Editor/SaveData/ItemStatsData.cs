#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class ItemStatsData : DAInterface<ItemStatsData>
    {
       internal int xLength { get; set; }
        public ItemAsset StatsData { get; set; }
        public float Value { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public ItemStatsData Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            StatsData = new ItemAsset().Read(io);
            Value = io.ReadSingle();
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength)io.WriteBits(Length, LengthBits);
                StatsData.Write(io);
                io.WriteSingle(Value);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}