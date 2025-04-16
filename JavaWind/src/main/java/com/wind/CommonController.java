package com.wind;

import com.baomidou.mybatisplus.core.conditions.query.QueryWrapper;
import com.codegeneration.mapper.SysAdminMapper;
import com.codegeneration.model.SysAdmin;
import com.wind.util.JWT;
import com.wind.util.AES;
import com.wind.util.Captcha;
import com.wind.util.UploadFile;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import javax.imageio.ImageIO;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import java.awt.image.BufferedImage;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.util.Map;

@RestController//接口方法返回对象并转换成json
@RequestMapping("/api/common")
public class CommonController {
    @Autowired
    SysAdminMapper sysAdminMapper;

    @PostMapping("/login")
    public Result login(@RequestBody Map<String, Object> param, HttpSession session) {
        String userName = param.get("userName").toString();
        String userPwd = param.get("userPwd").toString();
        String verifyCode = param.get("verifyCode").toString();

        //检查验证码
        String storedCaptcha = (String) session.getAttribute("captcha");
        if (storedCaptcha != null && !storedCaptcha.toLowerCase().equals(verifyCode.toLowerCase())) {
            return Result.Fail(-2, "验证码错误");
        }
        //验证用户名和密码
        QueryWrapper<SysAdmin> queryWrapper = new QueryWrapper<>();
        queryWrapper.eq("user_name", userName);
        queryWrapper.eq("user_pwd", AES.encrypt(userPwd));
        SysAdmin admin = sysAdminMapper.selectOne(queryWrapper);
        if (admin == null) {
            int loginCount = session.getAttribute("loginCount") == null ? 0 : (int) session.getAttribute("loginCount");
            loginCount++;
            session.setAttribute("loginCount", loginCount);
            if (loginCount >= 5) {
                session.setAttribute("captcha", new Captcha().generateRandomString(5));//重置验证码
                return Result.Fail(-2, "用户名或密码错误");
            } else {
                return Result.Fail(-1, "用户名或密码错误");
            }
        }
        String token = JWT.generateToken(admin.getId().toString(), admin.getUserName());
        return Result.OK(token);
    }


    @GetMapping("/verifyCode")
    public ResponseEntity<byte[]> verifyCode(HttpServletRequest request, HttpServletResponse response) throws IOException {
        Captcha captchaService = new Captcha();
        String captchaText = captchaService.generateRandomString(5);
        HttpSession session = request.getSession();
        session.setAttribute("captcha", captchaText);

        BufferedImage image = captchaService.generateCaptchaImage(captchaText);

        ByteArrayOutputStream baos = new ByteArrayOutputStream();
        ImageIO.write(image, "png", baos);
        byte[] imageBytes = baos.toByteArray();

        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.IMAGE_PNG);
        headers.setContentLength(imageBytes.length);
        headers.setCacheControl("no-cache, no-store, must-revalidate");
        headers.setPragma("no-cache");
        headers.setExpires(0);

        return new ResponseEntity<>(imageBytes, headers, HttpStatus.OK);
    }

    @Value("${file.upload-dir}")
    private String uploadDir;

    @PostMapping("/upload")
    public Result upload(@RequestParam("__file") MultipartFile[] files) {
        try {
            StringBuilder str = new StringBuilder();
            for (MultipartFile file : files) {
                String path = UploadFile.upload(file, uploadDir);//上传图片并返回路径
                str.append(path + "|");
            }
            return Result.OK(str.substring(0, str.length() - 1));
        } catch (Exception ex) {
            return Result.Fail("file upload error");
        }
    }
}
