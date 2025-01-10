const { test, expect } = require('@playwright/test');

// Mock post data for testing
const mockPosts = [
  {
    postNumber: 1,
    subject: 'Mock Post 1',
    description: 'This is a description for Mock Post 1',
    imageURL: 'https://via.placeholder.com/150',
  },
  {
    postNumber: 2,
    subject: 'Mock Post 2',
    description: 'This is a description for Mock Post 2',
    imageURL: 'https://via.placeholder.com/150',
  },
];

const newPost = {
  postNumber: 3,
  subject: 'New Post',
  description: 'This is a newly added post',
  imageURL: 'https://via.placeholder.com/150',
};

// Define the test suite
test.describe('Post Page', () => {
  test('should display posts fetched from the server', async ({ page }) => {
    // Intercept the fetch request to the /Post endpoint and mock the response
    await page.route('https://localhost:7237/Post', (route) => {
      route.fulfill({
        status: 200,
        body: JSON.stringify(mockPosts),
        headers: { 'Content-Type': 'application/json' },
      });
    });

    // Navigate to the post page
    await page.goto('http://localhost:3000/posts');

    // Verify that the posts are displayed correctly
    const postTitles = await page.locator('h3');
    await expect(postTitles).toHaveCount(2); // Ensure two posts are displayed
    await expect(postTitles.nth(0)).toHaveText('Mock Post 1');
    await expect(postTitles.nth(1)).toHaveText('Mock Post 2');
  });

  test('should toggle the Add Post form', async ({ page }) => {
    // Mock the initial post fetch
    await page.route('https://localhost:7237/Post', (route) => {
      route.fulfill({
        status: 200,
        body: JSON.stringify(mockPosts),
        headers: { 'Content-Type': 'application/json' },
      });
    });

    // Navigate to the post page
    await page.goto('http://localhost:3000/posts');

    // Verify the Add Post button toggles the form
    const addPostButton = page.locator('button:has-text("Add Post")');
    await addPostButton.click();
    await expect(page.locator('form')).toBeVisible(); // Form should appear

    await addPostButton.click();
    await expect(page.locator('form')).not.toBeVisible(); // Form should disappear
  });

  test('should add a new post and display it in the list', async ({ page }) => {
    // Mock the initial post fetch
    await page.route('https://localhost:7237/Post', (route) => {
      route.fulfill({
        status: 200,
        body: JSON.stringify(mockPosts),
        headers: { 'Content-Type': 'application/json' },
      });
    });

    // Mock the POST request when adding a new post
    await page.route('https://localhost:7237/Post', (route, request) => {
      if (request.method() === 'POST') {
        expect(request.postDataJSON()).toEqual({
          subject: 'New Post',
          description: 'This is a newly added post',
          imageURL: 'https://via.placeholder.com/150',
        });
        route.fulfill({
          status: 201,
          body: JSON.stringify(newPost),
          headers: { 'Content-Type': 'application/json' },
        });
      }
    });

    // Navigate to the post page
    await page.goto('http://localhost:3000/posts');

    // Toggle the Add Post form
    const addPostButton = page.locator('button:has-text("Add Post")');
    await addPostButton.click();

    // Fill out the form
    await page.fill('input[name="subject"]', 'New Post');
    await page.fill('textarea[name="description"]', 'This is a newly added post');
    await page.fill('input[name="imageURL"]', 'https://via.placeholder.com/150');

    // Submit the form
    const submitButton = page.locator('button[type="submit"]');
    await submitButton.click();

    // Verify the new post appears at the top of the list
    const postTitles = await page.locator('h3');
    await expect(postTitles).toHaveCount(3);
    await expect(postTitles.nth(0)).toHaveText('New Post');
  });
});