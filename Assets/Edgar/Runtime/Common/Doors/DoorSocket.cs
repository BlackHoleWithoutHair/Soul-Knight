using Edgar.GraphBasedGenerator.Common.Doors;
using UnityEngine;

namespace Edgar.Unity
{
    /// <summary>
    /// Basic implementation of door sockets. Two sockets are compatible if they are the same instances.
    /// </summary>
    [CreateAssetMenu(menuName = "Edgar/Door socket", fileName = "DoorSocket")]
    public class DoorSocket : DoorSocketBase
    {
        public Color Color = Color.red;

        public override bool IsCompatibleWith(IDoorSocket otherSocket)
        {
            return ReferenceEquals(this, otherSocket);
        }

        public override Color GetColor()
        {
            return Color;
        }
    }
}