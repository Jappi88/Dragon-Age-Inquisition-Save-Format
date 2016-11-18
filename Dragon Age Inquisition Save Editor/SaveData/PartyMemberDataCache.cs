#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class PartyMemberDataCache : DAInterface<PartyMemberDataCache>
    {
        public const int Complex = 0;
        public int PartyMemberID { get; set; }
        public int BufferSize { get; set; }
        public byte[] Buffer { get; set; }
        public uint LengthBits => 0;
        public int Length => this.InstanceLength();

        public PartyMemberDataCache Read(DAIIO io)
        {
            PartyMemberID = io.ReadInt32();
            BufferSize = io.ReadInt32();
            Buffer = new byte[BufferSize];
            io.Read(Buffer, 0, BufferSize);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                io.WriteInt32(PartyMemberID);
                io.WriteInt32(Buffer.Length);
                if (Buffer == null)
                    Buffer = new byte[BufferSize];
                io.Write(Buffer, 0, Buffer.Length);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}