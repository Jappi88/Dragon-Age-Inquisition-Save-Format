#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class SaveDataStructure : DAInterface<SaveDataStructure>
    {
        public static readonly byte[] DataMagic = {0x46, 0x42, 0x00, 0x53, 0x41, 0x56, 0x45, 0x0A};
        private byte[] tmpdata;
        public DateTime SavedTime { get; set; }
        public string SID { get; set; }
        public ushort GameVersion { get; set; }
        public ushort SaveVersion { get; set; }
        public ushort ProjectVersion { get; set; }
        public ushort BitstreamFeatures { get; set; }
        public uint ChangeList { get; set; }
        public string Level { get; set; }
        public uint Difficulty { get; set; }
        public ushort BundleCount { get; set; }
        public string[] BundleList { get; set; }
        public ushort InclusionCount { get; set; }
        public Inclusion[] Inclusions { get; set; }
        public int SubLevelInfoCount { get; set; }
        public SubLevelInfo[] SubLevelEntries { get; set; }
        public byte[] LevelChecksum { get; set; }
        public int DLC { get; set; }
        public short EntityVersion { get; set; }
        public short ProjectVersionContext { get; set; }
        public bool HasDebugInfo { get; set; }
        public int AgentTocBookmark { get; set; }
        public AgentToc AgentToc { get; set; }
        public int ClientDataBookmark { get; set; }
        internal int EntityContentLength { get; set; }
        public EntityData EntityContent { get; set; }
        public int SaveEntityComplexLength { get; set; }
        public byte[] SaveEntityComplex { get; set; }
        public int SaveEntityBookmark { get; set; }
        public SaveEntity SaveEntity { get; set; }
        public EntityMeta EntityMetaData { get; set; }
        public ClientAgent ClientData { get; set; }
        internal int LastPartSeedLength { get; set; }
        internal int LastPartSeed { get; set; }

        public int Length => this.InstanceLength();
        
        public SaveDataStructure Read(DAIIO io)
        {
            byte[] xdm = io.ReadBytes(0x41, false);
            if (!xdm.MemCompare(DataMagic, 0))
                throw new Exception("Invalid Save Data Header!");
            SavedTime = io.ReadInt64().ToUnixTime();
            ushort count = io.ReadUInt16();
            SID = io.ReadString(count);
            GameVersion = io.ReadUInt16();
            SaveVersion = io.ReadUInt16();
            ProjectVersion = io.ReadUInt16();
            BitstreamFeatures = io.ReadUInt16();
            ChangeList = io.ReadUInt32();
            count = io.ReadUInt16();
            Level = io.ReadString(count);
            Difficulty = io.ReadUInt32();
            BundleCount = io.ReadUInt16();
            BundleList = new string[BundleCount];
            for (int i = 0; i < BundleCount; i++)
            {
                count = io.ReadUInt16();
                BundleList[i] = io.ReadString(count);
            }
            InclusionCount = io.ReadUInt16();
            Inclusions = new Inclusion[InclusionCount];
            for (int i = 0; i < InclusionCount; i++)
                Inclusions[i] = new Inclusion().Read(io);
            SubLevelInfoCount = io.ReadBit2(0xc);
            if (SubLevelInfoCount <= 0x40 && SubLevelInfoCount > 0)
            {
                SubLevelEntries = new SubLevelInfo[SubLevelInfoCount];
                for (int i = 0; i < SubLevelInfoCount; i++)
                    SubLevelEntries[i] = new SubLevelInfo().Read(io);
            }
            LevelChecksum = new byte[0x10];
            for (int i = 0; i < 0x10; i++)
                LevelChecksum[i] = (byte) io.ReadBit(0x8);
            DLC = io.ReadBit2(0x14);
            EntityVersion = (short) io.ReadBit2(0x10);
            ProjectVersionContext = (short) io.ReadBit2(0x10);
            HasDebugInfo = io.ReadBoolean();
            AgentTocBookmark = io.ReadBit2(0x1A);
            ClientDataBookmark = io.ReadBit2(0x1A);
            EntityContentLength = io.ReadBit2(0x20);
            ///////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////UNFINISHED SECTION!!!!!/////////////////////////////////
            var xpos = io.Position;                                                      ////////
            EntityContent = new EntityData(this).Read(io);                              ////////
            tmpdata = io.ReadData((int) (EntityContentLength - (io.Position - xpos))); ////////
            //////////////////////////////////////////////////////////////////////////////////
            SaveEntityComplexLength = io.ReadBit2(6);
            SaveEntityComplex = io.ReadData(SaveEntityComplexLength);
            SaveEntityBookmark = io.ReadBit2(0x1A);
            SaveEntity = new SaveEntity(this).Read(io);
            EntityMetaData = new EntityMeta(this).Read(io);
            AgentToc = new AgentToc().Read(io);
            //var xdio = new DAIIO(io.xbaseStream, io.Position, ClientDataLength) {IsBigEndian = true};
            ClientData = new ClientAgent().Read(io);
            //io.Position += ClientDataLength;
            LastPartSeedLength = (int) (io.Length - io.Position);
            if (LastPartSeedLength > 0)
                LastPartSeed = io.ReadBit2((uint) LastPartSeedLength);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                io.xbaseStream.Write(DataMagic, 0, 8);
                io.Position = 0x41;
                io.WriteInt64(SavedTime.ToUnixSecondsWithAdd());
                io.WriteInt16((short) SID.Length);
                io.WriteString(SID);
                io.WriteUInt16(GameVersion);
                io.WriteUInt16(SaveVersion);
                io.WriteUInt16(ProjectVersion);
                io.WriteUInt16(BitstreamFeatures);
                io.WriteUInt32(ChangeList);
                io.WriteUInt16((ushort) Level.Length);
                io.WriteString(Level);
                io.WriteUInt32(Difficulty);
                if (BundleList == null)
                {
                    BundleList = new string[BundleCount];

                    for (int xb = 0; xb < BundleCount; xb++)
                        BundleList[xb] = "";
                }
                BundleCount = (ushort) BundleList.Length;
                io.WriteUInt16(BundleCount);
                for (int i = 0; i < BundleCount; i++)
                {
                    io.WriteInt16((short) BundleList[i].Length);
                    io.WriteString(BundleList[i]);
                }
                if (Inclusions == null)
                {
                    Inclusions = new Inclusion[InclusionCount];

                    for (int xb = 0; xb < InclusionCount; xb++)
                        Inclusions[xb] = new Inclusion();
                }
                InclusionCount = (ushort) Inclusions.Length;
                io.WriteInt16((short)InclusionCount);
                for (int i = 0; i < InclusionCount; i++)
                    Inclusions[i].Write(io);
                if (SubLevelInfoCount <= 0x40 && SubLevelInfoCount > 0)
                {
                    if (SubLevelEntries == null)
                    {
                        SubLevelEntries = new SubLevelInfo[SubLevelInfoCount];

                        for (int xb = 0; xb < SubLevelInfoCount; xb++)
                            SubLevelEntries[xb] = new SubLevelInfo();
                    }
                    SubLevelInfoCount = SubLevelEntries.Length;
                    io.WriteBits(SubLevelInfoCount, 0xc);
                    for (int i = 0; i < SubLevelInfoCount; i++)
                        SubLevelEntries[i].Write(io);
                }
                if (LevelChecksum == null)
                    LevelChecksum = new byte[0x10];
                for (int i = 0; i < 0x10; i++)
                    io.WriteBits(LevelChecksum[i], 0x8);
                io.WriteBits(DLC, 0x14);
                io.WriteBits(EntityVersion, 0x10);
                io.WriteBits(ProjectVersionContext, 0x10);
                io.WriteBoolean(HasDebugInfo);

                ///////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////CALCULATE SECTION BOOKMARKS/////////////////////////////////////////
                var pos = io.Position;///////////////////////////////////////////////////////////////////////////
                pos += 0x54;////////////////////////////////////////////////////////////////////////////////////
                pos += (EntityContentLength +//////////////////////////////////////////////////////////////////
                        (((SaveEntityComplexLength + 0x38) + SaveEntity.Length) + EntityMetaData.Length));////
                AgentTocBookmark = (int) pos;////////////////////////////////////////////////////////////////
                pos += (AgentToc.Length + 0x18);////////////////////////////////////////////////////////////
                ClientDataBookmark = (int) pos;////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////
                
                io.WriteBits(AgentTocBookmark, 0x1A);
                io.WriteBits(ClientDataBookmark, 0x1A);
                io.WriteBits(EntityContentLength, 0x20);

                ///////////////////////////////////////////////////////////////////////////////
                ////////////////////////WRITE UNFINISHED SECTION//////////////////////////////
                EntityContent.Write(io);/////////////////////////////////////////////////////
                //MUST WRITE UNFINISHED RAW DATA, REMOVE IF CONTENT HAS BEEN MAPPED OUT/////
                if (tmpdata.Length > 0)////////////////////////////////////////////////////
                    io.WriteData(tmpdata, EntityContentLength - EntityContent.Length);////
                /////////////////////////////////////////////////////////////////////////

                //Start writing SaveEntity
                SaveEntityBookmark = (int) io.Position;
                io.WriteBits(SaveEntityComplexLength, 6);
                io.WriteData(SaveEntityComplex, SaveEntityComplexLength);
                io.WriteBits(SaveEntityBookmark, 0x1A);
                SaveEntity?.Write(io);
                EntityMetaData?.Write(io);
                AgentToc?.Write(io);
                ClientData?.Write(io);
               // io.FinishWriter();
                if (LastPartSeedLength > 0)
                {
                    io.WriteBits((uint)LastPartSeed, (uint) LastPartSeedLength,true);
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