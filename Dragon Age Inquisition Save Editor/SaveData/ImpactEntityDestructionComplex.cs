#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class ImpactEntityDestructionComplex : DAInterface<ImpactEntityDestructionComplex>
    {
        public ImpactEntityDestructionComplex(SaveDataStructure xstruct)
        {
            SStructure = xstruct;
        }

        internal SaveDataStructure SStructure { get; private set; }
        internal int xLength { get; set; }
        public byte PositionPrecision { get; set; }
        public short PositionCompressionArray { get; set; }
        public byte[] PositionCompression { get; set; }
        public byte ImpactPrecision { get; set; }
        public int ImpactScale { get; set; }
        public int NextCallbackId { get; set; }
        public MasterInfo[] MasterInfoArray { get; set; }
        public int LastDestructionId { get; set; }

        public int Length => this.InstanceLength();

        public ImpactEntityDestructionComplex Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            PositionPrecision = (byte) io.ReadBit2(0x8);
            PositionCompressionArray = io.ReadInt16();
            PositionCompression = new byte[PositionCompressionArray];
            for (int i = 0; i < PositionCompressionArray; i++)
                PositionCompression[i] = (byte) io.ReadBit2(0x8);
            ImpactPrecision = (byte) io.ReadBit2(8);
            ImpactScale = io.ReadInt32();
            NextCallbackId = io.ReadInt32();
            MasterInfoArray = new MasterInfo[NextCallbackId];
            for (int i = 0; i < NextCallbackId; i++)
                MasterInfoArray[i] = new MasterInfo().Read(io);
            LastDestructionId = io.ReadInt32();
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength) io.WriteBits(Length, 0x18);
                io.WriteBits(PositionPrecision, 0x8);
                io.WriteInt16(PositionCompressionArray);
                if (PositionCompression == null)
                    PositionCompression = new byte[PositionCompressionArray];
                for (int i = 0; i < PositionCompressionArray; i++)
                    io.WriteBits(PositionCompression[i], 0x8);
                io.WriteBits(ImpactPrecision, 8);
                io.WriteInt32(ImpactScale);
                io.WriteInt32(NextCallbackId);
                if (MasterInfoArray == null)
                {
                    MasterInfoArray = new MasterInfo[NextCallbackId];

                    for (int xb = 0; xb < NextCallbackId; xb++)
                        MasterInfoArray[xb] = new MasterInfo();
                }
                for (int i = 0; i < NextCallbackId; i++)
                    MasterInfoArray[i].Write(io);
                io.WriteInt32(LastDestructionId);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}