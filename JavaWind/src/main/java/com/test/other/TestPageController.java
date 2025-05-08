/*
* thymeleaf模板测试专用
* */
package com.test.other;

import com.codegeneration.mapper.TestMainMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

@Controller
public class TestPageController {
    @Autowired
    private TestMainMapper mapper;

    //@GetMapping("/")
    //public String index() {return "index";}

    @GetMapping("/test11")
    public String test11() {
        return "index"; // 返回视图名称，对应src/main/resources/templates/index.html
    }
}