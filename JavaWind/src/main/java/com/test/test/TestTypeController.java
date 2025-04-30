package com.test.test;

import com.codegeneration.mapper.TestTypeMapper;
import com.codegeneration.model.TestMain;
import com.codegeneration.model.TestType;
import com.github.yulichang.wrapper.MPJLambdaWrapper;
import com.wind.Result;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

@RestController//接口方法返回对象并转换成json
@RequestMapping("/api/TestType")
public class TestTypeController {
    @Autowired
    private TestTypeMapper mapper;

    @PostMapping("/select")
    public Result select() {
        MPJLambdaWrapper<TestType> wrapper = new MPJLambdaWrapper<>(TestType.class)
                .selectAll(TestType.class)
                .orderByDesc(TestMain::getId);

        //查询数据库
        List<TestType> data = mapper.selectList(wrapper);
        return Result.OK(data);
    }
}
