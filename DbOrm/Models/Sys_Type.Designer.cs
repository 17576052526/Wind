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
    /// 类型表、分类表
    /// </summary>
    public partial class Sys_Type : IModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int? ID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string TypeID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 新增
        /// </summary>
        internal override string InsertSql()
        {
            return "insert into Sys_Type(TypeID,Name) values(@TypeID,@Name)";
        }
    }
}

