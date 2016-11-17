#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class DynamicMapPinInfo : DAInterface<DynamicMapPinInfo>
    {
        internal short xLength { get; set; }
        public byte[] PinGuid { get; set; }
        public byte[] WorldPos { get; set; }
        public short FloorId { get; set; }
        public bool VisibilityOverridden { get; set; }
        public bool Visible { get; set; }
        public bool DisplayAsDiscovered { get; set; }
        public bool DisplayAsExplored { get; set; }
        public bool DeadOrDying { get; set; }

        public int Length => this.InstanceLength();

        public DynamicMapPinInfo Read(DAIIO io)
        {
            xLength = (short) io.ReadBit2(0xA);
            PinGuid = new byte[0x10];
            io.Read(PinGuid, 0, 0x10);
            WorldPos = new byte[0xc];
            io.Read(WorldPos, 0, 0xc);
            FloorId = io.ReadInt16();
            VisibilityOverridden = io.ReadBoolean();
            if (VisibilityOverridden)
                Visible = io.ReadBoolean();
            DisplayAsDiscovered = io.ReadBoolean();
            DisplayAsExplored = io.ReadBoolean();
            DeadOrDying = io.ReadBoolean();
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength) io.WriteBits(Length, 0xA);
                if (PinGuid == null)
                    PinGuid = new byte[0x10];
                io.Write(PinGuid, 0, 0x10);
                if (WorldPos == null)
                    WorldPos = new byte[0xc];
                io.Write(WorldPos, 0, 0xC);
                io.WriteInt16(FloorId);
                io.WriteBoolean(VisibilityOverridden);
                if (VisibilityOverridden)
                    io.WriteBoolean(Visible);
                io.WriteBoolean(DisplayAsDiscovered);
                io.WriteBoolean(DisplayAsExplored);
                io.WriteBoolean(DeadOrDying);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}