#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class EntityData : DAInterface<EntityData>
    {
        private readonly SaveDataStructure xstruct;
        public int EntityCreate { get; set; }
        public int EntityInit { get; set; }
        public int TotalSpawnCount { get; set; }
        public short OccupiedPointCount { get; set; }
        public byte[][] OccupiedPoints { get; set; }
        public short RecentlyUsedPointCount { get; set; }
        public byte[][] RecentlyUsedPoints { get; set; }
        public int SaveDataByteCount { get; set; }
        public byte[] SaveData { get; set; }
        public short ClonedEventCount { get; set; }
        public int[] ClonedEventIds { get; set; }
        public bool HasLoadSession { get; set; }

        public int Length => this.InstanceLength();

        public EntityData(SaveDataStructure xstr)
        {
            xstruct = xstr;
        }

        public EntityData Read(DAIIO io)
        {
            EntityCreate = io.ReadBit2(0x18);
            SaveDataByteCount = io.ReadBit2(0x20);
            SaveData = io.ReadBytes(SaveDataByteCount);
            if (xstruct.EntityVersion < 7)
            {
                ClonedEventCount = io.ReadInt16();
                ClonedEventIds = new int[ClonedEventCount];
                for (int i = 0; i < ClonedEventCount; i++)
                    ClonedEventIds[i] = io.ReadBit2(0x18);
            }
            HasLoadSession = io.ReadBoolean();
            EntityInit = io.ReadBit2(0x18);
            if (xstruct.EntityVersion > 6)
            {
                ClonedEventCount = (short) io.ReadBit2(8);
            }

            //TotalSpawnCount = io.ReadBit2(0x18);
            //if (xstr.EntityVersion < 9)
            //{
            //    OccupiedPointCount = (short) io.ReadBit2(0x10);
            //    OccupiedPoints = new byte[OccupiedPointCount][];
            //    for (int i = 0; i < OccupiedPointCount; i++)
            //    {
            //        OccupiedPoints[i] = new byte[0xC];
            //        for (int j = 0; j < 0xC; j++)
            //            OccupiedPoints[i][j] = (byte) io.ReadBit(0x8);
            //    }
            //}
            //RecentlyUsedPointCount = (short)io.ReadBit2(0x10);
            //RecentlyUsedPoints = new byte[RecentlyUsedPointCount][];
            //for (int i = 0; i < RecentlyUsedPointCount; i++)
            //{
            //    RecentlyUsedPoints[i] = new byte[0xC];
            //    for (int j = 0; j < 0xC; j++)
            //        RecentlyUsedPoints[i][j] = (byte)io.ReadBit(0x8);
            //}
            //SpawnedGhostCount = (short) io.ReadBit2(0x10);
            //SpawnedGhosts = new SpawnedGhost[SpawnedGhostCount];
            //for (int i = 0; i < SpawnedGhostCount; i++)
            //{
            //    var sp = new SpawnedGhost();
            //    sp.Read(io);
            //    SpawnedGhosts[i] = sp;
            //}
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                io.WriteBits(EntityCreate, 0x18);
                io.WriteBits(SaveDataByteCount, 0x20);
                if (SaveData == null)
                    SaveData = new byte[SaveDataByteCount];
                io.Write(SaveData, 0, SaveDataByteCount);
                if (xstruct.EntityVersion < 7)
                {
                    io.WriteInt16(ClonedEventCount);
                    if (ClonedEventIds == null)
                        ClonedEventIds = new int[ClonedEventCount];
                    for (int i = 0; i < ClonedEventCount; i++)
                        io.WriteBits(ClonedEventIds[i], 0x18);
                }
                io.WriteBoolean(HasLoadSession);
                io.WriteBits(EntityInit, 0x18);
                if (xstruct.EntityVersion > 6)
                    io.WriteBits(ClonedEventCount, 8);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}