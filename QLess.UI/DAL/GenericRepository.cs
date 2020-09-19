using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using QLess.Model;

namespace QLess.DAL
{
    public class GenericRepository : IRepository, IDisposable
    {
        public QLessEntities Db { get; private set; }
        public GenericRepository(QLessEntities db)
        {
            Db = db;
        }
        public IQueryable<T> Get<T>() where T : class
        {
            return Db.Set<T>();
        }
        public T Find<T>(object id) where T : class
        {
            return Db.Set<T>().Find(id);
        }
        public async Task<T> FindAsync<T>(object id) where T : class
        {
            return await Db.Set<T>().FindAsync(id);
        }
        public void Add<T>(T item) where T : class
        {
            Db.Set<T>().Add(item);
        }
        public void Update<T>(T item) where T : class
        {
            Db.Entry(item).State = EntityState.Modified;
        }
        public void Remove<T>(T item) where T : class
        {
            Db.Set<T>().Remove(item);
        }
        public void Save()
        {
            if (Db.ChangeTracker.HasChanges())
                Db.SaveChanges();
        }
        public async Task<int> SaveAsync()
        {
            return await Db.SaveChangesAsync();
        }

        public DbRawSqlQuery<T> SqlQuery<T>(string query, params object[] parameters)
        {
            return Db.Database.SqlQuery<T>(query, parameters);
        }
        #region Dispose
        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                if (disposing)
                {
                    if (Db != null)
                    {
                        Db.Dispose();
                        Db = null;
                    }
                }
            }
        }

        IQueryable<T> IRepository.Get<T>()
        {
            throw new NotImplementedException();
        }

        T IRepository.Find<T>(object id)
        {
            throw new NotImplementedException();
        }

        Task<T> IRepository.FindAsync<T>(object id)
        {
            throw new NotImplementedException();
        }

        void IRepository.Add<T>(T item)
        {
            throw new NotImplementedException();
        }

        void IRepository.Update<T>(T item)
        {
            throw new NotImplementedException();
        }

        void IRepository.Remove<T>(T item)
        {
            throw new NotImplementedException();
        }

        void IRepository.Save()
        {
            throw new NotImplementedException();
        }

        Task<int> IRepository.SaveAsync()
        {
            throw new NotImplementedException();
        }

        DbRawSqlQuery<T> IRepository.SqlQuery<T>(string query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        ~GenericRepository()
        {
            Dispose(false);
        }
        #endregion
    }

    public  class GenericRepository<T> : IRepository<T>, IDisposable where T : class
    {
        private QLessEntities _db;

        public GenericRepository(QLessEntities db)
        {
            _db = db;
        }

        public IQueryable<T> Get()
        {
            return _db.Set<T>();
        }

        public T Find(object id)
        {
            return _db.Set<T>().Find(id);
        }
        public async Task<T> FindAsync(object id)
        {
            return await _db.Set<T>().FindAsync(id);
        }
        public void Add(T item)
        {
            _db.Set<T>().Add(item);
        }
        public void Remove(T item)
        {
            _db.Set<T>().Remove(item);
        }
        public void Update(T item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Save()
        {
            _db.SaveChanges();
        }
        public async Task<int> SaveAsync()
        {
            return await _db.SaveChangesAsync();
        }


        public DbRawSqlQuery<T> SqlQuery<T>(string query, params object[] parameters)
        {
            return _db.Database.SqlQuery<T>(query, parameters);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
            }
        }
    }
}