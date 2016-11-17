#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class MapPins : DAInterface<MapPins>
    {
        internal short PinIdCount { get; set; }
        public int[] PinIds { get; set; }

        public int Length => this.InstanceLength();

        public MapPins Read(DAIIO io)
        {
            PinIdCount = io.ReadInt16();
            PinIds = new int[PinIdCount];
            for (int i = 0; i < PinIdCount; i++)
                PinIds[i] = io.ReadInt32();
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                io.WriteInt16(PinIdCount);
                if (PinIds == null)
                    PinIds = new int[PinIdCount];
                for (int i = 0; i < PinIdCount; i++)
                    io.WriteInt32(PinIds[i]);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}