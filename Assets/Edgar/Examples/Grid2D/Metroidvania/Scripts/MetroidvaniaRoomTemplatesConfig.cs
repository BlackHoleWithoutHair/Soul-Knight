using System;
using UnityEngine;

namespace Edgar.Unity.Examples.Metroidvania
{
    #region codeBlock:2d_metroidvania_roomTemplatesConfig

    [Serializable]
    public class MetroidvaniaRoomTemplatesConfig
    {
        public GameObject[] DefaultRoomTemplates;

        public GameObject[] ShopRoomTemplates;

        #region hide

        public GameObject[] TeleportRoomTemplates;

        public GameObject[] TreasureRoomTemplates;

        public GameObject[] EntranceRoomTemplates;

        public GameObject[] ExitRoomTemplates;

        public GameObject[] CorridorRoomTemplates;

        #endregion


        public GameObject[] GetRoomTemplates(MetroidvaniaRoom room)
        {
            switch (room.Type)
            {
                case MetroidvaniaRoomType.Shop:
                    return ShopRoomTemplates;

                #region hide

                case MetroidvaniaRoomType.Teleport:
                    return TeleportRoomTemplates;

                case MetroidvaniaRoomType.Treasure:
                    return TreasureRoomTemplates;

                case MetroidvaniaRoomType.Entrance:
                    return EntranceRoomTemplates;

                case MetroidvaniaRoomType.Exit:
                    return ExitRoomTemplates;

                #endregion

                default:
                    return DefaultRoomTemplates;
            }
        }
    }

    #endregion
}