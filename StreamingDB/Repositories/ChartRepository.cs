using Microsoft.EntityFrameworkCore;
using StreamingDB.Classes;
using StreamingDB.Context;

namespace StreamingDB.Repositories
{
    public class ChartRepository
    {
        private ApplicationContext db;

        /// <summary>
        /// Constructor of ChartRepository
        /// </summary>
        /// <param name="db">Database context inherited from DbContext</param>
        public ChartRepository(ApplicationContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// The track chart is updated depending on their plays
        /// The more plays, the higher the place in the chat
        /// </summary>
        /// <param name="chartSize">Size of the Chart</param>
        /// <exception cref="ArgumentOutOfRangeException">if chartSize <= 0</exception>
        public void UpdateChart(int chartSize)
        {
            if (chartSize <= 0)
                throw new ArgumentOutOfRangeException("Chart size must be > 0");

            db.Chart.RemoveRange(db.Chart);

            List<Track> topTracks = db.Tracks.OrderByDescending(t => t.Plays).ToList();

            int limit = Math.Min(chartSize, topTracks.Count);

            for (int i = 0; i < limit; i++)
            {
                db.Chart.Add(new TopSong { Position = i + 1, Track = topTracks[i] });
            }

            db.SaveChanges();
        }

        /// <summary>
        /// Gets tracks from the chart
        /// </summary>
        /// <returns>List of the tracks</returns>
        public List<TopSong> GetTopTracks()
        {
            return db.Chart
                .Include(t => t.Track)
                .Include(t => t.Track.Artists)
                .ToList();
        }
    }
}
