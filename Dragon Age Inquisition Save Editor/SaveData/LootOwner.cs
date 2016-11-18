#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class LootOwner : DAInterface<LootOwner>
    {
       internal int xLength { get; set; }
        public BluePrint BluePrint { get; set; }
        public int IndexUsedByUniqueId { get; set; }
        public byte[][] TransForm { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public LootOwner Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            BluePrint = new BluePrint().Read(io);
            IndexUsedByUniqueId = io.ReadInt32();
            TransForm = new byte[4][];
            for (int i = 0; i < 4; i++)
            {
                TransForm[i] = new byte[0xc];
                for (int j = 0; j < 0xc; j++)
                    TransForm[i][j] = (byte) io.ReadByte();
            }
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength)io.WriteBits(Length, LengthBits);
                BluePrint.Write(io);
                io.WriteInt32(IndexUsedByUniqueId);
                if (TransForm == null)
                    TransForm = new byte[4][];
                for (int i = 0; i < 4; i++)
                {
                    if (TransForm[i] == null)
                        TransForm[i] = new byte[0xc];
                    for (int j = 0; j < 0xc; j++)
                        io.WriteBits(TransForm[i][j], 0x8);
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