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
        SECRET_KEY: env.SECRET_KEY, 
        baseURL: env.BASE_URL, 
        baseURL_Frontend: env.BASE_URL_FRONTEND, 
        SecretAccessKey: env.AWS_SECRET_ACCESS_KEY, 
        AccessKeyId: env.AWS_ACCESS_KEY_ID,
        Aws_region: env.AWS_REGION, 
        Aws_Url:env.AWS_URL,
        S3_BUCKET_NAME: env.S3_BUCKET_NAME, 
        x_api_key: env.API_KEY, 
        claims_Url:env.CLAIMS_URL,
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
