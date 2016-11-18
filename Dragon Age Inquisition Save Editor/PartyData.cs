using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;
using Dragon_Age_Inquisition_Save_Editor.SaveData;

namespace Dragon_Age_Inquisition_Save_Editor
{
   public class PartyData : DAInterface<PartyData>
   {
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

       internal int xLength { get; set; }
       internal short ArcheTypeCount { get; set; }
       public ArcheType[] ArcheTypes { get; set; }
       internal short DesiredPartyMemberIDCount { get; set; }
       public int[] DesiredPartyMemberIDs { get; set; }
       internal short OverridePartyMemberIDCount { get; set; }
       public int[] OverridePartyMemberIDs { get; set; }
       internal short PartyMemberCount { get; set; }
       public PartyMember[] PartyMembers { get; set; }

       public PartyData Read(DAIIO io)
       {
           xLength = io.ReadBit2(LengthBits);
           ArcheTypeCount = io.ReadInt16();
           ArcheTypes = new ArcheType[ArcheTypeCount];
           for (int i = 0; i < ArcheTypeCount; i++)
               ArcheTypes[i] = new ArcheType().Read(io);
           DesiredPartyMemberIDCount = io.ReadInt16();
           DesiredPartyMemberIDs = new int[DesiredPartyMemberIDCount];
           for (int i = 0; i < DesiredPartyMemberIDCount; i++)
               DesiredPartyMemberIDs[i] = io.ReadInt32();
           OverridePartyMemberIDCount = io.ReadInt16();
           OverridePartyMemberIDs = new int[OverridePartyMemberIDCount];
           for (int i = 0; i < OverridePartyMemberIDCount; i++)
               OverridePartyMemberIDs[i] = io.ReadInt32();

           PartyMemberCount = io.ReadInt16();
           PartyMembers = new PartyMember[PartyMemberCount];
           for (int i = 0; i < PartyMemberCount; i++)
               PartyMembers[i] = new PartyMember().Read(io);
           return this;
       }

       public bool Write(DAIIO io, bool skiplength = false)
       {
           try
           {
               if (!skiplength) io.WriteBits(Length, LengthBits);
               
               if (ArcheTypes == null)
               {
                   ArcheTypes = new ArcheType[ArcheTypeCount];

                   for (int xb = 0; xb < ArcheTypeCount; xb++)
                       ArcheTypes[xb] = new ArcheType();
               }
                io.WriteInt16((short) ArcheTypes.Length);
                foreach (ArcheType t in ArcheTypes)
                   t.Write(io);
                
               if (DesiredPartyMemberIDs == null)
               {
                   DesiredPartyMemberIDs = new int[DesiredPartyMemberIDCount];
               }
                io.WriteInt16((short) DesiredPartyMemberIDs.Length);
                foreach (int t in DesiredPartyMemberIDs)
                    io.WriteInt32(t);
              
               if (OverridePartyMemberIDs == null)
               {
                   OverridePartyMemberIDs = new int[OverridePartyMemberIDCount];
               }
                io.WriteInt16((short) OverridePartyMemberIDs.Length);
                foreach (int t in OverridePartyMemberIDs)
                    io.WriteInt32(t);
               
               if (PartyMembers == null)
               {
                   PartyMembers = new PartyMember[PartyMemberCount];

                   for (int xb = 0; xb < PartyMemberCount; xb++)
                       PartyMembers[xb] = new PartyMember();
               }
                io.WriteInt16((short) PartyMembers.Length);
                foreach (PartyMember t in PartyMembers)
                    t.Write(io);
               return true;
           }
           catch (Exception)
           {
               return false;
           }
       }
   }
}
