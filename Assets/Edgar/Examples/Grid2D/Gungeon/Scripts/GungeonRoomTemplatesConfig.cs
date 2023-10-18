using System;
using UnityEngine;

namespace Edgar.Unity.Examples.Gungeon
{
    [Serializable]
    public class GungeonRoomTemplatesConfig
    {
        public GameObject[] BasicRoomTemplates;

        public GameObject[] BossFoyersRoomTemplates;

        public GameObject[] BossRoomTemplates;

        public GameObject[] CorridorRoomTemplates;

        public GameObject[] EntranceRoomTemplates;

        public GameObject[] ExitRoomTemplates;

        public GameObject[] HubRoomTemplates;

        public GameObject[] RewardRoomTemplates;

        public GameObject[] ShopRoomTemplates;

        public GameObject[] SecretRoomTemplates;

        /// <summary>
        /// Get room templates for a given room.
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public GameObject[] GetRoomTemplates(GungeonRoom room)
        {
            switch (room.Type)
            {
                case GungeonRoomType.Boss:
                    return BossRoomTemplates;

                case GungeonRoomType.BossFoyers:
                    return BossFoyersRoomTemplates;

                case GungeonRoomType.Shop:
                    return ShopRoomTemplates;

                case GungeonRoomType.Reward:
                    return RewardRoomTemplates;

                case GungeonRoomType.Hub:
                    return HubRoomTemplates;

                case GungeonRoomType.Entrance:
                    return EntranceRoomTemplates;

                case GungeonRoomType.Exit:
                    return ExitRoomTemplates;

                case GungeonRoomType.Secret:
                    return SecretRoomTemplates;

                case GungeonRoomType.Normal:
                    return BasicRoomTemplates;

                default:
                    return BasicRoomTemplates;
            }
        }
    }
}