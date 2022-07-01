using StreamingDB.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamingDBController : ControllerBase
    {
        private StreamingDB.Services.Controller ctr;

        public StreamingDBController(StreamingDB.Services.Controller controller)
        {
            ctr = controller;
        }

        [HttpGet]
        [Route("GetAllTracks")]
        public ActionResult<List<Track>> GetAllTracks()
        {
            return Ok(ctr.GetAllTracks());
        }

        [HttpGet]
        [Route("GetAllAlbums")]
        public ActionResult<List<Album>> GetAllAlbums()
        {
            return Ok(ctr.GetAllAlbums());
        }

        [HttpGet]
        [Route("GetAllArtists")]
        public ActionResult<List<Artist>> GetAllArtists()
        {
            return Ok(ctr.GetAllArtists());
        }

        [HttpGet]
        [Route("GetTopTracksByArtist")]
        public ActionResult<List<Track>> GetTopTracksByArtist(string name)
        {
            return Ok(ctr.GetArtistTopTracks(name));
        }

        [HttpGet]
        [Route("GetChart")]
        public ActionResult<List<TopSong>> GetChart(int chartSize = 10)
        {
            ctr.UpdateChart(chartSize);
            return Ok(ctr.GetChartTracks());
        }

        [HttpGet]
        [Route("FindSimilar")]
        public ActionResult<List<Track>> GetSimilar(string name)
        {
            return Ok(ctr.FindSimilar(name));
        }

        [HttpGet]
        [Route("GetArtist")]
        public ActionResult<Artist> GetArtist(string name)
        {
            var artist = ctr.GetArtist(name);
            Console.WriteLine(artist.Name);
            
            return Ok(artist);
        }

        [HttpGet]
        [Route("GetAlbum")]
        public ActionResult<Album> GetAlbum(string albumName)
        {
            return Ok(ctr.GetAlbum(albumName));
        }

        [HttpGet]
        [Route("GetTrack")]
        public ActionResult<Track> GetTrack(string name)
        {
            return Ok(ctr.GetTrack(name));
        }

        [HttpPost]
        [Route("Play")]
        public ActionResult<string> Play(string name)
        {
            var track = ctr.GetTrack(name);
            ctr.PlayTrack(track);
            return Ok($"# of plays now {track.Plays}");
        }
        
        [HttpPost]
        [Route("AddArtist")]
        public ActionResult<string> AddArtist(string name)
        {
            ctr.AddArtist(name);
            return Ok($"Artist {name} have been added.");
        }

        [HttpPost]
        [Route("AddCollab")]
        public ActionResult<string> AddTrack(string trackName, List<string> artistNames)
        {
            ctr.AddTrack(trackName, artistNames);
            return Ok($"Track {trackName} have been  added.");
        }

        [HttpPost]
        [Route("AddTrack")]
        public ActionResult<string> AddTrack(string trackName, string artistName)
        {
            ctr.AddTrack(trackName, artistName);
            return Ok($"Track {trackName} have been  added.");
        }

        [HttpPost]
        [Route("AddAlbum")]
        public ActionResult<string> AddAlbum(string artistName, string albumName, List<string> trackList)
        {
            ctr.AddAlbum(artistName, albumName, trackList);
            return Ok($"Album {albumName} have been  added.");
        }

        [HttpDelete]
        [Route("DeleteTrack")]
        public ActionResult<string> DeleteTrack(string name)
        {
            ctr.DeleteTrack(name);
            return Ok($"Track {name} have been  deleted.");
        }

        [HttpDelete]
        [Route("DeleteAlbum")]
        public ActionResult<string> DeleteAlbum(string name)
        {
            ctr.DeleteAlbum(name);
            return Ok($"Album {name} have been  deleted.");
        }

        [HttpDelete]
        [Route("DeleteArtist")]
        public ActionResult<string> DeleteArtist(string name)
        {
            ctr.DeleteArtist(name);
            return Ok($"Artist {name} have been  deleted.");
        }

    }
}
