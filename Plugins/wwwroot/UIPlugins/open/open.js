+function ($) {
    //显示窗口，并设置窗口位置
    //node：定位在此节点周边，direction：方向（left,top,right,bottom,null(和node位置一样)）,align：对其方式(center,left,top,right,bottom)
    $.fn.open = function (node, direction, align) {
        //淡出（显示），必须放在前面，不能用css('display','block');
        this.fadeIn(300);
        if (node) {
            var offset = node.offset(), left = offset.left, top = offset.top;
            if (direction == 'right') {
                left = left + node.outerWidth();
            } else if (direction == 'bottom') {
                top = top + node.outerHeight();
            } else if (direction == 'top') {
                top = top - this.outerHeight();
            } else if (direction == 'left') {
                left = left - this.outerWidth();
            }
            //对其方式
            if ((direction == 'right' || direction == 'left') && align == 'center') {
                top = top - (this.outerHeight() / 2) + (node.outerHeight() / 2);
            }
            else if ((direction == 'bottom' || direction == 'top') && align == 'center') {
                left = left - (this.outerWidth() / 2) + (node.outerWidth() / 2);
            }
            else if ((direction == 'right' || direction == 'left') && align == 'bottom') {
                top = top - this.outerHeight() + node.outerHeight();
            }
            else if ((direction == 'top' || direction == 'bottom') && align == 'right') {
                left = left - this.outerWidth() + node.outerWidth();
            }
            //设置
            this.css({ 'position': 'fixed', 'left': left, 'top': top });
        } else {
            this.css({ 'position': 'fixed', 'left': '50%', 'top': '50%', 'margin-left': -this.outerWidth() / 2, 'margin-top': -this.outerHeight() / 2 });
        }
    }
}(jQuery)