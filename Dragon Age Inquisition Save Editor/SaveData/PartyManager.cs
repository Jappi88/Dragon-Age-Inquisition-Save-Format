#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class PartyManager : DAInterface<PartyManager>
    {
        internal int xLength { get; set; }
        public PartyData PartyData { get; set; }

        public int Length => this.InstanceLength();

        public PartyManager Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            PartyData = new PartyData().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength) io.WriteBits(Length, 0x18);
                if (PartyData == null)
                    PartyData = new PartyData();
                PartyData.Write(io);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}