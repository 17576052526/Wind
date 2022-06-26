using System;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    /// <summary>
    /// RSA加密解密（非对称加密解密）
    /// 1.CreatePublicPrivateKey() 创建 公钥、私钥
    /// 2.只能公钥加密，私钥解密，这个里面公钥、私钥是指 CreatePublicPrivateKey() 创建的公钥和私钥
    /// 2.对外公布的叫公钥，只自己保存的叫私钥，这句话中的公钥可以是 CreatePublicPrivateKey()里面的公钥，也可以是 CreatePublicPrivateKey()里面的私钥
    /// 应用：
    /// 一般情况下，私钥用于对数据进行签名
    /// 一般情况下，公钥用于对签名经行验证
    /// HTTP网站在浏览器端使用公钥加密敏感数据，然后再服务器端使用私钥解密
    /// 注意事项：
    /// 1.字节转换用的是 UTF-8 ，所以如果有其他程序要对本程序生成的内容加解密，也要用 UTF-8
    /// </summary>
    public class RSAHelper
    {
        /// <summary>
        /// 创建公钥、私钥
        /// </summary>
        public void CreatePublicPrivateKey(out string publicKey, out string privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            publicKey = Convert.ToBase64String(rsa.ExportCspBlob(false));//生成公钥
            privateKey = Convert.ToBase64String(rsa.ExportCspBlob(true));//生成私钥
        }

        /// <summary>
        /// 加密
        /// </summary>
        public string Encrypt(string content, string key)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportCspBlob(Convert.FromBase64String(key));
            return Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(content), false));
        }

        /// <summary>
        /// 解密
        /// </summary>
        public string Decrypt(string content, string key)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportCspBlob(Convert.FromBase64String(key));
            return Encoding.UTF8.GetString(rsa.Decrypt(Convert.FromBase64String(content), false));
        }
    }
}
