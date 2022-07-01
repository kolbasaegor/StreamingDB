using Microsoft.EntityFrameworkCore;
using StreamingDB.Classes;
using StreamingDB.Context;

namespace StreamingDB.Repositories
{
    public class ArtistRepository : IRepository<Artist>
    {
        private ApplicationContext db;

        /// <summary>
        /// Constructor of ArtistRepository
        /// </summary>
        /// <param name="db">Database context inherited from DbContext</param>
        public ArtistRepository(ApplicationContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Creates new Artist and inserts it into database
        /// </summary>
        /// <param name="item">Reference to new Artist</param>
        public void Create(Artist item)
        {
            db.Artists.Add(item);
            db.SaveChanges();
        }

        /// <summary>
        /// Finds Artist by Id
        /// </summary>
        /// <param name="id">Artist Id</param>
        /// <returns>Reference to created Artist</returns>
        /// <exception cref="NullReferenceException">If artist was not found</exception>
        public Artist Find(int id)
        {
            Artist? toReturn = db.Artists
                .Include(a => a.Tracks)
                .Include(a => a.Albums)
                .FirstOrDefault(a => a.Id == id);

            if (toReturn is null)
                throw new NullReferenceException("Artist not found");
            else
                return toReturn;
        }

        /// <summary>
        /// Finds Artist by name
        /// </summary>
        /// <param name="name">Artist name</param>
        /// <returns>Reference to created Artist</returns>
        /// <exception cref="NullReferenceException">If artist was not found</exception>
        public Artist Find(string name)
        {
            Artist? toReturn = db.Artists
                .Include(a => a.Tracks)
                .Include(a => a.Albums)
                .FirstOrDefault(a => a.Name == name);

            if (toReturn is null)
                throw new NullReferenceException("Artist not found");
            else
                return toReturn;
        }

        /// <summary>
        /// Gets list of all artists in database
        /// </summary>
        /// <returns>List of artists</returns>
        public List<Artist> GetAll()
        {
            return db.Artists
                .Include(a => a.Tracks)
                .Include(a => a.Albums)
                .ToList();
        }

        /// <summary>
        /// Deletes artist from database
        /// </summary>
        /// <param name="item">Reference to artist</param>
        public void Remove(Artist item)
        {
            db.Artists.Remove(item);
            db.SaveChanges();
        }

        /// <summary>
        /// Changes artist name from old to new
        /// </summary>
        /// <param name="oldName">Old name of artist</param>
        /// <param name="newName">New name of artist</param>
        public void UpdateName(string oldName, string newName)
        {
            Artist toUpdate = Find(oldName);
            toUpdate.Name = newName;
            db.Artists.Update(toUpdate);
            db.SaveChanges();
        }

    }
}
