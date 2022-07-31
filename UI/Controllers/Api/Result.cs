namespace UI.Controllers.Api
{
    //属性首字母小写，是因为 js 命名规范是小写
    /// <summary>
    /// 接口返回结果
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 业务状态码
        /// </summary>
        public int code { set; get; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string msg { set; get; }
        /// <summary>
        /// 主体数据
        /// </summary>
        public object data { set; get; }

        /// <summary>
        /// 成功的结果
        /// </summary>
        public static Result OK(object data = null)
        {
            return new Result()
            {
                code = 1,
                data = data,
                msg = "OK",
            };
        }

        /// <summary>
        /// 失败的结果
        /// </summary>
        public static Result Fail(string msg, int code = -1)
        {
            return new Result()
            {
                code = code,
                msg = msg,
            };
        }
    }
}
