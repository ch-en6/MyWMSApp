using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace wmsApp.utils
{
    public class RSAUtil
    {
        /// <summary>
        /// 使用公钥加密
        /// </summary>
        /// <returns></returns>
        public string Encrypt(string strText, string strPublicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportCspBlob(Convert.FromBase64String(strPublicKey));
            byte[] byteText = Encoding.UTF8.GetBytes(strText);
            byte[] byteEntry = rsa.Encrypt(byteText, false);
            return Convert.ToBase64String(byteEntry);
        }

        /// <summary>
        /// 私钥解密
        /// </summary>
        /// <param name="strEntryText"></param>
        /// <param name="strPrivateKey"></param>
        /// <returns></returns>
        public string Decrypt(string strEntryText, string strPrivateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportCspBlob(Convert.FromBase64String(strPrivateKey));
            byte[] byteEntry = Convert.FromBase64String(strEntryText);
            byte[] byteText = rsa.Decrypt(byteEntry, false);
            return Encoding.UTF8.GetString(byteText);
        }

        /// <summary>
        /// 获取公钥和私钥
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetKey()
        {
            Dictionary<string, string> dictKey = new Dictionary<string, string>();
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string public_Key = Convert.ToBase64String(rsa.ExportCspBlob(false));
            string private_Key = Convert.ToBase64String(rsa.ExportCspBlob(true));
            dictKey.Add("publickey", public_Key);
            dictKey.Add("privatekey", private_Key);
            return dictKey;
        }
    }

    public class AESUtil
    {
        //为什么要用base64,因为得到的密文是byte[]，所以默认用base64转成str方便查看
     

        // AES 加密的初始化向量,加密解密需设置相同的值。默认值我们设置成 16 个 0
        public static byte[] AES_IV = Encoding.UTF8.GetBytes("0000000000000000");

        // AES的key支持128位，最大支持256位。256位需要32个字节。
        // 所以这里使用密钥的前 32 字节作为 key ,不足32补 0。
        public static byte[] GetKey(string pwd)
        {
            while (pwd.Length < 32)
            {
                pwd += '0';
            }
            pwd = pwd.Substring(0, 32);
            return Encoding.UTF8.GetBytes(pwd);
        }




        /// <summary>
        ///  加密
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="password">要被加密的原始密码</param>
        /// <returns>密文base64</returns>
        public static string Encrypt(string key, string password)
        {
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = GetKey(key);
                aesAlg.IV = AES_IV;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(password);
                        }
                        byte[] bytes = msEncrypt.ToArray();
                        return Convert.ToBase64String(bytes);
                    }
                }
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="EncryptStr">密文</param>
        /// <returns>明文-原密码</returns>
        public static string Decrypt(string key, string EncryptStr)
        {
            byte[] inputBytes = Convert.FromBase64String(EncryptStr);
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = GetKey(key);
                aesAlg.IV = AES_IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream(inputBytes))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srEncrypt = new StreamReader(csEncrypt))
                        {
                            return srEncrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

}
