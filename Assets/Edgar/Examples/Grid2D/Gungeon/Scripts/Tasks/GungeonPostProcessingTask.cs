using System;
using System.Linq;
using UnityEngine;

namespace Edgar.Unity.Examples.Gungeon
{
    #region codeBlock:2d_gungeon_doors

    [CreateAssetMenu(menuName = "Edgar/Examples/Gungeon/Post-processing", fileName = "GungeonPostProcessing")]
    public class GungeonPostProcessingTask : DungeonGeneratorPostProcessingGrid2D
    {
        public GameObject[] Enemies;
        public override void Run(DungeonGeneratorLevelGrid2D level)
        {
            #region hide

            MovePlayerToSpawn(level);

            // The instance of the game manager will not exist in Editor
            if (GungeonGameManager.Instance != null)
            {
                // Set the Random instance of the GameManager to be the same instance as we use in the generator
                GungeonGameManager.Instance.Random = Random;
            }

            #endregion

            foreach (var roomInstance in level.RoomInstances)
            {
                var room = (GungeonRoom)roomInstance.Room;
                var roomTemplateInstance = roomInstance.RoomTemplateInstance;

                // Find floor tilemap layer
                var tilemaps = RoomTemplateUtilsGrid2D.GetTilemaps(roomTemplateInstance);
                var floor = tilemaps.Single(x => x.name == "Floor").gameObject;

                // Add current room detection handler
                floor.AddComponent<GungeonCurrentRoomHandler>();

                // Add room manager
                var roomManager = roomTemplateInstance.AddComponent<GungeonRoomManager>();

                if (room.Type != GungeonRoomType.Corridor)
                {
                    // Set enemies and floor collider to the room manager
                    roomManager.Enemies = Enemies;
                    roomManager.FloorCollider = floor.GetComponent<CompositeCollider2D>();

                    // Find all the doors of neighboring corridors and save them in the room manager
                    // The term "door" has two different meanings here:
                    //   1. it represents the connection point between two rooms in the level
                    //   2. it represents the door game object that we have inside each corridor
                    foreach (var door in roomInstance.Doors)
                    {
                        // Get the room instance of the room that is connected via this door
                        var corridorRoom = door.ConnectedRoomInstance;

                        // Get the room template instance of the corridor room
                        var corridorGameObject = corridorRoom.RoomTemplateInstance;

                        // Find the door game object by its name
                        var doorsGameObject = corridorGameObject.transform.Find("Door")?.gameObject;

                        // Each corridor room instance has a connection that represents the edge in the level graph
                        // We use the connection object to check if the corridor should be locked or not
                        var connection = (GungeonConnection)corridorRoom.Connection;

                        if (doorsGameObject != null)
                        {
                            // If the connection is locked, we set the Locked state and keep the game object active
                            // Otherwise we set the EnemyLocked state and deactivate the door. That means that the door is active and locked
                            // only when there are enemies in the room.
                            if (connection.IsLocked)
                            {
                                doorsGameObject.GetComponent<GungeonDoor>().State = GungeonDoor.DoorState.Locked;
                            }
                            else
                            {
                                doorsGameObject.GetComponent<GungeonDoor>().State = GungeonDoor.DoorState.EnemyLocked;
                                doorsGameObject.SetActive(false);
                            }

                            roomManager.Doors.Add(doorsGameObject);
                        }
                    }
                }
            }

            #region hide

            SetupFogOfWar(level);

            #endregion
        }

        #region hide

        private void SetupFogOfWar(DungeonGeneratorLevelGrid2D level)
        {
            // To setup the FogOfWar component, we need to get the root game object that holds the level.
            var generatedLevelRoot = level.RootGameObject;

            // If we use the Wave mode, we must specify the point from which the wave spreads as we reveal a room.
            // The easiest way to do so is to get the player game object and use its transform as the wave origin.
            // Change this line if your player game object does not have the "Player" tag.
            var player = GameObject.FindGameObjectWithTag("Player");

            // Now we can setup the FogOfWar component.
            // To make it easier to work with the component, the class is a singleton and provides the Instance property.
            FogOfWarGrid2D.Instance?.Setup(generatedLevelRoot, player.transform);

            // After the level is generated, we usually want to reveal the spawn room.
            // To do that, we have to find the room instance that corresponds to the Spawn room.
            // In this example, the spawn room has the GungeonRoomType.Entrance type.
            var spawnRoom = level
                .RoomInstances
                .SingleOrDefault(x => ((GungeonRoom)x.Room).Type == GungeonRoomType.Entrance);

            if (spawnRoom == null)
            {
                throw new InvalidOperationException("There must be exactly one room with the name 'Spawn' for this example to work.");
            }

            // When we have the spawn room instance, we can reveal the room from the fog.
            // We use revealImmediately: true so that the first room is revealed instantly,
            // but it is optional.
            FogOfWarGrid2D.Instance?.RevealRoom(spawnRoom, revealImmediately: true);
        }

        /// <summary>
        /// Move the player to the spawn position
        /// </summary>
        private void MovePlayerToSpawn(DungeonGeneratorLevelGrid2D level)
        {
            foreach (var roomInstance in level.RoomInstances)
            {
                var room = (GungeonRoom)roomInstance.Room;
                var roomTemplateInstance = roomInstance.RoomTemplateInstance;

                // Get spawn position if Entrance
                if (room.Type == GungeonRoomType.Entrance)
                {
                    var spawnPosition = roomTemplateInstance.transform.Find("SpawnPosition");
                    var player = GameObject.FindWithTag("Player");
                    player.transform.position = spawnPosition.position;
                }
            }
        }

        #endregion
    }

    #endregion
}