using System;

namespace Wind.Model
{
    /// <summary>
    /// 测试表
    /// </summary>
    public partial class Test
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
    }
}
