using UnityEditor;
using UnityEngine;

namespace Edgar.Unity
{
    public class HelpBoxAttribute : PropertyAttribute
    {
        public readonly string Text;

        // MessageType exists in UnityEditor namespace and can throw an exception when used outside the editor.
        // We spoof MessageType at the bottom of this script to ensure that errors are not thrown when
        // MessageType is unavailable.
        public readonly MessageType Type;

        /// <summary>
        /// Adds a HelpBox to the Unity property inspector above this field.
        /// </summary>
        /// <param name="text">The help text to be displayed in the HelpBox.</param>
        /// <param name="type">The icon to be displayed in the HelpBox.</param>
        public HelpBoxAttribute(string text, MessageType type = MessageType.Info)
        {
            this.Text = text;
            this.Type = type;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxAttributeDrawer : DecoratorDrawer
    {
        // Used for top and bottom padding between the text and the HelpBox border.
        const int paddingHeight = 8;

        // Used to add some margin between the the HelpBox and the property.
        const int marginHeight = 2;

        //  Global field to store the original (base) property height.
        float baseHeight = 0;

        private HelpBoxAttribute HelpBox => ((HelpBoxAttribute)attribute);

        public override float GetHeight()
        {
            var content = new GUIContent(HelpBox.Text);
            var style = GUI.skin.GetStyle("helpbox");
            var height = style.CalcHeight(content, EditorGUIUtility.currentViewWidth);
            height += marginHeight * 2;

            return height;
        }

        public override void OnGUI(Rect position)
        {
            var helpPos = position;

            helpPos.height -= baseHeight + marginHeight;

            // Renders the HelpBox in the Unity inspector UI.
            EditorGUI.HelpBox(helpPos, HelpBox.Text, HelpBox.Type);
        }
    }
#else
    // Replicate MessageType Enum if we are not in editor as this enum exists in UnityEditor namespace.
    // This should stop errors being logged the same as Shawn Featherly's commit in the Github repo but I
    // feel is cleaner than having the conditional directive in the middle of the HelpAttribute constructor.
    public enum MessageType
    {
        None,
        Info,
        Warning,
        Error,
    }
#endif
}