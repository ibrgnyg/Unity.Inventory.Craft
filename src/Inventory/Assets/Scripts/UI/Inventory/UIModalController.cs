using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIModalController : MonoBehaviour
{
    public Slider slider;
    public Image itemImage;
    public TextMeshProUGUI itemCountText, sliderText;
    public bool activeModal = false;

    public PickupItemController pickupItemController;

    [HideInInspector] public CollectableItem collectableItem;
    [HideInInspector] public string id;

    public void SetActviveModal()
    {
        activeModal = !activeModal;

        if (!activeModal)
        {
            id = string.Empty;
            collectableItem = null;
        }

        gameObject.SetActive(activeModal);
        UpdateTextSlider();
    }

    public void UpdateTextSlider()
    {
        slider.minValue = 1;
        slider.maxValue = float.Parse(itemCountText.text);
        sliderText.text = $"{slider.maxValue}/{slider.maxValue}";
        slider.value = slider.maxValue;
    }

    public void ChangeSlider()
    {
        slider.onValueChanged.AddListener((v) =>
        {
            int intValue = Mathf.RoundToInt(v);

            sliderText.text = $"{intValue}/{slider.maxValue}";
            slider.value = intValue;
        });
    }


    public void Drop()
    {
        if (collectableItem == null)
            return;

        pickupItemController.DropItem((int)slider.value, id, collectableItem);
        gameObject.SetActive(false);
    }
}
