using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using QLess.Model;
namespace QLess.DAL
{
    
    public interface IRepository
    {
        QLessEntities Db { get; }

        IQueryable<T> Get<T>() where T : class;
        T Find<T>(object id) where T : class;
        Task<T> FindAsync<T>(object id) where T : class;
        void Add<T>(T item) where T : class;
        void Update<T>(T item) where T : class;
        void Remove<T>(T item) where T : class;
        void Save();
        Task<int> SaveAsync();
        DbRawSqlQuery<T> SqlQuery<T>(string query, params object[] parameters);
    }

    public interface IRepository<T> where T : class 
    {
        IQueryable<T> Get();
        Task<T> FindAsync(object id);
        void Add(T item);
        void Update(T item);
        void Remove(T item);

        void Save();
        Task<int> SaveAsync();
        DbRawSqlQuery<T> SqlQuery<T>(string query, params object[] parameters);
    }
}