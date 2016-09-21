using UnityEngine;
using UnityEditor;

namespace Unitylock.Editor
{
    public class UnitylockWindow : EditorWindow
    {
        Vector2 _scrollPosition = Vector2.zero;
        bool shouldRepaint = false;
        UnitylockState state = new UnitylockState();
        UnitylockClient _client;

        UnitylockClient client
        {
            get
            {
                if (_client == null)
                {
                    _client = new UnitylockClient(state, () => shouldRepaint = true);
                }
                return _client;
            }
        }

        [MenuItem("Window/Unitylock")]
        public static void ShowWindow()
        {
            GetWindow(typeof(UnitylockWindow));
        }

        void OnGUI()
        {
            // Status
            RenderStatus();

            // FileList
            RenderFileList();

            // Sync
            if (GUILayout.Button("Sync"))
            {
                client.Search();
            }

            if (GUILayout.Button("Touch"))
            {
                client.Touch();
            }
        }

        void RenderStatus()
        {
            GUILayout.Label("Status", EditorStyles.boldLabel);
            EditorGUILayout.TextField("user", state.User);
            EditorGUILayout.TextField("url", state.Url);
        }

        void RenderFileList()
        {
            GUILayout.Label("Files", EditorStyles.boldLabel);
            GUILayout.BeginScrollView(_scrollPosition, GUI.skin.box);

            foreach (var entity in state.Entities)
            {
                bool enable = string.IsNullOrEmpty(entity.user) || entity.user == state.User;
                EditorGUI.BeginDisabledGroup(!enable);
                bool isOwner = entity.user == state.User;
                string line = string.Format(" {0}\t{1}", entity.user, entity.file);
                bool shouldOwn = EditorGUILayout.ToggleLeft(line, isOwner);

                if (isOwner != shouldOwn)
                {
                    if (shouldOwn)
                    {
                        client.Lock(entity.file);
                    }
                    else
                    {
                        client.Unlock(entity.file);
                    }
                }
                EditorGUI.EndDisabledGroup();
            }

            GUILayout.EndScrollView();
        }

        void Update()
        {
            if (shouldRepaint)
            {
                shouldRepaint = false;
                Repaint();
            }
        }
    }
}
