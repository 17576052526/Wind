package com.projectname.order;

import com.codegeneration.mapper.TestMainMapper;
import com.wind.Result;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController//接口方法返回对象并转换成json
@RequestMapping("/api/order")
public class OrderController {

    @Autowired
    private TestMainMapper mapper;

    @PostMapping("/orderlist")
    public Result orderlist() {

        return Result.OK();
    }




}
