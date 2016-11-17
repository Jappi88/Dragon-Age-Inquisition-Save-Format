#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class SpawnerInit : DAInterface<SpawnerInit>
    {
        internal int xLength { get; set; }
        public Spawner Spawner { get; set; }

        public int Length => this.InstanceLength();

        public SpawnerInit Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x14);
            Spawner = new Spawner().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength) io.WriteBits(Length, 0x14);
                Spawner.Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}