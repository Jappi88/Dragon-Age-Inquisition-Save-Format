#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class HeadVariations : DAInterface<HeadVariations>
    {
        internal int xLength { get; set; }
        internal short VariationCount { get; set; }
        public HeadVariation[] Variations { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public HeadVariations Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            VariationCount = (short) io.ReadBit2(0x10);
            Variations = new HeadVariation[VariationCount];
            for (int i = 0; i < VariationCount; i++)
                Variations[i] = new HeadVariation().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                if (Variations == null)
                {
                    Variations = new HeadVariation[VariationCount];
                    for (int xb = 0; xb < VariationCount; xb++)
                        Variations[xb] = new HeadVariation();
                }
                io.WriteBits(Variations.Length, 0x10);
                for (int i = 0; i < VariationCount; i++)
                    Variations[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}