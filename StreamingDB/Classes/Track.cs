namespace StreamingDB.Classes
{
    public class Track
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Artist> Artists { get; set; } = new();
        public Album? Album { get; set; }

        /// <summary>
        /// How many times has the track been listened to
        /// </summary>
        public int? Plays { get; set; }

        /// <summary>
        /// Converts the names of all artists to a string.
        /// </summary>
        /// <returns>String in format [ARTIST1] feat. [ARTIST2],...,[ARTISTN]</returns>
        public string ArtistsToString()
        {
            string s = Artists[0].Name;

            if (Artists.Count > 1)
            {
                s += " feat. ";
                for (int i = 1; i < Artists.Count; i++)
                {
                    s += Artists[i].Name;
                    if (i != Artists.Count - 1)
                        s += ", ";
                }
            }

            return s;
        }
    }
}
