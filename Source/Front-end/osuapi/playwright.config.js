const { defineConfig } = require('@playwright/test');

module.exports = defineConfig({
  testDir: './tests', // Ensure this points to the correct directory
  testMatch: '**/*.{spec,test}.{js,ts}', // Match files like '*.spec.js' or '*.test.js'
  use: {
    baseURL: 'http://localhost:3000', // Replace with your app's URL
  },
});