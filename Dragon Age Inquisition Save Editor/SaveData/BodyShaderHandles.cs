#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class BodyShaderHandles : DAInterface<BodyShaderHandles>
    {
        internal int xLength { get; set; }
        public int HandleCount { get; set; }
        public int[] Handles { get; set; }

        public int Length => this.InstanceLength();

        public BodyShaderHandles Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            HandleCount = io.ReadBit2(0x10);
            Handles = new int[HandleCount];
            for (int i = 0; i < HandleCount; i++)
                Handles[i] = io.ReadBit2(0x20);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength) io.WriteBits(Length, 0x18);
                if (Handles == null)
                    Handles = new int[HandleCount];
                io.WriteBits(Handles.Length, 0x10);
                for (int i = 0; i < Handles.Length; i++)
                    io.WriteBits(Handles[i], 0x20);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}