import react from '@vitejs/plugin-react-swc';
import { fileURLToPath } from 'node:url';
import { defineConfig } from 'vite';

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
    const isProd = mode === 'production';

    return {
        plugins: [react()],
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
                '/netgame': fileURLToPath(new URL('./src/netgame', import.meta.url)),
            },
        },
    };
});
