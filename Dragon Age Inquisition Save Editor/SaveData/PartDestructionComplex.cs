#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class PartDestructionComplex : DAInterface<PartDestructionComplex>
    {
        public PartDestructionComplex(SaveDataStructure xstruct)
        {
            SStructure = xstruct;
        }

        internal SaveDataStructure SStructure { get; private set; }
        internal int xLength { get; set; }
        public short OwnerInfoCount { get; set; }
        public OwnerInfo[] OwnerInfos { get; set; }

        public int Length => this.InstanceLength();

        public PartDestructionComplex Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            OwnerInfoCount = io.ReadInt16();
            OwnerInfos = new OwnerInfo[OwnerInfoCount];
            for (int i = 0; i < OwnerInfoCount; i++)
                OwnerInfos[i] = new OwnerInfo(SStructure).Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength) io.WriteBits(Length, 0x18);
                io.WriteInt16(OwnerInfoCount);
                if (OwnerInfos == null)
                {
                    OwnerInfos = new OwnerInfo[OwnerInfoCount];

                    for (int xb = 0; xb < OwnerInfoCount; xb++)
                        OwnerInfos[xb] = new OwnerInfo(SStructure);
                }
                for (int i = 0; i < OwnerInfoCount; i++)
                    OwnerInfos[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}