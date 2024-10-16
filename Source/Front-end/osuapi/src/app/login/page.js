"use client"
import { useEffect, useState } from "react"
import { useRouter } from 'next/navigation';

function LoginPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const router = useRouter();

  const handleLogin = async (e) => {
    e.preventDefault();

    const response = await fetch('https://localhost:7237/login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ email, password }),
    });

    if (response.ok) {
    const data = await response.json();
    localStorage.setItem('user', JSON.stringify(data));
        router.push('http://localhost:3000/');
      } else {
        const errorData = await response.json();
        setError(errorData.message || 'Login failed');
      }
  };
    return (
<div class="flex min-h-full flex-col justify-center px-6 py-12 lg:px-8">
  <div class="sm:mx-auto sm:w-full sm:max-w-sm">
    <h2 class="mt-10 text-center text-2xl font-bold leading-9 tracking-tight text-gray-900">Sign in to your account</h2>
  </div>
  
  <div class="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
  <form onSubmit={handleLogin} class="space-y-6 mt-4" action="#" method="POST">
                <div>
                    <label for="password" class="block text-sm font-medium text-gray-700">Email</label>
                    <div class="mt-1">
                        <input name="email" type="email-address" value={email} onChange={(e) => setEmail(e.target.value)} autocomplete="email-address" required
                            class="px-2 py-3 mt-1 block w-full rounded-md border border-gray-300 shadow-sm focus:border-black focus:outline-none sm:text-sm text-black" />
                    </div>
                </div>

                <div>
                    <label for="password" class="block text-sm font-medium text-gray-700">Password</label>
                    <div class="mt-1">
                        <input id="password" name="password" type="password" value={password} onChange={(e) => setPassword(e.target.value)} autocomplete="password" required
                            class="px-2 py-3 mt-1 block w-full rounded-md border border-gray-300 shadow-sm focus:border-black focus:outline-none sm:text-sm text-black" />
                    </div>
                </div>

                <div>
                    <button type="submit"
                        class="flex w-full justify-center rounded-md bg-pink-500 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-pink-600 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2">Sign
                        In
                    </button>
                </div>
            </form>

    <p class="mt-10 text-center text-sm text-gray-500">
      Don't have an account yet register here
      <a href="register/" class="font-semibold leading-6 text-pink-500 hover:text-black"> Sign up</a>
    </p>
  </div>
</div> 
    )
  }
  export default LoginPage;