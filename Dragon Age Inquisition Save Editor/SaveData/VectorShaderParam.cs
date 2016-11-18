#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class VectorShaderParam : DAInterface<VectorShaderParam>
    {
        public int VSParameterHandle { get; set; }
       internal int xLength { get; set; }
        public byte[] Value { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public VectorShaderParam Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            VSParameterHandle = io.ReadBit2(0x20);
            Value = new byte[0x10];
            for (int j = 0; j < 0x10; j++)
                Value[j] = (byte) io.ReadBit(0x8);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength)io.WriteBits(Length, LengthBits);
                io.WriteBits(VSParameterHandle, 0x20);
                if (Value == null)
                    Value = new byte[0x10];
                for (int j = 0; j < 0x10; j++)
                    io.WriteBits(Value[j], 0x8);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}