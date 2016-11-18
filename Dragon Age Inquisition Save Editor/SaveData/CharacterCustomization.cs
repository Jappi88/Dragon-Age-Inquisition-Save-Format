#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class CharacterCustomization : DAInterface<CharacterCustomization>
    {
        internal int xLength { get; set; }
        public bool CustomizationManagerExists { get; set; }
        public CharacterCustomizationManager CustomizationManager { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public CharacterCustomization Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            CustomizationManagerExists = io.ReadBoolean();
            if (CustomizationManagerExists)
                CustomizationManager = new CharacterCustomizationManager().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                io.WriteBoolean(CustomizationManagerExists);
                if (CustomizationManagerExists)
                    CustomizationManager.Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}