export default function FooTer() {
    return(
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
    )
}