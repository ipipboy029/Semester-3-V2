import { test, expect } from '@playwright/test';

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
test.describe('Login and Post Page End-to-End Tests', () => {
  // Base URL and API endpoints
  const baseUrl = 'http://localhost:3000';
  const apiUrl = 'https://localhost:7237/login';
  const postsApiUrl = 'https://localhost:7237/Post';

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

  test('should add a new post and display it in the list', async ({ page }) => {
    await login(page); // Perform login first
    
    // Mock the initial post fetch
    await page.route(postsApiUrl, (route) => {
      route.fulfill({
        status: 200,
        body: JSON.stringify(mockPosts),
        headers: { 'Content-Type': 'application/json' },
      });
    });
  
    // Mock the POST request when adding a new post
    await page.route(postsApiUrl, (route, request) => {
      if (request.method() === 'POST') {
        const newPostData = {
          subject: 'New Post',
          description: 'This is a newly added post',
          imageURL: 'https://via.placeholder.com/150',
        };
  
        // Log the request data to debug
        console.log('POST Request Data:', request.postDataJSON());
  
        // Ensure the request matches what we expect
        expect(request.postDataJSON()).toEqual(newPostData);
  
        // Mock the response as if the post was created
        route.fulfill({
          status: 201,
          body: JSON.stringify(newPost), // Respond with the new post
          headers: { 'Content-Type': 'application/json' },
        });
      }
    });
  
    // Navigate to the posts page
    await page.goto(`${baseUrl}/post`);
  
    // Wait for the "Add Post" button to be available
    const addPostButton = page.locator('button:has-text("Add Post")');
    await addPostButton.waitFor({ state: 'attached' });
  
    // Click the "Add Post" button and ensure the form appears
    await addPostButton.click();
    const postForm = page.locator('form');
    await expect(postForm).toBeVisible();
  
    // Wait for and fill in the subject field
    const subject = page.locator('input[name="subject"]');
    await subject.waitFor({ state: 'visible' });
    await subject.click();  // Ensure focus before filling
    await subject.fill('New Post');
    
    // Log value of subject field to confirm it's filled
    console.log('Subject value after filling:', await subject.inputValue());
  
    // Wait for and fill in the description field
    const description = page.locator('textarea[name="description"]');
    await description.waitFor({ state: 'visible' });
    await description.click();  // Ensure focus before filling
    await description.fill('This is a newly added post');
  
    // Log value of description field to confirm it's filled
    console.log('Description value after filling:', await description.inputValue());
  
    // Wait for and fill in the imageURL field
    const imageURL = page.locator('input[name="imageURL"]');
    await imageURL.waitFor({ state: 'visible' });
    await imageURL.click();  // Ensure focus before filling
    await imageURL.fill('https://via.placeholder.com/150');
  
    // Log value of imageURL field to confirm it's filled
    console.log('Image URL value after filling:', await imageURL.inputValue());
  
    // Submit the form
    const submitButton = page.locator('button[type="submit"]');
    await submitButton.waitFor({ state: 'visible' });
    await submitButton.click();
  
    // Ensure the page receives the new post and updates
    await page.waitForLoadState('networkidle');
  
    // Verify that the new post is visible in the post list
    const postTitles = page.locator('h3');
    await expect(postTitles).toHaveCount(3); // 2 mock posts + 1 new post
  
    // Check that the last post is the new one
    await expect(postTitles.nth(2)).toHaveText('New Post');
  });
  
  
  // Test for toggling the Add Post form visibility
  test('should toggle the Add Post form', async ({ page }) => {
    await login(page); // Perform login first

    // Mock the initial post fetch
    await page.route(postsApiUrl, (route) => {
      route.fulfill({
        status: 200,
        body: JSON.stringify(mockPosts),
        headers: { 'Content-Type': 'application/json' },
      });
    });

    // Navigate to the post page
    await page.goto(`${baseUrl}/post`);

    // Verify the Add Post button toggles the form visibility
    const addPostButton = page.locator('button:has-text("Add Post")');

    // Click the "Add Post" button and ensure the form appears
    await addPostButton.click();
    const postForm = page.locator('form');
    await expect(postForm).toBeVisible();

    // Click the "Add Post" button again and ensure the form disappears
    await addPostButton.click();
    await expect(postForm).not.toBeVisible({ timeout: 7000 }); // Extend timeout if needed
  });
});
