#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class AgentEntryData : DAInterface<AgentEntryData>
    {
        public short Version { get; set; }
        internal int xLength { get; set; }
        public int NameHash { get; set; }
        public bool HasCustomizationData { get; set; }
        public CustomizationData CustomizationData { get; set; }

        public int Length => this.InstanceLength();

        public AgentEntryData Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            NameHash = io.ReadBit2(0x20);
            Version = (short) io.ReadBit2(0x10);
            HasCustomizationData = io.ReadBoolean();
            if (HasCustomizationData)
                CustomizationData = new CustomizationData().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength) io.WriteBits(Length, 0x18);
                io.WriteBits(NameHash, 0x20);
                io.WriteBits(Version, 0x10);
                io.WriteBoolean(HasCustomizationData);
                if (HasCustomizationData)
                    CustomizationData.Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}