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

        public int Length => this.InstanceLength();

        public PlotSaveGame Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            PlotFlagValueMap = new PlotFlagValueMap();
            PlotFlagValueMap.Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength) io.WriteBits(Length, 0x18);
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