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
        public SpawnerInit[] Inits { get; set; }

        public int Length => this.InstanceLength();

        public SpawnerInits Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            Count = io.ReadInt16();
            Inits = new SpawnerInit[Count];
            for (int i = 0; i < Count; i++)
                Inits[i] = new SpawnerInit().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength) io.WriteBits(Length, 0x18);
                io.WriteInt16(Count);
                if (Inits == null)
                {
                    Inits = new SpawnerInit[Count];

                    for (int xb = 0; xb < Count; xb++)
                        Inits[xb] = new SpawnerInit();
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