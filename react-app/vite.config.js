import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import path from 'path'

// https://vite.dev/config/
export default defineConfig({
    plugins: [react()],
    base: './',  //将打包后的文件引用路径改成相对路径（默认是绝对路径，例如："/aa/bb.css"）
    server: {
        open: true, // 是否自动打开浏览器
        port: 3000, // 端口号

        // 代理解决跨域
        proxy: {
            '/server': {
                target: 'http://localhost:8080',  // 接口源地址
                changeOrigin: true,   // 开启跨域
                rewrite: (path) => path.replace(/^\/server/, ''),
            }
        }
    },
    resolve: {
        alias: {
            '@': path.resolve(__dirname, './src')
        }
    }
})
