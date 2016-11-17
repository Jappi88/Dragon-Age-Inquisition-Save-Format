using Dragon_Age_Inquisition_Save_Editor.DAIO;

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class AgentToc : DAInterface<AgentToc>
    {
        internal int xLength { get; set; }
        public int Length => this.InstanceLength();

        public int AgentCount { get; set; }
        public AgentEntry[] Agents { get; set; }

        public AgentToc Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            AgentCount = io.ReadBit2(0x4);
            Agents = new AgentEntry[AgentCount];
            for (int i = 0; i < AgentCount; i++)
                Agents[i] = new AgentEntry().Read(io);
            return this;
        }

        public bool Write(DAIIO io, bool skiplength = false)
        {
            if (!skiplength) io.WriteBits(Length, 0x18);
            if (Agents == null)
            {
                Agents = new AgentEntry[AgentCount];

                for (int xb = 0; xb < AgentCount; xb++)
                    Agents[xb] = new AgentEntry();
            }
            AgentCount = Agents.Length;
            io.WriteBits(AgentCount, 0x4);
            for (int i = 0; i < AgentCount; i++)
                Agents[i].Write(io);
            return true;
        }
    }
}
