using Edgar.GraphBasedGenerator.Grid2D;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Edgar.Unity
{
    public static class InputSetupUtils
    {
        /// <summary>
        /// Gets room templates for a given room.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="defaultRoomTemplatesSets"></param>
        /// <param name="defaultIndividualRoomTemplates"></param>
        /// <returns></returns>
        public static List<GameObject> GetRoomTemplates(RoomBase room, List<RoomTemplatesSet> defaultRoomTemplatesSets, List<GameObject> defaultIndividualRoomTemplates)
        {
            var roomTemplates = room.GetRoomTemplates();

            if (roomTemplates == null || roomTemplates.Count == 0)
            {
                return GetRoomTemplates(defaultRoomTemplatesSets, defaultIndividualRoomTemplates);
            }

            return roomTemplates;
        }

        /// <summary>
        /// Gets corridor room templates for a given connection.
        /// These room templates are only used if UseCorridors is enabled.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="defaultRoomTemplatesSets"></param>
        /// <param name="defaultIndividualRoomTemplates"></param>
        /// <returns></returns>
        public static List<GameObject> GetRoomTemplates(ConnectionBase connection, List<RoomTemplatesSet> defaultRoomTemplatesSets, List<GameObject> defaultIndividualRoomTemplates)
        {
            var roomTemplates = connection.GetRoomTemplates();

            if (roomTemplates == null || roomTemplates.Count == 0)
            {
                return GetRoomTemplates(defaultRoomTemplatesSets, defaultIndividualRoomTemplates);
            }

            return roomTemplates;
        }

        /// <summary>
        /// Combines room templates from room templates sets and individual room templates.
        /// </summary>
        /// <param name="roomTemplatesSets"></param>
        /// <param name="individualRoomTemplates"></param>
        /// <returns></returns>
        public static List<GameObject> GetRoomTemplates(List<RoomTemplatesSet> roomTemplatesSets, List<GameObject> individualRoomTemplates)
        {
            return individualRoomTemplates
                .Where(x => x != null)
                .Union(roomTemplatesSets
                    .Where(x => x != null)
                    .SelectMany(x => x.RoomTemplates))
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// Checks if the level graph does not have directed doors if it is not directed.
        /// </summary>
        /// <param name="levelDescription"></param>
        /// <param name="levelGraph"></param>
        public static void CheckIfDirected(LevelDescriptionBase levelDescription, LevelGraph levelGraph)
        {
            if (levelGraph.IsDirected)
            {
                return;
            }

            foreach (var roomTemplate in levelDescription.GetPrefabToRoomTemplateMapping().Values)
            {
                if (roomTemplate.Doors is ManualDoorModeGrid2D doorMode)
                {
                    var hasDirectedDoors = doorMode.Doors != null && doorMode.Doors.Any(x => x.Type != GraphBasedGenerator.Common.Doors.DoorType.Undirected);
                    var hasDirectedDoorLines = doorMode.DoorLines != null && doorMode.DoorLines.Any(x => x.Type != GraphBasedGenerator.Common.Doors.DoorType.Undirected);

                    if (hasDirectedDoors || hasDirectedDoorLines)
                    {
                        throw new ConfigurationException(
                            $"LevelGraph.IsDirected must be enabled when using entrance-only or exit-only doors. Level graph: \"{levelGraph.name}\", room template: \"{roomTemplate.Name}\"");
                    }
                }
            }
        }
    }
}