using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ToolLibrary
{
    public static class HttpCode
    {

        /// <summary>
        /// 调用HTTP_POST接口
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="Para"></param>
        /// <returns></returns>
        public static string HttpPost(string URL, string Para)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(URL);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            byte[] bytes = Encoding.UTF8.GetBytes(Para);
            using (Stream reqStream = httpRequest.GetRequestStream())
            {
                reqStream.Write(bytes, 0, bytes.Length);
                reqStream.Flush();
            }
            try
            {
                using (HttpWebResponse myResponse = (HttpWebResponse)httpRequest.GetResponse())
                {
                    StreamReader sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                    string responseString = sr.ReadToEnd();
                    return responseString;
                }
            }
            catch (WebException ex)
            {
                var res = (HttpWebResponse)ex.Response;
                StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                string str = sr.ReadToEnd();
                return str;
            }
        }

        /// <summary>
        /// 调用HTTP_GET接口
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string HttpGet(string url, Dictionary<string, string> dic)
        {
            string result = "";
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            if (dic.Count > 0)
            {
                builder.Append("?");
                int i = 0;
                foreach (var item in dic)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
            }
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(builder.ToString());
            //添加参数
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取内容
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            finally
            {
                stream.Close();
            }
            return result;
        }


        /// <summary>
        /// 异步调用HTTP_POST接口
        /// </summary>
        /// <param name="URL">请求地址</param>
        /// <param name="Para">JSON格式的参数</param>
        /// <returns>接口响应内容</returns>
        public static async Task<string> HttpPostAsync(string URL, string Para)
        {
            // 创建HTTP请求对象
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(URL);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";

            // 异步写入请求参数
            byte[] bytes = Encoding.UTF8.GetBytes(Para);
            using (Stream reqStream = await httpRequest.GetRequestStreamAsync())
            {
                await reqStream.WriteAsync(bytes, 0, bytes.Length);
                await reqStream.FlushAsync();
            }

            try
            {
                // 异步获取响应并读取内容
                using (HttpWebResponse myResponse = (HttpWebResponse)await httpRequest.GetResponseAsync())
                using (StreamReader sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8))
                {
                    return await sr.ReadToEndAsync();
                }
            }
            catch (WebException ex)
            {
                // 处理异常响应
                if (ex.Response != null)
                {
                    using (HttpWebResponse res = (HttpWebResponse)ex.Response)
                    using (StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8))
                    {
                        return await sr.ReadToEndAsync();
                    }
                }
                // 如果没有响应，返回异常信息
                return ex.Message;
            }
            catch (Exception ex)
            {
                // 处理其他异常
                return ex.Message;
            }
        }

        /// <summary>
        /// 异步调用HTTP_GET接口
        /// </summary>
        /// <param name="url">请求基础地址</param>
        /// <param name="dic">查询参数键值对</param>
        /// <returns>接口响应内容</returns>
        public static async Task<string> HttpGetAsync(string url, Dictionary<string, string> dic)
        {
            // 构建包含查询参数的完整URL
            StringBuilder builder = new StringBuilder(url);
            if (dic.Count > 0)
            {
                builder.Append("?");
                int i = 0;
                foreach (var item in dic)
                {
                    if (i > 0)
                        builder.Append("&");
                    // 对参数值进行URL编码，处理特殊字符
                    builder.AppendFormat("{0}={1}",
                        Uri.EscapeDataString(item.Key),
                        Uri.EscapeDataString(item.Value));
                    i++;
                }
            }

            try
            {
                // 创建HTTP请求对象
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(builder.ToString());

                // 异步获取响应
                using (HttpWebResponse resp = (HttpWebResponse)await req.GetResponseAsync())
                // 获取响应流
                using (Stream stream = resp.GetResponseStream())
                // 读取响应内容
                using (StreamReader reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            catch (WebException ex)
            {
                // 处理Web异常（如404、500等状态码）
                if (ex.Response != null)
                {
                    using (HttpWebResponse errorResp = (HttpWebResponse)ex.Response)
                    using (StreamReader errorReader = new StreamReader(errorResp.GetResponseStream()))
                    {
                        return await errorReader.ReadToEndAsync();
                    }
                }
                return ex.Message;
            }
            catch (Exception ex)
            {
                // 处理其他异常
                return ex.Message;
            }
        }

    }
}
