package com.wind;

import lombok.Data;

/*
* 公共状态码：
    请求成功  200
	未授权，需要跳转到登陆页面   403
	权限不够   401
	服务器报错   500
	前端只提示，不做任何处理  210
自定义状态码：单个接口的业务状态码，非公共状态码   -99到99
* */
@Data
public class Result {
    //状态码
    private int code;
    //提示信息
    private String msg;
    //主体数据
    private Object data;

    public static Result OK()
    {
        Result result= new Result();
        result.code=200;
        result.msg="OK";
        return  result;
    }

    public static Result OK(Object data)
    {
        Result result= new Result();
        result.code=200;
        result.msg="OK";
        result.data=data;
        return  result;
    }

    public static Result Fail(String msg)
    {
        Result result= new Result();
        result.code=210;
        result.msg=msg;
        return  result;
    }

    public static Result Fail(int code,String msg)
    {
        Result result= new Result();
        result.code=code;
        result.msg=msg;
        return  result;
    }

    public static Result Fail(int code,String msg,Object data)
    {
        Result result= new Result();
        result.code=code;
        result.msg=msg;
        result.data=data;
        return  result;
    }
}
