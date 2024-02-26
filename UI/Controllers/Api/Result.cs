namespace UI.Controllers.Api
{
    /*
状态码：
    请求成功  200
	未授权，需要跳转到登陆页面   403
	权限不够   401
	服务器报错   500
	前端弹框提示  210
	自定义状态码，针对单个接口的状态码，非公共状态码   -xxx
     */
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
                code = 200,
                data = data,
                msg = "OK",
            };
        }
        /// <summary>
        /// 弹框提示
        /// </summary>
        public static Result Alert(string msg)
        {
            return new Result()
            {
                code = 210,
                msg = msg
            };
        }
        /// <summary>
        /// 自定义返回结果
        /// </summary>
        public static Result Custom(int code, string msg, object data = null)
        {
            return new Result()
            {
                code = code,
                msg = msg,
                data = data
            };
        }
    }
}
