using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class SaveEntity : DAInterface<SaveEntity>
    {
        private readonly SaveDataStructure xstruct;
        public uint LengthBits => 0;
        public int Length => this.InstanceLength();
        public AgentEntity AgentEntityData { get; set; }
        public int AgentsLookupCount { get; set; }
        public AgentLookup[] AgentLookups { get; set; }

        public SaveEntity(SaveDataStructure xstr)
        {
            xstruct = xstr;
        }

        public SaveEntity Read(DAIIO io)
        {
            AgentEntityData = new AgentEntity(xstruct);
            AgentEntityData.Read(io);
            AgentsLookupCount = io.ReadBit2(0x4);
            AgentLookups = new AgentLookup[AgentsLookupCount];
            for (int i = 0; i < AgentsLookupCount; i++)
                AgentLookups[i] = new AgentLookup().Read(io);
            return this;
        }

        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                AgentEntityData.Write(io);
                if (AgentLookups == null)
                {
                    AgentLookups = new AgentLookup[AgentsLookupCount];

                    for (int xb = 0; xb < AgentsLookupCount; xb++)
                        AgentLookups[xb] = new AgentLookup();
                }
                AgentsLookupCount = AgentLookups.Length;
                io.WriteBits(AgentsLookupCount, 0x4);
                for (int i = 0; i < AgentsLookupCount; i++)
                    AgentLookups[i].Write(io);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
