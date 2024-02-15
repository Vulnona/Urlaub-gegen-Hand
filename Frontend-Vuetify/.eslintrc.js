module.exports = {
  root: true,
  env: {
    node: true,
  },
  extends: [
    "@vue/eslint-config-typescript",
    "plugin:vue/vue3-recommended",
  ],
  rules: {
    "vue/multi-word-component-names": "off",
  },
};
