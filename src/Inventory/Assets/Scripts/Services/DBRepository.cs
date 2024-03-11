using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class DBRepository<TEntity> : IDBRepositoy<TEntity> where TEntity : BaseEntity
{
    protected IMongoCollection<TEntity>? _collection;

    protected IMongoDatabase? _database;

    private IMongoCollection<TEntity>? Collection { get { return _collection; } }
    private IMongoDatabase? Database { get { return _database; } }

    private readonly SetInventroyResoruceType? setInventroyResoruceType;

    public DBRepository()
    {
        setInventroyResoruceType = SCOService.GetScriptableObject<SetInventroyResoruceType>();

        if (setInventroyResoruceType != null)
        {
            var client = new MongoClient(setInventroyResoruceType.ConnectionString);
            _database = client.GetDatabase(setInventroyResoruceType.DBName);
            _collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);
        }
    }

    public IQueryable<TEntity> Table => _collection.AsQueryable();


    public Task<TEntity> GetFilter(Expression<Func<TEntity, bool>> filter)
    {
        return _collection.Find(filter).FirstOrDefaultAsync();
    }

    public Task<long> GetCount(Expression<Func<TEntity, bool>> filter)
    {
        return _collection.Find(filter).CountDocumentsAsync();
    }

    public bool Exists(Expression<Func<TEntity, bool>> filter)
    {
        return GetCount(filter).GetAwaiter().GetResult() > 0;
    }

    public virtual TEntity Save(TEntity entity)
    {
        _collection.InsertOne(entity);
        return entity;
    }

    public virtual ReplaceOneResult Update(TEntity entity)
    {
        entity.UpdateDate = DateTime.UtcNow;
        return _collection.ReplaceOne(x => x.Id == entity.Id, entity);
    }

    public virtual DeleteResult Delete(string id)
    {
        return _collection.DeleteOne(e => e.Id == id);
    }
}
