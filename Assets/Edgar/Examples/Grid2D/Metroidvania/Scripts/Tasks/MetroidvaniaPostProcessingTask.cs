using Edgar.Unity.Examples.PC2D;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Edgar.Unity.Examples.Metroidvania
{
    [CreateAssetMenu(menuName = "Edgar/Examples/Metroidvania/Post-processing", fileName = "MetroidvaniaPostProcessing")]
    public class MetroidvaniaPostProcessingTask : DungeonGeneratorPostProcessingGrid2D
    {
        public bool SpawnEnemies;
        public GameObject[] Enemies;

        public bool CreateLevelMap;

        #region codeBlock:2d_metroidvania_setupLevelMap

        public TileBase WallTile;
        public TileBase LevelMapWallTile;
        public TileBase LevelMapWallBackgroundTile;
        public TileBase LevelMapBackgroundTile;
        public TileBase LevelMapPlatformTile;

        #region hide

        public override void Run(DungeonGeneratorLevelGrid2D level)
        {
            SetSpawnPosition(level);
            SetupLayers(level);

            if (SpawnEnemies)
            {
                DoSpawnEnemies(level);
            }

            if (CreateLevelMap)
            {
                SetupLevelMap(level);
            }
        }

        #endregion

        /// <summary>
        /// Setup a schematic level map.
        /// </summary>
        private void SetupLevelMap(DungeonGeneratorLevelGrid2D level)
        {
            // Return if level map not supported
            if (!Application.isPlaying || !MetroidvaniaGameManager.Instance.LevelMapSupported())
            {
                return;
            }

            // Create new tilemap layer for the level map
            var tilemaps = level.GetSharedTilemaps();
            var tilemapsRoot = level.RootGameObject.transform.Find(GeneratorConstantsGrid2D.TilemapsRootName);
            var tilemapObject = new GameObject("LevelMap");
            tilemapObject.transform.SetParent(tilemapsRoot);
            tilemapObject.transform.localPosition = Vector3.zero;
            var tilemap = tilemapObject.AddComponent<Tilemap>();
            var tilemapRenderer = tilemapObject.AddComponent<TilemapRenderer>();
            tilemapRenderer.sortingOrder = 20;

            // Assign special layer
            var mapLayer = LayerMask.NameToLayer(MetroidvaniaGameManager.LevelMapLayer);
            tilemapObject.layer = mapLayer;

            // Copy background tiles
            CopyTilesToLevelMap(level, new[] { "Background", "Other 1" }, tilemap, LevelMapBackgroundTile);

            // Copy wall background tiles
            CopyTilesToLevelMap(level, new[] { "Background" }, tilemap, LevelMapWallBackgroundTile, x => x == WallTile);

            // Copy platforms tiles
            CopyTilesToLevelMap(level, new[] { "Platforms" }, tilemap, LevelMapPlatformTile);

            // Copy wall tiles
            CopyTilesToLevelMap(level, new[] { "Walls" }, tilemap, LevelMapWallTile);
        }

        #endregion

        /// <summary>
        /// Platformer controller script needs to know the layer mask for static environment.
        /// However, Unity does not provide API to create layers from scripts.
        /// Therefore, we do all the setup here if we detect the that layer exists.
        ///
        /// This setup is only needed if the scripts are distributed as an asset to other users.
        /// Feel free to skip this if you develop a personal project.
        /// </summary>
        /// <param name="level"></param>
        private void SetupLayers(DungeonGeneratorLevelGrid2D level)
        {
            var environmentLayer = LayerMask.NameToLayer(MetroidvaniaGameManager.StaticEnvironmentLayer);

            // Return if the layers does not exist.
            if (environmentLayer == -1)
            {
                return;
            }

            // Set the environment layer for all the instances of room templates
            foreach (var roomInstance in level.RoomInstances)
            {
                foreach (var tilemap in RoomTemplateUtilsGrid2D.GetTilemaps(roomInstance.RoomTemplateInstance))
                {
                    tilemap.gameObject.layer = environmentLayer;
                }
            }

            // Set the environment layer for all the shared tilemaps
            foreach (var tilemap in level.GetSharedTilemaps())
            {
                tilemap.gameObject.layer = environmentLayer;
            }

            // Set the environment layer inside the motor script
            var player = GameObject.FindGameObjectWithTag("Player");
            var motor = player.GetComponent<PlatformerMotor2D>();
            motor.staticEnvLayerMask = LayerMask.GetMask(MetroidvaniaGameManager.StaticEnvironmentLayer);
        }

        #region codeBlock:2d_metroidvania_spawnEnemies

        /// <summary>
        /// Spawn enemies
        /// </summary>
        /// <remarks>
        /// The method is not named "SpawnEnemies" because there is already a public field with that name.
        /// </remarks>
        private void DoSpawnEnemies(DungeonGeneratorLevelGrid2D level)
        {
            // Check that we have at least one enemy to choose from
            if (Enemies == null || Enemies.Length == 0)
            {
                throw new InvalidOperationException("There must be at least one enemy prefab to spawn enemies");
            }

            // Go through individual rooms
            foreach (var roomInstance in level.RoomInstances)
            {
                var roomTemplate = roomInstance.RoomTemplateInstance;

                // Find the game object that holds all the spawn points
                var enemySpawnPoints = roomTemplate.transform.Find("EnemySpawnPoints");

                if (enemySpawnPoints != null)
                {
                    // Go through individual spawn points and choose a random enemy to spawn
                    foreach (Transform enemySpawnPoint in enemySpawnPoints)
                    {
                        var enemyPrefab = Enemies[Random.Next(Enemies.Length)];
                        var enemy = Instantiate(enemyPrefab);
                        enemy.transform.parent = roomTemplate.transform;
                        enemy.transform.position = enemySpawnPoint.position;
                    }
                }
            }
        }

        #endregion

        #region codeBlock:2d_metroidvania_setSpawnPosition

        /// <summary>
        /// Move the player to the spawn point of the level.
        /// </summary>
        /// <param name="level"></param>
        private void SetSpawnPosition(DungeonGeneratorLevelGrid2D level)
        {
            // Find the room with the Entrance type
            var entranceRoomInstance = level
                .RoomInstances
                .FirstOrDefault(x => ((MetroidvaniaRoom)x.Room).Type == MetroidvaniaRoomType.Entrance);

            if (entranceRoomInstance == null)
            {
                throw new InvalidOperationException("Could not find Entrance room");
            }

            var roomTemplateInstance = entranceRoomInstance.RoomTemplateInstance;

            // Find the spawn position marker
            var spawnPosition = roomTemplateInstance.transform.Find("SpawnPosition");

            // Move the player to the spawn position
            var player = GameObject.FindWithTag("Player");
            player.transform.position = spawnPosition.position;
        }

        #endregion

        /// <summary>
        /// Copy tiles from given source tilemaps to the level map tilemap.
        /// Instead of using the original tiles, we use a given level map tile (which is usually only a single color).
        /// If we want to copy only some of the tiles, we can provide a tile filter function.
        /// </summary>
        private void CopyTilesToLevelMap(DungeonGeneratorLevelGrid2D level, ICollection<string> sourceTilemapNames, Tilemap levelMapTilemap, TileBase levelMapTile, Predicate<TileBase> tileFilter = null)
        {
            // Go through the tilemaps with the correct name
            foreach (var sourceTilemap in level.GetSharedTilemaps().Where(x => sourceTilemapNames.Contains(x.name)))
            {
                // Go through positions inside the bounds of the tilemap
                foreach (var tilemapPosition in sourceTilemap.cellBounds.allPositionsWithin)
                {
                    // Check if there is a tile at a given position
                    var originalTile = sourceTilemap.GetTile(tilemapPosition);

                    if (originalTile != null)
                    {
                        // If a tile filter is provided, use it to check if the predicate holds
                        if (tileFilter != null)
                        {
                            if (tileFilter(originalTile))
                            {
                                levelMapTilemap.SetTile(tilemapPosition, levelMapTile);
                            }
                        }
                        // Otherwise set the levelMapTile to the correct position
                        else
                        {
                            levelMapTilemap.SetTile(tilemapPosition, levelMapTile);
                        }
                    }
                }
            }
        }
    }
}