#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class TextureShaderParam : DAInterface<TextureShaderParam>
    {
        public int TextureHandle { get; set; }
        public int TSParameterHandle { get; set; }
       internal int xLength { get; set; }

        public int Length => this.InstanceLength();

        public TextureShaderParam Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            TSParameterHandle = io.ReadBit2(0x20);
            TextureHandle = io.ReadBit2(0x20);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength)io.WriteBits(Length, 0x18);
                io.WriteBits(TSParameterHandle, 0x20);
                io.WriteBits(TextureHandle, 0x20);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}