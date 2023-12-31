﻿using System.Collections.Generic;
using UnityEngine;

namespace Edgar.Unity.Examples.CustomEditorControls
{
    #region codeBlock:2d_customEditorControls_customRoom

    /// <summary>
    /// Simple custom room to demonstrate custom editor controls.
    /// </summary>
    public class CustomEditorControlsRoom : RoomBase
    {
        /// <summary>
        /// Each room has a type that determines the display name and icon.
        /// </summary>
        public enum RoomType
        {
            Basic,
            Treasure,
            Shop,
            Boss,
            Spawn
        }

        /// <summary>
        /// Type of the room.
        /// </summary>
        public RoomType Type = RoomType.Basic;

        public override List<GameObject> GetRoomTemplates()
        {
            // Not implemented in this example
            throw new System.NotImplementedException();
        }

        public override string GetDisplayName()
        {
            return Type.ToString();
        }

        #region hide

        #region codeBlock:2d_customEditorControls_roomControl

        /// <summary>
        /// Simple example of a custom editor control for the CustomEditorControlsRoom room type.
        /// </summary>
        [CustomRoomControl(typeof(CustomEditorControlsRoom))]
        private class Control : RoomControl
        {
            /// <summary>
            /// Mapping from room types to icon textures.
            /// </summary>
            private static readonly Dictionary<RoomType, Texture2D> RoomTypeIcons;

            static Control()
            {
                // Initialize icons in the static constructor
                RoomTypeIcons = new Dictionary<RoomType, Texture2D>()
                {
                    {RoomType.Treasure, CustomEditorControlsIcons.Base64ToTexture(CustomEditorControlsIcons.TreasureIcon, FilterMode.Point)},
                    {RoomType.Shop, CustomEditorControlsIcons.Base64ToTexture(CustomEditorControlsIcons.ShopIcon, FilterMode.Point)},
                    {RoomType.Boss, CustomEditorControlsIcons.Base64ToTexture(CustomEditorControlsIcons.BossIcon, FilterMode.Point)},
                    {RoomType.Spawn, CustomEditorControlsIcons.Base64ToTexture(CustomEditorControlsIcons.SpawnIcon, FilterMode.Point)},
                };
            }

            /// <summary>
            /// Override the Draw method to draw a custom control.
            /// </summary>
            public override void Draw(Vector2 gridOffset, float zoom)
            {
                // Draw the base control
                base.Draw(gridOffset, zoom);

                // Make sure that we work with the correct room type
                var room = Room as CustomEditorControlsRoom;
                if (room == null)
                {
                    return;
                }

                // If we have an icon for the current room type, we will draw it
                if (RoomTypeIcons.TryGetValue(room.Type, out var icon))
                {
                    // Use the default control rect as a base for position and size of icon
                    var rect = GetRect(gridOffset, zoom);
                    rect.y -= rect.height / 4;
                    rect.height /= 2;

                    // Make the icon larger if it is the boss room
                    if (room.Type == RoomType.Boss)
                    {
                        rect.height *= 1.5f;
                    }

                    // Compute correct rect width
                    var textureRatio = icon.width / (float)icon.height;
                    rect.width = rect.height * textureRatio;

                    // Draw the icon
                    GUI.DrawTexture(rect, icon);
                }
            }
        }

        #endregion

        #endregion
    }

    #endregion
}