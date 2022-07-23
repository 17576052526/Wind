//设置代理，允许跨域请求
const proxy = require('http-proxy-middleware');
module.exports = function (app) {
    app.use(
        '/api',
        proxy.createProxyMiddleware({
            target: 'http://localhost:62881',//服务器地址
            changeOrigin: true,
            pathRewrite: { '^/api': '' }
        })
    );
    //此处能设置多个代理请求，axios.defaults.baseURL 读取配置的，就能实现通过配置调用不同的服务器地址
};