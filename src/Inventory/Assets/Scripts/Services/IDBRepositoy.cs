using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;

public interface IDBRepositoy<TEntity> where TEntity : BaseEntity
{
    IQueryable<TEntity> Table { get; }

    Task<TEntity> GetFilter(Expression<Func<TEntity, bool>> filter);

    Task<long> GetCount(Expression<Func<TEntity, bool>> filter);

    bool Exists(Expression<Func<TEntity, bool>> filter);
    TEntity Save(TEntity entity);

    ReplaceOneResult Update(TEntity entity);

    DeleteResult Delete(string id);
}
