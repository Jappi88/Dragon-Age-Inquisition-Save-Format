#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class CustomizationData : DAInterface<CustomizationData>
    {
        public short Version { get; set; }
        internal int CustomizationLength { get; set; }
        internal short Options { get; set; }
        public Customization[] Customizations { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public CustomizationData Read(DAIIO io)
        {
            Version = io.ReadInt16();
            CustomizationLength = io.ReadBit2(LengthBits);
            Options = io.ReadInt16();
            Customizations = new Customization[Options];
            for (int i = 0; i < Options; i++)
                Customizations[i] = new Customization(this).Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                io.WriteInt16(Version);
                if (!skiplength) io.WriteBits(Length - 0x10, LengthBits);
                if (Customizations == null)
                {
                    Customizations = new Customization[Options];

                    for (int xb = 0; xb < Options; xb++)
                        Customizations[xb] = new Customization(this);
                }
                io.WriteInt16((short) Customizations.Length);
                for (int i = 0; i < Customizations.Length; i++)
                    Customizations[i].Write(io);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}