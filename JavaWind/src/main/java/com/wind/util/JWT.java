package com.wind.util;

import io.jsonwebtoken.Claims;
import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.SignatureAlgorithm;

import javax.crypto.spec.SecretKeySpec;
import java.security.Key;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;

/*
* 加密算法 HS256和 RS256：
HS256 对称加密算法，签名和验签使用同一个密钥，计算速度快，适合单服务器使用
RS256 非对称加密算法，私钥用于生成签名，公钥用于验证签名，公钥可以公开，计算速度慢，用于微服务架构
* */
public class JWT {
    // 密钥，用于签名和验证 JWT
    //private static final Key SECRET_KEY = Keys.secretKeyFor(SignatureAlgorithm.HS256);
    private static final Key SECRET_KEY = new SecretKeySpec("aGYg49EW0X7Z1m2AlwlR2qwu4Hxisxia3UTjTGV+ct0=".getBytes(), SignatureAlgorithm.HS256.getJcaName());
    // 有效期
    private static final long EXPIRATION_TIME = 60 * 60 * 1000;

    // 生成 JWT
    public static String generateToken(String userID,String subject,String role) {
        Date now = new Date();
        Date expiration = new Date(now.getTime() + EXPIRATION_TIME);

        Map<String, Object> claims = new HashMap<>();
        claims.put("role", role);//角色存入进去

        return Jwts.builder()
                .setClaims(claims)
                .setId(userID)
                .setSubject(subject)
                .setIssuedAt(now)
                .setExpiration(expiration)
                .signWith(SECRET_KEY)
                .compact();
    }

    // 验证 JWT
    public static Claims validateJwt(String token) {
        return Jwts.parserBuilder()
                .setSigningKey(SECRET_KEY)
                .build()
                .parseClaimsJws(token)
                .getBody();
    }
}
