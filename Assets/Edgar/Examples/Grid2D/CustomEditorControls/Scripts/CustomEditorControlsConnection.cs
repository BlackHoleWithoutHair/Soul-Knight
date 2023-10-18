using UnityEngine;

namespace Edgar.Unity.Examples.CustomEditorControls
{
    /// <summary>
    /// Simple custom connection to demonstrate custom editor controls.
    /// </summary>
    public class CustomEditorControlsConnection : Connection
    {
        /// <summary>
        /// Whether the connection/corridor is locked.
        /// </summary>
        public bool IsLocked = false;

        /// <summary>
        /// Simple example of a custom editor control for the CustomEditorControlsConnection connection type.
        /// </summary>
        [CustomConnectionControl(typeof(CustomEditorControlsConnection))]
        private class Control : ConnectionControl
        {
            private static readonly Texture2D LockIconTexture = CustomEditorControlsIcons.Base64ToTexture(CustomEditorControlsIcons.LockIcon);

            /// <summary>
            /// Override the Draw method to draw a custom control.
            /// </summary>
            public override void Draw(Vector2 gridOffset, float zoom, bool isDirected)
            {
                base.Draw(gridOffset, zoom, isDirected);

                // Draw the lock icon if the connection is locked
                var connection = Connection as CustomEditorControlsConnection;
                if (connection != null && connection.IsLocked)
                {
                    var rect = GetHandleRect(gridOffset, zoom);
                    rect.y -= rect.height * 1.05f;
                    GUI.DrawTexture(rect, LockIconTexture);
                }
            }
        }
    }
}