package com.wind.util;

import java.io.FileWriter;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

public class Base {
    public static boolean isNullOrEmpty(Object str) {
        return str == null || str == "";
    }

    //写入日志（可以是错误日志，也可以是生产环境对某个数据的监控），写入的内容是给程序员看的，用于排错
    public static synchronized void writeLog(String str){
        try{
            // 不存在目录则创建目录
            Path uploadPath = Paths.get("../log");
            if (!Files.exists(uploadPath)) {
                Files.createDirectories(uploadPath);
            }
            //写入文件
            LocalDateTime currentTime = LocalDateTime.now();
            FileWriter writer = new FileWriter("../log/log_"+currentTime.format(DateTimeFormatter.ofPattern("yyyy-MM-dd"))+".txt", true);
            writer.write(currentTime.format(DateTimeFormatter.ofPattern("HH:mm:ss "))+str + System.lineSeparator() + System.lineSeparator());
            writer.close();
        } catch (IOException e) {
            System.err.println("Base.writeLog() 出错: " + e.getMessage());
        }
    }

}
