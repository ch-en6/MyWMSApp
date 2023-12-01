using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace wmsApp.utils
{
    class AES
    {
        public static byte[] aseKey = AESUtil.GetKeyBySeed("17837");
        // AES的key支持128位，最大支持256位。256位需要32个字节。
        // 所以这里使用密钥的前 32 字节作为 key ,不足32补 0。
        public string GetKey()
        {
            string keyString = Convert.ToBase64String(aseKey);
            return keyString;
        }
        public static byte[] GetKeyBySeed(string Seed, int KeyLen = 16)
        {
            byte[] bySeed = Encoding.UTF8.GetBytes(Seed);
            byte[] byKeyArray = null;
            using (var st = new SHA1CryptoServiceProvider())
            {
                using (var nd = new SHA1CryptoServiceProvider())
                {
                    var rd = nd.ComputeHash(st.ComputeHash(bySeed));
                    byKeyArray = rd.Take(KeyLen).ToArray();
                }
            }
            return byKeyArray;
        }
        /// <summary>
        ///  AES 加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public  string AesEncrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public  string AesDecrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }
    }
}
