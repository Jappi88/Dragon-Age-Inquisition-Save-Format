#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class SpawnerCreateManager : DAInterface<SpawnerCreateManager>
    {
        internal int xLength { get; set; }
        public short SpawnerCreateCount { get; set; }
        public SpawnerCreate[] SpawnerCreates { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public SpawnerCreateManager Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            SpawnerCreateCount = io.ReadInt16();
            SpawnerCreates = new SpawnerCreate[SpawnerCreateCount];
            for (int i = 0; i < SpawnerCreateCount; i++)
                SpawnerCreates[i] = new SpawnerCreate().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                io.WriteInt16(SpawnerCreateCount);
                if (SpawnerCreates == null)
                {
                    SpawnerCreates = new SpawnerCreate[SpawnerCreateCount];

                    for (int xb = 0; xb < SpawnerCreateCount; xb++)
                        SpawnerCreates[xb] = new SpawnerCreate();
                }
                for (int i = 0; i < SpawnerCreateCount; i++)
                    SpawnerCreates[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}