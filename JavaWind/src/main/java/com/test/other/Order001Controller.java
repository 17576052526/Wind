package com.test.other;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

@Controller
public class Order001Controller {

    @GetMapping("/")
    public String index() {
        return "index"; // 返回视图名称，对应src/main/resources/templates/index.html
    }

    @GetMapping("/aa")
    public String aa() {
        return "index"; // 返回视图名称，对应src/main/resources/templates/index.html
    }
}