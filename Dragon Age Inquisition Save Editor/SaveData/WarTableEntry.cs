#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class WarTableEntry : DAInterface<WarTableEntry>
    {
        public WarTableEntry(SaveDataStructure xstruc, bool isfirst)
        {
            SStructure = xstruc;
            _isfirst = isfirst;
        }

        internal SaveDataStructure SStructure { get; private set; }
        internal int xLength { get; set; }
        public bool IsEmpty { get; set; }
        public DateTime ProgressTime { get; set; }
        public WarTable WarTable { get; set; }
        private readonly bool _isfirst;

        public int Length => this.InstanceLength();

        public WarTableEntry Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            IsEmpty = io.ReadBoolean();
            if (!IsEmpty)
                WarTable = new WarTable().Read(io);
            if (_isfirst)
            {
                if (SStructure.ProjectVersion < 0x1A)
                    ProgressTime = io.ReadInt32().ToUnixTime();
                else ProgressTime = ((int) io.ReadInt64()).ToUnixTime();
            }
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength)
                    io.WriteBits(Length, 0x18);
                io.WriteBoolean(IsEmpty);
                if (!IsEmpty)
                {
                    if (WarTable == null)
                        WarTable = new WarTable();
                    WarTable.Write(io);
                }
                if (_isfirst)
                {
                    if (SStructure.ProjectVersion < 0x1A)
                        io.WriteInt32(ProgressTime.ToUnixSecondsNoAdd());
                    else
                        io.WriteInt64(ProgressTime.ToUnixSecondsNoAdd());
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