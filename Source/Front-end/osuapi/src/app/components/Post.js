import React from "react";

const Post = ({postNumber, subject, description, imageURL}) => (
    
    <div className="grid lg:grid-cols-3 gap-10">
        <div className="card">
        <span className="font-bold">Title: {subject} | PostId: {postNumber}</span>
            <img src={imageURL} alt="image" className="w-full h32 sm:h-48 object-cover"/>
            <div className="m-4">
            </div>
        </div>
        <div className="flex flex-col">
            <span className="font-bold"><h1>Description</h1></span>
            <span>{description}</span>
        </div>

    </div>
);
export default Post;