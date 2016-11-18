#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class CompositionDisplay : DAInterface<CompositionDisplay>
    {
        internal short StringIdCount { get; set; }
        public int[] StringIds { get; set; }
        public uint LengthBits => 0;
        public int Length => this.InstanceLength();

        public CompositionDisplay Read(DAIIO io)
        {
            StringIdCount = io.ReadInt16();
            StringIds = new int[StringIdCount];
            for (int i = 0; i < StringIdCount; i++)
                StringIds[i] = io.ReadInt32();
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (StringIds == null)
                    StringIds = new int[StringIdCount];
                io.WriteInt16((short)StringIds.Length);
                foreach (int t in StringIds)
                    io.WriteInt32(t);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}