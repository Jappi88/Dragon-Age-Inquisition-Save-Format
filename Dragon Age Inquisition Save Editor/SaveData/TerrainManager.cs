#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class TerrainManager : DAInterface<TerrainManager>
    {
        public TerrainManager(SaveDataStructure xstruct)
        {
            SStructure = xstruct;
        }

        internal SaveDataStructure SStructure { get; private set; }
        internal int xLength { get; set; }
        public Terrain Terrain { get; set; }
        public TerrainPart Part { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public TerrainManager Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            Terrain = new Terrain(SStructure).Read(io);
            Part = new TerrainPart(SStructure).Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                Terrain.Write(io);
                Part.Write(io);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}