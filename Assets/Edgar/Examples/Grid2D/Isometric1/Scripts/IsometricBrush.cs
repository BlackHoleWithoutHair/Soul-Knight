#if UNITY_EDITOR
#if OndrejNepozitekEdgarLegacy
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Edgar.Unity.Examples.Isometric1
{
    [CustomGridBrush(true, false, false, "Isometric Brush")]
    public class IsometricBrush : GridBrush
    {
    }

    [CustomEditor(typeof(IsometricBrush))]
    public class IsometricBrushEditor : GridBrushEditor
    {
        public override GameObject[] validTargets
        {
            get
            {
                var vt = base.validTargets;
                Array.Sort(vt, (go1, go2) =>
                {
                    var tilemapRenderer1 = go1.GetComponent<TilemapRenderer>();
                    var tilemapRenderer2 = go2.GetComponent<TilemapRenderer>();

                    var order1 = tilemapRenderer1 == null ? 0 : tilemapRenderer1.sortingOrder;
                    var order2 = tilemapRenderer2 == null ? 0 : tilemapRenderer2.sortingOrder;

                    return order1.CompareTo(order2);
                });
                return vt;
            }
        }
    }
}
#endif
#endif