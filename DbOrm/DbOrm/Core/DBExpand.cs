﻿using System;
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
        public int Insert(IEnumerable<IModel> list)
        {
            this.BeginTransaction();
            try
            {
                int count = 0;
                foreach (IModel m in list)
                {
                    count += this.ExecuteNonQuery(m.InsertSql(), m);
                }
                this.CommitTransaction();
                return count;
            }
            catch
            {
                this.RollbackTransaction();
                throw;
            }
        }
        public int Update(IModel model, string where, object param = null)
        {
            Dictionary<string, object> paramList = new Dictionary<string, object>();
            foreach (var p in model.GetType().GetProperties())
            {
                paramList.Add('@' + p.Name, p.GetValue(model));
            }
            foreach (var p in param.GetType().GetProperties())
            {
                paramList.Add('@' + p.Name, p.GetValue(param));
            }
            return this.ExecuteNonQuery(model.UpdateSql() + " where " + where, paramList);
        }
        public int Delete<T>(string where, object param = null) where T : IModel
        {
            return this.ExecuteNonQuery($"delete from {typeof(T).Name} where {where}", param);
        }
        public SqlBuilder<T> Select<T>(string column = "*") where T : IModel
        {
            return new SqlBuilder<T>(Connection, Transaction).Select(column).From(typeof(T).Name);
        }
    }
}
