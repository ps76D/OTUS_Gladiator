using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ResetGameHelper : EditorWindow
    {
        [MenuItem("MyTools/Reset Game State and Clear Prefs")]
        public static void ClearAll()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}