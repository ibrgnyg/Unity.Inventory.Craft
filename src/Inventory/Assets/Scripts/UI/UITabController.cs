using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITabController : MonoBehaviour
{
    public PlayerController playerController;
    public TextMeshProUGUI inventorySelectText;
    public GameObject tapPanels, cross, craftModal;

    bool openInventory = false;

    private void Start()
    {
        inventorySelectText.text = "Type: " + SCOService.GetScriptableObject<SetInventroyResoruceType>().InventoryResourceType.ToString();
    }

    private void Update()
    {
        if (playerController.inputController.pressTap && !openInventory)
        {
            cross.SetActive(false);
            craftModal.SetActive(false);    
            Cursor.lockState = CursorLockMode.None;
            openInventory = true;
            playerController.canMove = false;
            playerController.inputController.pressTap = false;
        }
        else if (playerController.inputController.pressTap && openInventory)
        {
            cross.SetActive(true);
            craftModal.SetActive(true);
            openInventory = false;
            playerController.inputController.pressTap = false;
            playerController.canMove = true;
            Cursor.lockState = CursorLockMode.Locked;
        }

        tapPanels.SetActive(openInventory);

        if (playerController.TryGetComponent(out PickupItemController pickupItemController))
        {
            if (pickupItemController.anyCollectable)
            {
                cross.GetComponent<Image>().color = Color.green;
            }
            else
            {
                cross.GetComponent<Image>().color = Color.white;
            }

        }

    }
}
