import { test, expect } from '@playwright/test';

const apiUrl = 'https://localhost:7237/login';

test.describe('PostPage Flow', () => {

  test.beforeAll(async () => {
    // Setup logic if needed, such as mocking requests
  });

  test('should display the PostPage correctly when logged in', async ({ page }) => {
    // Navigate to the login page
    await page.goto('http://localhost:3000/login'); // Adjust to your login page URL

    // Log in using credentials
    await page.fill('#email', 'testuser@example.com'); // Replace with valid credentials
    await page.fill('#password', 'password123'); // Replace with valid credentials

    // Wait for the navigation to complete
    await page.route(apiUrl, (route) => {
      route.fulfill({
        status: 200,
        body: JSON.stringify({ token: 'mocked-jwt-token' }),
        headers: { 'Content-Type': 'application/json' },
      });
    });
    await page.click('button[type="submit"]');
    await page.waitForNavigation();
    // Now check that we are on the PostPage
    await page.goto('http://localhost:3000/posts'); // Adjust to your PostPage URL

    // Ensure the page title is visible
    await expect(page.locator('h1')).toContainText('Posts');

    // Wait for posts to load and check if "Recent Posts" is visible
    await expect(page.locator('h2')).toContainText('Recent Posts');

    // Ensure that posts are displayed on the page (check for post containers)
    const posts = await page.locator('.flex.items-center');
    await expect(posts).toHaveCountGreaterThan(0);

    // Ensure the "Add Post" button is visible
    await expect(page.locator('button')).toHaveText('Add Post');
  });

  test('should allow the user to add a new post', async ({ page }) => {
    // Assuming we're already logged in (as part of previous test or using fixtures for login)

    // Navigate to the PostPage
    await page.goto('http://localhost:3000/posts'); // Adjust to your PostPage URL

    // Click the "Add Post" button to reveal the form
    await page.click('button:has-text("Add Post")');

    // Fill in the form fields for the new post
    await page.fill('#postSubject', 'New Post Subject'); // Adjust with the correct field ID
    await page.fill('#postDescription', 'This is a description of the new post.'); // Adjust with the correct field ID
    await page.fill('#postImageURL', 'https://example.com/new-post-image.jpg'); // Adjust with the correct field ID

    // Submit the form (assumed submit button)
    await page.click('button[type="submit"]'); // Adjust the selector as needed

    // Wait for the new post to appear in the post list (you might need to wait for a network request)
    await expect(page.locator('text=New Post Subject')).toBeVisible();
    await expect(page.locator('text=This is a description of the new post.')).toBeVisible();
  });
});
