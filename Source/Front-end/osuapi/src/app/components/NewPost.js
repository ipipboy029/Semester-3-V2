import React, { useState } from 'react';

function AddPost({ onPostAdded }) {
  const [subject, setSubject] = useState('');
  const [imageURL, setImageURL] = useState('');
  const [description, setDescription] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();

    // Validation
    if (!subject || !imageURL || !description) {
      setError('Please fill in all fields.');
      return;
    }

    // Create a new post object
    const newPost = {
      subject,
      imageURL,
      description, // Add description to the post data
    };
    // Send new post to backend
    fetch('https://localhost:7237/Post', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(newPost),
    })
    .then((res) => {
      console.log('Response status:', res.status);  // Log status
      if (!res.ok) {
        throw new Error(`HTTP error! Status: ${res.status}`);
      }
      if (res.status === 204) {  // No Content response
        return null;  // or return {} depending on your backend
      }
      return res.text();  // Read the response as text
    })
    .then((text) => {
      console.log('Response text:', text);  // Log the raw response text
      const data = text ? JSON.parse(text) : {};  // Parse response only if it has content
      onPostAdded(data);  // Update the parent component with the new post
      setSubject('');
      setImageURL('');
      setDescription('');
      setError('');
    })
    
      .catch((err) => {
        console.error('Error adding post:', err);
        setError('Failed to add post.');
      });
  };

  return (
    <div className="bg-white rounded-lg shadow-xl p-6">
      <h2 className="text-2xl font-semibold text-pink-400 mb-4">Add a New Post</h2>
      {error && <p className="text-red-500 text-sm mb-4">{error}</p>}
      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label htmlFor="subject" className="block text-sm font-medium text-gray-700">Subject</label>
          <input
            type="text"
            id="subject"
            value={subject}
            onChange={(e) => setSubject(e.target.value)}
            className="w-full p-2 border border-gray-300 rounded-md text-black"
            placeholder="Enter post subject"
          />
        </div>
        <div>
          <label htmlFor="imageURL" className="block text-sm font-medium text-gray-700">Image URL</label>
          <input
            type="text"
            id="imageURL"
            value={imageURL}
            onChange={(e) => setImageURL(e.target.value)}
            className="w-full p-2 border border-gray-300 rounded-md text-black"
            placeholder="Enter image URL"
          />
        </div>
        <div>
          <label htmlFor="description" className="block text-sm font-medium text-gray-700">Description</label>
          <textarea
            id="description"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            className="w-full p-2 border border-gray-300 rounded-md text-black"
            placeholder="Enter post description"
            rows="4"
          />
        </div>
        <button
          type="submit"
          className="w-full bg-pink-500 hover:bg-pink-600 text-white py-2 rounded-lg"
        >
          Add Post
        </button>
      </form>
    </div>
  );
}

export default AddPost;
