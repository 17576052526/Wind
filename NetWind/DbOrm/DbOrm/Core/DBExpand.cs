using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DbOrm
{
    /*
     * DBBase 的扩展，根据实体类的增删改查
     * 子类继承当前类，并实现 CreateConnection()，即拥有 DBBase和 DBExpand的功能
     */
    public abstract class DBExpand : DBBase
    {
        /// <summary>
        /// 创建数据库连接对象，由继承的子类实现
        /// </summary>
        public override abstract IDbConnection CreateConnection();

        public int Insert(IModel model)
        {
            return this.ExecuteNonQuery(model.InsertSql(), model);
        }
        //model参数中有哪些字段数据库就改哪些字段
        public int Update<T>(object model, string where, object param = null) where T : IModel
        {
            Dictionary<string, object> paramList = new Dictionary<string, object>();
            StringBuilder sql = new StringBuilder();
            sql.Append("update ").Append(typeof(T).Name).Append(" set ");
            foreach (var p in model.GetType().GetProperties())
            {
                sql.Append(p.Name).Append("=").Append("@").Append(p.Name).Append(",");
                paramList['@' + p.Name] = p.GetValue(model);
            }
            sql.Length--;//删除最后一个字符
            sql.Append(" where ").Append(where);
            if (param != null)
            {
                foreach (var p in param.GetType().GetProperties())
                {
                    paramList['@' + p.Name] = p.GetValue(param);
                }
            }
            return this.ExecuteNonQuery(sql.ToString(), paramList);
        }
        public int Delete<T>(string where, object param = null) where T : IModel
        {
            return this.ExecuteNonQuery($"delete from {typeof(T).Name} where {where}", param);
        }
        public SqlBuilder<T> Select<T>(string column = "*") where T : IModel
        {
            return new SqlBuilder<T>(Connection, false, Transaction).Select(column).From(typeof(T).Name);
        }
        public SqlBuilder Select(string table, string column = "*")
        {
            return new SqlBuilder(Connection, false, Transaction).Select(column).From(table);
        }
    }
}
