async function HomePage() {
  return (
    <div className='App'>
<div className="bg-gray-100 text-gray-800">
            {/* Banner Section */}
            <section className="bg-cover bg-center text-white py-20" style={{ backgroundImage: `url('banner.png')` }}>
                <div className="bg-black bg-opacity-50 py-16 px-4">
                    <h2 className="text-4xl font-bold mb-2">Where Rhythm Lives!</h2>
                    <p className="text-lg mb-4">Discover the world’s favorite rhythm game with millions of beatmaps, challenges, and community members!</p>
                    <a href="https://osu.ppy.sh/home/download"> <button className="bg-pink-500 text-white py-2 px-4 rounded hover:bg-pink-600 transition">Play Now</button></a>
                </div>
            </section>

            {/* About Section */}
            <section id="about" className="py-16">
                <div className="container mx-auto text-center">
                    <h2 className="text-3xl font-semibold mb-4">About osu!</h2>
                    <p className="text-lg max-w-2xl mx-auto">
                        osu! is a free-to-play rhythm game where players compete by following beats on the screen. With various game modes, players can experience music in a fun, competitive way.
                    </p>
                </div>
            </section>

            {/* Top Beatmaps Section */}
            <section id="beatmaps" className="py-16 bg-gray-50">
                <div className="container mx-auto text-center">
                    <h2 className="text-3xl font-semibold mb-8">Top Beatmaps</h2>
                    <div className="flex flex-wrap justify-center gap-8">
                        {['Popular Beatmap #1', 'Trending Beatmap #2', 'Challenging Beatmap #3'].map((title, index) => (
                            <div key={index} className="bg-white shadow-lg rounded-lg overflow-hidden w-72">
                                <img src={`beatmap${index + 1}.jpg`} alt={title} className="w-full h-40 object-cover" />
                                <div className="p-4">
                                    <h3 className="text-xl font-semibold mb-2">{title}</h3>
                                    <p className="text-gray-600">Difficulty: ★★★★☆</p>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </section>

            {/* Community Section */}
            <section id="community" className="py-16">
                <div className="container mx-auto text-center">
                    <h2 className="text-3xl font-semibold mb-4">Community Showcase</h2>
                    <p className="text-lg max-w-2xl mx-auto">
                        Join the osu! community and see recent plays, featured creators, and popular discussions!
                    </p>
                </div>
            </section>

            {/* News Section */}
            <section id="news" className="py-16 bg-gray-50">
                <div className="container mx-auto text-center">
                    <h2 className="text-3xl font-semibold mb-8">Latest News & Updates</h2>
                    <div className="flex flex-wrap justify-center gap-8">
                        {[
                            { title: 'Update 1.5 Released!', description: 'New features and improvements have arrived. Check out the latest patch notes!' },
                            { title: 'Community Event: osu! World Cup', description: 'Join players from around the world in the annual osu! World Cup!' }
                        ].map((news, index) => (
                            <div key={index} className="bg-white shadow-lg rounded-lg p-6 w-72">
                                <h3 className="text-xl font-semibold mb-2">{news.title}</h3>
                                <p className="text-gray-600">{news.description}</p>
                            </div>
                        ))}
                    </div>
                </div>
            </section>

            {/* Footer */}
            <footer className="bg-gray-800 text-white py-6">
                <div className="container mx-auto text-center">
                    <p>&copy; 2024 osu! | <a href="https://osu.ppy.sh/home/support" className="text-pink-400 hover:underline">Support</a></p>
                    <div className="space-x-4 mt-4">
                        <a href="https://twitter.com/osugame?ref_src=twsrc%5Egoogle%7Ctwcamp%5Eserp%7Ctwgr%5Eauthor" className="text-pink-400 hover:underline">Twitter</a>
                        <a href="https://www.youtube.com/@osugame" className="text-pink-400 hover:underline">YouTube</a>
                        <a href="https://discord.com/invite/ppy" className="text-pink-400 hover:underline">Discord</a>
                    </div>
                </div>
            </footer>
        </div>
        </div>
    );    
}
export default  HomePage;