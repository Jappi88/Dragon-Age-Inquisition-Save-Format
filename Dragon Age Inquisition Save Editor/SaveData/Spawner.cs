#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class Spawner : DAInterface<Spawner>
    {
        internal int xLength { get; set; }
        public int ID { get; private set; }
        public short ControllableCount { get; set; }
        public SpawnerControllable[] Controllables { get; set; }
        public uint LengthBits => 0x14;
        public int Length => this.InstanceLength();

        public Spawner Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            ID = io.ReadInt32();
            ControllableCount = (short) io.ReadBit2(0xA);
            Controllables = new SpawnerControllable[ControllableCount];
            for (int i = 0; i < ControllableCount; i++)
                Controllables[i] = new SpawnerControllable().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                io.WriteInt32(ID);
                if (Controllables == null)
                {
                    Controllables = new SpawnerControllable[ControllableCount];

                    for (int xb = 0; xb < ControllableCount; xb++)
                        Controllables[xb] = new SpawnerControllable();
                }
                io.WriteBits(Controllables.Length, 0xA);
                foreach (SpawnerControllable t in Controllables)
                    t.Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}