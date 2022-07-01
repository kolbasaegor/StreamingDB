namespace StreamingDB.Classes
{
    public class Album
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Artist Artist { get; set; }
        public List<Track> Tracks { get; set; } = new();
        public int? Plays;
    }
}