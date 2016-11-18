#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class Customization : DAInterface<Customization>
    {
        public int DnaSize { get; set; }
        public string FaceCode { get; set; }
        public int FaceCodeVersion { get; set; }
        public int Horns { get; set; }
        public byte[] DnaData { get; set; }
        public int FaceVerticesSize { get; set; }
        public byte[] FaceVerticesBytes { get; set; }
        public int Beard { get; set; }
        public int Hair { get; set; }
        public int Race { get; set; }
        public int Gender { get; set; }
        public int HeadOverrideIndex { get; set; }
       internal int xLength { get; set; }
        public VectorShader VectorShaderParams { get; set; }
        public TextureShader TextureShaderParams { get; set; }
        public BodyShaderHandles BodyShaderHandles { get; set; }
        public BoneOffsetsV1 BoneOffsetsV1 { get; set; }
        public BoneOffsetsV2 BoneOffsetsV2 { get; set; }
        public HeadVariations HeadVariations { get; set; }
        private CustomizationData _data;
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public Customization(CustomizationData data)
        {
            _data = data;
        }

        public Customization Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            HeadOverrideIndex = io.ReadBit2(8);
            Gender = io.ReadBit2(8);
            Race = io.ReadBit2(8);
            Hair = io.ReadBit2(8);
            Beard = io.ReadBit2(8);
            if (_data.Version >= 9)
                Horns = io.ReadBit2(0x8);
            VectorShaderParams = new VectorShader().Read(io);
            TextureShaderParams = new TextureShader().Read(io);
            if (_data.Version >= 0xF)
                BodyShaderHandles = new BodyShaderHandles().Read(io);
            if (_data.Version >= 0xC)
            {
                if (_data.Version < 0xE)
                    BoneOffsetsV1 = new BoneOffsetsV1().Read(io);
                else
                    BoneOffsetsV2 = new BoneOffsetsV2().Read(io);
            }
            if (_data.Version >= 0x9)
            {
                if (_data.Version >= 0xB)
                    HeadVariations = new HeadVariations().Read(io);
                else
                {
                    FaceVerticesSize = io.ReadBit2(0x20);
                    FaceVerticesBytes = new byte[FaceVerticesSize];
                    for (int j = 0; j < FaceVerticesSize; j++)
                        FaceVerticesBytes[j] = (byte) io.ReadBit(0x8);
                }
            }
            else
            {
                FaceCodeVersion = io.ReadBit2(0x20);
                int count = io.ReadBit(0x10);
                FaceCode = "";
                for (int j = 0; j < count; j++)
                    FaceCode += (char) io.ReadBit(0x8);
            }
            if (_data.Version >= 0x10)
            {
                DnaSize = io.ReadBit2(0x20);
                DnaData = new byte[DnaSize];
                for (int j = 0; j < DnaSize; j++)
                {
                    if (j == (DnaSize - 3))
                        Console.WriteLine();
                    DnaData[j] = (byte) io.ReadBit(0x8);
                }
            }
            return this;
        }

        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength)io.WriteBits(Length, LengthBits);
                io.WriteBits(HeadOverrideIndex, 0x8);
                io.WriteBits(Gender, 0x8);
                io.WriteBits(Race, 0x8);
                io.WriteBits(Hair, 0x8);
                io.WriteBits(Beard, 0x8);
                if (_data.Version >= 9)
                    io.WriteBits(Horns, 0x8);
                VectorShaderParams.Write(io);
                TextureShaderParams.Write(io);
                if (_data.Version >= 0xF)
                    BodyShaderHandles.Write(io);
                if (_data.Version >= 0xC)
                {
                    if (_data.Version < 0xE)
                        BoneOffsetsV1.Write(io);
                    else
                        BoneOffsetsV2.Write(io);
                }
                if (_data.Version >= 0x9)
                {
                    if (_data.Version >= 0xB)
                        HeadVariations.Write(io);
                    else
                    {
                        
                        if (FaceVerticesBytes == null)
                            FaceVerticesBytes = new byte[FaceVerticesSize];
                        FaceVerticesSize = FaceVerticesBytes.Length;
                        io.WriteBits(FaceVerticesSize, 0x20);
                        io.Write(FaceVerticesBytes, 0, FaceVerticesSize);
                    }
                }
                else
                {
                    io.WriteBits(FaceCodeVersion, 0x20);
                    io.WriteInt16((short) FaceCode.Length);
                    io.WriteString(FaceCode);
                }
                if (_data.Version >= 0x10)
                {

                    if (DnaData == null)
                        DnaData = new byte[DnaSize];
                    DnaSize = DnaData.Length;
                    io.WriteInt32(DnaSize);
                    for (int j = 0; j < DnaSize; j++)
                    {
                        if (j == (DnaSize - 3))
                            Console.WriteLine();
                        io.WriteBits(DnaData[j], 8);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}