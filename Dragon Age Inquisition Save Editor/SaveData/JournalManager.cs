#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class JournalManager : DAInterface<JournalManager>
    {
        internal int xLength { get; set; }
        public short Version { get; set; }
        public short ActiveJournalCount { get; set; }
        public int[] ActiveJournals { get; set; }
        public short ReadJournalCount { get; set; }
        public int[] ReadJournals { get; set; }
        public short RewardedJournalCount { get; set; }
        public int[] RewardedJournals { get; set; }

        public int Length => this.InstanceLength();

        public JournalManager Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            Version = io.ReadInt16();
            if (Version > 0)
            {
                ActiveJournalCount = io.ReadInt16();
                ActiveJournals = new int[ActiveJournalCount];
                for (int i = 0; i < ActiveJournalCount; i++)
                    ActiveJournals[i] = io.ReadInt32();

                ReadJournalCount = io.ReadInt16();
                ReadJournals = new int[ReadJournalCount];
                for (int i = 0; i < ReadJournalCount; i++)
                    ReadJournals[i] = io.ReadInt32();
            }
            if (Version > 1)
            {
                RewardedJournalCount = io.ReadInt16();
                RewardedJournals = new int[RewardedJournalCount];
                for (int i = 0; i < RewardedJournalCount; i++)
                    RewardedJournals[i] = io.ReadInt32();
            }
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength) io.WriteBits(Length, 0x18);
                io.WriteInt16(Version);
                if (Version > 0)
                {
                    io.WriteInt16(ActiveJournalCount);
                    if (ActiveJournals == null)
                    {
                        ActiveJournals = new int[ActiveJournalCount];
                    }
                    for (int i = 0; i < ActiveJournalCount; i++)
                        io.WriteInt32(ActiveJournals[i]);
                    io.WriteInt16(ReadJournalCount);
                    if (ReadJournals == null)
                    {
                        ReadJournals = new int[ReadJournalCount];
                    }
                    for (int i = 0; i < ReadJournalCount; i++)
                        io.WriteInt32(ReadJournals[i]);
                }
                if (Version > 1)
                {
                    io.WriteInt16(RewardedJournalCount);
                    if (RewardedJournals == null)
                    {
                        RewardedJournals = new int[RewardedJournalCount];
                    }
                    for (int i = 0; i < RewardedJournalCount; i++)
                        io.WriteInt32(RewardedJournals[i]);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}