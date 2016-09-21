using System;
using System.IO;
using System.Net;
using System.Text;

namespace Unitylock.Editor
{
    public class HttpRequestJson
    {
        public void Get(string url, Action<string> successCallback = null, Action<string> errorCallback = null)
        {
            var request = WebRequest.Create(url);
            request.Method = "GET";

            Process(request, response => HandleResponse(response, successCallback, errorCallback));
        }

        public void Put(string url, string body, Action<string> successCallback = null,
            Action<string> errorCallback = null)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "PUT";
            request.Headers.Add("ContentType", "application/json; charset=UTF-8");

            byte[] data = Encoding.UTF8.GetBytes(body);
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            Process(request, (response) => HandleResponse(response, successCallback, errorCallback));
        }

        void HandleResponse(HttpWebResponse response, Action<string> successCallback, Action<string> errorCallback)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var text = new StreamReader(response.GetResponseStream()).ReadToEnd();
                if (successCallback != null)
                {
                    successCallback(text);
                }
            }
            else
            {
                var text = new StreamReader(response.GetResponseStream()).ReadToEnd();
                if (errorCallback != null)
                {
                    errorCallback(text);
                }
            }
        }

        void Process(WebRequest request, Action<HttpWebResponse> responseCallback)
        {
            Action wrapper = () =>
            {
                request.BeginGetResponse(new AsyncCallback(iar =>
                {
                    var response = (iar.AsyncState as HttpWebRequest).EndGetResponse(iar) as HttpWebResponse;
                    responseCallback(response);
                }), request);
            };

            wrapper.BeginInvoke(new AsyncCallback(iar =>
            {
                var action = (Action) iar.AsyncState;
                action.EndInvoke(iar);
            }), wrapper);
        }
    }
}
