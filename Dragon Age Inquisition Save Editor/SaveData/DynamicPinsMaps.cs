#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class DynamicPinsMaps : DAInterface<DynamicPinsMaps>
    {
        internal short Count { get; set; }
        public DynamicPinsMap[] Pins { get; set; }
        public uint LengthBits => 0;
        public int Length => this.InstanceLength();

        public DynamicPinsMaps Read(DAIIO io)
        {
            Count = io.ReadInt16();
            Pins = new DynamicPinsMap[Count];
            for (int i = 0; i < Count; i++)
                Pins[i] = new DynamicPinsMap().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (Pins == null)
                {
                    Pins = new DynamicPinsMap[Count];

                    for (int xb = 0; xb < Count; xb++)
                        Pins[xb] = new DynamicPinsMap();
                }
                io.WriteInt16((short) Pins.Length);
                foreach (DynamicPinsMap t in Pins)
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