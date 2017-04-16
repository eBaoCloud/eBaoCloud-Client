using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyHttp.Http;
using Newtonsoft.Json.Linq;

/// <summary>
/// / 网络异常处理，类型处理。
/// </summary>
namespace com.ebao.gs.ebaocloud.sea.seg.client.pub
{
    class NetworkUtils
    {
        private static string host = ApiServiceFactory.getCurrentHost();

        public static JObject Get(string path, string token = null)
        {
            HttpClient httpClient = new HttpClient(host);
            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;
            httpClient.Request.AddExtraHeader("Authorization", "Basic " + token);
            HttpResponse httpResponse = httpClient.Get(path);
            JObject responseObj = JObject.Parse(httpResponse.RawText);
            requestSuccess(httpResponse);
            return responseObj;
        }

        public static JObject Post(string path, Object parameters = null, string token = null)
        {
            HttpClient httpClient = new HttpClient(host);
            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;
            httpClient.Request.AddExtraHeader("Authorization", "Basic " + token);
            HttpResponse httpResponse = httpClient.Post(path, parameters, HttpContentTypes.ApplicationJson);
            requestSuccess(httpResponse);
            JObject responseObj = JObject.Parse(httpResponse.RawText);
            return responseObj;
        }

        static private void requestSuccess(HttpResponse resp)
        {
            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(resp.StatusDescription);
            }
        }
    }
}
