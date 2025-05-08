package com.test.test.mapper;

import com.codegeneration.model.TestMain;
import org.apache.ibatis.annotations.Select;

import java.util.List;

public interface TestMainPlusMapper {

    @Select("select * from test_main where main_name like CONCAT('%', #{name}, '%') and is_show=#{isShow}")
    List<TestMain> getTestMainList(String name,boolean isShow);

}
