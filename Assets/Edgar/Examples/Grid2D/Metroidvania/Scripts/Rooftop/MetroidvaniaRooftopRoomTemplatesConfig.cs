using System;
using UnityEngine;

namespace Edgar.Unity.Examples.Metroidvania
{
    [Serializable]
    public class MetroidvaniaRooftopRoomTemplatesConfig
    {
        public GameObject[] EntranceRoomTemplates;

        public GameObject[] ExitRoomTemplates;

        public GameObject[] OutsideNormalRoomTemplates;

        public GameObject[] OutsideTeleportRoomTemplates;

        public GameObject[] InsideTreasureRoomTemplates;

        public GameObject[] InsideTeleportRoomTemplates;

        public GameObject[] InsideNormalRoomTemplates;

        public GameObject[] InsideShopRoomTemplates;

        public GameObject[] InsideCorridorRoomTemplates;

        public GameObject[] GetRoomTemplates(MetroidvaniaRoom room)
        {
            if (room.Outside)
            {
                switch (room.Type)
                {
                    case MetroidvaniaRoomType.Entrance:
                        return EntranceRoomTemplates;

                    case MetroidvaniaRoomType.Exit:
                        return ExitRoomTemplates;

                    case MetroidvaniaRoomType.Teleport:
                        return OutsideTeleportRoomTemplates;

                    case MetroidvaniaRoomType.Normal:
                        return OutsideNormalRoomTemplates;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                switch (room.Type)
                {
                    case MetroidvaniaRoomType.Teleport:
                        return InsideTeleportRoomTemplates;

                    case MetroidvaniaRoomType.Treasure:
                        return InsideTreasureRoomTemplates;

                    case MetroidvaniaRoomType.Shop:
                        return InsideShopRoomTemplates;

                    case MetroidvaniaRoomType.Normal:
                        return InsideNormalRoomTemplates;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}