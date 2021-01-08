using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DMSCCore
{
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
}
