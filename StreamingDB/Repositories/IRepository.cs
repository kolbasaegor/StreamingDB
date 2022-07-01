namespace StreamingDB.Repositories
{
    /// <summary>
    /// Interface for repositories
    /// </summary>
    /// <typeparam name="TEntity">Type of entity(Track, Album, etc.)</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        void Create(TEntity item);
        TEntity Find(int id);
        TEntity Find(string name);
        List<TEntity> GetAll();
        void Remove(TEntity item);
        void UpdateName(string oldName, string newName);
    }
}
