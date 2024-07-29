// Plugins
import vue from '@vitejs/plugin-vue'
import vuetify, { transformAssetUrls } from 'vite-plugin-vuetify'
import ViteFonts from 'unplugin-fonts/vite'

// Utilities
import { defineConfig, loadEnv } from 'vite'
import { fileURLToPath, URL } from 'node:url'

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '');
  return {
    server: {
      host: '0.0.0.0',
      port: 3000,
      watch: {
        usePolling: JSON.stringify(env.VITE_USE_POLLING) === 'true',
      }
    },
    plugins: [
      vue({
        template: { transformAssetUrls },
      }),
      // https://github.com/vuetifyjs/vuetify-loader/tree/master/packages/vite-plugin#readme
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
        'SECRET_KEY': 'thisismytestsecretkey', 'baseURL': 'http://localhost:8080/api/', 'baseURL_Frontend': 'http://localhost:3000/',
        'SecretAccessKey': 'G5BssUS++VqZ7WtQ14ivN9i6LDn/s+GJVwDdI9bH', 'AccessKeyId': 'AKIA5FTZAYJ5ASDPKHL6', 'Aws_region': 'eu-north-1',
        'S3_BUCKET_NAME': 'urlaub-gegen-hand','x-api-key': '79ea7130-21a6-11ef-9447-bb18dd052b84',
      }
    },
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url)),
      },
      extensions: ['.js', '.json', '.jsx', '.mjs', '.ts', '.tsx', '.vue'],
    },
    base: './',
  }
})
