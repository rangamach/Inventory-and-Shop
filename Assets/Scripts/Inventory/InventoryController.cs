using System.Linq;
using UnityEngine;

public class InventoryController
{
    private InventoryModel inventoryModel;
    private InventoryView inventoryView;
    public InventoryController(InventoryView view, int size, int max_weight)
    {
        this.inventoryModel = new InventoryModel(this, size, max_weight);
        this.inventoryView = view;

        inventoryView.SetBalanceText(inventoryModel.CurrentBalance);
        inventoryView.SetWeightText(inventoryModel.CurrentWeight);

        InitializeItemsInList();
    }

    public void InitializeItemsInList()
    {
        int i;
        for (i = 0; i < inventoryModel.InventorySize; i++)
            inventoryModel.InventoryItems.Add(null);
    }

    //public void InitializeItemsInList()
    //{
    //    int i = 0;
    //    for(i=0; i<inventoryModel.InventorySize; i++)
    //    { 
    //        inventoryModel.InventoryItems[i] = ScriptableObject.CreateInstance<InventoryItem>();

    //        inventoryModel.InventoryItems[i].ItemName = inventoryModel.emptyItemPrefab.ItemName;
    //        inventoryModel.InventoryItems[i].ItemType = inventoryModel.emptyItemPrefab.ItemType;
    //        inventoryModel.InventoryItems[i].ItemDescription = inventoryModel.emptyItemPrefab.ItemDescription;
    //        inventoryModel.InventoryItems[i].ItemBuyPrice = inventoryModel.emptyItemPrefab.ItemBuyPrice;
    //        inventoryModel.InventoryItems[i].ItemSellPrice = inventoryModel.emptyItemPrefab.ItemSellPrice;
    //        inventoryModel.InventoryItems[i].ItemWeight = inventoryModel.emptyItemPrefab.ItemWeight;
    //        inventoryModel.InventoryItems[i].ItemRarity = inventoryModel.emptyItemPrefab.ItemRarity;
    //        inventoryModel.InventoryItems[i].ItemQuantity = inventoryModel.emptyItemPrefab.ItemQuantity;
    //        inventoryModel.InventoryItems[i].ItemIcon = inventoryModel.emptyItemPrefab.ItemIcon;
    //    }
    //}

    public void AddResourcesInInventory(InventoryItem[] allItems)
    {
        if(inventoryModel.CurrentWeight < 10)
        {
            int x = Random.Range(1, 5);
            int i = 0;
            while (inventoryModel.InventoryItems[i] != null)
                i++;
            int j;
            for(j = i; j<i+x;j++)
            {
                inventoryModel.InventoryItems[j] = allItems[Random.Range(0,allItems.Count() - 1)];
                inventoryModel.CurrentWeight += inventoryModel.InventoryItems[j].ItemWeight;
                inventoryView.SetWeightText(inventoryModel.CurrentWeight);
                inventoryView.SetIcon(inventoryModel.InventoryItems[j], j);
            }
        }
    }

    public void AddItemInInventory(InventoryItem item)
    {
        if (CanAfford(item))
        {
            inventoryModel.CurrentBalance -= item.ItemBuyPrice;
            inventoryModel.CurrentWeight += item.ItemWeight;
        }
    }

    private bool CanAfford(InventoryItem item)
    {
        return inventoryModel.CurrentBalance >= item.ItemBuyPrice * item.ItemQuantity;
    }
}
