using System;

namespace Unitylock.Editor
{
    [Serializable]
    public class RequestLockBodyEntity
    {
        public string file;

        public RequestLockBodyEntity(string file)
        {
            this.file = file;
        }
    }

    [Serializable]
    public class RequestUnlockBodyEntity
    {
        public string file;

        public RequestUnlockBodyEntity(string file)
        {
            this.file = file;
        }
    }
}
