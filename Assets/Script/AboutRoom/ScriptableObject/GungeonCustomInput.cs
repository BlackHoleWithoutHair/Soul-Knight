using Edgar.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "GungeonCustomInput", menuName = "ScriptableObjects/GungeonCustomInput")]
public class GungeonCustomInput : DungeonGeneratorInputBaseGrid2D
{
    public IRoomConfig roomConfig;
    private LevelGraph selectLevelGraph;
    private List<RoomInfo> AllRooms;
    private class RoomInfo
    {
        public CustomRoom room;
        public List<CustomRoom> neighbors = new List<CustomRoom>();
    }
    protected override LevelDescriptionGrid2D GetLevelDescription()
    {
        AllRooms = new List<RoomInfo>();
        var levelDescription = new LevelDescriptionGrid2D();
        if (MemoryModelCommand.Instance.GetSmallStage() == 5)
        {
            selectLevelGraph = roomConfig.LevelGraphBoss;
        }
        else
        {
            if (!roomConfig.UseRandomLevelGraph)
            {
                selectLevelGraph = roomConfig.LevelGraph;
            }
            else
            {
                roomConfig.levelGraphs.Add(roomConfig.LevelGraph);
                selectLevelGraph = roomConfig.levelGraphs[Random.Next(roomConfig.levelGraphs.Count)];
            }
        }

        // Manually add all the rooms to the level description
        foreach (var room in selectLevelGraph.Rooms.Cast<CustomRoom>())
        {
            levelDescription.AddRoom(room, roomConfig.RoomTemplates.GetRoomTemplates(room).ToList());
        }

        // Go through individual connections between basic rooms to add corridor rooms
        foreach (var connection in selectLevelGraph.Connections)
        {
            var corridorRoom = ScriptableObject.CreateInstance<CustomRoom>();
            corridorRoom.RoomType = RoomType.Corridor;

            levelDescription.AddCorridorConnection(connection, corridorRoom, roomConfig.RoomTemplates.CorridorRoomTemplates.ToList());
        }
        foreach (CustomRoom room in levelDescription.GetGraph().Vertices.Cast<CustomRoom>())
        {
            RoomInfo roomInfo = new RoomInfo();
            roomInfo.room = room;
            roomInfo.neighbors = levelDescription.GetGraph().GetNeighbors(room).Cast<CustomRoom>().ToList();
            AllRooms.Add(roomInfo);
        }
        // Add extra rooms
        AddExtraRoom(levelDescription, RoomType.EnemyRoom, GetPossibleRoomAttachToEnemyRoom(levelDescription, roomConfig.ExtraEnemyRoomDeadEndChance), roomConfig.ExtraEnemyRoomChance);
        AddExtraRoom(levelDescription, RoomType.TreasureRoom, GetPossibleRoomAttachToEnemyRoom(levelDescription, roomConfig.TreasureRoomDeadEndChance), 1);
        return levelDescription;
    }
    private void AddExtraRoom(LevelDescriptionGrid2D levelDescription, RoomType type, List<RoomInfo> possibleRoomsToAttachTo, float RoomChance)
    {
        // Return early if no secret room should be added to the level
        if (Random.NextDouble() > RoomChance) return;

        // Get the graphs of rooms
        //var graph = levelDescription.GetGraph();

        // Find all the possible rooms to attach to and choose a random one
        if (possibleRoomsToAttachTo.Count > 0)
        {
            var roomToAttachTo = possibleRoomsToAttachTo[Random.Next(possibleRoomsToAttachTo.Count)];

            // Create secret room
            var secretRoom = ScriptableObject.CreateInstance<CustomRoom>();
            secretRoom.RoomType = type;
            levelDescription.AddRoom(secretRoom, roomConfig.RoomTemplates.GetRoomTemplates(secretRoom).ToList());
            RoomInfo roomInfo = new RoomInfo();
            roomInfo.room = secretRoom;

            AllRooms.Add(roomInfo);
            // Prepare the connection between secretRoom and roomToAttachTo

            if (secretRoom.RoomType == RoomType.BirthRoom || secretRoom.RoomType == RoomType.TeleportRoom)
            {

                var connection = ScriptableObject.CreateInstance<CustomConnection>();
                connection.From = roomToAttachTo.room;
                connection.To = secretRoom;

                var corridorRoom = ScriptableObject.CreateInstance<CustomRoom>();
                corridorRoom.RoomType = RoomType.Corridor;
                levelDescription.AddCorridorConnection(connection, corridorRoom, roomConfig.RoomTemplates.CorridorRoomTemplates.ToList());

                roomInfo.neighbors.Add(roomToAttachTo.room);

                CustomRoom secondRoom = GetRoomWhichNeighborsLessThanFour().room;
                var connection1 = ScriptableObject.CreateInstance<CustomConnection>();
                connection1.From = roomToAttachTo.room;
                connection1.To = secondRoom;
                corridorRoom = ScriptableObject.CreateInstance<CustomRoom>();
                corridorRoom.RoomType = RoomType.Corridor;
                levelDescription.AddCorridorConnection(connection1, corridorRoom, roomConfig.RoomTemplates.CorridorRoomTemplates.ToList());

                roomInfo.neighbors.Add(secondRoom);
            }
            else
            {
                var connection = ScriptableObject.CreateInstance<CustomConnection>();
                connection.From = roomToAttachTo.room;
                connection.To = secretRoom;

                // Connect the two rooms with a corridor
                var corridorRoom = ScriptableObject.CreateInstance<CustomRoom>();
                corridorRoom.RoomType = RoomType.Corridor;
                levelDescription.AddCorridorConnection(connection, corridorRoom, roomConfig.RoomTemplates.CorridorRoomTemplates.ToList());

                roomInfo.neighbors.Add(roomToAttachTo.room);
            }

        }
    }
    private List<RoomInfo> GetPossibleRoomAttachToEnemyRoom(LevelDescriptionGrid2D levelDescription, float RoomDeadEndChance)
    {
        //List<CustomRoom> rooms = new List<CustomRoom>();
        var graph = levelDescription.GetGraph();

        // Decide whether to attach the secret room to a dead end room or not
        var attachToDeadEnd = Random.NextDouble() < RoomDeadEndChance;
        return AllRooms.Where(x =>
        (!attachToDeadEnd || x.neighbors.Count() == 1) && x.room.RoomType != RoomType.BirthRoom
        && x.room.RoomType != RoomType.TeleportRoom).ToList();
    }
    private RoomInfo GetRoomWhichNeighborsLessThanFour()
    {
        List<RoomInfo> infos = new List<RoomInfo>();
        foreach (RoomInfo info in AllRooms)
        {
            if (info.room.RoomType != RoomType.BirthRoom && info.room.RoomType != RoomType.TeleportRoom)
            {
                if (info.neighbors.Count < 4)
                {
                    infos.Add(info);
                }
            }
        }
        return infos[Random.Next(infos.Count)];
    }
}
