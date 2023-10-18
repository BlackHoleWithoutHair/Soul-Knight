using System;
using System.Collections.Generic;
using UnityEngine;

#region codeBlock:3d_dungeon1_roomTemplates
namespace Edgar.Unity.Examples.Grid3D.Dungeon1
{
    [Serializable]
    public class Dungeon1RoomTemplates
    {
        public List<GameObject> Basic;
        public List<GameObject> Boss;
        public List<GameObject> BossEntrance;

        #region hide

        public List<GameObject> Social;
        public List<GameObject> Entrance;
        public List<GameObject> Corridors;
        public List<GameObject> Cave;
        public List<GameObject> Trap;

        #endregion

        public List<GameObject> Hub;

        public List<GameObject> GetRoomTemplates(Dungeon1Room.RoomType roomType)
        {
            switch (roomType)
            {
                case Dungeon1Room.RoomType.Basic:
                    return Basic;
                case Dungeon1Room.RoomType.Boss:
                    return Boss;
                case Dungeon1Room.RoomType.BossEntrance:
                    return BossEntrance;

                #region hide

                case Dungeon1Room.RoomType.Social:
                    return Social;
                case Dungeon1Room.RoomType.Entrance:
                    return Entrance;
                case Dungeon1Room.RoomType.Cave:
                    return Cave;
                case Dungeon1Room.RoomType.Trap:
                    return Trap;

                #endregion

                case Dungeon1Room.RoomType.Hub:
                    return Hub;
                default:
                    throw new ArgumentOutOfRangeException(nameof(roomType), roomType, null);
            }
        }
    }
}
#endregion