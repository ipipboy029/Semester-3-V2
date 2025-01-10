const { test, expect } = require('@playwright/test');

// Define the test suite
test.describe('Login Page', () => {
  test('should login successfully with valid credentials', async ({ page }) => {
    // Navigate to the login page
    await page.goto('http://localhost:3000/login');

    // Fill in the email and password fields
    await page.fill('input[name="email"]', 'test@example.com');
    await page.fill('input[name="password"]', 'password123');

    // Intercept the login API request and mock a successful response
    await page.route('https://localhost:7237/login', (route) => {
      route.fulfill({
        status: 200,
        body: JSON.stringify({ token: 'mocked-jwt-token' }),
        headers: { 'Content-Type': 'application/json' },
      });
    });

    // Click the Sign In button
    await page.click('button[type="submit"]');

    // Check if the page is redirected to the home page
    await expect(page).toHaveURL('http://localhost:3000/');
  });

  test('should display an error message for invalid login', async ({ page }) => {
    // Navigate to the login page
    await page.goto('http://localhost:3000/login');

    // Fill in the email and password fields with invalid credentials
    await page.fill('input[name="email"]', 'wrong@example.com');
    await page.fill('input[name="password"]', 'wrongpassword');

    // Intercept the login API request and mock a failed response
    await page.route('https://localhost:7237/login', (route) => {
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
    await expect(errorMessage).toContainText('Invalid email or password');
  });
});
