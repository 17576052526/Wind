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
    /// 备用表
    /// </summary>
    public partial class Temp_List8 : IModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int? ID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int? TypeID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Content { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Temp1 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Temp2 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Temp3 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Temp4 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Temp5 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Temp6 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Temp7 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Temp8 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Temp9 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Temp10 { set; get; }
        /// <summary>
        /// 新增
        /// </summary>
        internal override string InsertSql()
        {
            return "insert into Temp_List8(TypeID,Name,Content,CreateTime,Temp1,Temp2,Temp3,Temp4,Temp5,Temp6,Temp7,Temp8,Temp9,Temp10) values(@TypeID,@Name,@Content,@CreateTime,@Temp1,@Temp2,@Temp3,@Temp4,@Temp5,@Temp6,@Temp7,@Temp8,@Temp9,@Temp10)";
        }

        /// <summary>
        /// 修改
        /// </summary>
        internal override string UpdateSql()
        {
            StringBuilder str = new StringBuilder();
            if (TypeID != null) { str.AppendLine(",TypeID=@TypeID"); }
            if (Name != null) { str.AppendLine(",Name=@Name"); }
            if (Content != null) { str.AppendLine(",Content=@Content"); }
            if (CreateTime != null) { str.AppendLine(",CreateTime=@CreateTime"); }
            if (Temp1 != null) { str.AppendLine(",Temp1=@Temp1"); }
            if (Temp2 != null) { str.AppendLine(",Temp2=@Temp2"); }
            if (Temp3 != null) { str.AppendLine(",Temp3=@Temp3"); }
            if (Temp4 != null) { str.AppendLine(",Temp4=@Temp4"); }
            if (Temp5 != null) { str.AppendLine(",Temp5=@Temp5"); }
            if (Temp6 != null) { str.AppendLine(",Temp6=@Temp6"); }
            if (Temp7 != null) { str.AppendLine(",Temp7=@Temp7"); }
            if (Temp8 != null) { str.AppendLine(",Temp8=@Temp8"); }
            if (Temp9 != null) { str.AppendLine(",Temp9=@Temp9"); }
            if (Temp10 != null) { str.AppendLine(",Temp10=@Temp10"); }

            return "update Temp_List8 set " + str.Remove(0, 1);
        }
    }
}

