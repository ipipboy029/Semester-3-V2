"use client";  // Ensure the component is running on the client side
import { useState } from "react";
import { useRouter } from 'next/navigation';

function LoginPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const router = useRouter();

  const handleLogin = async (e) => {
    e.preventDefault();  // Prevent form submission from refreshing the page
    setIsLoading(true);   // Set loading state to true when the request is being processed
    setError('');         // Clear any previous errors

    try {
      const response = await fetch('https://localhost:7237/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password }),  // Send the email and password in the request body
      });

      setIsLoading(false); // Set loading state to false when the request finishes

      if (response.ok) {
        // If the response is successful, parse the token and store it in localStorage
        const data = await response.json();
        localStorage.setItem('jwtToken', data.token); // Store the JWT token
        
        // Redirect using router.push() for Next.js (client-side navigation)
        window.location.replace('/'); // Redirect to the home page or dashboard
      } else {
        // If response is not OK, handle errors from the API
        const errorData = await response.json();
        setError(errorData.message || 'Login failed');  // Display the error message from the API
      }
    } catch (error) {
      setIsLoading(false);  // Set loading to false when an error occurs
      setError('An error occurred while logging in');  // Display a generic error message
    }
  };

  return (
    <div className="flex min-h-full flex-col justify-center px-6 py-12 lg:px-8">
      <div className="sm:mx-auto sm:w-full sm:max-w-sm">
        <h2 className="mt-10 text-center text-2xl font-bold leading-9 tracking-tight text-gray-900">
          Sign in to your account
        </h2>
      </div>
      
      <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
        <form onSubmit={handleLogin} className="space-y-6 mt-4" action="#" method="POST">
          <div>
            <label htmlFor="email" className="block text-sm font-medium text-gray-700">Email</label>
            <div className="mt-1">
              <input
                id="email"
                name="email"
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}  // Update the email value as the user types
                autoComplete="email"
                required
                className="px-2 py-3 mt-1 block w-full rounded-md border border-gray-300 shadow-sm focus:border-black focus:outline-none sm:text-sm text-black"
              />
            </div>
          </div>

          <div>
            <label htmlFor="password" className="block text-sm font-medium text-gray-700">Password</label>
            <div className="mt-1">
              <input
                id="password"
                name="password"
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}  // Update the password value as the user types
                autoComplete="password"
                required
                className="px-2 py-3 mt-1 block w-full rounded-md border border-gray-300 shadow-sm focus:border-black focus:outline-none sm:text-sm text-black"
              />
            </div>
          </div>

          {/* Display any error messages if present */}
          {error && <div className="text-red-500 text-sm">{error}</div>}

          <div>
            <button
              type="submit"
              disabled={isLoading}  // Disable the button if the request is in progress
              className="flex w-full justify-center rounded-md bg-pink-500 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-pink-600 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2"
            >
              {isLoading ? 'Signing In...' : 'Sign In'}
            </button>
          </div>
        </form>

        <p className="mt-10 text-center text-sm text-gray-500">
          Don't have an account yet? <a href="register/" className="font-semibold leading-6 text-pink-500 hover:text-black">Sign up</a>
        </p>
      </div>
    </div>
  );
}

export default LoginPage;
