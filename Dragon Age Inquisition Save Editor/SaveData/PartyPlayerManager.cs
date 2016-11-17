#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class PartyPlayerManager : DAInterface<PartyPlayerManager>
    {
        public PartyPlayerManager(SaveDataStructure xstruc)
        {
            SStructure = xstruc;
        }

        internal SaveDataStructure SStructure { get; private set; }
        internal int xLength { get; set; }
        public PlayerManager Manager { get; set; }
        public DataCache DataCache { get; set; }
        public CharacterCustomization CharacterCustomization { get; set; }
        public bool ItemManagerExists { get; set; }

        public int Length => this.InstanceLength();

        public PartyPlayerManager Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            Manager = new PlayerManager(SStructure).Read(io);
            DataCache = new DataCache().Read(io);
            CharacterCustomization = new CharacterCustomization().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, 0x18);
                Manager.Write(io);
                DataCache.Write(io);
                CharacterCustomization.Write(io);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}