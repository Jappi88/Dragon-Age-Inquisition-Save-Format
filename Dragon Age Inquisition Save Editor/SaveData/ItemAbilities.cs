#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class ItemAbilities : DAInterface<ItemAbilities>
    {
        public short Count { get; set; }
        public ItemAsset[] Abilities { get; set; }
        public uint LengthBits => 0;
        public int Length => this.InstanceLength();

        public ItemAbilities Read(DAIIO io)
        {
            Count = io.ReadInt16();
            Abilities = new ItemAsset[Count];
            for (int i = 0; i < Count; i++)
                Abilities[i] = new ItemAsset().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (Abilities == null)
                {
                    Abilities = new ItemAsset[Count];

                    for (int xb = 0; xb < Count; xb++)
                        Abilities[xb] = new ItemAsset();
                }
                io.WriteInt16((short) Abilities.Length);
                foreach (ItemAsset t in Abilities)
                    t.Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}