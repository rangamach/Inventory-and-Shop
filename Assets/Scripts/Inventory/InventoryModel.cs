using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel
{
    private InventoryController inventoryController;
    public List<InventoryItem> InventoryItems = new List<InventoryItem>();

    public int InventorySize;
    public int CurrentWeight;
    public int MaximumWeight;
    public int CurrentBalance;


    public InventoryModel(InventoryController controller, int size, int max_weight)
    {
        this.inventoryController = controller;
        this.InventorySize = size;
        this.MaximumWeight = max_weight;
        this.CurrentWeight = 0;
        this.CurrentBalance = 0;
    }
}
