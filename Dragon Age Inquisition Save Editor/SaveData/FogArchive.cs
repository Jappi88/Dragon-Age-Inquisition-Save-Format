#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class FogArchive : DAInterface<FogArchive>
    {
        internal int xLength { get; set; }
        internal short MapCount { get; set; }
        public Map[] Maps { get; set; }

        public int Length => this.InstanceLength();

        public FogArchive Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            MapCount = io.ReadInt16();
            Maps = new Map[MapCount];
            for (int i = 0; i < MapCount; i++)
                Maps[i] = new Map().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength) io.WriteBits(Length, 0x18);
                if (Maps == null)
                {
                    Maps = new Map[MapCount];

                    for (int xb = 0; xb < MapCount; xb++)
                        Maps[xb] = new Map();
                }
                io.WriteInt16((short) Maps.Length);
                for (int i = 0; i < MapCount; i++)
                    Maps[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}