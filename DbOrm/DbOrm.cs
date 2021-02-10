using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Dynamic;

namespace DbOrm
{
    /// <summary>
    /// 数据访问层接口（为了实现多态）
    /// </summary>
    public abstract class IDAL
    {
        internal abstract string Insert();
        internal abstract string Update();
    }

    internal class TNull { }

    #region Sql构造器
    //SqlBuilder 的查询有两种实现方式：第一种，直接在SqlBuilder里面调用DB的底层查询，第二种，在DB里面写一个 Qyery(SqlBuilder sql) 的方法，这样可以不传Command，分页参数在Query里面填充
    public class SqlBuilder
    {
        private IDbCommand _Command;
        private string _Column;
        private string _Table;
        private string _Where;
        private string _Join;
        private string _OrderBy;
        private int _SkipCount;//跳过多少条（分页用到）   ，此处不用 startIndex 起始索引，因为不同数据库的起始索引是不一样的
        private int _TakeCount;//返回之后的多少条（分页用到）
        private bool _IsClose;//执行sql之后是否关闭数据库连接，true：关闭，false：不关闭
        /// <summary>
        /// Command == null 执行sql之后关闭数据库连接，Command != null 执行sql之后不关闭数据库连接
        /// </summary>
        internal SqlBuilder(IDbCommand cmd = null)
        {
            if (cmd == null)
            {
                this._Command = DB.CreateConnection().CreateCommand();
                _IsClose = true;
            }
            else
            {
                this._Command = cmd;
            }
        }
        public SqlBuilder Select(string column)
        {
            this._Column = column;
            return this;
        }
        public SqlBuilder From(string table)
        {
            this._Table = table;
            return this;
        }
        public SqlBuilder Where(string where)
        {
            this._Where = where;
            return this;
        }
        public SqlBuilder LeftJoin<T>(string joinWhere)
        {
            this._Join += $"\nleft join {typeof(T).Name} on {joinWhere}";
            return this;
        }
        public SqlBuilder InnerJoin<T>(string joinWhere)
        {
            this._Join += $"\ninner join {typeof(T).Name} on {joinWhere}";
            return this;
        }
        public SqlBuilder OrderAsc(string columnName)
        {
            this._OrderBy += $"{Regex.Replace(columnName, "\\s", "")} asc,";//正则表达式去掉空白字符，防止sql注入
            return this;
        }
        public SqlBuilder OrderDesc(string columnName)
        {
            this._OrderBy += $"{Regex.Replace(columnName, "\\s", "")} desc,";//正则表达式去掉空白字符，防止sql注入
            return this;
        }
        /// <summary>
        /// 跳过多少条，返回之后的多少条
        /// </summary>
        public SqlBuilder Page(int skipCount, int takeCount)
        {
            this._SkipCount = skipCount;
            this._TakeCount = takeCount;
            return this;
        }
        /// <summary>
        /// 查询第一行第一例
        /// </summary>
        public T QueryScalar<T>(object param = null)
        {
            if (_IsClose)
            {
                try { return (T)DB.ExecuteScalar(_Command, this.ToString(), param); }
                finally { _Command.Connection.Close(); }
            }
            return (T)DB.ExecuteScalar(_Command, this.ToString(), param);
        }
        //查询实体不关注多表联查
        /// <summary>
        /// 查询满足条件的第一行
        /// </summary>
        public T QueryFirstRow<T>(object param = null)
        {
            if (_IsClose)
            {
                try { return DB.Query<T, TNull, TNull, TNull, TNull, TNull, TNull, T>(_Command, this.ToString(), (t1, t2, t3, t4, t5, t6, t7) => { return t1; }, param)[0]; }
                finally { _Command.Connection.Close(); }
            }
            return DB.Query<T, TNull, TNull, TNull, TNull, TNull, TNull, T>(_Command, this.ToString(), (t1, t2, t3, t4, t5, t6, t7) => { return t1; }, param)[0];
        }
        public List<dynamic> Query(object param = null)
        {
            if (_IsClose)
            {
                try { return DB.Query(_Command, this.ToString(), param); }
                finally { _Command.Connection.Close(); }
            }
            return DB.Query(_Command, this.ToString(), param);
        }
        public List<T> Query<T>(object param = null)
        {
            if (_IsClose)
            {
                try { return DB.Query<T, TNull, TNull, TNull, TNull, TNull, TNull, T>(_Command, this.ToString(), (t1, t2, t3, t4, t5, t6, t7) => { return t1; }, param); }
                finally { _Command.Connection.Close(); }
            }
            return DB.Query<T, TNull, TNull, TNull, TNull, TNull, TNull, T>(_Command, this.ToString(), (t1, t2, t3, t4, t5, t6, t7) => { return t1; }, param);
        }
        public List<T1> Query<T1, T2>(Action<T1, T2> func, object param = null)
        {
            if (_IsClose)
            {
                try { return DB.Query<T1, T2, TNull, TNull, TNull, TNull, TNull, T1>(_Command, this.ToString(), (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2); return t1; }, param); }
                finally { _Command.Connection.Close(); }
            }
            return DB.Query<T1, T2, TNull, TNull, TNull, TNull, TNull, T1>(_Command, this.ToString(), (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2); return t1; }, param);
        }
        public List<T1> Query<T1, T2, T3>(Action<T1, T2, T3> func, object param = null)
        {
            if (_IsClose)
            {
                try { return DB.Query<T1, T2, T3, TNull, TNull, TNull, TNull, T1>(_Command, this.ToString(), (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2, t3); return t1; }, param); }
                finally { _Command.Connection.Close(); }
            }
            return DB.Query<T1, T2, T3, TNull, TNull, TNull, TNull, T1>(_Command, this.ToString(), (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2, t3); return t1; }, param);
        }
        public List<T1> Query<T1, T2, T3, T4>(Action<T1, T2, T3, T4> func, object param = null)
        {
            if (_IsClose)
            {
                try { return DB.Query<T1, T2, T3, T4, TNull, TNull, TNull, T1>(_Command, this.ToString(), (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2, t3, t4); return t1; }, param); }
                finally { _Command.Connection.Close(); }
            }
            return DB.Query<T1, T2, T3, T4, TNull, TNull, TNull, T1>(_Command, this.ToString(), (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2, t3, t4); return t1; }, param);
        }
        public List<T1> Query<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> func, object param = null)
        {
            if (_IsClose)
            {
                try { return DB.Query<T1, T2, T3, T4, T5, TNull, TNull, T1>(_Command, this.ToString(), (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2, t3, t4, t5); return t1; }, param); }
                finally { _Command.Connection.Close(); }
            }
            return DB.Query<T1, T2, T3, T4, T5, TNull, TNull, T1>(_Command, this.ToString(), (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2, t3, t4, t5); return t1; }, param);
        }
        public List<T1> Query<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> func, object param = null)
        {
            if (_IsClose)
            {
                try { return DB.Query<T1, T2, T3, T4, T5, T6, TNull, T1>(_Command, this.ToString(), (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2, t3, t4, t5, t6); return t1; }, param); }
                finally { _Command.Connection.Close(); }
            }
            return DB.Query<T1, T2, T3, T4, T5, T6, TNull, T1>(_Command, this.ToString(), (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2, t3, t4, t5, t6); return t1; }, param);
        }
        public List<T1> Query<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> func, object param = null)
        {
            if (_IsClose)
            {
                try { return DB.Query<T1, T2, T3, T4, T5, T6, T7, T1>(_Command, this.ToString(), (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2, t3, t4, t5, t6, t7); return t1; }, param); }
                finally { _Command.Connection.Close(); }
            }
            return DB.Query<T1, T2, T3, T4, T5, T6, T7, T1>(_Command, this.ToString(), (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2, t3, t4, t5, t6, t7); return t1; }, param);
        }
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("select ").Append(_Column);
            if (_TakeCount > 0) { str.Append($",row_number() over(order by {_OrderBy ?? "(select 0)"}) as __RowNum"); }//分页的序号列
            str.Append(" from ").Append(_Table);
            str.AppendLine(this._Join);
            if (_Where != null && _Where.Length > 0) { str.Append(" where ").Append(_Where); }
            if (_TakeCount == 0 && _OrderBy != null) { str.Append(" order by ").Append(_OrderBy.TrimEnd(',')); }//如果有分页排序则在上面构建好了
            //分页必须在最后构造
            if (_TakeCount > 0)
            {
                DB.AddParameter(_Command, new { __start = _SkipCount + 1, __end = _SkipCount + _TakeCount });//添加分页参数
                return $@"
with __tab as(
{str}
)
select * from __tab where __RowNum between @__start and @__end
";
            }
            return str.ToString();
        }
    }
    #endregion

    public partial class DB
    {
        #region 实例扩展方法，调用之后不关闭数据库连接，由外面关闭
        public int Inserts(IDAL model)
        {
            return this.ExecuteNonQuerys(model.Insert(), model);
        }
        public int Updates(IDAL model, string where, object param = null)
        {
            AddParameter(Command, model, true);
            return this.ExecuteNonQuerys(model.Update() + " where " + where, param);
        }
        public int Deletes<T>(string where, object param = null) where T : IDAL
        {
            return this.ExecuteNonQuerys($"delete from {typeof(T).Name} where {where}", param);
        }
        public SqlBuilder Selects<T>(string column = "*")
        {
            return new SqlBuilder(Command).Select(column).From(typeof(T).Name);
        }
        #endregion

        #region 静态扩展方法，调用之后关闭数据库连接
        public static int Insert(IDAL model)
        {
            return ExecuteNonQuery(model.Insert(), model);
        }

        public static int Update(IDAL model, string where, object param = null)
        {
            using (IDbConnection conn = CreateConnection())
            {
                IDbCommand cmd = conn.CreateCommand();
                AddParameter(cmd, model, true);
                return ExecuteNonQuery(cmd, model.Update() + " where " + where, param);
            }
        }

        public static int Delete<T>(string where, object param = null) where T : IDAL
        {
            return ExecuteNonQuery($"delete from {typeof(T).Name} where {where}", param);
        }
        //select 不用 where T : IDAL
        public static SqlBuilder Select<T>(string column = "*")
        {
            return new SqlBuilder().Select(column).From(typeof(T).Name);
        }
        #endregion
    }

    public partial class DB : IDisposable
    {
        private IDbConnection Connection;
        private IDbCommand Command;
        public DB()
        {
            Connection = CreateConnection();
            Command = Connection.CreateCommand();
        }
        public void Dispose()
        {
            Connection.Close();
        }

        #region 实例基函数，调用之后不关闭数据库连接，由外面关闭
        /// <summary>
        /// 执行sql语句，返回受影响的行
        /// </summary>
        public int ExecuteNonQuerys(string sql, object param = null)
        {
            return ExecuteNonQuery(Command, sql, param);
        }
        /// <summary>
        /// 执行Select查询语句，返回第一行第一列
        /// </summary>
        public T ExecuteScalars<T>(string sql, object param = null)
        {
            return (T)ExecuteScalar(Command, sql, param);
        }
        //事务不用委托的方式实现，因为太不灵活了
        public void BeginTransaction()
        {
            Command.Transaction = Connection.BeginTransaction();
        }
        public void Commit()
        {
            Command.Transaction.Commit();
        }
        public void Rollback()
        {
            Command.Transaction.Rollback();
        }
        public List<dynamic> Querys(string sql, object param = null)
        {
            return Query(Command, sql, param);
        }
        public List<T> Querys<T>(string sql, object param = null)
        {
            return Query<T, TNull, TNull, TNull, TNull, TNull, TNull, T>(Command, sql, (t1, t2, t3, t4, t5, t6, t7) => { return t1; }, param);
        }
        public List<T1> Querys<T1, T2>(string sql, Action<T1, T2> func, object param = null)
        {
            return Query<T1, T2, TNull, TNull, TNull, TNull, TNull, T1>(Command, sql, (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2); return t1; }, param);
        }
        public List<T1> Querys<T1, T2, T3>(string sql, Action<T1, T2, T3> func, object param = null)
        {
            return Query<T1, T2, T3, TNull, TNull, TNull, TNull, T1>(Command, sql, (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2, t3); return t1; }, param);
        }
        public List<T1> Querys<T1, T2, T3, T4>(string sql, Action<T1, T2, T3, T4> func, object param = null)
        {
            return Query<T1, T2, T3, T4, TNull, TNull, TNull, T1>(Command, sql, (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2, t3, t4); return t1; }, param);
        }
        public List<T1> Querys<T1, T2, T3, T4, T5>(string sql, Action<T1, T2, T3, T4, T5> func, object param = null)
        {
            return Query<T1, T2, T3, T4, T5, TNull, TNull, T1>(Command, sql, (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2, t3, t4, t5); return t1; }, param);
        }
        public List<T1> Querys<T1, T2, T3, T4, T5, T6>(string sql, Action<T1, T2, T3, T4, T5, T6> func, object param = null)
        {
            return Query<T1, T2, T3, T4, T5, T6, TNull, T1>(Command, sql, (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2, t3, t4, t5, t6); return t1; }, param);
        }
        public List<T1> Querys<T1, T2, T3, T4, T5, T6, T7>(string sql, Action<T1, T2, T3, T4, T5, T6, T7> func, object param = null)
        {
            return Query<T1, T2, T3, T4, T5, T6, T7, T1>(Command, sql, (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2, t3, t4, t5, t6, t7); return t1; }, param);
        }
        #endregion

        #region 静态基函数，调用之后关闭数据库连接
        /// <summary>
        /// 执行sql语句，返回受影响的行
        /// </summary>
        public static int ExecuteNonQuery(string sql, object param = null)
        {
            using (IDbConnection conn = CreateConnection())
            {
                return ExecuteNonQuery(conn.CreateCommand(), sql, param);
            }
        }
        /// <summary>
        /// 执行Select查询语句，返回第一行第一列
        /// </summary>
        public static T ExecuteScalar<T>(string sql, object param = null)
        {
            using (IDbConnection conn = CreateConnection())
            {
                return (T)ExecuteScalar(conn.CreateCommand(), sql, param);
            }
        }

        public static List<dynamic> Query(string sql, object param = null)
        {
            using (IDbConnection conn = CreateConnection())
            {
                return Query(conn.CreateCommand(), sql, param);
            }
        }
        public static List<T> Query<T>(string sql, object param = null)
        {
            using (IDbConnection conn = CreateConnection())
            {
                return Query<T, TNull, TNull, TNull, TNull, TNull, TNull, T>(conn.CreateCommand(), sql, (t1, t2, t3, t4, t5, t6, t7) => { return t1; }, param);
            }
        }
        public static List<T1> Query<T1, T2>(string sql, Action<T1, T2> func, object param = null)
        {
            using (IDbConnection conn = CreateConnection())
            {
                return Query<T1, T2, TNull, TNull, TNull, TNull, TNull, T1>(conn.CreateCommand(), sql, (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2); return t1; }, param);
            }
        }
        public static List<T1> Query<T1, T2, T3>(string sql, Action<T1, T2, T3> func, object param = null)
        {
            using (IDbConnection conn = CreateConnection())
            {
                return Query<T1, T2, T3, TNull, TNull, TNull, TNull, T1>(conn.CreateCommand(), sql, (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2, t3); return t1; }, param);
            }
        }
        public static List<T1> Query<T1, T2, T3, T4>(string sql, Action<T1, T2, T3, T4> func, object param = null)
        {
            using (IDbConnection conn = CreateConnection())
            {
                return Query<T1, T2, T3, T4, TNull, TNull, TNull, T1>(conn.CreateCommand(), sql, (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2, t3, t4); return t1; }, param);
            }
        }
        public static List<T1> Query<T1, T2, T3, T4, T5>(string sql, Action<T1, T2, T3, T4, T5> func, object param = null)
        {
            using (IDbConnection conn = CreateConnection())
            {
                return Query<T1, T2, T3, T4, T5, TNull, TNull, T1>(conn.CreateCommand(), sql, (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2, t3, t4, t5); return t1; }, param);
            }
        }
        public static List<T1> Query<T1, T2, T3, T4, T5, T6>(string sql, Action<T1, T2, T3, T4, T5, T6> func, object param = null)
        {
            using (IDbConnection conn = CreateConnection())
            {
                return Query<T1, T2, T3, T4, T5, T6, TNull, T1>(conn.CreateCommand(), sql, (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2, t3, t4, t5, t6); return t1; }, param);
            }
        }
        public static List<T1> Query<T1, T2, T3, T4, T5, T6, T7>(string sql, Action<T1, T2, T3, T4, T5, T6, T7> func, object param = null)
        {
            using (IDbConnection conn = CreateConnection())
            {
                return Query<T1, T2, T3, T4, T5, T6, T7, T1>(conn.CreateCommand(), sql, (t1, t2, t3, t4, t5, t6, t7) => { func(t1, t2, t3, t4, t5, t6, t7); return t1; }, param);
            }
        }
        #endregion
    }

    public partial class DB
    {
        //"Data Source=.;Initial Catalog=Wind;uid=sa;pwd=123456";   //用户名密码连接
        //"Data Source=.;Initial Catalog=Wind;Integrated Security=True";   //Windows身份验证，本地应用程序可用，但在 IIS中不可用，因为权限不够，可修改应用程序池进程模型标识提高权限
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnString;
        /// <summary>
        /// 创建 Connection，通过创建不同类型的实例来达到调用不同类型的数据库
        /// </summary>
        public static IDbConnection CreateConnection()
        {
            return new Microsoft.Data.SqlClient.SqlConnection(ConnString);
        }
        #region 最底层函数，不对外调用，实例基函数和静态基函数都调用最底层函数,不关闭数据库连接，由外层关闭，共用同一个数据库连接，传入Command 是因为Command中可以存在事务、Sql参数、数据库连接对象
        //执行sql语句，返回受影响的行
        internal static int ExecuteNonQuery(IDbCommand cmd, string sql, object param = null)
        {
            cmd.CommandText = sql;
            AddParameter(cmd, param);
            if (cmd.Connection.State == ConnectionState.Closed) { cmd.Connection.Open(); } else if (cmd.Connection.State == ConnectionState.Broken) { cmd.Connection.Close(); cmd.Connection.Open(); }
            int count = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();//防止多次使用同一个 Command且添加相同的参数名的
            return count;
        }
        //执行sql查询，返回第一行第一列
        internal static object ExecuteScalar(IDbCommand cmd, string sql, object param = null)
        {
            cmd.CommandText = sql;
            AddParameter(cmd, param);
            if (cmd.Connection.State == ConnectionState.Closed) { cmd.Connection.Open(); } else if (cmd.Connection.State == ConnectionState.Broken) { cmd.Connection.Close(); cmd.Connection.Open(); }
            object obj = cmd.ExecuteScalar();
            cmd.Parameters.Clear();//防止多次使用同一个 Command且添加相同的参数名的
            return obj;
        }
        //执行sql查询，返回 DataReader对象
        internal static IDataReader ExecuteReader(IDbCommand cmd, string sql, object param = null)
        {
            cmd.CommandText = sql;
            AddParameter(cmd, param);
            if (cmd.Connection.State == ConnectionState.Closed) { cmd.Connection.Open(); } else if (cmd.Connection.State == ConnectionState.Broken) { cmd.Connection.Close(); cmd.Connection.Open(); }
            IDataReader reader = cmd.ExecuteReader();
            cmd.Parameters.Clear();//防止多次使用同一个 Command且添加相同的参数名的
            return reader;
        }
        //添加Sql参数，isIgNull：是否属性值为 null的不参与构造，true 不参与构造，false 参与构造
        internal static void AddParameter(IDbCommand cmd, object model, bool isIgNull = false)
        {
            if (model == null) { return; }
            foreach (var p in model.GetType().GetProperties())
            {
                object val = p.GetValue(model);//获取属性值
                if (isIgNull && val == null) { continue; }
                var param = cmd.CreateParameter();
                //根据属性的类型设置Parameter 的DbType和size，只设置String类型的其他类型让其自动设置，如果不设置string类型每次size会根据value的长度自动设置，这样不会重用执行计划（网上说的没测试）
                if (val is String)
                {
                    param.Size = ((string)val).Length <= 4000 ? 4000 : -1;
                }
                //设置Parameter属性
                param.ParameterName = '@' + p.Name;
                param.Value = val ?? DBNull.Value;//value要在最后设置，不要在设置size之前设置
                cmd.Parameters.Add(param);
            }
        }
        #endregion

        #region DataReader->Model 查询且转实体类
        /*
        *性能测试结果：
        *                                反射               Expression或Emit      SqlDataReader手写填充实体类      SqlDataAdapter.Fill()      SqlDataReader填充DataTable
        *  调用1次查询113万条数据      7500毫秒              3500毫秒                  3500毫秒                       5100毫秒                  3500毫秒
        *  查询15条耗时比例            11                     8                                                                                   8
        *  
        * 结论：Expression或Emit生成IL 接近手写效率，查询大量数据 Expression或Emit 比反射快一倍
        *       查询小量数据（15条） Expression或Emit生成委托方式比反射快20%-30%，Expression或Emit生成委托方式，缓存一千个委托占 11Mb内存
        */

        #region 查询 DataReader->dynamic，此处没用 new ExpandoObject()，因为性能太低了，查询100万数据，耗时比为 7:3
        private class DbTable
        {
            private readonly Dictionary<string, int> columnLookup;
            internal int ColumnCount { get { return columnLookup.Count; } }
            internal string[] Columns { get { return columnLookup.Keys.ToArray(); } }

            internal DbTable(string[] columnNames)
            {
                columnLookup = new Dictionary<string, int>(columnNames.Length, StringComparer.Ordinal);
                //多个相同的列，已第一个为准
                for (int i = columnNames.Length - 1; i >= 0; i--)
                {
                    string key = columnNames[i];
                    if (key != null) { columnLookup[key] = i; }//相同的键第一次是添加第二次是修改
                }
            }

            internal int IndexOfName(string name)
            {
                return columnLookup.TryGetValue(name, out int result) ? result : -1;
            }

            internal int AddColumn(string name)
            {
                return columnLookup[name] = columnLookup.Count;
            }
        }
        sealed partial class DbRowMetaObject : DynamicMetaObject
        {
            static readonly MethodInfo getValueMethod = typeof(DbRow).GetMethod("GetValue", new Type[] { typeof(string) });
            static readonly MethodInfo setValueMethod = typeof(DbRow).GetMethod("SetValue", new Type[] { typeof(string), typeof(object) });

            public DbRowMetaObject(Expression expression, BindingRestrictions restrictions, object value) : base(expression, restrictions, value) { }

            DynamicMetaObject CallMethod(MethodInfo method, Expression[] parameters)
            {
                var callMethod = new DynamicMetaObject(
                    Expression.Call(
                        Expression.Convert(Expression, LimitType),
                        method,
                        parameters),
                    BindingRestrictions.GetTypeRestriction(Expression, LimitType)
                    );
                return callMethod;
            }

            //获取
            public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
            {
                var parameters = new Expression[]
                                     {
                                         Expression.Constant(binder.Name)
                                     };

                var callMethod = CallMethod(getValueMethod, parameters);
                return callMethod;
            }

            //调用方法
            //public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args) { throw new Exception("未实现此方法"); }

            //设置
            public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
            {
                var parameters = new Expression[]
                                     {
                                         Expression.Constant(binder.Name),
                                         value.Expression,
                                     };

                var callMethod = CallMethod(setValueMethod, parameters);
                return callMethod;
            }
        }

        private sealed class DbRow : IDynamicMetaObjectProvider
        {
            readonly DbTable table;
            object[] values;
            public DbRow(DbTable table, object[] values)
            {
                this.table = table;
                this.values = values;
            }
            public object SetValue(string key, object value)//此方法一定要有返回
            {
                var index = table.IndexOfName(key);
                if (index < 0)
                {
                    index = table.AddColumn(key);
                    Array.Resize(ref values, table.ColumnCount);//增加数组长度
                }
                return values[index] = value;
            }

            public object GetValue(string key)
            {
                var index = table.IndexOfName(key);
                if (index < 0)
                {
                    throw new Exception("Key不存在");
                }
                else if (index >= values.Length)
                {
                    return null;
                }
                return values[index];
            }

            public DynamicMetaObject GetMetaObject(Expression parameter)
            {
                return new DbRowMetaObject(parameter, System.Dynamic.BindingRestrictions.Empty, this);
            }
        }
        /// <summary>
        /// 查询数据，不对外开放，调用此方法需要外面关闭数据库连接
        /// </summary>
        internal static List<dynamic> Query(IDbCommand cmd, string sql, object param = null)
        {
            using (IDataReader reader = ExecuteReader(cmd, sql, param))
            {
                List<dynamic> list = new List<dynamic>();
                var table = new DbTable(Enumerable.Range(0, reader.FieldCount).Select(s => reader.GetName(s)).ToArray());
                while (reader.Read())
                {
                    object[] values = new object[reader.FieldCount];//不要放到外边
                    reader.GetValues(values);
                    list.Add(new DbRow(table, values));
                }
                return list;
            }
        }
        #endregion

        //多表联查用到，数据库列与实体类的属性对应关系
        #region 多表联查类的属性与数据库字段对应关系，算法：查询出来的列先和 T1找对应关系，剩余的列在和 T2找对应关系，在剩余的列和 T3找对应关系，以此类推
        //RelationModel 实体类和数据库列对应关系
        private class RelationModel
        {
            //属性
            public PropertyInfo Property { set; get; }
            //属性的类型，如果是可空类型，则这里就是基本类型
            public Type PropertyType { set; get; }
            //对应DataReader 列的索引
            public int DbIndex { set; get; }
            //DataReader 列的类型
            public Type DbType { set; get; }
        }
        //实体类和数据库列对应关系 List<RelationModel>[] 是一个二维数组，List[0]是T1与数据库字段的关系，List[1]是T2与数据库字段的关系，以此类推，List[0][0]T1的第一个字段与数据库字段的关系，List[0][1]T1的第二个字段与数据库字段的关系
        private static List<RelationModel>[] DbModelRelation(IDataReader reader, Type[] types)
        {
            var allCol = Enumerable.Range(0, reader.FieldCount).Select(s => reader.GetName(s)).ToList();
            List<RelationModel>[] list = new List<RelationModel>[types.Length];
            for (int i = 0; i < types.Length; i++)//遍历T1-T7的类
            {
                if (types[i] == typeof(TNull)) { break; }//如果遇到 TNull就退出循环（因为后面的一定也是 TNull）
                List<RelationModel> col = new List<RelationModel>();//当前类的匹配到的属性填充到此
                foreach (var m in types[i].GetProperties())//遍历属性
                {
                    int index = allCol.IndexOf(m.Name);
                    if (index != -1)//找到与数据库对应的字段
                    {
                        col.Add(new RelationModel()//对应关系填充到 List
                        {
                            Property = m,
                            //如果是 int?、bool? 这种可空类型就取基类型 int、bool
                            PropertyType = m.PropertyType.IsGenericType && m.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? m.PropertyType.GetGenericArguments()[0] : m.PropertyType,
                            DbIndex = index,
                            DbType = reader.GetFieldType(index)
                        });
                        allCol[index] = null;//该数据库字段已经与当前属性有了对应关系，不能在与其他类的属性产生对应关系，所以设为null，此处不能用 Remove()，不然索引就对不上
                    }
                }
                list[i] = col;
            }
            return list;
        }
        #endregion

        #region 查询 DataReader->Model（反射方式）
        private static T GetModel<T>(IDataReader reader, List<RelationModel> relationList)
        {
            if (relationList == null) { return default(T); }
            T t = (T)Activator.CreateInstance(typeof(T));//创建实体类对象
            foreach (var r in relationList)//给实体类的每个属性赋值
            {
                if (!reader.IsDBNull(r.DbIndex))
                {
                    object val = reader.GetValue(r.DbIndex);
                    r.Property.SetValue(t, r.PropertyType != r.DbType ? Convert.ChangeType(val, r.PropertyType) : val);
                }
            }
            return t;
        }
        /// <summary>
        /// 查询数据，反射方式把数据填充到List，不对外开放，调用此方法需要外面关闭数据库连接
        /// </summary>
        internal static List<TReturn> Query1<T1, T2, T3, T4, T5, T6, T7, TReturn>(IDbCommand cmd, string sql, Func<T1, T2, T3, T4, T5, T6, T7, TReturn> func, object param = null)
        {
            using (IDataReader reader = ExecuteReader(cmd, sql, param))
            {
                List<RelationModel>[] relationList = DbModelRelation(reader, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7) });//获取实体类属性与数据库字段对应关系
                List<TReturn> list = new List<TReturn>();
                T1 t1; T2 t2; T3 t3; T4 t4; T5 t5; T6 t6; T7 t7;//临时变量，放在 while外面
                while (reader.Read())
                {
                    t1 = GetModel<T1>(reader, relationList[0]);
                    t2 = GetModel<T2>(reader, relationList[1]);
                    t3 = GetModel<T3>(reader, relationList[2]);
                    t4 = GetModel<T4>(reader, relationList[3]);
                    t5 = GetModel<T5>(reader, relationList[4]);
                    t6 = GetModel<T6>(reader, relationList[5]);
                    t7 = GetModel<T7>(reader, relationList[6]);

                    list.Add(func(t1, t2, t3, t4, t5, t6, t7));//指定实体类之间的关系
                }
                return list;
            }
        }
        #endregion

        #region 查询 DataReader->Model（表达式树方式）
        private static readonly ConcurrentDictionary<Identity, Action<IDataReader, TModels>> FuncCache = new ConcurrentDictionary<Identity, Action<IDataReader, TModels>>();
        /// <summary>
        /// 查询数据，Expression 生成委托方式填充数据到List，不对外开放，调用此方法需要外面关闭数据库连接
        /// </summary>
        internal static List<TReturn> Query<T1, T2, T3, T4, T5, T6, T7, TReturn>(IDbCommand cmd, string sql, Func<T1, T2, T3, T4, T5, T6, T7, TReturn> func, object param = null)
        {
            using (IDataReader reader = ExecuteReader(cmd, sql, param))
            {
                Type[] types = new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7) };
                //构造 Identity
                StringBuilder cols = new StringBuilder();
                for (int i = 0; i < reader.FieldCount; i++) { cols.AppendLine(reader.GetName(i)); }
                Identity identity = new Identity(types, cols.ToString());
                //获取委托
                Action<IDataReader, TModels> getModelFunc;
                if (!FuncCache.TryGetValue(identity, out getModelFunc))
                {
                    getModelFunc = GetFunc(reader, types);
                    FuncCache[identity] = getModelFunc;
                }
                //DataReader 读取数据
                List<TReturn> list = new List<TReturn>();
                TModels models = new TModels();//临时变量
                while (reader.Read())
                {
                    getModelFunc(reader, models);//获取数据并填充到 TModels
                    list.Add(func((T1)models.T1, (T2)models.T2, (T3)models.T3, (T4)models.T4, (T5)models.T5, (T6)models.T6, (T7)models.T7));//指定类与类之间的关系
                }
                return list;//不要用yield return，用 yield 每次foreach 都会查一遍数据库
            }
        }

        //因为 out T1 t1 在表达式树中没找到对应的用法，Action<>  在表达式树中也没找到对应调用方式，所以才用这种方式
        private class TModels
        {
            public object T1 { set; get; }
            public object T2 { set; get; }
            public object T3 { set; get; }
            public object T4 { set; get; }
            public object T5 { set; get; }
            public object T6 { set; get; }
            public object T7 { set; get; }
        }
        //表达式树生成映射委托
        private static Action<IDataReader, TModels> GetFunc(IDataReader reader, Type[] types)
        {
            List<RelationModel>[] relationList = DbModelRelation(reader, types);//获取关系
            List<Expression> block = new List<Expression>();//表达式块的集合
            //声明方法参数
            var rea = Expression.Parameter(typeof(IDataRecord));// IDataRecord 是 IDataReader 的父类，此处必须是IDataRecord，否则会报错
            var tModels = Expression.Parameter(typeof(TModels));
            //变量列表
            List<ParameterExpression> variableList = new List<ParameterExpression>();
            for (int i = 0; i < relationList.Length; i++)
            {
                if (relationList[i] == null) { break; }
                var model = Expression.Variable(types[i]);//var model; 声明实体类变量
                block.Add(Expression.Assign(model, Expression.New(types[i]))); //model=new Model();实体类赋值
                variableList.Add(model);//添加到变量列表
                //给实体类的属性赋值
                foreach (var r in relationList[i])
                {
                    block.Add(Expression.IfThen(    //if 条件判断
                        Expression.Not(Expression.Call(rea, "IsDBNull", null, Expression.Constant(r.DbIndex, typeof(int)))),//if(!reader.IsDBNull(i))    //if中的条件
                        Expression.Assign(  //属性赋值
                            Expression.Property(model, r.Property),
                            Expression.Convert(
                                r.PropertyType == r.DbType ? //三元运算符判断
                                Expression.Call(rea, "GetValue", null, Expression.Constant(r.DbIndex, typeof(int))) :
                                Expression.Call(    //Convert.ChangeType转换
                                    typeof(Convert),
                                    "ChangeType",
                                    null,
                                    Expression.Call(rea, "GetValue", null, Expression.Constant(r.DbIndex, typeof(int))),//取值
                                    Expression.Constant(r.PropertyType, typeof(Type))  //此处是proType,因为此处不能是可空类型
                                )
                            , r.Property.PropertyType)
                        )
                    ));
                }
                block.Add(Expression.Assign(Expression.Property(tModels, "T" + (i + 1)), model)); //赋值给 TModels的属性
            }
            //创建返回表达式（return model）
            //var returnLabel = Expression.Label(type);
            //block.Add(Expression.Return(returnLabel, model));
            //block.Add(Expression.Label(returnLabel, Expression.Default(type)));
            var body = Expression.Block(
                    variableList,// 变量添加到块中
                    block
                );
            return Expression.Lambda<Action<IDataReader, TModels>>(body, rea, tModels).Compile();
        }
        //Type[]不存（节省内存空间，一开始就有获取 HashCode），sql 只存列名（防止条件不同而存多个委托）
        private class Identity : IEquatable<Identity>
        {
            public string Cols { set; get; }
            private int hashCode;
            public Identity(Type[] types, string cols)
            {
                this.Cols = cols;
                this.hashCode = cols.GetHashCode() * 17;//为避免偶然相等，所以乘以质数
                foreach (var m in types)
                {
                    if (m == typeof(TNull)) { break; }
                    this.hashCode = this.hashCode + m.GetHashCode() * 17;
                }
            }
            public override bool Equals(object obj)
            {
                return Equals(obj as Identity);
            }
            //此方法必须要有
            public bool Equals(Identity obj)
            {
                return this.Cols == obj.Cols;
            }
            //重新Equals 必须重写 GetHashCode()，会自动先判断哈希吗是否相等，如果相等才进入Equals判断
            public override int GetHashCode()
            {
                return hashCode;
            }
        }
        #endregion
        #endregion

    }
}
