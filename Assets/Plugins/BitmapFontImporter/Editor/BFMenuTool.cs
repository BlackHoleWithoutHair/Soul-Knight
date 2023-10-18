﻿using UnityEngine;
using UnityEditor;
using System.IO;

namespace litefeel
{

    public class BFMenuTool
    {
        [MenuItem("Assets/Bitmap Font/Rebuild Bitmap Font", true)]
        public static bool CheckRebuildFont()
        {
            if (EditorApplication.isPlaying) return false;

            TextAsset selected = Selection.activeObject as TextAsset;
            if (selected == null) return false;
            return BFImporter.IsFnt(AssetDatabase.GetAssetPath(selected));
        }

        [MenuItem("Assets/Bitmap Font/Rebuild Bitmap Font")]
        public static void RebuildFont()
        {
            TextAsset selected = Selection.activeObject as TextAsset;
            BFImporter.DoImportBitmapFont(AssetDatabase.GetAssetPath(selected));
        }

        [MenuItem("Assets/Bitmap Font/Rebuild All Bitmap Font", true)]
        public static bool CheckRebuildAllFont()
        {
            return !EditorApplication.isPlaying;
        }

        [MenuItem("Assets/Bitmap Font/Rebuild All Bitmap Font")]
        public static void RebuildAllFont()
        {
            string dataPath = Application.dataPath;
            int startPos = dataPath.Length - "Assets".Length;
            string[] files = Directory.GetFiles(Application.dataPath, "*.fnt", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                BFImporter.DoImportBitmapFont(files[i].Substring(startPos));
            }
        }
    }

}