﻿using System.Linq;
using UnityEngine;

#region codeBlock:3d_dungeon1_inputSetup
namespace Edgar.Unity.Examples.Grid3D.Dungeon1
{
    [CreateAssetMenu(menuName = "Edgar (Grid3D)/Examples/Dungeon 1/Input setup", fileName = "Dungeon 1 Input Setup")]
    public class Dungeon1InputSetup : DungeonGeneratorInputBaseGrid3D
    {
        public LevelGraph LevelGraph;

        public bool UseCorridors;

        public Dungeon1RoomTemplates RoomTemplates;

        protected override LevelDescriptionGrid3D GetLevelDescription()
        {
            // Make sure that level graph is not null
            if (LevelGraph == null)
            {
                throw new ConfigurationException("The LevelGraph field must not be null.");
            }

            var levelDescription = new LevelDescriptionGrid3D();

            // Setup individual rooms
            foreach (var room in LevelGraph.Rooms.Cast<Dungeon1Room>())
            {
                // Get room templates
                var roomTemplates = RoomTemplates.GetRoomTemplates(room.Type);

                if (roomTemplates.Count == 0)
                {
                    throw new ConfigurationException($"There are no room templates for the room type \"{room.Type}\".");
                }

                // Add room to the level description
                levelDescription.AddRoom(room, roomTemplates);
            }

            #region codeBlock:3d_dungeon1_inputSetupCorridors
            // Add passages/corridors
            foreach (var connection in LevelGraph.Connections.Cast<Dungeon1Connection>())
            {
                // Handle both corridor connections and direct room-to-room connections
                // This is useful for the "Extra stuff" in the tutorial
                if (UseCorridors && connection.IsCorridor)
                {
                    var corridorRoom = (RoomBase)CreateInstance<Dungeon1Room>();

                    levelDescription.AddCorridorConnection(connection, corridorRoom,
                        RoomTemplates.Corridors);
                }
                else
                {
                    levelDescription.AddConnection(connection);
                }
            }
            #endregion

            // Just a helper to make sure that directed graphs work properly
            InputSetupUtils.CheckIfDirected(levelDescription, LevelGraph);

            return levelDescription;
        }
    }
}
#endregion