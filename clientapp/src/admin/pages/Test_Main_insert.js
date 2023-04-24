import React, { useEffect, useLayoutEffect, useRef } from 'react';
import axios from 'axios'
import $ from 'jquery'
import { useStates } from '../../common'


export default function ({ close }) {
    let [state, setState] = useStates({
        //定义状态
        
    })

    let form = useRef();

    //页面加载，useLayoutEffect有防抖动效果，例如页面加载事件中关闭当前组件
    useLayoutEffect(() => {


    }, []);

    //提交
    function submit() {
        var param = {};
        for (var m of $(form.current).serializeArray()) {
            param[m.name] = m.value;
        }
        axios.post("/api/test_main/insert", param).then(msg => {
            $.alert('操作成功');
            close();
        });
    }

    return (
        <div className="box box-move fixed-center" style={{ width: '800px', height: '500px' }}>
            <div className="box-head box-move-switch flex">
                <div className="flex-1">新建</div>
                <i className="icon-remove" onClick={() => close()}></i>
            </div>
            <div className="box-body">
                <form ref={form}>
                    <div className="form-row">
                        <label className="form-item">
                            <span>编号</span>
                            <input type="text" className="input-text" name="MainID" />
                        </label>
                        <label className="form-item">
                            <span>名称</span>
                            <input type="text" className="input-text" name="MainName" />
                        </label>
                        <label className="form-item">
                            <span>数量</span>
                            <input type="text" className="input-text" name="Quantity" />
                        </label>
                        <label className="form-item">
                            <span>价格</span>
                            <input type="text" className="input-text" name="Amount" />
                        </label>
                        <label className="form-item">
                            <span>是否</span>
                            <input type="checkbox" name="IsShow" value="true" />
                        </label>
                        <label className="form-item">
                            <span>备注</span>
                            <input type="text" className="input-text" name="Remark" />
                        </label>
                    </div>
                </form>
            </div>
            <div className="box-foot">
                <input type="button" className="btn-primary mlr-10" value="确定" onClick={() => submit()} />
                <input type="button" className="btn-default mlr-10" value="取消" onClick={() => close()} />
            </div>
        </div>
    );
}
