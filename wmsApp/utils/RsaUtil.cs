using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace wmsApp.utils
{
    public class RSAUtil
    {
        // C#的Base64转java的Base64
        private static string Base64CsharpToJava(string base64EncodedCipherText)
        {
            char[] padding = { '=' };
            string dummyData = base64EncodedCipherText.TrimEnd(padding).Replace('+', '-').Replace('/', '_');
            return dummyData;
        }

        // java的Base64转C#的Base64
        private static string Base64JavaToCsharp(string base64EncodedCipherText)
        {
            string dummyData = base64EncodedCipherText.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+").Replace("_", "/").Replace("-", "+");
            if (dummyData.Length % 4 > 0)
            {
                dummyData = dummyData.PadRight(dummyData.Length + 4 - dummyData.Length % 4, '=');
            }
            return dummyData;
        }


        /// <summary>
        /// 使用公钥加密
        /// </summary>
        /// <returns></returns>
        /*  public string Encrypt(string strText, string strPublicKey)
          {
              try
              {

               MessageBox.Show("rsa" );
              RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
              byte[] publicKeyBytes = Convert.FromBase64String(strPublicKey);
              rsa.ImportCspBlob(publicKeyBytes);
              MessageBox.Show("成功");
              byte[] byteText = Encoding.UTF8.GetBytes(strText);
              byte[] byteEntry = rsa.Encrypt(byteText, false);
              return Convert.ToBase64String(byteEntry);
              }
              catch (Exception ex)
              {
                  MessageBox.Show("导入公钥出现问题：" + ex.Message);
              }
              return "无";
          }*/
        private static Encoding Encoding_UTF8 = Encoding.UTF8;
        private AsymmetricKeyParameter GetPublicKeyParameter(string keyBase64)
        {
            keyBase64 = keyBase64.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            byte[] publicInfoByte = Convert.FromBase64String(keyBase64);
            Asn1Object pubKeyObj = Asn1Object.FromByteArray(publicInfoByte);//这里也可以从流中读取，从本地导入
            AsymmetricKeyParameter pubKey = PublicKeyFactory.CreateKey(publicInfoByte);
            return pubKey;
        }
        public string Encrypt(string data, string publicKey)
        {
            //非对称加密算法，加解密用
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());

            //加密
            try
            {
                engine.Init(true, GetPublicKeyParameter(publicKey));
                byte[] byteData = Encoding_UTF8.GetBytes(data);
                var ResultData = engine.ProcessBlock(byteData, 0, byteData.Length);
                return Convert.ToBase64String(ResultData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private AsymmetricKeyParameter GetPrivateKeyParameter(string keyBase64)
        {
            keyBase64 = keyBase64.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            byte[] privateInfoByte = Convert.FromBase64String(keyBase64);
            // Asn1Object priKeyObj = Asn1Object.FromByteArray(privateInfoByte);//这里也可以从流中读取，从本地导入
            // PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);
            AsymmetricKeyParameter priKey = PrivateKeyFactory.CreateKey(privateInfoByte);
            return priKey;
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
            MessageBox.Show("1");
        
            rsa.ImportCspBlob(Convert.FromBase64String(strPrivateKey));
            MessageBox.Show("2");
            byte[] byteEntry = Convert.FromBase64String(strEntryText);
            MessageBox.Show("3");
            byte[] byteText = rsa.Decrypt(byteEntry, false);
            return Encoding.UTF8.GetString(byteText);
        }

        /// <summary>
        /// 获取公钥和私钥
        /// </summary>
        /// <returns></returns>
        /*    public Dictionary<string, string> GetKey()
            {
                Dictionary<string, string> dictKey = new Dictionary<string, string>();
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                string public_Key = Convert.ToBase64String(rsa.ExportCspBlob(false));
                string private_Key = Convert.ToBase64String(rsa.ExportCspBlob(true));
                dictKey.Add("publickey", public_Key);
                dictKey.Add("privatekey", private_Key);
                return dictKey;
            }*/
  /*      public Dictionary<string, string> GetKey()
        {
            Dictionary<string, string> dictKey = new Dictionary<string, string>();
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            string public_Key = Convert.ToBase64String(RSA.ExportCspBlob(false));
            string private_Key = Convert.ToBase64String(RSA.ExportCspBlob(true));

            dictKey.Add("publickey", public_Key);
            dictKey.Add("privatekey", public_Key);
            return dictKey;
        }*/
        public Dictionary<string, string> GetKey()
        {
            Dictionary<string, string> dictKey = new Dictionary<string, string>();
            //RSA密钥对的构造器
            RsaKeyPairGenerator keyGenerator = new RsaKeyPairGenerator();

            //RSA密钥构造器的参数
            RsaKeyGenerationParameters param = new RsaKeyGenerationParameters(
                    Org.BouncyCastle.Math.BigInteger.ValueOf(3),
                    new Org.BouncyCastle.Security.SecureRandom(),
                    1024,   //密钥长度 
                    25);
            //用参数初始化密钥构造器
            keyGenerator.Init(param);
            //产生密钥对
            AsymmetricCipherKeyPair keyPair = keyGenerator.GenerateKeyPair();
            //获取公钥和密钥
            AsymmetricKeyParameter publicKey = keyPair.Public;
            AsymmetricKeyParameter privateKey = keyPair.Private;

            SubjectPublicKeyInfo subjectPublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);

            Asn1Object asn1ObjectPublic = subjectPublicKeyInfo.ToAsn1Object();

            byte[] publicInfoByte = asn1ObjectPublic.GetEncoded("UTF-8");
            Asn1Object asn1ObjectPrivate = privateKeyInfo.ToAsn1Object();
            byte[] privateInfoByte = asn1ObjectPrivate.GetEncoded("UTF-8");

            dictKey.Add("publickey", Convert.ToBase64String(publicInfoByte));
            dictKey.Add("privatekey", Convert.ToBase64String(privateInfoByte));

            return dictKey;
        }


        /// <summary>
        /// 私钥加密
        /// </summary>
        /// <param name="data">加密内容</param>
        /// <param name="privateKey">私钥（Base64后的）</param>
        /// <returns>返回Base64内容</returns>
        public string EncryptByPrivateKey(string data, string privateKey)
        {
            //非对称加密算法，加解密用
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());

            //加密
            try
            {
                engine.Init(true, GetPrivateKeyParameter(privateKey));
                byte[] byteData = Encoding_UTF8.GetBytes(data);
                var ResultData = engine.ProcessBlock(byteData, 0, byteData.Length);
                return Convert.ToBase64String(ResultData);
                //Console.WriteLine("密文（base64编码）:" + Convert.ToBase64String(testData) + Environment.NewLine);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 私钥解密
        /// </summary>
        /// <param name="data">待解密的内容</param>
        /// <param name="privateKey">私钥（Base64编码后的）</param>
        /// <returns>返回明文</returns>
        #region 使用私钥解密内容
        /// <summary>
        /// 私钥解密
        /// </summary>
        /// <param name="data">待解密的内容</param>
        /// <param name="privateKey">私钥（Base64编码后的）</param>
        /// <returns>返回明文</returns>

        /*        public string DecryptByPrivateKey(string data, string privateKey)
                {
                    data = data.Replace("\r", "").Replace("\n", "").Replace(" ", "");
                    //非对称加密算法，加解密用
                    IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());

                    //解密
                    try
                    {
                        engine.Init(false, GetPrivateKeyParameter(privateKey));
                        byte[] byteData = Convert.FromBase64String(data);
                        MessageBox.Show(byteData.Length.ToString());
                        var ResultData = engine.ProcessBlock(byteData, 0, byteData.Length);
                        return Encoding_UTF8.GetString(ResultData);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("错误");
                        throw ex;
                    }
                }
        */
        /*  public string DecryptByPrivateKey(string encryptedData, string privateKey)
          {
              try
              {
                  byte[] encryptedBytes = Convert.FromBase64String(encryptedData);

                  using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                  {
                      rsa.FromXmlString(privateKey);
                      byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, false);
                      string decryptedData = Encoding.UTF8.GetString(decryptedBytes);
                      return decryptedData;
                  }
              }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message);
                  // 处理解密异常
                  throw ex;
              }
          }*/

        /*     public string DecryptByPrivateKey(string data, string privateKeyJava)
             {
                 try
                 {
                     string hashAlgorithm = "SHA256withRSA";
                     MessageBox.Show("1");
                     string encoding = "UTF-8";

                     RsaKeyParameters privateKeyParam = (RsaKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKeyJava));
                     ISigner signer = SignerUtilities.GetSigner(hashAlgorithm);
                     MessageBox.Show("2");
                     signer.Init(true, privateKeyParam);//参数为true验签，参数为false加签  
                     var dataByte = Encoding.GetEncoding(encoding).GetBytes(data);
                     signer.BlockUpdate(dataByte, 0, dataByte.Length);
                     MessageBox.Show("3");
                     MessageBox.Show(Convert.ToBase64String(signer.GenerateSignature()));
                     //return Encoding.GetEncoding(encoding).GetString(signer.GenerateSignature()); //签名结果 非Base64String  
                     return Convert.ToBase64String(signer.GenerateSignature());
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show("解析错误");
                     throw ex;
                 }
             }
     */
       
        public string DecryptByPrivateKey(string data, string privateKey)
        {
            data = data.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            MessageBox.Show("data!!!!"+data);
            //非对称加密算法，加解密用
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());
         
            //解密
            try
            {

                engine.Init(false, GetPrivateKeyParameter(privateKey));
                byte[] byteData = Convert.FromBase64String(data);
                var ResultData = engine.ProcessBlock(byteData, 0, byteData.Length);
                return Encoding_UTF8.GetString(ResultData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
        }


        #endregion


        /// <summary>
        /// 公钥加密
        /// </summary>
        /// <param name="data">加密内容</param>
        /// <param name="publicKey">公钥（Base64编码后的）</param>
        /// <returns>返回Base64内容</returns>
        public string EncryptByPublicKey(string data, string publicKey)
        {
            //非对称加密算法，加解密用
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());

            //加密
            try
            {
                engine.Init(true, GetPublicKeyParameter(publicKey));
                byte[] byteData = Encoding_UTF8.GetBytes(data);
                var ResultData = engine.ProcessBlock(byteData, 0, byteData.Length);
                return Convert.ToBase64String(ResultData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 公钥解密
        /// </summary>
        /// <param name="data">待解密的内容</param>
        /// <param name="publicKey">公钥（Base64编码后的）</param>
        /// <returns>返回明文</returns>
        public string DecryptByPublicKey(string data, string publicKey)
        {
            data = data.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            //非对称加密算法，加解密用
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());

            //解密
            try
            {
                engine.Init(false, GetPublicKeyParameter(publicKey));
                byte[] byteData = Convert.FromBase64String(data);
                var ResultData = engine.ProcessBlock(byteData, 0, byteData.Length);
                return Encoding_UTF8.GetString(ResultData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }

   

}
