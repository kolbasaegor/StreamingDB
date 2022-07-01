using StreamingDB.Classes;
using StreamingDB.Services;

namespace ControllerTests
{
    /// <summary>
    /// Tests for buisness logic. I recomend to use another database for tests.
    /// </summary>
    public class ControllerTests
    {
        [Theory]
        [InlineData("morgen")]
        [InlineData("shtern")]
        [InlineData("bi-2")]
        public void CreateAndAddArtist(string artistName)
        {
            Controller controller = new Controller();

            controller.AddArtist(artistName);

            Assert.NotNull(controller.db.Artists.FirstOrDefault(a => a.Name == artistName));
        }

        [Theory]
        [InlineData("sueta", "oleg")]
        [InlineData("one dance", "Drake")]
        [InlineData("lil ansty", "molly")]
        public void CreateAndAddTrackWithOneArtist(string trackName, string artistName)
        {
            Controller controller = new Controller();

            controller.AddArtist(artistName);

            controller.AddTrack(trackName, artistName);
            Track? track = controller.db.Tracks.FirstOrDefault(t => t.Name == trackName);

            Assert.NotNull(track);
            Assert.Equal(artistName, track.Artists[0].Name);
        }

        [Theory]
        [InlineData("trap", "lil b", "2pac")]
        [InlineData("s", "u", "s")]
        [InlineData("among us", "bebra", "abobus")]
        public void CreateAndAddTrackWithTwoArtist(string trackName, string artist1Name, string artist2Name)
        {
            Controller controller = new Controller();

            controller.AddArtist(artist1Name);
            controller.AddArtist(artist2Name);
            var artist1 = controller.GetArtist(artist1Name);
            var artist2 = controller.GetArtist(artist2Name);

            controller.AddTrack(trackName, new List<string>() { artist1Name, artist2Name });
            Track? track = controller.db.Tracks.FirstOrDefault(t => t.Name == trackName);

            Assert.NotNull(track);
            Assert.Equal(artist1Name, track.Artists[0].Name);
            Assert.Equal(artist2Name, track.Artists[1].Name);
        }


        [Fact]
        public void CreateAndAddAlbum()
        {
            Controller controller = new Controller();
            string artistName = "Alpine";
            string albumName = "Foolish";
            List<string> tracksList = new() { "track1", "track2", "track3" };

            controller.AddArtist(artistName);
            controller.AddAlbum(artistName, albumName, tracksList);

            Album? album = controller.db.Albums.FirstOrDefault(t => t.Name == albumName);

            Assert.NotNull(album);
            Assert.Equal(albumName, album.Name);
            Assert.Equal(artistName, album.Artist.Name);

            int tt = 0;
            foreach (var t in album.Tracks)
            {
                Assert.Equal(tracksList[tt], t.Name);
                tt++;
            }

        }

        [Theory]
        [InlineData("q w e r t y")]
        [InlineData("!dagestan?")]
        [InlineData("hmmmmmm")]
        public void DeleteTrack(string trackName)
        {
            Controller controller = new Controller();

            string someArtist = "OG Succi";
            controller.AddArtist(someArtist);
            controller.AddTrack(trackName, someArtist);
            controller.DeleteTrack(trackName);

            Track? track = controller.db.Tracks.FirstOrDefault(t => t.Name == trackName);

            Assert.Null(track);
        }

        [Theory]
        [InlineData("Crazy Album")]
        [InlineData(" ")]
        [InlineData("!!!!!!!!2222")]
        public void DeleteAlbumAndAllTracksFromIt(string albumName)
        {
            Controller controller = new Controller();
            string artistName = "Ded inside";
            List<string> tracksList = new() { "track1", "track2", "track3" };

            controller.AddArtist(artistName);
            controller.AddAlbum(artistName, albumName, tracksList);
            controller.DeleteAlbum(albumName);

            Assert.Null(controller.db.Albums.FirstOrDefault(a => a.Name == albumName));
            foreach (var track in tracksList)
            {
                Assert.Null(controller.db.Tracks.FirstOrDefault(t => t.Name == track));
            }
        }

        [Fact]
        public void DeleteArtistAndAllHisWork()
        {
            Controller controller = new Controller();
            string album1Name = "Ded inside";
            string album2Name = "Ded inside Deluxe";
            string singleName = "Avito skam";
            string artistName = "mayot";
            List<string> tracksList1 = new() { "track1", "track2", "track3" };
            List<string> tracksList2 = new() { "track4", "track5", "track6" };

            controller.AddArtist(artistName);
            controller.AddAlbum(artistName, album1Name, tracksList1);
            controller.AddAlbum(artistName, album2Name, tracksList2);
            controller.AddTrack(singleName, artistName);

            controller.DeleteArtist(artistName);

            Assert.Null(controller.db.Artists.FirstOrDefault(a => a.Name == artistName));
            Assert.Null(controller.db.Albums.FirstOrDefault(a => a.Name == album1Name));
            Assert.Null(controller.db.Albums.FirstOrDefault(a => a.Name == album2Name));
            Assert.Null(controller.db.Tracks.FirstOrDefault(t => t.Name == singleName));
            foreach (var track in tracksList1)
            {
                Assert.Null(controller.db.Tracks.FirstOrDefault(t => t.Name == track));
            }
            foreach (var track in tracksList2)
            {
                Assert.Null(controller.db.Tracks.FirstOrDefault(t => t.Name == track));
            }
        }

        [Fact]
        public void UpdateChartCheck()
        {
            Controller controller = new Controller();

            var artistNames = new List<string>() { "top artist", "medium artist", "poor artist" };
            var trackNames = new List<string>() { "top1", "top2", "top3" };

            controller.AddArtist(artistNames[0]);
            controller.AddArtist(artistNames[1]);
            controller.AddArtist(artistNames[2]);

            controller.AddTrack(trackNames[0], new List<string>() { artistNames[0], artistNames[1] });
            controller.AddTrack(trackNames[1], new List<string>() { artistNames[1], artistNames[2] });
            controller.AddTrack(trackNames[2], artistNames[0]);
            var track1 = controller.GetTrack(trackNames[0]);
            var track2 = controller.GetTrack(trackNames[1]);
            var track3 = controller.GetTrack(trackNames[2]);

            for (int i = 0; i < 10; i++) controller.PlayTrack(track1);
            for (int i = 0; i < 5; i++) controller.PlayTrack(track2);
            for (int i = 0; i < 3; i++) controller.PlayTrack(track3);

            controller.UpdateChart(10);

            Assert.Equal(trackNames[0], controller.db.Chart.FirstOrDefault(t => t.Position == 1).Track.Name);
            Assert.Equal(trackNames[1], controller.db.Chart.FirstOrDefault(t => t.Position == 2).Track.Name);
            Assert.Equal(trackNames[2], controller.db.Chart.FirstOrDefault(t => t.Position == 3).Track.Name);
            Assert.Equal(10, controller.db.Chart.FirstOrDefault(t => t.Position == 1).Track.Plays);
            Assert.Equal(5, controller.db.Chart.FirstOrDefault(t => t.Position == 2).Track.Plays);
            Assert.Equal(3, controller.db.Chart.FirstOrDefault(t => t.Position == 3).Track.Plays);

        }
    }
}