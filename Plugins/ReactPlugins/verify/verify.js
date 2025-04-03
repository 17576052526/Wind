import alert from '../alert/alert';

export default function (box) {
    var isVerify = true;
    box.querySelectorAll('[verify]').forEach(s => {
        var val = s.value;
        var v = s.getAttribute('verify');
        var msg = s.getAttribute('msg');
        if (v == 'notnull') {
            v = '.+';
        }
        if (!new RegExp(v).test(val)) {
            isVerify = false;
            alert(msg);
        }
    })
    return isVerify;
}