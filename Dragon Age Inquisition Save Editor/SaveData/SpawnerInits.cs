#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class SpawnerInits : DAInterface<SpawnerInits>
    {
        internal int xLength { get; set; }
        public short Count { get; set; }
        public Spawner[] Inits { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public SpawnerInits Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            Count = io.ReadInt16();
            Inits = new Spawner[Count];
            for (int i = 0; i < Count; i++)
                Inits[i] = new Spawner().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                io.WriteInt16(Count);
                if (Inits == null)
                {
                    Inits = new Spawner[Count];

                    for (int xb = 0; xb < Count; xb++)
                        Inits[xb] = new Spawner();
                }
                for (int i = 0; i < Count; i++)
                    Inits[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}