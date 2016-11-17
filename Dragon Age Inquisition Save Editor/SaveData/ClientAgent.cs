#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class ClientAgent : DAInterface<ClientAgent>
    {
        internal int xLength { get; set; }
        public short Version { get; set; }
        internal short AgentCount { get; set; }
        public AgentEntryData[] Agents { get; set; }

        public int Length => this.InstanceLength();

        public ClientAgent Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x20);
            Version = (short) io.ReadBit2(0x10);
            if (Version == 1)
            {
                AgentCount = io.ReadInt16();
                Agents = new AgentEntryData[AgentCount];
                for (int i = 0; i < AgentCount; i++)
                    Agents[i] = new AgentEntryData().Read(io);
            }
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, 0x20);
                io.WriteBits(Version, 0x10);
                if (Version == 1)
                {
                    if (Agents == null)
                    {
                        Agents = new AgentEntryData[AgentCount];

                        for (int xb = 0; xb < AgentCount; xb++)
                            Agents[xb] = new AgentEntryData();
                    }
                    io.WriteInt16((short) Agents.Length);
                    for (int i = 0; i < Agents.Length; i++)
                        Agents[i].Write(io);
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