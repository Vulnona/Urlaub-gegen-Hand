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
      port: 3000,
      watch: {
        usePolling: env.VITE_USE_POLLING === 'true',
      },
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
        SECRET_KEY: env.VITE_SECRET_KEY, 
        baseURL: env.VITE_BASE_URL, 
        baseURL_Frontend: env.VITE_BASE_URL_FRONTEND, 
        SecretAccessKey: env.VITE_AWS_SECRET_ACCESS_KEY, 
        AccessKeyId: env.VITE_AWS_ACCESS_KEY_ID,
        Aws_region: env.VITE_AWS_REGION, 
        S3_BUCKET_NAME: env.VITE_S3_BUCKET_NAME, 
        x_api_key: env.VITE_API_KEY, 
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
