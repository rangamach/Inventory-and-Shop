using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController
{
    private InventoryModel inventoryModel;
    private InventoryView inventoryView;

    private int selectedItem;

    public InventoryController(InventoryView view, int size, int max_weight, InventoryItem[] allItems)
    {
        this.inventoryModel = new InventoryModel(this, size, max_weight);
        this.inventoryView = view;

        inventoryView.SetBalanceText(inventoryModel.CurrentBalance);
        inventoryView.SetWeightText(inventoryModel.CurrentWeight);

        InitializeNullItemsInInventoryItems();
        InitializeNullItemsInShopItems();
        InitializeRarityItemsLists(allItems);
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

    private void InitializeRarityItemsLists(InventoryItem[] allItems)
    {
        foreach ( InventoryItem item in allItems)
        {
            switch(item.ItemRarity)
            {
                case Rarity.Very_Common:
                    inventoryModel.VeryCommonItems.Add(item);
                    break;
                case Rarity.Common:
                    inventoryModel.CommonItems.Add(item);
                    break;
                case Rarity.Rare:
                    inventoryModel.RareItems.Add(item);
                    break;
                case Rarity.Epic:
                    inventoryModel.EpicItems.Add(item);
                    break;
                case Rarity.Legendary:
                    inventoryModel.LegendaryItems.Add(item);
                    break;
            }
        }
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
                List<InventoryItem> itemToAddList = RarityChooser();
                int z  = Random.Range(0,itemToAddList.Count());
                InventoryItem itemToAdd = itemToAddList[z];
                
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

    private List<InventoryItem> RarityChooser()
    {
        int i = Random.Range(1, 100);
        if (0 < i && i <= 45)
            return inventoryModel.VeryCommonItems;
        else if (46 <= i && i <= 70)
            return inventoryModel.CommonItems;
        else if (71 <= i && i <= 85)
            return inventoryModel.RareItems;
        else if (86 <= i && i <= 99)
            return inventoryModel.LegendaryItems;
        else
            return null;
    }

    private bool CanAfford(InventoryItem item)
    {
        return inventoryModel.CurrentBalance >= item.ItemBuyPrice && inventoryModel.CurrentWeight + item.ItemWeight <= inventoryModel.MaximumWeight;
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

        RemoveItemInScene(inventoryIconTransforms, emptySprite);

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

        else if(i==1)
        {
            actionPopUp.transform.GetChild(0).gameObject.SetActive(false);
            actionPopUp.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void RefreshShopItems(GameObject shopIconTransforms)
    {
        int i;
        for(i = 0; i<inventoryModel.InventorySize; i++)
        {
            //int random = Random.Range(0,allItems.Count());
            List<InventoryItem> itemToAddList = RarityChooser();
            int random = Random.Range(0, itemToAddList.Count());

            GameObject icon = shopIconTransforms.transform.GetChild(i).gameObject;
            icon.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = itemToAddList[random].ItemIcon;
            icon.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = itemToAddList[random].ItemQuantity.ToString();

            inventoryModel.ShopItems[i] = itemToAddList[random];
        }
    }

    public void ShopPopUpIcon(int i, GameObject actionPopUp)
    {
        TextMeshProUGUI buyText = actionPopUp.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        buyText.text = $"Do you want to buy\n{inventoryModel.ShopItems[i].ItemName}\nfor {inventoryModel.ShopItems[i].ItemBuyPrice.ToString()} gold?";

        ActionPopUp(1, actionPopUp);

        selectedItem = i;

        actionPopUp.SetActive(true);
    }
    public void BuyItem(GameObject inventoryItemTransforms, GameObject shopItemTransforms, Sprite emptySprite)
    {
        InventoryItem buyingItem = inventoryModel.ShopItems[selectedItem];

        if (!CanAfford(buyingItem)) return;

        RemoveItemInScene(shopItemTransforms, emptySprite);

        inventoryModel.ShopItems[selectedItem] = null;

        AddItemToInventory(buyingItem,inventoryItemTransforms);

    }

    private bool IsInventoryFull(out int index)
    {
        index = 0;
        while (index <= inventoryModel.InventorySize)
        {
            if (inventoryModel.InventoryItems[index] == null)
            {
                return false;
            }
            index++;
        }
        return true;
    }
    private void AddItemToInventory(InventoryItem itemToAdd, GameObject inventoryItemTransforms)
    {
        int index;
        if(!IsInventoryFull(out index))
        {
            GameObject emptyIcon = inventoryItemTransforms.transform.GetChild(index).gameObject;
            emptyIcon.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = itemToAdd.ItemIcon;
            emptyIcon.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = itemToAdd.ItemQuantity.ToString();

            inventoryModel.CurrentBalance -= itemToAdd.ItemBuyPrice;
            inventoryModel.CurrentWeight += itemToAdd.ItemWeight;

            inventoryView.SetBalanceText(inventoryModel.CurrentBalance);
            inventoryView.SetWeightText(inventoryModel.CurrentWeight);

            inventoryModel.InventoryItems[index] = itemToAdd;
        }
    }
    private void RemoveItemInScene(GameObject iconTransforms, Sprite emptySprite)
    {
        GameObject icon = iconTransforms.transform.GetChild(selectedItem).gameObject;
        icon.transform.GetChild(0).GetComponent<Image>().sprite = emptySprite;
        icon.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
    }
}
