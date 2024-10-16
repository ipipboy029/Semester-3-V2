"use client"
import { useEffect, useState } from "react"
import { useRouter } from 'next/navigation';
import Image from "next/image"

export default function NavBar(){
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const router = useRouter();

  useEffect(() => {
    const user = localStorage.getItem('user');
    setIsLoggedIn(!!user); 
  }, []); 

  const handleLogout = () => {
    localStorage.removeItem('user'); 
    setIsLoggedIn(false); 
    router.push('/login'); 
  };

  return(
        <header class="shadow mb-2">
        <div class="relative flex max-w-screen-xl flex-col overflow-hidden px-4 py-4 md:mx-auto md:flex-row md:items-center">
          <a href="https://osu.ppy.sh/" class="flex items-center whitespace-nowrap text-2xl font-black">
            <span class="mr-2 text-4xl text-blue-600">
            <Image src="/osu_logo.png" alt="Logo" width={100} height={50} className='object-top-center'/>  
            </span>
          </a>
          <input type="checkbox" class="peer hidden" id="navbar-open" />
          <label class="absolute top-5 right-7 cursor-pointer md:hidden" for="navbar-open">
            <span class="sr-only">Toggle Navigation</span>
          </label>
          <nav aria-label="Header Navigation" class="peer-checked:mt-8 peer-checked:max-h-56 flex max-h-0 w-full flex-col items-center justify-between overflow-hidden transition-all md:ml-24 md:max-h-full md:flex-row md:items-start">
            <ul class="flex flex-col items-center space-y-2 md:ml-auto md:flex-row md:space-y-0">
            <li class="text-gray-600 md:mr-12 hover:text-blue-600"><a href="http://localhost:3000/">Home</a></li>
              <li class="text-gray-600 md:mr-12 hover:text-blue-600"><a href="beatmaps/">Beatmaps</a></li>
              <li class="text-gray-600 md:mr-12 hover:text-blue-600"><a href="player/">Search players</a></li>
              <li class="text-gray-600 md:mr-12 hover:text-blue-600"><a href="rankings/">Rankings</a></li>
              {isLoggedIn && (<li class="text-gray-600 md:mr-12 hover:text-blue-600"><a href="post/">Posts</a></li>)}

              {isLoggedIn ? (
          <button class="bg-transparent hover:bg-pink-500 text-pink-400 font-semibold hover:text-white py-2 px-4 border border-pink-500 hover:border-transparent rounded" onClick={handleLogout}>Logout</button> // Show Logout button if user is logged in
        ) : (
          <>
            <button class="bg-transparent hover:bg-pink-500 text-pink-400 font-semibold hover:text-white py-2 px-4 border border-pink-500 hover:border-transparent rounded" onClick={() => router.push('/login')}>Login</button>  {/* Hide this if logged in */}
          </>
        )}
            </ul>
          </nav>
        </div>
      </header>
      
    )
}
