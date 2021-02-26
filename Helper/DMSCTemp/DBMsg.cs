using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DMSCCore
{
    #region 数据库辅助类
    public sealed class DbHelper
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static readonly string ConnString = ((Func<string>)(() =>
        {
            string str = File.ReadAllText(Environment.CurrentDirectory + "/Wind.UI/appsettings.json");
            return Regex.Match(str, "(?<=\"ConnString\"\\s*:\\s*\")[^\"]+(?=\")").Value;
        }))();
        /// <summary>
        /// 创建 Connection，通过创建不同类型的实例来达到调用不同类型的数据库
        /// </summary>
        public static IDbConnection CreateConnection()
        {
            return new System.Data.SqlClient.SqlConnection(ConnString);
        }


        /// <summary>
        /// 查询数据，以 DataTable返回查询结果
        /// </summary>
        public static DataTable Select(string sql, CommandType type = CommandType.Text)
        {
            IDbConnection conn = CreateConnection();
            IDataReader reader = null;
            IDbCommand comm = conn.CreateCommand();
            comm.CommandText = sql;
            comm.CommandType = type;
            try
            {
                conn.Open();
                reader = comm.ExecuteReader();
                //----------------开始填充----------------//
                DataTable dt = new DataTable();
                int fieldCount = reader.FieldCount;
                for (int i = 0; i < fieldCount; i++)
                {
                    dt.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                }
                dt.BeginLoadData();

                object[] values = new object[fieldCount];
                var dr = dt.Rows;
                while (reader.Read())
                {
                    reader.GetValues(values);
                    dr.Add(values);
                }
                return dt;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }
        }
    }
    #endregion

    #region 实体类
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
    #endregion

    #region 获取 SqlServer数据库相关信息
    public class SqlServer
    {
        /// <summary>
        /// 获取Sql Server 数据库表和字段
        /// </summary>
        /// <returns></returns>
        public static List<Table> Main()
        {
            //查询所有表
            StringBuilder tableSql = new StringBuilder();
            tableSql.AppendLine("select a.name,b.value,d.column_name from sys.tables a ");
            tableSql.AppendLine("left join sys.extended_properties b on (a.object_id = b.major_id AND b.minor_id = 0) ");
            tableSql.AppendLine("left join information_schema.table_constraints c on(a.name=c.table_name and c.constraint_type='PRIMARY KEY')");
            tableSql.AppendLine("left join information_schema.constraint_column_usage d on(c.constraint_name = d.constraint_name)");
            DataTable tableDt = DbHelper.Select(tableSql.ToString());
            //查询所有表的所有列
            StringBuilder columnSql = new StringBuilder();
            columnSql.AppendLine("select sys.columns.name Name, sys.types.name Type, sys.columns.max_length Length, sys.columns.is_nullable IsNull,");
            columnSql.AppendLine("(select value from sys.extended_properties where sys.extended_properties.major_id = sys.columns.object_id and sys.extended_properties.minor_id = sys.columns.column_id) as Description ,");
            columnSql.AppendLine("(select count(*) from sys.identity_columns where sys.identity_columns.object_id = sys.columns.object_id and sys.columns.column_id = sys.identity_columns.column_id) as IsIdentity ,");
            columnSql.AppendLine("sys.tables.name TableName");
            columnSql.AppendLine("from sys.columns, sys.tables, sys.types where sys.columns.object_id = sys.tables.object_id and sys.columns.system_type_id=sys.types.system_type_id AND sys.types.name<>'sysname' order by sys.tables.name,sys.columns.column_id");
            DataTable columnDt = DbHelper.Select(columnSql.ToString());
            //遍历表
            List<Table> list = new List<Table>();
            foreach (DataRow tableDr in tableDt.Rows)
            {
                Table tableModel = new Table();
                tableModel.Name = Convert.ToString(tableDr["name"]);//表名
                tableModel.Description = Convert.ToString(tableDr["value"]);//表说明
                tableModel.PrimaryKey = Convert.ToString(tableDr["column_name"]);//主键
                tableModel.Column = new List<Column>();//表的列集合
                //遍历当前表的列
                foreach (DataRow dr in columnDt.Rows)//columnDt是所有表的列
                {
                    if (tableModel.Name == Convert.ToString(dr["TableName"]))
                    {
                        Column columnModel = new Column();
                        columnModel.Name = Convert.ToString(dr["Name"]);
                        columnModel.Description = Convert.ToString(dr["Description"]);
                        columnModel.IsIdentity = Convert.ToBoolean(dr["IsIdentity"]);
                        columnModel.Type = Convert.ToString(dr["Type"]);
                        columnModel.Length = Convert.ToInt32(dr["Length"]);
                        columnModel.IsNull = Convert.ToBoolean(dr["IsNull"]);
                        columnModel.CType = SqlTypeToCType[columnModel.Type];
                        tableModel.Column.Add(columnModel);
                    }
                }
                list.Add(tableModel);
            }
            return list;
        }

        /// <summary>
        /// SqlServer 数据类型与C# 数据类型对应键值对，key:SqlType类型，value:C#类型
        /// </summary>
        public static readonly Dictionary<string, string> SqlTypeToCType = new Dictionary<string, string> {
            { "int","int?" }, {"varchar","string" },{"bit","bool?" } ,{"datetime","DateTime?" },{"decimal","decimal?" },{"float","double?" },
            {"image","byte[]" },{"money","Single?" },{ "ntext","string" },{"nvarchar","string" },{"smalldatetime","DateTime?" },{"smallint","Int16?" },
            {"text","string" },{"bigint","Int64?" },{"binary","byte[]" },{"char" ,"string"},{"nchar","string" },{"numeric" ,"decimal?"},{ "real","Single?" },
            {"smallmoney","Single?" }, {"sql_variant","object" },{"timestamp","byte[]" },{"tinyint","byte?" },{"uniqueidentifier","Guid?" },{"varbinary","byte[]" }
        };
    }
    #endregion

    public class DMSC
    {
        /// <summary>
        /// 入口方法
        /// </summary>
        public static List<Table> Main()
        {
            return SqlServer.Main();
        }
    }
}
