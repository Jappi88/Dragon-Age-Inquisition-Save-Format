using Dragon_Age_Inquisition_Save_Editor.DAIO;

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class WarTable : DAInterface<WarTable>
    {
        internal int xLength { get; set; }
        public int ID { get; set; }
        public bool HasNoGuid { get; set; }
        public byte[] Guid { get; set; }
        public int Length => this.InstanceLength();

        public WarTable Read(DAIIO io)
        {
            xLength = io.ReadInt32();
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
            if (!skiplength) io.WriteInt32(Length);
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
