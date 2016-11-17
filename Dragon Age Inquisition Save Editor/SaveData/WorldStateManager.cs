#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class WorldStateManager : DAInterface<WorldStateManager>
    {
        public WorldStateManager(SaveDataStructure xstruct)
        {
            SStructure = xstruct;
        }

        internal SaveDataStructure SStructure { get; private set; }
        internal int xLength { get; set; }
        internal short StateHistoryCount { get; set; }
        public StateHistory[] StateHistories { get; set; }

        public int Length => this.InstanceLength();

        public WorldStateManager Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            StateHistoryCount = io.ReadInt16();
            StateHistories = new StateHistory[StateHistoryCount];
            for (int i = 0; i < StateHistoryCount; i++)
                StateHistories[i] = new StateHistory().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength)
                    io.WriteBits(Length, 0x18);
                io.WriteInt16(StateHistoryCount);
                if (StateHistories == null)
                {
                    StateHistories = new StateHistory[StateHistoryCount];

                    for (int xb = 0; xb < StateHistoryCount; xb++)
                        StateHistories[xb] = new StateHistory();
                }
                for (int i = 0; i < StateHistoryCount; i++)
                    StateHistories[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}