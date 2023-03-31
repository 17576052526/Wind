using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DbOrm
{
    /*
     * 访问数据库的入口
     * 此文件代码由用户实现（1.由用户决定访问的是哪种类型的数据库，2.一个项目可能会同时用到多个数据库）
     * 如果一个项目有同时用到多个数据库，可另外在写一个类，继承 DBExpand或 DBBase类，并实现 CreateConnection()
     */
    public class DB : DBExpand
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnString;    //"Data Source=.;Initial Catalog=Wind;uid=sa;pwd=As727863984;TrustServerCertificate=True;"      //TrustServerCertificate=True; 解决 “证书链由不受信任的颁发机构颁发”错误
        //创建数据库连接对象，通过创建不同类型的实例来达到调用不同类型的数据库
        public override IDbConnection CreateConnection()
        {
            return new Microsoft.Data.SqlClient.SqlConnection(ConnString);
        }


        /*-------------------注意：NewThis()创建的类型是否是对的，以下静态方法只是为了方便调用而写的，以下代码可有可无-------------------*/

        public static DBExpand NewThis()
        {
            return new DB();
        }

        #region 对应 DBExpand 静态方法
        public static int Inserts(IModel model)
        {
            using (var db = NewThis())
            {
                return db.Insert(model);
            }
        }
        public static int Inserts(IEnumerable<IModel> list)
        {
            using (var db = NewThis())
            {
                return db.Insert(list);
            }
        }
        public static int Updates(IModel model, string where, object param = null)
        {
            using (var db = NewThis())
            {
                return db.Update(model, where, param);
            }
        }
        public static int Deletes<T>(string where, object param = null) where T : IModel
        {
            using (var db = NewThis())
            {
                return db.Delete<T>(where, param);
            }
        }
        public static SqlBuilder<T> Selects<T>(string column = "*") where T : IModel
        {
            //此处不用using，此方法返回的是SqlBuilder，外面要调用数据库查询，所以此方法不能关闭数据库连接
            DBBase db = NewThis();
            return new SqlBuilder<T>(db.Connection, null, true).Select(column).From(typeof(T).Name);
        }
        #endregion

        #region 对应 DBBase 静态方法
        public static int ExecuteNonQuerys(string sql, object param = null)
        {
            using (var db = NewThis())
            {
                return db.ExecuteNonQuery(sql, param);
            }
        }
        public static object ExecuteScalars(string sql, object param = null)
        {
            using (var db = NewThis())
            {
                return db.ExecuteScalar(sql, param);
            }
        }
        public static List<dynamic> Querys(string sql, object param = null)
        {
            using (var db = NewThis())
            {
                return db.Query(sql, param);
            }
        }
        public static List<T> Querys<T>(string sql, object param = null)
        {
            using (var db = NewThis())
            {
                return db.Query<T>(sql, param);
            }
        }
        public static List<T1> Querys<T1, T2>(string sql, object param = null)
        {
            using (var db = NewThis())
            {
                return db.Query<T1, T2>(sql, param);
            }
        }
        public static List<T1> Querys<T1, T2, T3>(string sql, object param = null)
        {
            using (var db = NewThis())
            {
                return db.Query<T1, T2, T3>(sql, param);
            }
        }
        public static List<T1> Querys<T1, T2, T3, T4>(string sql, object param = null)
        {
            using (var db = NewThis())
            {
                return db.Query<T1, T2, T3, T4>(sql, param);
            }
        }
        public static List<T1> Querys<T1, T2, T3, T4, T5>(string sql, object param = null)
        {
            using (var db = NewThis())
            {
                return db.Query<T1, T2, T3, T4, T5>(sql, param);
            }
        }
        #endregion
    }
}
