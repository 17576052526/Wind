package com.wind;

import org.mybatis.spring.annotation.MapperScan;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.ComponentScan;

@SpringBootApplication
@ComponentScan({"com.wind","com.test"})
@MapperScan({"com.codegeneration.mapper","com.test"})
public class JavaWindApplication {

    public static void main(String[] args) {
        SpringApplication.run(JavaWindApplication.class, args);
    }

}
