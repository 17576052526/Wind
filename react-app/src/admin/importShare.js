/*
引入公共插件
*/
import '../_plugins/atomCss/atom.css'
import './assets/font/css/fontello.css'
import '../_plugins/AdminUI2/UI.css'
import '../_plugins/table-fixed/table-fixed.css'
import '../_plugins/box-move/box-move'
import '../_plugins/img-show/img-show'
import { useStates, formToJSON } from '../_utils/base';
import alert from '../_plugins/alert/alert';
import confirm from '../_plugins/confirm/confirm'
import usePager from '../_hooks/pager/usePager'
import useChecked from '../_hooks/checked/useChecked'

export { useStates, formToJSON, alert, confirm, usePager, useChecked }