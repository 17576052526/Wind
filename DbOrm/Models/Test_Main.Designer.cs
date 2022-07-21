﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码是由工具生成的。
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Text;

namespace DbOrm.Model
{
    /// <summary>
    /// 测试模块主表
    /// </summary>
    public partial class Test_Main : IDAL
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int? ID { set; get; }
        /// <summary>
        /// 编号
        /// </summary>
        public string MainID { set; get; }
        /// <summary>
        /// 名称
        /// </summary>
        public string MainName { set; get; }
        /// <summary>
        /// 所属类型（外键）
        /// </summary>
        public int? Test_Type_ID { set; get; }
        /// <summary>
        /// 数量
        /// </summary>
        public int? Quantity { set; get; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal? Amount { set; get; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool? IsShow { set; get; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Img { set; get; }
        /// <summary>
        /// 文件
        /// </summary>
        public string Files { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { set; get; }
        /// <summary>
        /// 新增
        /// </summary>
        internal override string Insert()
        {
            return "insert into Test_Main(MainID,MainName,Test_Type_ID,Quantity,Amount,IsShow,Img,Files,Remark,CreateTime) values(@MainID,@MainName,@Test_Type_ID,@Quantity,@Amount,@IsShow,@Img,@Files,@Remark,@CreateTime)";
        }

        /// <summary>
        /// 修改
        /// </summary>
        internal override string Update()
        {
            StringBuilder str = new StringBuilder();
            if (ID != null) { str.AppendLine(",ID=@ID"); }
            if (MainID != null) { str.AppendLine(",MainID=@MainID"); }
            if (MainName != null) { str.AppendLine(",MainName=@MainName"); }
            if (Test_Type_ID != null) { str.AppendLine(",Test_Type_ID=@Test_Type_ID"); }
            if (Quantity != null) { str.AppendLine(",Quantity=@Quantity"); }
            if (Amount != null) { str.AppendLine(",Amount=@Amount"); }
            if (IsShow != null) { str.AppendLine(",IsShow=@IsShow"); }
            if (Img != null) { str.AppendLine(",Img=@Img"); }
            if (Files != null) { str.AppendLine(",Files=@Files"); }
            if (Remark != null) { str.AppendLine(",Remark=@Remark"); }
            if (CreateTime != null) { str.AppendLine(",CreateTime=@CreateTime"); }

            return "update Test_Main set " + str.Remove(0, 1);
        }
    }
}
