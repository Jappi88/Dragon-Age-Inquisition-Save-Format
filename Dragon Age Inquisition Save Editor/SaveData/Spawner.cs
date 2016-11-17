#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class Spawner : DAInterface<Spawner>
    {
        public int ID { get; set; }
        public short ControllableCount { get; set; }
        public SpawnerControllable[] Controllables { get; set; }

        public int Length => this.InstanceLength();

        public Spawner Read(DAIIO io)
        {
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
                io.WriteInt32(ID);
                if (Controllables == null)
                {
                    Controllables = new SpawnerControllable[ControllableCount];

                    for (int xb = 0; xb < ControllableCount; xb++)
                        Controllables[xb] = new SpawnerControllable();
                }
                io.WriteBits(Controllables.Length, 0xA);
                for (int i = 0; i < Controllables.Length; i++)
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