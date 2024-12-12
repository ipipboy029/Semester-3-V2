"use client";
import { useEffect, useState } from "react";

function RankingPage() {
  const [data, setData] = useState([]);
  const [isLoading, setLoading] = useState(true);

  useEffect(() => {
    fetch("https://localhost:7237/Api/performance")
      .then((res) => res.json())
      .then((data) => {
        console.log(data);  
        setData(data.ranking || []);  
        setLoading(false);
      })
      .catch((error) => {
        console.error("Error fetching data:", error);
        setLoading(false);
      });
  }, []);

  //if (isLoading) return <p>Loading...</p>;
 // if (data.length === 0) return <p>No data available</p>;

  return (
    <div className="min-h-screen bg-gray-100">
      <div className="flex justify-center items-center pt-10">
        <h1 className="text-4xl font-bold text-pink-500">Rankings</h1>
      </div>
      <div className="box-inner max-w-3xl mx-auto mt-8">
        <div className="boxbar bg-white rounded-lg shadow-xl p-6">
          <div className="space-y-6 mt-6">
            {data.map((ranking) => (
              <div key={ranking.global_rank} className="flex items-center bg-purple-50 rounded-lg p-6 shadow-lg">
                <img
                  src={ranking.user.avatar_url}
                  alt={ranking.user.username}
                  className="w-56 h-40 object-contain mr-6 rounded-lg"  
                />
                <div>
                  <h3 className="text-xl font-semibold text-purple-900">{ranking.user.username}</h3>
                  <p className="text-sm text-gray-600">Rank: {ranking.global_rank}</p>
                  <p className="text-sm text-gray-600">Country: {ranking.user.country.name}</p>
                  <p className="text-sm text-gray-600">PP: {ranking.pp}</p>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}

export default RankingPage;
