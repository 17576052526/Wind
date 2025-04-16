package com.wind.util;

import org.springframework.web.multipart.MultipartFile;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.Random;

public class UploadFile {
    public static String upload(MultipartFile file,String uploadDir) throws IOException {
        // 获取当前日期
        LocalDate currentDate = LocalDate.now();
        String datePath = currentDate.format(DateTimeFormatter.ofPattern("/yyyy/MM/dd/"));

        // 创建上传目录（如果不存在）
        Path uploadPath = Paths.get(uploadDir, datePath);
        if (!Files.exists(uploadPath)) {
            Files.createDirectories(uploadPath);
        }

        //获取文件后缀
        String name = file.getOriginalFilename();
        int dotIndex = name.lastIndexOf('.');
        String plusName = name.substring(dotIndex);
        //构造文件名
        Random random = new Random();
        int randomNumber = random.nextInt(900000) + 100000;
        String newFileName = System.currentTimeMillis() + randomNumber + plusName;//时间戳+随机数构造文件名

        //构造路径
        Path path = uploadPath.resolve(newFileName);
        //上传文件
        Files.write(path, file.getBytes());

        return datePath + newFileName;
    }
}
