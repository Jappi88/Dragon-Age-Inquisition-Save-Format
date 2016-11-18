#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class PlotSaveGame : DAInterface<PlotSaveGame>
    {
        internal int xLength { get; set; }
        public PlotFlagValueMap PlotFlagValueMap { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public PlotSaveGame Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            PlotFlagValueMap = new PlotFlagValueMap();
            PlotFlagValueMap.Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                PlotFlagValueMap.Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}