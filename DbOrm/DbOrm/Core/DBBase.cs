using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbOrm
{
    /*
     * IDbConnection 的实例管理
     * ConnectionExpand类的上一层
     * 子类继承当前类，并实现 CreateConnection()，即可访问数据库
     */
    public abstract class DBBase : IDisposable
    {
        //连接对象
        protected IDbConnection Connection;
        protected IDbTransaction Transaction;
        //初始化
        public DBBase()
        {
            Connection = CreateConnection();
            Connection.Open();
        }
        //释放对象
        public void Dispose()
        {
            Connection.Close();
        }
        /// <summary>
        /// 创建数据库连接对象，由继承的子类实现
        /// </summary>
        public abstract IDbConnection CreateConnection();
        /// <summary>
        /// 执行sql语句，返回受影响的行
        /// </summary>
        public int ExecuteNonQuery(string sql, object param = null)
        {
            return Connection.ExecuteNonQuery(sql, param, Transaction);
        }
        /// <summary>
        /// 查询第一行第一列
        /// </summary>
        public object ExecuteScalar(string sql, object param = null)
        {
            return Connection.ExecuteScalar(sql, param, Transaction);
        }
        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            Transaction = Connection.BeginTransaction();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            Transaction.Commit();
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTransaction()
        {
            Transaction.Rollback();
        }
        /// <summary>
        /// 查询
        /// </summary>
        public List<dynamic> Query(string sql, object param = null)
        {
            return Connection.Query(sql, param, Transaction);
        }
        /// <summary>
        /// 查询
        /// </summary>
        public List<T> Query<T>(string sql, object param = null)
        {
            return Connection.Query<T>(sql, param, new Type[] { typeof(T) }, Transaction);
        }
        /// <summary>
        /// 多表联查
        /// </summary>
        public List<T1> Query<T1, T2>(string sql, object param = null)
        {
            return Connection.Query<T1>(sql, param, new Type[] { typeof(T1), typeof(T2) }, Transaction);
        }
        /// <summary>
        /// 多表联查
        /// </summary>
        public List<T1> Query<T1, T2, T3>(string sql, object param = null)
        {
            return Connection.Query<T1>(sql, param, new Type[] { typeof(T1), typeof(T2), typeof(T3) }, Transaction);
        }
        /// <summary>
        /// 多表联查
        /// </summary>
        public List<T1> Query<T1, T2, T3, T4>(string sql, object param = null)
        {
            return Connection.Query<T1>(sql, param, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, Transaction);
        }
        /// <summary>
        /// 多表联查
        /// </summary>
        public List<T1> Query<T1, T2, T3, T4, T5>(string sql, object param = null)
        {
            return Connection.Query<T1>(sql, param, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) }, Transaction);
        }
    }
}
