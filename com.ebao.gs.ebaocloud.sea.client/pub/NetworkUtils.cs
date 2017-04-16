using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyHttp.Http;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using com.ebao.gs.ebaocloud.sea.seg.client.vmi.parameters;

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
            HttpResponse httpResponse;
            try
            {
                httpResponse = httpClient.Get(path);
            }
            catch (Exception e)
            {
                throw e;
            }
            requestSuccess(httpResponse);
            JObject responseObj = JObject.Parse(httpResponse.RawText);
            return responseObj;
        }

        public static JObject Post(string path, Object parameters = null, string token = null)
        {
            HttpClient httpClient = new HttpClient(host);
            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;
            httpClient.Request.AddExtraHeader("Authorization", "Basic " + token);
            HttpResponse httpResponse;
            try
            {
                httpResponse = httpClient.Post(path, JsonConvert.SerializeObject(parameters, Formatting.Indented), HttpContentTypes.ApplicationJson);
            }
            catch (Exception e)
            {
                throw e;
            }
            requestSuccess(httpResponse);
            JObject responseObj = JObject.Parse(httpResponse.RawText);
            return responseObj;
        }
        
        public static JObject UploadFile(string path, FileInfo file, UploadFileParams parameters = null, string token = null)
        {
       
            HttpClient httpClient = new HttpClient(host);
            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;
            httpClient.Request.AddExtraHeader("Authorization", "Basic " + token);

            UploadFileParams parameter = new UploadFileParams();
            parameter.uploadExtraData = new UploadFileExtraData();
            parameter.uploadExtraData.policyId = "1111";
            parameter.uploadExtraData.docName = "doc";
            parameter.uploadExtraData.docType = "1";
           parameter.fileStream = new FileStream(file.FullName, FileMode.OpenOrCreate);
            HttpResponse httpResponse = httpClient.Post(path, parameters, token);
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
