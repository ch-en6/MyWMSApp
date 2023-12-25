using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Windows;
using wmsApp.utils;
using WindowsFormsApp1.dto;

namespace wms.utils
{

    public class HttpHelper
    {
        private static readonly object LockObj = new object();
        private static HttpClient client = null;

        private static RSAUtil rsaUtil;
        private static AESUtil aesUtil;
        /*
         *  设置为服务器IP地址
         */
        private static readonly string BASE_ADDRESS = "http://localhost:8081/";
        public HttpHelper()
        {
            GetInstance();
            rsaUtil = new RSAUtil();
            aesUtil = new AESUtil();
            SetAuthorizationHeader(TokenManager.token);
        }

        // 添加Authorization请求头
        public void SetAuthorizationHeader(string token)
        {  
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token);
        }
        // 添加PublicKey请求头
        public void SetPublicKeyHeader(string headerValue)
        {
            client.DefaultRequestHeaders.Add("PublicKey",headerValue);
            Console.WriteLine(headerValue);
        }
        /**
         * 发送明文GET请求,获取密文数据
         */
        public Result GetDncryptedData(string url)
        {
            try
            {
                RSA rsa = new RSA();
                AES aes = new AES();
                SetPublicKeyHeader(TokenManager.csKey["publickey"]);
                var responseString = client.GetStringAsync(url);
                Result result = JsonHelper.JSONToObject<Result>(responseString.Result); //包含data，aeskey
                // rsa私钥解密获得aeskey
                string aesKeyByRsaDecode = rsa.DecryptByPrivateKey(result.aesKey, TokenManager.csKey["privatekey"]);

                string Resultdata = result.data.ToString();

                return JsonHelper.JSONToObject<Result>(aes.AesDecrypt(result.data.ToString(), aesKeyByRsaDecode));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /**
         * 发送明文POST请求,获取密文数据
         */
        public Result PostDncryptedData(string url, string strJson)
        {
            try
            {
                RSA rsa = new RSA();
                AES aes = new AES();
                HttpContent content = new StringContent(strJson);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                SetPublicKeyHeader(TokenManager.csKey["publickey"]);

                //由HttpClient发出Post请求
                Task<HttpResponseMessage> res = client.PostAsync(url, content);
                if (res.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    string resMsgStr = res.Result.Content.ReadAsStringAsync().Result;

                    Result result = JsonHelper.JSONToObject<Result>(resMsgStr); //包含data，aeskey
                    // rsa私钥解密获得aeskey
                    string aesKeyByRsaDecode = rsa.DecryptByPrivateKey(result.aesKey, TokenManager.csKey["privatekey"]);

                    string Resultdata = result.data.ToString();

                    return JsonHelper.JSONToObject<Result>(aes.AesDecrypt(result.data.ToString(), aesKeyByRsaDecode));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /**
         * 发送密文GET请求,获取密文数据
         */
        /*   public Result GetEncryptedData(string url)
           {
               try
               {
                   RSA rsa = new RSA();
                   AES aes = new AES();
                   //加密数据
                   string aesKey = aes.GetKey();
                   MessageBox.Show("1");

                   Result data = new Result();
                   data.data = aes.AesEncrypt(url, aesKey); //AES加密后的数据
                   data.aesKey = rsa.EncryptByPublicKey(aesKey, TokenManager.javaPublicKey); //后端RSA公钥加密后的AES的key
                   data.publicKey = TokenManager.csKey["publickey"];//前端公钥
                   string dataStr = JsonHelper.DateObjectTOJson(data);//将加密后的Result对象转换为json类型

                   var responseString = client.GetStringAsync(dataStr);
                   Result result = JsonHelper.JSONToObject<Result>(responseString.Result); //包含data，aeskey
                   // rsa私钥解密获得aeskey
                   string aesKeyByRsaDecode = rsa.DecryptByPrivateKey(result.aesKey, TokenManager.csKey["privatekey"]);

                   string Resultdata = result.data.ToString();

                   return JsonHelper.JSONToObject<Result>(aes.AesDecrypt(result.data.ToString(), aesKeyByRsaDecode));
               }
               catch (Exception ex)
               {
                   return null;
               }
           }*/
        /**
         * 发送密文POST请求,获取密文数据，参数含@RequestBody不能用
         */
        public Result PostEncryptedData(string url, string strJson)
        {
            try
            {
                //加密再传输
                AES aes = new AES();
                RSA rsa = new RSA();
                string aesKey = aes.GetKey();
                Result data = new Result();

                data.data = aes.AesEncrypt(strJson, aesKey); //AES加密后的数据
  
                data.aesKey = rsa.EncryptByPublicKey(aesKey, TokenManager.javaPublicKey); //后端RSA公钥加密后的AES的key
 
                data.publicKey = TokenManager.csKey["publickey"];//前端公钥
              
                string dataStr = JsonHelper.DateObjectTOJson(data);//将加密后的Result对象转换为json类型
               
                HttpContent content = new StringContent(dataStr);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                //由HttpClient发出Post请求
                Task<HttpResponseMessage> res = client.PostAsync(url, content);
                if (res.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
         
                    string resMsgStr = res.Result.Content.ReadAsStringAsync().Result;

                    Result result = JsonHelper.JSONToObject<Result>(resMsgStr); //包含data，aeskey
                    // rsa私钥解密获得aeskey
                    string aesKeyByRsaDecode = rsa.DecryptByPrivateKey(result.aesKey, TokenManager.csKey["privatekey"]);
            
                    string Resultdata = result.data.ToString();
                  
                    return JsonHelper.JSONToObject<Result>(aes.AesDecrypt(result.data.ToString(), aesKeyByRsaDecode));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //发送密文，返回明文
        public Result PostAndEncryptData(string url, string strJson)
        {
            try
            {
                //加密再传输
                AES aes = new AES();
                RSA rsa = new RSA();
                string aesKey = aes.GetKey();
                Result data = new Result();

                data.data = aes.AesEncrypt(strJson, aesKey); //AES加密后的数据

                data.aesKey = rsa.EncryptByPublicKey(aesKey, TokenManager.javaPublicKey); //后端RSA公钥加密后的AES的key

                data.publicKey = TokenManager.csKey["publickey"];//前端公钥

                string dataStr = JsonHelper.DateObjectTOJson(data);//将加密后的Result对象转换为json类型

                HttpContent content = new StringContent(dataStr);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                //由HttpClient发出Post请求
                Task<HttpResponseMessage> res = client.PostAsync(url, content);
                if (res.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string resMsgStr = res.Result.Content.ReadAsStringAsync().Result;
                    return JsonHelper.JSONToObject<Result>(resMsgStr);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 制造单例
        /// </summary>
        /// <returns></returns>
        public static HttpClient GetInstance()
        {
            if (client == null)
            {
                lock (LockObj)
                {
                    if (client == null)
                    {
                        client = new HttpClient()
                        {
                            BaseAddress = new Uri(BASE_ADDRESS)
                        };
                    }
                }
            }
            return client;
        }
        /// <summary>
        /// 异步Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="strJson">传入的数据</param>
        /// <returns></returns>
        public async Task<string> PostAsync(string url, string strJson)
        {
            try
            {
                HttpContent content = new StringContent(strJson);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpResponseMessage res = await client.PostAsync(url, content);
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string resMsgStr = await res.Content.ReadAsStringAsync();
                    return resMsgStr;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 同步Post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public string Post(string url, string strJson)
        {
            try
            {
                HttpContent content = new StringContent(strJson);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                //client.DefaultRequestHeaders.Connection.Add("keep-alive");
                //由HttpClient发出Post请求
                Task<HttpResponseMessage> res = client.PostAsync(url, content);
                if (res.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string resMsgStr = res.Result.Content.ReadAsStringAsync().Result;
                    return resMsgStr;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 异步Post请求
        /// </summary>
        /// <typeparam name="TResult">返回参数的数据类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="data">传入的数据</param>
        /// <returns></returns>
        public async Task<TResult> PostAsync<TResult>(string url, object data)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(jsonData);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpResponseMessage res = await client.PostAsync(url, content);
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string resMsgStr = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ResultDto<TResult>>(resMsgStr);
                    return result != null ? result.Data : default;
                }
                else
                {
                    MessageBox.Show(res.StatusCode.ToString());
                    return default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return default;
            }
        }
        /// <summary>
        /// 同步Get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public string Get(string url)
        {
            try
            {
                var responseString = client.GetStringAsync(url);
                return responseString.Result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 异步Get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public async Task<string> GetAsync(string url)
        {
            try
            {
                var responseString = await client.GetStringAsync(url);
                return responseString;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 异步Get请求
        /// </summary>
        /// <typeparam name="TResult">返回参数的数据</typeparam>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public async Task<TResult> GetAsync<TResult>(string url)
        {
            try
            {
                var resMsgStr = await client.GetStringAsync(url);
                var result = JsonConvert.DeserializeObject<ResultDto<TResult>>(resMsgStr);
                return result != null ? result.Data : default;
            }
            catch (Exception ex)
            {
                return default(TResult);
            }
        }

    }

    public class ResultDto<TResult>
    {
        public string Msg { get; set; }
        public TResult Data { get; set; }
        public bool Success { get; set; }
    }
}

