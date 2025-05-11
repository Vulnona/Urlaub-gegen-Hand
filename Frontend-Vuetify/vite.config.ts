import vue from '@vitejs/plugin-vue';
import vuetify, { transformAssetUrls } from 'vite-plugin-vuetify';
import ViteFonts from 'unplugin-fonts/vite';
import { defineConfig, loadEnv, Plugin } from 'vite';
import { fileURLToPath, URL } from 'node:url';
import axios from 'axios';

// fetch the metadata on startup, transformIndexHtml is not compatible to asynchronous fetches
let metadata = new Map();
const url = `http://172.19.0.4:8080/api/offer/get-meta-data`;
const headers = {'Content-Type': 'application/json'};
axios.get(url, { headers })
    .then(res => {
        res.data.forEach( (o) => {
            metadata.set(o.id.toString(), o);
        })

    })
    .catch(error => {
        console.error('Error:', error.message);
    });

const modifyOfferMetaPlugin = () => {
  return {
    name: 'html-transform',
      transformIndexHtml(html, ctx) {
          if(/\/offer\/\d+/.test(ctx.originalUrl)){
              let offerNum = ctx.originalUrl.substring(7);
              if (metadata.has(offerNum)) {
                  const meta = metadata.get(offerNum);
                  html = html.replace(/<meta property="og:title" (.*?)>/, `<meta property="og:title" content="${meta.title}">`);
                  html = html.replace(/<meta property="og:description" (.*?)>/, `<meta property="og:description" content="${meta.description}">`);
              }
              return html.replace(
                  /<meta property="og:image" (.*?)>/,
                  `<meta property="og:image" content="https://alreco.de:8443/api/offer/get-preview-picture/${offerNum}">`
              );
          }
    },
  }
}

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '');
  return {
    server: {
        host: '0.0.0.0',
        port: 3000,
        allowedHosts: ['alreco.de'],
        hmr: {
          clientPort: 443,
      },
    },
    optimizeDeps: {
      include: ['vue', 'vue-router', 'sweetalert2', 'crypto-js', 'axios', 'vue-toastification',
        'jwt-decode', 'lodash/debounce', 'bootstrap/dist/js/bootstrap.min.js', 'vue-multiselect',
        'crypto-js/aes', 'vue3-datepicker','@stripe/stripe-js'],
    },
    plugins: [
      vue({
        template: {
          transformAssetUrls: {
            ...transformAssetUrls,
            img: ['src'],
            image: ['xlink:href', 'href'],
            'v-img': ['src', 'lazy-src'],
            'v-carousel-item': ['src', 'lazy-src'],
          },
        },
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
        modifyOfferMetaPlugin(),
    ],
    css: {
      preprocessorOptions: {
        scss: {
          additionalData: `@use "sass:math";`,
        },
      },
    },  
    define: {
      'process.env': {
        SECRET_KEY: env.SECRET_KEY,
        baseURL: env.BASE_URL,
        baseURL_Frontend: env.BASE_URL_FRONTEND,
        claims_Url: env.CLAIMS_URL,
        StripeKey: env.STRIPE_KEY,
      },
    },
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url)),
        'images': fileURLToPath(new URL('./src/assets/images', import.meta.url)),
      },
      extensions: ['.js', '.json', '.jsx', '.mjs', '.ts', '.tsx', '.vue'],
    },
    base: '/', // Changed from './' to '/'
    build: {
      assetsDir: 'assets',
      rollupOptions: {
        output: {
          assetFileNames: (assetInfo) => {
            const info = assetInfo.name.split('.');
            const ext = info[info.length - 1];
            if (/png|jpe?g|svg|gif|tiff|bmp|ico|webp/i.test(ext)) {
              return `assets/images/[name].[hash][extname]`;
            }
            return `assets/[name].[hash][extname]`;
          },
        },
      },
    },
  };
});
