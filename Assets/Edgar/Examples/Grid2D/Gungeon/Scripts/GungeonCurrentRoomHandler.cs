using UnityEngine;

namespace Edgar.Unity.Examples.Gungeon
{
    public class GungeonCurrentRoomHandler : MonoBehaviour
    {
        private GungeonRoomManager roomManager;
        private RoomInstanceGrid2D roomInstance;

        public void Start()
        {
            var parent = transform.parent.parent;
            roomManager = parent.gameObject.GetComponent<GungeonRoomManager>();
            roomInstance = parent.gameObject.GetComponent<RoomInfoGrid2D>().RoomInstance;
        }

        public void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (otherCollider.gameObject.tag == "Player")
            {
                roomManager?.OnRoomEnter(otherCollider.gameObject);

                // Handle Fog of War
                if (roomInstance.IsCorridor)
                {
                    FogOfWarGrid2D.Instance?.RevealRoomAndNeighbors(roomInstance);
                }
            }
        }

        public void OnTriggerExit2D(Collider2D otherCollider)
        {
            if (otherCollider.gameObject.tag == "Player")
            {
                roomManager?.OnRoomLeave(otherCollider.gameObject);
            }
        }
    }
}