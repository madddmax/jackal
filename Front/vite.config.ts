import react from '@vitejs/plugin-react-swc';
import { fileURLToPath } from 'node:url';
import { defineConfig, loadEnv } from 'vite';

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
    const isProd = mode === 'production';
    const env = loadEnv(mode, process.cwd(), '');

    return {
        plugins: [react()],
        define: {
            'process.env': {
                NODE_ENV: env.NODE_ENV,
                BASE_API: env.BASE_API,
                HUB_API: env.HUB_API,
            },
        },
        build: {
            outDir: isProd ? 'dist' : 'dev',
            assetsDir: isProd ? 'dist' : 'dev',
        },
        resolve: {
            alias: {
                '/app': fileURLToPath(new URL('./src/app', import.meta.url)),
                '/content': fileURLToPath(new URL('./src/content', import.meta.url)),
                '/hubs': fileURLToPath(new URL('./src/hubs', import.meta.url)),
                '/redux': fileURLToPath(new URL('./src/redux', import.meta.url)),
                '/auth': fileURLToPath(new URL('./src/auth', import.meta.url)),
                '/common': fileURLToPath(new URL('./src/common', import.meta.url)),
                '/game': fileURLToPath(new URL('./src/game', import.meta.url)),
                '/lobby': fileURLToPath(new URL('./src/lobby', import.meta.url)),
            },
        },
    };
});
