using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Collections.Concurrent;

/// <summary>
/// 数据库辅助类
/// </summary>
public sealed class DbHelper
{
    //"Data Source=.;Initial Catalog=Wind;uid=sa;pwd=123456";   //用户名密码连接
    //"Data Source=.;Initial Catalog=Wind;Integrated Security=True";   //Windows身份验证，本地应用程序可用，但在 IIS中不可用，因为权限不够，可修改应用程序池进程模型标识提高权限
    //System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ToString(); //获取配置文件中的连接字符串，需要添加引用
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
    /// <summary>
    /// 执行sql语句，返回受影响的行，没有事务
    /// </summary>
    public static int ExecuteNonQuery(string sql, object param = null, CommandType type = CommandType.Text)
    {
        IDbConnection conn = CreateConnection();
        IDbCommand comm = conn.CreateCommand();
        comm.CommandText = sql;
        comm.CommandType = type;
        if (param != null) { ObjectToParam(comm, param); }
        try
        {
            conn.Open();
            return comm.ExecuteNonQuery();
        }
        finally { conn.Close(); }
    }

    /// <summary>
    /// 执行Select查询语句，返回第一行第一列
    /// </summary>
    public static object ExecuteScalar(string sql, object param = null, CommandType type = CommandType.Text)
    {
        IDbConnection conn = CreateConnection();
        IDbCommand comm = conn.CreateCommand();
        comm.CommandText = sql;
        comm.CommandType = type;
        if (param != null) { ObjectToParam(comm, param); }
        try
        {
            conn.Open();
            return comm.ExecuteScalar();
        }
        finally { conn.Close(); }
    }

    /// <summary>
    /// 执行事务，场合一：一条sql 多条param，场合二：多条sql，param=null
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    public static void ExecuteTran(string sql, IEnumerable<object> param = null)
    {
        ExecuteTran((IDbCommand comm) =>
        {
            comm.CommandText = sql;
            if (param != null)
            {
                foreach (object m in param)
                {
                    ObjectToParam(comm, m);//此处不用判断空
                    comm.ExecuteNonQuery();
                    comm.Parameters.Clear();//如果不清除添加相同的变量会出错
                }
            }
            else
            {
                comm.ExecuteNonQuery();
            }
            return true;
        });
    }
    /// <summary>
    /// 执行事务
    /// </summary>
    /// <param name="fun">要执行的方法，返回true 提交事务，false或异常 回滚事务</param>
    public static void ExecuteTran(Func<IDbCommand, bool> fun)
    {
        IDbConnection conn = CreateConnection();
        IDbCommand comm = conn.CreateCommand();
        try
        {
            conn.Open();
            comm.Transaction = conn.BeginTransaction();
            if (fun(comm))
            {
                comm.Transaction.Commit();
            }
            else
            {
                comm.Transaction.Rollback();
            }
        }
        catch
        {
            if (comm.Transaction != null) { comm.Transaction.Rollback(); }
            throw;
        }
        finally
        {
            conn.Close();
        }
    }

    /*
     *性能测试结果：
     *                                反射               Expression或Emit      SqlDataReader手写填充实体类      SqlDataAdapter.Fill()      SqlDataReader填充DataTable
     *  调用1次查询113万条数据      7500毫秒              3500毫秒                  3500毫秒                       5100毫秒                  3500毫秒
     *  查询15条耗时比例            11                     8                                                                                   8
     *  
     * 结论：Expression或Emit生成IL 接近手写效率，查询大量数据 Expression或Emit 比反射快一倍
     *       查询小量数据（15条） Expression或Emit生成委托方式比反射快20%-30%，Expression或Emit生成委托方式，缓存一千个委托占 11Mb内存
     */
    /// <summary>
    /// 查询数据，以 DataTable返回查询结果
    /// </summary>
    public static DataTable Select(string sql, object param = null, CommandType type = CommandType.Text)
    {
        IDbConnection conn = CreateConnection();
        IDataReader reader = null;
        IDbCommand comm = conn.CreateCommand();
        comm.CommandText = sql;
        comm.CommandType = type;
        if (param != null) { ObjectToParam(comm, param); }
        try
        {
            conn.Open();
            reader = comm.ExecuteReader();
            //----------------开始填充----------------//
            DataTable dt = new DataTable();
            int fieldCount = reader.FieldCount;
            for (int i = 0; i < fieldCount; i++)
            {
                dt.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
            }
            dt.BeginLoadData();

            object[] values = new object[fieldCount];
            var dr = dt.Rows;
            while (reader.Read())
            {
                reader.GetValues(values);
                dr.Add(values);
            }
            return dt;
        }
        finally
        {
            if (reader != null) { reader.Close(); }
            conn.Close();
        }
    }
    /// <summary>
    /// 查询数据，反射方式把数据填充到List
    /// </summary>
    public static List<T> Query<T>(string sql, object param = null, CommandType type = CommandType.Text) where T : new()
    {
        IDbConnection conn = CreateConnection();
        IDataReader reader = null;
        IDbCommand comm = conn.CreateCommand();
        comm.CommandText = sql;
        comm.CommandType = type;
        if (param != null) { ObjectToParam(comm, param); }
        try
        {
            conn.Open();
            reader = comm.ExecuteReader();
            //----------------开始映射----------------//
            int colCount = reader.FieldCount;
            Type[] tColType = new Type[colCount];  //实体T的属性的类型，如果是可空类型这里就是基类型 例如 实际是int? ，这里是int
            PropertyInfo[] tColPro = new PropertyInfo[colCount];   //实体T的属性，与 reader的列对应
            Type[] readerCol = new Type[colCount]; //reader 列的类型

            List<T> list = new List<T>();
            Type tType = typeof(T);
            //反射前的初始化操作
            for (int i = 0; i < colCount; i++)
            {
                string name = reader.GetName(i);
                PropertyInfo p = tType.GetProperty(name);
                if (p != null)
                {
                    tColType[i] = p.PropertyType.Name == "Nullable`1" ? p.PropertyType.GetGenericArguments()[0] : p.PropertyType;  //如果是可空类型就返回基类型，例如：int? 的基础数据类型为int
                    tColPro[i] = p;
                    readerCol[i] = reader.GetFieldType(i);
                }
            }
            //reader 反射到对象
            while (reader.Read())
            {
                T t = new T();
                for (int i = 0; i < colCount; i++)
                {
                    if (tColPro[i] != null && !reader.IsDBNull(i))
                    {
                        object val = reader.GetValue(i);//取reader的值
                        tColPro[i].SetValue(t, readerCol[i] != tColType[i] ? Convert.ChangeType(val, tColType[i]) : val, null);//给对象的属性赋值，此处最耗时
                    }
                }
                list.Add(t);
            }
            return list;//不要用yield return，用 yield 每次foreach 都会查一遍数据库
        }
        finally
        {
            if (reader != null) { reader.Close(); }
            conn.Close();
        }
    }
    //model的属性转换成 DbParameter 数组,并填充到Command.Parameter
    private static void ObjectToParam(IDbCommand comm, object model)
    {
        foreach (var p in model.GetType().GetProperties())
        {
            var param = comm.CreateParameter();
            object val = p.GetValue(model, null);//获取属性值
                                                 //根据属性的类型设置Parameter 的DbType和size，只设置String类型的其他类型让其自动设置，如果不设置string类型每次size会根据value的长度自动设置，这样不会重用执行计划（网上说的没测试）
            if (p.PropertyType == typeof(String))
            {
                param.Size = val == null || ((string)val).Length <= 4000 ? 4000 : -1;
            }
            //设置Parameter属性
            param.ParameterName = '@' + p.Name;
            param.Value = val != null ? val : DBNull.Value;//value要在最后设置，不要在设置size之前设置
            comm.Parameters.Add(param);
        }
    }
    private static readonly ConcurrentDictionary<Identity, Func<IDataReader, object>> FuncCache = new ConcurrentDictionary<Identity, Func<IDataReader, object>>();
    /// <summary>
    /// 查询数据，Expression 生成委托方式填充数据到List
    /// </summary>
    public static List<T> Select<T>(string sql, object param = null, CommandType type = CommandType.Text)
    {
        IDbConnection conn = CreateConnection();
        IDataReader reader = null;
        IDbCommand comm = conn.CreateCommand();
        comm.CommandText = sql;
        comm.CommandType = type;
        if (param != null) { ObjectToParam(comm, param); }
        try
        {
            conn.Open();
            reader = comm.ExecuteReader();
            //----------------开始映射----------------//
            //构造 Identity
            StringBuilder cols = new StringBuilder();
            for (int i = 0; i < reader.FieldCount; i++) { cols.AppendLine(reader.GetName(i)); }
            Identity identity = new Identity(typeof(T), cols.ToString());
            //获取委托
            Func<IDataReader, object> func;
            if (!FuncCache.TryGetValue(identity, out func))
            {
                func = GetFunc(reader, typeof(T));
                FuncCache[identity] = func;
            }
            //DataReader 读取数据
            List<T> list = new List<T>();
            while (reader.Read())
            {
                list.Add((T)func(reader));
            }
            return list;//不要用yield return，用 yield 每次foreach 都会查一遍数据库
        }
        finally
        {
            if (reader != null) { reader.Close(); }
            conn.Close();
        }
    }
    //表达式树生成映射委托
    private static Func<IDataReader, object> GetFunc(IDataReader reader, Type type)
    {
        string[] fields = Enumerable.Range(0, reader.FieldCount).Select(s => reader.GetName(s)).ToArray();//获取所有列名
        List<Expression> block = new List<Expression>();//表达式块的集合
                                                        //reader 参数
        var rea = Expression.Parameter(typeof(IDataRecord), "reader");// IDataRecord 是 IDataReader 的父类，此处必须是IDataRecord，否则会报错
                                                                      //实体类变量
        var model = Expression.Variable(type, "model");
        block.Add(Expression.Assign(model, Expression.New(type))); //变量赋值
                                                                   //用于返回值
        var returnLabel = Expression.Label(type);

        for (int i = 0; i < fields.Length; i++)
        {
            var pro = type.GetProperty(fields[i]);
            if (pro != null)
            {
                Type proType = pro.PropertyType.IsGenericType ? pro.PropertyType.GetGenericArguments()[0] : pro.PropertyType;
                var property = Expression.Property(model, pro);//获取属性
                block.Add(Expression.IfThen(
                    Expression.Not(Expression.Call(rea, "IsDBNull", null, Expression.Constant(i, typeof(int)))),
                    Expression.Assign(property, Expression.Convert(
                    (
                        //三元运算符判断 类型是否一致
                        proType == reader.GetFieldType(i) ?
                            Expression.Call(rea, "GetValue", null, Expression.Constant(i, typeof(int)))
                        :
                            //类型不一致时，先reader取值,通过 Convert.ChangeType转换，Convert.ChangeType返回的是object类型，然后在转换一次
                            Expression.Call(    //Convert.ChangeType转换
                               typeof(Convert),
                                "ChangeType",
                                null,
                                Expression.Call(rea, "GetValue", null, Expression.Constant(i, typeof(int))),//取值
                                Expression.Constant(proType, typeof(Type))  //此处是proType,因为此处不能是可空类型
                            )
                    ), pro.PropertyType))
                ));
            }
        }
        block.Add(Expression.Return(returnLabel, model));
        //创建返回表达式的目标Label
        block.Add(Expression.Label(returnLabel, Expression.Default(type)));
        var body = Expression.Block(
                new[] { model },// 变量添加到块中
                block
            );

        return Expression.Lambda<Func<IDataReader, object>>(body, rea).Compile();
    }
}
internal class Identity : IEquatable<Identity>
{
    public Identity(Type Type, string Cols)
    {
        this.Type = Type;
        this.Cols = Cols;
        this.hashCode = Type.GetHashCode() * 17 + Cols.GetHashCode() * 17;//为避免偶然相等，所以乘以质数
    }
    public Type Type { set; get; }
    public string Cols { set; get; }
    private int hashCode;
    public override bool Equals(object obj)
    {
        return Equals(obj as Identity);
    }
    //此方法必须要有
    public bool Equals(Identity obj)
    {
        return this.Type == obj.Type && this.Cols == obj.Cols;
    }
    //重新Equals 必须重写 GetHashCode()，会自动先判断哈希吗是否相等，如果相等才进入Equals判断
    public override int GetHashCode()
    {
        return hashCode;
    }
}