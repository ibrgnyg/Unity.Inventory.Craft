using System;
using System.Linq.Expressions;
using System.Linq;
using UnityEngine;

public class InventoryService : InvetorCheckService, IInventoryService
{
    private readonly IDBRepositoy<SlotDB> _dbRepository;

    public InventoryService()
    {
        _dbRepository = new DBRepository<SlotDB>();
    }

    public virtual IQueryable<SlotDB> Table => _dbRepository.Table;

    public virtual bool Exist(Expression<Func<SlotDB, bool>> filter) => _dbRepository.Exists(filter);

    public virtual SlotDB Get(Expression<Func<SlotDB, bool>> filter)
    {
        return _dbRepository.GetFilter(filter).ConfigureAwait(false).GetAwaiter().GetResult();
    }

    public virtual void Remove(string id)
    {
        _dbRepository.Delete(id);
    }

    public bool Remove(int dropCount, string id, CollectableItem collectableItem)
    {
        var getSlot = Get(x => x.Id == id && x.CollectableItem.CollectablItemType == collectableItem.CollectablItemType);

        if (getSlot == null)
            return false;

        var updateSlot = RemoveUpdateMap(getSlot, dropCount);

        if (updateSlot.CollectableItem == null)
        {
            _dbRepository.Delete(id);
            return true;
        }

        var updateResult = _dbRepository.Update(updateSlot);

        return updateResult.IsAcknowledged ? true : false;
    }

    public virtual bool Update(CollectableItem model)
    {
        try
        {
            if (model == null || model.CollectablItemType == null)
                return false;

            var getSlot = Get(x => x.CollectableItem.CollectablItemType == model.CollectablItemType && !x.IsFull);

            if (getSlot != null)
            {
                getSlot = ExistSlotCountAndIsFull(getSlot, model);

                var updateResult = _dbRepository.Update(UpdateMapModel(getSlot, getSlot));

                return updateResult.IsAcknowledged ? true : false;
            }

            var insertResult = _dbRepository.Save(AddMapModel(model));

            return insertResult != null ? true : false;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            return false;
        }
    }



}
