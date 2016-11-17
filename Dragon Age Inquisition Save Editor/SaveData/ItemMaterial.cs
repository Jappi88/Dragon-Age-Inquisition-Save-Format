#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class ItemMaterial : DAInterface<ItemMaterial>
    {
       internal int xLength { get; set; }
        public int Index { get; set; }

        public int ShiftedIndex => ((Index + 0x18) << 2);

        public ItemAsset Asset { get; set; }

        public int Length => this.InstanceLength();

        public ItemMaterial Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            Index = io.ReadInt32();
            Asset = new ItemAsset().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength)io.WriteBits(Length, 0x18);
                io.WriteInt32(Index);
                Asset.Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}