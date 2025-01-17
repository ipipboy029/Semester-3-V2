import { test, expect } from '@playwright/test';


  const baseUrl = 'http://localhost:3000';
  const apiUrl = 'https://localhost:7237/login';
// Define the test suite
test.describe('Login Component Tests', () => {
  // Helper function to login before each test
  const login = async (page) => {
    // Navigate to the login page
    await page.goto(`${baseUrl}/login`);

    // Fill in the email and password fields
    await page.fill('input[name="email"]', 'test@example.com');
    await page.fill('input[name="password"]', 'password123');

    // Intercept the login API request and mock a successful response
    await page.route(apiUrl, (route) => {
      route.fulfill({
        status: 200,
        body: JSON.stringify({ token: 'mocked-jwt-token' }),
        headers: { 'Content-Type': 'application/json' },
      });
    });

    // Click the Sign In button
    await page.click('button[type="submit"]');

    // Wait for the page to be redirected to the home page after login
    await page.waitForNavigation();
  };

  // Test for successful login
  test('should login successfully with valid credentials', async ({ page }) => {
    await login(page); // Perform login first

    // Verify if the page is redirected to the home page
    await expect(page).toHaveURL(`${baseUrl}/`);
  });

  // Test for invalid login
  test('should display an error message for invalid login', async ({ page }) => {
    // Navigate to the login page
    await page.goto(`${baseUrl}/login`);

    // Fill in the email and password fields with invalid credentials
    await page.fill('input[name="email"]', 'wrong@example.com');
    await page.fill('input[name="password"]', 'wrongpassword');

    // Intercept the login API request and mock a failed response
    await page.route(apiUrl, (route) => {
      route.fulfill({
        status: 401,
        body: JSON.stringify({ message: 'Invalid email or password' }),
        headers: { 'Content-Type': 'application/json' },
      });
    });

    // Click the Sign In button
    await page.click('button[type="submit"]');

    // Verify the error message is displayed
    const errorMessage = await page.locator('.text-red-500');
    await expect(errorMessage).toHaveText('Invalid email or password');
  });
});
test('should display an error message for empty email field', async ({ page }) => {
  await page.goto(`${baseUrl}/login`);
  
  // Leave the email field empty and fill in the password
  await page.fill('input[name="password"]', 'password123');

  // Intercept the login API request and mock a response
  await page.route(apiUrl, (route) => {
    route.fulfill({
      status: 400,
      body: JSON.stringify({ message: 'Email is required' }),
      headers: { 'Content-Type': 'application/json' },
    });
  });

  // Click the Sign In button
  await page.click('button[type="submit"]');

  // Verify the error message is displayed
  const errorMessage = await page.locator('.text-red-500');
  await errorMessage.waitFor({ state: 'visible', timeout: 5000 });

  // Verify the error message is displayed
  await expect(errorMessage).toHaveText('Email is required');
});

test('should display an error message for empty password field', async ({ page }) => {
  await page.goto(`${baseUrl}/login`);
  
  // Fill in the email field and leave the password empty
  await page.fill('input[name="email"]', 'test@example.com');
  await page.fill('input[name="password"]', '');

  // Intercept the login API request and mock a response
  await page.route(apiUrl, (route) => {
    route.fulfill({
      status: 400,
      body: JSON.stringify({ message: 'Password is required' }),
      headers: { 'Content-Type': 'application/json' },
    });
  });

  // Click the Sign In button
  await page.click('button[type="submit"]');

  // Verify the error message is displayed
  const errorMessage = await page.locator('.text-red-500');
  await errorMessage.waitFor({ state: 'visible', timeout: 5000 }); // Wait for the error message to be visible

  // Verify the error message is displayed
  await expect(errorMessage).toHaveText('Password is required');
});
