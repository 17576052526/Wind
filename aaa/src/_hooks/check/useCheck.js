import React, { useState } from 'react';

export default function () {
    let [checks, setChecks] = useState([]);

    function set(param, type) {
        if (Array.isArray(param)) {
            checks = param;
        } else {
            let index = checks.indexOf(param);
            if (type == null) {
                if (index == -1) {
                    checks.push(param);
                } else {
                    checks.splice(index, 1);
                }
            } else {
                if (type == '+') {
                    index == -1 && checks.push(param);
                } else if (type == '-') {
                    index != -1 && checks.splice(index, 1);
                }
            }
        }
        setChecks([...checks]);
    }

    //Í¬²½
    function sync(data, equals) {
        for (let i = 0; i < checks.length; i++) {
            let model = data.find(s => equals(checks[i], s));
            if (model) {
                checks[i] = model;
            } else {
                checks.splice(i, 1);
            }
        }
        setChecks([...checks]);
    }

    return [checks, set, sync];
}
