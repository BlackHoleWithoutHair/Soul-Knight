using Edgar.Geometry;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Edgar.Unity
{
    // TODO(Grid3D): This class needs refactoring
    public static class UtilsGrid3D
    {
        public static int IndexOf<T>(this IReadOnlyList<T> self, T elementToFind)
        {
            int i = 0;
            foreach (T element in self)
            {
                if (Equals(element, elementToFind))
                    return i;
                i++;
            }

            return -1;
        }

        public static Vector3Int To3DSpace(this EdgarVector2Int vector)
        {
            return new Vector3Int(vector.X, 0, vector.Y);
        }

        public static Vector3Int To3DSpace(this Vector3Int vector)
        {
            return new Vector3Int(vector.x, 0, vector.y);
        }

        internal static TComponent RemoveThenAddComponent<TComponent>(this GameObject o) where TComponent : MonoBehaviour
        {
            var component = o.GetComponent<TComponent>();

            if (component != null)
            {
                Destroy(component);
            }

            return o.AddComponent<TComponent>();
        }

        internal static Vector3 PrecisionRound(this Vector3 vector)
        {
            return vector.ApplyPointwise(PrecisionRound);
        }

        internal static float PrecisionRound(float number)
        {
            var tolerance = 0.001f;
            var floor = Mathf.Floor(number);
            var ceil = Mathf.Ceil(number);

            if (Math.Abs(floor - number) < tolerance)
            {
                return floor;
            }

            if (Math.Abs(ceil - number) < tolerance)
            {
                return ceil;
            }

            return number;
        }

        internal static Vector3 ApplyPointwise(this Vector3 vector, Func<float, float> func)
        {
            return new Vector3(func(vector.x), func(vector.y), func(vector.z));
        }

        internal static Vector3Int Round(this Vector3 vector)
        {
            return new Vector3Int((int)Math.Round(vector.x), (int)Math.Round(vector.y), (int)Math.Round(vector.z));
        }

        internal static Vector3Int Floor(this Vector3 vector)
        {
            return new Vector3Int((int)Math.Floor(vector.x), (int)Math.Floor(vector.y), (int)Math.Floor(vector.z));
        }

        internal static Vector3Int Ceil(this Vector3 vector)
        {
            return new Vector3Int((int)Math.Ceiling(vector.x), (int)Math.Ceiling(vector.y), (int)Math.Ceiling(vector.z));
        }

        // TODO: move later
        internal static void Destroy(Object gameObject)
        {
            if (Application.isPlaying)
            {
                Object.Destroy(gameObject);
            }
            else
            {
                Object.DestroyImmediate(gameObject);
            }
        }
    }
}