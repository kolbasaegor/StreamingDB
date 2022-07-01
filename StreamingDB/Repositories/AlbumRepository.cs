using Microsoft.EntityFrameworkCore;
using StreamingDB.Classes;
using StreamingDB.Context;

namespace StreamingDB.Repositories
{
    public class AlbumRepository : IRepository<Album>
    {
        private ApplicationContext db;

        /// <summary>
        /// Constructor of AlbumRepository
        /// </summary>
        /// <param name="db">Database context inherited from DbContext</param>
        public AlbumRepository(ApplicationContext db)
        {
            this.db = db;
        }
        
        /// <summary>
        /// Creates new Album and inserts it into database
        /// </summary>
        /// <param name="item">Reference to new Album</param>
        public void Create(Album item)
        {
            db.Albums.Add(item);
            db.SaveChanges();
        }

        /// <summary>
        /// Finds Album by Id
        /// </summary>
        /// <param name="id">Album Id</param>
        /// <returns>Reference to created Album</returns>
        /// <exception cref="NullReferenceException">If album was not found</exception>
        public Album Find(int id)
        {
            Album? toReturn = db.Albums
                .Include(a => a.Tracks)
                .FirstOrDefault(a => a.Id == id);

            if (toReturn is null)
                throw new NullReferenceException("Album not found");
            else
                return toReturn;
        }

        /// <summary>
        /// Finds Album by Name
        /// </summary>
        /// <param name="name">Album name</param>
        /// <returns>Reference to created Album</returns>
        /// <exception cref="NullReferenceException">If album was not found</exception>
        public Album Find(string name)
        {
            Album? toReturn = db.Albums
                .Include(a => a.Tracks)
                .FirstOrDefault(a => a.Name == name);

            if (toReturn is null)
                throw new NullReferenceException("Album not found");
            else
                return toReturn;
        }

        /// <summary>
        /// Gets list of all albums in database
        /// </summary>
        /// <returns>List of albums</returns>
        public List<Album> GetAll()
        {
            return db.Albums
                .Include(a => a.Tracks)
                .Include(a => a.Artist)
                .ToList();
        }

        /// <summary>
        /// Deletes album from database
        /// </summary>
        /// <param name="item">Reference to album</param>
        public void Remove(Album item)
        {
            db.Albums.Remove(item);
            db.SaveChanges();
        }

        /// <summary>
        /// Changes album name from old to new
        /// </summary>
        /// <param name="oldName">Old name of album</param>
        /// <param name="newName">New name of album</param>
        public void UpdateName(string oldName, string newName)
        {
            Album toUpdate = Find(oldName);
            toUpdate.Name = newName;
            db.Albums.Update(toUpdate);
            db.SaveChanges();
        }
    }
}
