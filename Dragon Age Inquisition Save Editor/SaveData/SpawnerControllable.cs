#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class SpawnerControllable : DAInterface<SpawnerControllable>
    {
        public short xLength { get; set; }
        public bool IsInVehicle { get; set; }
        public InVehicleControllable InVehicleControllable { get; set; }
        public uint LengthBits => 0x10;
        public int Length => this.InstanceLength();

        public SpawnerControllable Read(DAIIO io)
        {
            xLength = (short) io.ReadBit2(LengthBits);
            IsInVehicle = io.ReadBoolean();
            if (IsInVehicle)
            {
                InVehicleControllable = new InVehicleControllable
                {
                    ID = io.ReadInt32(),
                    EntryID = io.ReadInt32()
                };
            }
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                io.WriteBoolean(IsInVehicle);
                if (IsInVehicle)
                {
                    io.WriteInt32(InVehicleControllable.ID);
                    io.WriteInt32(InVehicleControllable.EntryID);
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