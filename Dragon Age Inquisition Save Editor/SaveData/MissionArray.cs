#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class MissionArray : DAInterface<MissionArray>
    {
        public MissionArray(SaveDataStructure xstruc, bool isfirst)
        {
            SStructure = xstruc;
            _isfirst = isfirst;
        }

        internal SaveDataStructure SStructure { get; private set; }
       internal int xLength { get; set; }
        public int Index { get; set; }
        private readonly bool _isfirst;
        public WarTableEntry WarTableEntry { get; set; }

        public int Length => this.InstanceLength();

        public MissionArray Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            if (_isfirst)
                Index = io.ReadInt32();
            WarTableEntry = new WarTableEntry(SStructure,_isfirst).Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength)io.WriteBits(Length, 0x18);
                if (_isfirst)
                    io.WriteInt32(Index);
                WarTableEntry.Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}