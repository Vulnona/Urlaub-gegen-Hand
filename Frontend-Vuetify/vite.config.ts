import vue from '@vitejs/plugin-vue';
import vuetify, { transformAssetUrls } from 'vite-plugin-vuetify';
import ViteFonts from 'unplugin-fonts/vite';
import { defineConfig, loadEnv } from 'vite';
import { fileURLToPath, URL } from 'node:url';

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '');
  return {
    server: {
      host: '0.0.0.0',
      port: 80,
      watch: {
        usePolling: env.VITE_USE_POLLING === 'true',
      },
    },
    optimizeDeps: {
      include: ['vue', 'vue-router', 'sweetalert2', 'crypto-js', 'axios', 'vue-toastification',
         'vue-jwt-decode', 'lodash/debounce', 'bootstrap/dist/js/bootstrap.min.js', 'vue-multiselect',
          'crypto-js/aes','vue3-datepicker'], // Pre-bundle these dependencies for better performance
    },
    plugins: [
      vue({
        template: { transformAssetUrls },
      }),
      vuetify({
        autoImport: true,
        styles: {
          configFile: 'src/styles/settings.scss',
        },
      }),
      ViteFonts({
        google: {
          families: [
            {
              name: 'Roboto',
              styles: 'wght@100;300;400;500;700;900',
            },
          ],
        },
      }),
    ],
    define: {
      'process.env': {
        SECRET_KEY: env.SECRET_KEY,
        baseURL: env.BASE_URL,
        baseURL_Frontend: env.BASE_URL_FRONTEND,
        claims_Url: env.CLAIMS_URL,
      },
    },
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url)),
      },
      extensions: ['.js', '.json', '.jsx', '.mjs', '.ts', '.tsx', '.vue'],
    },
    base: './',
  };
});
