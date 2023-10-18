using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Edgar.Unity.Examples.Metroidvania
{
    [CreateAssetMenu(menuName = "Edgar/Examples/Metroidvania/Rooftop post-processing", fileName = "MetroidvaniaRooftopPostProcessing")]
    public class MetroidvaniaRooftopPostProcessTask : DungeonGeneratorPostProcessingGrid2D
    {
        public bool AddWalls = true;

        public int WallDepth = 100;

        public TileBase WallTile;

        private Tilemap wallsTilemap;

        private Tilemap backgroundTilemap;

        #region codeBlock:2d_metroidvania_registerCallbacks

        public override void RegisterCallbacks(DungeonGeneratorCallbacksGrid2D callbacks)
        {
            if (AddWalls)
            {
                callbacks.RegisterCallbackAfter(PostProcessPrioritiesGrid2D.InitializeSharedTilemaps, AddWallsUnderRooms);
            }
        }

        #endregion

        private void AddWallsUnderRooms(DungeonGeneratorLevelGrid2D level)
        {
            // Store the "Walls" and "Background" tilemaps
            var tilemaps = level.GetSharedTilemaps();
            wallsTilemap = tilemaps.Single(x => x.name == "Walls");
            backgroundTilemap = tilemaps.Single(x => x.name == "Background");

            // Add walls under outside rooms
            foreach (var roomInstance in level.RoomInstances)
            {
                var room = (MetroidvaniaRoom)roomInstance.Room;

                if (room.Outside)
                {
                    AddWallsUnderRoom(roomInstance);
                }
            }
        }

        #region codeBlock:2d_metroidvania_addWalls

        private void AddWallsUnderRoom(RoomInstanceGrid2D roomInstance)
        {
            // Get the room template and all the used tiles
            var roomTemplate = roomInstance.RoomTemplateInstance;
            var tilemaps = RoomTemplateUtilsGrid2D.GetTilemaps(roomTemplate);
            var usedTiles = RoomTemplateLoaderGrid2D.GetUsedTiles(tilemaps).ToList();
            var roomTemplateWalls = tilemaps.Single(x => x.name == "Walls");

            // Find the minimum y coordinate of all the tiles and use it to find the bottom layer of tiles
            var minY = usedTiles.Min(x => x.y);
            var bottomLayerTiles = usedTiles.Where(x => x.y == minY).ToList();

            foreach (var pos in bottomLayerTiles)
            {
                var tilemap = backgroundTilemap;

                // Use the walls tilemap only if the collider is really needed
                // That means we only use it if the tile is the border tile of a tower
                var leftTilePos = pos + Vector3Int.left;
                var rightTilePos = pos + Vector3Int.right;
                if (roomTemplateWalls.GetTile(pos) != null && !(bottomLayerTiles.Contains(leftTilePos) && bottomLayerTiles.Contains(rightTilePos)))
                {
                    tilemap = wallsTilemap;
                }

                // Add tiles under this position
                for (var i = 1; i <= WallDepth; i++)
                {
                    var wallPosition = roomInstance.Position + pos + Vector3Int.down * i;
                    tilemap.SetTile(wallPosition, WallTile);
                }
            }
        }

        #endregion
    }
}