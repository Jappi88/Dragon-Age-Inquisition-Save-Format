#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class StateHistory : DAInterface<StateHistory>
    {
        internal int xLength { get; set; }
        public string Name { get; set; }
        public byte[] Hash { get; set; }
        public byte[] Unknown { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public StateHistory Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            Name = "";
            var xpos = io.Position;
            var count = (ushort) io.ReadBit(0x10);
            if (count == 0)
                Unknown = io.ReadData(xLength - 0x10);
            else
            {
                Name = io.ReadString(count);
                count = (ushort) io.ReadBit(0x10);
                var xl = io.Position - xpos;
                Hash = new byte[count];
                if (count == 0)
                    Unknown = io.ReadData((int) (xLength - xl));
                else
                    io.Read(Hash, 0, count);
                xl = io.Position - xpos;
                if (xl < xLength)
                {
                    //var x1 = io.ReadBit2(0x18);
                    var d = io.ReadData((int) (xLength - xl));
                    Console.WriteLine(BitConverter.ToString(d).Replace("-", "") + @" ==== > ");
                }
            }

            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                io.WriteBits(Name.Length, 0x10);
                if (Name.Length > 0)
                {
                    io.WriteString(Name);
                    if (Hash == null)
                        Hash = new byte[0];
                    io.WriteBits(Hash.Length, 0x10);
                    if (Hash.Length > 0)
                        io.Write(Hash, 0, Hash.Length);
                    else if (Unknown != null)
                        io.Write(Unknown, 0, Unknown.Length);
                }
                else if (Unknown != null)
                    io.Write(Unknown, 0, Unknown.Length);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}