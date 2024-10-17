async function HomePage() {
  return (
    <div className='App'>
      <div className="flex justify-center items-center pt-3">      
    </div>    
    <div className='box-inner flex justify-center align-middle'>
        <div className='boxbar text-white'>
          <h1 className='text-2xl text-black'>Welcome to Osu</h1>
        </div>
        <div className='boxcontent aspect-w-16 aspect-h-9'>
        <div class="aspect-w-16 aspect-h-9">
        <video class="h-full w-full rounded-lg aspect-w-16 aspect-h-9" controls>
      <source
        src="https://www.youtube.com/watch?v=Kqu4TUOO5IY"/>
    </video>
</div>
        </div>
      </div>
    </div>   
  )
}
export default  HomePage;