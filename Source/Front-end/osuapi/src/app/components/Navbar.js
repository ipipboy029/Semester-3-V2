import Image from "next/image"

async function NavBar(){
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
            </ul>
          </nav>
        </div>
      </header>
      
    )
}
export default NavBar