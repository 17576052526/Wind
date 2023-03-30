using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DbOrm
{
    /*
     * 最底层代码，Connection的扩展
     * 1.Connection 实例上扩展方法，扩展的方法跟着 Connection的实例走，这样能实现支持多种不同类型的数据库，和同时支持多数据库（一个项目同时用到A、B两个数据库）
     * 2.查询也可能会有事务
     * 3.conn.Open() 没写在里面，在外面控制，因为事务是在外面创建的，而创建事务前要先打开数据库连接
     * 4.存储过程用 exec
     */
    public static partial class ConnectionExpand
    {
        /// <summary>
        /// 执行sql语句，返回受影响的行
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(this IDbConnection conn, string sql, object param = null, IDbTransaction transaction = null)
        {
            return conn.CreateCommand(sql, param, transaction).ExecuteNonQuery();
        }

        /// <summary>
        /// 执行sql查询，返回第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object ExecuteScalar(this IDbConnection conn, string sql, object param = null, IDbTransaction transaction = null)
        {
            return conn.CreateCommand(sql, param, transaction).ExecuteScalar();
        }
        /// <summary>
        /// 获取 IDataReader 数据读取对象
        /// </summary>
        public static IDataReader ExecuteReader(this IDbConnection conn, string sql, object param = null, IDbTransaction transaction = null)
        {
            return conn.CreateCommand(sql, param, transaction).ExecuteReader();
        }
        /// <summary>
        /// 创建 command对象
        /// </summary>
        public static IDbCommand CreateCommand(this IDbConnection conn, string sql, object param, IDbTransaction transaction)
        {
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            if (param != null) { cmd.AddParameter(param); }
            if (transaction != null) { cmd.Transaction = transaction; }
            return cmd;
        }
        /// <summary>
        /// 添加Sql参数
        /// </summary>
        /// <param name="param">实体类或 实体类集合</param>
        public static void AddParameter(this IDbCommand cmd, object param)
        {
            var list = param as IEnumerable<object>;
            if (list == null)
            {
                //实体类
                foreach (var p in param.GetType().GetProperties())
                {
                    cmd.AddParameter('@' + p.Name, p.GetValue(param));
                }
            }
            else
            {
                var cmdParam = cmd.Parameters;
                //实体类数组
                foreach (var m in list)
                {
                    foreach (var p in m.GetType().GetProperties())
                    {
                        string name = '@' + p.Name;
                        //实体类数组可能会有重复的参数，存在重复的则以后面的为准
                        if (cmdParam.Contains(name))
                        {
                            cmdParam.RemoveAt(name);
                        }
                        cmd.AddParameter(name, p.GetValue(m));
                    }
                }
            }
        }
        /// <summary>
        /// 添加sql参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void AddParameter(this IDbCommand cmd, string name, object value)
        {
            var param = cmd.CreateParameter();
            //根据属性的类型设置Parameter 的DbType和size，只设置String类型的其他类型让其自动设置，如果不设置string类型每次size会根据value的长度自动设置，这样不会重用执行计划（网上说的没测试）
            if (value is String)
            {
                param.Size = ((string)value).Length <= 4000 ? 4000 : -1;
            }
            //设置Parameter属性
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;//value要在最后设置，不要在设置size之前设置
            cmd.Parameters.Add(param);
        }

    }

    /*
     * 查询性能测试结果：
     *                                反射               Expression或Emit      SqlDataReader手写填充实体类      SqlDataAdapter.Fill()      SqlDataReader填充DataTable
     *  调用1次查询113万条数据      7500毫秒              3500毫秒                  3500毫秒                       5100毫秒                  3500毫秒
     *  查询15条耗时比例            11                     8                                                                                   8
     *  
     * 结论：Expression或Emit生成IL 接近手写效率，查询大量数据 Expression或Emit 比反射快一倍
     *       查询小量数据（15条） Expression或Emit生成委托方式比反射快20%-30%，Expression或Emit生成委托方式，缓存一千个委托占 11Mb内存
     */

    public static partial class ConnectionExpand
    {
        /// <summary>
        /// 查询 DataReader->dynamic，此处没用 new ExpandoObject()，因为性能太低了，查询100万数据，耗时比为 7:3
        /// </summary>
        public static List<dynamic> Query(this IDbConnection conn, string sql, object param = null, IDbTransaction transaction = null)
        {
            using (IDataReader reader = conn.ExecuteReader(sql, param, transaction))
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
    }
    #region 查询 DataReader->dynamic 相关
    public class DbTable
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

    public sealed class DbRow : IDynamicMetaObjectProvider
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
    #endregion

    public static partial class ConnectionExpand
    {
        /// <summary>
        /// 查询 DataReader->Model（表达式树方式）
        /// </summary>
        public static List<T> Query<T>(this IDbConnection conn, string sql, object param, Type[] types, IDbTransaction transaction = null)
        {
            using (IDataReader reader = conn.ExecuteReader(sql, param, transaction))
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

        //查询 DataReader->Model（表达式树方式）相关
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
