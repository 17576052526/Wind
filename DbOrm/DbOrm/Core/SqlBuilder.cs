using Microsoft.Extensions.Caching.Memory;  //需要安装 Microsoft.Extensions.Caching.Memory
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DbOrm
{
    /// <summary>
    /// Sql查询构造器
    /// </summary>
    /// <typeparam name="T">最终返回的数据类型</typeparam>
    public class SqlBuilder<T>
    {
        private IDbConnection Connection;
        private IDbTransaction Transaction;
        private string Column;
        private string Table;
        private string _Where;
        private string Join;
        private string _OrderBy;
        private int SkipCount;//跳过多少条（分页用到）   ，此处不用 startIndex 起始索引，因为不同数据库的起始索引是不一样的
        private int TakeCount;//返回之后的多少条（分页用到）
        private Dictionary<string, object> Params = new Dictionary<string, object>();//sql参数
        private List<Type> Types;
        private bool IsClose;//调用完后是否关闭数据库连接
        public SqlBuilder(IDbConnection conn, IDbTransaction transaction = null, bool isClose = false)
        {
            Connection = conn;
            Transaction = transaction;
            Types = new List<Type>() { typeof(T) };
            IsClose = isClose;
        }
        public void AddParameter(string name, object value)
        {
            this.Params.Add(name, value);
        }
        public SqlBuilder<T> Select(string column)
        {
            this.Column = column;
            return this;
        }
        public SqlBuilder<T> From(string table)
        {
            this.Table = table;
            return this;
        }
        public SqlBuilder<T> Where(string where, object param = null)
        {
            if (param != null)
            {
                foreach (var p in param.GetType().GetProperties())
                {
                    this.Params.Add('@' + p.Name, p.GetValue(param));
                }
            }
            this._Where = where;
            return this;
        }
        public SqlBuilder<T> WhereAnd(string where, object param = null)
        {
            return Where(this._Where != null && this._Where.Length > 0 ? this._Where + " and " + where : where, param);
        }
        public SqlBuilder<T> WhereOr(string where, object param = null)
        {
            return Where(this._Where != null && this._Where.Length > 0 ? this._Where + " or " + where : where, param);
        }
        public SqlBuilder<T> LeftJoin<TJoin>(string joinWhere)
        {
            this.Types.Add(typeof(TJoin));
            this.Join += $"\nleft join {typeof(TJoin).Name} on {joinWhere}";
            return this;
        }
        public SqlBuilder<T> InnerJoin<TJoin>(string joinWhere)
        {
            this.Types.Add(typeof(TJoin));
            this.Join += $"\ninner join {typeof(TJoin).Name} on {joinWhere}";
            return this;
        }
        public SqlBuilder<T> RightJoin<TJoin>(string joinWhere)
        {
            this.Types.Add(typeof(TJoin));
            this.Join += $"\nright join {typeof(TJoin).Name} on {joinWhere}";
            return this;
        }
        public SqlBuilder<T> OrderBy(string orderByColumn)
        {
            if (!Regex.IsMatch(orderByColumn, @"^(\w+|\[\w+\]|\w+\.(\w+|\[\w+\]))(\s+(asc|desc))?$", RegexOptions.IgnoreCase)) { throw new Exception($"字符串”{orderByColumn}“存在sql注入风险"); }
            this._OrderBy += orderByColumn + ',';
            return this;
        }
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("select ").Append(Column);
            if (TakeCount > 0) { str.Append($",row_number() over(order by {(_OrderBy == null ? "(select 0)" : _OrderBy.TrimEnd(','))}) as _RowNum"); }//分页的序号列
            str.Append(" from ").Append(Table);
            str.AppendLine(this.Join);
            if (_Where != null && _Where.Length > 0) { str.Append(" where ").Append(_Where); }
            if (TakeCount == 0 && _OrderBy != null) { str.Append(" order by ").Append(_OrderBy.TrimEnd(',')); }//如果有分页排序则在上面构建好了
            //分页必须在最后构造
            if (TakeCount > 0)
            {
                return $@"
with _tab as(
{str}
)
select * from _tab where _RowNum between @_start and @_end
";
            }
            return str.ToString();
        }
        /// <summary>
        /// 查询第一行第一例
        /// </summary>
        public object QueryScalar()
        {
            if (IsClose)
            {
                try { return Connection.ExecuteScalar(this.ToString(), Params, Transaction); }
                finally { Connection.Close(); }
            }
            return Connection.ExecuteScalar(this.ToString(), Params, Transaction);
        }
        /// <summary>
        /// 查询
        /// </summary>
        public List<T> Query()
        {
            if (IsClose)
            {
                try { return Connection.Query<T>(this.ToString(), Params, Types.ToArray(), Transaction); }
                finally { Connection.Close(); }
            }
            return Connection.Query<T>(this.ToString(), Params, Types.ToArray(), Transaction);
        }
        /// <summary>
        /// 查询 跳过 skipCount条，返回连续的 takeCount条
        /// </summary>
        /// <param name="skipCount">跳过的条数</param>
        /// <param name="takeCount">返回连续的条数</param>
        public List<T> Query(int skipCount, int takeCount)
        {
            this.SkipCount = skipCount;
            this.TakeCount = takeCount;
            Params.Add("@_start", SkipCount + 1);
            Params.Add("@_end", SkipCount + TakeCount);
            return Query();
        }
        /// <summary>
        /// 查询第一行
        /// </summary>
        public T QueryFirstRow()
        {
            this.TakeCount = 1;
            Params.Add("@_start", SkipCount + 1);
            Params.Add("@_end", SkipCount + TakeCount);
            List<T> list = Query();
            return list.Count > 0 ? list[0] : default(T);
        }

        #region 缓存
        private static MemoryCache _Cache;
        //使用 MemoryCache需要安装 Microsoft.Extensions.Caching.Memory
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
        //获取缓存，func：取数据方法
        private static TCache GetCache<TCache>(object key, Func<TCache> func)
        {
            TCache list;
            if (!Cache.TryGetValue(key, out list))
            {
                list = func();
                Cache.Set(key, list, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(20),//相对过期时间，此处固定使用20分钟过期
                });
            }
            return list;
        }
        /// <summary>
        /// 查询，先找缓存，如果没找到去数据库查然后在存入缓存
        /// </summary>
        public List<T> QueryCache()
        {
            return GetCache<List<T>>(this.ToString(), () => Query());
        }
        /// <summary>
        /// 查询，跳过 skipCount条，返回连续的 takeCount条，先找缓存，如果没找到去数据库查然后在存入缓存
        /// </summary>
        public List<T> QueryCache(int skipCount, int takeCount)
        {
            return GetCache<List<T>>(this.ToString(), () => Query(skipCount, takeCount));
        }
        /// <summary>
        /// 查询第一行第一列，先找缓存，如果没找到去数据库查然后在存入缓存
        /// </summary>
        public object QueryScalarCache<TReturn>()
        {
            return GetCache(this.ToString(), () => QueryScalar());
        }
        /// <summary>
        /// 查询第一行，先找缓存，如果没找到去数据库查然后在存入缓存
        /// </summary>
        public T QueryFirstRowCache()
        {
            return GetCache<T>(this.ToString(), () => QueryFirstRow());
        }
        #endregion
    }
}
