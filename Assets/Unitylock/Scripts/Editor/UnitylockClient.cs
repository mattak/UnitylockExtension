using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Unitylock.Editor
{
    public class UnitylockClient
    {
        private UnitylockState state;
        private Action responseHook = () => { };

        public string SearchUrl
        {
            get { return state.Url + "/files"; }
        }

        public string LockUrl
        {
            get { return string.Format("{0}/user/{1}/lock", state.Url, state.User); }
        }

        public string UnlockUrl
        {
            get { return string.Format("{0}/user/{1}/unlock", state.Url, state.User); }
        }

        public UnitylockClient(UnitylockState state)
        {
            this.state = state;
        }

        public UnitylockClient(UnitylockState state, Action responseHook)
        {
            this.state = state;
            this.responseHook = responseHook;
        }

        public List<UnitylockEntity> LoadFromFile(string file)
        {
            if (File.Exists(file))
            {
                string text = File.ReadAllText(file);
                return Parse(text);
            }

            Debug.Log("flie not found: " + file);
            return new List<UnitylockEntity>();
        }

        public void Search()
        {
            new HttpRequestJson().Get(this.SearchUrl, responseText =>
            {
                UpdateList(responseText);
                responseHook();
            });
        }

        public void Lock(string file)
        {
            var body = JsonUtility.ToJson(new RequestLockBodyEntity(file));
            new HttpRequestJson().Put(this.LockUrl, body, responseText =>
            {
                UpdateList(responseText);
                responseHook();
            });
        }

        public void Unlock(string file)
        {
            var body = JsonUtility.ToJson(new RequestUnlockBodyEntity(file));
            new HttpRequestJson().Put(this.UnlockUrl, body, responseText =>
            {
                UpdateList(responseText);
                responseHook();
            });
        }

        private void UpdateList(string responseText)
        {
            var list = Parse(responseText);
            state.UpdateList(list, true);
        }

        private List<UnitylockEntity> Parse(string json)
        {
            var wrapped = "{\"data\":" + json + "}";
            var wrapper = JsonUtility.FromJson<UnitylockEntityListWrapper>(wrapped);
            return wrapper.data;
        }
    }
}
