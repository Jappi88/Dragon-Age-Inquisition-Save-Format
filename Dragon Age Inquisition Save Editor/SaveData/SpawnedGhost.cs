#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class SpawnedGhost : DAInterface<SpawnedGhost>
    {
        public short Complex { get; set; }
        public uint LengthBits => 0;
        public int Length => this.InstanceLength();


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                io.WriteBits(Complex, 0xA);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public SpawnedGhost Read(DAIIO io)
        {
            Complex = (short) io.ReadBit2(0xA);
            return this;
        }
    }
}