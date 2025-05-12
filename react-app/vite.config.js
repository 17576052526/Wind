import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import path from 'path'

// https://vite.dev/config/
export default defineConfig({
    plugins: [react()],
    base: './',  //���������ļ�����·���ĳ����·����Ĭ���Ǿ���·�������磺"/aa/bb.css"��
    server: {
        open: true, // �Ƿ��Զ��������
        port: 3000, // �˿ں�

        // ����������
        proxy: {
            '/server': {
                target: 'http://localhost:8080',  // �ӿ�Դ��ַ
                changeOrigin: true,   // ��������
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
