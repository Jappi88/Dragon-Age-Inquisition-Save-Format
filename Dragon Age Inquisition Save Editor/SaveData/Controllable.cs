#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class Controllable : DAInterface<Controllable>
    {
        public bool ShouldSave { get; set; }
        public bool IsHuman { get; set; }
        public bool HasPlayer { get; set; }
        public int PlayerID { get; set; }
        public uint LengthBits => 0;
        public int Length => this.InstanceLength();

        public Controllable Read(DAIIO io)
        {
            ShouldSave = io.ReadBoolean();
            if (ShouldSave)
            {
                IsHuman = io.ReadBoolean();
                HasPlayer = io.ReadBoolean();
                if (HasPlayer)
                    PlayerID = io.ReadInt32();
            }
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                io.WriteBoolean(ShouldSave);
                if (ShouldSave)
                {
                    io.WriteBoolean(IsHuman);
                    io.WriteBoolean(HasPlayer);
                    if (HasPlayer)
                        io.WriteInt32(PlayerID);
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