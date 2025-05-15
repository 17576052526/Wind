/*
* 公用的多表联查实体类
*   类名命名规则：主表名+连接表名，例如：TestMainTestType，如果是连接三个表，就三个表名都加上去
* */
package com.test.model;

import com.codegeneration.model.TestMain;
import lombok.Data;

@Data
public class TestMainTestType extends TestMain {
    private String testTypeName;
}
