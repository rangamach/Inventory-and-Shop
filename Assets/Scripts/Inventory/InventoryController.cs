using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController
{
    private InventoryModel inventoryModel;
    private InventoryView inventoryView;

    private int selectedItem;
    public InventoryController(InventoryView view, int size, int max_weight)
    {
        this.inventoryModel = new InventoryModel(this, size, max_weight);
        this.inventoryView = view;

        inventoryView.SetBalanceText(inventoryModel.CurrentBalance);
        inventoryView.SetWeightText(inventoryModel.CurrentWeight);

        InitializeNullItemsInInventoryItems();
        InitializeNullItemsInShopItems();
    }

    private void InitializeNullItemsInInventoryItems()
    {
        int i;
        for (i = 0; i < inventoryModel.InventorySize; i++)
            inventoryModel.InventoryItems.Add(null);
    }

    private void InitializeNullItemsInShopItems()
    {
        int i;
        for (i = 0; i < inventoryModel.InventorySize; i++)
            inventoryModel.ShopItems.Add(null);
    }

    public void AddResourcesInInventory(InventoryItem[] allItems)
    {
        if(inventoryModel.CurrentWeight < 50)
        {
            int x = Random.Range(1, 5);
            int i = 0;
            while (inventoryModel.InventoryItems[i] != null)
            {
                i++;
                if (i >= inventoryModel.InventorySize) return;
            }
            int j;
            for(j = i; j<i+x;j++)
            {
                if (j >= inventoryModel.InventorySize) break;
                InventoryItem itemToAdd = allItems[Random.Range(0, allItems.Count())];
                InventoryItem newItem = inventoryModel.InventoryItems[j];
                if (newItem != null)
                {
                    x++;
                    continue;
                }
                if (itemToAdd.ItemWeight + inventoryModel.CurrentWeight <= inventoryModel.MaximumWeight)
                {
                    inventoryModel.InventoryItems[j] = itemToAdd;
                    inventoryModel.CurrentWeight += inventoryModel.InventoryItems[j].ItemWeight;
                    inventoryView.SetWeightText(inventoryModel.CurrentWeight);
                    inventoryView.SetIcon(inventoryModel.InventoryItems[j], j);
                }
            }
        }
    }

    //public void AddItemInInventory(InventoryItem item)
    //{
    //    if (CanAfford(item))
    //    {
    //        inventoryModel.CurrentBalance -= item.ItemBuyPrice;
    //        inventoryModel.CurrentWeight += item.ItemWeight;
    //    }
    //}

    private bool CanAfford(InventoryItem item)
    {
        return inventoryModel.CurrentBalance >= item.ItemBuyPrice * item.ItemQuantity && inventoryModel.CurrentWeight + item.ItemWeight < inventoryModel.MaximumWeight;
    }

    public void InventoryPopUpIcon(int index, GameObject IconPopUp, TextMeshProUGUI iconNameText, TextMeshProUGUI iconQuantityText, TextMeshProUGUI iconDescriptionText, TextMeshProUGUI iconPriceText, TextMeshProUGUI iconWeightText, TextMeshProUGUI iconRarityText, TextMeshProUGUI iconTypeText)
    {
        InventoryItem item = inventoryModel.InventoryItems[index];
        if (item == null) return;

        selectedItem = index;

        iconNameText.text = item.ItemName;
        iconQuantityText.text = $"x {item.ItemQuantity.ToString()}";
        iconDescriptionText.text = item.ItemDescription;
        iconPriceText.text = $"Price: {(item.ItemSellPrice * item.ItemQuantity).ToString()}";
        iconWeightText.text = $"Weight: {item.ItemWeight.ToString()}";
        iconRarityText.text = $"Rarity: {item.ItemRarity.ToString()}";
        iconTypeText.text = $"Type: {item.ItemType.ToString()}";

        IconPopUp.SetActive(true);
    }

    public void SellItem(GameObject inventoryIconTransforms, Sprite emptySprite, GameObject actionPopUp)
    {
        Transform selectedIcon = inventoryIconTransforms.transform.GetChild(selectedItem);

        selectedIcon.GetChild(0).gameObject.GetComponent<Image>().sprite = emptySprite;
        selectedIcon.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "";

        inventoryModel.CurrentBalance += inventoryModel.InventoryItems[selectedItem].ItemSellPrice * inventoryModel.InventoryItems[selectedItem].ItemQuantity;
        inventoryView.SetBalanceText(inventoryModel.CurrentBalance);

        inventoryModel.CurrentWeight -= inventoryModel.InventoryItems[selectedItem].ItemWeight;
        inventoryView.SetWeightText(inventoryModel.CurrentWeight);

        ActionPopUp(0, actionPopUp);

        inventoryModel.InventoryItems[selectedItem] = null;

        inventoryView.StartActionPopUpCoroutine();
    }

    private void ActionPopUp(int i,GameObject actionPopUp)
    {
        if(i==0)
        {
            actionPopUp.transform.GetChild(0).gameObject.SetActive(true);
            actionPopUp.transform.GetChild(1).gameObject.SetActive(false);
            
            TextMeshProUGUI soldText = actionPopUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            soldText.text = $"Item Sold!!! You Gained {inventoryModel.InventoryItems[selectedItem].ItemSellPrice * inventoryModel.InventoryItems[selectedItem].ItemQuantity} Gold";
        }
    }

    public void RefreshShopItems(GameObject shopIconTransforms, InventoryItem[] allItems)
    {
        int i;
        for(i = 0; i<inventoryModel.InventorySize; i++)
        {
            int random = Random.Range(0,allItems.Count());

            GameObject icon = shopIconTransforms.transform.GetChild(i).gameObject;
            icon.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = allItems[random].ItemIcon;
            icon.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = allItems[random].ItemQuantity.ToString();
        }
    }
}
