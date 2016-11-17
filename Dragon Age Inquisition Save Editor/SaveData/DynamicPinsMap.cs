#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class DynamicPinsMap : DAInterface<DynamicPinsMap>
    {
       internal int xLength { get; set; }
        public byte[] MapGuid { get; set; }
        internal short PinsCount { get; set; }
        public DynamicMapPinInfo[] Pins { get; set; }

        public int Length => this.InstanceLength();

        public DynamicPinsMap Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            MapGuid = new byte[0x10];
            io.Read(MapGuid, 0, 0x10);
            PinsCount = io.ReadInt16();
            Pins = new DynamicMapPinInfo[PinsCount];
            for (int i = 0; i < PinsCount; i++)
                Pins[i] = new DynamicMapPinInfo().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength)io.WriteBits(Length, 0x18);
                if (MapGuid == null)
                    MapGuid = new byte[0x10];
                io.Write(MapGuid, 0, 0X10);
                if (Pins == null)
                {
                    Pins = new DynamicMapPinInfo[PinsCount];

                    for (int xb = 0; xb < PinsCount; xb++)
                        Pins[xb] = new DynamicMapPinInfo();
                }
                io.WriteInt16((short) Pins.Length);
                for (int i = 0; i < Pins.Length; i++)
                    Pins[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}