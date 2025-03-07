using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbOrm
{
    /// <summary>
    /// 由实体类继承的接口
    /// </summary>
    public abstract class IModel
    {
        /// <summary>
        /// 返回 insert sql语句
        /// </summary>
        /// <returns></returns>
        internal abstract string InsertSql();
    }

}
