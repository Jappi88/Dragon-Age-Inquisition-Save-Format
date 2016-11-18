#region

using System;
using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class BWMapManager : DAInterface<BWMapManager>
    {
        internal SaveDataStructure SStructure { get; private set; }
        internal int xLength { get; set; }
        public int UserWaypoint_MapId { get; set; }
        public byte[] UserWaypoint_WorldPos { get; set; }
        public FogArchive FogArchive { get; set; }
        public DynamicPinsMaps DynamicPinsMaps { get; set; }
        public MapPins DiscoveredPins { get; set; }
        public MapPins ExploredPins { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();

        public BWMapManager(SaveDataStructure xstruc)
        {
            SStructure = xstruc;
        }

        public BWMapManager Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            //if(6 (<-????) > 5)
            //{ 
            UserWaypoint_MapId = io.ReadInt32();
            UserWaypoint_WorldPos = new byte[8];
            for (int i = 0; i < 8; i++)
                UserWaypoint_WorldPos[i] = (byte) io.ReadBit(0x8);
            FogArchive = new FogArchive().Read(io);
            DynamicPinsMaps = new DynamicPinsMaps().Read(io);
            DiscoveredPins = new MapPins().Read(io);
            ExploredPins = new MapPins().Read(io);
            //}
            return this;
        }


        public bool Write(DAIIO io, bool skiplength = false)
        {
            try
            {
                if (!skiplength) io.WriteBits(Length, LengthBits);
                //if(6 (<-????) > 5)
                //{ 
                io.WriteInt32(UserWaypoint_MapId);
                if (UserWaypoint_WorldPos == null)
                    UserWaypoint_WorldPos = new byte[8];
                for (int i = 0; i < 8; i++)
                    io.WriteBits(UserWaypoint_WorldPos[i], 0x8);
                FogArchive.Write(io);
                DynamicPinsMaps.Write(io);
                DiscoveredPins.Write(io);
                ExploredPins.Write(io);
                //}

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}