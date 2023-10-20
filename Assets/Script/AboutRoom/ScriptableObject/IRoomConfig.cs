using Edgar.Unity;
using System.Collections.Generic;
using UnityEngine;

public class IRoomConfig : ScriptableObject
{
    public LevelGraph LevelGraph;
    public LevelGraph LevelGraphBoss;
    public List<LevelGraph> levelGraphs;
    public bool UseRandomLevelGraph;
    public float ExtraEnemyRoomChance;
    public float ExtraEnemyRoomDeadEndChance;
    public float TreasureRoomDeadEndChance;
    public float SecretRoomChance;
    public float SecretRoomDeadEndChance;

    public RoomTamplatesConfig RoomTemplates;
}
