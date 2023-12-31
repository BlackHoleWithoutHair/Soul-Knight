using Edgar.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomPostProcessing : DungeonGeneratorPostProcessingComponentGrid2D
{
    private List<RoomInstanceGrid2D> roomInstances;
    public override void Run(DungeonGeneratorLevelGrid2D level)
    {
        roomInstances = level.RoomInstances;
        foreach (var roomInstance in level.RoomInstances)
        {
            var roomTemplateInstance = roomInstance.RoomTemplateInstance;

            // Find floor tilemap layer
            var tilemaps = RoomTemplateUtilsGrid2D.GetTilemaps(roomTemplateInstance);
            var floor = tilemaps.Single(x => x.name == "Floor").gameObject;
            // Add floor collider
            AddFloorCollider(floor);
            //floor.AddComponent<CurrentRoomDetectionTriggerHandler>();

        }
    }

    private void AddFloorCollider(GameObject floor)
    {
        var tilemapCollider2D = floor.AddComponent<TilemapCollider2D>();
        tilemapCollider2D.usedByComposite = true;

        var compositeCollider2d = floor.AddComponent<CompositeCollider2D>();
        compositeCollider2d.geometryType = CompositeCollider2D.GeometryType.Polygons;
        compositeCollider2d.isTrigger = true;
        compositeCollider2d.generationType = CompositeCollider2D.GenerationType.Synchronous;
        floor.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }
    public List<RoomInstanceGrid2D> GetRoomInstances()
    {
        return roomInstances;
    }
}
