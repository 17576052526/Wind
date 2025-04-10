package com.wind.util;

import javax.crypto.Cipher;
import javax.crypto.spec.SecretKeySpec;
import java.util.Base64;
/*
* AES 加密、解密
* */
public class AES {
    private static final String ALGORITHM = "AES";
    //密钥
    private static final String KEY = "a396d906b978475d90b040139c48bd3f"; // 16字节的密钥

    //加密
    public static String encrypt(String plainText){
        try {
            SecretKeySpec secretKey = new SecretKeySpec(KEY.getBytes("UTF-8"), ALGORITHM);
            Cipher cipher = Cipher.getInstance("AES/ECB/PKCS5Padding");
            cipher.init(Cipher.ENCRYPT_MODE, secretKey);
            byte[] encryptedBytes = cipher.doFinal(plainText.getBytes("UTF-8"));
            return Base64.getEncoder().encodeToString(encryptedBytes);
        } catch (Exception ex) {
            return null;
        }
    }

    //解密
    public static String decrypt(String encryptedText){
        try {
            SecretKeySpec secretKey = new SecretKeySpec(KEY.getBytes("UTF-8"), ALGORITHM);
            Cipher cipher = Cipher.getInstance("AES/ECB/PKCS5Padding");
            cipher.init(Cipher.DECRYPT_MODE, secretKey);
            byte[] decodedBytes = Base64.getDecoder().decode(encryptedText);
            byte[] decryptedBytes = cipher.doFinal(decodedBytes);
            return new String(decryptedBytes, "UTF-8");
        }catch (Exception ex){
            return null;
        }
    }
}
