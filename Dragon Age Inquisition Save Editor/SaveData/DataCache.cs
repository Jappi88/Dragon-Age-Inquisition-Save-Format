#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class DataCache : DAInterface<DataCache>
    {
        internal int xLength { get; set; }
        public DA3PartyMemberDataCache DA3PartyMemberDataCache { get; set; }

        public int Length => this.InstanceLength();

        public DataCache Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            DA3PartyMemberDataCache = new DA3PartyMemberDataCache().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength) io.WriteBits(Length, 0x18);
                DA3PartyMemberDataCache.Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}