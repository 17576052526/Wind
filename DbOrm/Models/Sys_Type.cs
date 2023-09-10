//----------------------------
//手写代码写在此文件
//----------------------------
using System;

namespace DbOrm.Model
{
    /// <summary>
    /// 类型表、分类表
    /// </summary>
    public partial class Sys_Type
    {
        public List<Sys_Type> Children { get; set; }
    }
}
