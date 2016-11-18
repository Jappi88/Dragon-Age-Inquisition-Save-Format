#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class TerrainPart : DAInterface<TerrainPart>
    {
        public TerrainPart(SaveDataStructure xstruct)
        {
            SStructure = xstruct;
        }

        internal SaveDataStructure SStructure { get; private set; }
        internal int xLength { get; set; }
        public PartDestructionComplex DestructionComplex { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public TerrainPart Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            if (SStructure.SaveVersion >= 0x11)
                DestructionComplex = new PartDestructionComplex(SStructure).Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                if (SStructure.SaveVersion >= 0x11)
                    DestructionComplex.Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}