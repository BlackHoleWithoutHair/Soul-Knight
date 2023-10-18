using Edgar.Geometry;
using Edgar.GraphBasedGenerator.Common.Doors;
using Edgar.GraphBasedGenerator.Grid2D;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace Edgar.Unity
{
    // TODO(Grid3D): This class needs refactoring
    internal static class GeneratorUtilsGrid3D
    {
        public static DungeonGeneratorLevelGrid3D TransformLayout(LayoutGrid2D<RoomBase> layout, LevelDescriptionGrid3D levelDescription, GameObject rootGameObject, Random random, GeneratorSettingsGrid3D generatorSettings)
        {
            var prefabToRoomTemplateMapping = levelDescription.GetPrefabToRoomTemplateMapping();
            var corridorToConnectionMapping = levelDescription.GetCorridorToConnectionMapping();

            // Prepare an object to hold instantiated room templates
            var roomTemplateInstancesRoot = new GameObject(GeneratorConstantsGrid2D.RoomsRootName);
            roomTemplateInstancesRoot.transform.parent = rootGameObject.transform;
            roomTemplateInstancesRoot.transform.localPosition = new Vector3();

            // Initialize rooms
            var roomToRoomInstanceMapping = new Dictionary<RoomBase, RoomInstanceGrid3D>();
            var layoutRooms = layout.Rooms.ToDictionary(x => x.Room, x => x);
            foreach (var layoutRoom in layoutRooms.Values)
            {
                var roomTemplatePrefab = prefabToRoomTemplateMapping.GetByValue(layoutRoom.RoomTemplate);

                // Instantiate room template
                var roomTemplateInstance = Object.Instantiate(roomTemplatePrefab);
                roomTemplateInstance.transform.SetParent(roomTemplateInstancesRoot.transform);
                roomTemplateInstance.name = $"{layoutRoom.Room.GetDisplayName()} - {roomTemplatePrefab.name}";

                // Compute correct rotation
                var rotation = GetRotation(layoutRoom.Transformation);
                roomTemplateInstance.transform.Rotate(new Vector3(0, 1, 0), rotation);

                // Compute correct room position
                var position = layoutRoom.Position.ToUnityIntVector3().To3DSpace();

                roomTemplateInstance.transform.position = generatorSettings.CellToLocal(position);

                // Compute outline polygon
                var outline = layoutRoom.Outline;
                var polygon = new Polygon2D(outline + layoutRoom.Position);

                var connection = layoutRoom.IsCorridor ? corridorToConnectionMapping[layoutRoom.Room] : null;
                var roomInstance = new RoomInstanceGrid3D(layoutRoom.Room, layoutRoom.IsCorridor, connection, roomTemplatePrefab, roomTemplateInstance, position, polygon /*rotation,*/ /*tilePosition, tilePolygon*/);

                // Add room info to the GameObject
                var roomInfo = roomTemplateInstance.RemoveThenAddComponent<RoomInfoGrid3D>();
                roomInfo.RoomInstance = roomInstance;

                roomToRoomInstanceMapping.Add(layoutRoom.Room, roomInstance);
            }

            foreach (var roomInstance in roomToRoomInstanceMapping.Values)
            {
                ComputeDoorLines(roomInstance, layoutRooms[roomInstance.Room], roomToRoomInstanceMapping);
            }

            FixElevation(roomToRoomInstanceMapping, generatorSettings);

            // Add level info
            var levelInfo = rootGameObject.RemoveThenAddComponent<LevelInfoGrid3D>();
            levelInfo.RoomInstances = roomToRoomInstanceMapping.Values.ToList();

            return new DungeonGeneratorLevelGrid3D(roomToRoomInstanceMapping, rootGameObject, levelDescription, random, generatorSettings);
        }

        private static void ComputeDoorLines(RoomInstanceGrid3D roomInstance, LayoutRoomGrid2D<RoomBase> layoutRoom, Dictionary<RoomBase, RoomInstanceGrid3D> roomInstances)
        {
            var doorLineInfos = new List<DoorLineInfoGrid3D>();

            // Get all the door handlers from the room template
            var doorHandlers = roomInstance.RoomTemplateInstance.GetComponentsInChildren<DoorHandlerGrid3D>().ToList();

            var layoutDoors = layoutRoom.Doors.ToList();
            var transformation = GetRotation(layoutRoom.Transformation);

            // Go through all the available door lines and find all the doors from that door line that were actually used
            foreach (var doorHandler in doorHandlers)
            {
                var doorInstances = new List<DoorInstanceGrid3D>();

                // Door line represented as tiles
                var doorLineTiles = doorHandler.GetDoorLine();

                var fromNormalized = doorLineTiles.From;
                fromNormalized.y = 0;
                var toNormalized = doorLineTiles.To;
                toNormalized.y = 0;
                var doorLineTilesNormalized = new OrthogonalLine(fromNormalized, toNormalized);

                var usedDoors = new List<LayoutDoorGrid2D<RoomBase>>();

                foreach (var door in layoutDoors)
                {
                    // Other door line represented as points
                    var otherDoorLinePoints = door.DoorLine;
                    var otherDoorLinePointsRotated = otherDoorLinePoints.Rotate(-transformation);
                    var otherDoorLineTiles = otherDoorLinePointsRotated.ToTiles();
                    var otherDoorLineTilesFrom3D = otherDoorLineTiles.From.To3DSpace();

                    if (doorLineTilesNormalized.Contains(otherDoorLineTilesFrom3D) != -1 && doorLineTiles.DirectionVector == otherDoorLinePointsRotated.GetDirectionVector().To3DSpace())
                    {
                        var from = otherDoorLineTiles.From.To3DSpace();
                        from.y = doorLineTiles.From.y;

                        var to = otherDoorLineTiles.To.To3DSpace();
                        to.y = doorLineTiles.From.y;

                        var convertedDoorLine = new OrthogonalLine(from, to);
                        var doorInstance = new DoorInstanceGrid3D(door.ToRoom, roomInstances[door.ToRoom], door.Socket as DoorSocketBase, GetDirection(door.Type), doorHandler, convertedDoorLine);
                        doorInstances.Add(doorInstance);
                        usedDoors.Add(door);
                    }
                }

                foreach (var door in usedDoors)
                {
                    layoutDoors.Remove(door);
                }

                var doorLineInfo = new DoorLineInfoGrid3D(
                    doorLineTiles,
                    doorLineTiles.DirectionVector,
                    doorInstances,
                    doorHandler
                );
                doorLineInfos.Add(doorLineInfo);
            }

            if (layoutDoors.Count != 0)
            {
                throw new InvalidOperationException();
            }

            roomInstance.SetDoors(doorLineInfos);
        }

        internal static OrthogonalLineGrid2D ToTiles(this OrthogonalLineGrid2D line)
        {
            line = line.Shrink(0, 1);

            var from = line.From;
            var to = line.To;

            EdgarVector2Int transformedFrom;
            EdgarVector2Int transformedTo;
            var directionVector = line.GetDirectionVector();

            if (directionVector.Y > 0)
            {
                transformedFrom = @from;
                transformedTo = to;
            }
            else if (directionVector.Y < 0)
            {
                var offset = new EdgarVector2Int(1, 1);
                transformedFrom = from - offset;
                transformedTo = to - offset;
            }
            else if (directionVector.X > 0)
            {
                var offset = new EdgarVector2Int(0, 1);
                transformedFrom = @from - offset;
                transformedTo = to - offset;
            }
            else if (directionVector.X < 0)
            {
                var offset = new EdgarVector2Int(1, 0);
                transformedFrom = @from - offset;
                transformedTo = to - offset;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            return new OrthogonalLineGrid2D(transformedFrom, transformedTo, line.GetDirection());
        }

        // TODO(Grid3D): Keep DRY
        private static DoorDirection GetDirection(DoorType type)
        {
            switch (type)
            {
                case DoorType.Undirected:
                    return DoorDirection.Undirected;
                case DoorType.Entrance:
                    return DoorDirection.Entrance;
                case DoorType.Exit:
                    return DoorDirection.Exit;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static int GetRotation(TransformationGrid2D transformation)
        {
            switch (transformation)
            {
                case TransformationGrid2D.Identity:
                    return 0;

                case TransformationGrid2D.Rotate90:
                    return 90;

                case TransformationGrid2D.Rotate180:
                    return 180;

                case TransformationGrid2D.Rotate270:
                    return 270;

                default:
                    throw new ArgumentOutOfRangeException(nameof(transformation), transformation, "Only rotations are currently supported as transformations");
            }
        }

        private static void FixElevation(Dictionary<RoomBase, RoomInstanceGrid3D> layoutData, GeneratorSettingsGrid3D generatorSettings)
        {
            var firstRoom = layoutData.Values.First();
            var firstRoomPosition = firstRoom.Position;
            firstRoomPosition.y = 0;
            firstRoom.UpdatePosition(firstRoomPosition);

            var queue = new Queue<Tuple<RoomInstanceGrid3D, RoomInstanceGrid3D>>();
            var isVisited = new HashSet<RoomInstanceGrid3D> { firstRoom };

            foreach (var door in firstRoom.Doors)
            {
                queue.Enqueue(Tuple.Create(firstRoom, door.ConnectedRoomInstance));
            }

            while (queue.Count > 0)
            {
                var tuple = queue.Dequeue();
                var room = tuple.Item2;
                var previousRoom = tuple.Item1;

                var doorInstance1 = room.Doors.Single(x => x.ConnectedRoom == previousRoom.Room);
                var doorInstance2 = previousRoom.Doors.Single(x => x.ConnectedRoom == room.Room);

                var door1 = doorInstance1.DoorHandler.GetDoorLine();
                var door2 = doorInstance2.DoorHandler.GetDoorLine();

                var elevation1 = Math.Min(door1.From.y, door1.To.y);
                var elevation2 = Math.Min(door2.From.y, door2.To.y);
                var diff = elevation2 - elevation1;

                var position = room.Position;
                position.y = previousRoom.Position.y + diff;

                if (isVisited.Contains(room))
                {
                    if (position.y != room.Position.y)
                    {
                        throw new GeneratorException($"It was not possible to set the correct elevation for rooms {room.Room.GetDisplayName()} and {previousRoom.Room.GetDisplayName()}. This happens when there is a room template in a cycle that has different elevations of its doors.");
                    }

                    continue;
                }

                room.UpdatePosition(position);
                room.RoomTemplateInstance.transform.position = generatorSettings.CellToLocal(position);
                isVisited.Add(room);

                foreach (var door in room.Doors)
                {
                    if (door.ConnectedRoom != previousRoom.Room)
                    {
                        queue.Enqueue(Tuple.Create(room, door.ConnectedRoomInstance));
                    }
                }
            }
        }
    }
}