import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react-swc';
import { fileURLToPath } from 'node:url';

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
                '/sagas': fileURLToPath(new URL('./src/sagas', import.meta.url)),
            },
        },
    };
});
