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
    /// 杂项表
    /// </summary>
    public partial class Temp_List4 : IModel
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
        public string Img { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Url { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Time { set; get; }
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
        /// 新增
        /// </summary>
        internal override string InsertSql()
        {
            return "insert into Temp_List4(TypeID,Name,Content,Img,Url,Time,Temp1,Temp2,Temp3,Temp4,Temp5) values(@TypeID,@Name,@Content,@Img,@Url,@Time,@Temp1,@Temp2,@Temp3,@Temp4,@Temp5)";
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
            if (Img != null) { str.AppendLine(",Img=@Img"); }
            if (Url != null) { str.AppendLine(",Url=@Url"); }
            if (Time != null) { str.AppendLine(",Time=@Time"); }
            if (Temp1 != null) { str.AppendLine(",Temp1=@Temp1"); }
            if (Temp2 != null) { str.AppendLine(",Temp2=@Temp2"); }
            if (Temp3 != null) { str.AppendLine(",Temp3=@Temp3"); }
            if (Temp4 != null) { str.AppendLine(",Temp4=@Temp4"); }
            if (Temp5 != null) { str.AppendLine(",Temp5=@Temp5"); }

            return "update Temp_List4 set " + str.Remove(0, 1);
        }
    }
}

