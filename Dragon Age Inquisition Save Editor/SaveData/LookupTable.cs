#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class LookupTable : DAInterface<LookupTable>
    {
        public int Id { get; set; }
        public bool HasCreatePos { get; set; }
        public int CreatePos { get; set; }
        public bool HasInitPos { get; set; }
        public int InitPos { get; set; }
        public uint LengthBits => 0;
        public int Length => this.InstanceLength();

        public LookupTable Read(DAIIO io)
        {
            Id = io.ReadBit2(0x20);
            HasCreatePos = io.ReadBoolean();
            if (HasCreatePos)
                CreatePos = io.ReadBit2(0x1A);
            HasInitPos = io.ReadBoolean();
            if (HasInitPos)
                InitPos = io.ReadBit2(0x1A);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                io.WriteBits(Id, 0x20);
                io.WriteBoolean(HasCreatePos);
                if (HasCreatePos)
                    io.WriteBits(CreatePos, 0x1A);
                io.WriteBoolean(HasInitPos);
                if (HasInitPos)
                    io.WriteBits(InitPos, 0x1A);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}