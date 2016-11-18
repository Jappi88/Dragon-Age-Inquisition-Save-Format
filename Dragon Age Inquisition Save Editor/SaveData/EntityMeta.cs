using Dragon_Age_Inquisition_Save_Editor.DAIO;

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    public class EntityMeta : DAInterface<EntityMeta>
    {
        private readonly SaveDataStructure xstruct;
        public int xLength { get; set; }
        public short LookupTableCount { get; set; }
        public LookupTable[] LookupTables { get; set; }
        public short InternalReferenceCount { get; set; }
        public int[] InternalReferences { get; set; }
        public short ExternalReferenceCount { get; set; }
        public ExternalReference[] ExternalReferences { get; set; }
        public short DeletedEntityCount { get; set; }
        public int[] DeletedEntities { get; set; }
        public uint LengthBits => 0x18;
        public int Length => this.InstanceLength();
        public EntityMeta Read(DAIIO io)
        {
            xLength = io.ReadBit2(LengthBits);
            LookupTableCount = (short)io.ReadBit2(0x10);
            LookupTables = new LookupTable[LookupTableCount];
            for (int i = 0; i < LookupTableCount; i++)
                LookupTables[i] = new LookupTable().Read(io);
            InternalReferenceCount = (short)io.ReadBit2(0x10);
            InternalReferences = new int[InternalReferenceCount];
            for (int i = 0; i < InternalReferenceCount; i++)
                InternalReferences[i] = io.ReadBit2(0x20);
            ExternalReferenceCount = (short)io.ReadBit2(0x10);
            ExternalReferences = new ExternalReference[ExternalReferenceCount];
            for (int i = 0; i < ExternalReferenceCount; i++)
                ExternalReferences[i] = new ExternalReference().Read(io);

            DeletedEntityCount = (short)io.ReadBit2(0x10);
            DeletedEntities = new int[DeletedEntityCount];
            for (int i = 0; i < DeletedEntityCount; i++)
                DeletedEntities[i] = io.ReadBit2(0x20);

            return this;
        }

        public bool Write(DAIIO io, bool skiplength = false)
        {
            if (!skiplength) io.WriteBits(Length, LengthBits);
            if (LookupTables == null)
            {
                LookupTables = new LookupTable[LookupTableCount];

                for (int xb = 0; xb < LookupTableCount; xb++)
                    LookupTables[xb] = new LookupTable();
            }
            LookupTableCount = (short)LookupTables.Length;
            io.WriteBits(LookupTableCount, 0x10);
            for (int i = 0; i < LookupTableCount; i++)
                LookupTables[i].Write(io);
            if (InternalReferences == null)
                InternalReferences = new int[InternalReferenceCount];
            InternalReferenceCount = (short)InternalReferences.Length;
            io.WriteBits(InternalReferenceCount, 0x10);
            for (int i = 0; i < InternalReferenceCount; i++)
                io.WriteBits(InternalReferences[i], 0x20);

            if (ExternalReferences == null)
            {
                ExternalReferences = new ExternalReference[ExternalReferenceCount];

                for (int xb = 0; xb < ExternalReferenceCount; xb++)
                    ExternalReferences[xb] = new ExternalReference();
            }
            ExternalReferenceCount = (short)ExternalReferences.Length;
            io.WriteBits(ExternalReferenceCount, 0x10);
            for (int i = 0; i < ExternalReferenceCount; i++)
                ExternalReferences[i].Write(io);

            if (DeletedEntities == null)
                DeletedEntities = new int[DeletedEntityCount];
            DeletedEntityCount = (short)DeletedEntities.Length;
            io.WriteBits(DeletedEntityCount, 0x10);
            for (int i = 0; i < DeletedEntityCount; i++)
                io.WriteBits(DeletedEntities[i], 0x20);
            return true;
        }

        public EntityMeta(SaveDataStructure xstr)
        {
            xstruct = xstr;
        }
    }
}
