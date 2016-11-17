#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class AgentLookup : DAInterface<AgentLookup>
    {
        public int[] Agent { get; set; }
        public short Index { get; set; }

        public int Length => this.InstanceLength();

        public AgentLookup Read(DAIIO io)
        {
            Agent = new int[5];
            Index = (short) io.ReadBit2(0x8);
            for (int j = 0; j < 5; j++)
                Agent[j] = io.ReadBit2(0x1A);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (Agent == null)
                    Agent = new int[5];
                io.WriteBits(Index, 0x8);
                for (int j = 0; j < 5; j++)
                    io.WriteBits(Agent[j], 0x1A);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}