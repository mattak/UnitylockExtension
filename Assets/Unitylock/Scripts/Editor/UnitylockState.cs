using System;
using System.Collections.Generic;

namespace Unitylock.Editor
{
    [Serializable]
    public class UnitylockState
    {
        public string User = "guest";
        public string Url = "http://localhost:3000";
        public Dictionary<string, UnitylockEntity> File2EntityMap = new Dictionary<string, UnitylockEntity>();

        public IEnumerable<UnitylockEntity> Entities
        {
            get { return this.File2EntityMap.Values; }
        }

        public UnitylockState()
        {
        }

        public void UpdateList(List<UnitylockEntity> list, bool force)
        {
            if (force)
            {
                this.File2EntityMap.Clear();
            }

            foreach (var entity in list)
            {
                this.File2EntityMap[entity.file] = entity;
            }
        }
    }
}
