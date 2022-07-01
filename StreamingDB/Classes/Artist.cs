namespace StreamingDB.Classes
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Album> Albums { get; set; } = new();
        public List<Track> Tracks { get; set; } = new();
        public int? Plays { get; set; }
    }
}