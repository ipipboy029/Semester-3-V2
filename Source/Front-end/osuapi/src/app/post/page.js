"use client";
import { useEffect, useState } from "react";
import AddPost from "../components/NewPost";

function PostPage() {
  const [data, setData] = useState([]); // Initialize as empty array
  const [isLoading, setLoading] = useState(true);
  const [showAddPost, setShowAddPost] = useState(false);
  const [error, setError] = useState(null); // For handling errors

  // Fetch posts only once on component mount
  const fetchPosts = () => {
    fetch("https://localhost:7237/Post") // Replace with your actual API endpoint for fetching posts
      .then((res) => res.json())
      .then((data) => {
        setData(data);
        setLoading(false);
      })
      .catch((err) => {
        console.error("Error fetching posts:", err);
        setError("Failed to load posts. Please try again later.");
        setLoading(false);
      });
  };

  // Only fetch posts once when the component mounts
  useEffect(() => {
    fetchPosts();
  }, []);

  const handlePostAdded = (newPost) => {
    // Optionally, you could refetch the posts here
    fetchPosts(); // Fetch posts again after adding a new post
  };

  if (isLoading) return <p>Loading...</p>;
  if (error) return <p>{error}</p>; // Show error message if something goes wrong
  if (!data || data.length === 0) return <p>No posts available.</p>;

  return (
    <div className="min-h-screen bg-gray-100">
      <div className="flex justify-center items-center pt-10">
        <h1 className="text-4xl font-bold text-pink-500">Posts</h1>
      </div>

      <div className="box-inner max-w-3xl mx-auto mt-8">
        <div className="boxbar bg-white rounded-lg shadow-xl p-6">
          <div className="flex justify-between items-center mb-4">
            <h2 className="text-2xl font-semibold text-pink-400">Recent Posts</h2>
            <button
              onClick={() => setShowAddPost(!showAddPost)}
              className="text-sm text-white bg-pink-500 hover:bg-pink-600 px-3 py-1 rounded-lg"
            >
              {showAddPost ? "Cancel" : "Add Post"}
            </button>
          </div>

          {showAddPost && <AddPost onPostAdded={handlePostAdded} />}

          <div className="space-y-6 mt-6">
            {data.map((post) => (
              <div key={post.postNumber} className="flex items-center bg-purple-50 rounded-lg p-6 shadow-lg">
                <img
                  src={post.imageURL}
                  alt={post.subject}
                  className="w-56 h-40 object-contain mr-6 rounded-lg"
                />
                <div>
                  <h3 className="text-xl font-semibold text-purple-900">{post.subject}</h3>
                  <p className="text-sm text-gray-600">Post ID: {post.postNumber}</p>
                  <p className="text-base text-gray-800 mt-3">{post.description}</p>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}

export default PostPage;
