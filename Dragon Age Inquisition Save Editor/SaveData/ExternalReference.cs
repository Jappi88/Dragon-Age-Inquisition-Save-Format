#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class ExternalReference : DAInterface<ExternalReference>
    {
        public int SubLevel { get; set; }
        public int Bundle { get; set; }
        internal short EntityCount { get; set; }
        public int[] Entities { get; set; }

        public int Length => this.InstanceLength();

        public ExternalReference Read(DAIIO io)
        {
            SubLevel = io.ReadBit2(0x18);
            // x.Bundle = io.ReadBit2(0x18);
            EntityCount = (short) io.ReadBit2(0x10);
            Entities = new int[EntityCount];
            for (int j = 0; j < EntityCount; j++)
                Entities[j] = io.ReadBit2(0x20);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                io.WriteBits(SubLevel, 0x18);
                //io.WriteBits( //x.Bundle, 0x18);
                if (Entities == null)
                    Entities = new int[EntityCount];
                io.WriteBits(Entities.Length, 0x10);
                for (int j = 0; j < Entities.Length; j++)
                    io.WriteBits(Entities[j], 0x20);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}