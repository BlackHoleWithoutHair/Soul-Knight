using Edgar.GraphBasedGenerator.Grid2D;
using Edgar.Unity.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Edgar.Unity
{
    /// <inheritdoc />
    public class LevelDescriptionGrid3D : LevelDescriptionBase
    {
        private readonly bool? allowRotationOverride;

        public LevelDescriptionGrid3D(bool? allowRotationOverride = null)
        {
            this.allowRotationOverride = allowRotationOverride;
        }

        protected override bool TryGetRoomTemplate(GameObject roomTemplatePrefab, out RoomTemplateGrid2D roomTemplate, out ActionResult result)
        {
            return RoomTemplateLoaderGrid3D.TryGetRoomTemplate(roomTemplatePrefab, allowRotationOverride, true, true, out roomTemplate, out result);
        }

        public void FixElevationsInsideCycles()
        {
            var hasDifferentElevations = new HashSet<RoomTemplateGrid2D>();

            foreach (var pair in PrefabToRoomTemplateMapping)
            {
                var prefab = pair.Key;
                var roomTemplate = pair.Value;

                if (RoomTemplateLoaderGrid3D.TryGetDoors(prefab, null, out var doorLoadingResult))
                {
                    if (doorLoadingResult.HasDifferentElevations)
                    {
                        hasDifferentElevations.Add(roomTemplate);
                    }
                }
                else
                {
                    throw new GeneratorException("Something went wrong inside FixElevationsInsideCycles, please contact the author of the asset.");
                }
            }

            var fixedLevelDescription = new LevelDescriptionGrid2D<RoomBase>();

            var graph = LevelDescription.GetGraph(false, true);
            var graphWithoutCorridors = LevelDescription.GetGraph(true, false);

            var nodesInsideCycle = GraphUtilsGrid3D.GetNodesInsideCycle(graphWithoutCorridors);

            foreach (var room in graph.Vertices)
            {
                var roomDescription = LevelDescription.GetRoomDescription(room);
                var needsToBeFixed = false;

                if (roomDescription.IsCorridor)
                {
                    var connection = CorridorToConnectionMapping[room];
                    var fromInsideCycle = nodesInsideCycle.Contains(connection.From);
                    var toInsideCycle = nodesInsideCycle.Contains(connection.To);

                    // Both ends of the corridor are inside a cycle
                    if (fromInsideCycle && toInsideCycle)
                    {
                        needsToBeFixed = true;
                    }
                }
                else
                {
                    // Non-corridor room is inside cycle
                    if (nodesInsideCycle.Contains(room))
                    {
                        needsToBeFixed = true;
                    }
                }

                if (needsToBeFixed)
                {
                    var fixedRoomTemplates = roomDescription
                        .RoomTemplates
                        .Where(x => !hasDifferentElevations.Contains(x))
                        .ToList();
                    var fixedRoomDescription = new RoomDescriptionGrid2D(roomDescription.IsCorridor, fixedRoomTemplates);
                    fixedLevelDescription.AddRoom(room, fixedRoomDescription);
                }
                else
                {
                    fixedLevelDescription.AddRoom(room, roomDescription);
                }
            }

            foreach (var edge in graph.Edges)
            {
                fixedLevelDescription.AddConnection(edge.From, edge.To);
            }

            LevelDescription = fixedLevelDescription;
        }
    }
}