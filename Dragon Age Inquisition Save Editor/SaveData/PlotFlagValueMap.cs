#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class PlotFlagValueMap : DAInterface<PlotFlagValueMap>
    {
        internal int xLength { get; set; }
        public short Version { get; set; }
        public short MarkerDataLength { get; set; }
        public string MarkerData { get; set; }
        public BooleanFlags BooleanFlagsThatAreTrue { get; set; }
        public BooleanFlags BooleanFlagsThatAreFalse { get; set; }
        public short IntegerFlagsCount { get; set; }
        public ValueFlag[] IntegerFlags { get; set; }
        public short FloatFlagsCount { get; set; }
        public ValueFlag[] FloatFlags { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                io.WriteInt16(Version);
                if ((Version & 0xFFFF) >= 2)
                {
                    io.WriteInt16(MarkerDataLength);
                    io.WriteString(MarkerData);
                }
                if ((Version & 0xFFFF) >= 1)
                {
                    BooleanFlagsThatAreTrue.Write(io);
                    BooleanFlagsThatAreFalse.Write(io);
                    io.WriteInt16(IntegerFlagsCount);
                    if (IntegerFlags == null)
                    {
                        IntegerFlags = new ValueFlag[IntegerFlagsCount];

                        for (int xb = 0; xb < IntegerFlagsCount; xb++)
                            IntegerFlags[xb] = new ValueFlag();
                    }
                    for (int i = 0; i < IntegerFlagsCount; i++)
                        IntegerFlags[i].Write(io);
                    io.WriteInt16(FloatFlagsCount);
                    if (FloatFlags == null)
                    {
                        FloatFlags = new ValueFlag[FloatFlagsCount];

                        for (int xb = 0; xb < FloatFlagsCount; xb++)
                            FloatFlags[xb] = new ValueFlag();
                    }
                    for (int i = 0; i < FloatFlagsCount; i++)
                        FloatFlags[i].Write(io);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public PlotFlagValueMap Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            Version = io.ReadInt16();
            if ((Version & 0xFFFF) >= 2)
            {
                MarkerDataLength = io.ReadInt16();
                MarkerData = io.ReadString(MarkerDataLength);
            }
            if ((Version & 0xFFFF) >= 1)
            {
                BooleanFlagsThatAreTrue = new BooleanFlags();
                BooleanFlagsThatAreTrue.Read(io);

                BooleanFlagsThatAreFalse = new BooleanFlags();
                BooleanFlagsThatAreFalse.Read(io);

                IntegerFlagsCount = io.ReadInt16();
                IntegerFlags = new ValueFlag[IntegerFlagsCount];
                for (int i = 0; i < IntegerFlagsCount; i++)
                {
                    IntegerFlags[i] = new ValueFlag();
                    IntegerFlags[i].Read(io);
                }

                FloatFlagsCount = io.ReadInt16();
                FloatFlags = new ValueFlag[FloatFlagsCount];
                for (int i = 0; i < FloatFlagsCount; i++)
                {
                    FloatFlags[i] = new ValueFlag();
                    FloatFlags[i].Read(io);
                }
            }
            return this;
        }
    }
}