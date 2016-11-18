#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class CallBackData : DAInterface<CallBackData>
    {
        internal int xLength { get; set; }
        public int Id { get; set; }
        public int DestroyedPartsCount { get; set; }
        public bool KeepInfoOnUnregister { get; set; }
        internal short PartIdsCount { get; set; }
        public int[] PartIds { get; set; }
        public int BitCount { get; set; }
        public byte[] BitArray { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public CallBackData Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            Id = io.ReadInt32();
            DestroyedPartsCount = io.ReadInt32();
            KeepInfoOnUnregister = io.ReadBoolean();
            PartIdsCount = io.ReadInt16();
            PartIds = new int[PartIdsCount];
            for (int i = 0; i < PartIdsCount; i++)
                PartIds[i] = io.ReadInt32();
            BitCount = io.ReadInt32();
            int x = BitCount.NumberOfSetBits() << 2;
            BitArray = new byte[x];
            io.Read(BitArray, 0, x);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                io.WriteInt32(Id);
                io.WriteInt32(DestroyedPartsCount);
                io.WriteBoolean(KeepInfoOnUnregister);
                if (PartIds == null)
                    PartIds = new int[PartIdsCount];
                io.WriteInt16((short) PartIds.Length);
                foreach (int t in PartIds)
                    io.WriteInt32(t);
                io.WriteInt32(BitCount);
                if (BitArray == null)
                    BitArray = new byte[BitCount.NumberOfSetBits() << 2];
                io.Write(BitArray, 0, BitArray.Length);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}