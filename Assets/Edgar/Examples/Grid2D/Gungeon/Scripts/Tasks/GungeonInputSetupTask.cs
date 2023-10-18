using Edgar.Legacy.Utils;
using System;
using System.Linq;
using UnityEngine;

namespace Edgar.Unity.Examples.Gungeon
{
    [CreateAssetMenu(menuName = "Edgar/Examples/Gungeon/Input setup", fileName = "Gungeon Input Setup")]
    public class GungeonInputSetupTask : DungeonGeneratorInputBaseGrid2D
    {
        public LevelGraph LevelGraph;

        public bool UseRandomLevelGraph;

        [Range(1, 2)]
        public int Stage = 1;

        public LevelGraph[] Stage1LevelGraphs;

        public LevelGraph[] Stage2LevelGraphs;

        public GungeonRoomTemplatesConfig RoomTemplates;

        // The probability that a secret room is added to the level
        [Range(0f, 1f)]
        public float SecretRoomChance = 0.9f;

        // The probability that a secret room is attached to a dead-end room
        [Range(0f, 1f)]
        public float SecretRoomDeadEndChance = 0.5f;

        protected override LevelDescriptionGrid2D GetLevelDescription()
        {
            // Pick random level graph
            if (UseRandomLevelGraph)
            {
                var levelGraphs = Stage == 1 ? Stage1LevelGraphs : Stage2LevelGraphs;
                LevelGraph = levelGraphs.GetRandom(Random);
            }
            else if (!UseRandomLevelGraph && LevelGraph == null)
            {
                throw new InvalidOperationException("The level graph must not be null if UseRandomLevelGraph is set to false");
            }

            // The instance of the game manager will not exist in Editor
            if (GungeonGameManager.Instance != null)
            {
                GungeonGameManager.Instance.CurrentLevelGraph = LevelGraph;
            }

            var levelDescription = new LevelDescriptionGrid2D();

            // Manually add all the rooms to the level description
            foreach (var room in LevelGraph.Rooms.Cast<GungeonRoom>())
            {
                levelDescription.AddRoom(room, RoomTemplates.GetRoomTemplates(room).ToList());
            }

            // Go through individual connections between basic rooms to add corridor rooms
            foreach (var connection in LevelGraph.Connections.Cast<GungeonConnection>())
            {
                var corridorRoom = ScriptableObject.CreateInstance<GungeonRoom>();
                corridorRoom.Type = GungeonRoomType.Corridor;

                levelDescription.AddCorridorConnection(connection, corridorRoom, RoomTemplates.CorridorRoomTemplates.ToList());
            }

            // Add secret rooms
            AddSecretRoom(levelDescription);

            return levelDescription;
        }

        private void AddSecretRoom(LevelDescriptionGrid2D levelDescription)
        {
            // Return early if no secret room should be added to the level
            if (Random.NextDouble() > SecretRoomChance) return;

            // Get the graphs of rooms
            var graph = levelDescription.GetGraph();

            // Decide whether to attach the secret room to a dead end room or not
            var attachToDeadEnd = Random.NextDouble() < SecretRoomDeadEndChance;

            // Find all the possible rooms to attach to and choose a random one
            var possibleRoomsToAttachTo = graph.Vertices.Cast<GungeonRoom>().Where(x =>
                (!attachToDeadEnd || graph.GetNeighbors(x).Count() == 1) && x.Type != GungeonRoomType.Entrance
            ).ToList();
            var roomToAttachTo = possibleRoomsToAttachTo[Random.Next(possibleRoomsToAttachTo.Count)];

            // Create secret room
            var secretRoom = ScriptableObject.CreateInstance<GungeonRoom>();
            secretRoom.Type = GungeonRoomType.Secret;
            levelDescription.AddRoom(secretRoom, RoomTemplates.GetRoomTemplates(secretRoom).ToList());

            // Prepare the connection between secretRoom and roomToAttachTo
            var connection = ScriptableObject.CreateInstance<GungeonConnection>();
            connection.From = roomToAttachTo;
            connection.To = secretRoom;

            // Connect the two rooms with a corridor
            var corridorRoom = ScriptableObject.CreateInstance<GungeonRoom>();
            corridorRoom.Type = GungeonRoomType.Corridor;
            levelDescription.AddCorridorConnection(connection, corridorRoom, RoomTemplates.CorridorRoomTemplates.ToList());
        }
    }
}