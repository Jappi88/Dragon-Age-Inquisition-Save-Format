#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class AgentEntity : DAInterface<AgentEntity>
    {
        public AgentEntity(SaveDataStructure xstruct)
        {
            SStructure = xstruct;
        }

        internal int xLength { get; set; }
        internal SaveDataStructure SStructure { get; private set; }
        internal int WorldStateManagerLength { get; set; }
        public WorldStateManager WorldStateManager { get; set; }
        internal int PartyManagerLength { get; set; }
        public PartyManager PartyManager { get; set; }
        internal int PlotSaveGameAgentLength { get; set; }
        public PlotSaveGame PlotSaveGameAgent { get; set; }
        public LootManager LootManager { get; set; }
        public SpawnerCreateManager SpawnerCreateManager { get; set; }
        internal int MapManagerLength { get; set; }
        public BWMapManager MapManager { get; set; }
        internal int MissionManagerLength { get; set; }
        public MissionManager MissionManager { get; set; }
        public SpawnerInits SpawnerInits { get; set; }
        public TerrainManager Terrain { get; set; }
        public StoreInventoryManager StoreInventoryManager { get; set; }
        internal int PartyPlayerManagerLength { get; set; }
        public PartyPlayerManager PartyInventory { get; set; }
        public JournalManager JournalManager { get; set; }
        public uint LengthBits => 0x20;
        public int Length => this.InstanceLength();

        public AgentEntity Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            WorldStateManagerLength = io.ReadBit2(0x18);
            WorldStateManager = new WorldStateManager(SStructure).Read(io);
            PartyManagerLength = io.ReadBit2(0x18);
            PartyManager = new PartyManager().Read(io);
            PlotSaveGameAgentLength = io.ReadBit2(0x18);
            PlotSaveGameAgent = new PlotSaveGame().Read(io);
            LootManager = new LootManager(SStructure).Read(io);
            SpawnerCreateManager = new SpawnerCreateManager().Read(io);
            MapManagerLength = io.ReadBit2(0x18);
            MapManager = new BWMapManager(SStructure).Read(io);
            MissionManagerLength = io.ReadBit2(0x18);
            MissionManager = new MissionManager(SStructure);
            MissionManager.Read(io);
            SpawnerInits = new SpawnerInits().Read(io);
            Terrain = new TerrainManager(SStructure).Read(io);
            StoreInventoryManager = new StoreInventoryManager(SStructure).Read(io);
            PartyPlayerManagerLength = io.ReadBit2(0x18);
            PartyInventory = new PartyPlayerManager(SStructure).Read(io);
            JournalManager = new JournalManager().Read(io);
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                io.WriteBits(WorldStateManager.Length + 0x18, 0x18);
                WorldStateManager.Write(io);
                io.WriteBits(PartyManager.Length + 0x18, 0x18);
                PartyManager.Write(io);
                io.WriteBits(PlotSaveGameAgent.Length + 0x18, 0x18);
                PlotSaveGameAgent.Write(io);
                LootManager.Write(io);
                SpawnerCreateManager.Write(io);
                io.WriteBits(MapManager.Length + 0x18, 0x18);
                MapManager.Write(io);
                io.WriteBits(MissionManager.Length + 0x18, 0x18);
                MissionManager.Write(io);
                SpawnerInits.Write(io);
                Terrain.Write(io);
                StoreInventoryManager.Write(io);
                io.WriteBits(PartyInventory.Length + 0x18, 0x18);
                PartyInventory.Write(io);
                JournalManager.Write(io);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}