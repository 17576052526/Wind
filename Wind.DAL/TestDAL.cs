using System;
using System.Collections.Generic;
using System.Text;
using Wind.Model;

namespace Wind.DAL
{
    /// <summary>
    /// 测试类
    /// </summary>
    public partial class TestDAL
    {
        /// <summary>
        /// 新增
        /// </summary>
        public int Insert(Test model)
        {
            string sql = "insert into Test(TypesID,Title,Dates,Img,Num,Price,Che,Desc,Files) values(@TypesID,@Title,@Dates,@Img,@Num,@Price,@Che,@Desc,@Files)";
            return DbHelper.ExecuteNonQuery(sql, model);
        }
        /*
         * Update用反射的理由：根据属性构造要修改的列，下次只需要修改某几个列的时候不用再去写个方法
         * Update用dynamic类型作为参数的理由：dynamic的实例不能是Model里面的实体类，只能是匿名实体类，因为如果加了外键字段构造出来的列是不对的
         */
        /// <summary>
        /// 修改，根据类（匿名类或普通类）中的属性构造要修改的列，model中必须包含主键列（根据主键修改，主键不参与列的构造），此方法是不安全的，因为是根据类的属性构造要修改的列，如果属性有误，那么修改的列也会有误
        /// </summary>
        public int Update(object model)
        {
            //非反射，正常写法
            //string sql = "update Table set col1=@col1,col2=@col2 where ID=@ID";
            //return DbHelper.ExecuteNonQuery(sql, model);

            StringBuilder str = new StringBuilder();
            foreach (var p in model.GetType().GetProperties())
            {
                if (p.Name.ToLower() != "id") { str.Append(',' + p.Name + "=@" + p.Name); }//主键不参与列的构造
            }
            str.Remove(0, 1);//移除第一个逗号
            string sql = "update Test set " + str + " where ID=@ID";
            return DbHelper.ExecuteNonQuery(sql, model);
        }

        /// <summary>
        /// 删除，可删除单条或多条
        /// </summary>
        public void Delete(params string[] id)
        {
            string sql = "delete from test where ID=@ID";
            List<object> param = new List<object>();
            foreach (var m in id)
            {
                param.Add(new { ID = m });
            }
            DbHelper.ExecuteTran(sql, param);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        public List<Test> Select(string where = null, object param = null, int? top = null)
        {
            string sql = "select " + (top != null ? "top " + top : null) + " * from Test " + (!String.IsNullOrEmpty(where) ? "where " + where : null);
            return DbHelper.Select<Test>(sql, param);
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        public Test SelectModel(string id)
        {
            string sql = "select top 1 * from Test where ID=@ID";
            List<Test> list = DbHelper.Select<Test>(sql, new { ID = id });
            return list != null && list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// 分页查询-------若想尽可能多的重用Sql执行计划，index，size,与查询条件都写成参数化的形式，而不是参与sql构造
        /// </summary>
        /// <param name="index">页码</param>
        /// <param name="size">每页显示的条数</param>
        /// <param name="where">条件</param>
        /// <param name="param">参数，例如：new{title="查询条件的@title"}</param>
        /// <param name="order">排序</param>
        public List<Test> SelectPage(int index, int size, string where = null, object param = null, string order = null)
        {
            order = !String.IsNullOrEmpty(order) ? order : "Test.ID desc";
            where = !String.IsNullOrEmpty(where) ? "where " + where : null;
            string sql = $@"
with tab as(
select Test.*,Types.TypesTitle,row_number() over(order by {order}) as SysRowNum from Test
left join Types on(Test.TypesID=Types.ID) {where}
)
select * from tab where SysRowNum between {(index - 1) * size + 1} and {index * size}
";
            return DbHelper.Select<Test>(sql, param);
        }

        /// <summary>
        /// 获取总数据量
        /// </summary>
        public int SelectCount(string where = null, object param = null)
        {
            string sql = "select count(*) from Test " + (!String.IsNullOrEmpty(where) ? "where " + where : null);
            return (int)DbHelper.ExecuteScalar(sql, param);
        }
    }
}
