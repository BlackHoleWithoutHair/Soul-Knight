﻿using System.Collections.Generic;
using UnityEngine;

#region codeBlock:3d_dungeon1_room
namespace Edgar.Unity.Examples.Grid3D.Dungeon1
{
    public class Dungeon1Room : RoomBase
    {
        // Type of the room
        public RoomType Type = RoomType.Basic;

        // Enum for all types of rooms
        public enum RoomType
        {
            Basic = 0, Boss = 1, BossEntrance = 2, Social = 5,
            Entrance = 7, Cave = 8, Trap = 9, Hub = 10
        }

        // This method will not be used as we use a custom logic
        public override List<GameObject> GetRoomTemplates()
        {
            throw new System.NotImplementedException();
        }

        // Use the RoomType enum as display name
        public override string GetDisplayName()
        {
            return Type.ToString();
        }
    }
}
#endregion