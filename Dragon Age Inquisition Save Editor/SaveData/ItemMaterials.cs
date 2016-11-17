#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class ItemMaterials : DAInterface<ItemMaterials>
    {
        public short Count { get; set; }
        public ItemMaterial[] Materials { get; set; }

        public int Length => this.InstanceLength();

        public ItemMaterials Read(DAIIO io)
        {
            Count = io.ReadInt16();
            Materials = new ItemMaterial[Count];
            for (int i = 0; i < Count; i++)
                Materials[i] = new ItemMaterial().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (Materials == null)
                {
                    Materials = new ItemMaterial[Count];

                    for (int xb = 0; xb < Count; xb++)
                        Materials[xb] = new ItemMaterial();
                }
                io.WriteInt16((short) Materials.Length);
                for (int i = 0; i < Materials.Length; i++)
                    Materials[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}