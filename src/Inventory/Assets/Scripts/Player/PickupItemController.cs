using System.Linq;
using UnityEngine;

public class PickupItemController : MonoBehaviour
{
    public LayerMask layerMask;
    [HideInInspector] public bool anyCollectable, destroy;
    public float distance;

    RaycastHit hit;
    InputController inputController;

    public UIInventoryController _uiInventoryController;

    private IInventoryService _inventoryService;
    private InventoryServiceFile _inventoryServiceFile;
    private InventoryServiceFDB _inventoryServiceFDB;

    SetInventroyResoruceType resourceType;

    Transform camera;
    private void Awake()
    {
        camera = Camera.main.transform;
        inputController = GetComponent<InputController>();
        _inventoryService = new InventoryService();
        _inventoryServiceFile = new InventoryServiceFile();
        _inventoryServiceFDB = new InventoryServiceFDB();
        resourceType = SCOService.GetScriptableObject<SetInventroyResoruceType>();
    }

    private void LateUpdate()
    {
        anyCollectable = Physics.Raycast(camera.position, camera.forward, out hit, distance, layerMask);

        AddPickupItem();
    }

    public void AddPickupItem()
    {
        if (!anyCollectable || !inputController.pressM1)
            return;

        if (!hit.transform.TryGetComponent(out CollectItem outCollectItem))
            return;

        switch (resourceType.InventoryResourceType)
        {
            case InventoryType.LOCAL:
                _inventoryServiceFile.Update(outCollectItem.item);
                break;
            case InventoryType.MDB:
                _inventoryService.Update(outCollectItem.item);
                break;
            case InventoryType.FDB:
                _inventoryServiceFDB.UpdateItem(outCollectItem.item);
                break;
            case InventoryType.SCO:
                SCOService.GetScriptableObject<InventoryServiceSC>().AddItem(outCollectItem.item);
                break;
        }

        outCollectItem.Destroy();

        if (outCollectItem.isDestroy)
        {
            Destroy(hit.transform.gameObject);
        }

        _uiInventoryController.UpdateUISlot();
    }

    public void AddItem(CollectableItem collectableItem = null, bool reloadUI = false)
    {
        switch (resourceType.InventoryResourceType)
        {
            case InventoryType.LOCAL:
                _inventoryServiceFile.Update(collectableItem);
                break;
            case InventoryType.MDB:
                _inventoryService.Update(collectableItem);
                break;
            case InventoryType.FDB:
                _inventoryServiceFDB.UpdateItem(collectableItem);
                break;
            case InventoryType.SCO:
                SCOService.GetScriptableObject<InventoryServiceSC>().AddItem(collectableItem);
                break;
        }

        _uiInventoryController.UpdateUISlot(reloadUI);
    }

    public void DropItem(int dropCount, string id, CollectableItem collectableItem)
    {
        switch (resourceType.InventoryResourceType)
        {
            case InventoryType.LOCAL:
                _inventoryServiceFile.Remove(dropCount, id, collectableItem);
                break;
            case InventoryType.MDB:
                _inventoryService.Remove(dropCount, id, collectableItem);
                break;
            case InventoryType.FDB:
                break;
            case InventoryType.SCO:
                SCOService.GetScriptableObject<InventoryServiceSC>().Remove(dropCount, id, collectableItem);
                break;
        }

        DropItemInstantiate(dropCount, collectableItem.CollectablItemType);
        _uiInventoryController.UpdateUISlot(true);
    }

    private void DropItemInstantiate(int dropCount, CollectablItemType collectablItemType)
    {
        var getCollectableItem = SCOService.GetAllScriptableObjects<CollectableItem>().Where(x => x.CollectablItemType == collectablItemType).FirstOrDefault();

        if (getCollectableItem == null)
            return;

        if (dropCount < 5)
        {
            for (int i = 0; i < dropCount; i++)
            {
                GameObject created = Instantiate(getCollectableItem.Prefab, transform.position + Vector3.forward * 1 * i, Quaternion.identity);

                created.GetComponent<CollectItem>().isDestroy = true;
                created.GetComponent<Rigidbody>().AddForce(transform.forward * 0.3f, ForceMode.Impulse);
                created.transform.rotation = getCollectableItem.Prefab.transform.rotation;
            }
        }
        else
        {
            GameObject created = Instantiate(getCollectableItem.Prefab, transform.position + Vector3.forward * 1f, Quaternion.identity);
            created.GetComponent<CollectItem>().isDestroy = true;
            created.GetComponent<Rigidbody>().AddForce(transform.forward * 0.3f, ForceMode.Impulse);
        }
    }
}
