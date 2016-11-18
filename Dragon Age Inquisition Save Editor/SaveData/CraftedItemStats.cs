#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class CraftedItemStats : DAInterface<CraftedItemStats>
    {
       internal int xLength { get; set; }
        public ItemAsset StatData { get; set; }
        public ItemAsset Script { get; set; }
        internal short ArgsCount { get; set; }
        public ItemAsset[] DelArgs { get; set; }
        public float DefaultValue { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public CraftedItemStats Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            StatData = new ItemAsset().Read(io);
            Script = new ItemAsset().Read(io);
            ArgsCount = io.ReadInt16();
            DelArgs = new ItemAsset[ArgsCount];
            for (int i = 0; i < ArgsCount; i++)
                DelArgs[i] = new ItemAsset().Read(io);
            DefaultValue = io.ReadSingle();
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength)io.WriteBits(Length, LengthBits);
                StatData.Write(io);
                Script.Write(io);
                if (DelArgs == null)
                {
                    DelArgs = new ItemAsset[ArgsCount];

                    for (int xb = 0; xb < ArgsCount; xb++)
                        DelArgs[xb] = new ItemAsset();
                }
                io.WriteInt16((short) DelArgs.Length);
                foreach (ItemAsset t in DelArgs)
                    t.Write(io);
                io.WriteSingle(DefaultValue);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}