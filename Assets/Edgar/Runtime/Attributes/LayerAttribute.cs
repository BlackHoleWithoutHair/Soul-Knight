using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Edgar.Unity
{
    /// <summary>
    /// Attribute to select a single layer.
    /// </summary>
    public class LayerAttribute : PropertyAttribute
    {
        // Empty
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(LayerAttribute))]
    class LayerAttributeEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
        }
    }

#endif
}