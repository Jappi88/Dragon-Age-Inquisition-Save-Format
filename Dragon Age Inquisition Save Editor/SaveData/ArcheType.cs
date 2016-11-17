#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class ArcheType : DAInterface<ArcheType>
    {
       internal int xLength { get; set; }
        public int ArcheTypeID { get; set; }
        public bool Enabled { get; set; }
        public bool Unlocked { get; set; }

        public int Length => this.InstanceLength();

        public ArcheType Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            ArcheTypeID = io.ReadInt32();
            Enabled = io.ReadBoolean();
            Unlocked = io.ReadBoolean();
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength)io.WriteBits(Length, 0x18);
                io.WriteInt32(ArcheTypeID);
                io.WriteBoolean(Enabled);
                io.WriteBoolean(Unlocked);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}