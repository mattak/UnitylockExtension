using System;

namespace Unitylock.Editor
{
    [Serializable]
    public class UnitylockEntity
    {
        public string user;
        public string file;

        public bool TakeOwnership(string loginUser, bool enable)
        {
            if (!string.IsNullOrEmpty(this.user) && this.user != loginUser)
            {
                return false;
            }

            this.user = enable ? loginUser : null;
            return true;
        }
    }
}
