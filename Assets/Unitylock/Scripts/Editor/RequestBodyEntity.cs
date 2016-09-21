using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        public String ToJson()
        {
            return JsonUtility.ToJson(this);
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

        public String ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }

    [Serializable]
    public class RequestTouchBodyEntity
    {
        public List<string> files;

        public RequestTouchBodyEntity(List<string> files)
        {
            this.files = files;
        }

        public String ToJson()
        {
            // XXX: JsonUtility cannot handle root array element
            var doubleQuatedValues = this.files.Select(it => '"' + it + '"').ToArray();
            return "[" + string.Join(",", doubleQuatedValues) + "]";
        }
    }
}
