"use client"
import { useEffect, useState } from "react"

function PlayerPage() {
    const [data, setData] = useState(null) 
    const [isLoading, setLoading] = useState(true) 
    useEffect(()=> {
      fetch("https://localhost:7254/Post")
        .then((res)=> res.json())
        .then((data)=>{
          setData(data)
          setLoading(false)
        })
    },[])
    if(isLoading)return <p>loading</p>
    if(!data)return <p>no data</p>
    return (
      <div>
        <div className="flex justify-center items-center pt-10">     
        </div>    
          <div className='box-inner flex justify-center align-middle'>
            <div className='boxbar bg-purple-500'>
              <h2 className='text-black bg-purple-400'>Posts</h2>
              <p className="flex justify-end">Add post</p>
              {
                data.map((post) => (<Post key={ post.postNumber} postNumber={post.postNumber} subject={post.subject} comment={post.comment} imageURL={post.imageURL} ></Post>))
              }
            </div>
          <div className='boxcontent'>
        </div>
      </div>
    </div>   
    )
  }
  export default  PlayerPage;