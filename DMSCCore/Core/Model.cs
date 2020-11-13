using System;
using System.Collections.Generic;

namespace DMSCCore
{
    public class Table
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 表说明
        /// </summary>
        public string Description { set; get; }
        /// <summary>
        /// 主键（列名）
        /// </summary>
        public string PrimaryKey { set; get; }
        /// <summary>
        /// 表中的列
        /// </summary>
        public List<Column> Column { set; get; }
        /********************************** 解析出来的数据 **********************************/
        /// <summary>
        /// 当前表的明细表
        /// </summary>
        public List<string> DtlTable { set; get; }
    }
    /// <summary>
    /// 列
    /// </summary>
    public class Column
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 列说明
        /// </summary>
        public string Description { set; get; }
        /// <summary>
        /// 是否是自增列（与主键无关，自增列不参与 insert）
        /// </summary>
        public bool IsIdentity { set; get; }
        /// <summary>
        /// 数据库中的类型
        /// </summary>
        public string Type { set; get; }
        /// <summary>
        /// 字段长度
        /// </summary>
        public int Length { set; get; }
        /// <summary>
        /// 是否允许空
        /// </summary>
        public bool IsNull { set; get; }
        /// <summary>
        /// C#对应的类型
        /// </summary>
        public string CType { set; get; }
        /********************************** 解析出来的数据 **********************************/
        /// <summary>
        /// 外键表表名，不是外键就是null
        /// </summary>
        public string ForeignKeyTable { set; get; }
        /// <summary>
        /// 外键表显示的列名
        /// </summary>
        public string ForeignKeyName { set; get; }
        /// <summary>
        /// 外键表显示的列名描述
        /// </summary>
        public string ForeignKeyDescription { set; get; }
        /// <summary>
        /// 是否有搜索框（列表页搜索）
        /// </summary>
        public bool IsSearch { set; get; }
        /// <summary>
        /// 是否是图片列
        /// </summary>
        public bool IsImg { set; get; }
        /// <summary>
        /// 是否是文件列
        /// </summary>
        public bool IsFile { set; get; }
        /// <summary>
        /// 是否是<textarea>框
        /// </summary>
        public bool IsDesc { set; get; }
    }

}
