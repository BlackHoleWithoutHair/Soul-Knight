using Edgar.GraphBasedGenerator.Common.Doors;
using UnityEngine;

namespace Edgar.Unity
{
    /// <summary>
    /// Base class for door sockets.
    /// </summary>
    /// <remarks>
    /// Inherit from this class if you need more control over the sockets than the DoorSocket class provides.
    /// </remarks>
    public abstract class DoorSocketBase : ScriptableObject, IDoorSocket
    {
        /// <summary>
        /// Decides whether this socket is compatible with the other socket.
        /// </summary>
        /// <param name="otherSocket"></param>
        /// <returns></returns>
        public abstract bool IsCompatibleWith(IDoorSocket otherSocket);

        /// <summary>
        /// Returns the color that should be used for this socket when creating room templates.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetColor()
        {
            return Color.red;
        }
    }
}