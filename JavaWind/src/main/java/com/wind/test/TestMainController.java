package com.wind.test;

import com.baomidou.mybatisplus.core.conditions.update.UpdateWrapper;
import com.baomidou.mybatisplus.extension.plugins.pagination.Page;
import com.codegeneration.mapper.TestMainMapper;
import com.codegeneration.model.TestMain;
import com.codegeneration.model.TestType;
import com.github.yulichang.wrapper.MPJLambdaWrapper;
import com.wind.Result;
import com.wind.test.model.TestMainPlus;
import com.wind.util.Base;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;
import java.util.Map;

@RestController//接口方法返回对象并转换成json
@RequestMapping("/api/testmain")
public class TestMainController {

    @Autowired
    private TestMainMapper mapper;

    @PostMapping("/insert")
    public Result insert(@RequestBody TestMain param) {
        mapper.insert(param);
        return Result.OK();
    }

    @PostMapping("/delete")
    public Result delete(@RequestBody List<Integer> param) {
        mapper.deleteBatchIds(param);
        return Result.OK();
    }

    @PostMapping("/update")
    public Result update(@RequestBody TestMain param) {
        UpdateWrapper<TestMain> updateWrapper = new UpdateWrapper<>();
        updateWrapper.eq("id", param.getId())
                .set("main_id", param.getMainId())
                .set("main_name", param.getMainName())
                .set("test_type_id", param.getTestTypeId())
                .set("quantity", param.getQuantity())
                .set("amount", param.getAmount())
                .set("is_show", param.getIsShow())
                .set("img", param.getImg())
                .set("files", param.getFiles())
                .set("remark", param.getRemark())
                .set("create_time", param.getCreateTime());
        mapper.update(null, updateWrapper);
        return Result.OK();
    }

    @PostMapping("/selectPage")
    public Result selectPage(@RequestBody Map<String, Object> param) {
        MPJLambdaWrapper<TestMain> wrapper = new MPJLambdaWrapper<>(TestMain.class)
                .selectAll(TestMain.class)
                .select(TestType::getName)
                .leftJoin(TestType.class, TestType::getId, TestMain::getTestTypeId)
                .orderByDesc(TestMain::getId);
        //构造条件
        if (!Base.isNullOrEmpty(param.get("MainName"))) {
            wrapper.like(TestMain::getMainName, param.get("MainName"));//查询条件
        }
        //查询数据库
        Page<TestMainPlus> data = mapper.selectJoinPage(new Page((int)param.get("pageIndex"), (int)param.get("pageSize")), TestMainPlus.class, wrapper);
        return Result.OK(data);
    }
}
