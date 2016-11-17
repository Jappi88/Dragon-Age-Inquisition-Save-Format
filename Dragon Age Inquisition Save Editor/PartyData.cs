using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;
using Dragon_Age_Inquisition_Save_Editor.SaveData;

namespace Dragon_Age_Inquisition_Save_Editor
{
   public class PartyData : DAInterface<PartyData>
   {
       public int GetLength(bool skiplength)
       {
           throw new NotImplementedException();
       }

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
           xLength = io.ReadBit2(0x18);
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
               if (!skiplength) io.WriteBits(Length, 0x18);
               io.WriteInt16(ArcheTypeCount);
               if (ArcheTypes == null)
               {
                   ArcheTypes = new ArcheType[ArcheTypeCount];

                   for (int xb = 0; xb < ArcheTypeCount; xb++)
                       ArcheTypes[xb] = new ArcheType();
               }
               for (int i = 0; i < ArcheTypeCount; i++)
                   ArcheTypes[i].Write(io);
               io.WriteInt16(DesiredPartyMemberIDCount);
               if (DesiredPartyMemberIDs == null)
               {
                   DesiredPartyMemberIDs = new int[DesiredPartyMemberIDCount];
               }
               for (int i = 0; i < DesiredPartyMemberIDCount; i++)
                   io.WriteInt32(DesiredPartyMemberIDs[i]);
               io.WriteInt16(OverridePartyMemberIDCount);
               if (OverridePartyMemberIDs == null)
               {
                   OverridePartyMemberIDs = new int[OverridePartyMemberIDCount];
               }
               for (int i = 0; i < OverridePartyMemberIDCount; i++)
                   io.WriteInt32(OverridePartyMemberIDs[i]);
               io.WriteInt16(PartyMemberCount);
               if (PartyMembers == null)
               {
                   PartyMembers = new PartyMember[PartyMemberCount];

                   for (int xb = 0; xb < PartyMemberCount; xb++)
                       PartyMembers[xb] = new PartyMember();
               }
               for (int i = 0; i < PartyMemberCount; i++)
                   PartyMembers[i].Write(io);
               return true;
           }
           catch (Exception)
           {
               return false;
           }
       }
   }
}
