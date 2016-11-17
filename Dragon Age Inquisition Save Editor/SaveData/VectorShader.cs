#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class VectorShader : DAInterface<VectorShader>
    {
        internal int xLength { get; set; }
        internal short ParamCount { get; set; }
        public VectorShaderParam[] VectorShaderParams { get; set; }

        public int Length => this.InstanceLength();

        public VectorShader Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            ParamCount = (short) io.ReadBit2(0x10);
            VectorShaderParams = new VectorShaderParam[ParamCount];
            for (int i = 0; i < ParamCount; i++)
                VectorShaderParams[i] = new VectorShaderParam().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength) io.WriteBits(Length, 0x18);
                io.WriteBits(ParamCount, 0x10);
                if (VectorShaderParams == null)
                {
                    VectorShaderParams = new VectorShaderParam[ParamCount];

                    for (int xb = 0; xb < ParamCount; xb++)
                        VectorShaderParams[xb] = new VectorShaderParam();
                }
                for (int i = 0; i < ParamCount; i++)
                    VectorShaderParams[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}