using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DMSCCore
{
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
            foreach(DataRow tableDr in tableDt.Rows)
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
            { "int","int" }, {"varchar","string" },{"bit","bool" } ,{"datetime","DateTime?" },{"decimal","decimal" },{"float","double" },
            {"image","byte[]" },{"money","Single" },{ "ntext","string" },{"nvarchar","string" },{"smalldatetime","DateTime?" },{"smallint","Int16" },
            {"text","string" },{"bigint","Int64" },{"binary","byte[]" },{"char" ,"string"},{"nchar","string" },{"numeric" ,"decimal"},{ "real","Single" },
            {"smallmoney","Single" }, {"sql_variant","object" },{"timestamp","byte[]" },{"tinyint","byte" },{"uniqueidentifier","Guid" },{"varbinary","byte[]" }
        };
    }
}
