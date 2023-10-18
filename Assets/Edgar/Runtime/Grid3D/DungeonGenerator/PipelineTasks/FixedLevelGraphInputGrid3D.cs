using System.Collections;
using System.Linq;
using UnityEngine;

namespace Edgar.Unity
{
    internal class FixedLevelGraphInputGrid3D : PipelineTaskGrid3D
    {
        private readonly FixedLevelGraphConfigGrid3D config;

        public FixedLevelGraphInputGrid3D(FixedLevelGraphConfigGrid3D config)
        {
            this.config = config;
        }

        public override IEnumerator Process()
        {
            if (config.LevelGraph == null)
            {
                throw new ConfigurationException("The LevelGraph field must not be null. Please assign a level graph in the Input config section of the generator component.");
            }

            if (config.LevelGraph.Rooms.Count == 0)
            {
                throw new ConfigurationException($"Each level graph must contain at least one room. Please add some rooms to the level graph called \"{config.LevelGraph.name}\".");
            }

            var levelDescription = new LevelDescriptionGrid3D(config.AllowRotationOverride.GetBoolValue());

            // Setup individual rooms
            foreach (var room in config.LevelGraph.Rooms)
            {
                var roomTemplates = InputSetupUtils.GetRoomTemplates(room, config.LevelGraph.DefaultRoomTemplateSets, config.LevelGraph.DefaultIndividualRoomTemplates);

                if (roomTemplates.Count == 0)
                {
                    throw new ConfigurationException($"There are no room templates for the room \"{room.GetDisplayName()}\" and also no room templates in the default set of room templates. Please make sure that the room has at least one room template available.");
                }

                levelDescription.AddRoom(room, roomTemplates);
            }

            var typeOfRooms = config.LevelGraph.Rooms.First().GetType();

            // Add passages
            foreach (var connection in config.LevelGraph.Connections)
            {
                if (config.UseCorridors)
                {
                    var corridorRoom = (RoomBase)ScriptableObject.CreateInstance(typeOfRooms);

                    if (corridorRoom is Room basicRoom)
                    {
                        basicRoom.Name = "Corridor";
                    }

                    levelDescription.AddCorridorConnection(connection, corridorRoom,
                        InputSetupUtils.GetRoomTemplates(connection, config.LevelGraph.CorridorRoomTemplateSets, config.LevelGraph.CorridorIndividualRoomTemplates));
                }
                else
                {
                    levelDescription.AddConnection(connection);
                }
            }

            InputSetupUtils.CheckIfDirected(levelDescription, config.LevelGraph);

            if (config.FixElevationsInsideCycles)
            {
                levelDescription.FixElevationsInsideCycles();
            }

            Payload.LevelDescription = levelDescription;

            yield return null;
        }
    }
}