using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace UI.Controllers.Api
{
    /// <summary>
    /// JWT 的加密解密，这里应该用 RS256 私钥签名，公钥验签，但是目前没找到 RS256相关用法
    /// </summary>
    public class JWTEncryDecrypt
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <returns></returns>
        public static string Encry(string content)
        {
            string key = "wfnk5kfp";//密钥，加密解密密钥必须一致，只能填8位
            byte[] val = Encoding.UTF8.GetBytes(content);
            byte[] KeyByte = Encoding.UTF8.GetBytes(key);
            byte[] IVByte = { 0xB3, 0x24, 0x36, 0x82, 0x17, 0xDF, 0x48, 0x93 };
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(KeyByte, IVByte), CryptoStreamMode.Write);
            cs.Write(val, 0, val.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="content"></param>
        public static string Decrypt(string content)
        {
            string key = "wfnk5kfp";//密钥，加密解密密钥必须一致，只能填8位
            byte[] val = Convert.FromBase64String(content);
            byte[] KeyByte = Encoding.UTF8.GetBytes(key);
            byte[] IVByte = { 0xB3, 0x24, 0x36, 0x82, 0x17, 0xDF, 0x48, 0x93 };
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(KeyByte, IVByte), CryptoStreamMode.Write);
            cs.Write(val, 0, val.Length);
            cs.FlushFinalBlock();
            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }

    public class JWTCache
    {
        private static MemoryCache _Cache;
        private static MemoryCache Cache
        {
            get
            {
                if (_Cache == null)
                {
                    _Cache = new MemoryCache(new MemoryCacheOptions()
                    {
                        //SizeLimit = 1000,//缓存最大为份数
                        //CompactionPercentage = 0.2,//缓存满了时，压缩20%（即删除 SizeLimit*CompactionPercentage份优先级低的缓存项）
                        //ExpirationScanFrequency = TimeSpan.FromSeconds(60)//每隔多久查找一次过期项，默认一分钟查找一次
                    });
                }
                return _Cache;
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="expirationTime">过期时间（单位分钟）</param>
        /// <param name="absolutely">true：绝对过期时间，false：相对过期时间（还未过期被访问则刷新过期时间）</param>
        public static void SetCache(object key, object value, int expirationTime)
        {
            Cache.Set(key, value, TimeSpan.FromMinutes(expirationTime));//绝对过期时间
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        public static object GetCache(object key)
        {
            return Cache.Get(key);
        }
        /// <summary>
        /// 移除缓存
        /// </summary>
        public static void RemoveCache(object key)
        {
            Cache.Remove(key);
        }
    }
    public class JWTResult
    {
        /// <summary>
        /// JWT 状态码，200：认证成功， 403：认证失败
        /// </summary>
        public int Code { set; get; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message { set; get; }
        public dynamic Header { set; get; }
        public dynamic Payload { set; get; }

    }
    /// <summary>
    /// JWT 认证标记
    /// </summary>
    public class JWTAuthorize : Attribute, IAuthorizationFilter
    {
        private string Name;
        /// <summary>
        /// 权限认证
        /// </summary>
        /// <param name="name">权限标识，用于判断某个接口是否有权限</param>
        public JWTAuthorize(string name = "")
        {
            this.Name = name;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            JWTResult result = JWT.Validation(context.HttpContext.Request.Headers["Authorization"], Name);
            if (result.Code == 403)
            {
                context.Result = new JsonResult(new { code = result.Code, msg = result.Message });
            }
            else
            {
                List<Claim> list = new List<Claim>();
                JObject payload = result.Payload;
                foreach (var m in payload)
                {
                    list.Add(new Claim(m.Key, (string)m.Value));
                }
                context.HttpContext.User.AddIdentity(new ClaimsIdentity(list));
            }

        }
    }

    
    public class JWT
    {
        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="name">用于判断某个接口是否有权限，验签的时候有用到此参数</param>
        public static string Signature(object model, string name = "")
        {
            var header = new
            {
                alg = "DES",//一般是用 HS256，或 RS256（推荐），但没找到相关的用法
                typ = "JWT"
            };
            var payload = new
            {
                iss = "WindAdmin",
                exp = DateTime.Now.AddMinutes(20),//过期时间
                iat = DateTime.Now,//发布时间
                tokenid = model.GetHashCode()+ new Random().Next(1, 9999),//token的 id，必须要加随机数
                __name = name,
            };
            //payload，model 通过反射合并到同一个json字符串里面
            StringBuilder str = new StringBuilder();
            str.AppendLine("{");
            foreach (var p in payload.GetType().GetProperties())
            {
                str.AppendFormat("\"{0}\":\"{1}\",", p.Name, p.GetValue(payload));
            }
            foreach (var p in model.GetType().GetProperties())
            {
                str.AppendFormat("\"{0}\":\"{1}\",", p.Name, p.GetValue(model));
            }
            str = str.Remove(str.Length - 1, 1);
            str.AppendLine("}");
            string headerBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header)));
            string payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(str.ToString()));
            string signature = JWTEncryDecrypt.Encry(headerBase64 + "." + payloadBase64);
            return headerBase64 + "." + payloadBase64 + "." + signature;
        }
        
        /// <summary>
        /// 验签
        /// </summary>
        public static JWTResult Validation(string token, string name)
        {
            JWTResult result = new JWTResult();
            try
            {
                string[] tokenArr = token.Split('.');
                string header = tokenArr[0];
                string payload = tokenArr[1];
                string signature = JWTEncryDecrypt.Decrypt(tokenArr[2]);
                string[] signatureArr = signature.Split('.');
                //验证是否成功
                if (header == signatureArr[0] && payload == signatureArr[1])
                {
                    dynamic model = JsonConvert.DeserializeObject<dynamic>(Encoding.UTF8.GetString(Convert.FromBase64String(payload)));
                    DateTime exp = JWTCache.GetCache(model.tokenid) ?? model.exp;
                    if (model.__name != name)//由签名和验签的两个参数，判断某个接口是否有权限，此处代码可以根据实际业务修改
                    {
                        result.Code = 403;
                        result.Message = "您无此权限";
                    }
                    else if (exp < DateTime.Now)
                    {
                        result.Code = 403;
                        result.Message = "Token 已过期";
                    }
                    else
                    {
                        result.Header = JsonConvert.DeserializeObject<dynamic>(Encoding.UTF8.GetString(Convert.FromBase64String(header)));
                        result.Payload = model;
                        result.Code = 200;
                        //自动续期
                        if (DateTime.Now > exp.AddMinutes(-10))
                        {
                            JWTCache.SetCache(model.tokenid, DateTime.Now.AddMinutes(20), 20);
                        }
                    }
                }
                else
                {
                    result.Code = 403;
                    result.Message = "非法Token，Token被篡改过";
                }
            }
            catch
            {
                result.Code = 403;
                result.Message = "ToKen 认证失败";
            }
            return result;
        }

        /// <summary>
        /// 注销
        /// </summary>
        public static void Cancel(string token)
        {
            try
            {
                string[] tokenArr = token.Split('.');
                string header = tokenArr[0];
                string payload = tokenArr[1];
                string signature = JWTEncryDecrypt.Decrypt(tokenArr[2]);
                string[] signatureArr = signature.Split('.');
                //验证是否成功
                if (header == signatureArr[0] && payload == signatureArr[1])
                {
                    dynamic model = JsonConvert.DeserializeObject<dynamic>(Encoding.UTF8.GetString(Convert.FromBase64String(payload)));
                    DateTime exp = model.exp;
                    if (exp < DateTime.Now)//token 里面的exp 已经过期了，则移除缓存
                    {
                        JWTCache.RemoveCache(model.tokenid);
                    }
                    else//未过期，则设置为过期
                    {
                        JWTCache.SetCache(model.tokenid, DateTime.Now.AddMinutes(-1), (exp - DateTime.Now).Minutes + 1);
                    }
                }
                else
                {
                    throw new Exception("非法Token，Token被篡改过");
                }
            }
            catch
            {
                throw new Exception("token 解析时发生异常");//此处是抛错
            }
        }
    }
}
