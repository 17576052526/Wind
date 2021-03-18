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
    #region 查询构造器
    public class SqlBuilder<T>
    {
        private IDbCommand Command;
        private string Column;
        private string Table;
        private string _Where;
        private string Join;
        private string _OrderBy;
        private int SkipCount;//跳过多少条（分页用到）   ，此处不用 startIndex 起始索引，因为不同数据库的起始索引是不一样的
        private int TakeCount;//返回之后的多少条（分页用到）
        private bool IsClose;//执行sql之后是否关闭数据库连接，true：关闭，false：不关闭
        private List<Type> Types;
        internal SqlBuilder(IDbCommand cmd = null)
        {
            Types = new List<Type>() { typeof(T) };
            if (cmd == null)
            {
                this.Command = DB.CreateConnection().CreateCommand();
                this.IsClose = true;
            }
            else
            {
                this.Command = cmd;
            }
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
        public SqlBuilder<T> Where(string where)
        {
            this._Where = where;
            return this;
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
        public SqlBuilder<T> OrderBy(string orderByColumn)
        {
            if (!Regex.IsMatch(orderByColumn, @"^(\w+|\[\w+\]|\w+\.(\w+|\[\w+\]))(\s+(asc|desc))?$", RegexOptions.IgnoreCase)) { throw new Exception($"字符串”{orderByColumn}“存在sql注入风险"); }
            this._OrderBy += orderByColumn + ',';
            return this;
        }
        /// <summary>
        /// 查询第一行第一例
        /// </summary>
        public TReturn QueryScalar<TReturn>(object param = null)
        {
            if (this.IsClose)
            {
                try { return (TReturn)DB.ExecuteScalar(this.Command, this.ToString(), param); }
                finally { this.Command.Connection.Close(); }
            }
            return (TReturn)DB.ExecuteScalar(this.Command, this.ToString(), param);
        }
        /// <summary>
        /// 查询
        /// </summary>
        public List<T> Query(object param = null)
        {
            if (IsClose)
            {
                try { return DB.Query<T>(Command, this.ToString(), param, Types.ToArray()); }
                finally { Command.Connection.Close(); }
            }
            return DB.Query<T>(Command, this.ToString(), param, Types.ToArray());
        }
        /// <summary>
        /// 跳过 skipCount条，返回连续的 takeCount条
        /// </summary>
        /// <param name="skipCount">跳过的条数</param>
        /// <param name="takeCount">返回连续的条数</param>
        public List<T> Query(int skipCount, int takeCount, object param = null)
        {
            this.SkipCount = skipCount;
            this.TakeCount = takeCount;
            return Query(param);
        }
        /// <summary>
        /// 查询第一行
        /// </summary>
        public T QueryFirstRow(object param = null)
        {
            this.TakeCount = 1;
            return Query(param)[0];
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
                DB.AddParameter(Command, new { _start = SkipCount + 1, _end = SkipCount + TakeCount });//添加分页参数
                return $@"
with _tab as(
{str}
)
select * from _tab where _RowNum between @_start and @_end
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
        public int Inserts(IEnumerable<IDAL> list)
        {
            this.BeginTransaction();
            try
            {
                int count = 0;
                foreach (IDAL m in list)
                {
                    count += this.ExecuteNonQuerys(m.Insert(), m);
                }
                this.Commit();
                return count;
            }
            catch { this.Rollback(); return -1; }
        }
        /// <summary>
        /// 修改，不能将列值修改为 NULL值，如果要修改某列值为NULL值，请用 ExecuteNonQuery()
        /// </summary>
        public int Updates(IDAL model, string where, object param = null)
        {
            AddParameter(Command, model, true);
            return this.ExecuteNonQuerys(model.Update() + " where " + where, param);
        }
        public int Deletes<T>(string where, object param = null) where T : IDAL
        {
            return this.ExecuteNonQuerys($"delete from {typeof(T).Name} where {where}", param);
        }
        public SqlBuilder<T> Selects<T>(string column = "*") where T : IDAL
        {
            return new SqlBuilder<T>(Command).Select(column).From(typeof(T).Name);
        }
        #endregion

        #region 静态扩展方法，调用之后关闭数据库连接
        public static int Insert(IDAL model)
        {
            return ExecuteNonQuery(model.Insert(), model);
        }
        public static int Insert(IEnumerable<IDAL> list)
        {
            IDbConnection conn = CreateConnection();
            IDbCommand cmd = conn.CreateCommand();
            IDbTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                cmd.Transaction = tran;

                int count = 0;
                foreach (var m in list)
                {
                    count += ExecuteNonQuery(cmd, m.Insert(), m);
                }
                tran.Commit();
                return count;
            }
            catch
            {
                if (tran != null) { tran.Rollback(); }
                return -1;
            }
            finally { conn.Close(); }
        }
        /// <summary>
        /// 修改，不能将列值修改为 NULL值，如果要修改某列值为NULL值，请用 ExecuteNonQuery()
        /// </summary>
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

        public static SqlBuilder<T> Select<T>(string column = "*") where T : IDAL
        {
            return new SqlBuilder<T>().Select(column).From(typeof(T).Name);
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
            if (Connection.State == ConnectionState.Closed) { Connection.Open(); } else if (Connection.State == ConnectionState.Broken) { Connection.Close(); Connection.Open(); }
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
            return Query<T>(Command, sql, param, new Type[] { typeof(T) });
        }
        public List<T1> Querys<T1, T2>(string sql, object param = null)
        {
            return Query<T1>(Command, sql, param, new Type[] { typeof(T1), typeof(T2) });
        }
        public List<T1> Querys<T1, T2, T3>(string sql, object param = null)
        {
            return Query<T1>(Command, sql, param, new Type[] { typeof(T1), typeof(T2), typeof(T3) });
        }
        public List<T1> Querys<T1, T2, T3, T4>(string sql, object param = null)
        {
            return Query<T1>(Command, sql, param, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) });
        }
        public List<T1> Querys<T1, T2, T3, T4, T5>(string sql, object param = null)
        {
            return Query<T1>(Command, sql, param, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) });
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
                return Query<T>(conn.CreateCommand(), sql, param, new Type[] { typeof(T) });
            }
        }
        public static List<T1> Query<T1, T2>(string sql, object param = null)
        {
            using (IDbConnection conn = CreateConnection())
            {
                return Query<T1>(conn.CreateCommand(), sql, param, new Type[] { typeof(T1), typeof(T2) });
            }
        }
        public static List<T1> Query<T1, T2, T3>(string sql, object param = null)
        {
            using (IDbConnection conn = CreateConnection())
            {
                return Query<T1>(conn.CreateCommand(), sql, param, new Type[] { typeof(T1), typeof(T2), typeof(T3) });
            }
        }
        public static List<T1> Query<T1, T2, T3, T4>(string sql, object param = null)
        {
            using (IDbConnection conn = CreateConnection())
            {
                return Query<T1>(conn.CreateCommand(), sql, param, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) });
            }
        }
        public static List<T1> Query<T1, T2, T3, T4, T5>(string sql, object param = null)
        {
            using (IDbConnection conn = CreateConnection())
            {
                return Query<T1>(conn.CreateCommand(), sql, param, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) });
            }
        }

        #endregion
    }
    //调用存储过程 用exec
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
        internal static IDbConnection CreateConnection()
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

        /*
        *查询性能测试结果：
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
                var callMethod = new DynamicMetaObject(//此处报错，赋值时需要强转为object类型
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
                }
                if (index >= values.Length)
                {
                    Array.Resize(ref values, table.ColumnCount);//增加数组长度
                }
                return values[index] = value;
            }

            public object GetValue(string key)
            {
                var index = table.IndexOfName(key);
                if (index < 0)//无key则报错
                {
                    throw new Exception("Key不存在");
                }
                else if (index >= values.Length)//无值返回 null，例如：第一行新加了一列，第二行则没加
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

        #region 多表联查，实体类与属性的关系
        //实例所对应的属性
        private class ModelAttrRelation
        {
            //实例
            public int Case { set; get; }
            //赋值给第几个对象
            public int Obj { set; get; }
            //哪个属性
            public PropertyInfo Property { set; get; }
        }
        private static List<ModelAttrRelation> GetModelAttrRelations(Type[] types, Type returnType)
        {
            List<ModelAttrRelation> modelAttrRelation = new List<ModelAttrRelation>();
            for (int i = 0; i < types.Length; i++)
            {
                Type t = types[i];
                if (t == returnType) { continue; }
                bool b = true;
                //查找关系
                for (int j = 0; j < types.Length; j++)
                {
                    if (i == j) { continue; }
                    foreach (var m in types[j].GetProperties())
                    {
                        if (t == m.PropertyType && !modelAttrRelation.Any(s => s.Property == m))
                        {
                            modelAttrRelation.Add(new ModelAttrRelation() { Case = i, Obj = j, Property = m });
                            b = false;
                            j = types.Length;//退出外层循环
                            break;
                        }
                    }
                }
                if (b) { throw new Exception($"未找到与{t.Name}关联的属性"); }
            }
            return modelAttrRelation;
        }
        #endregion

        #region 查询 DataReader->Model（表达式树方式）
        private static readonly ConcurrentDictionary<Identity, Func<IDataReader, object>> FuncCache = new ConcurrentDictionary<Identity, Func<IDataReader, object>>();
        internal static List<T> Query<T>(IDbCommand cmd, string sql, object param, Type[] types)
        {
            using (IDataReader reader = ExecuteReader(cmd, sql, param))
            {
                //构造 Identity
                StringBuilder cols = new StringBuilder();
                for (int i = 0; i < reader.FieldCount; i++) { cols.AppendLine(reader.GetName(i)); }
                Identity identity = new Identity(types, cols.ToString());
                //获取委托
                Func<IDataReader, object> func;
                if (!FuncCache.TryGetValue(identity, out func))
                {
                    func = GetFunc<T>(reader, types);
                    FuncCache[identity] = func;
                }
                //DataReader 读取数据
                List<T> list = new List<T>();
                while (reader.Read())
                {
                    list.Add((T)func(reader));//指定类与类之间的关系
                }
                return list;//不要用yield return，用 yield 每次foreach 都会查一遍数据库
            }
        }
        //表达式树生成映射委托
        private static Func<IDataReader, object> GetFunc<T>(IDataReader reader, Type[] types)
        {
            List<RelationModel>[] relationList = DbModelRelation(reader, types);//获取关系
            List<Expression> block = new List<Expression>();//表达式块的集合
            //声明方法参数
            var rea = Expression.Parameter(typeof(IDataRecord));// IDataRecord 是 IDataReader 的父类，此处必须是IDataRecord，否则会报错
            //变量列表
            ParameterExpression[] variableList = new ParameterExpression[types.Length];
            for (int i = 0; i < relationList.Length; i++)
            {
                var model = Expression.Variable(types[i]);//var model; 声明实体类变量
                block.Add(Expression.Assign(model, Expression.New(types[i]))); //model=new Model();实体类赋值
                variableList[i] = model;//添加到变量列表
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
            }
            //实例赋值给别的类的属性
            List<ModelAttrRelation> modelAttrList = GetModelAttrRelations(types, typeof(T));//多表联查实体类与属性的关系
            foreach (var m in modelAttrList)
            {
                block.Add(Expression.Assign(Expression.Property(variableList[m.Obj], m.Property), variableList[m.Case]));
            }
            //创建返回表达式（return model）
            var returnLabel = Expression.Label(typeof(T));
            block.Add(Expression.Return(returnLabel, variableList[Array.IndexOf(types, typeof(T))]));
            block.Add(Expression.Label(returnLabel, Expression.Default(typeof(T))));

            var body = Expression.Block(
                    variableList,// 变量添加到块中
                    block
                );
            return Expression.Lambda<Func<IDataReader, object>>(body, rea).Compile();
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

    }
}