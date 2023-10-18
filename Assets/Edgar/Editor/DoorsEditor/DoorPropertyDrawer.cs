using UnityEditor;
using UnityEngine;

namespace Edgar.Unity.Editor
{
    [CustomPropertyDrawer(typeof(DoorGrid2D))]
    [CustomPropertyDrawer(typeof(DoorLineGrid2D))]
    public class DoorPropertyDrawer : PropertyDrawer
    {
        private const int LineHeight = 16;
        private const int LineMargin = 2;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var rect = position;
            rect.height = LineHeight;

            EditorGUI.PrefixLabel(rect, label, EditorStyles.boldLabel);

            rect.y += LineHeight + LineMargin;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative(nameof(DoorGrid2D.Direction)));

            rect.y += LineHeight + LineMargin;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative(nameof(DoorGrid2D.Socket)));
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var propertyCount = 2;

            return LineHeight * (propertyCount + 1) + LineMargin * propertyCount;
        }
    }
}