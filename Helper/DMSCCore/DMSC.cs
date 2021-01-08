using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DMSCCore
{
    public class DMSC
    {
        /// <summary>
        /// DMSC 的入口
        /// </summary>
        /// <returns></returns>
        public static List<Table> Main()
        {
            return SqlServer.Main();
        }
    }
}
