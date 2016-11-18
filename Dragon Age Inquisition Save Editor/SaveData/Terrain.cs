#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class Terrain : DAInterface<Terrain>
    {
        public Terrain(SaveDataStructure xstruct)
        {
            SStructure = xstruct;
        }

        internal SaveDataStructure SStructure { get; private set; }
        internal int xLength { get; set; }
        public ImpactEntityDestructionComplex ImpactEntityDestructionComplex { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public Terrain Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            ImpactEntityDestructionComplex = new ImpactEntityDestructionComplex(SStructure).Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                ImpactEntityDestructionComplex.Write(io);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}