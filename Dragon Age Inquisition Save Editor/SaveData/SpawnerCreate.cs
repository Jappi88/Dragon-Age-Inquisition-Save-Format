#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class SpawnerCreate : DAInterface<SpawnerCreate>
    {
        internal int xLength { get; set; }
        public int ID { get; set; }
        public short ControllablesCount { get; set; }
        public ControllableCreate[] Controllables { get; set; }
        public uint LengthBits => 0x14;
        public int Length => this.InstanceLength();

        public SpawnerCreate Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            ID = io.ReadBit2(0x20);
            ControllablesCount = io.ReadInt16();
            Controllables = new ControllableCreate[ControllablesCount];
            for (int i = 0; i < ControllablesCount; i++)
                Controllables[i] = new ControllableCreate().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                io.WriteBits(ID, 0x20);
                if (Controllables == null)
                {
                    Controllables = new ControllableCreate[ControllablesCount];

                    for (int xb = 0; xb < ControllablesCount; xb++)
                        Controllables[xb] = new ControllableCreate();
                }
                ControllablesCount = (short) Controllables.Length;
                io.WriteInt16(ControllablesCount);
                for (int i = 0; i < ControllablesCount; i++)
                    Controllables[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}