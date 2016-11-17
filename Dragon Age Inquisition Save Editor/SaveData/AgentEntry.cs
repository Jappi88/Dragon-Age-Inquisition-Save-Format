#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class AgentEntry : DAInterface<AgentEntry>
    {
        public short Entry { get; set; }
        public int NameHash { get; set; }
        public short Index { get; set; }

        public int Length => this.InstanceLength();

        public AgentEntry Read(DAIIO io)
        {
            NameHash = io.ReadInt32();
            Index = (short) io.ReadBit2(0x8);
            Entry = (short) io.ReadBit2(0x10);
            return this;
        }

        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                io.WriteInt32(NameHash);
                io.WriteBits((uint) Index, 8);
                io.WriteInt16(Entry);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}