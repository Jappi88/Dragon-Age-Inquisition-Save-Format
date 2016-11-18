#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class MissionManager : DAInterface<MissionManager>
    {
        public MissionManager(SaveDataStructure xstruc)
        {
            SStructure = xstruc;
        }

        internal SaveDataStructure SStructure { get; private set; }
        internal int xLength { get; set; }
        public int InProgress { get; set; }
        public int MissionArraySize { get; set; }
        public int MissionArray0Count { get; set; }
        public MissionArray[] MissionArray0 { get; set; }
        public int MissionArray1Count { get; set; }
        public MissionArray[] MissionArray1 { get; set; }
        public int Completed { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                io.WriteBits(InProgress, 0x18);
                io.WriteInt32(MissionArraySize);
                if (MissionArray0 == null)
                {
                    MissionArray0 = new MissionArray[MissionArray0Count];

                    for (int xb = 0; xb < MissionArray0Count; xb++)
                        MissionArray0[xb] = new MissionArray(SStructure,true);
                }
                io.WriteInt16((short)MissionArray0.Length);
                for (int i = 0; i < MissionArray0.Length; i++)
                    MissionArray0[i].Write(io);
                io.WriteBits(Completed, 0x18);
                if (MissionArray1 == null)
                {
                    MissionArray1 = new MissionArray[MissionArray1Count];

                    for (int xb = 0; xb < MissionArray1Count; xb++)
                        MissionArray1[xb] = new MissionArray(SStructure,false);
                }
                io.WriteInt16((short)MissionArray1.Length);
                for (int i = 0; i < MissionArray1.Length; i++)
                    MissionArray1[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public MissionManager Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            InProgress = io.ReadBit2(0x18);
            MissionArraySize = io.ReadInt32();
            MissionArray0Count = io.ReadInt16();
            MissionArray0 = new MissionArray[MissionArray0Count];
            for (int i = 0; i < MissionArray0Count; i++)
                MissionArray0[i] = new MissionArray(SStructure,true).Read(io);

            Completed = io.ReadBit2(0x18);

            MissionArray1Count = io.ReadInt16();
            MissionArray1 = new MissionArray[MissionArray1Count];
            for (int i = 0; i < MissionArray1Count; i++)
                MissionArray1[i] = new MissionArray(SStructure,false).Read(io);
            return this;
        }
    }
}