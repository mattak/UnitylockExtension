using System.Collections;
using UnityEditor;

namespace Unitylock.Editor
{
    public class EditorCoroutine
    {
        public static EditorCoroutine Start(IEnumerator enumerator)
        {
            var coroutine = new EditorCoroutine(enumerator);
            coroutine.Start();
            return coroutine;
        }

        readonly IEnumerator enumerator;

        EditorCoroutine(IEnumerator enumerator)
        {
            this.enumerator = enumerator;
        }

        void Start()
        {
            EditorApplication.update += Update;
        }

        public void Stop()
        {
            EditorApplication.update -= Update;
        }

        void Update()
        {
            if (!enumerator.MoveNext())
            {
                Stop();
            }
        }
    }
}
