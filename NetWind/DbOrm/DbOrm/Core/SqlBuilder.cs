using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;

namespace DbOrm
{
    public class SqlBuilderCore
    {
        protected IDbConnection Connection;
        protected IDbTransaction Transaction;
        protected bool IsClose;//调用完后是否关闭数据库连接
        protected Dictionary<string, object> Params = new Dictionary<string, object>();//sql参数

        private string Column;
        private string Table;
        private string _Where;
        private string Join;
        private string _OrderBy;
        private int SkipCount;//跳过多少条（分页用到）   ，此处不用 startIndex 起始索引，因为不同数据库的起始索引是不一样的
        private int TakeCount;//返回之后的多少条（分页用到）
        
        public SqlBuilderCore(IDbConnection conn, bool isClose, IDbTransaction transaction = null)
        {
            Connection = conn;
            IsClose = isClose;
            Transaction = transaction;
        }
        public void AddParams(object param)
        {
            foreach (var p in param.GetType().GetProperties())
            {
                Params['@' + p.Name] = p.GetValue(param);
            }
        }
        public void Select(string column) { this.Column = column; }
        public void From(string table) { this.Table = table; }
        public void Where(string where, object param = null)
        {
            if (param != null) { AddParams(param); }
            this._Where = where;
        }
        public void WhereAnd(string where, object param = null)
        {
            if (param != null) { AddParams(param); }
            this._Where = this._Where != null && this._Where.Length > 0 ? this._Where + " and " + where : where;
        }
        public void WhereOr(string where, object param = null)
        {
            if (param != null) { AddParams(param); }
            this._Where = this._Where != null && this._Where.Length > 0 ? this._Where + " or " + where : where;
        }
        public void LeftJoin(string joinTable, string joinWhere, object param = null)
        {
            if (param != null) { AddParams(param); }
            this.Join += $"\nleft join {joinTable} on {joinWhere}";
        }
        public void InnerJoin(string joinTable, string joinWhere, object param = null)
        {
            if (param != null) { AddParams(param); }
            this.Join += $"\ninner join {joinTable} on {joinWhere}";
        }
        public void RightJoin(string joinTable, string joinWhere, object param = null)
        {
            if (param != null) { AddParams(param); }
            this.Join += $"\nright join {joinTable} on {joinWhere}";
        }
        public void OrderBy(string orderByColumn)
        {
            if (!Regex.IsMatch(orderByColumn, @"^(\w+|\[\w+\]|\w+\.(\w+|\[\w+\]))(\s+(asc|desc))?$", RegexOptions.IgnoreCase)) { throw new Exception($"字符串”{orderByColumn}“存在sql注入风险"); }
            _OrderBy = _OrderBy != null && _OrderBy.Length > 0 ? _OrderBy + ',' + orderByColumn : orderByColumn;
        }
        public void SkipTake(int skipCount, int takeCount)
        {
            this.SkipCount = skipCount;
            this.TakeCount = takeCount;
            Params["@_skipCount"] = SkipCount;
            Params["@_takeCount"] = TakeCount;
        }
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            if (Connection is Microsoft.Data.SqlClient.SqlConnection)
            {
                str.Append("select ").Append(Column);
                if (TakeCount > 0) { str.Append($",row_number() over(order by {(_OrderBy == null ? "(select 0)" : _OrderBy)}) as _RowNum"); }//分页的序号列
                str.Append(" from ").Append(Table);
                str.AppendLine(this.Join);
                if (_Where != null && _Where.Length > 0) { str.Append(" where ").Append(_Where); }
                if (TakeCount == 0 && _OrderBy != null) { str.Append(" order by ").Append(_OrderBy); }//如果有分页排序则在上面构建好了
                if (TakeCount > 0)
                {
                    return $@"
with _tab as(
{str}
)
select * from _tab where _RowNum between @_skipCount+1 and @_skipCount+@_takeCount
";
                }
            }
            else if (Connection is Microsoft.Data.Sqlite.SqliteConnection)
            {
                str.Append("select ").Append(Column);
                str.Append(" from ").Append(Table);
                str.AppendLine(this.Join);
                if (_Where != null && _Where.Length > 0) { str.Append(" where ").Append(_Where); }
                if (_OrderBy != null) { str.Append(" order by ").Append(_OrderBy); }
                if (TakeCount > 0) { str.Append(" LIMIT @_skipCount,@_takeCount"); }
            }
            else
            {
                throw new Exception("请在此处加判断");
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
    }

    /// <summary>
    /// Sql查询构造器
    /// </summary>
    public class SqlBuilder : SqlBuilderCore
    {
        public SqlBuilder(IDbConnection conn, bool isClose, IDbTransaction transaction = null) : base(conn, isClose, transaction) { }
        public new SqlBuilder Select(string column) { base.Select(column); return this; }
        public new SqlBuilder From(string table) { base.From(table); return this; }
        public new SqlBuilder Where(string where, object param = null) { base.Where(where, param); return this; }
        public new SqlBuilder WhereAnd(string where, object param = null) { base.WhereAnd(where, param); return this; }
        public new SqlBuilder WhereOr(string where, object param = null) { base.WhereOr(where, param); return this; }
        public new SqlBuilder LeftJoin(string joinTable, string joinWhere, object param = null) { base.LeftJoin(joinTable, joinWhere, param); return this; }
        public new SqlBuilder InnerJoin(string joinTable, string joinWhere, object param = null) { base.InnerJoin(joinTable, joinWhere, param); return this; }
        public new SqlBuilder RightJoin(string joinTable, string joinWhere, object param = null) { base.RightJoin(joinTable, joinWhere, param); return this; }
        public new SqlBuilder OrderBy(string orderByColumn) { base.OrderBy(orderByColumn); return this; }
        /// <summary>
        /// 查询
        /// </summary>
        public List<dynamic> Query()
        {
            if (IsClose)
            {
                try { return Connection.Query(this.ToString(), Params, Transaction); }
                finally { Connection.Close(); }
            }
            return Connection.Query(this.ToString(), Params, Transaction);
        }
        /// <summary>
        /// 查询 跳过 skipCount条，返回连续的 takeCount条
        /// </summary>
        /// <param name="skipCount">跳过的条数</param>
        /// <param name="takeCount">返回连续的条数</param>
        public List<dynamic> Query(int skipCount, int takeCount)
        {
            base.SkipTake(skipCount, takeCount);
            return Query();
        }
        /// <summary>
        /// 查询第一行
        /// </summary>
        public dynamic QuerySingle()
        {
            base.SkipTake(0, 1);
            List<dynamic> list = Query();
            return list.Count > 0 ? list[0] : null;
        }
    }

    /// <summary>
    /// Sql查询构造器
    /// </summary>
    public class SqlBuilder<T> : SqlBuilderCore
    {
        private List<Type> Types = new List<Type>();
        public SqlBuilder(IDbConnection conn, bool isClose, IDbTransaction transaction = null) : base(conn, isClose, transaction) { Types.Add(typeof(T)); }
        public new SqlBuilder<T> Select(string column) { base.Select(column); return this; }
        public new SqlBuilder<T> From(string table) { base.From(table); return this; }
        public new SqlBuilder<T> Where(string where, object param = null) { base.Where(where, param); return this; }
        public new SqlBuilder<T> WhereAnd(string where, object param = null) { base.WhereAnd(where, param); return this; }
        public new SqlBuilder<T> WhereOr(string where, object param = null) { base.WhereOr(where, param); return this; }
        public new SqlBuilder<T> LeftJoin(string joinTable, string joinWhere, object param = null) { base.LeftJoin(joinTable, joinWhere, param); return this; }
        public new SqlBuilder<T> InnerJoin(string joinTable, string joinWhere, object param = null) { base.InnerJoin(joinTable, joinWhere, param); return this; }
        public new SqlBuilder<T> RightJoin(string joinTable, string joinWhere, object param = null) { base.RightJoin(joinTable, joinWhere, param); return this; }
        public SqlBuilder<T> LeftJoin<TTable>(string joinWhere, object param = null) { this.Types.Add(typeof(TTable)); base.LeftJoin(typeof(TTable).Name, joinWhere, param); return this; }
        public SqlBuilder<T> InnerJoin<TTable>(string joinWhere, object param = null) { this.Types.Add(typeof(TTable)); base.InnerJoin(typeof(TTable).Name, joinWhere, param); return this; }
        public SqlBuilder<T> RightJoin<TTable>(string joinWhere, object param = null) { this.Types.Add(typeof(TTable)); base.RightJoin(typeof(TTable).Name, joinWhere, param); return this; }
        public new SqlBuilder<T> OrderBy(string orderByColumn) { base.OrderBy(orderByColumn); return this; }
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
            base.SkipTake(skipCount, takeCount);
            return Query();
        }
        /// <summary>
        /// 查询第一行
        /// </summary>
        public T QuerySingle()
        {
            base.SkipTake(0, 1);
            List<T> list = Query();
            return list.Count > 0 ? list[0] : default(T);
        }
    }
    
}
