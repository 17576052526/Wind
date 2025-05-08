/*
* 接口测试专用
* */
package com.test.other;

import com.codegeneration.mapper.TestMainMapper;
import com.wind.Result;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/test")
public class TestInterController {

    @Autowired
    private TestMainMapper mapper;

    @GetMapping("/select")
    public Result select() {

        return Result.OK();
    }




}
