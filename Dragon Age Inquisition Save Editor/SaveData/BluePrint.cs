#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class BluePrint : DAInterface<BluePrint>
    {
        public bool IsNull { get; set; }
        public int Value1 { get; set; }
        public int Value2 { get; set; }
        public bool IsEmpty { get; set; }
        public byte[] DataBytes { get; set; }
        public uint LengthBits => 0;
        public int Length => this.InstanceLength();

        public BluePrint Read(DAIIO io)
        {
            IsNull = io.ReadBoolean();
            if (!IsNull)
            {
                Value1 = io.ReadInt32();
                Value2 = io.ReadInt32();
                IsEmpty = io.ReadBoolean();
                if (!IsEmpty)
                {
                    DataBytes = new byte[0x10];
                    for (int i = 0; i < 0x10; i++)
                        DataBytes[i] = (byte) io.ReadByte();
                }
            }
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                io.WriteBoolean(IsNull);
                if (!IsNull)
                {
                    io.WriteInt32(Value1);
                    io.WriteInt32(Value2);
                    io.WriteBoolean(IsEmpty);
                    if (!IsEmpty)
                    {
                        if (DataBytes == null)
                            DataBytes = new byte[0x10];
                        for (int i = 0; i < 0x10; i++)
                            io.WriteBits(DataBytes[i], 0x8);
                    }
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