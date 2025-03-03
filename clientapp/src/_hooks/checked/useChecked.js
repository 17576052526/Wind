import { useState } from 'react';

export default function () {
    let [checked, setChecked] = useState([]);

    function set(data, isAdd) {
        if (!data) { return; }
        if (!Array.isArray(data)) { data = [data]; }

        for (let m of data) {
            let jsonStr = JSON.stringify(m);
            let index = checked.findIndex(s => JSON.stringify(s) == jsonStr);
            if (isAdd) {
                if (index == -1) {
                    checked.push(m);
                }
            } else {
                if (index != -1) { checked.splice(index, 1); }
            }
        }
        setChecked([...checked])
    }

    return [checked, set];
}