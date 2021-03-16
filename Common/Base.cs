using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// 扩展方法，调用时必须“对象实例.xxx()”
/// </summary>
public static class Extend
{
    #region Cookie 操作
    /// <summary>
    /// 设置Cookie
    /// </summary>
    /// <param name="hour">过期时间（单位：小时），默认关闭浏览器过期</param>
    public static void SetCookie(this Controller cur, string key, string value, double? hour = null)
    {
        if (hour == null)
        {
            cur.HttpContext.Response.Cookies.Append(key, value);
        }
        else
        {
            cur.HttpContext.Response.Cookies.Append(key, value, new CookieOptions { Expires = DateTime.Now.AddHours(hour.Value) });
        }
    }

    /// <summary>
    /// 获取Cookie
    /// </summary>
    public static string GetCookie(this Controller cur, string key)
    {
        cur.HttpContext.Request.Cookies.TryGetValue(key, out string value);
        return value;
    }

    /// <summary>
    /// 删除Cookie
    /// </summary>
    public static void DeleteCookie(this Controller cur, string key)
    {
        cur.HttpContext.Response.Cookies.Delete(key);
    }
    #endregion

    /// <summary>
    /// 时间格式化（第三方扩展代码）
    /// </summary>
    /// <param name="format">格式，例如：2020-02-02 的格式为 yyyy-MM-dd</param>
    public static string ToString(this DateTime? cur, string format)
    {
        if (cur.HasValue)
        {
            return cur.Value.ToString(format);
        }
        return null;
    }

}

/// <summary>
/// 公用辅助类
/// </summary>
public class Base
{
    //文件上传---代码已迁移到Common控制器
    //文件下载---代码已迁移到Common控制器

    
    #region Json与字符串的互换
    /// <summary>
    /// DataTable 转json字符串
    /// </summary>
    public static string DataTable_Json(DataTable dt)
    {
        StringBuilder JsonStr = new StringBuilder();     //  [{"ID":1,"Title":"a{a"},{"ID":2,"Title":"b]b"},{"ID":3,"Title":"c\"c"}]
        JsonStr.Append("[");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            JsonStr.Append("{");
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                JsonStr.Append("\"");
                JsonStr.Append(dt.Columns[j].ColumnName);
                JsonStr.Append("\":\"");
                JsonStr.Append(dt.Rows[i][j].ToString());
                JsonStr.Append("\",");
            }
            JsonStr.Remove(JsonStr.Length - 1, 1);
            JsonStr.Append("},");
        }
        JsonStr.Remove(JsonStr.Length - 1, 1);   //删除最后一个字符串
        JsonStr.Append("]");
        return JsonStr.ToString();
    }
    /// <summary>
    /// DataRow 转json字符串
    /// </summary>
    public static string DataRow_Json(DataRow dr)
    {
        System.Text.StringBuilder str = new System.Text.StringBuilder("{");
        for (int i = 0; i < dr.Table.Columns.Count; i++)
        {
            str.Append("\"");
            str.Append(dr.Table.Columns[i].ColumnName);
            str.Append("\":");
            str.Append("\"");
            str.Append(dr[i]);
            str.Append("\",");
        }
        str.Remove(str.Length - 1, 1);
        str.Append("}");
        return str.ToString();
    }
    #endregion

    #region 加密 解密
    /// <summary>
    /// 加密
    /// </summary>
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
    #endregion

    #region 缓存
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
    public static void SetCache(object key, object value, int expirationTime = 20, bool absolutely = true)
    {
        if (absolutely)
        {
            Cache.Set(key, value, TimeSpan.FromMinutes(expirationTime));//绝对过期时间
        }
        else
        {
            Cache.Set(key, value, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(expirationTime),//相对过期时间
            });
        }
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
    #endregion
}
