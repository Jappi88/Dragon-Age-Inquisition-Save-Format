#region

using System;
using System.Windows.Forms;
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
        public DA3PartyMemberDataCache DataCache { get; set; }
        public CharacterCustomization CharacterCustomization { get; set; }
        public bool ItemManagerExists { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public PartyPlayerManager Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            Manager = new PlayerManager(SStructure).Read(io);
            DataCache = new DA3PartyMemberDataCache().Read(io);
            CharacterCustomization = new CharacterCustomization().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
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