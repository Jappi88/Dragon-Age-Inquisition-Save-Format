#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class PartyMember : DAInterface<PartyMember>
    {
       internal int xLength { get; set; }
        public int PartyMemberID { get; set; }
        public byte FixedIndex { get; set; }
        public int Player { get; set; }
        public bool IsLeader { get; set; }
        public bool IsUnspawned { get; set; }

        public int Length => this.InstanceLength();

        public PartyMember Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            PartyMemberID = io.ReadInt32();
            FixedIndex = (byte) io.ReadBit2(8);
            Player = io.ReadInt32();
            IsLeader = io.ReadBoolean();
            IsUnspawned = io.ReadBoolean();
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength)io.WriteBits(Length, 0x18);
                io.WriteInt32(PartyMemberID);
                io.WriteBits(FixedIndex, 8);
                io.WriteInt32(Player);
                io.WriteBoolean(IsLeader);
                io.WriteBoolean(IsUnspawned);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}