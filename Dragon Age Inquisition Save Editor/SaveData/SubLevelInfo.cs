#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class SubLevelInfo : DAInterface<SubLevelInfo>
    {
        public int GroupID { get; set; }
        public int BundleType { get; set; }
        public int Status { get; set; }
        public bool AllowGrow { get; set; }
        public int InitSize { get; set; }
        public int HeapType { get; set; }
        public int ParentBundleHash { get; set; }
        public int BundleHash { get; set; }
        public string Name { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();
        internal int XLength { get; private set; }

        public SubLevelInfo Read(DAIIO io)
        {
            XLength = io.ReadBit2(LengthBits);
            var count = (ushort) io.ReadBit2(0x10);
            Name = io.ReadString(count);
            BundleHash = io.ReadBit2(0x20);
            ParentBundleHash = io.ReadBit2(0x20);
            HeapType = io.ReadBit2(0x8);
            InitSize = io.ReadBit2(0x20);
            AllowGrow = io.ReadBoolean();
            Status = io.ReadBit2(0x8);
            BundleType = io.ReadBit2(0x8);
            GroupID = io.ReadBit2(0x20);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength) io.WriteBits(Length, LengthBits);
                io.WriteBits(Name.Length, 0x10);
                io.WriteString(Name);
                io.WriteBits(BundleHash, 0x20);
                io.WriteBits(ParentBundleHash, 0x20);
                io.WriteBits(HeapType, 0x8);
                io.WriteBits(InitSize, 0x20);
                io.WriteBoolean(AllowGrow);
                io.WriteBits(Status, 0x8);
                io.WriteBits(BundleType, 0x8);
                io.WriteBits(GroupID, 0x20);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}