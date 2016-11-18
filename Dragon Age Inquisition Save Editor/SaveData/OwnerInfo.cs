#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class OwnerInfo : DAInterface<OwnerInfo>
    {
        public OwnerInfo(SaveDataStructure xstruct)
        {
            SStructure = xstruct;
        }

        internal SaveDataStructure SStructure { get; private set; }
        internal int xLength { get; set; }
        public int Id { get; set; }
        public short CallbackCount { get; set; }
        public CallBackData[] Callbacks { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public OwnerInfo Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            Id = io.ReadInt32();
            CallbackCount = io.ReadInt16();
            Callbacks = new CallBackData[CallbackCount];
            for (int i = 0; i < CallbackCount; i++)
                Callbacks[i] = new CallBackData().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                io.WriteInt32(Id);
                io.WriteInt16(CallbackCount);
                if (Callbacks == null)
                {
                    Callbacks = new CallBackData[CallbackCount];

                    for (int xb = 0; xb < CallbackCount; xb++)
                        Callbacks[xb] = new CallBackData();
                }
                for (int i = 0; i < CallbackCount; i++)
                    Callbacks[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}