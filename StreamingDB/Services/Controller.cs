using Serilog;
using Serilog.Formatting.Compact;
using StreamingDB.Classes;
using StreamingDB.Context;
using StreamingDB.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace StreamingDB.Services
{
    /// <summary>
    /// Сlass in which the business logic is described.
    /// For interacting with BD uses repositories.
    /// </summary>
    public class Controller
    {
        public ApplicationContext db { get; private set; }
        private AlbumRepository album_rp;
        private TrackRepository track_rp;
        private ArtistRepository artist_rp;
        private ChartRepository chart_rp;

        /// <summary>
        /// Constructor for class Controller.
        /// Creates a class ApplicationContext and connects 
        /// to the database using the path stored in the .json file.
        /// Creates repositories.
        /// </summary>
        public Controller()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            var options = optionsBuilder.UseSqlite(connectionString).Options;

            db = new ApplicationContext(options);
            album_rp = new(db);
            track_rp = new(db);
            artist_rp = new(db);
            chart_rp = new(db);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(new CompactJsonFormatter(), "log.json")
                .CreateLogger();
        }

        /// <summary>
        /// Creates new Track with several artists.
        /// Artists must already be in the database.
        /// </summary>
        /// <param name="trackName">Track name</param>
        /// <param name="artistNames">List of artists name</param>
        /// <example>
        /// <code>
        /// Controller.AddTrack("Met Me At Our Spot", new List<Artist>(){ "WILLOW", "Tyler Cole" });
        /// </code>
        /// Added Track: Met Me At Our Spot by WILLOW feat. Tyler Cole
        /// </example>
        public void AddTrack(string trackName, List<string> artistNames)
        {
            List<Artist> artists = new();
            foreach (var name in artistNames)
            {
                artists.Add(artist_rp.Find(name));
            }
            Track newTrack = new Track { Name = trackName, Artists = artists };

            track_rp.Create(newTrack);

            Log.Logger.Information("A new track has been added: {@NewTrack}", newTrack);
        }

        /// <summary>
        /// Creates new Track with one artist.
        /// Artist must already be in the database.
        /// </summary>
        /// <param name="trackName">Track name</param>
        /// <param name="artistName">Artist name</param>
        /// <example>
        /// <code>
        /// Controller.AddTrack("Bonfire", "Knife Party");
        /// </code>
        /// Added Track: Bonfire by Knife Party
        /// </example>
        public void AddTrack(string trackName, string artistName)
        {
            Artist artist = artist_rp.Find(artistName);
            Track newTrack = new Track {
                Name = trackName,
                Artists = new List<Artist>() { artist }
            };

            track_rp.Create(newTrack);

            Log.Logger.Information("A new track has been added: {@NewTrack}", newTrack);
        }

        /// <summary>
        /// Deletes track from database.
        /// </summary>
        /// <param name="trackName">Track name</param>
        public void DeleteTrack(string trackName)
        {
            Track trackToDelete = track_rp.Find(trackName);
            track_rp.Remove(trackToDelete);

            Log.Logger.Information("The track has been removed: {@Track}", trackToDelete);
        }

        /// <summary>
        /// Creates new Album and also creates all tracks from album.
        /// Artist must already be in the database.
        /// </summary>
        /// <param name="artistName">Artist name</param>
        /// <param name="albumName">Album name</param>
        /// <param name="trackList">List of the tracks</param>
        /// <example>
        /// <code>
        /// List<string> trackList = new(){ "Track1", "Track2", "Track3" };
        /// Controller.AddAlbum("2Pac", "Better Dayz", trackList);
        /// </code>
        /// </example>
        public void AddAlbum(string artistName, string albumName, List<string> trackList)
        {
            Artist artist = artist_rp.Find(artistName);
            Album newAlbum = new Album { Name = albumName, Artist = artist };

            foreach (var name in trackList)
            {
                Track newTrack = new Track
                {
                    Name = name,
                    Artists = new List<Artist>() { artist }
                };
                newAlbum.Tracks.Add(newTrack);
            }

            album_rp.Create(newAlbum);

            Log.Logger.Information("A new album has been added: {@NewAlbum}", newAlbum);
        }

        /// <summary>
        /// Deletes album and all tracks from album from database.
        /// </summary>
        /// <param name="albumName">Album name</param>
        public void DeleteAlbum(string albumName)
        {
            Album album = album_rp.Find(albumName);

            int amountOfTracks = album.Tracks.Count;

            for (int i = 0; i < amountOfTracks; i++)
            {
                track_rp.Remove(album.Tracks[0]);
            }

            album_rp.Remove(album);

            Log.Logger.Information("The album has been removed: {@Album}", album);
        }

        /// <summary>
        /// Adds Artist to database.
        /// </summary>
        /// <param name="artistName">Artist name</param>
        /// <example>
        /// <code>Controller.AddArtist("Drake");</code>
        /// </example>
        public void AddArtist(string artistName)
        {
            Artist newArtist = new Artist { Name = artistName };
            artist_rp.Create(newArtist);

            Log.Logger.Information("A new artist has been added: {@NewArtist}", newArtist);
        }

        /// <summary>
        /// Deletes artist, all his albums, all his singles(track without album)
        /// from database.
        /// </summary>
        /// <param name="artistName"></param>
        public void DeleteArtist(string artistName)
        {
            Artist artist = artist_rp.Find(artistName);
            int amountOfAlbums = artist.Albums.Count;

            for (int i = 0; i < amountOfAlbums; i++)
            {
                int amountOfTracks = artist.Albums[0].Tracks.Count;

                for (int j = 0; j < amountOfTracks; j++)
                {
                    track_rp.Remove(artist.Albums[0].Tracks[0]);
                }

                album_rp.Remove(artist.Albums[0]);
            }

            int amountOfRemainingTracks = artist.Tracks.Count;

            for (int i = 0; i < amountOfRemainingTracks; i++)
            {
                track_rp.Remove(artist.Tracks[0]);
            }

            artist_rp.Remove(artist);

            Log.Logger.Information("The artist has been removed: {@Artist}", artist);
        }

        /// <summary>
        /// Gets the most streamed tracks by an artistю
        /// </summary>
        /// <param name="artistName">Artist name</param>
        /// <returns>List of tracks</returns>
        public List<Track> GetArtistTopTracks(string artistName)
        {
            Artist artist = artist_rp.Find(artistName);

            return artist.Tracks.OrderByDescending(t => t.Plays).ToList();
        }

        public List<Track> GetAllTracks()
        {
            return track_rp.GetAll();
        }
        public List<Album> GetAllAlbums()
        {
            return album_rp.GetAll();
        }

        public List<Artist> GetAllArtists()
        {
            return artist_rp.GetAll();
        }

        /// <summary>
        /// Finds Artist by name
        /// </summary>
        /// <param name="artistName">Artist name</param>
        /// <returns>Reference to artist</returns>
        public Artist GetArtist(string artistName)
        {
            return artist_rp.Find(artistName);
        }

        /// <summary>
        /// Finds Album by name
        /// </summary>
        /// <param name="albumName">Album name</param>
        /// <returns>Reference to album</returns>
        public Album GetAlbum(string albumName)
        {
            return album_rp.Find(albumName);
        }

        /// <summary>
        /// Finds Track by name
        /// </summary>
        /// <param name="trackName">Track name</param>
        /// <returns>Reference to track</returns>
        public Track GetTrack(string trackName)
        {
            return track_rp.Find(trackName);
        }

        /// <summary>
        /// The track chart is updated depending on their plays.
        /// The more plays, the higher the place in the chat
        /// </summary>
        /// <param name="chartSize">Size of the Chart</param>
        public void UpdateChart(int chartSize)
        {
            chart_rp.UpdateChart(chartSize);

            Log.Logger.Information("Chart has been updated");
        }

        /// <summary>
        /// Gets tracks from the chart
        /// </summary>
        /// <returns>List of the tracks</returns>
        public List<TopSong> GetChartTracks()
        {
            return chart_rp.GetTopTracks();
        }

        /// <summary>
        /// Finds tracks with similar to query names.
        /// </summary>
        /// <param name="query">string</param>
        /// <returns>List of the tarcks</returns>
        /// <example>
        /// <code>Controller.FindSimilar("Big")</code>
        /// Output:
        /// Big Shark by Russ Millions
        /// Big by Rita Ora feat. David Guetta
        /// </example>
        public List<Track> FindSimilar(string query)
        {
            return track_rp.FindSimilar(query);
        }

        /// <summary>
        /// Adds 1 play to track, album(if track in album) and artist
        /// </summary>
        /// <param name="item">Reference to track</param>
        public void PlayTrack(Track item)
        {
            track_rp.Play(item);
        }
    }
}
