using UnityEngine;
[System.Serializable]
public class RoomTamplatesConfig
{
    public GameObject[] BasicRoomTemplates;

    public GameObject[] BossRoomTemplates;

    public GameObject[] EnemyRoomTemplates;

    public GameObject[] EliteEnemyRoomTemplates;

    public GameObject[] CorridorRoomTemplates;

    public GameObject[] BirthRoomTemplates;

    public GameObject[] TeleportRoomTemplates;

    public GameObject[] HubRoomTemplates;

    public GameObject[] TreasureRoomTemplates;

    public GameObject[] ShopRoomTemplates;

    public GameObject[] SecretRoomTemplates;

    /// <summary>
    /// Get room templates for a given room.
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    public GameObject[] GetRoomTemplates(CustomRoom room)
    {
        switch (room.RoomType)
        {
            case RoomType.BossRoom:
                return BossRoomTemplates;

            case RoomType.EnemyRoom:
                return EnemyRoomTemplates;

            case RoomType.EliteEnemyRoom:
                return EliteEnemyRoomTemplates;

            case RoomType.ShopRoom:
                return ShopRoomTemplates;

            case RoomType.TreasureRoom:
                return TreasureRoomTemplates;

            case RoomType.BirthRoom:
                return BirthRoomTemplates;

            case RoomType.TeleportRoom:
                return TeleportRoomTemplates;

            case RoomType.SecretRoom:
                return SecretRoomTemplates;

            default:
                return BasicRoomTemplates;
        }
    }
}
