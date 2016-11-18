#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class DA3PartyMemberDataCache : DAInterface<DA3PartyMemberDataCache>
    {
        internal int xLength;
        internal short Count { get; set; }
        public PartyMemberDataCache[] Caches { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public DA3PartyMemberDataCache Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            Count = io.ReadInt16();
            Caches = new PartyMemberDataCache[Count];
            for (int i = 0; i < Count; i++)
                Caches[i] = new PartyMemberDataCache().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                if (Caches == null)
                {
                    Caches = new PartyMemberDataCache[Count];

                    for (int xb = 0; xb < Count; xb++)
                        Caches[xb] = new PartyMemberDataCache();
                }
                io.WriteInt16((short) Caches.Length);
                for (int i = 0; i < Count; i++)
                    Caches[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}