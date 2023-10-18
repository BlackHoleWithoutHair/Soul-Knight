using Edgar.Geometry;
using System;
using UnityEngine;

namespace Edgar.Unity
{
    [Serializable]
    public class DoorLineGrid2D : IDoorLine
    {
        [HideInInspector]
        public Vector3Int From;

        [HideInInspector]
        public Vector3Int To;

        public int Length;

        public DoorDirection Direction;

        public DoorSocketBase Socket;

        Vector3Int IDoorLine.From => From;

        Vector3Int IDoorLine.To => To;

        int IDoorLine.Length => Length;

        internal GraphBasedGenerator.Grid2D.DoorLineGrid2D ToInternal()
        {
            var line = new OrthogonalLineGrid2D(From.ToCustomIntVector2(), To.ToCustomIntVector2());

            if (Length > 1)
            {
                line = line.Shrink(0, Length - 1);
            }

            return new GraphBasedGenerator.Grid2D.DoorLineGrid2D(
                line,
                Length - 1,
                Socket,
                DoorsGrid2D.GetDoorType(Direction));
        }

        #region Equals

        protected bool Equals(DoorLineGrid2D other)
        {
            return From.Equals(other.From) && To.Equals(other.To) && Length == other.Length && Direction == other.Direction && Equals(Socket, other.Socket);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DoorLineGrid2D)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = From.GetHashCode();
                hashCode = (hashCode * 397) ^ To.GetHashCode();
                hashCode = (hashCode * 397) ^ Length;
                hashCode = (hashCode * 397) ^ (int)Direction;
                hashCode = (hashCode * 397) ^ (Socket != null ? Socket.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(DoorLineGrid2D left, DoorLineGrid2D right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DoorLineGrid2D left, DoorLineGrid2D right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}