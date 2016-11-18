#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class HeadVariation : DAInterface<HeadVariation>
    {
        internal int xLength { get; set; }
        public byte Index { get; set; }
        public int FaceVerticesSize { get; set; }
        public byte[] FaceVerticesBytes { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public HeadVariation Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            Index = (byte) io.ReadBit2(0x8);
            FaceVerticesSize = io.ReadBit2(0x20);
            FaceVerticesBytes = new byte[FaceVerticesSize];
            for (int j = 0; j < FaceVerticesSize; j++)
                FaceVerticesBytes[j] = (byte) io.ReadBit(0x8);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength)io.WriteBits(Length, LengthBits);
                io.WriteBits(Index, 0x8);
                if (FaceVerticesBytes == null)
                    FaceVerticesBytes = new byte[FaceVerticesSize];
                FaceVerticesSize = FaceVerticesBytes.Length;
                io.WriteBits(FaceVerticesSize, 0x20);
                io.Write(FaceVerticesBytes,0,FaceVerticesSize);
                ////for (int j = 0; j < FaceVerticesSize; j++)
                ////    io.WriteBits(FaceVerticesBytes[j], 0x8);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}