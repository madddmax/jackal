import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import { fileURLToPath } from 'node:url'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  build: {
    assetsDir: 'dist'
  },
  resolve: {
    alias: {
      '/app': fileURLToPath(new URL('./src/app', import.meta.url)),
      '/redux': fileURLToPath(new URL('./src/redux', import.meta.url))
    }
  }
})
