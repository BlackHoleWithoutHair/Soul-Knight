using System.Linq;
using UnityEngine;

namespace Edgar.Unity.Examples.Metroidvania
{
    [CreateAssetMenu(menuName = "Edgar/Examples/Metroidvania/Rooftop input setup", fileName = "Metroidvania Rooftop Input Setup")]
    public class MetroidvaniaRooftopInputSetup : DungeonGeneratorInputBaseGrid2D
    {
        public LevelGraph LevelGraph;

        public MetroidvaniaRooftopRoomTemplatesConfig RoomTemplates;

        protected override LevelDescriptionGrid2D GetLevelDescription()
        {
            var levelDescription = new LevelDescriptionGrid2D();

            // Go through individual rooms and add each room to the level description
            foreach (var room in LevelGraph.Rooms.Cast<MetroidvaniaRoom>())
            {
                levelDescription.AddRoom(room, RoomTemplates.GetRoomTemplates(room).ToList());
            }

            foreach (var connection in LevelGraph.Connections.Cast<MetroidvaniaConnection>())
            {
                var from = (MetroidvaniaRoom)connection.From;
                var to = (MetroidvaniaRoom)connection.To;

                // If both rooms are outside, we do not need a corridor room
                if (from.Outside && to.Outside)
                {
                    levelDescription.AddConnection(connection);
                }
                // If at least one room is inside, we need a corridor room to properly connect the two rooms
                else
                {
                    var corridorRoom = ScriptableObject.CreateInstance<MetroidvaniaRoom>();
                    corridorRoom.Type = MetroidvaniaRoomType.Corridor;

                    levelDescription.AddCorridorConnection(connection, corridorRoom, RoomTemplates.InsideCorridorRoomTemplates.ToList());
                }
            }

            return levelDescription;
        }
    }
}