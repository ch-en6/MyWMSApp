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
                MessageBox.Show("服务器异常");
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
                MessageBox.Show("服务器异常");
                return null;
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
                MessageBox.Show("服务器异常");
                return null;
            }
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
                MessageBox.Show("服务器异常");
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
                MessageBox.Show("服务器异常");
                return null;
            }
        }
        /**
         * 发送密文POST请求,获取密文数据，参数含@RequestBody不能用
         */
        public Result PostEncryptedData(string url, string strJson)
        {
            try
            {
                AES aes = new AES();
                RSA rsa = new RSA();
                string aesKey = aes.GetKey();

                //加密再传输
                Result data = new Result();
                //用AES加密传输的数据
                data.data = aes.AesEncrypt(strJson, aesKey); 
                //用RSA加密AES的Key
                data.aesKey = rsa.EncryptByPublicKey(aesKey, TokenManager.javaPublicKey);
                //前端RSA公钥
                data.publicKey = TokenManager.csKey["publickey"];
                //将加密后的Result对象转换为json类型
                string dataStr = JsonHelper.DateObjectTOJson(data);
                //设置请求体
                HttpContent content = new StringContent(dataStr);
                //设置请求的 Content-Type 为 "application/json"
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                //由HttpClient发出Post请求
                Task<HttpResponseMessage> res = client.PostAsync(url, content);
                if (res.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    string resMsgStr = res.Result.Content.ReadAsStringAsync().Result;
                    // 后端返回的数据包含data，aeskey
                    Result result = JsonHelper.JSONToObject<Result>(resMsgStr); 
                    // 用RSA私钥解密获得aeskey
                    string aesKeyByRsaDecode = rsa.DecryptByPrivateKey(result.aesKey, TokenManager.csKey["privatekey"]);

                    string Resultdata = result.data.ToString();
                    // 返回aesKey解密出的数据
                    return JsonHelper.JSONToObject<Result>(aes.AesDecrypt(result.data.ToString(), aesKeyByRsaDecode));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务器异常");
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
                MessageBox.Show("服务器异常");
                return null;
            }
        }

    }

}

