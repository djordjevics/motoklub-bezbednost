import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 3000,
    proxy: {
      '/api': {
        target: 'http://localhost:5000',
        changeOrigin: true,
      }
    }
  },
  optimizeDeps: {
    include: ['aws-amplify', '@aws-amplify/auth'],
    esbuildOptions: {
      target: 'es2020',
    },
  },
  resolve: {
    alias: {
      './runtimeConfig': './runtimeConfig.browser',
      'aws-amplify/auth': '@aws-amplify/auth',
    },
  },
  define: {
    global: 'globalThis',
  },
})

