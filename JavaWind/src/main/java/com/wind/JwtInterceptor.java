package com.wind;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.wind.util.JWT;
import io.jsonwebtoken.Claims;
import org.springframework.stereotype.Component;
import org.springframework.web.servlet.HandlerInterceptor;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

/*
* 服务端不应该保存任何有关jwt的信息，例如：过期时间，因为jwt是为微服务架构设计的，你不能只在某一台服务器上设置有效时间
jwt 目前没做续签，有效时长为一个小时，续签方案：当有效时长小于10分钟的时候，重新签发JWT
jwt 退出登录，目前只是客户端清空jwt，如果需要服务端也清空，那应该用redis来保存jwt过期时间
* */
@Component
public class JwtInterceptor implements HandlerInterceptor {

    @Override
    public boolean preHandle(HttpServletRequest request, HttpServletResponse response, Object handler) throws Exception {
        String token = request.getHeader("Authorization");
        Result result = new Result();
        if (token != null) {
            try {
                Claims claims = JWT.validateJwt(token);//会验证token的有效期
                // 将用户信息存入请求属性，供后续处理使用
                request.setAttribute("claims", claims);
                return true;
            } catch (Exception ex) {
                result.setCode(403);
                result.setMsg(ex.getMessage());
            }
        } else {
            result.setCode(403);
            result.setMsg("token is null");
        }
        //转换成json并输出到前端
        String jsonString = new ObjectMapper().writeValueAsString(result);
        response.setContentType("application/json;charset=UTF-8");
        response.getWriter().write(jsonString);
        return false;
    }
}