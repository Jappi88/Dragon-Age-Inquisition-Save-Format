#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class MasterInfo : DAInterface<MasterInfo>
    {
        public short DestructionInfoCount { get; set; }
        public byte[][] PosAndImpacts { get; set; }

        public int Length => this.InstanceLength();

        public MasterInfo Read(DAIIO io)
        {
            DestructionInfoCount = io.ReadInt16();
            PosAndImpacts = new byte[DestructionInfoCount][];
            for (int i = 0; i < DestructionInfoCount; i++)
            {
                PosAndImpacts[i] = new byte[0x10];
                io.Read(PosAndImpacts[i], 0, 0x10);
            }
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
               
                if (PosAndImpacts == null)
                    PosAndImpacts = new byte[DestructionInfoCount][];
                io.WriteInt16((short) PosAndImpacts.Length);
                for (int i = 0; i < PosAndImpacts.Length; i++)
                {
                    if (PosAndImpacts[i] == null)
                        PosAndImpacts[i] = new byte[0x10];
                    io.Write(PosAndImpacts[i], 0, 0x10);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}