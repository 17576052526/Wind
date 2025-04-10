/*
当前项目业务相关的配置
*/

//设置服务器请求地址，值是 vite.config.js 配置的代理服务
export const apiUrl = process.env.NODE_ENV == 'development' ? '/server' : ''


//分页，每页显示多少条
export const pageSize = 15;

//文件，图片上传接口地址
export const uploadPath = '/api/common/upload';