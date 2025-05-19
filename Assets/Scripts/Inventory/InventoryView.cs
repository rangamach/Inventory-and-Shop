using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;

public class InventoryView : MonoBehaviour
{
    [Header("Controller")]
    private InventoryController inventoryController;

    [Header("Initial Data")]
    [SerializeField] private int size;
    [SerializeField] private int maxWeight;
    [SerializeField] private GameObject inventoryIconTransforms;
    [SerializeField] private GameObject shopIconTransforms;
    [SerializeField] private InventoryItem[] AllItems;
    [SerializeField] private GameObject iconPopUp;
    [SerializeField] private Sprite emptyIcon;

    [Header("Main UI")]
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private TextMeshProUGUI weightText;

    [Header("Inventory UI")]
    [SerializeField] private TextMeshProUGUI iconNameText;
    [SerializeField] private TextMeshProUGUI iconQuantityText;
    [SerializeField] private TextMeshProUGUI iconDescriptionText;
    [SerializeField] private TextMeshProUGUI iconPriceText;
    [SerializeField] private TextMeshProUGUI iconWeightText;
    [SerializeField] private TextMeshProUGUI iconRarityText;
    [SerializeField] private TextMeshProUGUI iconTypeText;

    [Header("Shop UI")]
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private Button allButton;

    [Header("Action Pop Up")]
    [SerializeField] private float durationOfActionPopUp;
    [SerializeField] private GameObject actionPopUp;

    [Header("Audio Manager")]
    [SerializeField] private AudioManager audioManager;
    

    void Start() => inventoryController = new InventoryController(this, size, maxWeight,AllItems); 
    public void SetBalanceText(int balance) => balanceText.text = $"Gold: {balance}";
    public void SetWeightText(int weight) => weightText.text = $"Weight: {weight} / {maxWeight}";
    public void SetIcon(InventoryItem item, int i)
    {
        if (item == null) return;

        GameObject icon = inventoryIconTransforms.transform.GetChild(i).gameObject;

        icon.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = item.ItemIcon;
        icon.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = item.ItemQuantity.ToString();
    }
    public void GatherResources() => inventoryController.AddResourcesInInventory(AllItems);
    public void InventoryIconButtonEvent()
    {
        audioManager.PlaySoundEffect(AudioType.ButtonClick);

        int i;
        for (i = 0; i < inventoryIconTransforms.transform.childCount; i++)
        {
            Button iconButton = inventoryIconTransforms.transform.GetChild(i).GetComponent<Button>();
            iconButton.onClick.RemoveAllListeners();
            int index = i;
            iconButton.onClick.AddListener(() => OnInventoryIconClicked(index));
        }
    }
    public void ShopIconButtonEvent()
    {
        audioManager.PlaySoundEffect(AudioType.ButtonClick);

        int i;
        for (i = 0; i < shopIconTransforms.transform.childCount; i++)
        {
            Button iconButton = shopIconTransforms.transform.GetChild(i).GetComponent<Button>();
            iconButton.onClick.RemoveAllListeners();
            int index = i;
            iconButton.onClick.AddListener(() => OnShopIconClicked(index));
        }
    }
    public void SellItem() => inventoryController.SellItem(inventoryIconTransforms, emptyIcon, actionPopUp);
    public void StartActionPopUpCoroutine() => StartCoroutine(PopUpDuration());
    private IEnumerator PopUpDuration()
    {
        actionPopUp.SetActive(true);
        yield return new WaitForSeconds(durationOfActionPopUp);
        actionPopUp.SetActive(false);
    }
    private void OnInventoryIconClicked(int i) => inventoryController.InventoryPopUpIcon(i, iconPopUp,iconNameText,iconQuantityText,iconDescriptionText,iconPriceText,iconWeightText,iconRarityText,iconTypeText);
    private void OnShopIconClicked(int i) => inventoryController.ShopPopUpIcon(i,actionPopUp);
    public void RefreshShopItems() => inventoryController.RefreshShopItems(shopIconTransforms);
    public void BuyItem() => inventoryController.BuyItem(inventoryIconTransforms,shopIconTransforms,emptyIcon);
    public void ToggleShopItems(string type) => inventoryController.ToggleShopItems(shopIconTransforms,type,emptyIcon);
}
