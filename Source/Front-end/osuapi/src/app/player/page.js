"use client"
import { useEffect, useState } from "react"

function PlayerPage() {
    const [data, setData] = useState(null);
    const [isLoading, setLoading] = useState(false);
    const [player, setPlayer] = useState(""); // State to store player query
    const [error, setError] = useState(""); // State for error handling
  
    const handleSearch = (event) => {
      event.preventDefault();
      setLoading(true);
      setError(""); // Reset error
  
      // Ensure the player name is not empty
      if (player.trim() === "") {
        setError("Please enter a player name or ID.");
        setLoading(false);
        return;
      }
  
      // Fetch data for the specific player
<<<<<<< HEAD
      fetch(`https://localhost:7237/Api/player/${player}/osu`)
=======
      fetch(`http://localhost:8080/Api/player/${player}/osu`)
>>>>>>> 6446dccd2dc1ca49272cbb20bd5a83637932babd
        .then((res) => res.json())
        .then((data) => {
          console.log(data); // Log the response to inspect the data
          setData(data);
          setLoading(false);
        })
        .catch((error) => {
          console.error("Error fetching data:", error);
          setError("Error fetching player data. Please try again.");
          setLoading(false);
        });
    };
  
    return (
      <div className="App bg-gray-50 min-h-screen">
        {/* Heading Section */}
        <div className="flex justify-center items-center pt-8">
          <h1 className="text-4xl font-bold text-pink-500">Search Any Player on osu!</h1>
        </div>
  
        {/* Search Box */}
        <div className="flex justify-center mt-8 px-4">
          <form className="w-full max-w-2xl" onSubmit={handleSearch}>
            <div className="relative flex items-center">
              <input
                type="search"
                id="default-search"
                className="block w-full p-4 pl-12 text-lg text-gray-900 border border-gray-300 rounded-xl bg-white shadow-md focus:ring-4 focus:ring-blue-500 focus:border-blue-500 dark:bg-white dark:border-gray-600 dark:placeholder-gray-400 dark:text-black dark:focus:ring-blue-500 dark:focus:border-blue-500"
                placeholder="Search players..."
                required
                value={player}
                onChange={(e) => setPlayer(e.target.value)} // Update player state on input change
              />
              <button
                type="submit"
                className="absolute right-3 top-3 text-white bg-pink-500 hover:bg-pink-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-xl text-sm px-6 py-3 transition-all"
              >
                Search
              </button>
              <div className="absolute inset-y-0 left-4 flex items-center pointer-events-none">
                <svg
                  className="w-5 h-5 text-gray-500"
                  aria-hidden="true"
                  fill="none"
                  viewBox="0 0 20 20"
                >
                  <path
                    stroke="currentColor"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    strokeWidth="2"
                    d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z"
                  />
                </svg>
              </div>
            </div>
          </form>
        </div>
  
        {/* Loading and Error Handling */}
        {isLoading && <p className="text-center text-black mt-4">Loading...</p>}
        {error && <p className="text-center text-red-400 mt-4">{error}</p>}
  
        {/* Player Info Display - Card Element */}
        {data && !isLoading && (
          <div className="flex justify-center mt-8 px-4">
            <div className="max-w-3xl w-full bg-white rounded-lg shadow-2xl p-8">
              <div className="flex items-center bg-purple-50 rounded-lg p-8 shadow-lg">
                <img
                  src={data.avatar_url}
                  alt={data.username}
                  className="w-64 h-48 object-contain mr-6 rounded-xl shadow-lg"
                />
                <div>
                  <h3 className="text-2xl font-semibold text-purple-900">{data.username}</h3>
                  <p className="text-lg text-gray-600 mt-2">Country: {data.country_Code}</p>
                  <p className="text-lg text-gray-600 mt-1">Global Rank: {data.userStatistics?.globalRank}</p>
                  <p className="text-lg text-gray-600 mt-1">Country Rank: {data.userStatistics?.countryRank}</p>
                </div>
              </div>
            </div>
          </div>
        )}
  
        {/* No Data Case */}
        {data && data.length === 0 && <p className="text-center text-black mt-4">No data available for this player.</p>}
      </div>
    );
}

<<<<<<< HEAD
export default PlayerPage;
=======
export default PlayerPage;
>>>>>>> 6446dccd2dc1ca49272cbb20bd5a83637932babd
