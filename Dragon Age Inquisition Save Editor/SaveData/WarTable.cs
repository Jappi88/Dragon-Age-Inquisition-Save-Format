using Dragon_Age_Inquisition_Save_Editor.DAIO;

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class WarTable : DAInterface<WarTable>
    {
        internal int xLength { get; set; }
        public int ID { get; set; }
        public bool HasNoGuid { get; set; }
        public byte[] Guid { get; set; }
        public uint LengthBits => 0x20;
        public int Length => this.InstanceLength();

        public WarTable Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            ID = io.ReadInt32();
            HasNoGuid = io.ReadBoolean();
            if (!HasNoGuid)
            {
                Guid = new byte[0x10];
                io.Read(Guid, 0, 0x10);
            }
            return this;
        }

        public bool Write(DAIIO io, bool skiplength = false)
        {
            if (!skiplength) io.WriteBits(Length, LengthBits);
            io.WriteInt32(ID);
            io.WriteBoolean(HasNoGuid);
            if (!HasNoGuid)
            {
                if (Guid == null)
                    Guid = new byte[0x10];
                io.Write(Guid, 0, 0X10);
            }
            return true;
        }
    }
}
