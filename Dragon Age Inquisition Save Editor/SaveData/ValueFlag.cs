#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class ValueFlag : DAInterface<ValueFlag>
    {
       internal int xLength { get; set; }
        public byte[] Guid { get; set; }
        public int Value { get; set; }

        public int Length => this.InstanceLength();


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (Guid == null)
                    Guid = new byte[0x10];
                for (int j = 0; j < 0x10; j++)
                    io.WriteBits(Guid[j], 0x8);
                io.WriteInt32(Value);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ValueFlag Read(DAIIO io)
        {
            Guid = new byte[0x10];
            for (int j = 0; j < 0x10; j++)
                Guid[j] = (byte) io.ReadBit(0x8);
            Value = io.ReadInt32();
            return this;
        }
    }
}