using System;
using System.Linq.Expressions;
using System.Linq;

public interface IInventoryService
{
    IQueryable<SlotDB> Table { get; }
    bool Update(CollectableItem model);
    bool Exist(Expression<Func<SlotDB, bool>> filter);
    SlotDB Get(Expression<Func<SlotDB, bool>> filter);
    bool Remove(int dropCount, string id, CollectableItem collectableItem);
}
