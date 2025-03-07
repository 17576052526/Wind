using System;
using System.Text;

namespace DbOrm.Model
{
    /// <summary>
    /// 测试表
    /// </summary>
    public class Test : IModel
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int? ID { set; get; }
        /// <summary>
        /// 所属类型（外键）
        /// </summary>
        public int? TypesID { set; get; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { set; get; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime? Dates { set; get; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Img { set; get; }
        /// <summary>
        /// 数量
        /// </summary>
        public int? Num { set; get; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal? Price { set; get; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool? Che { set; get; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { set; get; }
        /// <summary>
        /// 文件
        /// </summary>
        public string Files { set; get; }

        //实体类转SqlParameter不在DAL中做，在DB中用反射，如果后期要在DAL中手写代码转，就在T4模板中写就行了
        //理由1.：插入10万条要耗时10秒，用10万次反射也就20毫秒，2.string类型要做判断

        /// <summary>
        /// 新增
        /// </summary>
        internal override string InsertSql()
        {
            return "insert into Test(TypesID,Title,Dates,Img,Num,Price,Che,Desc,Files) values(@TypesID,@Title,@Dates,@Img,@Num,@Price,@Che,@Desc,@Files)";
        }
    }
}
