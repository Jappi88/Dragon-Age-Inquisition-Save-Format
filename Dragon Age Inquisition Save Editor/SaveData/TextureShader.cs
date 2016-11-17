#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class TextureShader : DAInterface<TextureShader>
    {
        internal int xLength { get; set; }
        public int ParamCount { get; set; }
        public TextureShaderParam[] TextureShaderParams { get; set; }

        public int Length => this.InstanceLength();

        public TextureShader Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            ParamCount = (short) io.ReadBit2(0x10);
            TextureShaderParams = new TextureShaderParam[ParamCount];
            for (int i = 0; i < ParamCount; i++)
                TextureShaderParams[i] = new TextureShaderParam().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength) io.WriteBits(Length, 0x18);
                io.WriteBits(ParamCount, 0x10);
                if (TextureShaderParams == null)
                {
                    TextureShaderParams = new TextureShaderParam[ParamCount];

                    for (int xb = 0; xb < ParamCount; xb++)
                        TextureShaderParams[xb] = new TextureShaderParam();
                }
                for (int i = 0; i < ParamCount; i++)
                    TextureShaderParams[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}