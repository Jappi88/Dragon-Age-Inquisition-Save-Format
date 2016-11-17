#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class CharacterCustomizationManager : DAInterface<CharacterCustomizationManager>
    {
        internal int xLength { get; set; }
        public int Version { get; set; }
        public int ClassId { get; set; }
        public int BackgroundId { get; set; }
        public int GenderId { get; set; }
        public int RaceId { get; set; }
        public string CharacterName { get; set; }
        public int VoiceVariationID { get; set; }
        public int DifficultyModeID { get; set; }
        public int LowestDifficultyModeID { get; set; }
        public int CharacterSubclassID { get; set; }
        public byte[] CharacterID { get; set; }
        public float AgeInRealTimeSeconds { get; set; }

        public int Length => this.InstanceLength();

        public CharacterCustomizationManager Read(DAIIO io)
        {
            xLength = io.ReadBit2(0x18);
            Version = io.ReadInt32();
            ClassId = io.ReadInt32();
            BackgroundId = io.ReadInt32();
            GenderId = io.ReadInt32();
            RaceId = io.ReadInt32();
            var x = io.ReadInt16();
            CharacterName = io.ReadString(x);
            VoiceVariationID = io.ReadInt32();
            DifficultyModeID = io.ReadInt32();
            if (Version > 9)
                LowestDifficultyModeID = io.ReadInt32();
            CharacterSubclassID = io.ReadInt32();
            if (Version >= 8)
            {
                CharacterID = new byte[0x10];
                io.Read(CharacterID, 0, 0x10);
            }
            if (Version >= 0xD)
                AgeInRealTimeSeconds = io.ReadSingle();
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if(!skiplength) io.WriteBits(Length, 0x18);
                io.WriteInt32(Version);
                io.WriteInt32(ClassId);
                io.WriteInt32(BackgroundId);
                io.WriteInt32(GenderId);
                io.WriteInt32(RaceId);
                io.WriteInt16((short) CharacterName.Length);
                io.WriteString(CharacterName);
                io.WriteInt32(VoiceVariationID);
                io.WriteInt32(DifficultyModeID);
                if (Version > 9)
                    io.WriteInt32(LowestDifficultyModeID);
                io.WriteInt32(CharacterSubclassID);
                if (Version >= 8)
                {
                    if (CharacterID == null)
                        CharacterID = new byte[0x10];
                    io.Write(CharacterID, 0, 0x10);
                }
                if (Version >= 0xD)
                    io.WriteSingle(AgeInRealTimeSeconds);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}