using System;

namespace Edgar.Unity
{
    public class ConnectorsAndBlockersContextGrid3D
    {
        public DoorLineInfoGrid3D DoorLine { get; }

        public RoomInstanceGrid3D RoomInstance { get; }

        public DungeonGeneratorLevelGrid3D Level { get; }

        public Random Random => Level.Random;

        public bool AddConnectors { get; }

        public bool AddBlockers { get; }

        public ConnectorsModeGrid3D ConnectorsMode { get; }

        public ConnectorsAndBlockersContextGrid3D(DoorLineInfoGrid3D doorLine, RoomInstanceGrid3D roomInstance, DungeonGeneratorLevelGrid3D level, bool addConnectors, bool addBlockers, ConnectorsModeGrid3D connectorsMode)
        {
            DoorLine = doorLine;
            RoomInstance = roomInstance;
            Level = level;
            AddConnectors = addConnectors;
            AddBlockers = addBlockers;
            ConnectorsMode = connectorsMode;
        }
    }
}