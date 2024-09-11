using System;

namespace BusinessLayer.Models
{
    public class Player
    {

        public class Global
        {
            public string Name { get; set; }
            public string Tag { get; set; }
            public string UID { get; set; }
            public string Avatar { get; set; }
            public string Platform { get; set; }
            public int Level { get; set; }
            public Rank rank { get; set; }
        }
    }
}
