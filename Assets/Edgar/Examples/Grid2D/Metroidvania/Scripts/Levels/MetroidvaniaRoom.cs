using System.Collections.Generic;
using UnityEngine;

namespace Edgar.Unity.Examples.Metroidvania
{
    #region codeBlock:2d_metroidvania_room

    public class MetroidvaniaRoom : RoomBase
    {
        public MetroidvaniaRoomType Type;

        public bool Outside;

        public override List<GameObject> GetRoomTemplates()
        {
            // We do not need any room templates here because they are resolved based on the type of the room.
            return null;
        }

        public override string GetDisplayName()
        {
            // Use the type of the room as its display name.
            return Type.ToString();
        }
    }

    #endregion
}