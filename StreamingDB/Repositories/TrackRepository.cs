using Microsoft.EntityFrameworkCore;
using StreamingDB.Classes;
using StreamingDB.Context;

namespace StreamingDB.Repositories
{
    public class TrackRepository : IRepository<Track>
    {
        private ApplicationContext db;

        /// <summary>
        /// Constructor of TrackRepository
        /// </summary>
        /// <param name="db">Database context inherited from DbContext</param>
        public TrackRepository(ApplicationContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Creates new Track and inserts it into database
        /// </summary>
        /// <param name="item">Reference to new Track</param>
        public void Create(Track item)
        {
            db.Tracks.Add(item);
            db.SaveChanges();
        }

        /// <summary>
        /// Finds Track by Id
        /// </summary>
        /// <param name="id">Track Id</param>
        /// <returns>Reference to created Track</returns>
        /// <exception cref="NullReferenceException">If Track was not found</exception>
        public Track Find(int id)
        {
            Track? toReturn = db.Tracks
                .Include(t => t.Artists)
                .Include(t => t.Album)
                .FirstOrDefault(t => t.Id == id);

            if (toReturn is null)
                throw new NullReferenceException("Track not found");
            else
                return toReturn;
        }

        /// <summary>
        /// Finds Track by name
        /// </summary>
        /// <param name="name">Track name</param>
        /// <returns>Reference to created Track</returns>
        /// <exception cref="NullReferenceException">If Track was not found</exception>
        public Track Find(string name)
        {
            Track? toReturn = db.Tracks
                .Include(t => t.Artists)
                .Include(t => t.Album)
                .FirstOrDefault(t => t.Name == name);

            if (toReturn is null)
                throw new NullReferenceException("Track not found");
            else
                return toReturn;
        }

        /// <summary>
        /// Finds tracks with similar names to input string
        /// </summary>
        /// <param name="name">track name</param>
        /// <returns>List of Tracks</returns>
        public List<Track> FindSimilar(string name)
        {
            string query = "%" + name + "%";

            var tracks = db.Tracks
                .Include(t => t.Artists)
                .Where(t => EF.Functions.Like(t.Name, query))
                .ToList();

            return tracks;
        }

        /// <summary>
        /// Gets list of all Tracks in database
        /// </summary>
        /// <returns>List of Tracks</returns>
        public List<Track> GetAll()
        {
            return db.Tracks
                .Include(t => t.Artists)
                .Include(t => t.Album)
                .ToList();
        }

        /// <summary>
        /// Deletes Track from database
        /// </summary>
        /// <param name="item">Reference to Track</param>
        public void Remove(Track item)
        {
            db.Tracks.Remove(item);
            db.SaveChanges();
        }

        /// <summary>
        /// Changes Track name from old to new
        /// </summary>
        /// <param name="oldName">Old name of Track</param>
        /// <param name="newName">New name of Track</param>
        public void UpdateName(string oldName, string newName)
        {
            Track toUpdate = Find(oldName);
            toUpdate.Name = newName;
            db.Tracks.Update(toUpdate);
            db.SaveChanges();
        }

        /// <summary>
        /// Adds 1 play to track, album(if track in album) and artist
        /// </summary>
        /// <param name="item">Reference to track</param>
        public void Play(Track item)
        {

            item.Plays++;

            foreach(Artist artist in item.Artists)
            {
                artist.Plays++;
            }

            if ( item.Album is not null)
            {
                item.Album.Plays++;
            }

            db.SaveChanges();

        }

    }
}
