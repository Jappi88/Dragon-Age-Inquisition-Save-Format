#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class Inclusion : DAInterface<Inclusion>
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public uint LengthBits => 0;
        public int Length => this.InstanceLength();

        public Inclusion Read(DAIIO io)
        {
            ID = io.ReadInt24();
            var count = io.ReadUInt16();
            Name = io.ReadString(count);
            count = io.ReadUInt16();
            Value = io.ReadString(count);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                io.WriteInt24(ID);
                io.WriteInt16((short) Name.Length);
                io.WriteString(Name);
                io.WriteInt16((short) Value.Length);
                io.WriteString(Value);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}