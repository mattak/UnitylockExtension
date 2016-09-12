using UnityEngine;
using UnityEditor;

namespace Unitylock.Editor
{
    public class UnitylockWindow : EditorWindow
    {
        bool file1state = false;
        string user = "guest";
        string url = "http://localhost:3000";
        Vector2 scrollPosition = Vector2.zero;

        [MenuItem("Window/Unitylock")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(UnitylockWindow));
        }

        void OnGUI()
        {
            // Status
            GUILayout.Label("Status", EditorStyles.boldLabel);
            EditorGUILayout.TextField("user", user);
            EditorGUILayout.TextField("url", url);

            // FileList
            GUILayout.Label("Files", EditorStyles.boldLabel);
            GUILayout.BeginScrollView(scrollPosition, GUI.skin.box);
            foreach (var entity in Parse())
            {
                bool enable = (entity.user == null || entity.user == user);
                EditorGUI.BeginDisabledGroup(!enable);
                bool isChecked = entity.user == user;
                bool newState = EditorGUILayout.ToggleLeft(string.Format(" {0}\t{1}", isChecked, entity.file), isChecked);
                EditorGUI.EndDisabledGroup();
            }
            GUILayout.EndScrollView();
            GUILayout.Button("SyncFiles");
        }

        UnitylockEntity[] Parse()
        {
            string text = "[{'user':'g','file':'file1'},{'user':'guest','file':'file2'}]"; // System.IO.File.ReadAllText("");
            return JsonUtility.FromJson<UnitylockEntity[]>(text);
        }
    }
}
