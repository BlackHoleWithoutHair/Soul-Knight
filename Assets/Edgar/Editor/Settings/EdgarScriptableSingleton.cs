using System.IO;
using UnityEditorInternal;
using UnityEngine;

namespace Edgar.Unity.Editor
{
    public class EdgarScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        private static T s_Instance;

        public static T instance
        {
            get
            {
                if (s_Instance == null)
                    EdgarScriptableSingleton<T>.CreateAndLoad();
                return EdgarScriptableSingleton<T>.s_Instance;
            }
        }

        protected EdgarScriptableSingleton()
        {
            if (s_Instance != null)
                Debug.LogError("ScriptableSingleton already exists. Did you query the singleton in a constructor?");
            else
                EdgarScriptableSingleton<T>.s_Instance = (object)this as T;
        }

        private static void CreateAndLoad()
        {
            string filePath = EdgarScriptableSingleton<T>.GetFilePath();
            if (!string.IsNullOrEmpty(filePath))
                InternalEditorUtility.LoadSerializedFileAndForget(filePath);
            if (!(s_Instance == null))
                return;
            ScriptableObject.CreateInstance<T>().hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSave;
        }

        protected virtual void Save(bool saveAsText)
        {
            if (s_Instance == null)
            {
                Debug.Log("Cannot save ScriptableSingleton: no instance!");
            }
            else
            {
                string filePath = EdgarScriptableSingleton<T>.GetFilePath();
                if (string.IsNullOrEmpty(filePath))
                    return;
                string directoryName = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);
                InternalEditorUtility.SaveToSerializedFileAndForget(new T[1]
                {
                    EdgarScriptableSingleton<T>.s_Instance
                }, filePath, saveAsText);
            }
        }

        private static string GetFilePath()
        {
            return EdgarSettings.FilePath;
        }
    }
}