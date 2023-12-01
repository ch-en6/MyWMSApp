using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wmsApp.utils
{
    public class AESUtil
    {
        //为什么要用base64,因为得到的密文是byte[]，所以默认用base64转成str方便查看


        // AES 加密的初始化向量,加密解密需设置相同的值。默认值我们设置成 16 个 0
        public static byte[] AES_IV = Encoding.UTF8.GetBytes("0000000000000000");
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


        public static byte[] GenerateKey()
        {
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.GenerateKey();
                return aesAlg.Key;
            }
        }
        /// <summary>
        /// AES加密（128位，密码模式ECB，填充类型PKCS5Padding或者PKCS7Padding。注：ECB模式不需要初始化向量iv。）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Encrypt1(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = Convert.FromBase64String(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            StringBuilder ret = new StringBuilder();
            foreach (byte b in resultArray.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// AES解密（128位，密码模式ECB，填充类型PKCS5Padding或者PKCS7Padding。注：ECB模式不需要初始化向量iv。）
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Decrypt(string toDecrypt, string key)
        {
            try
            {
                MessageBox.Show("aes1");
                byte[] keyArray = Convert.FromBase64String(key);//将TestGenAESByteKey类输出的字符串转为byte数组
                MessageBox.Show(keyArray.Length.ToString());
                MessageBox.Show("aes2");
                byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
                MessageBox.Show("aes3");
                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;        //必须设置为ECB
                rDel.Padding = PaddingMode.PKCS7;  //必须设置为PKCS7
                ICryptoTransform cTransform = rDel.CreateDecryptor();
                MessageBox.Show("aes4");
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                MessageBox.Show(UTF8Encoding.UTF8.GetString(resultArray));
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }

        }

        /// <summary>
        ///  AES 加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Encrypt(string str, string key)
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
     /*   public string Decrypt(string str, string key)
        {
            try
            {
                MessageBox.Show("str:"+str);
                if (string.IsNullOrEmpty(str)) return null;
                Byte[] toEncryptArray = Convert.FromBase64String(str);
                MessageBox.Show("1");
                System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
                {
                    Key = Encoding.UTF8.GetBytes(key),
                    Mode = System.Security.Cryptography.CipherMode.ECB,
                    Padding = System.Security.Cryptography.PaddingMode.PKCS7
                };
                MessageBox.Show("2");
                System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                MessageBox.Show("aes解码："+Encoding.UTF8.GetString(resultArray));
                return Encoding.UTF8.GetString(resultArray);
            }catch(Exception EX)
            {
                MessageBox.Show("有错");
                throw EX;
            }
           
        }*/
        /*    /// <summary>
            ///  加密
            /// </summary>
            /// <param name="key">密钥</param>
            /// <param name="password">要被加密的原始密码</param>
            /// <returns>密文base64</returns>
            public string Encrypt(string key, string password)
            {
                using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
                {
                    aesAlg.Key = aseKey;
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
            public string Decrypt(string key, string EncryptStr)
            {
                byte[] inputBytes = Convert.FromBase64String(EncryptStr);
                using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
                {
                    aesAlg.Key = aseKey;
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
            }*/
    }
}
