#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class ControllableCreate : DAInterface<ControllableCreate>
    {
        internal int xLength { get; set; }
        public bool HasControllable { get; set; }
        public Controllable Controllable { get; set; }
        public byte[][] TransForm { get; set; }
        public short IndexUsedByUniqueId { get; set; }
        public uint LengthBits => 0x10;
        public int Length => this.InstanceLength();

        public ControllableCreate Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            HasControllable = io.ReadBoolean();
            if (HasControllable)
            {
                Controllable = new Controllable().Read(io);
                if (Controllable.ShouldSave)
                {
                    TransForm = new byte[4][];
                    for (int i = 0; i < 4; i++)
                    {
                        TransForm[i] = new byte[0xc];
                        for (int j = 0; j < 0xc; j++)
                            TransForm[i][j] = (byte) io.ReadByte();
                    }
                    IndexUsedByUniqueId = io.ReadInt16();
                }
            }
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                io.WriteBoolean(HasControllable);
                if (HasControllable)
                {
                    Controllable.Write(io);
                    if (Controllable.ShouldSave)
                    {
                        if (TransForm == null)
                            TransForm = new byte[4][];
                        for (int i = 0; i < 4; i++)
                        {
                            if (TransForm[i] == null)
                                TransForm[i] = new byte[0xc];
                            for (int j = 0; j < 0xc; j++)
                                io.WriteBits(TransForm[i][j], 0x8);
                        }
                        io.WriteInt16(IndexUsedByUniqueId);
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