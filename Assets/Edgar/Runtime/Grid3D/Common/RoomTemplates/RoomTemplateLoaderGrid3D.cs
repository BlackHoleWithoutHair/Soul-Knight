using System;
using System.Collections.Generic;
using System.Linq;
using Edgar.Geometry;
using Edgar.GraphBasedGenerator.Grid2D;
using Edgar.GraphBasedGenerator.Grid2D.Exceptions;
using Edgar.Legacy.GeneralAlgorithms.Algorithms.Common;
using Edgar.Unity.Diagnostics;
using Edgar.Unity.Exceptions;
using UnityEngine;
#if UNITY_EDITOR
#if UNITY_2021_2_OR_NEWER
using UnityEditor.SceneManagement;
#else
using UnityEditor.Experimental.SceneManagement;
#endif
#endif
using Object = UnityEngine.Object;

namespace Edgar.Unity
{
    public class RoomTemplateLoaderGrid3D
    {
        /// <summary>
        /// Computes the outline of the room template.
        /// </summary>
        /// <returns>Throws if outline is not valid.</returns>
        public static PolygonGrid2D GetOutline(GameObject roomTemplate)
        {
            var roomTemplateSettings = roomTemplate.GetComponent<RoomTemplateSettingsGrid3D>();
            var blocks = GetBlocks(roomTemplate, roomTemplateSettings);

            var positions2d = blocks.Select(x => new Vector2Int(x.x, x.z)).ToHashSet();
            var outline = GetPolygonFromTiles(positions2d);

            return outline;
        }

        internal static PolygonGrid2D GetPolygonFromTiles(HashSet<Vector2Int> points, PolygonOutlineModeGrid3D outlineMode = PolygonOutlineModeGrid3D.Points)
        {
            var pointsInternal = points.Select(x => x.ToCustomIntVector2()).ToHashSet();
            return GetPolygonFromTiles(pointsInternal, outlineMode);
        }

        /// <summary>
        /// Computes a room room template from a given room template game object.
        /// </summary>
        /// <param name="roomTemplatePrefab"></param>
        /// <param name="allowRotationOverride"></param>
        /// <param name="checkOrigin"></param>
        /// <param name="handleComputationMode"></param>
        /// <param name="roomTemplate"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryGetRoomTemplate(GameObject roomTemplatePrefab, bool? allowRotationOverride, bool checkOrigin, bool handleComputationMode, out RoomTemplateGrid2D roomTemplate, out ActionResult result)
        {
            roomTemplate = null;

            // Check if the room template is positioned at (0,0,0)
            if (checkOrigin)
            {
                if (roomTemplatePrefab.transform.localPosition != Vector3.zero)
                {
                    result = new ActionResult("The room template prefab root must be positioned at (0,0,0).");
                    return false;
                }
            }

            var roomTemplateSettings = roomTemplatePrefab.GetComponent<RoomTemplateSettingsGrid3D>();

            // Check that the room template has all the required components
            var requiredComponentsResult = RoomTemplateDiagnosticsGrid3D.CheckComponents(roomTemplatePrefab);
            if (requiredComponentsResult.HasErrors)
            {
                result = requiredComponentsResult;
                return false;
            }

            // Try to get the outline of the room template
            PolygonGrid2D polygon;

            if (handleComputationMode &&
                roomTemplateSettings.GeneratorSettings.OutlineComputationMode == RoomTemplateOutlineComputationModeGrid3D.InsideEditor)
            {
                var precomputedOutline = roomTemplateSettings.Outline;

                if (precomputedOutline == null || precomputedOutline.Count == 0)
                {
                    result = new ActionResult();
                    result.AddError($"The outline of the room template is not valid: The outline computation mode is set to InsideEditor but no outline was provided.");
                    return false;
                }

                try
                {
                    var points = precomputedOutline.Select(x => new EdgarVector2Int(x.x, x.y));
                    polygon = new PolygonGrid2D(points);
                }
                catch (Exception e)
                {
                    result = new ActionResult();
                    result.AddError($"The precomputed outline of the room template is not valid: {e.Message}.");
                    return false;
                }
            }

            else
            {
                try
                {
                    polygon = GetOutline(roomTemplatePrefab);
                }
                catch (InvalidOutlineException e)
                {
                    result = new ActionResult();
                    result.AddError($"The outline of the room template is not valid: {e.Message}");
                    return false;
                }
            }

            // Rotations and repeat mode
            var allowRotation = allowRotationOverride ?? roomTemplateSettings.AllowRotation;
            var allowedTransformations = allowRotation
                ? TransformationGrid2DHelper.GetRotations()
                : new List<TransformationGrid2D> { TransformationGrid2D.Identity };
            var repeatMode = roomTemplateSettings.RepeatMode;

            // Try to get the doors
            var polygonForDoorsCheck = Application.isPlaying ? null : polygon;
            if (!TryGetDoors(roomTemplatePrefab, polygonForDoorsCheck, out var doorLoadingResult))
            {
                result = doorLoadingResult.ActionResult;
                return false;
            }

            // Check that the doors are correct
            var doorsCheck = RoomTemplateDiagnosticsGrid3D.CheckDoors(polygon, doorLoadingResult.DoorMode);
            if (doorsCheck.HasErrors)
            {
                result = doorsCheck;
                return false;
            }

            roomTemplate = new RoomTemplateGrid2D(polygon, doorLoadingResult.DoorMode, roomTemplatePrefab.name, repeatMode, allowedTransformations);

            result = new ActionResult();
            return true;
        }

        /// <summary>
        /// Tries to get the doors of the room template.
        /// </summary>
        /// <remarks>
        /// Use the checkDoorsAgainstOutline argument if you want to check individual doors against the room template outline.
        /// Use null for the outline otherwise.
        ///
        /// By doing it this way, we can check the error status of individual room templates rather than having a generic message.
        /// </remarks>
        /// <param name="roomTemplatePrefab"></param>
        /// <param name="checkDoorsAgainstOutline"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryGetDoors(GameObject roomTemplatePrefab, PolygonGrid2D checkDoorsAgainstOutline, out DoorLoadingResultGrid3D result)
        {
            var doorLines = new List<GraphBasedGenerator.Grid2D.DoorLineGrid2D>();
            var doorComponents = roomTemplatePrefab.GetComponentsInChildren<DoorHandlerGrid3D>();
            var actionResult = new ActionResult();
            var doorElevations = new HashSet<int>();

            foreach (var doorComponent in doorComponents)
            {
                if (doorComponent.GeneratorSettings == null)
                {
                    actionResult.AddError($"The door object '{doorComponent.gameObject.name}' does not have {nameof(DoorHandlerGrid3D.GeneratorSettings)} assigned.");
                    continue;
                }

                try
                {
                    var doorGrid3D = doorComponent.GetDoorLine();
                    doorElevations.Add(doorGrid3D.From.y);
                    var door = doorGrid3D.GetInternalPointRepresentation();
                    doorLines.Add(door);

                    // Check if the door is positioned correctly with respect to the outline (if provided)S
                    if (checkDoorsAgainstOutline != null)
                    {
                        try
                        {
                            var testDoorMode = new ManualDoorModeGrid2D(new List<GraphBasedGenerator.Grid2D.DoorLineGrid2D>()
                            {
                                door,
                            });
                            testDoorMode.GetDoors(checkDoorsAgainstOutline);
                        }
                        catch (DoorLineOutsideOfOutlineException)
                        {
                            actionResult.AddError($"The door object '{doorComponent.gameObject.name}' is not located on the outline of the room template or is incorrectly rotated.");
                        }
                    }
                }
                catch (MisalignedPositionException)
                {
                    actionResult.AddError($"The door object '{doorComponent.gameObject.name}' is not correctly positioned on the grid.");
                }
                catch (DoorRotationOutOfSyncException)
                {
                    actionResult.AddError($"The door object '{doorComponent.gameObject.name}' has an incorrect orientation.");
                }
            }

            var hasDifferentElevations = doorElevations.Count > 1;

            if (actionResult.HasErrors)
            {
                result = new DoorLoadingResultGrid3D(null, false, actionResult);
                return false;
            }

            var doorMode = new ManualDoorModeGrid2D(doorLines);
            result = new DoorLoadingResultGrid3D(doorMode, hasDifferentElevations, actionResult);

            return true;
        }

        private static List<PositionAlias> GetAliases(EdgarVector2Int position, List<EdgarVector2Int> innerPositions)
        {
            var aliases = new List<PositionAlias>();

            if (innerPositions == null || innerPositions.Count == 0)
            {
                aliases.Add(new PositionAlias(position, new EdgarVector2Int(0, 0)));
            }
            else
            {
                foreach (var innerPosition in innerPositions)
                {
                    aliases.Add(new PositionAlias(position - innerPosition, innerPosition));
                }
            }

            return aliases;
        }

        /// <summary>
        /// Gets all the blocks from which this room template is made of.
        /// </summary>
        /// <returns></returns>
        public static HashSet<Vector3Int> GetBlocks(GameObject roomTemplate, RoomTemplateSettingsGrid3D roomTemplateSettings)
        {
            if (roomTemplateSettings.OutlineMode == RoomTemplateOutlineModeGrid3D.FromColliders)
            {
                return GetBlocksFromMeshes(roomTemplate, roomTemplateSettings);
            }

            return GetBlocksFromBlocks(roomTemplate, roomTemplateSettings);
        }

        /// <summary>
        /// Gets blocks from explicit <see cref="BlockMarkerGrid3D"/> markers.
        /// </summary>
        /// <returns></returns>
        public static HashSet<Vector3Int> GetBlocksFromBlocks(GameObject roomTemplate, RoomTemplateSettingsGrid3D roomTemplateSettings)
        {
            var blocks = new HashSet<Vector3Int>();

            foreach (var blockComponent in GetBlocksRoot(roomTemplate).GetComponentsInChildren<BlockMarkerGrid3D>())
            {
                var positionRelativeToRoot = roomTemplate.transform.InverseTransformPoint(blockComponent.transform.position);
                var positionRounded = roomTemplateSettings.GeneratorSettings.LocalToCell(positionRelativeToRoot);
                blocks.Add(positionRounded);
            }

            return blocks;
        }

        /// <summary>
        /// Gets blocks from meshes found in the room template.
        /// </summary>
        /// <returns></returns>
        public static HashSet<Vector3Int> GetBlocksFromMeshes(GameObject roomTemplate, RoomTemplateSettingsGrid3D roomTemplateSettings)
        {
#if UNITY_EDITOR
            if (PrefabStageUtility.GetCurrentPrefabStage() == null)
            {
                roomTemplate = Object.Instantiate(roomTemplate);
            }
#else
            roomTemplate = Object.Instantiate(roomTemplate);
#endif

            var colliders = GetBlocksRoot(roomTemplate).GetComponentsInChildren<Collider>();
            var blocks = new HashSet<Vector3Int>();
            var generatorSettings = roomTemplateSettings.GeneratorSettings;

            foreach (var collider in colliders)
            {
                var bounds = collider.bounds;
                var boundsLocal = new Bounds(
                    roomTemplate.transform.InverseTransformPoint(bounds.center),
                    roomTemplate.transform.InverseTransformPoint(bounds.size)
                );
                var intBounds = SnapLocalBoundsToGrid(boundsLocal, generatorSettings);

                foreach (var position in intBounds.allPositionsWithin)
                {
                    blocks.Add(position);
                }
            }

#if UNITY_EDITOR
            if (PrefabStageUtility.GetCurrentPrefabStage() == null)
            {
                UtilsGrid3D.Destroy(roomTemplate);
            }
#else
            UtilsGrid3D.Destroy(roomTemplate);
#endif

            return blocks;
        }

        /// <summary>
        /// Transform bounds in local coordinates to int bounds snapped to the grid.
        /// </summary>
        /// <param name="localBounds"></param>
        /// <param name="generatorSettings"></param>
        /// <returns></returns>
        private static BoundsInt SnapLocalBoundsToGrid(Bounds localBounds, GeneratorSettingsGrid3D generatorSettings)
        {
            var sizeToleranceFloat = generatorSettings.ColliderSizeTolerance;
            var sizeTolerance = new Vector3(sizeToleranceFloat, sizeToleranceFloat, sizeToleranceFloat);

            var minimumSize = 0.1f;
            var originalSize = localBounds.size;
            var newSize = originalSize - 2 * sizeTolerance;
            newSize.Set(Math.Max(newSize.x, minimumSize), Math.Max(newSize.y, minimumSize), Math.Max(newSize.z, minimumSize));

            var newBounds = new Bounds(localBounds.center, newSize);

            var min = generatorSettings.LocalToCellInterpolated(newBounds.min).Floor();
            var max = generatorSettings.LocalToCellInterpolated(newBounds.max).Ceil();

            var intBounds = new BoundsInt(min, max - min);

            return intBounds;
        }

        /// <summary>
        /// Gets the root object that contains blocks in the room template.
        /// </summary>
        /// <returns></returns>
        public static GameObject GetBlocksRoot(GameObject roomTemplate)
        {
            var blocksRoot = roomTemplate.transform.Find(GeneratorConstantsGrid3D.BlocksRootName)?.gameObject;

            if (blocksRoot == null)
            {
                blocksRoot = roomTemplate.transform.Find("Objects")?.gameObject;
            }

            return blocksRoot;
        }

        /// <summary>
        /// A more general version of the 2D algorithm that can also work with outlines made of Points (rather than tiles).
        /// </summary>
        /// <param name="points"></param>
        /// <param name="outlineMode"></param>
        /// <returns></returns>
        public static PolygonGrid2D GetPolygonFromTiles(HashSet<EdgarVector2Int> points, PolygonOutlineModeGrid3D outlineMode = PolygonOutlineModeGrid3D.Points)
        {
            if (points.Count == 0)
            {
                throw new InvalidOutlineException($"There must be at least one block to compute the outline. If you have blocks in your room template, please check that you have the correct {nameof(RoomTemplateSettingsGrid3D.OutlineMode)} selected.");
            }

            var orderedDirections = new Dictionary<EdgarVector2Int, List<EdgarVector2Int>>
            {
                {IntVector2Helper.Top, new List<EdgarVector2Int> {IntVector2Helper.Left, IntVector2Helper.Top, IntVector2Helper.Right}},
                {IntVector2Helper.Right, new List<EdgarVector2Int> {IntVector2Helper.Top, IntVector2Helper.Right, IntVector2Helper.Bottom}},
                {IntVector2Helper.Bottom, new List<EdgarVector2Int> {IntVector2Helper.Right, IntVector2Helper.Bottom, IntVector2Helper.Left}},
                {IntVector2Helper.Left, new List<EdgarVector2Int> {IntVector2Helper.Bottom, IntVector2Helper.Left, IntVector2Helper.Top}}
            };

            var innerPositions = outlineMode == PolygonOutlineModeGrid3D.Points
                ? new List<EdgarVector2Int>()
                {
                    new EdgarVector2Int(0, 0),
                    new EdgarVector2Int(0, 1),
                    new EdgarVector2Int(1, 0),
                    new EdgarVector2Int(1, 1),
                }
                : new List<EdgarVector2Int>()
                {
                    new EdgarVector2Int(0, 0),
                };

            var firstPoint = GetSmallestPoint(points);

            var currentPoint = firstPoint;

            var previousDirection = IntVector2Helper.Top;
            var first = true;

            var polygonPoints = new List<EdgarVector2Int>()
            {
                firstPoint,
            };

            while (true)
            {
                var foundNeighbor = false;
                var currentPointAliases = GetAliases(currentPoint, innerPositions);

                var currentDirection = new EdgarVector2Int();
                var nextPoint = new EdgarVector2Int();

                foreach (var directionVector in orderedDirections[previousDirection])
                {
                    if (foundNeighbor)
                    {
                        break;
                    }

                    var neighborPoint = currentPoint + directionVector;
                    var neighborPointAliases = GetAliases(neighborPoint, innerPositions);

                    foreach (var currentPointAlias in currentPointAliases)
                    {
                        if (foundNeighbor)
                        {
                            break;
                        }

                        if (!points.Contains(currentPointAlias.Position))
                        {
                            continue;
                        }

                        foreach (var neighborPointAlias in neighborPointAliases)
                        {
                            if (!points.Contains(neighborPointAlias.Position))
                            {
                                continue;
                            }

                            if (EdgarVector2Int.ManhattanDistance(currentPointAlias.Position, neighborPointAlias.Position) < 2)
                            {
                                currentDirection = directionVector;
                                nextPoint = neighborPoint;
                                foundNeighbor = true;
                                break;
                            }
                        }
                    }
                }

                if (!foundNeighbor)
                {
                    throw new InvalidOutlineException("Invalid room shape.");
                }

                if (currentDirection != previousDirection)
                {
                    polygonPoints.Add(currentPoint);
                }

                currentPoint = nextPoint;
                previousDirection = currentDirection;

                if (first)
                {
                    first = false;
                }
                else if (currentPoint == firstPoint)
                {
                    break;
                }
            }

            if (polygonPoints.ToHashSet().Count != polygonPoints.Count)
            {
                throw new InvalidOutlineException("All polygon tiles must share at least a single side with a different tile");
            }

            return new PolygonGrid2D(polygonPoints);
        }

        private class PositionAlias
        {
            public EdgarVector2Int Position { get; }

            public EdgarVector2Int InnerPosition { get; }

            public PositionAlias(EdgarVector2Int position, EdgarVector2Int innerPosition)
            {
                Position = position;
                InnerPosition = innerPosition;
            }
        }

        private static EdgarVector2Int GetSmallestPoint(HashSet<EdgarVector2Int> points)
        {
            var smallestX = points.Min(x => x.X);
            var smallestXPoints = points.Where(x => x.X == smallestX).ToList();
            var smallestXYPoint = smallestXPoints[smallestXPoints.MinBy(x => x.Y)];

            return smallestXYPoint;
        }
    }
}