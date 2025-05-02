import { createApp } from 'vue';
import App from './App.vue';
import { registerPlugins } from './plugins';
import router from './router';

const app = createApp(App);
registerPlugins(app);
router.isReady().then(() => {
    app.mount('#app');
  });
