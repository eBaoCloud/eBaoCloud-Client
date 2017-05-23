using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyHttp.Http;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using com.ebaocloud.client.thai.seg.vmi.parameters;
using System.Net.Mime;

namespace com.ebaocloud.client.thai.seg.vmi.pub
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

        public static JObject UploadFile(string path, UploadFileParams parameters = null, string token = null)
        {
            WebClient webClient = new WebClient();
            System.Net.WebRequest webRequest = System.Net.WebRequest.Create(host + path);
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
            webRequest.Method = "POST";
            webRequest.Headers.Add("Authorization", "Basic " + token);
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            Stream postDataStream = GetPostStream(parameters.fileInfo, parameters.uploadExtraData, boundary);
            webRequest.ContentLength = postDataStream.Length;
            Stream reqStream = webRequest.GetRequestStream();
            postDataStream.Position = 0;
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            while ((bytesRead = postDataStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                reqStream.Write(buffer, 0, bytesRead);
            }
            postDataStream.Close();
            reqStream.Close();
            string result;
            try
            {
                StreamReader streamReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                result = streamReader.ReadToEnd();
            }
            catch (Exception e)
            {
                throw e;
            }
            return JsonConvert.DeserializeObject<JObject>(result);
        }

        public static void download(string urlPath, string filePath, string token)
        {
            FileStream stream = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host + urlPath);
                request.Method = "GET";
                request.Headers.Add("Authorization", "Basic " + token);
                WebResponse response = request.GetResponse();
                string resp = response.Headers["Content-Disposition"];
                Array temp = resp.Split(';');
                String newFileName = null;
                foreach (String str in temp)
                {
                    if (str.IndexOf("filename") != -1)
                    {
                        newFileName =  str.Split('=')[1];
                    }
                }
                //ContentDisposition contentDisposition = new ContentDisposition(response.Headers["Content-Disposition"]);
               // string newFileName = contentDisposition.FileName;
                stream = File.Create(filePath + Path.DirectorySeparatorChar + newFileName);
                Stream responseStream = response.GetResponseStream();
                response.GetResponseStream().CopyTo(stream);
            }
            catch (FormatException fe)
            {
                throw fe;
            }
            catch (Exception e)
            {
                throw new Exception("download file error, " + e.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

        }

        private static Stream GetPostStream(FileInfo fileInfo, JObject uploadExtraData, string boundary)
        {
            Stream postDataStream = new System.IO.MemoryStream();
            //adding form data
            string formDataHeaderTemplate = Environment.NewLine + "--" + boundary + Environment.NewLine +
            "Content-Disposition: form-data; name=\"{0}\";" + Environment.NewLine + Environment.NewLine + "{1}";
            byte[] formItemBytes = System.Text.Encoding.UTF8.GetBytes(string.Format(formDataHeaderTemplate, "uploadExtraData", JsonConvert.SerializeObject(uploadExtraData, Formatting.Indented)));
            postDataStream.Write(formItemBytes, 0, formItemBytes.Length);

            //add fileData
            string fileHeaderTemplate = Environment.NewLine + "--" + boundary + Environment.NewLine +
            "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"" +
            Environment.NewLine + "Content-Type: \"{2}\"" + Environment.NewLine + Environment.NewLine;

            byte[] fileHeaderBytes = System.Text.Encoding.UTF8.GetBytes(string.Format(fileHeaderTemplate, "fileData", fileInfo.FullName, MimeMapping.GetMimeMapping(fileInfo.FullName)));
            postDataStream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);
            FileStream fileStream = fileInfo.OpenRead();
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                postDataStream.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();
            byte[] endBoundaryBytes = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            postDataStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);

            return postDataStream;
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
