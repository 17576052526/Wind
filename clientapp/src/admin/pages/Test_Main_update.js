import React, { useEffect, useLayoutEffect, useRef } from 'react';
import axios from 'axios'
import $ from 'jquery'
import { useStates } from '../../common'


export default function ({ close, checks }) {
    let [state, setState] = useStates({
        //定义状态

    })

    let form = useRef();

    //页面加载，useLayoutEffect有防抖动效果，例如页面加载事件中关闭当前组件
    useLayoutEffect(() => {
        if (checks.length == 0) { $.alert('请先勾选'); close(); return; }
        if (checks.length > 1) { $.alert('一次只能修改一条'); close(); return; }

        setState({ data: checks[0] });

    }, []);

    //提交
    function submit() {
        var param = {};
        for (var m of $(form.current).serializeArray()) {
            param[m.name] = m.value;
        }
        param.ID = state.data.ID;
        axios.post("/api/test_main/update", param).then(msg => {
            $.alert('操作成功');
            close();
        });
    }

    return (
        <div className="box box-move fixed-center" style={{ width: '800px', height: '500px' }}>
            <div className="box-head box-move-switch flex">
                <div className="flex-1">修改</div>
                <i className="icon-remove" onClick={() => close()}></i>
            </div>
            <div className="box-body">
                <form ref={form}>
                    <div className="form-row">
                        <label className="form-item">
                            <span>编号</span>
                            <input type="text" className="input-text" name="MainID" defaultValue={state.data && state.data.MainID} />
                        </label>
                        <label className="form-item">
                            <span>名称</span>
                            <input type="text" className="input-text" name="MainName" defaultValue={state.data && state.data.MainName} />
                        </label>
                        <label className="form-item">
                            <span>数量</span>
                            <input type="text" className="input-text" name="Quantity" defaultValue={state.data && state.data.Quantity} />
                        </label>
                        <label className="form-item">
                            <span>价格</span>
                            <input type="text" className="input-text" name="Amount" defaultValue={state.data && state.data.Amount} />
                        </label>
                        <label className="form-item">
                            <span>是否显示</span>
                            <div>
                                <input type="hidden" name="IsShow" value="false" />
                                <input type="checkbox" name="IsShow" value="true" defaultChecked={state.data && state.data.IsShow} />
                            </div>
                        </label>
                        <label className="form-item">
                            <span>备注</span>
                            <input type="text" className="input-text" name="Remark" defaultValue={state.data && state.data.Remark} />
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
